using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum DIR
{
    Left,
    Right
}

public enum PLAYERSTATE
{
    NONE = 0,
    IDLE,
    RUN,
    ATTACK1,
    ATTACK2,
    ATTACK3,
    JUMP1,
    JUMP2,
    JUMP3,
    SLIDE,
    HURT,
    AIRHURT1,
    AIRHURT2,
    AIRHURT3,
    DEATH
}
public class PlayerScript : MonoBehaviour
{
    StageManagerRoot StageManager;
    Animator Ani;
    public GameObject FootStep;
    [HideInInspector] public DIR Dir = DIR.Left;
    PLAYERSTATE State = PLAYERSTATE.IDLE;
    SpriteRenderer Renderer;

    GameObject Pivot;
    float Speed = 5;

    //스테이터스
    
    
    float MaxStamina = 100;
    float CurStamina = 100;
    float StaminaRetoreSpeed = 20;

    int PressedKey = 0;

    [HideInInspector]public bool IsAttack2Ready = false;
    [HideInInspector]public  bool Attack2Check = true; 

    //점프
    float JumpMaxHeight = 2.0f;
    float JumpCurHeight=0;
    float JumpSpeed=7.5f;

    //슬라이딩
    float SlideSpeed = 8;
    float SlideCool = 3;
    float SlideCurCool = 0;
    bool IsOnSlideCool = false;

    //공중피격
    float JumpStartHeight;
    float AirHurtSpeed = 3;

    //지진 공격 당함
    bool IsStomped = false;
    

    // 마지막 공격
    bool FAttackMoveOn = false;
    float FAttackMoveSpeed = 0.8f;

    //이벤트
    bool IsOnEvent = false;

    //spike 피격
    bool GetSpiked = false;
    float SpikeTime = 0;

    //사운드
    AudioClip AClip;
    AudioSource ASource;
    AudioSource FootStepSource;
    // Start is called before the first frame update
    void Awake()
    {
        ASource = GetComponent<AudioSource>();
        FootStepSource = FootStep.GetComponent<AudioSource>();
       
        ASource.volume = GValue.GSoundVolume;
        FootStepSource.volume = GValue.GSoundVolume;
        Ani = GetComponentInChildren<Animator>();
        Renderer = GetComponentInChildren<SpriteRenderer>();
        Pivot = this.gameObject;
    }
    void Start()
    {
        StageManager = FindObjectOfType<StageManagerRoot>();
        if (Ani == null)
        {

            Debug.Log("AniNull");
        }

        if (UIMgrScript.Inst != null)
            UIMgrScript.Inst.SetHPBar(GValue.MAXHP, GValue.PHP);

        ASource.clip = Resources.Load<AudioClip>("Sound/Player/Walk");
        ASource.loop = true;

    }

   
    void Update()
    {
        if (IsOnEvent)
        {
            FootStepSource.Stop();
            return;
        }

        
        PressedKey = 0;

        MoveKeyCheck();

        KeyCheck();

        GetSpikedUpdate();
        if (PLAYERSTATE.RUN != State)
            FootStepSource.Stop();
        //Move();

            //JumpUpdate();

            //SlideUpdate();

            //AirHurtUpdate();

            //Attack3Update();
            //if (IsOnSlideCool)
            //{
            //    SlideCurCool += Time.deltaTime;
            //    if (SlideCurCool > SlideCool)
            //    {
            //        SlideCurCool = 0;
            //        IsOnSlideCool = false;
            //    }
            //}
    }
    


    private void FixedUpdate()
    {
        
        if (IsOnEvent)
            return;
        //PressedKey = 0;

        //MoveKeyCheck();

        //KeyCheck();

        Move();

        JumpUpdate();

        SlideUpdate();

        AirHurtUpdate();

        Attack3Update();
        if (IsOnSlideCool)
        {
            SlideCurCool += Time.deltaTime;
            if (SlideCurCool > SlideCool)
            {
                SlideCurCool = 0;
                IsOnSlideCool = false;
            }
        }

        
    }

    private void LateUpdate()
    {
        StaminaUpdate();
    }

