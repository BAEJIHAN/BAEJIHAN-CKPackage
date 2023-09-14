using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Manager : StageManagerRoot
{

 

    [Header("Boss Event")]
    public GameObject BossBorder1;
    public GameObject BossBorder2;
    public GameObject Boss;
    public GameObject ClearPos;

   

    public bool IsClearEventOn = false;
    float CurClearEventTime = 0;
    float ClearEventTime = 4;

    bool IsBorderZeroOn = false;


    

    public GameObject BGM;
    AudioSource ABGMSource;
    // Start is called before the first frame update
    protected void Awake()
    {
        base.Awake();
        
        ABGMSource = BGM.GetComponent<AudioSource>();
        ABGMSource.volume = GValue.GSoundVolume;

    }
    void Start()
    {
        base.Start();
                
      
        Cam.GetComponent<CameraScript>().SetMinMax(-5.0f, BossBorder1.transform.position.x, -6.84f, 10.0f);//ī�޶� ����ó��
        


    }

    // Update is called once per frame
    void Update()
    {
        MonHPZero();//����� ġƮŰ

        ClearStageEventUpdate();//�̺�Ʈ ó��
       

    }

    override public void BossEventOn()//���� �̺�Ʈ ����. ī�޶� ���� ����. ���� ��ġ ����. �÷��̾� ����. UI ����
    {
        BossBorder1.SetActive(true);
        BossBorder2.SetActive(false);
        Cam.GetComponent<CameraScript>().SetMinMax(BossBorder1.transform.position.x-0.5f, 108.0f, -6.84f, 10.0f);
        Cam.GetComponent<CameraScript>().SetBossEvent(true, Boss.transform.position);
        Player.GetComponent<PlayerScript>().SetOnEvent();
        UIMgrScript.Inst.SetBossUI(Boss.GetComponent<MonRootScript>());
    }

    override public void BossEventOff()//�÷��̾� ������. ������ ������ ����
    {                   
        
        Player.GetComponent<PlayerScript>().SetOffEvent();
        Cam.GetComponent<CameraScript>().SetBossEvent(false, Boss.transform.position);
        Boss.GetComponent<BanditBossScript>().SetEventOff();
        if(!IsStageCleared)
            ABGMSource.Play();
    }

    override public void ClearStage()//�������� Ŭ���� ó��.
    {
        IsClearEventOn = true;
        IsStageCleared = true;
        Player.GetComponent<PlayerScript>().SetOnEvent();
        Time.timeScale = 0.3f;
        ABGMSource.Stop();



    }

    void ClearStageEventUpdate()//�������� Ŭ����� ���ο� ȿ��
    {
        if (!IsClearEventOn)
            return;


        CurClearEventTime += Time.unscaledDeltaTime;
        if(CurClearEventTime>ClearEventTime)
        {
            Cam.GetComponent<CameraScript>().SetBossEvent(true, ClearPos.transform.position);
            CurClearEventTime = 0;
            IsClearEventOn = false;
            Time.timeScale = 1;
        }
        
    }

    override public void StageClearEventFunc1()//�ٸ�����Ʈ �ı�.
    {
        BarricadeScript[] Barricades = FindObjectsOfType<BarricadeScript>();
        AClip = Resources.Load<AudioClip>("Sound/Misc/BarrelDestroy");
        ASource.PlayOneShot(AClip);
        for (int i=0; i<Barricades.Length; i++)
        {
            Barricades[i].DestroyBari();
        }
    }

    public override void SubMonCount()
    {
        base.SubMonCount();
        
        if(MonCount<=0)
        {
            BossBorder1.SetActive(false);
            Cam.GetComponent<CameraScript>().ExPandMaxX(112.0f);
        }
    }
}
