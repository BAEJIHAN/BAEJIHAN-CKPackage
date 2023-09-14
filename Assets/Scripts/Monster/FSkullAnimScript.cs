using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSkullAnimScript : MonoBehaviour
{
    public GameObject MonsterAttackObj;
    FSkullScript Pivot;
    Animator Ani;
    // Start is called before the first frame update
    void Start()
    {
        Pivot = transform.parent.gameObject.GetComponent<FSkullScript>();
        Ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BombFunc()
    {
        GameObject tempObj = Instantiate(MonsterAttackObj);
        tempObj.GetComponent<MonsterAttackScript>().Damage = Pivot.Damage;
        tempObj.transform.position = Pivot.transform.position;
        Pivot.AClip = Resources.Load<AudioClip>("Sound/Monster/Explosion");
        Pivot.ASource.PlayOneShot(Pivot.AClip);
        Destroy(tempObj, 1.0f);
    }

    public void BombEndFunc()
    {
        Destroy(Pivot.gameObject);
    }

    public void DeathEndFunc()
    {
        Destroy(Pivot.gameObject);
    }
    public void HitEndFunc()
    {
        Pivot.SetFSkullState(FSKULLSTATE.CHASE);
        Ani.SetTrigger("Fly");
    }
}
