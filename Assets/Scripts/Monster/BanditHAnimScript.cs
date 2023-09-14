using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditHAnimScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject AttackObj;
    GameObject PlayerPivot;
    BanditHScript Pivot;
    Animator Ani;

    private void Awake()
    {
        Pivot = transform.parent.gameObject.GetComponent<BanditHScript>();
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
        tempV += new Vector3(-Pivot.gameObject.transform.right.x * 0.37f, 0.4f);
        tempAttack.transform.position = tempV;
        tempAttack.transform.localScale = new Vector3(1.52f, 1.22f, 1);
        Pivot.AClip = Resources.Load<AudioClip>("Sound/Monster/BanditSword");
        Pivot.ASource.PlayOneShot(Pivot.AClip);
        Destroy(tempAttack.gameObject, 0.3f);

    }

    public void AttackEndFunc()
    {
        if ((PlayerPivot.transform.position - Pivot.transform.position).magnitude < 1.2f)//플레이어와 가까우면 다시 공격
        {
            Ani.SetTrigger("CombatIdle");
            Pivot.SetBanditHState(BANDITHSTATE.COMBAT);
            Pivot.CombatIdleTime = Random.Range(0.5f, 1.0f);
        }
        else
        {
            Ani.SetTrigger("Run");
            Pivot.SetBanditHState(BANDITHSTATE.CHASE);

        }
    }

    public void HurtEndFunc()
    {
        if ((PlayerPivot.transform.position - Pivot.transform.position).magnitude < 1.2f)//플레이어와 가까우면 다시 공격
        {
            Ani.SetTrigger("CombatIdle");
            Pivot.SetBanditHState(BANDITHSTATE.COMBAT);
            Pivot.CombatIdleTime = Random.Range(0.5f, 1.0f);
        }
        else
        {
            Ani.SetTrigger("Run");
            Pivot.SetBanditHState(BANDITHSTATE.CHASE);

        }
    }

    public void RecoverEndFunc()
    {

        Pivot.GetComponent<BoxCollider2D>().enabled = true;
        if ((PlayerPivot.transform.position - Pivot.transform.position).magnitude < 1.2f)//플레이어와 가까우면 다시 공격
        {
            Ani.SetTrigger("CombatIdle");
            Pivot.SetBanditHState(BANDITHSTATE.COMBAT);
            Pivot.CombatIdleTime = Random.Range(1.5f, 2.5f);
        }
        else
        {
            Ani.SetTrigger("Run");
            Pivot.SetBanditHState(BANDITHSTATE.CHASE);

        }
    }
}