    void MoveKeyCheck()
    {
        if (PLAYERSTATE.DEATH == State)
            return;
        
        if (Input.GetKey(KeyCode.RightArrow))//오른쪽 키
        {
            PressedKey = PressedKey | 0b00000001;
        }

        if (Input.GetKey(KeyCode.LeftArrow))//왼쪽 키
        {
            PressedKey = PressedKey | 0b00000010;
        }

        if (Input.GetKey(KeyCode.UpArrow))//위 키
        {
            PressedKey = PressedKey | 0b00000100;
        }

        if (Input.GetKey(KeyCode.DownArrow))//아래 키
        {
            PressedKey = PressedKey | 0b00001000;
        }
    }

    private void KeyCheck()
    {
        if (PLAYERSTATE.DEATH == State)
            return;

        if (PressedKey== 0b00000001 //이동 키 눌렸을 때
            ||  PressedKey== 0b00000010
            || PressedKey == 0b00000100
            || PressedKey == 0b00001000
            || PressedKey == 0b00000101
            || PressedKey == 0b00001001
            || PressedKey == 0b00000110
            || PressedKey == 0b00001010)
        {
            if(PLAYERSTATE.IDLE == State)//Idle 상태면 움직임.
            {
                Ani.SetTrigger("Run");
                State = PLAYERSTATE.RUN;
                FootStepSource.Play();
            }
        }
        else if(!(PressedKey == 0b00000001//이동 키 안 눌리거나 8방향키 아니면
            || PressedKey == 0b00000010
            || PressedKey == 0b00000100
            || PressedKey == 0b00001000
            || PressedKey == 0b00000101
            || PressedKey == 0b00001001
            || PressedKey == 0b00000110
            || PressedKey == 0b00001010))
        {
            if (PLAYERSTATE.RUN == State)//Run 상태면 Idle로
            {
                Ani.SetTrigger("Idle");
                State = PLAYERSTATE.IDLE;
            }
        }

        if(Input.GetKeyDown(KeyCode.Z) )//일반 공격
        {
            if(PLAYERSTATE.IDLE==State 
                || PLAYERSTATE.RUN==State)
            {
                Ani.SetTrigger("Attack1");
                State = PLAYERSTATE.ATTACK1;
                AClip = Resources.Load<AudioClip>("Sound/Player/Attack1");
                ASource.PlayOneShot(AClip);
            }
            
        }

        if(Input.GetKeyDown(KeyCode.X))//콤보 공격
        {
            if(PLAYERSTATE.ATTACK1 == State
                && IsAttack2Ready
                && Attack2Check)
            {
                IsAttack2Ready = false;
                Ani.SetTrigger("Attack2");
                State = PLAYERSTATE.ATTACK2;
                AClip = Resources.Load<AudioClip>("Sound/Player/Attack2");
                ASource.PlayOneShot(AClip);
            }
            else if(PLAYERSTATE.ATTACK1 == State
                && !IsAttack2Ready)
            {
                Attack2Check = false;
            }            
            else if(PLAYERSTATE.ATTACK2==State)
            {
                Ani.SetTrigger("Attack3");
                State = PLAYERSTATE.ATTACK3;
                AClip = Resources.Load<AudioClip>("Sound/Player/FAttack");
                ASource.PlayOneShot(AClip);
            }       
                
        }     
        
        if(Input.GetKeyDown(KeyCode.C))//점프
        {
            if (PLAYERSTATE.IDLE == State || PLAYERSTATE.RUN == State)
            {
                Ani.SetTrigger("Jump1");
                State = PLAYERSTATE.JUMP1;
                JumpStartHeight = transform.position.y;
                AClip = Resources.Load<AudioClip>("Sound/Player/Jump");
                ASource.PlayOneShot(AClip);
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))//슬라이드
        {
            if((PLAYERSTATE.IDLE==State
                || PLAYERSTATE.RUN==State) && CurStamina>=MaxStamina)
            {
               
                SlideCurCool = 0;
                Ani.SetTrigger("Slide");
                State = PLAYERSTATE.SLIDE;
                CurStamina = 0;

                AClip = Resources.Load<AudioClip>("Sound/Player/Sliding");
                ASource.PlayOneShot(AClip);

                if (UIMgrScript.Inst != null)
                    UIMgrScript.Inst.SetStaminaBar(MaxStamina, CurStamina);
            }
            
        }
    }

