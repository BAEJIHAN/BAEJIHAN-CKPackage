using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BEETLESTATE
{
    APPEAR,
    IDLE,
    WALK,
    ROLLATTACK,
    SPIKEATTACK,
    JUMPATTACK,    
    GROGGY,
    DEAD,
    HIT,
    ROAR
    

}
public class BeetleScript : MonRootScript
{
    public GameObject CrackEffect;

    
    BEETLESTATE State=BEETLESTATE.IDLE;
    float Speed = 3;
    int PreRNum = -1;

    Vector3 DestPos = Vector3.zero;
    float WalkDist;
    float IdleTime;
    int WalkCount;

    Vector3 P1, P2, P3, P4;
    float JumpValue;
    float JumpSpeed = 0.7f;
    int JumpPhase = 0;

    float GroggyTime = 4;

    bool IsRolling=false;
    float CurRollTime = 0;
    float RollTime = 5;
    float RollSpeed = 10;

    bool IsGroggyRoll = false;
    Vector3 RollDir = Vector3.zero;
    float GroggyRollSpeed = 14;
    float GroggyRollTime = 10;

    float AppearSpeed = 6.0f;
    float AppearTime = 2;

    float RoarTime = 6.0f;

    bool OnEvent = true;

    //사운드
    public GameObject RollSound;
    [HideInInspector]public AudioSource RollSoundSource;
    // Start is called before the first frame update
    void Start()
    {
        PortraitTexture = Resources.Load<Texture>("Texture/Boss2Portrait");
        MaxHP = 60;
        HP = 60;
        base.Start();
        IdleTime = Random.Range(2.0f, 3.0f);
        State = BEETLESTATE.APPEAR;
        Ani.SetTrigger("Fall");
        RollSoundSource = RollSound.GetComponent<AudioSource>();
        RollSoundSource.volume = GValue.GSoundVolume;
    }

    // Update is called once per frame
    void Update()
    {
        AppearUpdate();

        IdleUpdate();

        WalkUpdate();

        JumpUpdate();

        GroggyUpdate();

        RollUpdate();

        RoarUpdate();

        if (!IsRolling)
            RollSoundSource.Stop();
    }

    void AppearUpdate()
    {
        if (BEETLESTATE.APPEAR != State)
            return;

        AppearTime -= Time.deltaTime;
        Pivot.transform.position += Time.deltaTime * Vector3.down * AppearSpeed;
        if (AppearTime <= 0)
        {
            Ani.SetTrigger("Landing");
            State= BEETLESTATE.ROAR;
            AClip = Resources.Load<AudioClip>("Sound/Monster/Stomp");
            ASource.PlayOneShot(AClip);
        }
            
    }
    

    void RoarUpdate()
    {
        if(BEETLESTATE.ROAR != State)
            return;

        RoarTime-= Time.deltaTime;
        if (RoarTime <= 0)
        {
            State = BEETLESTATE.IDLE;

            Ani.SetTrigger("PostRoar");
            
        }
            
    }
    void IdleUpdate()
    {
        if (BEETLESTATE.IDLE != State)
            return;

        IdleTime -= Time.deltaTime;
        if (IdleTime <0)
        {
            if (OnEvent)
                return;
            IdleTime = Random.Range(2.0f, 3.0f);
            TakeNextAct();
        }
    }
    void WalkUpdate()
    {
        if (BEETLESTATE.WALK != State)
            return;

        Vector3 Dir = DestPos - Pivot.transform.position;
        Dir.Normalize();
        Pivot.transform.position += Dir * Time.deltaTime * Speed;

        WalkDist -= Time.deltaTime * Speed;

        if(PlayerPivot.transform.position.x>Pivot.transform.position.x)//방향전환
        {
            Pivot.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            Pivot.transform.eulerAngles = new Vector3(0, 0, 0);
        }


        if(WalkDist<0)
        {
            WalkCount--;
            SetWalkDest();
            if (WalkCount<=0)
            {

                TakeNextAct();
            }
                
        }
    }

