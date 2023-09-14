using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FSKULLSTATE
{
    IDLE,
    CHASE,    
    ATTACK,
    BOMB,
    HIT,
    DEATH
    
}
public class FSkullScript : MonRootScript
{
    public GameObject MonsterAttackObj;
    FSKULLSTATE State = FSKULLSTATE.IDLE;
    float Speed = 3;

    //Idle관련
    float IdleSpeed = 1;
    float IdleDist = 1;
    float CurIdleDist=0;
    Vector3 IdleDir;

    //Attack 관련
    float AttackTime=0.005f;
    float CurAttackTime = 0;
    Vector3 CurAttackPos;
    Vector3[] AttackShakePos = new Vector3[9];
    float BombTime = 0.8f;
    float CurBombTime = 0;
    bool AttackFlag = false;

    //사운드 관련
    public GameObject PreBombSound;
    AudioSource PreBombSoundSource;
    // Start is called before the first frame update
    void Start()
    {
        PreBombSoundSource=PreBombSound.GetComponent<AudioSource>();
        PreBombSoundSource.volume = GValue.GSoundVolume;
        base.Start();
        PlayerDetectDist = 5;
        if (Random.Range(0, 2)==0)
        {
            IdleDir = Vector3.up;
        }
        else
        {
            IdleDir = Vector3.down;
        }
        HP = 4;
    }

    // Update is called once per frame
    void Update()
    {
        IdleUpdate();

        DetectPlayer();

        Chase();

        Attack();

        if (FSKULLSTATE.ATTACK != State)
            PreBombSoundSource.Stop();
    }

 

    void IdleUpdate()
    {
        if (FSKULLSTATE.IDLE != State)
            return;
                
        Pivot.transform.position += Time.deltaTime * IdleDir * IdleSpeed;
        CurIdleDist += Time.deltaTime * IdleSpeed;

        if(CurIdleDist>IdleDist)
        {
            CurIdleDist = 0;
            IdleDir = -IdleDir;
        }
    }

    private void DetectPlayer()
    {
        if (FSKULLSTATE.IDLE != State)
            return;

        if ((PlayerPivot.transform.position - Pivot.transform.position).magnitude < PlayerDetectDist)
        {
            State = FSKULLSTATE.CHASE;
            Ani.SetTrigger("Fly");
            if (MonGroup)
                MonGroup.AwakeMons();
        }
    }

    private void Chase()
    {
        if (FSKULLSTATE.CHASE != State)
            return;
        PreBombSoundSource.Play();

        if (PlayerPivot.transform.position.x < Pivot.transform.position.x)//방향전환
        {
            Pivot.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            Pivot.transform.eulerAngles = new Vector3(0, 180, 0);
        }

        Vector3 Dir = PlayerPivot.transform.position - Pivot.transform.position;
        Dir.Normalize();
        Pivot.transform.position += (Dir * Time.deltaTime * Speed);

        if ((PlayerPivot.transform.position - Pivot.transform.position).magnitude < 0.3f)//플레이어 가까이에 가면 공격 시작.
        {
            State = FSKULLSTATE.ATTACK;
            Ani.SetTrigger("Idle");
            SetAttackPos();
            CurAttackTime = 0;
            CurBombTime = 0;
        }
        
    }

    private void Attack()
    {
        if (FSKULLSTATE.ATTACK != State)
            return;

        CurAttackTime += Time.deltaTime;
        if(CurAttackTime>AttackTime)
        {
            CurAttackTime = 0;
            Pivot.transform.position = CurAttackPos;
            AttackFlag = !AttackFlag;
            if(AttackFlag)
            {
                CurAttackPos = AttackShakePos[Random.Range(0, 8)];
            }
            else
            {
                CurAttackPos = AttackShakePos[8];
            }
            
        }

        CurBombTime += Time.deltaTime;
        if(CurBombTime>BombTime)
        {
            Ani.SetTrigger("Bomb");
            State = FSKULLSTATE.BOMB;
            Pivot.GetComponent<BoxCollider2D>().enabled = false;
            if (StageManager)
                StageManager.SubMonCount();
        }
        ////터지기 전 떨림.

    }

    override public void AwakeMon()
    {
        State = FSKULLSTATE.CHASE;
        Ani.SetTrigger("Fly");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (FSKULLSTATE.DEATH == State)
            return;


        if (collision.tag == "PlayerAttack" || collision.tag == "FPlayerAttack")
        {
            if (collision.tag == "PlayerAttack")
                HP--;
            if (collision.tag == "FPlayerAttack")
                HP -= 2;

            if(HP<=0)
            {
                Ani.SetTrigger("Death");
                State = FSKULLSTATE.DEATH;
                AClip = Resources.Load<AudioClip>("Sound/Monster/FSkullDeath");
                ASource.PlayOneShot(AClip);
                if (StageManager)
                    StageManager.SubMonCount();
            }
            else
            {
                Ani.SetTrigger("Hit");
                State = FSKULLSTATE.HIT;
            }
            

           
        }

        
    }

    void SetAttackPos()
    {
        Vector3 temp = Pivot.transform.position;

        float tempOffset = 0.1f;
        float edgeOffset=Mathf.Sqrt(tempOffset*tempOffset*2);
        for(int i=0; i<9; i++)
        {
            AttackShakePos[i] = temp;
        }
        // 0 1 2
        // 3 8 4 
        // 5 6 7
        AttackShakePos[0].x -= edgeOffset;
        AttackShakePos[0].y += edgeOffset;

        AttackShakePos[1].y += tempOffset;

        AttackShakePos[2].x += edgeOffset;
        AttackShakePos[2].y += edgeOffset;

        AttackShakePos[3].x -= tempOffset;

        AttackShakePos[4].x += tempOffset;

        AttackShakePos[5].x -= edgeOffset;
        AttackShakePos[5].y -= edgeOffset;

        AttackShakePos[6].y -= tempOffset;

        AttackShakePos[7].x += edgeOffset;
        AttackShakePos[7].y -= edgeOffset;

        CurAttackPos = temp;

    }

    public void SetFSkullState(FSKULLSTATE _state)
    {
        State = _state;
    }
}
