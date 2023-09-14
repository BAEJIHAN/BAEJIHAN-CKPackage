using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MODELSTATE
{
    PREIDLE,
    IDLE,
    RUN,  
    SLIDING,
    ATTACK,
    COMBOATTACK,
    JUMP1,
    JUMP2,
    JUMP3,
    HEALING

}

public enum MANUALTYPE
{
    NONE,
    MOVE,
    JUMP,
    SLIDING,
    ATTACK,
    COMBOATTACK,
    HEALING
}
public class ManualModelScript : MonoBehaviour
{
    Animator Ani;
    MODELSTATE State = MODELSTATE.IDLE;
    MANUALTYPE mtype=MANUALTYPE.NONE;


    bool OnPreIdle = false;
    float PreIdleTime=1.5f;
    float CurPreIdleTime = 0.0f;

    //이동 관련
    float Speed = 5;   
    float IdleTime = 1.5f;
    float RunTime = 1.2f;
    float CurIdleTime = 0;
    float CurRunTime = 0;
    Vector3 OriginPos;

    int RunDirNum = 0;
    Vector3 RunDir;
    
    //점프 관련      
    float JumpMaxHeight = 2.0f;
    float JumpCurHeight = 0;
    float JumpSpeed = 7.5f;

    //슬라이딩 관련
    float CurSlidngTime = 0;
    float SlidingTime = 0.5f;
    float SlidingSpeed = 8;

    //기본공격 관련
    float CurAttackTime = 0;
    float AttackTime = 0.6f;

    //콤보공격 관련
    float CurComboAttackTime = 0;
    float ComboAttack2Time = 0.8f;
    float ComboAttack3Time = 1.5f;
    bool IsFirst = false;

    private void Awake()
    {
        Ani = GetComponentInChildren<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        

        RunDir = transform.right;
        OriginPos = transform.position;
      
    }

    // Update is called once per frame
    void Update()
    {
        if (mtype == MANUALTYPE.NONE)
            return;

        IdleUpdate();

        RunUpdate();

        JumpUpdate();

        SlidingUpdate();

        AttackUpdate();

        ComboAttackUpdate();

        HealingUpdate();
    }

    void ToIdle()
    {
        Ani.SetTrigger("Idle");
        State = MODELSTATE.IDLE;
     
    }
    /////////////////////////////////////////// 이동 시연    
    void ToRun()
    {
       

        
        State = MODELSTATE.RUN;


        if(RunDirNum==0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            RunDir = transform.right;
            RunTime = 1.2f;
        }
        else if(RunDirNum==1)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            RunDir = transform.right;
            RunTime = 1.2f;
        }
        else if (RunDirNum == 2)
        {
            RunDir = transform.up;
            RunTime = 0.8f;
        }
        else if (RunDirNum == 3)
        {
            RunDir = -transform.up;
            RunTime = 0.8f;
        }
        else if (RunDirNum == 4)
        {
            RunReset();
            return;
        }

