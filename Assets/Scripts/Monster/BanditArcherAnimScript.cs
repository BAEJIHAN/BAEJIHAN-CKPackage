using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BanditArcherAnimScript : MonoBehaviour
{
    public GameObject ArrowObj;
    public GameObject ArrowPos;
    GameObject PlayerPivot;
    BanditArcherScript Pivot;
    Animator Ani;
   
    
    // Start is called before the first frame update
    private void Awake()
    {
        Pivot = transform.parent.gameObject.GetComponent<BanditArcherScript>();
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

    public void AttackSpawnFunc()
    {
        GameObject tempArrow=Instantiate(ArrowObj);
        tempArrow.transform.position=ArrowPos.transform.position;
        tempArrow.transform.rotation = ArrowPos.transform.rotation;
        Pivot.AClip = Resources.Load<AudioClip>("Sound/Monster/BowFire");
        Pivot.ASource.PlayOneShot(Pivot.AClip);
    }

    public void AttackEndFunc()
    {
        Ani.SetTrigger("Run");
        Pivot.SetBanditArcherState(BANDITARCHERSTATE.COMBAT);
        Pivot.SetDest();
        Pivot.SetPreAttackNum();
    }

    public void HitEndFunc()
    {
        Ani.SetTrigger("Run");
        Pivot.SetBanditArcherState(BANDITARCHERSTATE.COMBAT);
        Pivot.SetDest();
        Pivot.SetPreAttackNum();
    }

    public void DeathEndFunc()
    {
        Pivot.IsFadeOn = true;
    }
}
