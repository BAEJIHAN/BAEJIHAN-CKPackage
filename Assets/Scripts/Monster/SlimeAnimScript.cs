using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimScript : MonoBehaviour
{
    SlimeScript Pivot;
    GameObject PlayerPivot;
    Animator Ani;
    // Start is called before the first frame update
   

    private void Awake()
    {
        Pivot = transform.parent.gameObject.GetComponent<SlimeScript>();
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

    public void JumpFunc1()
    {
        Pivot.JumpLevel = 1;
        Pivot.AClip = Resources.Load<AudioClip>("Sound/Monster/SlimePreJump");
        Pivot.ASource.PlayOneShot(Pivot.AClip);
    }

    public void JumpFunc2()
    {
        Pivot.JumpLevel = 2;
    }

    public void JumpFunc3()
    {
        Pivot.JumpLevel = 0;
        Pivot.AClip = Resources.Load<AudioClip>("Sound/Monster/SlimeJump");
        Pivot.ASource.PlayOneShot(Pivot.AClip);
    }
    public void JumpEndFunc()
    {
        Pivot.SetSlimeState(SLIMESTATE.CHASE);
        Ani.SetTrigger("Walk");
        Pivot.MonsterAttackObj.gameObject.SetActive(false);
        

    }

    public void OnAttackFunc()
    {
        Pivot.MonsterAttackObj.gameObject.SetActive(true);
    }

    public void SpinEndFunc()
    {
        Pivot.SetSlimeState(SLIMESTATE.CHASE);        
        Ani.SetTrigger("Walk");
        Pivot.SpinTime = 1;
        Pivot.MonsterAttackObj.gameObject.SetActive(false);

    }

    void DeathEndFunc()
    {
        Destroy(Pivot.gameObject);
    }
}
