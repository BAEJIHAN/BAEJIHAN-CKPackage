using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DeathBringerScript : MonoBehaviour
{
    enum STATE
    {
        IDLE,
        ATTACK
    }
    // Start is called before the first frame update
    Animator Ani;
    float AttackCoolTime=3;
    float AttackCurCoolTime = 0;
    STATE State=STATE.IDLE;
    public GameObject DeathBringerAttackPref;
    GameObject AttackObject;
    private void Awake()
    {
        Ani = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(STATE.IDLE==State)
        {
            AttackCurCoolTime += Time.deltaTime;
            if(AttackCurCoolTime>AttackCoolTime)
            {
                State = STATE.ATTACK;
                Ani.SetTrigger("Attack");
                AttackCurCoolTime = 0;
            }
        }
    }

    public void DBAttackEnd()
    {
        State = STATE.IDLE;
        Ani.SetTrigger("Idle");

        Destroy(AttackObject);


    }

    public void DBAttackObjOn()
    {
        AttackObject = Instantiate(DeathBringerAttackPref);
        AttackObject.transform.SetParent(gameObject.transform);
        AttackObject.transform.localScale=new Vector3(0.6f, 0.6f, 1);
        AttackObject.transform.localPosition = new Vector3(-0.112f, -0.077f, 1);
    }

    public void DBAttackObjDestroy()
    {
        Destroy(AttackObject);
    }
}
