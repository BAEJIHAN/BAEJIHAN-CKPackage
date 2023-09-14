using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleAnimScript : MonoBehaviour
{
    public GameObject Spike;
    public GameObject DustPos;
    public GameObject Dust;
    public GameObject AttackCol;
    BeetleScript Pivot;
    Animator Ani;
    bool IsSecondAttack = false;
    

    // Start is called before the first frame update
    void Start()
    {
        Pivot = transform.parent.gameObject.GetComponent<BeetleScript>();
        Ani = GetComponent<Animator>();
    }
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        
    }

    void JumpStartFunc()
    {
        Pivot.AClip = Resources.Load<AudioClip>("Sound/Monster/BeetleJump");
        Pivot.ASource.PlayOneShot(Pivot.AClip);
        Pivot.SetJumpPhase(1);
    }

    void LandingEndFunc()
    {
        
        Pivot.GetComponent<BoxCollider2D>().enabled = true;
        Pivot.TakeNextAct();
     
    }

    void PreSpikeEndFunc()
    {
        Ani.SetTrigger("Spike");


    }

    

    void SpikeEndFunc()
    {
        float PivotRot= Random.Range(0, 18);
        float tempRot = 0;

        Pivot.AClip = Resources.Load<AudioClip>("Sound/Monster/Spike");
        Pivot.ASource.PlayOneShot(Pivot.AClip);

        for (int i=0; i<20; i++)//여기서 가시 소환

        {
            tempRot = i * 18+ PivotRot;
            GameObject tempObj = Instantiate(Spike);
            tempObj.transform.position = Pivot.transform.position;
            tempObj.transform.eulerAngles = new Vector3(0, 0, tempRot);
            
        }
        
        if(Pivot.GetHP()<=30)
        {
            if(IsSecondAttack)
            {
                Pivot.SetBeetleState(BEETLESTATE.GROGGY);
                Ani.SetTrigger("Groggy");
                IsSecondAttack = false;
            }
            else
            {
                Ani.SetTrigger("Spike");
                IsSecondAttack = true;
            }
            
        }
        else
        {
            Ani.SetTrigger("PostSpike");
        }
    }

    void PostSpikeEndFunc()
    {
        Pivot.TakeNextAct();
    }

    void PreRollEndFunc()
    {
        Pivot.SetRolling(true);
        Ani.SetTrigger("Roll");
        GameObject tempDust = Instantiate(Dust);
        tempDust.transform.position = DustPos.transform.position;
        tempDust.transform.rotation = Pivot.transform.rotation;
        Pivot.RollSoundSource.Play();
        AttackCol.gameObject.SetActive(true);
    }

    void PostRollStartFunc()
    {

        Pivot.AClip = Resources.Load<AudioClip>("Sound/Monster/RollStart");
        Pivot.ASource.PlayOneShot(Pivot.AClip);

        Pivot.SetRolling(false);        
        GameObject tempDust = Instantiate(Dust);
        tempDust.transform.position = DustPos.transform.position;
        tempDust.transform.rotation = Pivot.transform.rotation;
        AttackCol.gameObject.SetActive(false);
    }
    void PostRollEndFunc()
    {
        Pivot.TakeNextAct();

    }

    void PreRoarEndFunc()
    {
        Ani.SetTrigger("Roar");
       
    }

    void RoarStartFunc()
    {
        Pivot.AClip = Resources.Load<AudioClip>("Sound/Monster/Roar3");
        Pivot.ASource.PlayOneShot(Pivot.AClip);
        //소리
    }

    void PostRoarEndFunc()
    {

        Ani.SetTrigger("Idle");
        
        Camera cam= Camera.main;
        cam.GetComponent<CameraScript>().AddStage3EventPhase();
    }
}
