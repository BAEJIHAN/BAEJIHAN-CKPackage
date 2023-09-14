using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FEYESTATE
{
    IDLE,
    CHASE,
    PREATTACK,
    ATTACK1,
    ATTACK2,
    HIT,
    DEATH,
    TOORIGIN
}
public class FEyeScript : MonRootScript
{

    public GameObject MonsterAttackObj;
    FEYESTATE State = FEYESTATE.IDLE;
    float Speed = 3;

    //Idle 관련
    float IdleSpeed = 2.5f;    
    Vector3 IdlePivotPos;
    Vector3[] IdleSubPos=new Vector3[4];
    Vector3 IdleDestPos;
    float IdleMoveDist;
    int IdleMoveCount = 0;

    //ToOrigin 관련
    Vector3 DestPos;
    Vector3 OriginPos;
    float OriginPosDist = 10;


    //강공격 관련
    float FAttackTime = 0.2f;
    float FAttackSpeed = 10;
    bool IsFAttackOn = false;

    //PreAttack 관련
    int PreAttackNum;  
    float PreAttackMoveDist;
    Vector3 PreAttackPivotPos;

    //Attack1 관련
    Vector3 P1;
    Vector3 P2;
    Vector3 P3;
    Vector3 P4;
    float Attack1Power;
    [HideInInspector] public bool IsAttack1On = false;

    //Attack2 관련
    float Attack2Speed=6;
    [HideInInspector] public bool IsAttack2On = false;

    //Death 관련
    float DeathHeight = 1.5f;
    float DeathSpeed = 2.0f;

    //MoveShake 관련
    float ShakeAcc;
    float ShakeFlag = 1f;
    float ShakeSpeed = 2.0f;
    float ShakeDistance = 0;

    //sound 관련
  