    void Move()
    {
        if (PLAYERSTATE.RUN != State)
            return;
        if (PressedKey == 0b00000001)  //오른쪽 키만 눌렸을 때
        {
            Dir = DIR.Right;
            Pivot.transform.eulerAngles = new Vector3(0, 0, 0);           
            Pivot.transform.position+=new Vector3(Speed * Time.deltaTime, 0, 0);
        }
        else if (PressedKey == 0b00000010)   //왼쪽 키만 눌렸을 때
        {
            Dir = DIR.Left;
            Pivot.transform.eulerAngles = new Vector3(0, 180, 0);
            Pivot.transform.position += new Vector3(-Speed * Time.deltaTime, 0, 0);
        }
        else if (PressedKey == 0b00000100)   //위 키만 눌렸을 때
        {            
            Pivot.transform.position += new Vector3(0, Speed * Time.deltaTime, 0);
        }
        else if (PressedKey == 0b00001000)   //아래 키만 눌렸을 때
        {
            
            Pivot.transform.position += new Vector3(0, -Speed * Time.deltaTime, 0);
        }
        else if (PressedKey == 0b00000101)   //우상
        {
            Dir = DIR.Right;
            Pivot.transform.eulerAngles = new Vector3(0, 0, 0);            
            Pivot.transform.position += new Vector3(Speed * Time.deltaTime * 0.707f, Speed * Time.deltaTime * 0.707f, 0);
        }
        else if (PressedKey == 0b00001001)   //우하
        {
            Dir = DIR.Right;
            Pivot.transform.eulerAngles = new Vector3(0, 0, 0);            
            Pivot.transform.position += new Vector3(Speed * Time.deltaTime * 0.707f, -Speed * Time.deltaTime * 0.707f, 0);
        }
        else if (PressedKey == 0b00000110)   //좌상
        {
            Dir = DIR.Left;
            Pivot.transform.eulerAngles = new Vector3(0, 180, 0);            
            Pivot.transform.position += new Vector3(-Speed * Time.deltaTime * 0.707f, Speed * Time.deltaTime * 0.707f, 0);

        }
        else if (PressedKey == 0b00001010)   //좌하
        {
            Dir = DIR.Left;
            Pivot.transform.eulerAngles = new Vector3(0, 180, 0);            
            Pivot.transform.position += new Vector3(-Speed * Time.deltaTime * 0.707f, -Speed * Time.deltaTime * 0.707f, 0);
        }     
        else
        {
                        
        }

    } 

    void JumpUpdate()
    {
        if(PLAYERSTATE.JUMP1 != State 
            && PLAYERSTATE.JUMP2 != State 
            && PLAYERSTATE.JUMP3 != State)
        {
            return;
        }

        if (PressedKey == 0b00000001)  //오른쪽 키만 눌렸을 때
        {
            Dir = DIR.Right;
            Pivot.transform.eulerAngles = new Vector3(0, 0, 0);            
            Pivot.transform.position += new Vector3(Speed * Time.deltaTime, 0, 0);
        }
        else if (PressedKey == 0b00000010)   //왼쪽 키만 눌렸을 때
        {
            Dir = DIR.Left;
            Pivot.transform.eulerAngles = new Vector3(0, 180, 0);            
            Pivot.transform.position += new Vector3(-Speed * Time.deltaTime, 0, 0);
        }

        if (PLAYERSTATE.JUMP1==State)
        {
            
            JumpCurHeight += Time.deltaTime * JumpSpeed;           
            Pivot.transform.position += new Vector3(0, JumpSpeed * Time.deltaTime, 0);
            if (JumpCurHeight>=JumpMaxHeight)
            {
                Ani.SetTrigger("Jump2");
                State = PLAYERSTATE.JUMP2;
            }
        }

        if(PLAYERSTATE.JUMP3==State)
        {
            JumpCurHeight -= Time.deltaTime * JumpSpeed;           
            Pivot.transform.position += new Vector3(0, -JumpSpeed * Time.deltaTime, 0);
            if (JumpCurHeight <= 0)
            {
                Ani.SetTrigger("Idle");
                State = PLAYERSTATE.IDLE;
                AClip = Resources.Load<AudioClip>("Sound/Player/Landing");
                ASource.PlayOneShot(AClip);
            }
        }
    }

