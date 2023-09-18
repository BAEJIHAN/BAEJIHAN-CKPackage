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
            State = IMPSTATE.COMBAT;//플레이어 추격 시작.
            Ani.SetTrigger("Walk");
            if (MonGroup)
                MonGroup.AwakeMons();

            SetDest();
            SetPreAttackNum();
        }
    }

    void CombatUpdate()//전투. 
    {
        if (State != IMPSTATE.COMBAT)
            return;

        if ((PlayerPivot.transform.position - Pivot.transform.position).magnitude > 15)//플레이어가 너무 멀어지면 쫓아감
        {
            State = IMPSTATE.CHASE;
            return;
        }

        Vector3 tempDir = DestPos - Pivot.transform.position;
        tempDir.Normalize();

        if (PlayerPivot.transform.position.x > Pivot.transform.position.x)//방향전환
        {
            Pivot.transform.rotation = Quaternion.Euler(0, 180, 0);

        }
        else
        {
            Pivot.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        Pivot.transform.position += tempDir * Speed * Time.deltaTime;
        DestPosDist -= Speed * Time.deltaTime;
        if (DestPosDist < 0)//배회 목적지까지 다 가면
        {
            PreAttackNum--;//배회 횟수 감소
            if (PreAttackNum <= 0)//배회 횟수 다 채우면 공격함.
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

            //다음 위치 선정

        }
    }

    void ChaseUpdate()//플레이어와 멀어졌을 때 쫓아옴
    {
        if (State != IMPSTATE.CHASE)
            return;

        Vector3 tempDir = PlayerPivot.transform.position - Pivot.transform.position;
        tempDir.Normalize();

        Pivot.transform.position += Time.deltaTime * Speed * tempDir;
        if ((PlayerPivot.transform.position - Pivot.transform.position).magnitude < 8)//플레이어 범위 8 안으로 들어오면 다시 공격
        {
            SetPreAttackNum();
            SetDest();
            State = IMPSTATE.COMBAT;
        }

    }

    void FAttackUpdate()//강공격 피격시 넉백
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
        //끼었을 때 돌아가는 함수
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

    public void SetPreAttackNum()//공격 하기 전에 배회할 횟수 설정
    {
        PreAttackNum = Random.Range(2, 5);
    }

    public void SetDest()//다음 목표 지점 설정
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
