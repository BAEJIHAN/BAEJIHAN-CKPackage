using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BANDITBOSSSTATE
{
    IDLE,
    CHASE,
    COMBAT,
    ATTACK1,
    ATTACK2,
    ATTACK3,
    ATTACK4,
    COMBO1,
    COMBO2,
    BACKSTEP,
    HIT,
    DEATH
}
public class BanditBossScript : MonRootScript
{
    
    BANDITBOSSSTATE State = BANDITBOSSSTATE.IDLE;
    Vector3 DestPos;
    float ChaseSpeed = 4;
    float CombatSpeed = 2.5f;
    float HurtTime = 3;
    float Combo1Speed = 8;
    float Combo2Speed = 14;

    int Combo1Level = 0;
    int Combo2Level = 0;
    int Attack3Level = 0;

    int PreAttackNum = 0;
    float PreAttackMoveDist;

    

    //백스텝
    float BackStepSpeed = 10;
    float BackStepXStart;
    float BackStepXEnd;
    float BackStepYStart;
    float BackStepXDist=5;
    float BackStepHeightDelta=0.1f;

    float HitTime = 3;
    float CurHitTime = 0;
    [HideInInspector] int HitStack = 0;

    //이벤트
    bool IsOnEvent = true;
    // Start is called before the first frame update
    void Start()
    {
        PortraitTexture = Resources.Load<Texture>("Texture/Boss1Portrait");
        
        base.Start();
        State = BANDITBOSSSTATE.CHASE;
        Ani.SetTrigger("Idle");

        MaxHP = 50;
        HP = 50;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOnEvent)
            return;

        ChaseUpdate();

        CombatUpdate();

        Attack3Update();

        Combo1Update();

        Combo2Update();

        BackStepUpdate();

