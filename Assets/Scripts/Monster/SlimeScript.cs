using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SLIMESTATE
{
    IDLE,
    CHASE,
    PREATTACK,
    SPIN,
    JUMP,
    TOORIGIN,
    DEATH
}

public class SlimeScript : MonRootScript
{

    public GameObject MonsterAttackObj;
    SLIMESTATE State = SLIMESTATE.IDLE;
    float Speed = 2;
    
    float IdleTime=3;
    Vector3 DestPos;
    Vector3 OriginPos;
    float OriginPosDist = 10;

    float MoveDist=0;

    float FAttackTime = 0.2f;
    float FAttackSpeed = 10;   
    bool IsFAttackOn = false;

    float SpinSpeed = 5;
    [HideInInspector] public float SpinTime = 1;


    [HideInInspector]public int JumpLevel = 0;
    int JumpSpeed = 5;
    float JumpXStart;
    float JumpXEnd;
    float JumpYStart;
    float JumpXDist = 5;
    float JumpHeightDelta = 0.5f;

    int PreAttackNum;
    float PreAttackMoveDist;

    //����
    public GameObject SpinSound;
    AudioSource SpinSoundSource;
   
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        PlayerDetectDist = 5;
        OriginPos = Pivot.transform.position;
        OriginPosDist = 10;
        HP = 10;
        SpinSoundSource = SpinSound.GetComponent<AudioSource>();
        SpinSoundSource.volume = GValue.GSoundVolume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        IdleUpdate();

        DetectPlayer();

        Chase();

        PreAttackUpdate();

        SpinUpdate();

        JumpUpdate();

        ToOriginUpdate();

        FAttackUpdate();

        StuckUpdate();

