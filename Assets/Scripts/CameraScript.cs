using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraScript : MonoBehaviour
{

    StageManagerRoot StageManager;
    GameObject Player;
    float MinX, MaxX, MinY, MaxY;
    float DistX = 10;
    float DistY = 6;

    bool IsRestoringPers = false;
    float RestoringValue = 0;
    float RestoringSpeed = 0.05f;

    float RestoreTime = 1.5f;

    bool IsOnBossEvent = false;
    Vector3 TargetPos = Vector3.zero;
    float CamEventSpeed = 5.0f;
    float CamOnBossTime = 3.0f;
    int EventCamPhase=0;


    float PreXCamPos = 0;
    float PreYCamPos = 0;
    Vector3 EventPivotCampos=Vector3.zero;

    bool IsOnStage3BossEvent = false;
    int Stage3EventPhase = 0;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("PlayerPivot");
        StageManager=FindObjectOfType<StageManagerRoot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (Player == null)
            return;

        if (IsOnBossEvent)
        {
            BossEventUpdate();
            return;
        }

        if(IsOnStage3BossEvent)
        {
            Stage3BossEventUpdate();
            return;
        }
       
        if (!IsRestoringPers)
        {
            Vector3 temp = Player.transform.position;
            temp.z = -10;

            if (temp.x - DistX < MinX)
                temp.x = MinX + DistX;

            if (temp.x + DistX > MaxX)
                temp.x = MaxX - DistX;

            if (temp.y - DistY < MinY)
                temp.y = MinY + DistY;

            if (temp.y + DistY > MaxY)
                temp.y = MaxY - DistY;




            transform.position = temp;
        }
        else
        {
            RestorePers();


        }

        

    }

    public void SetMinMax(float minX, float maXX, float minY, float maxY)
    {
        
        MinX = minX;
        MaxX = maXX;
        MinY = minY;
        MaxY=maxY;
    }

    public void ExPandMaxX(float maxX)
    {
        MaxX = maxX;
        IsRestoringPers = true;
        PreXCamPos = transform.position.x;
        PreYCamPos = transform.position.y;
    }

    public void SetMinX(float minx)
    {
        MinX = minx;
        IsRestoringPers = true;
        PreXCamPos = transform.position.x;
        PreYCamPos = transform.position.y;
    }



    void RestorePers()//카메라 가장자리 범위 바뀔 때 시점 회복.
    {
        RestoreTime -= Time.deltaTime;
        RestoringValue += Time.deltaTime * RestoringSpeed;
       
        Vector2 temp = Vector2.zero;        
        temp = Vector2.Lerp(transform.position, Player.transform.position, RestoringValue);

        bool OutFlg = true;

        if (temp.x - DistX < MinX)
        {
            OutFlg = false;
            temp.x = MinX + DistX;
        }
            

        if (temp.x + DistX > MaxX)
        {
            OutFlg = false;
            temp.x = MaxX - DistX;
        }
            
        

        if (temp.y - DistY < MinY)
        {
            OutFlg = false;
            temp.y = PreYCamPos;
        }
           

        if (temp.y + DistY > MaxY)
        {
            OutFlg = false;
            temp.y = PreYCamPos;
        }
         

        transform.position = new Vector3(temp.x, temp.y, -10);

        Vector2 CamPos = transform.position;
        Vector2 PlayerPos=Player.transform.position;
        //if ((PlayerPos -CamPos).magnitude< 0.05f || RestoreTime<0)
        if(OutFlg && (PlayerPos - CamPos).magnitude < 0.05f)
        {
            IsRestoringPers = false;
            RestoringValue = 0;
            RestoreTime = 2.0f;
        }
    }

    public void SetBossEvent(bool b, Vector3 targetPos)
    {
        IsOnBossEvent = b;
        TargetPos = targetPos;

        if(StageManager.IsStageCleared)
        {
            EventPivotCampos = transform.position;
            EventPivotCampos.z = 0;
        }
        else
        {
            EventPivotCampos = Player.transform.position;
            EventPivotCampos.z = 0;
        }
        
    }
    void BossEventUpdate()
    {
        if(EventCamPhase == 0)
        {
            
            Vector3 Dir = TargetPos - Player.transform.position;
            Dir.Normalize();
            transform.position += Dir * Time.deltaTime * CamEventSpeed;

            if(Mathf.Abs(transform.position.x- TargetPos.x)<0.05f)
            {
                EventCamPhase++;                
            }
                    

        }

        if (EventCamPhase == 1)
        {
            CamOnBossTime-=Time.deltaTime;
            if(CamOnBossTime < 0 )
            {
                EventCamPhase++;
                if(StageManager.IsStageCleared)
                {
                    StageManager.StageClearEventFunc1();
                }
            }

        }

        if (EventCamPhase == 2)
        {
            Vector3 CamPos = transform.position;
            CamPos.z = 0;
            Vector3 Dir = EventPivotCampos - CamPos;
            Dir.Normalize();
            transform.position += Dir * Time.deltaTime * CamEventSpeed;

            if (Mathf.Abs(EventPivotCampos.x - CamPos.x) < 0.05f)
            {
                StageManager.BossEventOff();
                EventCamPhase = 0;
                CamOnBossTime = 3;
                if (StageManager.IsStageCleared)
                {
                   UIMgrScript.Inst.SetOffBossUI();
                }
                
            }


        }


    }

    public void SetStage3BossEvent(bool b, Vector3 targetPos)
    {
        IsOnStage3BossEvent = b;
        TargetPos = targetPos;
    }

    void Stage3BossEventUpdate()
    {
        if (Stage3EventPhase == 0)
        {

            Vector3 Dir = TargetPos - Player.transform.position;
            Dir.Normalize();
            transform.position += Dir * Time.deltaTime * CamEventSpeed;

            if (Mathf.Abs(transform.position.x - TargetPos.x) < 0.05f)
            {
                StageManager.Stage3BossFunc();
                Stage3EventPhase++;
            }


        }

        if(Stage3EventPhase==2)
        {
            Vector3 CamPos = transform.position;
            CamPos.z = 0;
            Vector3 Dir = Player.transform.position - CamPos;
            Dir.Normalize();
            transform.position += Dir * Time.deltaTime * CamEventSpeed;

            
            if (Mathf.Abs(Player.transform.position.x - CamPos.x) < 0.05f)
            {

                Stage3EventPhase++;
                IsOnStage3BossEvent = false;
                Player.GetComponent<PlayerScript>().SetOffEvent();
                GameObject Stage3Boss = GameObject.FindGameObjectWithTag("Boss");
                Stage3Boss.GetComponent<BeetleScript>().OffEvent();
                UIMgrScript.Inst.SetBossUI(Stage3Boss.GetComponent<BeetleScript>());
                StageManager.Stage3BGMFunc1();

            }
        }
    }

    public void AddStage3EventPhase()
    {
        Stage3EventPhase++;
    }
}
