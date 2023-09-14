using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WMonAnimScript : MonoBehaviour
{
    public GameObject TentacleObj;
    public GameObject MonsterAttackObj;
    public GameObject AttackPosObj;
    GameObject PlayerPivot;
    WMonScript Pivot;

    Animator Ani;
    // Start is called before the first frame update
    private void Awake()
    {
        Pivot = transform.parent.gameObject.GetComponent<WMonScript>();
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

    public void Attack1Spawn()
    {
        //x1.67 y0.58
        GameObject tempObj = Instantiate(MonsterAttackObj);
        tempObj.GetComponent<MonsterAttackScript>().Damage = Pivot.Damage;
        tempObj.transform.position = AttackPosObj.transform.position;
        tempObj.transform.localScale = new Vector3(1.67f, 0.58f, 1.0f);
        Pivot.AClip = Resources.Load<AudioClip>("Sound/Monster/WMonAttack");
        Pivot.ASource.PlayOneShot(Pivot.AClip);
        Destroy(tempObj, 0.15f);


    }

    public void Attack1EndFunc()
    {
        Pivot.TakeNextAction();      
      
    }

    public void SpawnTentacle()
    {
        GameObject tempObj = Instantiate(TentacleObj);
        float RanX = Random.Range(-0.5f, 0.5f);
        float RanY = Random.Range(-0.5f, 0.5f);
        tempObj.transform.position= PlayerPivot.transform.position;
        tempObj.transform.position += new Vector3(RanX, RanY, 0);
        
        Pivot.AddAttackNum();
    }


    public void Attack2EndFunc()
    {
        Pivot.TakeNextAction();
        Pivot.ZeroAttackNum();
    }

    public void BlinkEndFunc()
    {
        Ani.SetTrigger("Run");
        Pivot.SetWMonState(WMONSTATE.COMBAT);
    }

    public void HitEndFunc()
    {
        Ani.SetTrigger("Blink");
        Pivot.SetWMonState(WMONSTATE.BLINK);
        Pivot.ZeroAttackNum();
    }

    public void DeathEndFunc()
    {
        Pivot.IsFadeOn = true;
    }

}
