using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMgrScript : MonoBehaviour
{
    public Button StartBtn;
    public Button ManualBtn;
    public Button ExitBtn;    
    public GameObject Graveyard;
    public GameObject Mountain;
    public GameObject UIs;
    public GameObject[] Clouds=new GameObject[3];

    [Header("Manual")]   
    public GameObject ManualWindow;
    public Button ManualMoveBtn;
    public Button ManualJumpBtn;
    public Button ManualSlidingBtn;
    public Button ManualAttackBtn;
    public Button ManualComboAttackBtn;
    public Button ManualHealingBtn; 
    public Button ExitButton;
    public GameObject ManualModel;
    public Image KeyImage;
    public Text KeyText;
    public Sprite[] KeySprites;
    public GameObject CatPos;
    public GameObject ModelCat;
    public Slider SoundSlider;
    public Button SoundApplyBtn;


    GameObject ModelCatPtr;
    int flag = 0;
    float Dist = 0;

    AudioSource ASource;

    // Start is called before the first frame update
    void Start()
    {
  

        if (StartBtn != null)
        {
            StartBtn.onClick.AddListener(StartBtnFunc);
            
        }
                    

        if (ExitBtn != null)
        {
            ExitBtn.onClick.AddListener(ExitBtnFunc);
        }
                  
        if (ManualBtn != null)
        {
            ManualBtn.onClick.AddListener(ManualBtnFunc);
        }
            

        ///////////////////////////////////매뉴얼창

        if (ManualMoveBtn != null)
        {
            ManualMoveBtn.onClick.AddListener(ManualMoveBtnFunc);

        }

        if (ManualJumpBtn != null)
        {
            ManualJumpBtn.onClick.AddListener(ManualJumpBtnFunc);
        }

        if (ManualSlidingBtn != null)
        {
            ManualSlidingBtn.onClick.AddListener(ManualSlidingBtnFunc);
        }

        if (ManualAttackBtn != null)
        {
            ManualAttackBtn.onClick.AddListener(ManualAttackBtnFunc);
        }

        if (ManualComboAttackBtn != null)
        {
            ManualComboAttackBtn.onClick.AddListener(ManualComboAttackBtnFunc);
        }

        if (ManualHealingBtn != null)
        {
            ManualHealingBtn.onClick.AddListener(ManualHealingBtnFunc);
        }

       


        if (ExitButton != null)
        {
            ExitButton.onClick.AddListener(ExitBtnFunc);

        }

        if (SoundApplyBtn != null)
        {
            SoundApplyBtn.onClick.AddListener(SoundApplyBtnFunc);
        }

        ASource = GetComponent<AudioSource>();
        ASource.volume = GValue.GSoundVolume;
        ASource.Play();

      
        GValue.PHP = 100;
        GValue.MAXHP = 100;
        GValue.StageLevel = 1;
        //SceneManager.LoadScene("UIScene");
        // SceneManager.LoadScene("Stage1", LoadSceneMode.Additive);
        //SceneManager.LoadScene("Stage2", LoadSceneMode.Additive);
        //SceneManager.LoadScene("Stage3", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        BackUpdate();

        CloudUpdate();
    }

    void StartBtnFunc()
    {
        SceneManager.LoadScene("UIScene");
        SceneManager.LoadScene("Stage1", LoadSceneMode.Additive);
        
    }

    void ManualBtnFunc()
    {
        ManualWindow.SetActive(true);
        
        SoundSlider.value= GValue.GSoundVolume;
    }

    void ExitBtnFunc()
    {
        if (ModelCatPtr)
            Destroy(ModelCatPtr);
        KeyImage.sprite = KeySprites[5];
        KeyText.text = "";
        ManualWindow.SetActive(false);        
        ManualModel.GetComponent<ManualModelScript>().ExitManual();
    }


    void ManualMoveBtnFunc()
    {
        if (ModelCatPtr)
            Destroy(ModelCatPtr);
        ManualModel.GetComponent<ManualModelScript>().SetManualType(MANUALTYPE.MOVE);
        KeyImage.sprite = KeySprites[0];
        KeyText.text = "이동";

    }

    void ManualJumpBtnFunc()
    {
        if (ModelCatPtr)
            Destroy(ModelCatPtr);
        ManualModel.GetComponent<ManualModelScript>().SetManualType(MANUALTYPE.JUMP);
        KeyImage.sprite = KeySprites[1];
        KeyText.text = "점프";
    }

    void ManualSlidingBtnFunc()
    {
        if (ModelCatPtr)
            Destroy(ModelCatPtr);
        ManualModel.GetComponent<ManualModelScript>().SetManualType(MANUALTYPE.SLIDING);
        KeyImage.sprite = KeySprites[2];
        KeyText.text = "슬라이딩";
    }

    void ManualAttackBtnFunc()
    {
        if (ModelCatPtr)
            Destroy(ModelCatPtr);
        ManualModel.GetComponent<ManualModelScript>().SetManualType(MANUALTYPE.ATTACK);
        KeyImage.sprite = KeySprites[3];
        KeyText.text = "기본 공격";
    }

    void ManualComboAttackBtnFunc()
    {
        if (ModelCatPtr)
            Destroy(ModelCatPtr);
        ManualModel.GetComponent<ManualModelScript>().SetManualType(MANUALTYPE.COMBOATTACK);
        KeyImage.sprite = KeySprites[4];
        KeyText.text = "연속 공격";
    }

    void ManualHealingBtnFunc()
    {
        ManualModel.GetComponent<ManualModelScript>().SetManualType(MANUALTYPE.HEALING);
        KeyImage.sprite = KeySprites[5];
        KeyText.text = "고양이에게 슬라이딩해서 체력 회복";
        SpawnModelCat();


    }

    void SoundApplyBtnFunc()
    {
        GValue.GSoundVolume = SoundSlider.value;
        AudioSource[] tempSources = FindObjectsOfType<AudioSource>();

        for (int i = 0; i < tempSources.Length; i++)
        {
            tempSources[i].volume = GValue.GSoundVolume;
        }
    }
    public void SpawnModelCat()
    {
        ModelCatPtr = Instantiate(ModelCat);
        ModelCatPtr.transform.position = CatPos.transform.position;
    }

    #region 배경
    void BackUpdate()
    {
        if(flag==0)
        {
            Dist += Time.deltaTime;
            Mountain.transform.position += new Vector3(0, Time.deltaTime, 0);
            if(Dist>3)
            {
                flag++;
                Dist = 0;
            }
        }
        else if(flag==1)
        {
            Dist += Time.deltaTime;
            Graveyard.transform.position += new Vector3(0, Time.deltaTime, 0);
            if (Dist > 2)
            {
                flag++;
                Dist = 0;
            }
        }
        else if (flag == 2)
        {
            Dist += Time.deltaTime;
            
            if (Dist > 1)
            {
                UIs.SetActive(true);
            }
        }


    }

    void CloudUpdate()
    {
        for(int i=0; i<Clouds.Length; i++)
        {
            Clouds[i].transform.position += new Vector3(Time.deltaTime * 0.6f, 0, 0);
            if(Clouds[i].transform.position.x>15)
            {
                Vector3 temp = Vector3.zero;
                temp.x = -15;
                temp.y = Random.Range(-1.8f, 2.8f);
                Clouds[i].transform.position = temp;
            }
        }        
    }
    #endregion


}