        Ani.SetTrigger("Run");
        RunDirNum++;
        CurRunTime = 0;

    }

    void RunUpdate()
    {
        if(MODELSTATE.RUN != State)
        {
            return;
        }
        
        CurRunTime += Time.unscaledDeltaTime;

        transform.position += RunDir * Time.unscaledDeltaTime * Speed;

        if(CurRunTime>RunTime)
        {
            CurRunTime = 0;
            ToIdle();
        }
    }
    void IdleUpdate()
    {
        if (MODELSTATE.IDLE != State)
        {
            return;
        }

        CurIdleTime += Time.unscaledDeltaTime;

        if (CurIdleTime > IdleTime)
        {
            CurIdleTime = 0;

            if(mtype==MANUALTYPE.MOVE)
            {
                ToRun();
            }
            else if (mtype == MANUALTYPE.JUMP)
            {
                ToJump();
            }
            else if (mtype == MANUALTYPE.SLIDING)
            {
                ToSliding();
            }
            else if (mtype == MANUALTYPE.ATTACK)
            {
                ToAttack();
            }
            else if (mtype == MANUALTYPE.COMBOATTACK)
            {
                ToComboAttack();
            }
            else if (mtype == MANUALTYPE.HEALING)
            {
                ToHealing();
            }

        }
            
    }
    
    void RunReset()
    {
        
        RunDir = transform.right;
        CurRunTime = 0;
        CurIdleTime = 0;
        RunDirNum = 0;
        transform.position = OriginPos;
        transform.eulerAngles = new Vector3(0, 0, 0);
        ToIdle();
    }

    void ResetManual()
    {
        ToIdle();
        CurIdleTime = 0;
        CurRunTime = 0;
        transform.position = OriginPos;
        transform.eulerAngles = new Vector3(0, 0, 0);
        RunDir = transform.right;
        RunDirNum = 0;
        JumpCurHeight = 0;   
    }
    ////////////////////////////////점프 시연
    void ToJump()
    {
        State = MODELSTATE.JUMP1;
        Ani.SetTrigger("Jump1");
    }

    void JumpUpdate()
    {
        if (MODELSTATE.JUMP1 != State && MODELSTATE.JUMP2 != State && MODELSTATE.JUMP3 != State)
            return;

        if(MODELSTATE.JUMP1==State)
        {
            
            JumpCurHeight += JumpSpeed * Time.unscaledDeltaTime;
            transform.position += new Vector3(0, JumpSpeed * Time.unscaledDeltaTime, 0);
            if(JumpCurHeight>JumpMaxHeight)
            {
                State = MODELSTATE.JUMP2;
                Ani.SetTrigger("Jump2");
            }
            
        }

        if (MODELSTATE.JUMP3 == State)
        {
            JumpMaxHeight = 2.0f;
            JumpCurHeight -= JumpSpeed * Time.unscaledDeltaTime;
            transform.position += new Vector3(0, -JumpSpeed * Time.unscaledDeltaTime, 0);
            if (JumpCurHeight < 0)
            {
                ToIdle();
            }

        }

    }


    //////////////////////슬라이딩 시연
    void ToSliding()
    {
        State = MODELSTATE.SLIDING;
        
        transform.position = OriginPos;
        OnPreIdle = true;
    }

    void SlidingUpdate()
    {
        if (MODELSTATE.SLIDING != State)
            return;

        if (OnPreIdle)
        {
            CurPreIdleTime += Time.unscaledDeltaTime;
            if (CurPreIdleTime > PreIdleTime)
            {
                OnPreIdle = false;
                CurPreIdleTime = 0;
                Ani.SetTrigger("Sliding");
            }
            return;
        }

        CurSlidngTime += Time.unscaledDeltaTime;
        transform.position += transform.right * Time.unscaledDeltaTime * SlidingSpeed;
        if (CurSlidngTime > SlidingTime)
        {
            CurSlidngTime = 0;
            ToIdle();
        }
            


    }

    //공격 시연
    void ToAttack()
    {
        State = MODELSTATE.ATTACK;

        transform.position = OriginPos;
        OnPreIdle = true;
    }

    void AttackUpdate()
    {
        if (MODELSTATE.ATTACK != State)
            return;

        if (OnPreIdle)
        {
            CurPreIdleTime += Time.unscaledDeltaTime;
            if (CurPreIdleTime > PreIdleTime)
            {
                OnPreIdle = false;
                CurPreIdleTime = 0;
                Ani.SetTrigger("Attack1");
            }
            return;
        }        
        
        CurAttackTime += Time.unscaledDeltaTime;
        
        if (CurAttackTime > AttackTime)
        {
            CurAttackTime = 0;
            ToIdle();
        }
        
    }


    //콤보 공격 시연
    void ToComboAttack()
    {
        State = MODELSTATE.COMBOATTACK;

        transform.position = OriginPos;
        OnPreIdle = true;
        IsFirst = !IsFirst;
    }

    void ComboAttackUpdate()
    {
        if (MODELSTATE.COMBOATTACK != State)
            return;

        if (OnPreIdle)
        {
            CurPreIdleTime += Time.unscaledDeltaTime;
            if (CurPreIdleTime > PreIdleTime)
            {
                OnPreIdle = false;
                CurPreIdleTime = 0;
                if(IsFirst)
                {
                    Ani.SetTrigger("Attack2");
                }
                else
                {
                    Ani.SetTrigger("Attack3");
                }
                
            }
            return;
        }

        CurComboAttackTime += Time.unscaledDeltaTime;

        if (IsFirst)
        {
            if (CurComboAttackTime > ComboAttack2Time)
            {
                CurComboAttackTime = 0;
                ToIdle();
            }
        }
        else
        {
            if (CurComboAttackTime > ComboAttack3Time)
            {
                CurComboAttackTime = 0;
                ToIdle();
            }
        }
        

        
    }

    void ToHealing()
    {
        State = MODELSTATE.HEALING;

        transform.position = OriginPos;
        OnPreIdle = true;
        if(UIMgrScript.Inst)
        {
            UIMgrScript.Inst.SpawnModelCat();
        }
        else
        {
            GameObject.Find("TitleMgr").GetComponent<TitleMgrScript>().SpawnModelCat();
        }
        
    }
    void HealingUpdate()
    {
        if (MODELSTATE.HEALING != State)
            return;

        if (OnPreIdle)
        {
            CurPreIdleTime += Time.unscaledDeltaTime;
            if (CurPreIdleTime > PreIdleTime)
            {
                OnPreIdle = false;
                CurPreIdleTime = 0;
                Ani.SetTrigger("Sliding");
            }
            return;
        }

        CurSlidngTime += Time.unscaledDeltaTime;
        transform.position += transform.right * Time.unscaledDeltaTime * SlidingSpeed;
        if (CurSlidngTime > SlidingTime)
        {
            CurSlidngTime = 0;
            ToIdle();
        }
    }
    public void SetModelState(MODELSTATE state)
    {
        State = state;
    }

    public void SetManualType(MANUALTYPE type)
    {
        mtype = type;
        ResetManual();
    }

    public void ExitManual()
    {
        ToIdle();
        mtype = MANUALTYPE.NONE;
        CurIdleTime = 0;
        CurRunTime = 0;
        CurPreIdleTime = 0.0f;
        CurSlidngTime = 0;
        CurAttackTime = 0;
        CurComboAttackTime = 0;

        transform.position = OriginPos;
        transform.eulerAngles = new Vector3(0, 0, 0);
        RunDir = transform.right;
        RunDirNum = 0;
        JumpCurHeight = 0;

    }
}
