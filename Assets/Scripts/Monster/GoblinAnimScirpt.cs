using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAnimScirpt : MonoBehaviour
{
    public GameObject Attack1Pos;
    public GameObject Attack2Pos;
    public GameObject AttackObj;

    GoblinScript Pivot;
    Animator Ani;
    // Start is called before the first frame update
    void Start()
    {
        Pivot = transform.parent.gameObject.GetComponent<GoblinScript>();
        Ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack1SpawnFunc()
    {
        GameObject tempAttack = Instantiate(AttackObj, Attack1Pos.transform);
        tempAttack.GetComponent<MonsterAttackScript>().Damage = Pivot.Damage;
        tempAttack.GetComponent<BoxCollider2D>().size = new Vector2(0.81f, 0.41f);
        Destroy(tempAttack, 0.1f);
        
    }

    public void Attack1EndFunc()
    {
        Pivot.SetGoblinState(GOBLINSTATE.CHASE);
        Ani.SetTrigger("Run");
    }

    public void Attack2StartFunc()
    {
        Pivot.AddAttack2Num();
        Pivot.SetPs();
    }
    public void Attack2Func()
    {
        Pivot.AddAttack2Num();
    }

  
    public void Attack2SpawnFunc()
    {
        GameObject tempAttack = Instantiate(AttackObj, Attack2Pos.transform);
        tempAttack.GetComponent<MonsterAttackScript>().Damage = Pivot.Damage;
        tempAttack.GetComponent<BoxCollider2D>().size = new Vector2(0.4f, 0.26f);
        Destroy(tempAttack, 0.25f);
        
    }

    public void Attack2EndFunc()
    {
        Pivot.ZeroAttack2Num();
        Pivot.SetGoblinState(GOBLINSTATE.CHASE);
        Ani.SetTrigger("Run");
    }

    public void HitEnd()
    {
        Pivot.ZeroAttack2Num();
        Pivot.SetGoblinState(GOBLINSTATE.CHASE);
        Ani.SetTrigger("Run");
    }

    public void RiseEnd()
    {
        Pivot.SetGoblinState(GOBLINSTATE.CHASE);
        Ani.SetTrigger("Run");
    }
}
