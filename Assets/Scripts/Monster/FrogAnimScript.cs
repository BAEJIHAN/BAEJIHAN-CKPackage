using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAnimScript : MonoBehaviour
{
    public GameObject MonsterAttackObj;
    public GameObject AttackPosObj;
    GameObject PlayerPivot;
    FrogScript Pivot;
    Animator Ani;
    //x 2.2 y 0.45
    // Start is called before the first frame update
    private void Awake()
    {
        Pivot = transform.parent.gameObject.GetComponent<FrogScript>();
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

    void LandEndFunc()
    {
        Pivot.LandEndFunc();
    }

    public void BlinkEndFunc()
    {
        Pivot.TakeNextAction();
    }

    public void SpawnAttack()
    {
        GameObject tempObj = Instantiate(MonsterAttackObj);
        tempObj.GetComponent<MonsterAttackScript>().Damage = Pivot.Damage;
        tempObj.transform.position = AttackPosObj.transform.position;
        tempObj.transform.localScale = new Vector3(2.2f, 0.45f, 1);
        Destroy(tempObj, 0.1f);
    }
    public void AttackEndFunc()
    {
        Pivot.SetFrogState(FROGSTATE.IDLE);
        Ani.SetTrigger("Idle");
    }

    public void HitEndFunc()
    {
        Pivot.SetFrogState(FROGSTATE.IDLE);
        Ani.SetTrigger("Idle");
    }

    public void DeathEndFunc()
    {
        Pivot.IsFadeOn = true;
    }

}