        HitUpdate();
    }

    void ChaseUpdate()
    {
        if (State != BANDITBOSSSTATE.CHASE)
            return;

        DestPos = PlayerPivot.transform.position;//목표지점 플레이어 위치.


        if (DestPos.x < Pivot.transform.position.x)//방향전환
        {
            Pivot.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            Pivot.transform.eulerAngles = new Vector3(0, 0, 0);
        }


        Vector3 Dir = DestPos - Pivot.transform.position;
        Dir.Normalize();
        Pivot.transform.position += (Dir * Time.deltaTime * ChaseSpeed);

        if ((DestPos - Pivot.transform.position).magnitude < 2)//플레이어 가까이에 가면 공격 시작.
        {
            State = BANDITBOSSSTATE.COMBAT;

            SetPreAttackNum();

            SetDest();
            
            Ani.speed = 0.625f;

        }
    }

    void CombatUpdate()
    {
        if (State != BANDITBOSSSTATE.COMBAT)
            return;

        Vector3 Dir = DestPos - Pivot.transform.position;
        Dir.Normalize();
        Pivot.transform.position += (Dir * Time.deltaTime * CombatSpeed);
        PreAttackMoveDist -= Time.deltaTime * CombatSpeed;
        if (PlayerPivot.transform.position.x < Pivot.transform.position.x)//방향전환
        {
            Pivot.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            Pivot.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if(PreAttackMoveDist<0)
        {
            PreAttackNum--;
            SetDest();
        }

        if(PreAttackNum<=0)
        {
            Ani.speed = 1;
            SelectAttack();
        }
    }

    void Attack3Update()
    {
        if (BANDITBOSSSTATE.ATTACK3 != State)
            return;

        if(Attack3Level==1)
        {
            Vector3 tempV = Pivot.transform.position;
            tempV += Pivot.transform.right * Time.deltaTime * Combo1Speed;
            Pivot.transform.position = tempV;
        }
        
    }

    void Combo1Update()
    {
        if (BANDITBOSSSTATE.COMBO1 != State)
            return;
        
        if(Combo1Level == 1)
        {
            Vector3 tempV = Pivot.transform.position;
            tempV += Pivot.transform.right * Time.deltaTime * Combo1Speed;
            Pivot.transform.position = tempV;
        }

        if (Combo1Level == 3)
        {
            Vector3 tempV = Pivot.transform.position;
            tempV += Pivot.transform.right * Time.deltaTime * Combo1Speed*0.5f;
            Pivot.transform.position = tempV;
        }

        if (Combo1Level == 5)
        {
            Vector3 tempV = Pivot.transform.position;
            tempV += Pivot.transform.right * Time.deltaTime * Combo1Speed;
            Pivot.transform.position = tempV;
        }
    }

    void Combo2Update()
    {
        if (BANDITBOSSSTATE.COMBO2 != State)
            return;

        if(Combo2Level == 1)
        {
            Vector3 tempV = Pivot.transform.position;
            tempV += Pivot.transform.right * Time.deltaTime * Combo2Speed;
            Pivot.transform.position = tempV;
        }
    }

    void BackStepUpdate()
    {
        if (State != BANDITBOSSSTATE.BACKSTEP)
            return;
        Vector3 tempv = Pivot.transform.position;
        float tempx = tempv.x;
        float tempy = tempv.y;
        Vector3 tempDir = -Pivot.transform.right;
        tempDir.Normalize();
        tempv.x += Time.deltaTime * BackStepSpeed * tempDir.x;
        tempv.y = BackStepYStart - BackStepHeightDelta * (tempv.x - BackStepXStart) * (tempv.x - BackStepXEnd);
        Pivot.transform.position = tempv;
    }

    public void BackStepStartFunc()
    {
        State = BANDITBOSSSTATE.BACKSTEP;
        Ani.SetTrigger("BackStep");

        BackStepXStart = Pivot.transform.position.x;
        if (Pivot.transform.right.x < 0)
        {
            BackStepXEnd = Pivot.transform.position.x + BackStepXDist;
        }
        else
        {
            BackStepXEnd = Pivot.transform.position.x - BackStepXDist;
        }

        BackStepYStart = Pivot.transform.position.y;
    }

    void HitUpdate()
    {
        if(State != BANDITBOSSSTATE.HIT)
            return;

        CurHitTime += Time.deltaTime;
        if(CurHitTime>HitTime)
        {
            HitStack = 0;
            CurHitTime = 0;
            SelectAttack();
        }
    }

    public void SetPreAttackNum()//공격 하기 전에 배회할 횟수 설정
    {
        PreAttackNum = Random.Range(2, 5);
    }

    public void SetDest()//다음 목표 지점 설정
    {
        Vector3 tempV= PlayerPivot.transform.position;
       
        tempV.x += Random.Range(-3.0f, 3.0f);
        tempV.y += Random.Range(-1.0f, 1.0f);

        DestPos = tempV;
        PreAttackMoveDist = (DestPos - Pivot.transform.position).magnitude;
    }

    void SelectAttack()
    {
        int Sel = Random.Range(0, 6);//0 Attack1, 1 Attack2, 2 Attack3, 3 Attack4, 4 Combo1, 5 Combo2
        

        if (Sel == 0)
        {
            Ani.SetTrigger("Attack1");
            State = BANDITBOSSSTATE.ATTACK1;

        }
        else if (Sel == 1)
        {
            Ani.SetTrigger("Attack2");
            State = BANDITBOSSSTATE.ATTACK2;
        }
        else if (Sel == 2)
        {
            Ani.SetTrigger("Attack3");
            State = BANDITBOSSSTATE.ATTACK3;
        }
        else if (Sel == 3)
        {
            Ani.SetTrigger("Attack4");
            State = BANDITBOSSSTATE.ATTACK4;
        }
        else if (Sel == 4)
        {
            Ani.SetTrigger("Combo1");
            State = BANDITBOSSSTATE.COMBO1;
        }
        else if (Sel == 5)
        {
            Ani.SetTrigger("Combo2");
            State = BANDITBOSSSTATE.COMBO2;
        }
    }

    public void Combo1LevelAdd()
    {
        Combo1Level++;
    }

    public void Combo1LevelZero()
    {
        Combo1Level = 0;
    }

    public void Combo2LevelAdd()
    {
        Combo2Level++;
    }

    public void Combo2LevelZero()
    {
        Combo2Level = 0;
    }

    public void Attack3LevelAdd()
    {
        Attack3Level++;
    }

    public void Attack3LevelZero()
    {
        Attack3Level = 0;
    }
    public void SetBanditBossState(BANDITBOSSSTATE _state)
    {
        State = _state;
    }


    public void SetEventOff()
    {
        IsOnEvent = false;
        if(!StageManager.IsStageCleared)
        {
            State = BANDITBOSSSTATE.CHASE;
            Ani.SetTrigger("Run");
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (BANDITBOSSSTATE.DEATH == State)
            return;
        if (collision.tag=="PlayerAttack" || collision.tag=="FPlayerAttack")
        {
            HitStack++;
            if(collision.tag == "PlayerAttack")
            {
                if(BANDITBOSSSTATE.HIT==State)
                {
                    TakeDamage(2);
                }
                else
                {
                    TakeDamage(1);
                }
                
            }
            else
            {
                if (BANDITBOSSSTATE.HIT == State)
                {
                    TakeDamage(4);
                }
                else
                {
                    TakeDamage(2);
                }
            }
            UIMgrScript.Inst.SetBossHPBar(MaxHP, HP);
            if (HP<=0)
            {
                State = BANDITBOSSSTATE.DEATH;
                Ani.SetTrigger("Death");
                GetComponent<BoxCollider2D>().enabled = false;
                AClip = Resources.Load<AudioClip>("Sound/Monster/BanditBossDeath");
                ASource.PlayOneShot(AClip);
                StageManager.ClearStage();
            }
            else if (BANDITBOSSSTATE.HIT != State && HitStack>7)
            {
                State = BANDITBOSSSTATE.HIT;
                Ani.SetTrigger("Hit");
                CurHitTime = 0;
            }                 
             
        }
    }
}
