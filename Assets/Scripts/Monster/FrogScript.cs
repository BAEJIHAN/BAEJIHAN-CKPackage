using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FROGSTATE
{
    IDLE,
    BLINK,
    JUMP,
    HIT,
    ATTACK,
    DEATH
}
public class FrogScript : MonRootScript
{
    FROGSTATE State = FROGSTATE.IDLE;

    float Speed = 3;
    float fvalue = 0;
    Vector3 P1, P2, P3, P4;

    float IdleTime = 1.5f;
    float CurIdleTime=0;

    int JumpLevel = 0;
    float JumpDist = 4;
    float TotalJumpDist = 0;

    LayerMask FLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();


        HP = 10;

        FLayerMask = 1 << LayerMask.NameToLayer("Obstacle");  
        FLayerMask |= 1 << LayerMask.NameToLayer("Border");  
    }

    // Update is called once per frame
    void Update()
    {
        IdleUpdate();

       

        base.DeathBip();
        
    }

    private void FixedUpdate()
    {
        JumpUpdate();
    }

  

    void IdleUpdate()
    {
        if(FROGSTATE.IDLE!=State)
        {
            return;
        }

        CurIdleTime += Time.deltaTime;
        if(CurIdleTime>IdleTime)
        {
            CurIdleTime = 0;
            TakeNextAction();
        }
        

    }
       
    void JumpUpdate()
    {
        if (FROGSTATE.JUMP != State)
        {
            return;
        }

        float temp = Time.deltaTime / TotalJumpDist;

        fvalue += temp * Speed;

        Pivot.transform.position = MMath.Lerp(P1, P2, P3, P4, fvalue);

        if (fvalue >= 0.5f && JumpLevel==0)
        {
            JumpLevel++;
            Ani.SetTrigger("JumpDown");
        }

        if(fvalue >= 0.9f && JumpLevel == 1)
        {
            JumpLevel++;
            Ani.SetTrigger("JumpLand");            
        }

        if (fvalue > 1 )
        {
            JumpLevel=0;

            TakeNextAction();
            fvalue = 0;
        }
    }

  
    void InitJump()
    {
        fvalue = 0;
        P1 = Pivot.transform.position;
        float Ranx = Random.Range(-1.0f, 1.0f);
        float Rany = Random.Range(-1.0f, 1.0f);

        int RanNum = Random.Range(0, 2);
        if (RanNum == 0)//방향 결정
        {
            P4 = P1 + new Vector3(Ranx+JumpDist, Rany, 0);
            Pivot.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            P4 = P1 + new Vector3(Ranx - JumpDist, Rany, 0);
            Pivot.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        Vector3 RayDir = P4 - P1;
        RayDir.Normalize();
        
        RaycastHit2D hit = Physics2D.Raycast(P1, RayDir, (P4 - P1).magnitude, FLayerMask);
       
       
        if (hit)
        {
            P4.x = hit.point.x;
            P4.y = hit.point.y;

        }



        float MaxHeight = Mathf.Abs(P1.x - P4.x) * 0.5f;
        P2 = Vector3.zero;
        P3 = Vector3.zero;
        P2.y = P1.y + MaxHeight;
        P3.y = P4.y + MaxHeight;
        P2.x = ((P1.x + P4.x) / 2 + P1.x) / 2;
        P3.x = ((P1.x + P4.x) / 2 + P4.x) / 2;

        
        TotalJumpDist = Mathf.Abs(P1.x - P4.x);

    }

    public void SetFrogState(FROGSTATE _state)
    {
        State = _state;
    }

   

    public void TakeNextAction()
    {
        
        if((PlayerPivot.transform.position-Pivot.transform.position).magnitude<2.0f
            && Mathf.Abs(PlayerPivot.transform.position.y - Pivot.transform.position.y)<0.8f)//가까이 있으면 3분의 2 확률로 공격
        {
            
            if(Random.Range(0,3)>=1)
            {
                if (PlayerPivot.transform.position.x > Pivot.transform.position.x)//방향전환
                {
                    Pivot.transform.rotation = Quaternion.Euler(0, 180, 0);

                }
                else
                {
                    Pivot.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                State = FROGSTATE.ATTACK;
                Ani.SetTrigger("Attack");
                AClip = Resources.Load<AudioClip>("Sound/Monster/FrogAttack");
                ASource.PlayOneShot(AClip);
                return;
            }
        }

        int RanNum = Random.Range(0, 10);

        
        if (0<= RanNum && RanNum<=3)//Idle
        {
            State = FROGSTATE.IDLE;
            Ani.SetTrigger("Idle");
        }
        else if(4 <= RanNum && RanNum <= 7)//Blink
        {
            State = FROGSTATE.BLINK;
            Ani.SetTrigger("Blink");
        }
        else if (8 <= RanNum && RanNum <= 9)//Jump
        {
            State = FROGSTATE.JUMP;
            Ani.SetTrigger("JumpUp");
            if((PlayerPivot.transform.position-Pivot.transform.position).magnitude<15)
            {
                AClip = Resources.Load<AudioClip>("Sound/Monster/FrogJump");
                ASource.PlayOneShot(AClip);
            }
            
            InitJump();
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (State == FROGSTATE.DEATH)
            return;

        if (collision.gameObject.tag == "PlayerAttack")
        {
            HP--;
            

            if(HP<=0)
            {
                AClip = Resources.Load<AudioClip>("Sound/Monster/FrogDeath");
                ASource.PlayOneShot(AClip);
                Ani.SetTrigger("Death");
                State = FROGSTATE.DEATH;
                GetComponent<BoxCollider2D>().enabled = false;
                if (StageManager)
                    StageManager.SubMonCount();
                IsFadeOn = true;
            }
            else
            {
                Ani.SetTrigger("Hit");
                State = FROGSTATE.HIT;
            }

           
        }
        else if (collision.gameObject.tag == "FPlayerAttack")
        {
            HP-=2;
            if (HP <= 0)
            {
                AClip = Resources.Load<AudioClip>("Sound/Monster/FrogDeath");
                ASource.PlayOneShot(AClip);
                Ani.SetTrigger("Death");
                State = FROGSTATE.DEATH;
                GetComponent<BoxCollider2D>().enabled = false;
                if (StageManager)
                    StageManager.SubMonCount();
                IsFadeOn = true;
            }
            else
            {
                Ani.SetTrigger("Hit");
                State = FROGSTATE.HIT;
            }
            

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "Border")
        {
            JumpLevel = 0;

            TakeNextAction();
            fvalue = 0;
        }
    }

    public void LandEndFunc()
    {
        JumpLevel = 0;

        TakeNextAction();
        fvalue = 0;
    }

}