    private void SlideUpdate()
    {
        if(PLAYERSTATE.SLIDE!=State)
        {
            return;
        }
       
        if(Dir == DIR.Right)
        {
            Pivot.transform.position += new Vector3(SlideSpeed * Time.deltaTime, 0, 0);
        }
        else if(Dir == DIR.Left)
        {
            Pivot.transform.position += new Vector3(-SlideSpeed * Time.deltaTime, 0, 0);
        }
       

    }

    private void AirHurtUpdate()
    {
        if(PLAYERSTATE.AIRHURT1!=State && PLAYERSTATE.AIRHURT2!=State && PLAYERSTATE.AIRHURT3 != State)
        {
            return;
        }

        if(PLAYERSTATE.AIRHURT1==State)
        {
            if (Dir == DIR.Right)
            {
                Pivot.transform.position += new Vector3(-Time.deltaTime * AirHurtSpeed, Time.deltaTime * AirHurtSpeed, 0);
            }
            else if (Dir == DIR.Left)
            {
                Pivot.transform.position += new Vector3(Time.deltaTime * AirHurtSpeed, Time.deltaTime * AirHurtSpeed, 0);
            }
            
            JumpCurHeight += Time.deltaTime * AirHurtSpeed;
        }
        else if(PLAYERSTATE.AIRHURT2==State)
        {
            if (Dir == DIR.Right)
            {
                Pivot.transform.position += new Vector3(-Time.deltaTime * AirHurtSpeed * 0.5f, -Time.deltaTime * AirHurtSpeed, 0);
            }
            else if (Dir == DIR.Left)
            {
                Pivot.transform.position += new Vector3(Time.deltaTime * AirHurtSpeed * 0.5f, -Time.deltaTime * AirHurtSpeed, 0);
            }
           
            JumpCurHeight -= Time.deltaTime * AirHurtSpeed;
            if (JumpCurHeight < 0)
            {
                State = PLAYERSTATE.AIRHURT3;
                Ani.SetTrigger("AirHurt3");
            }
        }
        else if(PLAYERSTATE.AIRHURT3==State)
        {

        }
    }

    void GetSpikedUpdate()
    {
        if(GetSpiked)
        {
            SpikeTime += Time.deltaTime;
            if (SpikeTime > 1.5f)
            {
                SpikeTime = 0;
                GetSpiked = false;
            }
                
        }
    }

    void Attack3Update()
    {
        if (PLAYERSTATE.DEATH == State)
            return;

        if (!FAttackMoveOn)
        {
            return;
        }

        Pivot.transform.position += new Vector3(Pivot.transform.right.x*Time.deltaTime* FAttackMoveSpeed, 0, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PLAYERSTATE.DEATH == State)
            return;

        if ((collision.tag=="MonsterAttack" || collision.tag == "BeetleSpike") && State != PLAYERSTATE.SLIDE)//슬라이드 상태에서는 공격 안 맞음
        {
            
            if (GetSpiked)
                return;

            
            if (collision.tag == "BeetleSpike")
            {
                GetSpiked = true;
            }

            


            GValue.PHP -= collision.gameObject.GetComponent<MonsterAttackScript>().Damage;
            
            if (UIMgrScript.Inst != null)
                UIMgrScript.Inst.SetHPBar(GValue.MAXHP, GValue.PHP);
            if (GValue.PHP < 0)
            {
                StartCoroutine(PlayerDeathEvent());
                Ani.SetTrigger("Death");
                State = PLAYERSTATE.DEATH;
                GetComponentInChildren<BoxCollider2D>().enabled = false;
                Time.timeScale = 0.3f;
                AClip = Resources.Load<AudioClip>("Sound/Player/Death");
                ASource.PlayOneShot(AClip);
                return;
            }

            string tempS = "Sound/Player/Hit"+ Random.Range(1, 4).ToString();
            
            AClip = Resources.Load<AudioClip>(tempS);
            ASource.PlayOneShot(AClip);

            if (PLAYERSTATE.JUMP1 == State || PLAYERSTATE.JUMP2 == State || PLAYERSTATE.JUMP3 == State)
            {
                Ani.SetTrigger("AirHurt1");
                State = PLAYERSTATE.AIRHURT1;
                SetFAttackMoveOn(false);
            }
            else
            {
                Ani.SetTrigger("Hurt");
                State = PLAYERSTATE.HURT;
                SetFAttackMoveOn(false);
            }

            



        }
       
        if(collision.gameObject.name=="CatToPlayer" & State==PLAYERSTATE.SLIDE)
        {
            GameObject tempObj= collision.gameObject.transform.parent.gameObject;
            tempObj.GetComponent<CatScript>().SetFreeCat();
            UIMgrScript.Inst.SpawnHPUp(Pivot.transform.position);
            GValue.PHP += 20;
            if(GValue.PHP >= 100)
            {
                GValue.PHP = 100;
            }
            UIMgrScript.Inst.SetHPBar(GValue.MAXHP, GValue.PHP);
        }
    }

