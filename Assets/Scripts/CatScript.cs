using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CatScript : MonoBehaviour
{
    enum CATSTATE
    {
        IDLE,
        ITCH,
        MEOW,
        WALK,
        STRECTCHING,
        RUN,
        LAYING,
        LICKING
    }

    Animator Ani;
    CATSTATE State = CATSTATE.IDLE;
    // Start is called before the first frame update
    float ItchTime = 0;
    float CurItchTime = 0;

    float LickingTime = 0;
    float CurLickingTime = 0;

    float CurLayingTime = 0;
    float LayingTime = 0;

    Vector3 DestPos;
    float DestDist;

    float WalkSpeed = 1.0f;
    float RunSpeed = 6;

    int PreSel = -1;

    AudioSource ASource;
    AudioClip AClip;

    private void Awake()
    {
        Ani= GetComponentInChildren<Animator>();
    }
    void Start()
    {        
        
        ASource = GetComponent<AudioSource>();
        ASource.volume = GValue.GSoundVolume;
        if (ASource == null)
            Debug.Log("null");
        ASource.volume = GValue.GSoundVolume;
        AClip = Resources.Load<AudioClip>("Sound/Misc/BarrelDestroy");
        ASource.PlayOneShot(AClip);
        SelAct();
    }

    // Update is called once per frame
    void Update()
    {
        WalkUpdate();

        ItchUpdate();

        LickingUpdate();

        LayingUpdate();

        RunUpdate();
    }

    void SetDest()
    {
        State = CATSTATE.WALK;        
        Vector3 tempV = transform.position;
        tempV.x = Random.Range(-3.0f, 3.0f);
        tempV.y = Random.Range(-2.0f, 2.0f);
        tempV.z = 0;
        DestPos = tempV;
        DestDist = (DestPos - transform.position).magnitude;
       
    }

    void WalkUpdate()
    {
        if (CATSTATE.WALK != State)
            return;

        if(DestPos.x>transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        Vector3 Dir = DestPos - transform.position;
        Dir.Normalize();

        transform.position += Dir * Time.deltaTime * WalkSpeed;
        DestDist -= Time.deltaTime * WalkSpeed;
        if(DestDist<0)
        {
            SelAct();
        }
    }

    void ItchUpdate()
    {
        if (CATSTATE.ITCH != State)
            return;

        CurItchTime += Time.deltaTime;
        if(CurItchTime>ItchTime)
        {
            CurItchTime = 0;
            SelAct();
        }
    }

    void LickingUpdate()
    {
        if (CATSTATE.LICKING != State)
            return;
        CurLickingTime += Time.deltaTime;
        if (CurLickingTime > LickingTime)
        {
            CurLickingTime = 0;
            SelAct();
        }
    }

    void LayingUpdate()
    {
        if (CATSTATE.LAYING != State)
            return;
        CurLayingTime += Time.deltaTime;
        if (CurLayingTime > LayingTime)
        {
            CurLayingTime = 0;
            SelAct();
        }
    }

    void RunUpdate()
    {
        if(CATSTATE.RUN != State)
            return;
        transform.position += Vector3.left * Time.deltaTime * RunSpeed;
    }
    public void SelAct()
    {
        int Sel = Random.Range(0, 11); //0 ~5 Walk 6 Itch 7 Meow 8 Licking 9 Laying 10 Stretching 

       
        if (Sel == 6)
        {
            Ani.SetTrigger("Itch");
            State= CATSTATE.ITCH;
            ItchTime = Random.Range(3, 7);
        }
        else if (Sel == 7)
        {
            Ani.SetTrigger("Meow");
            State = CATSTATE.MEOW;
            AClip = Resources.Load<AudioClip>("Sound/Misc/CatMeow");
            ASource.PlayOneShot(AClip);
        }
        else if (Sel == 8)
        {
            Ani.SetTrigger("Licking");
            State = CATSTATE.LICKING;
            LickingTime = Random.Range(3, 7);

        }
        else if (Sel == 9)
        {
            if(PreSel==9)
            {
                SelAct();
            }
            else
            {
                Ani.SetTrigger("Laying");
                State = CATSTATE.LAYING;
                LayingTime = Random.Range(6, 10);
            }
           

        }
        else if (Sel == 10)
        {
            if (PreSel == 10)
            {
                SelAct();
            }                
            else
            {
                Ani.SetTrigger("Stretching");
                State = CATSTATE.STRECTCHING;
            }    
            
        }
        else//Sel 0~5
        {           
            Ani.SetTrigger("Walk");
            SetDest();            
        }
        PreSel = Sel;

    }

    public void SetFreeCat()
    {
        Ani.SetTrigger("Run");
        State = CATSTATE.RUN;
        BoxCollider2D[] tempBoxs=GetComponentsInChildren<BoxCollider2D>();
        for(int i=0; i<tempBoxs.Length; i++)
        {
            tempBoxs[i].enabled = false;
        }
        
        transform.rotation = Quaternion.Euler(0, 180, 0);
        AClip = Resources.Load<AudioClip>("Sound/Misc/CatEscape");
        ASource.PlayOneShot(AClip);
        Destroy(gameObject, 10);
    }
   
}