    void JumpUpdate()
    {
        if (BEETLESTATE.JUMPATTACK != State)//0일 때 뛰는 시늉. 1일때 상승. 2일때 하강. 3일때 착지.
            return;

        if (JumpPhase == 0)
            return;

        if (JumpPhase == 3)
            return;

        JumpValue += Time.deltaTime * JumpSpeed;
        Pivot.transform.position = MMath.Lerp(P1, P2, P3, P4, JumpValue);

        if(JumpValue > 0.5f && JumpPhase==1)
        {
            Ani.SetTrigger("Fall");
            JumpPhase++;
        }

        if (JumpValue > 1.0f && JumpPhase == 2)
        {
            Ani.SetTrigger("Landing");
            AClip = Resources.Load<AudioClip>("Sound/Monster/Stomp");
            ASource.PlayOneShot(AClip);
            JumpPhase++;
            GameObject CrackObj = Instantiate(CrackEffect);
            Vector3 tempV = Pivot.transform.position;
            tempV.y -= 2.3f;
            CrackObj.transform.position = tempV;
            CrackObj.transform.localScale = new Vector3(8.0f, 8.0f, 1.0f);
            PlayerPivot.GetComponent<PlayerScript>().SetStomp();
        }
    }

    void GroggyUpdate()
    {
        if (BEETLESTATE.GROGGY != State)
            return;

        GroggyTime -= Time.deltaTime;
        if(GroggyTime<0)
        {
            GroggyTime = 4;
            State = BEETLESTATE.SPIKEATTACK;
            Ani.SetTrigger("PostSpike");
        }
    }

    void RollUpdate()
    {
        if (BEETLESTATE.ROLLATTACK != State)
            return;

        if (!IsRolling)
            return;

        

        if(IsGroggyRoll)
        {
            CurRollTime += Time.deltaTime;

            Pivot.transform.position += RollDir * Time.deltaTime * GroggyRollSpeed;
            if (CurRollTime > GroggyRollTime)
            {
                CurRollTime = 0;
                Ani.SetTrigger("PostRoll");
            }
        }
        else
        {
            CurRollTime += Time.deltaTime;

            Pivot.transform.position += RollDir * Time.deltaTime * RollSpeed;
            if (CurRollTime > RollTime)
            {
                CurRollTime = 0;
                Ani.SetTrigger("PostRoll");
            }
        }
        

    }