        if(SLIMESTATE.SPIN!=State)
            SpinSoundSource.Stop();

    }
    void IdleUpdate()
    {
        if (State != SLIMESTATE.IDLE)
            return;

        if(IdleTime>0)//���ڸ��� �ֱ�
        {
            IdleTime -= Time.deltaTime;
            if (IdleTime < 0)//���ڸ��� �ֱ� ����.//���� �̵� ��ġ ���ϱ�.
            {
                Ani.SetTrigger("Walk");
                Vector3 tempV = Pivot.transform.position;
                tempV.x += Random.Range(-4.0f, 3.0f);
                tempV.y += Random.Range(-3.0f, 2.0f);
                tempV.z = 0;
                DestPos = tempV;
                
                if(DestPos.x<Pivot.transform.position.x)//������ȯ
                {
                    Pivot.transform.eulerAngles = new Vector3(0, 180, 0);
                }
                else
                {
                    Pivot.transform.eulerAngles = new Vector3(0, 0, 0);
                }
                                
                MoveDist = (DestPos - Pivot.transform.position).magnitude;

            }
                
        }
        else//Idle �� �̵�.
        {
            
            Vector3 Dir = DestPos - Pivot.transform.position; ;
            Dir.Normalize();
            Pivot.transform.position+=(Dir * Time.deltaTime * Speed);
            MoveDist -= Time.deltaTime * Speed;
                       
            if (MoveDist<0)
            {                
                Ani.SetTrigger("Idle");
                IdleTime = 3;
            }
        } 
    }

    void DetectPlayer()//�÷��̾� ����. IDLE�� ����.
    {
        if (State != SLIMESTATE.IDLE)
            return;        
       
        float DetectDist = (PlayerPivot.transform.position - Pivot.transform.position).magnitude;
        if(DetectDist < PlayerDetectDist)
        {
            State = SLIMESTATE.CHASE;//�÷��̾� �߰� ����.
            Ani.SetTrigger("Walk");

        }
    }

    void Chase()//�÷��̾� �߰�.
    {
        if (State != SLIMESTATE.CHASE)
            return;

        
        DestPos = PlayerPivot.transform.position;//��ǥ���� �÷��̾� ��ġ.
       

        if (DestPos.x < Pivot.transform.position.x)//������ȯ
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

        if((DestPos- Pivot.transform.position).magnitude<2)//�÷��̾� �����̿� ���� ���� ����.
        {
            State = SLIMESTATE.PREATTACK;

            PreAttackNum = Random.Range(2, 5);

            Vector3 tempV = PlayerPivot.transform.position;

            tempV.x += Random.Range(-3, 3);
            tempV.y += Random.Range(-2, 2);
            DestPos = tempV;

            PreAttackMoveDist = (DestPos - Pivot.transform.position).magnitude;
            

        }
        else if((DestPos - Pivot.transform.position).magnitude > PlayerDetectDist*2)//�ʹ� �־����� ���ڸ��� ��.
        {
            State = SLIMESTATE.TOORIGIN;
        }
    }

    void PreAttackUpdate()//���� �� �÷��̾� ���� ���ƴٴϱ�
    {
        if (SLIMESTATE.PREATTACK != State)
            return;

        Vector3 Dir = DestPos - Pivot.transform.position;
        Dir.Normalize();

        if (PlayerPivot.transform.position.x < Pivot.transform.position.x)//������ȯ
        {
            Pivot.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            Pivot.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        Pivot.transform.position += (Dir * Time.deltaTime * Speed);
        PreAttackMoveDist -= Time.deltaTime * Speed;
        if(PreAttackMoveDist<0)
        {
            PreAttackNum--;
            if(PreAttackNum==0)//���ƴٴϱ� ������ ����
            {                          

                int RandomInt = Random.Range(0, 2);
                if (RandomInt == 1)
                {
                    State = SLIMESTATE.SPIN;
                    Ani.SetTrigger("Spin");
                    SpinSoundSource.Play();
                }
                else
                {
                    State = SLIMESTATE.JUMP;
                    Ani.SetTrigger("Jump");
                    JumpXStart = Pivot.transform.position.x;
                    if (Pivot.transform.right.x > 0)
                    {
                        JumpXEnd = Pivot.transform.position.x + JumpXDist;
                    }
                    else
                    {
                        JumpXEnd = Pivot.transform.position.x - JumpXDist;
                    }

                    JumpYStart = Pivot.transform.position.y;
                }
                return;
            }
            Vector3 tempV = PlayerPivot.transform.position;

            tempV.x += Random.Range(-3, 3);
            tempV.y += Random.Range(-2, 2);
            DestPos = tempV;
            PreAttackMoveDist = (DestPos - Pivot.transform.position).magnitude;
        }

    }

    void ToOriginUpdate()//�ʹ� �־����� ���� ��ġ�� ���ư��� �Լ�
    {
        if (SLIMESTATE.TOORIGIN != State)
            return;

        Vector3 Dir = OriginPos - Pivot.transform.position;
        Dir.Normalize();

        Pivot.transform.position += Dir * Time.deltaTime * Speed;
        OriginPosDist -= Time.deltaTime * Speed;
        if (OriginPos.x < Pivot.transform.position.x)//������ȯ
        {
            Pivot.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            Pivot.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if(OriginPosDist<0)
        {
            State = SLIMESTATE.IDLE;
            OriginPosDist = 10;
        }
    }

    void SpinUpdate()//�ҿ뵹�� ����
    {
        if (State != SLIMESTATE.SPIN)
            return;

        SpinTime -= Time.deltaTime;
        if(SpinTime<0)
        {
            Pivot.transform.position += -(Pivot.transform.right * Time.deltaTime * SpinSpeed);
        }
        else
        {
            Pivot.transform.position += (Pivot.transform.right * Time.deltaTime * SpinSpeed);
        }
        
    }

    void JumpUpdate()//���� ����
    {
        if (State != SLIMESTATE.JUMP)
            return;

        if(JumpLevel==0)
        {

        }
        else if(JumpLevel==1 || JumpLevel==2)
        {
            Vector3 tempv = Pivot.transform.position;
            float tempx = tempv.x;
            float tempy = tempv.y;
            Vector3 tempDir = Pivot.transform.right;
            tempDir.Normalize();
            tempv.x += Time.deltaTime * JumpSpeed* tempDir.x;
            tempv.y = JumpYStart - JumpHeightDelta*(tempv.x - JumpXStart) * (tempv.x - JumpXEnd);
            Pivot.transform.position = tempv;

            //Pivot.transform.position += (Pivot.transform.right * Time.deltaTime * Speed)+
            //                            (Pivot.transform.up * Time.deltaTime * JumpSpeed);
        }
        //else if(JumpLevel==2)
        //{
        //    Pivot.transform.position += (Pivot.transform.right * Time.deltaTime * Speed) -
        //                                (Pivot.transform.up * Time.deltaTime * JumpSpeed);
        //}

        
    }

    void FAttackUpdate()
    {
        if (!IsFAttackOn)
            return;
        if (State == SLIMESTATE.DEATH)
            return;

        FAttackTime -= Time.deltaTime;
        Vector3 tempV = Pivot.transform.position;

        if (PlayerPivot.transform.position.x < tempV.x)
        {
            tempV.x += FAttackSpeed * Time.deltaTime;
        }
        else
        {
            tempV.x += -FAttackSpeed * Time.deltaTime;
        }

        Pivot.transform.position = tempV;
        if (FAttackTime < 0)
        {
            FAttackTime = 0.2f;
            IsFAttackOn = false;
        }

    }

    void StuckUpdate()
    {
        //������ �� ���ư��� �Լ�
        if (!IsStuck)
            return;

        if (State != SLIMESTATE.CHASE)
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
    public void SetSlimeState(SLIMESTATE _state)
    {
        State = _state;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "PlayerAttack")
        {
            //ü�� ����
            HP--;
            if(HP<=0)
            {
                Ani.SetTrigger("Death");
                State = SLIMESTATE.DEATH;
                AClip = Resources.Load<AudioClip>("Sound/Monster/SlimeDeath");
                ASource.PlayOneShot(AClip);
                BoxCollider2D[] Colliders = GetComponentsInChildren<BoxCollider2D>();
                for (int i = 0; i < Colliders.Length; i++)
                {
                    Colliders[i].enabled = false;
                }
                if (StageManager)
                    StageManager.SubMonCount();
            }    

        }
        else if (collision.gameObject.tag == "FPlayerAttack")
        {
            HP -= 2;
           
            if (HP <= 0)
            {
                Ani.SetTrigger("Death");
                State = SLIMESTATE.DEATH;
                AClip = Resources.Load<AudioClip>("Sound/Monster/SlimeDeath");
                ASource.PlayOneShot(AClip);
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
