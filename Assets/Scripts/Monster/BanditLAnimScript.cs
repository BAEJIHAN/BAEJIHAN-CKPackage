using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditLAnimScript : MonoBehaviour
{
    public GameObject AttackObj;
    GameObject PlayerPivot;
    BanditLScript Pivot;
    Animator Ani;

    private void Awake()
    {
        Pivot = transform.parent.gameObject.GetComponent<BanditLScript>();
        Ani = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerPivot = GameObject.Find("PlayerPivot");

       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttackSpawn()
    {
        GameObject tempAttack = Instantiate(AttackObj);
        tempAttack.GetComponent<MonsterAttackScript>().Damage = Pivot.Damage;
        Vector3 tempV = Pivot.gameObject.transform.position;
        tempV.x += -Pivot.gameObject.transform.right.x * 0.37f;
        tempV.y += 0.6f;
        tempAttack.transform.position = tempV;
        tempAttack.transform.localScale = new Vector3(1.52f, 1.22f, 1);
        Pivot.AClip = Resources.Load<AudioClip>("Sound/Monster/BanditSword");
        Pivot.ASource.PlayOneShot(Pivot.AClip);
        Destroy(tempAttack.gameObject, 0.3f);

    }

    public void AttackEndFunc()
    {
        Vector3 DestPos = PlayerPivot.transform.position;
        DestPos -= new Vector3(0, 0.5f, 0);
        if ((DestPos - Pivot.transform.position).magnitude < Pivot.AttackRange)//플레이어와 가까우면 다시 공격
        {
            Ani.SetTrigger("CombatIdle");
            Pivot.SetBanditLState(BANDITLSTATE.COMBAT);
            Pivot.CombatIdleTime = Random.Range(0.8f, 2.0f);
            
        }
        else
        {
            Ani.SetTrigger("Run");
            Pivot.SetBanditLState(BANDITLSTATE.CHASE);

        }
    }

    public void HurtEndFunc()
    {
        Vector3 DestPos = PlayerPivot.transform.position;
        DestPos -= new Vector3(0, 0.5f, 0);
        if ((DestPos - Pivot.transform.position).magnitude < Pivot.AttackRange)//플레이어와 가까우면 다시 공격
        {
            Ani.SetTrigger("CombatIdle");
            Pivot.SetBanditLState(BANDITLSTATE.COMBAT);
            Pivot.CombatIdleTime = Random.Range(0.5f, 1.5f);
            
        }
        else
        {
            Ani.SetTrigger("Run");
            Pivot.SetBanditLState(BANDITLSTATE.CHASE);

        }
    }

    public void DeathEndFunc()
    {
        Pivot.IsFadeOn = true;
    }
}