    void SetWalkDest()
    {
        float Dist = (PlayerPivot.transform.position - Pivot.transform.position).magnitude;
        if(Dist>10)
        {
            DestPos = PlayerPivot.transform.position;
        }
        else
        {
            Vector3 tempV=Pivot.transform.position;
            DestPos = tempV + new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-4.0f, 4.0f), 0);
        }
        
        WalkDist = (DestPos - Pivot.transform.position).magnitude - 1.0f;
    }
    public void TakeNextAct()
    {
        if (BEETLESTATE.ROAR == State)
        {
            Ani.SetTrigger("PreRoar");                  
            return;
        }
           
        int RNum;
        while (true)
        {
            RNum = Random.Range(0, 4);
            if (RNum != PreRNum)
                break;
        }
        PreRNum = RNum;
       //RNum = 1;
        if (0==RNum)//걷기
        {
            State = BEETLESTATE.WALK;
            Ani.SetTrigger("Walk");
            WalkCount = Random.Range(1, 5);
            SetWalkDest();


        }
        else if(1==RNum)//점프
        {
            State = BEETLESTATE.JUMPATTACK;
            Ani.SetTrigger("Jump");
            JumpValue = 0;
            JumpPhase = 0;
            SetJumpPs();
        }
        else if (2 == RNum)//가시 날리기
        {
            State = BEETLESTATE.SPIKEATTACK;
            Ani.SetTrigger("PreSpike");
            
            
        }
        else if(3==RNum)//구르기
        {
            State = BEETLESTATE.ROLLATTACK;
            Ani.SetTrigger("PreRoll");
            AClip = Resources.Load<AudioClip>("Sound/Monster/RollStart");
            ASource.PlayOneShot(AClip);
            if (HP < 30)
                IsGroggyRoll = true;

            RollDir = -Pivot.transform.right;
            if (IsGroggyRoll)
            {
                int RYDir = Random.Range(0, 2);
                
                if (RYDir==0)
                {
                    RollDir += Pivot.transform.up;
                }
                else
                {
                    RollDir -= Pivot.transform.up;
                }
            }
            
            RollDir.Normalize();
        }
    }

    void SetJumpPs()
    {
        P1 = Pivot.transform.position;
        Vector3 Dir = PlayerPivot.transform.position - Pivot.transform.position;
        Dir.Normalize();
        float RScale = Random.Range(0.6f, 1.5f);
        Vector3 temp=PlayerPivot.transform.position;
        temp -= Dir * RScale;
        P4 = temp;
        float MidX = (P1.x + P4.x) / 2.0f;
        P2.x = (P1.x+MidX)/2.0f;
        P3.x = (P4.x + MidX) / 2.0f;
        P2.y = P1.y+3.0f;
        P3.y = P4.y+3.0f;

        if (PlayerPivot.transform.position.x > Pivot.transform.position.x)//방향전환
        {
            Pivot.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            Pivot.transform.eulerAngles = new Vector3(0, 0, 0);
        }

    }

    public void SetJumpPhase(int a)
    {
        JumpPhase = a;
    }

    public void SetRolling(bool b)
    {
        IsRolling = b;
    }

    public void SetBeetleState(BEETLESTATE _State)
    {
        State = _State;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {           
            
            string colName = collision.gameObject.name;
            if (IsRolling)//벽에 부딪혔을 때 방향전환
            {
                

                if (IsGroggyRoll)//체력 50이하일 때 대각선으로 방향전환
                {
                    if (colName == "BossRoomLeft")
                    {
                        RollDir.x = -RollDir.x;
                        Pivot.transform.eulerAngles = new Vector3(0, 180, 0);
                        AClip = Resources.Load<AudioClip>("Sound/Monster/RollHitWall");
                        ASource.PlayOneShot(AClip);
                    }

                    if (colName == "BossRoomRight")
                    {
                        RollDir.x = -RollDir.x;
                        Pivot.transform.eulerAngles = new Vector3(0, 0, 0);
                        AClip = Resources.Load<AudioClip>("Sound/Monster/RollHitWall");
                        ASource.PlayOneShot(AClip);
                    }

                    if (colName == "BossRoomUp")
                    {
                        RollDir.y = -RollDir.y;
                        AClip = Resources.Load<AudioClip>("Sound/Monster/RollHitWall");
                        ASource.PlayOneShot(AClip);
                    }

                    if (colName == "BossRoomDown")
                    {
                        RollDir.y = -RollDir.y;
                        AClip = Resources.Load<AudioClip>("Sound/Monster/RollHitWall");
                        ASource.PlayOneShot(AClip);
                    }


                }
                else//평시 좌우 방향전환
                {
                    

                    if (colName == "BossRoomLeft")
                    {
                        RollDir.x = -RollDir.x;
                        Pivot.transform.eulerAngles = new Vector3(0, 180, 0);
                        AClip = Resources.Load<AudioClip>("Sound/Monster/RollHitWall");
                        ASource.PlayOneShot(AClip);
                    }

                    if (colName == "BossRoomRight")
                    {
                        RollDir.x = -RollDir.x;
                        Pivot.transform.eulerAngles = new Vector3(0, 0, 0);
                        AClip = Resources.Load<AudioClip>("Sound/Monster/RollHitWall");
                        ASource.PlayOneShot(AClip);
                    }

                   
                }


            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (BEETLESTATE.DEAD == State)
            return;
        if (collision.tag == "PlayerAttack" || collision.tag == "FPlayerAttack")
        {
           
            if (collision.tag == "PlayerAttack")
            {
                if (BEETLESTATE.HIT == State)
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
                if (BEETLESTATE.GROGGY == State)
                {
                    TakeDamage(4);
                }
                else
                {
                    TakeDamage(2);
                }
            }
           
            UIMgrScript.Inst.SetBossHPBar(MaxHP, HP);
            if (HP <= 0)
            {
                State = BEETLESTATE.DEAD;
                Ani.SetTrigger("Dead");
                IsRolling = false;
                AClip = Resources.Load<AudioClip>("Sound/Monster/Roar4");
                ASource.PlayOneShot(AClip);
                BoxCollider2D[] Colliders = GetComponentsInChildren<BoxCollider2D>();
                for(int i=0; i< Colliders.Length; i++)
                {
                    Colliders[i].enabled = false;
                }
                CircleCollider2D[] CColliders = GetComponentsInChildren<CircleCollider2D>();
                for (int i = 0; i < CColliders.Length; i++)
                {
                    CColliders[i].enabled = false;
                }

                StageManager.ClearStage();
            }
           

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

       
    }

    public void OffEvent()
    {
        OnEvent = false;
    }
}
