using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage3Manager : StageManagerRoot
{

    public SpikeGroup Spikes;
    public GameObject SpikeObstacle;
    public GameObject BossRoomLeft;
    public GameObject Boss;
    public GameObject BossPos;

    protected void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        
        Cam.GetComponent<CameraScript>().SetMinMax(-10.0f, 68.0f, -13.0f, 15.0f );//카메라 예외처리
    }

    // Update is called once per frame
    void Update()
    {
        MonHPZero();

        OnBossFunc();
    }

    public override void SubMonCount()
    {
        base.SubMonCount();
        if(MonCount<=0)
        {
            Spikes.SpikesDown();
            SpikeObstacle.SetActive(false);
        }
    }

    void OnBossFunc()
    {
        if (IsStage3BossOn)
            return;

        if(Player.transform.position.x>45)
        {
            IsStage3BossOn = true;
            Spikes.SpikesUp();
            BossRoomLeft.SetActive(true);
            Player.GetComponent<PlayerScript>().SetOnEvent();

            
            Cam.GetComponent<CameraScript>().SetStage3BossEvent(true, BossPos.transform.position - new Vector3(0, 12, 0));
        }
    }

    public override void Stage3BossFunc()
    {
        GameObject tempBoss = Instantiate(Boss);
        tempBoss.transform.position = BossPos.transform.position;
    }

    public override void Stage3BGMFunc1()
    {
        ASource.Play();
    }

    public override void ClearStage()
    {
        Time.timeScale = 0.3f;
        StartCoroutine(Stage3ClearEvent());
        ASource.Stop();


    }

    IEnumerator Stage3ClearEvent()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        StartCoroutine(TOEnding(5.0f));
        Time.timeScale = 1.0f;
    }

    IEnumerator TOEnding(float t)
    {
        yield return new WaitForSecondsRealtime(t);

        SceneManager.LoadScene("EndingScene");
    }




}
