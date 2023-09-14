using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimScript : MonoBehaviour
{
    public GameObject AttackObj;
    public GameObject FAttackObj;

    PlayerScript Player;
    Animator Ani;
    // Start is called before the first frame update
    private void Awake()
    {
        Player = transform.parent.gameObject.GetComponent<PlayerScript>();
        Ani = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToIdle()
    {

        Ani.SetTrigger("Idle");
        Player.SetPlayerState(PLAYERSTATE.IDLE);

    }

    public void Attack2Ready()
    {
        Player.IsAttack2Ready = true;
    }

    public void Attack1End()
    {
        Player.IsAttack2Ready = false;
        Player.Attack2Check = true;
        ToIdle();
    }

    public void Attack2End()
    {
        ToIdle();
    }

    public void Attack3End()
    {
        ToIdle();
    }

    public void Jump1Start()
    {

    }
    public void Jump2Start()
    {

    }
    public void Jump3Start()
    {

    }

    public void Jump1End()
    {
        Ani.SetTrigger("Jump2");
    }
    public void Jump2End()
    {
        Ani.SetTrigger("Jump3");
        Player.SetPlayerState(PLAYERSTATE.JUMP3);
    }
    public void Jump3End()
    {
        Ani.SetTrigger("Idle");
        Player.SetPlayerState(PLAYERSTATE.IDLE);
       
    }

    public void SlideEnd()
    {
        Ani.SetTrigger("Idle");
        Player.SetPlayerState(PLAYERSTATE.IDLE);
        
    }

    public void HurtStart()
    {
        Player.SetFAttackMoveOn(false);
    }
    public void HurtEnd()
    {
        Ani.SetTrigger("Idle");
        Player.SetPlayerState(PLAYERSTATE.IDLE);
        
    }
    public void AirHurt1End()
    {
        Ani.SetTrigger("AirHurt2");
        Player.SetPlayerState(PLAYERSTATE.AIRHURT2);
       
    }

    public void AirHurt3End()
    {
        Ani.SetTrigger("Rise");

    }

    public void RiseEnd()
    {
        Ani.SetTrigger("Idle");
        Player.SetPlayerState(PLAYERSTATE.IDLE);
        
    }

    public void AttackSpawn()
    {
        GameObject tempObj = Instantiate(AttackObj);
        Vector3 tempV = Player.gameObject.transform.position;
        if (DIR.Right == Player.Dir)
        {
            tempV += new Vector3(0.1f, -0.3f, 0);
        }
        else
        {
            tempV += new Vector3(-0.9f, -0.3f, 0);
        }

        tempObj.transform.position = tempV;
        Destroy(tempObj, 0.1f);
    }

    public void FAttackSpawn()
    {
        GameObject tempObj = Instantiate(FAttackObj);
        Vector3 tempV = Player.gameObject.transform.position;

        if (DIR.Right == Player.Dir)
        {
            tempV += new Vector3(3.8f, -0.3f, 0);
        }
        else
        {
            tempV += new Vector3(2.6f, -0.3f, 0);
        }

        tempObj.transform.position = tempV;
        Destroy(tempObj, 0.1f);

        Player.SetFAttackMoveOn(true);
    }

   

    public void FAttackMoveEnd()
    {
        Player.SetFAttackMoveOn(false);
    }
}
