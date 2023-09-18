using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BANDITLSTATE
{
    IDLE,
    CHASE,
    COMBAT,
    ATTACK,
    HURT,
    DEATH
}
public class BanditLScript : MonRootScript
{
  

    BANDITLSTATE State = BANDITLSTATE.IDLE;
    Vector3 DestPos;
    float Speed = 4;
    float HurtTime = 3;
    float FAttackTime = 0.2f;
    float FAttackSpeed = 8;

    [HideInInspector] public float AttackRange = 1.2f;

    bool IsHurtReady = false;
    bool IsFAttackOn = false;

 
    [HideInInspector] public float CombatIdleTime;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        PlayerDetectDist = 5;        
        MaxHP = 10;
        HP = 10;
        CombatIdleTime = Random.Range(0.8f, 2.0f);

    }

    // Update is called once per frame
    void Update()
    {
        
        
        DetectPlayer();

        Chase();

        CombatIdleUpdate();

        //HurtUpdate();

        FAttackUpdate();

        DeathBip();

        StuckUpdate();
    }

    void DetectPlayer()
    {
        if (State != BANDITLSTATE.IDLE)
            return;

        if (PlayerPivot == null)
            return;

        float DetectDist = (PlayerPivot.transform.position - Pivot.transform.position).magnitude;
        if (DetectDist < PlayerDetectDist)
        {
            if(MonGroup)
                MonGroup.AwakeMons();
            State = BANDITLSTATE.CHASE;//플레이어 추격 시작.
            Ani.SetTrigger("Run");

            float rnum = Random.Range(-0.4f, 0.4f);
            AttackRange = 0.7f + rnum;
        }
    }

    void Chase()
    {
        if (State != BANDITLSTATE.CHASE)
            return;

        DestPos = PlayerPivot.transform.position;//목표지점 플레이어 위치.
        DestPos -= new Vector3(0, 0.5f, 0);
        if (PlayerPivot.transform.position.x < Pivot.transform.position.x)//방향전환
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

        float rnum=Random.Range(-0.4f, 0.4f);
        if ((DestPos - Pivot.transform.position).magnitude < AttackRange)//플레이어 가까이에 가면 공격 시작.
        {
            State = BANDITLSTATE.COMBAT;
            Ani.SetTrigger("CombatIdle");
            CombatIdleTime = Random.Range(0.8f, 2.0f);
            
            
        }        
    }

    void CombatIdleUpdate()
    {
        if (State != BANDITLSTATE.COMBAT)
        {
            return;
        }

        CombatIdleTime -= Time.deltaTime;
        if(CombatIdleTime<0)
        {
            DestPos = PlayerPivot.transform.position;
            DestPos -= new Vector3(0, 0.5f, 0);

            if ((DestPos - Pivot.transform.position).magnitude < AttackRange)
            {
                Ani.SetTrigger("Attack");
                State = BANDITLSTATE.ATTACK;
                
            }
            else
            {
                Ani.SetTrigger("Run");
                State = BANDITLSTATE.CHASE;

                float rnum = Random.Range(-0.4f, 0.4f);
                AttackRange = 0.7f + rnum;
            }

        }            
    }

    void HurtUpdate()
    {
        if(IsHurtReady)
        {
            return;
        }
        HurtTime-= Time.deltaTime;
        if(HurtTime<0)
        {
            HurtTime = 3;
            IsHurtReady = true;
        }

    }

    void FAttackUpdate()
    {
        if (!IsFAttackOn)
            return;

        if (State == BANDITLSTATE.DEATH)
            return;

        FAttackTime-= Time.deltaTime;
        Vector3 tempV=Pivot.transform.position;

        if(PlayerPivot.transform.position.x<tempV.x)
        {
            tempV.x += FAttackSpeed*Time.deltaTime;
        }
        else
        {
            tempV.x += -FAttackSpeed * Time.deltaTime;
        }

        Pivot.transform.position = tempV;
        if(FAttackTime<0)
        {
            FAttackTime = 0.2f;
            IsFAttackOn = false;
        }

    }

    void StuckUpdate()
    {
        //끼었을 때 돌아가는 함수
        if (!IsStuck)
            return;

        if (State != BANDITLSTATE.CHASE && State != BANDITLSTATE.COMBAT)
            return;
        
        
        Vector3 Dir = PlayerPivot.transform.position - Pivot.transform.position;
        Dir.z = 0;
        Dir.Normalize();

        Vector3 MoveAxis = Vector3.zero;
        float DotAngle = 2;
        if(Mathf.Abs(Vector3.Dot(Vector3.up, Dir))<DotAngle)
        {
            DotAngle = Mathf.Abs(Vector3.Dot(Vector3.up, Dir));
            MoveAxis = Vector3.up;
        }

        if (Mathf.Abs(Vector3.Dot(Vector3.down, Dir)) < DotAngle)
        {
            DotAngle = Mathf.Abs(Vector3.Dot(Vector3.down, Dir));
            MoveAxis = Vector3.down;
        }

        if (Mathf.Abs(Vector3.Dot(Vector3.right, Dir)) < DotAngle)
        {
            DotAngle = Mathf.Abs(Vector3.Dot(Vector3.right, Dir));
            MoveAxis = Vector3.right;
        }

        if (Mathf.Abs(Vector3.Dot(Vector3.left, Dir)) < DotAngle)
        {
            DotAngle = Mathf.Abs(Vector3.Dot(Vector3.left, Dir));
            MoveAxis = Vector3.left;
        }

        Pivot.transform.position += MoveAxis * Time.deltaTime * Speed;      
                   

    }
    public void SetBanditLState(BANDITLSTATE _State)
    {
        State = _State;
    }

    public override void AwakeMon()
    {        
        State = BANDITLSTATE.CHASE;
        Ani.SetTrigger("Run");

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (State == BANDITLSTATE.DEATH)
            return;

        if (collision.gameObject.tag == "PlayerAttack" )
        {

            TakeDamage(1);        
                             
                        
            if (HP <= 0)
            {
                Ani.SetTrigger("Death");
                State = BANDITLSTATE.DEATH;
                GetComponent<BoxCollider2D>().enabled = false;
                AClip = Resources.Load<AudioClip>("Sound/Monster/BanditLDeath");
                ASource.PlayOneShot(AClip);
                IsFadeOn = true;
                if(StageManager)
                    StageManager.SubMonCount();
            }
            else
            {
                Ani.SetTrigger("Hurt");
            }


        }
        else if(collision.gameObject.tag == "FPlayerAttack")
        {
           

            TakeDamage(2);
           

            if (HP <= 0)
            {
                Ani.SetTrigger("Death");
                State = BANDITLSTATE.DEATH;
                GetComponent<BoxCollider2D>().enabled = false;
                AClip = Resources.Load<AudioClip>("Sound/Monster/BanditLDeath");
                ASource.PlayOneShot(AClip);
                IsFadeOn = true;
                if(StageManager)
                    StageManager.SubMonCount();
            }
            else
            {
                Ani.SetTrigger("Hurt");
                IsFAttackOn = true;
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
