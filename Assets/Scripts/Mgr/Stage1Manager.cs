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
                
      
        Cam.GetComponent<CameraScript>().SetMinMax(-5.0f, BossBorder1.transform.position.x, -6.84f, 10.0f);//카메라 예외처리
        


    }

    // Update is called once per frame
    void Update()
    {
        MonHPZero();//진행용 치트키

        ClearStageEventUpdate();//이벤트 처리
       

    }

    override public void BossEventOn()//보스 이벤트 시작. 카메라 범위 변경. 막는 위치 변경. 플레이어 정지. UI 변경
    {
        BossBorder1.SetActive(true);
        BossBorder2.SetActive(false);
        Cam.GetComponent<CameraScript>().SetMinMax(BossBorder1.transform.position.x-0.5f, 108.0f, -6.84f, 10.0f);
        Cam.GetComponent<CameraScript>().SetBossEvent(true, Boss.transform.position);
        Player.GetComponent<PlayerScript>().SetOnEvent();
        UIMgrScript.Inst.SetBossUI(Boss.GetComponent<MonRootScript>());
    }

    override public void BossEventOff()//플레이어 움직임. 보스도 움직임 시작
    {                   
        
        Player.GetComponent<PlayerScript>().SetOffEvent();
        Cam.GetComponent<CameraScript>().SetBossEvent(false, Boss.transform.position);
        Boss.GetComponent<BanditBossScript>().SetEventOff();
        if(!IsStageCleared)
            ABGMSource.Play();
    }

    override public void ClearStage()//스테이지 클리어 처리.
    {
        IsClearEventOn = true;
        IsStageCleared = true;
        Player.GetComponent<PlayerScript>().SetOnEvent();
        Time.timeScale = 0.3f;
        ABGMSource.Stop();



    }

    void ClearStageEventUpdate()//스테이지 클리어시 슬로우 효과
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

    override public void StageClearEventFunc1()//바리케이트 파괴.
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
