using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Manager : StageManagerRoot
{
    [Header("OutBorders")]
    public Transform LeftBoder;
    public Transform RightBoder;
    public Transform DownBoder;
    public Transform UpBoder;

    bool OnLeverAlram = false;
    // Start is called before the first frame update
    private void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        base.Start();
        AClip = Resources.Load<AudioClip>("Sound/BGM/Stage2BGM");
        ASource.PlayOneShot(AClip);
        Cam.GetComponent<CameraScript>().SetMinMax(LeftBoder.position.x-0.5f, RightBoder.position.x+0.5f, -8, 18);//카메라 예외처리
    }

    // Update is called once per frame
    void Update()
    {
        MonHPZero();

        LeverCheck();


    }

    void LeverCheck()
    {
        if (OnLeverAlram)
            return;
        if (Player.transform.position.x > 54)
        {
            OnLeverAlram = true;
            UIMgrScript.Inst.OnGuideText("레버를 작동시켜 장애물을 해제하세요");
        }
    }
}
