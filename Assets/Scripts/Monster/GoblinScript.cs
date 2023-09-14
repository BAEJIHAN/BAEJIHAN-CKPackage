using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public enum GOBLINSTATE
{
    IDLE,
    CHASE,
    COMBAT,
    ATTACK1,
    ATTACK2,
    HIT,
    FHIT,
    RISE,
    DEATH
}

public class GoblinScript : MonRootScript
{
    GOBLINSTATE State= GOBLINSTATE.IDLE;

    Vector3 DestPos;
    float Speed=5;

    //Combat 관련
    float CombatMoveDist;
    int CombatNum;
    Vector3 CombatPivot;

    //Attack2 관련
    //뒷점프 포물선 기준점
    Vector3 P1, P2, P3, P4;
    float BackValue;
    float BackSpeed = 2;

    int Attack2Num = 0;
    float Attack2Speed = 10;

    //FHIT 관련
    Vector3 FP1, FP2, FP3, FP4;
    float FBackValue;
    float FBackSpeed=1.5f;
    float FHitTime=2;
    float CurFHitTime = 0;
    // Start is called before the first frame update


    void Start()
    {
        
        base.Start();
        PlayerDetectDist = 8;
        HP = 10;
    }

    // Update is called once per frame
    void Update()
    {  

        DetectPlayer();

        Chase();

        CombatUpdate();

        Attack1Update();

        Attack2Update(); 

        FHitUpdate();

        DeathUpdate();

        StuckUpdate();



    }

  

    private void DetectPlayer()
    {
        if (GOBLINSTATE.IDLE != State)
            return;

        if ((PlayerPivot.transform.position - Pivot.transform.position).magnitude < PlayerDetectDist)
        {
            State = GOBLINSTATE.CHASE;
            Ani.SetTrigger("Run");
            if (MonGroup)
                MonGroup.AwakeMons();
        }
    }

    private void Chase()
    {
        if (GOBLINSTATE.CHASE != State)
            return;       


        if (PlayerPivot.transform.position.x < Pivot.transform.position.x)//방향전환
        {
            Pivot.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            Pivot.transform.eulerAngles = new Vector3(0, 0, 0);
        }


        Vector3 Dir = PlayerPivot.transform.position - Pivot.transform.position;
        Dir.Normalize();
        Pivot.transform.position += (Dir * Time.deltaTime * Speed);

        if ((PlayerPivot.transform.position - Pivot.transform.position).magnitude < 1)//플레이어 가까이에 가면 공격 시작.
        {
            State = GOBLINSTATE.COMBAT;
            CombatPivot = Pivot.transform.position;
            CombatNum = Random.Range(1, 5);    
                                         
            Vector3 tempV = CombatPivot;
            tempV.x += Random.Range(-1.0f, 1.0f);
            tempV.y += Random.Range(-1.0f, 1.0f);
            DestPos = tempV;

            CombatMoveDist = (DestPos - Pivot.transform.position).magnitude;
        }
    }