    void StaminaUpdate()
    {
        if(CurStamina>=MaxStamina)
        {
            return;
        }

        CurStamina += Time.deltaTime * StaminaRetoreSpeed;

        if(UIMgrScript.Inst!= null)
            UIMgrScript.Inst.SetStaminaBar(MaxStamina, CurStamina);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        
    }
   
    public void SetStomp()
    {
        if(PLAYERSTATE.JUMP1==State ||
            PLAYERSTATE.JUMP2 == State ||
            PLAYERSTATE.JUMP3 == State ||
            PLAYERSTATE.AIRHURT1 == State ||
            PLAYERSTATE.AIRHURT2 == State ||
            PLAYERSTATE.AIRHURT3 == State ||
            PLAYERSTATE.HURT==State ||
            PLAYERSTATE.DEATH==State)
        {
            return;
        }

        

        GValue.PHP -= 10;

        if (UIMgrScript.Inst != null)
            UIMgrScript.Inst.SetHPBar(GValue.MAXHP, GValue.PHP);

        if (GValue.PHP < 0)
        {
            StartCoroutine(PlayerDeathEvent());
            Ani.SetTrigger("Death");
            State = PLAYERSTATE.DEATH;
            GetComponentInChildren<BoxCollider2D>().enabled = false;
            AClip = Resources.Load<AudioClip>("Sound/Player/Death");
            ASource.PlayOneShot(AClip);
            Time.timeScale = 0.3f;
            return;
        }

        string tempS = "Sound/Player/Hit" + Random.Range(1, 4).ToString();

        AClip = Resources.Load<AudioClip>(tempS);
        ASource.PlayOneShot(AClip);

        Ani.SetTrigger("AirHurt1");
        State = PLAYERSTATE.AIRHURT1;
        SetFAttackMoveOn(false);

    }

    public void SetHP(float hp)
    {
        GValue.PHP = hp;
    }
    public void AddStamina(float stamina)
    {
        CurStamina += stamina;
        if(CurStamina >= MaxStamina)
        {
            CurStamina = MaxStamina;
        }

        if (UIMgrScript.Inst!=null)
            UIMgrScript.Inst.SetStaminaBar(CurStamina, MaxStamina);
    }
    public void SetFAttackMoveOn(bool b)
    {
        FAttackMoveOn = b;
    }
    public void SetPlayerState(PLAYERSTATE _state)
    {
        State = _state;
    }

    /// ////////////디버그용
    public PLAYERSTATE GetState()
    {
        return State;
    }
    
    public void SetOnEvent()
    {
        IsOnEvent = true;
        if(!StageManager.IsStageCleared)
        {
            Ani.SetTrigger("Idle");
            State = PLAYERSTATE.IDLE;
        }
    }

    public void SetOffEvent()
    {
        
        IsOnEvent = false;
    }

    IEnumerator PlayerDeathEvent()
    {
        yield return new WaitForSecondsRealtime(3.0f);

        Time.timeScale = 1.0f;
        UIMgrScript.Inst.OnGameOverWnd();
    }

}
