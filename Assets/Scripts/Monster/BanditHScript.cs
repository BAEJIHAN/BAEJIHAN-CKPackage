using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BANDITHSTATE
{
    IDLE,
    CHASE,
    COMBAT,
    ATTACK,
    HURT,
    DOWN,
    DEATH
}

public class BanditHScript : MonRootScript
{
    BANDITHSTATE State = BANDITHSTATE.IDLE;
    Vector3 DestPos;
    float Speed = 4;
    float CurDownTime = 0;
    float DownTime = 4;

    bool DownTrigger=true;

    int FAttackCount = 3;

    [HideInInspector] public float CombatIdleTime;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        PlayerDetectDist = 5;
        Damage = 3;
        MaxHP = 15;
        HP = 15;
    }

    // Update is called once per frame
    void Update()
    {      

        DetectPlayer();

        Chase();

        CombatIdleUpdate();

        DownUpdate();
        //HurtUpdate();

        DeathBip();

        StuckUpdate();
    }

    void DetectPlayer()
    {
        if (State != BANDITHSTATE.IDLE)
            return;

        if (PlayerPivot == null)
            return;

        float DetectDist = (PlayerPivot.transform.position - Pivot.transform.position).magnitude;
        if (DetectDist < PlayerDetectDist)
        {
            State = BANDITHSTATE.CHASE;//플레이어 추격 시작.
            Ani.SetTrigger("Run");

        }
    }

    void Chase()
    {
        if (State != BANDITHSTATE.CHASE)
            return;

        DestPos = PlayerPivot.transform.position;//목표지점 플레이어 위치.
        DestPos -= new Vector3(0, 0.5f, 0);
        if (DestPos.x < Pivot.transform.position.x)//방향전환
        {
            Pivot.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            Pivot.transform.eulerAngles = new Vector3(0, 180, 0);
        }

        Vector3 Dir = DestPos - Pivot.transform.position;
        Dir.Normalize();
        Pivot.transform.position += (Dir * Time.deltaTime * Speed);

        if ((DestPos - Pivot.transform.position).magnitude < 0.8f)//플레이어 가까이에 가면 공격 시작.
        {
            State = BANDITHSTATE.COMBAT;
            Ani.SetTrigger("CombatIdle");
            CombatIdleTime = Random.Range(0.5f, 1.0f);
        }
    }

    void CombatIdleUpdate()
    {
        if (State != BANDITHSTATE.COMBAT)
        {
            return;
        }

        CombatIdleTime -= Time.deltaTime;
        if (CombatIdleTime < 0)
        {
            if ((PlayerPivot.transform.position - Pivot.transform.position).magnitude < 1.2f)
            {
                Ani.SetTrigger("Attack");
                State = BANDITHSTATE.ATTACK;
            }
            else
            {
                Ani.SetTrigger("Run");
                State = BANDITHSTATE.CHASE;
            }

        }
    }

    void DownUpdate()
    {
        if(BANDITHSTATE.DOWN!=State)
        {
            return;
        }


        if (!DownTrigger)
            return;

        CurDownTime += Time.deltaTime;
        
        if(CurDownTime >DownTime) 
        {
            CurDownTime = 0;
            Ani.SetTrigger("Recover");
            DownTrigger = false;
        }
    }

    void StuckUpdate()
    {
        //끼었을 때 돌아가는 함수
        if (!IsStuck)
            return;

        if (State != BANDITHSTATE.CHASE && State != BANDITHSTATE.COMBAT)
            return;


        Vector3 Dir = PlayerPivot.transform.position - Pivot.transform.position;
        Dir.z = 0;
        Dir.Normalize();

        Vector3 MoveAxis = Vector3.zero;
        float DotAngle = 2;
        if (Mathf.Abs(Vector3.Dot(Vector3.up, Dir)) < DotAngle)
        {
            DotAngle = Mathf.Abs(Vector3.Dot(Vector3.up, Dir));
            MoveAxis = Vector3.up;
        }

        if (Mathf.Abs(Vector3.Dot(Vector3.down, Dir)) < DotAngle)
        {
            DotAngle = Mathf.Abs(Vector3.Dot(Vector3.up, Dir));
            MoveAxis = Vector3.down;
        }

        if (Mathf.Abs(Vector3.Dot(Vector3.right, Dir)) < DotAngle)
        {
            DotAngle = Mathf.Abs(Vector3.Dot(Vector3.up, Dir));
            MoveAxis = Vector3.right;
        }

        if (Mathf.Abs(Vector3.Dot(Vector3.down, Dir)) < DotAngle)
        {
            DotAngle = Mathf.Abs(Vector3.Dot(Vector3.down, Dir));
            MoveAxis = Vector3.down;
        }

        Pivot.transform.position += MoveAxis * Time.deltaTime * Speed;


    }

    public override void AwakeMon()
    {
       
        State = BANDITHSTATE.CHASE;
        Ani.SetTrigger("Run");

    }
    
    public void SetColliderOn()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
    

    public void SetBanditHState(BANDITHSTATE _State)
    {
        State = _State;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (BANDITHSTATE.DOWN == State || BANDITHSTATE.DEATH==State)
            return;

       

        if (collision.gameObject.tag == "PlayerAttack")
        {

            TakeDamage(1);


            if (HP <= 0)
            {
                Ani.SetTrigger("Death");
                State = BANDITHSTATE.DEATH;
                GetComponent<BoxCollider2D>().enabled = false;
                IsFadeOn = true;
                AClip = Resources.Load<AudioClip>("Sound/Monster/BanditHDeath");
                ASource.PlayOneShot(AClip);

                if (StageManager)
                    StageManager.SubMonCount();
            }
           


        }
        else if (collision.gameObject.tag == "FPlayerAttack")
        {


            TakeDamage(2);

            FAttackCount--;

            if (HP <= 0)
            {
                Ani.SetTrigger("Death");
                State = BANDITHSTATE.DEATH;
                GetComponent<BoxCollider2D>().enabled = false;
                IsFadeOn = true;
                AClip = Resources.Load<AudioClip>("Sound/Monster/BanditHDeath");
                ASource.PlayOneShot(AClip);

                if (StageManager)
                    StageManager.SubMonCount();
                return;
            }

            if(FAttackCount<=0)
            {
                Ani.SetTrigger("Down");
                State = BANDITHSTATE.DOWN;
                GetComponent<BoxCollider2D>().enabled = false;
                FAttackCount = 3;
            }
            else
            {
                Ani.SetTrigger("Hurt");
            }
            
            

            
           
        }

        

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            ObstacleCollisionTime += Time.deltaTime;
            if (ObstacleCollisionTime > MaxObstacleCollisionTime)
            {
                IsStuck = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            ObstacleCollisionTime = 0;
            IsStuck = false;
        }

    }
}
