using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIMgrScript : MonoBehaviour
{
    public Canvas UICanVas;
    public Text GuideText;
    public Text DebugText;

    [Header("PlayerInfo")]    
    public GameObject HPUpTextRoot;
    public Image HPBar;
    public Image StaminaBar;

    [Header("BossInfo")]
    public GameObject BossUIRoot;
    public Image BossHPBar;
    public RawImage BossPortrait;


    [Header("Manual")]
    public Button ManualOpenBtn;
    public GameObject ManualWindow;
    public Button ManualMoveBtn;
    public Button ManualJumpBtn;
    public Button ManualSlidingBtn;
    public Button ManualAttackBtn;
    public Button ManualComboAttackBtn;
    public Button ManualHealingBtn;
    public Button ManualToTitleBtn;
    public Button ManualGameQuitBtn;
    public Button ExitButton;    
    public GameObject ManualModel;
    public Image KeyImage;
    public Text KeyText;
    public Sprite[] KeySprites;
    public GameObject CatPos;
    public GameObject ModelCat;

    [Header("Sound")]
    public Slider SoundSlider;
    public Button SoundApplyBtn;

    [Header("GameOver")]
    public GameObject GameOverWnd;
    public Button ToTitleBtn;
    public Button QuitBtn;







    GameObject ModelCatPtr;
    GameObject PlayerPivot;

    public static UIMgrScript Inst;
    private void Awake()
    {
        Inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerPivot = GameObject.Find("PlayerPivot");
        SoundSlider.value = GValue.GSoundVolume;
        

        if (ManualOpenBtn!=null)
        {
            ManualOpenBtn.onClick.AddListener(ManualOpenBtnFunc);
           
        }

        if(ManualMoveBtn!=null)
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

        if (ManualToTitleBtn != null)
        {
            ManualToTitleBtn.onClick.AddListener(ManualToTitleBtnFunc);
        }

        if (ManualGameQuitBtn != null)
        {
            ManualGameQuitBtn.onClick.AddListener(ManualGameQuitBtnFunc);
        }


        if (ExitButton!=null)
        {
            ExitButton.onClick.AddListener(ExitBtnFunc);
           
        }

        if(ToTitleBtn != null)
        {
            ToTitleBtn.onClick.AddListener(ToTitleBtnFunc);
        }

        if (QuitBtn != null)
        {
            QuitBtn.onClick.AddListener(QuitBtnFunc);
        }

        if(SoundApplyBtn!=null)
        {
            SoundApplyBtn.onClick.AddListener(SoundApplyBtnFunc);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    public void ManualOpenBtnFunc()
    {

        ManualWindow.SetActive(true);
        Time.timeScale = 0.0f;
        SoundSlider.value = GValue.GSoundVolume;
    }
    void ManualMoveBtnFunc()
    {
        if(ModelCatPtr)
            Destroy(ModelCatPtr);
        ManualModel.GetComponent<ManualModelScript>().SetManualType(MANUALTYPE.MOVE);
        KeyImage.sprite=KeySprites[0];
        KeyText.text="이동";
       
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

    void ManualToTitleBtnFunc()
    {

        if (ModelCatPtr)
            Destroy(ModelCatPtr);
        ManualModel.GetComponent<ManualModelScript>().ExitManual();
        KeyImage.sprite = KeySprites[5];
        KeyText.text = "";
        ManualWindow.SetActive(false);

        SceneManager.LoadScene("TitleScene");
        GValue.PHP = 100;
        GValue.StageLevel = 1;
        Time.timeScale = 1f;


    }

    void ManualGameQuitBtnFunc()
    {
        Application.Quit();
    }

    void ExitBtnFunc()
    {
        if (ModelCatPtr)
            Destroy(ModelCatPtr);
        KeyImage.sprite = KeySprites[5];
        KeyText.text = "";
        ManualWindow.SetActive(false);
        Time.timeScale = 1f;
        ManualModel.GetComponent<ManualModelScript>().ExitManual();
     
        
    }

    void ToTitleBtnFunc()
    {
        SceneManager.LoadScene("TitleScene");
        GValue.PHP = 100;
        GValue.StageLevel= 1;
        Time.timeScale= 1f;
        OffGameWnd();
    }

    void QuitBtnFunc()
    {        
        Application.Quit();
    }

    void SoundApplyBtnFunc()
    {
        GValue.GSoundVolume = SoundSlider.value;
        AudioSource[] tempSources = FindObjectsOfType<AudioSource>();

        for(int i=0; i<tempSources.Length; i++)
        {
            tempSources[i].volume = GValue.GSoundVolume;
        }
    }
    public void SpawnModelCat()
    {
        ModelCatPtr = Instantiate(ModelCat);
        ModelCatPtr.transform.position = CatPos.transform.position;
    }
    public void SpawnHPUp(Vector3 VPos)
    {
        GameObject temp = Instantiate(HPUpTextRoot);
        temp.GetComponent<HPUPTextScript>().Init(VPos, UICanVas.transform);
    }

    public void SetHPBar(float MaxHp, float CurHp)
    {
        HPBar.fillAmount = CurHp / MaxHp;
    }

    public void SetStaminaBar(float MaxStamina, float CurStamina)
    {
        StaminaBar.fillAmount=CurStamina / MaxStamina;
    }

    public void SetBossUI(MonRootScript Boss)
    {
        BossUIRoot.SetActive(true);
        BossPortrait.texture = Boss.PortraitTexture;
    }

    public void SetBossHPBar(float MaxHp, float CurHp)
    {
        BossHPBar.fillAmount = CurHp / MaxHp;
        
    }

    public void SetOffBossUI()
    {
        BossHPBar.fillAmount = 1.0f;
        BossUIRoot.SetActive(false);
    }


    public void SetDebugText(string s)
    {
        DebugText.text = s;
    }

    public void OnGameOverWnd()
    {
        GameOverWnd.SetActive(true);
    }

    public void OffGameWnd()
    {
        GameOverWnd.SetActive(false);
    }

    public void OnGuideText(string s)
    {
        GuideText.gameObject.SetActive(true);
        GuideText.text = s;
    }

    public void OffGuideText()
    {
        GuideText.gameObject.SetActive(false);
        GuideText.text = "";
    }

}
