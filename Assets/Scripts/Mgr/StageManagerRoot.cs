using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManagerRoot : MonoBehaviour
{
    [HideInInspector]public bool IsStageCleared = false;
    protected GameObject Player;
    public GameObject StartPos;
    protected Camera Cam;
    protected int MonCount;

    protected AudioSource ASource;
    protected AudioClip AClip;

    public bool IsStage3BossOn = false;
    // Start is called before the first frame update
    protected void Awake()
    {
        ASource = GetComponent<AudioSource>();
        AudioSource[] tempSources = FindObjectsOfType<AudioSource>();

        for (int i = 0; i < tempSources.Length; i++)
        {
            tempSources[i].volume = GValue.GSoundVolume;
        }
    }
    protected void Start()
    {
        Player = GameObject.Find("PlayerPivot");
       
        if (Player)
        {
            Player.transform.position = StartPos.transform.position;
            Player.GetComponent<PlayerScript>().SetHP(GValue.PHP);
        }

        Cam = Camera.main;
        if (Cam == null)
            Debug.Log("CamNull");

        if (UIMgrScript.Inst)
        {
            UIMgrScript.Inst.SetHPBar(GValue.MAXHP, GValue.PHP);
        }

        MonCount = GameObject.FindGameObjectsWithTag("Monster").Length;
        MonCount+= GameObject.FindGameObjectsWithTag("FlyMon").Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    virtual public void BossEventOn()
    {

    }

    virtual public void BossEventOff()
    {

    }

    virtual public void ClearStage()
    {

    }

    virtual public void StageClearEventFunc1()
    {

    }

    virtual public void Stage3BossFunc()
    {

    }

    virtual public void Stage3BGMFunc1()
    {

    }
    protected void MonHPZero()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            MonRootScript[] Mons = GameObject.FindObjectsOfType<MonRootScript>();

            for (int i = 0; i < Mons.Length; i++)
            {
                Mons[i].MonHPOne();
            }
        }
    }

    public virtual void SubMonCount()
    {

        MonCount--;
       
    }
}
