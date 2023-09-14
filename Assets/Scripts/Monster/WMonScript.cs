using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WMONSTATE
{
    IDLE,
    COMBAT,
    CHASE,
    RUN,
    ATTACK1,
    ATTACK2,
    HIT,
    BLINK,
    DEATH
}
public class WMonScript : MonRootScript
{
    // Start is called before the first frame update
    WMONSTATE State = WMONSTATE.IDLE;
    
    float Speed = 3;
   


    float Attack2Time = 2;
    float CurAttack2Time = 0;
    int Attack2Num = 0;

   
    // Start is called before the first frame update
    void Start()
    {
        HP = 10;
        base.Start();
        PlayerDetectDist = 8;
    }

    // Update is called once per frame
    void Update()
    {
        
        DetectPlayer();

        CombatUpdate();

        ChaseUpdate();

        Attack2Update();

        base.DeathBip();

        StuckUpdate();
    }

    void DetectPlayer()
    {
       
        if (State != WMONSTATE.IDLE)
            return;

        if (PlayerPivot == null)
            return;

      
        float DetectDist = (PlayerPivot.transform.position - Pivot.transform.position).magnitude;
        if (DetectDist < PlayerDetectDist)
        {
            State = WMONSTATE.CHASE;//플레이어 추격 시작.
            Ani.SetTrigger("Run");
            if (MonGroup)
                MonGroup.AwakeMons();
                      
        }
    }

   

    void ChaseUpdate()//플레이어와 멀어졌을 때 쫓아옴
    {
        if (State != WMONSTATE.CHASE)
            return;

        Vector3 tempDir = PlayerPivot.transform.position - Pivot.transform.position;
        tempDir.Normalize();
    
        Pivot.transform.position += Time.deltaTime * Speed * tempDir;

        if (PlayerPivot.transform.position.x > Pivot.transform.position.x)//방향전환
        {
            Pivot.transform.rotation = Quaternion.Euler(0, 0, 0);

        }
        else
        {
            Pivot.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if ((PlayerPivot.transform.position - Pivot.transform.position).magnitude < 5)//플레이어 범위 안으로 들어오면 다시 공격
        {            
            State = WMONSTATE.COMBAT;
        }

    }

    void CombatUpdate()//전투. 
    {
        if (State != WMONSTATE.COMBAT)
            return;

        if ((PlayerPivot.transform.position - Pivot.transform.position).magnitude > 9)//플레이어가 너무 멀어지면 쫓아감
        {
            State = WMONSTATE.CHASE;//플레이어 추격 시작.
            Ani.SetTrigger("Run");
            return;
        }
               
             
           
        if ((PlayerPivot.transform.position-Pivot.transform.position).magnitude<1.8f)//플레이어와 가까우면 공격1
        {
            
            if (PlayerPivot.transform.position.x > Pivot.transform.position.x)//방향전환
            {
                Pivot.transform.rotation = Quaternion.Euler(0, 0, 0);

            }
            else
            {
                Pivot.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            State = WMONSTATE.ATTACK1;
            Ani.SetTrigger("Attack1");
            

        }
        else//공격2
        {
            if (PlayerPivot.transform.position.x > Pivot.transform.position.x)//방향전환
            {
                Pivot.transform.rotation = Quaternion.Euler(0, 0, 0);

            }
            else
            {
                Pivot.transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            State = WMONSTATE.ATTACK2;
            Ani.SetTrigger("Attack2_a");
        }



    }


    void Attack2Update()
    {
        if (WMONSTATE.ATTACK2 != State)
            return;

        if(Attack2Num==1)
        {
            CurAttack2Time += Time.deltaTime;
            if(CurAttack2Time>Attack2Time)
            {
                CurAttack2Time = 0;
                Attack2Num++;
                Ani.SetTrigger("Attack2_b");
            }
        }   
        
    }

    public override void AwakeMon()
    {
        State = WMONSTATE.CHASE;
        Ani.SetTrigger("Run");
    }
    void StuckUpdate()
    {
        //끼었을 때 돌아가는 함수
        if (!IsStuck)
            return;

        if (State != WMONSTATE.CHASE)
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
    public void SetWMonState(WMONSTATE _State)
    {
        State = _State;
    }

    public void AddAttackNum()
    {
        Attack2Num++;
    }

    public void ZeroAttackNum()
    {
        Attack2Num=0;
    }

    public void TakeNextAction()
    {
        if ((PlayerPivot.transform.position - Pivot.transform.position).magnitude < 5)
        {
            Ani.SetTrigger("Blink");
            State = WMONSTATE.BLINK;
        }
        else
        {
            State = WMONSTATE.CHASE;
            Ani.SetTrigger("Run");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (State == WMONSTATE.DEATH)
            return;

        if (collision.gameObject.tag == "PlayerAttack")
        {
            HP--;
            if(HP<=0)
            {
                Ani.SetTrigger("Death");
                State = WMONSTATE.DEATH;
                AClip = Resources.Load<AudioClip>("Sound/Monster/WMonDeath");
                ASource.PlayOneShot(AClip);
                GetComponent<BoxCollider2D>().enabled = false;
                if (StageManager)
                    StageManager.SubMonCount();
            }

        }
        else if (collision.gameObject.tag == "FPlayerAttack")
        {
            HP-=2;
            if (HP <= 0)
            {
                Ani.SetTrigger("Death");
                State = WMONSTATE.DEATH;
                AClip = Resources.Load<AudioClip>("Sound/Monster/WMonDeath");
                ASource.PlayOneShot(AClip);
                GetComponent<BoxCollider2D>().enabled = false;
                if (StageManager)
                    StageManager.SubMonCount();
            }
            else
            {
                Ani.SetTrigger("Hit");
                State = WMONSTATE.HIT;
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