    private void CombatUpdate()
    {
        if (GOBLINSTATE.COMBAT != State)
            return;

        Vector3 Dir = DestPos - Pivot.transform.position;
        Dir.Normalize();

        if (PlayerPivot.transform.position.x < Pivot.transform.position.x)//방향전환
        {
            Pivot.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            Pivot.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        Pivot.transform.position += (Dir * Time.deltaTime * Speed);
        CombatMoveDist -= Time.deltaTime * Speed;

        if (CombatMoveDist < 0)
        {


            CombatNum--;

            if (CombatNum == 0)//돌아다니기 끝나면 공격
            {

                int RandomInt = Random.Range(0, 3);
                AttackSoundPlay();
                if (RandomInt >= 1)
                {
                    State = GOBLINSTATE.ATTACK1;
                    Ani.SetTrigger("Attack1");
                    SetPs();
                    
                }
                else
                {
                    State = GOBLINSTATE.ATTACK2;
                    Ani.SetTrigger("Attack2");
                }
                return;
            }

            Vector3 tempV = CombatPivot;
            tempV.x += Random.Range(-1.0f, 1.0f);
            tempV.y += Random.Range(-1.0f, 1.0f);
            DestPos = tempV;
            CombatMoveDist = (DestPos - Pivot.transform.position).magnitude;

        }

        if ((PlayerPivot.transform.position - Pivot.transform.position).magnitude > 4)//도중에 플레이어와 멀어짐
        {
            State = GOBLINSTATE.CHASE;

        }

    }

    void Attack1Update()
    {
        if (GOBLINSTATE.ATTACK1 != State)
            return;

      

    }
    void Attack2Update()
    {
        if (GOBLINSTATE.ATTACK2 != State)
            return;

        if(Attack2Num==1)
        {
            BackValue += Time.deltaTime * BackSpeed;
            Pivot.transform.position=MMath.Lerp(P1, P2, P3, P4, BackValue);
            if(BackValue>1.0f)
            {
                BackValue = 0;
                Attack2Num++;
            }
        }

        if (Attack2Num == 2)
        {

        }

        if (Attack2Num == 3)
        {
            Pivot.transform.position += Pivot.transform.right * Time.deltaTime * Attack2Speed;
        }
    }
   

    private void FHitUpdate()
    {
        if (GOBLINSTATE.FHIT != State)
            return;
        
        FBackValue += Time.deltaTime * FBackSpeed;
        Pivot.transform.position=MMath.Lerp(FP1, FP2, FP3, FP4, FBackValue);

        if(FBackValue>1)
        {
            CurFHitTime += Time.deltaTime;
            if(CurFHitTime>FHitTime)
            {
                CurFHitTime = 0;

                State = GOBLINSTATE.RISE;
                Ani.SetTrigger("Rise");
            }
        }

    }

   

    private void DeathUpdate()
    {
        if (GOBLINSTATE.DEATH != State)
            return;
               
        FBackValue += Time.deltaTime * FBackSpeed;
        Pivot.transform.position = MMath.Lerp(FP1, FP2, FP3, FP4, FBackValue);

        if (FBackValue > 1)
        {
            CurFHitTime += Time.deltaTime;
            if (CurFHitTime > FHitTime)
            {               
                                

                base.DeathBip();
            }
        }

    }

    void StuckUpdate()
    {
        //끼었을 때 돌아가는 함수
        if (!IsStuck)
            return;

        if (State != GOBLINSTATE.CHASE && State != GOBLINSTATE.COMBAT)
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
    public void SetPs()
    {

        P1 = Pivot.transform.position;
        P2 = P1;
        P2.y += 3f;
        P3 = P2;
        if (PlayerPivot.transform.position.x < Pivot.transform.position.x)//방향전환
        {
            P3.x += 2;
            P4 = P1;
            P4.x += 2;
        }
        else
        {
            P3.x += -2;
            P4 = P1;
            P4.x += -2;
        }
        BackValue = 0;
    }

    public void SetFPs()
    {

        FP1 = Pivot.transform.position;
        FP2 = FP1;
        FP2.y += 1.5f;
        FP3 = FP2;
        if (PlayerPivot.transform.position.x < Pivot.transform.position.x)//방향전환
        {
            FP3.x += 2.5f;
            FP4 = FP1;
            FP4.x += 2.5f;
        }
        else
        {
            FP3.x += -2.5f;
            FP4 = FP1;
            FP4.x += -2.5f;
        }
        FBackValue = 0;
    }

    public override void AwakeMon()
    {
        State = GOBLINSTATE.CHASE;
        Ani.SetTrigger("Run");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GOBLINSTATE.DEATH == State)
            return;

        if (collision.tag == "PlayerAttack")
        {
            HP--;
           
            if (HP <= 0)
            {
                SetFPs();
                Ani.SetTrigger("Death");
                State = GOBLINSTATE.DEATH;
                AClip = Resources.Load<AudioClip>("Sound/Monster/GoblinDeath");
                ASource.PlayOneShot(AClip);
                IsFadeOn = true;
                GetComponent<BoxCollider2D>().enabled = false;
                if (StageManager)
                    StageManager.SubMonCount();
            }
            else
            {
                Ani.SetTrigger("Hit");
                State = GOBLINSTATE.HIT;
            }
        }

        if (collision.tag == "FPlayerAttack")
        {
            HP -= 2;
            SetFPs();
            
            if (HP <= 0)//진짜 죽음.
            {
                Ani.SetTrigger("Death");
                State = GOBLINSTATE.DEATH;
                AClip = Resources.Load<AudioClip>("Sound/Monster/GoblinDeath");
                ASource.PlayOneShot(AClip);
                IsFadeOn = true;
                GetComponent<BoxCollider2D>().enabled = false;
                if (StageManager)
                    StageManager.SubMonCount();
            }
            else//진짜 죽는 거 아님.
            {
                Ani.SetTrigger("Death");
                State = GOBLINSTATE.FHIT;
                AClip = Resources.Load<AudioClip>("Sound/Monster/GoblinHit");
                ASource.PlayOneShot(AClip);
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

    public void SetGoblinState(GOBLINSTATE _state)
    {
        State = _state;
    }

    public void AddAttack2Num()
    {
        Attack2Num++;
    }

    public void ZeroAttack2Num()
    {
        Attack2Num = 0;
    }

    public void AttackSoundPlay()
    {
        string temp = "Sound/Monster/Goblin";
        int RanNum = Random.Range(1, 6);
        temp += RanNum.ToString();
        AClip = Resources.Load<AudioClip>(temp);
        ASource.PlayOneShot(AClip);
    }

  
}
