using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IMPSTATE
{ 

    IDLE,
    COMBAT,
    CHASE,
    WALK,
    ATTACK,
    HIT,
    DEATH

}
public class ImpScript : MonRootScript
{
    // Start is called before the first frame update
    IMPSTATE State = IMPSTATE.IDLE;
    Vector3 DestPos;
    float Speed = 3;
    int PreAttackNum;
    float DestPosDist;

    float FAttackSpeed = 8;
    float FAttackTime = 0.2f;
    bool IsFAttackOn = false;

    // Start is called before the first frame update
    void Start()
    {
       
        base.Start();
        PlayerDetectDist = 5;
        HP = 7;
    }

    // Update is called once per frame
    void Update()
    {
        
        DetectPlayer();

        CombatUpdate();

        ChaseUpdate();

        FAttackUpdate();

        StuckUpdate();
    }

    void DetectPlayer()
    {
        if (State != IMPSTATE.IDLE)
            return;

        if (PlayerPivot == null)
            return;


        float DetectDist = (PlayerPivot.transform.position - Pivot.transform.position).magnitude;
        if (DetectDist < PlayerDetectDist)
        {
            State = IMPSTATE.COMBAT;//�÷��̾� �߰� ����.
            Ani.SetTrigger("Walk");
            if (MonGroup)
                MonGroup.AwakeMons();

            SetDest();
            SetPreAttackNum();
        }
    }

    void CombatUpdate()//����. 
    {
        if (State != IMPSTATE.COMBAT)
            return;

        if ((PlayerPivot.transform.position - Pivot.transform.position).magnitude > 15)//�÷��̾ �ʹ� �־����� �Ѿư�
        {
            State = IMPSTATE.CHASE;
            return;
        }

        Vector3 tempDir = DestPos - Pivot.transform.position;
        tempDir.Normalize();

        if (PlayerPivot.transform.position.x > Pivot.transform.position.x)//������ȯ
        {
            Pivot.transform.rotation = Quaternion.Euler(0, 180, 0);

        }
        else
        {
            Pivot.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        Pivot.transform.position += tempDir * Speed * Time.deltaTime;
        DestPosDist -= Speed * Time.deltaTime;
        if (DestPosDist < 0)//��ȸ ���������� �� ����
        {
            PreAttackNum--;//��ȸ Ƚ�� ����
            if (PreAttackNum <= 0)//��ȸ Ƚ�� �� ä��� ������.
            {
                if(Mathf.Abs(PlayerPivot.transform.position.y-Pivot.transform.position.y)>2)
                {
                    State = IMPSTATE.CHASE;
                }
                else
                {
                    State = IMPSTATE.ATTACK;
                    Ani.SetTrigger("Attack");
                }
                    
                
               
            }

            //���� ��ġ ����

        }
    }

    void ChaseUpdate()//�÷��̾�� �־����� �� �Ѿƿ�
    {
        if (State != IMPSTATE.CHASE)
            return;

        Vector3 tempDir = PlayerPivot.transform.position - Pivot.transform.position;
        tempDir.Normalize();

        Pivot.transform.position += Time.deltaTime * Speed * tempDir;
        if ((PlayerPivot.transform.position - Pivot.transform.position).magnitude < 8)//�÷��̾� ���� 8 ������ ������ �ٽ� ����
        {
            SetPreAttackNum();
            SetDest();
            State = IMPSTATE.COMBAT;
        }

    }

    void FAttackUpdate()//������ �ǰݽ� �˹�
    {
        if (!IsFAttackOn)
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

        if (State != IMPSTATE.CHASE)
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

    public override void AwakeMon()
    {
        State = IMPSTATE.CHASE;
        Ani.SetTrigger("Walk");
    }
    public void SetImpState(IMPSTATE _State)
    {
        State = _State;
    }

    public void SetPreAttackNum()//���� �ϱ� ���� ��ȸ�� Ƚ�� ����
    {
        PreAttackNum = Random.Range(2, 5);
    }

    public void SetDest()//���� ��ǥ ���� ����
    {
        Vector3 tempV;
        tempV.x = Pivot.transform.position.x;
        tempV.y = PlayerPivot.transform.position.y;
        tempV.z = 0;
        tempV.x += Random.Range(-2.0f, 2.0f);
        tempV.y += Random.Range(-1.0f, 1.0f);

        DestPos = tempV;
        DestPosDist = (DestPos - Pivot.transform.position).magnitude;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (State == IMPSTATE.DEATH)
            return;

        if (collision.gameObject.tag == "PlayerAttack")
        {
            HP--;
            if(HP<=0)
            {
                Ani.SetTrigger("Death");
                State = IMPSTATE.DEATH;
                AClip = Resources.Load<AudioClip>("Sound/Monster/ImpDeath");
                ASource.PlayOneShot(AClip);
                GetComponent<BoxCollider2D>().enabled = false;
                if (StageManager)
                    StageManager.SubMonCount();
            }
            else
            {

                Ani.SetTrigger("Hit");
                State = IMPSTATE.HIT;
            }          
            
        }
        else if (collision.gameObject.tag == "FPlayerAttack")
        {
            HP -= 2;
            if (HP <= 0)
            {
                Ani.SetTrigger("Death");
                State = IMPSTATE.DEATH;
                AClip = Resources.Load<AudioClip>("Sound/Monster/ImpDeath");
                ASource.PlayOneShot(AClip);
                GetComponent<BoxCollider2D>().enabled = false;
                if (StageManager)
                    StageManager.SubMonCount();
            }
            else
            {
                Ani.SetTrigger("Hit");
                State = IMPSTATE.HIT;
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