    private void Awake()
    {
        base.Awake();
       
    }
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        IdlePivotPos=Pivot.transform.position;
        OriginPos= Pivot.transform.position;
        SetIdlePos();
        PlayerDetectDist = 5;
        Pivot.transform.eulerAngles = new Vector3(0, 180, 0);
        HP = 8;
        


    }

    // Update is called once per frame
    void Update()
    {

        
        IdleUpdate();

        DetectPlayer();

        Chase();

        PreAttackUpdate();

        Attack1Update();

        Attack2Update();

        ToOriginUpdate();

        FAttackUpdate();

        DeathUpdate();

        MoveShakeUpdate();

        DeathBip();

       

    }    

    private void IdleUpdate()
    {
        if (FEYESTATE.IDLE != State)
            return;


        Vector3 Dir = (IdleDestPos - Pivot.transform.position);
        Dir.Normalize();
        Pivot.transform.position += Dir * Time.deltaTime * IdleSpeed;
        IdleMoveDist-= Time.deltaTime * IdleSpeed; ;
        if(IdleMoveDist < 0)
        {
            IdleMoveCount++;
            IdleMoveCount %= 4;

            if(IdleMoveCount==1)
                Pivot.transform.eulerAngles = new Vector3(0, 0, 0);
            else if(IdleMoveCount==3)
                Pivot.transform.eulerAngles = new Vector3(0, 180, 0);

            IdleDestPos = IdleSubPos[IdleMoveCount];
            IdleMoveDist = (IdleDestPos - Pivot.transform.position).magnitude;
            
        }

    }

    private void DetectPlayer()
    {
        if (FEYESTATE.IDLE != State)
            return;

        if((PlayerPivot.transform.position-Pivot.transform.position).magnitude<PlayerDetectDist)
        {
            State = FEYESTATE.CHASE;
            if(MonGroup)
                MonGroup.AwakeMons();
        }
    }

    private void Chase()
    {
        if (FEYESTATE.CHASE != State)
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
        Pivot.transform.position += (Dir * Time.deltaTime * Speed);

        if ((DestPos - Pivot.transform.position).magnitude < 2)//플레이어 가까이에 가면 공격 시작.
        {
            State = FEYESTATE.PREATTACK;


            PreAttackNum = Random.Range(4, 7);

            
            PreAttackPivotPos = Pivot.transform.position;
            Vector3 tempV = PreAttackPivotPos;
            tempV.x += Random.Range(-1.0f, 1.0f);
            tempV.y += Random.Range(-1.0f, 1.0f);
            DestPos = tempV;

            PreAttackMoveDist = (DestPos - Pivot.transform.position).magnitude;
        }
        //else if ((DestPos - Pivot.transform.position).magnitude > PlayerDetectDist * 2)//너무 멀어지면 제자리로 감.
        //{
        //    State = FEYESTATE.TOORIGIN;
        //}
    }

    private void PreAttackUpdate()
    {
        if (FEYESTATE.PREATTACK != State)
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
        PreAttackMoveDist -= Time.deltaTime * Speed;

        if (PreAttackMoveDist < 0)
        {
           
            
            PreAttackNum--;
           
            if (PreAttackNum == 0)//돌아다니기 끝나면 공격
            {
                AClip = Resources.Load<AudioClip>("Sound/Monster/FEyeAttack");                
                ASource.PlayOneShot(AClip);
                int RandomInt = Random.Range(0, 2);
                
                if (RandomInt == 1)
                {
                    State = FEYESTATE.ATTACK1;
                    Ani.SetTrigger("Attack1");                    
                    SetPs();
                }
                else
                {
                    State = FEYESTATE.ATTACK2;
                    Ani.SetTrigger("Attack2");
                }
                return;
            }                    

            Vector3 tempV = PreAttackPivotPos;
            tempV.x += Random.Range(-1.0f, 1.0f);
            tempV.y += Random.Range(-1.0f, 1.0f);
            DestPos = tempV;
            PreAttackMoveDist = (DestPos - Pivot.transform.position).magnitude;

        }

        if ((PlayerPivot.transform.position - Pivot.transform.position).magnitude > 4)//도중에 플레이어와 멀어짐
        {
            State = FEYESTATE.CHASE;
            
        }
    }

    void Attack1Update()
    {
        if (FEYESTATE.ATTACK1 != State)
            return;

        if(IsAttack1On)
        {
            Attack1Power += Time.deltaTime*1.2f;
            Pivot.transform.position = MMath.Lerp(P1, P2, P3, P4, Attack1Power);
            if(Attack1Power>=1.0f)
            {
                Attack1Power = 0;
                IsAttack1On = false;
                MonsterAttackObj.gameObject.SetActive(false);
                
                EndAttack();
               

            }
        }
       
    }
    void Attack2Update()
    {
        if (FEYESTATE.ATTACK2 != State)
            return;

        if(!IsAttack2On)
        {
            Vector3 tempDir = -Pivot.transform.right;
            tempDir.y = 1f;
            tempDir.Normalize();
            Pivot.transform.position += tempDir * Time.deltaTime * Attack2Speed * 0.1f;

        }
        else
        {
            Pivot.transform.position += Pivot.transform.right * Time.deltaTime * Attack2Speed;
        }
    }
    private void ToOriginUpdate()
    {
        if (FEYESTATE.TOORIGIN != State)
            return;

        Vector3 Dir = OriginPos - Pivot.transform.position;
        Dir.Normalize();

        Pivot.transform.position += Dir * Time.deltaTime * Speed;
        OriginPosDist -= Time.deltaTime * Speed;

        if (OriginPos.x < Pivot.transform.position.x)//방향전환
        {
            Pivot.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            Pivot.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (OriginPosDist < 0)
        {
            State = FEYESTATE.IDLE;
            OriginPosDist = 10;
            IdlePivotPos = Pivot.transform.position;
            SetIdlePos();
        }
    }

    private void FAttackUpdate()
    {

        if (!IsFAttackOn)
            return;

        Vector3 tempDir;
        if(PlayerPivot.transform.position.x>Pivot.transform.position.x)
        {
            tempDir = Vector3.left;
        }
        else
        {
            tempDir = Vector3.right;
        }
        Pivot.transform.position += Time.deltaTime * FAttackSpeed * tempDir;
        FAttackTime -= Time.deltaTime;
        if (FAttackTime < 0)
        {
            FAttackTime = 0.2f;
            IsFAttackOn = false;
        }
     
        
    }

    void MoveShakeUpdate()
    {
        if(FEYESTATE.CHASE==State || FEYESTATE.PREATTACK == State)
        {
            ShakeAcc = Time.deltaTime * ShakeSpeed * ShakeFlag;
            Pivot.transform.position += new Vector3(0, ShakeAcc, 0);
            ShakeDistance += Time.deltaTime * ShakeSpeed;
            if (ShakeDistance>0.5f)
            {
                ShakeFlag = -ShakeFlag;
                ShakeDistance = 0;
               
            }
        }
    }

    private void DeathUpdate()
    {
        if (FEYESTATE.DEATH != State)
            return;

        if(DeathHeight>0)
        {
            DeathHeight -= Time.deltaTime * DeathSpeed;
            Vector3 tempV = Pivot.transform.position;
            tempV.y -= Time.deltaTime * DeathSpeed;
            Pivot.transform.position = tempV;
            IsFadeOn = true;
        }

    }
    void SetIdlePos()
    {
        IdleSubPos[0] = IdlePivotPos+new Vector3(-1,1,0);
        IdleSubPos[1] = IdlePivotPos + new Vector3(1, -1, 0);
        IdleSubPos[2] = IdlePivotPos + new Vector3(1, 1, 0);
        IdleSubPos[3] = IdlePivotPos + new Vector3(-1, -1, 0);
        IdleDestPos = IdleSubPos[0];
        IdleMoveDist = (IdleDestPos - Pivot.transform.position).magnitude;
    }

    void SetPs()
    {

        P1 = Pivot.transform.position;
        P2 = P1;
        P2.y += -1f;
        P3 = P2;
        if (PlayerPivot.transform.position.x < Pivot.transform.position.x)//방향전환
        {           
            P3.x += -5;
            P4 = P1;
            P4.x += -5;
        }
        else
        {
            P3.x += 5;
            P4 = P1;
            P4.x += 5;
        }
        
    }

    public override void AwakeMon()
    {
        State = FEYESTATE.CHASE;     

    }
    public void EndAttack()
    {
       
        Ani.SetTrigger("Flight");
        if ((PlayerPivot.transform.position - Pivot.transform.position).magnitude > 4)//도중에 플레이어와 멀어짐
        {
            State = FEYESTATE.CHASE;

        }
        else
        {
            State = FEYESTATE.PREATTACK;
            PreAttackPivotPos = Pivot.transform.position;
            Vector3 tempV = PreAttackPivotPos;
            tempV.x += Random.Range(-1.0f, 1.0f);
            tempV.y += Random.Range(-1.0f, 1.0f);
            DestPos = tempV;
            PreAttackNum = Random.Range(4, 7);
            PreAttackMoveDist = (DestPos - Pivot.transform.position).magnitude;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (FEYESTATE.DEATH == State)
            return;

        if (collision.tag == "PlayerAttack")
        {
            HP--;
            if(HP<=0)
            {
                Ani.SetTrigger("Death");
                State = FEYESTATE.DEATH;

                BoxCollider2D[] Colliders = GetComponentsInChildren<BoxCollider2D>();
                for (int i = 0; i < Colliders.Length; i++)
                {
                    Colliders[i].enabled = false;
                }
                if (StageManager)
                    StageManager.SubMonCount();
            }
            else
            {
                Ani.SetTrigger("Hit");
                State = FEYESTATE.HIT;
            }

           
        }

        if (collision.tag == "FPlayerAttack")
        {
            HP-=2;
            if (HP <= 0)
            {
                Ani.SetTrigger("Death");
                State = FEYESTATE.DEATH;

                BoxCollider2D[] Colliders = GetComponentsInChildren<BoxCollider2D>();
                for (int i = 0; i < Colliders.Length; i++)
                {
                    Colliders[i].enabled = false;
                }
                if (StageManager)
                    StageManager.SubMonCount();
            }
            else
            {
                Ani.SetTrigger("Hit");
                State = FEYESTATE.HIT;
                IsFAttackOn = true;
            }
            
        }
    }
}
