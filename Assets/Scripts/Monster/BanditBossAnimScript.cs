using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditBossAnimScript : MonoBehaviour
{
    public GameObject Attack1Pos;
    public GameObject Attack2Pos;
    public GameObject Attack3Pos;    
    public GameObject AttackObj;

    public GameObject CrackPos;
    public GameObject CrackObj;
    GameObject PlayerPivot;
    BanditBossScript Pivot;
    Animator Ani;
    // Start is called before the first frame update
    private void Awake()
    {
        Pivot = transform.parent.gameObject.GetComponent<BanditBossScript>();
        Ani = GetComponent<Animator>();
    }
    void Start()
    {
        
        PlayerPivot = GameObject.Find("PlayerPivot");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack1SpawnFunc()
    {
        GameObject tempAttack = Instantiate(AttackObj, Attack1Pos.transform);
        tempAttack.GetComponent<MonsterAttackScript>().Damage = Pivot.Damage;
        tempAttack.GetComponent<BoxCollider2D>().size = new Vector2(0.44f, 0.36f);
        Pivot.AClip = Resources.Load<AudioClip>("Sound/Monster/BossSword");
        Pivot.ASource.PlayOneShot(Pivot.AClip);
        Destroy(tempAttack, 0.15f);
        //tempAttack.transform.position= Attack1Pos.transform.position;
    }

    public void Attack2SpawnFunc()
    {
        GameObject tempAttack = Instantiate(AttackObj, Attack2Pos.transform);
        tempAttack.GetComponent<MonsterAttackScript>().Damage = Pivot.Damage;
        tempAttack.GetComponent<BoxCollider2D>().size = new Vector2(0.59f, 0.59f);
        Pivot.AClip = Resources.Load<AudioClip>("Sound/Monster/BossSword");
        Pivot.ASource.PlayOneShot(Pivot.AClip);
        Destroy(tempAttack, 0.15f);
        //tempAttack.transform.position = Attack2Pos.transform.position;
    }

    public void Attack3SpawnFunc()
    {
        GameObject tempAttack = Instantiate(AttackObj, Attack3Pos.transform);
        tempAttack.GetComponent<MonsterAttackScript>().Damage = Pivot.Damage;
        tempAttack.GetComponent<BoxCollider2D>().size = new Vector2(0.68f, 0.06f);
        Pivot.AClip = Resources.Load<AudioClip>("Sound/Monster/BossSpear");
        Pivot.ASource.PlayOneShot(Pivot.AClip);
        Destroy(tempAttack, 0.15f);
        
    }

    public void Attack4SpawnFunc()
    {
       
        GameObject tempCrack = Instantiate(CrackObj);
        tempCrack.transform.position = CrackPos.transform.position;

        Pivot.AClip = Resources.Load<AudioClip>("Sound/Monster/Stomp");
        Pivot.ASource.PlayOneShot(Pivot.AClip);

        if (Vector3.Distance(PlayerPivot.transform.position, CrackPos.transform.position) > 15)
            return;

       
        PlayerPivot.GetComponent<PlayerScript>().SetStomp();
       
    }

    public void Attack1EndFunc()
    {
        if (Random.Range(0, 6) > 1)
        {
            Ani.SetTrigger("Run");
            Pivot.SetBanditBossState(BANDITBOSSSTATE.CHASE);
        }
        else
        {
            Pivot.BackStepStartFunc();
        }

    }

    public void Attack2EndFunc()
    {
        if (Random.Range(0, 6) > 1)
        {
            Ani.SetTrigger("Run");
            Pivot.SetBanditBossState(BANDITBOSSSTATE.CHASE);
        }
        else
        {
            Pivot.BackStepStartFunc();
        }
    }

    public void Attack3EndFunc()
    {
        Pivot.Attack3LevelZero();
        if (Random.Range(0, 6) > 1)
        {
            Ani.SetTrigger("Run");
            Pivot.SetBanditBossState(BANDITBOSSSTATE.CHASE);
        }
        else
        {
            Pivot.BackStepStartFunc();
        }
    }

    public void Attack4EndFunc()
    {
        if (Random.Range(0, 6) > 1)
        {
            Ani.SetTrigger("Run");
            Pivot.SetBanditBossState(BANDITBOSSSTATE.CHASE);
        }
        else
        {
            Pivot.BackStepStartFunc();
        }
    }

    public void Combo1Func()
    {
        Pivot.Combo1LevelAdd();
    }

    public void Combo1EndFunc()
    {
        Pivot.Combo1LevelZero();
        Ani.SetTrigger("Run");
        Pivot.SetBanditBossState(BANDITBOSSSTATE.CHASE);
    }

    public void Combo2Func()
    {
        Pivot.Combo2LevelAdd();
    }

    public void Combo2EndFunc()
    {
        Pivot.Combo2LevelZero();
        Ani.SetTrigger("Run");
        Pivot.SetBanditBossState(BANDITBOSSSTATE.CHASE);
    }

    public void Attack3Func()
    {
        Pivot.Attack3LevelAdd();
    }

    public void BackStepEndFunc()
    {
        Ani.SetTrigger("Run");
        Pivot.SetBanditBossState(BANDITBOSSSTATE.CHASE);
    }

  
}
