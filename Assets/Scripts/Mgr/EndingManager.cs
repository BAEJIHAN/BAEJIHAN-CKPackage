using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    public Button ToTitleBtn;
    public Button QuitBtn;
    // Start is called before the first frame update
    void Start()
    {
        if(ToTitleBtn != null)
            ToTitleBtn.onClick.AddListener(ToTitleBtnFunc);

        if (QuitBtn != null)
            QuitBtn.onClick.AddListener(QuitBtnFunc);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ToTitleBtnFunc()
    {
        SceneManager.LoadScene("TitleScene");
    }

    void QuitBtnFunc()
    {
        Application.Quit();
    }
}
