using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleSceneManagerScript : MonoBehaviour
{
    public GameObject DebugCanvas;
    public Text DebugText;
    public GameObject Player;
    bool IsDebugOn = false;

    static public SampleSceneManagerScript Inst;
    private void Awake()
    {
        Inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            IsDebugOn = !IsDebugOn;
            DebugCanvas.gameObject.SetActive(IsDebugOn);
            

        }
    }

    public void SetDebugText(string text)
    {
        DebugText.text = text;
    }
}
