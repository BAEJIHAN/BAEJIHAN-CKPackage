using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class HPUPTextScript : MonoBehaviour
{
    UnityEngine.Color TextColor;
    Text HPText;
    float Speed=100;

    private void Awake()
    {
        HPText = GetComponentInChildren<Text>();    
    }
    private void Update()
    {
        MoveUpdate();

        FadeUpdate();
    }

    void MoveUpdate()
    {
        Vector3 tempV = HPText.transform.position;
        tempV.y += Time.deltaTime * Speed;
        HPText.transform.position = tempV;
    }

    void FadeUpdate()
    {
        TextColor = HPText.color;
        TextColor.a -= Time.deltaTime * 0.75f;
        HPText.color = TextColor;
        if (TextColor.a < 0)
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    public void Init(Vector3 WSpawnPos, Transform PCanvas)
    {

        Vector3 Pos = new Vector3(WSpawnPos.x, WSpawnPos.y, WSpawnPos.z);
        transform.SetParent(PCanvas, false);            

       
        RectTransform CanvasRect = PCanvas.GetComponent<RectTransform>();
        Vector2 ScreenPos = Camera.main.WorldToViewportPoint(Pos);

        Vector2 WScPos = Vector2.zero;

        WScPos.x = (ScreenPos.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f);
        WScPos.y = (ScreenPos.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f);
       
        //a_CanvasRect.sizeDelta.x UI 기준의 화면의 크기 1280 * 720
        this.GetComponent<RectTransform>().anchoredPosition = WScPos;       
    }
}
