using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FEyeAnimScript : MonoBehaviour
{
    FEyeScript Pivot;   
    Animator Ani;
    // Start is called before the first frame update
    void Start()
    {
        Pivot = transform.parent.gameObject.GetComponent<FEyeScript>();
        Ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attack1StartFunc()
    {
        Pivot.IsAttack1On = true;
        Pivot.MonsterAttackObj.gameObject.SetActive(true);
    }

    void Attack2StartFunc()
    {
        Pivot.IsAttack2On = true;
        Pivot.MonsterAttackObj.gameObject.SetActive(true);
    }

    void Attack2EndFunc()
    {
        Pivot.IsAttack2On = false;
        Pivot.MonsterAttackObj.gameObject.SetActive(false);
        Pivot.EndAttack();
    }

    void HitEndFunc()
    {
        Pivot.EndAttack();
    }
}
