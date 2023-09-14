using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackScript : MonoBehaviour
{
    float FadeStartTime = 3;    
    float FadeColor = 1;
   
    SpriteRenderer Renderer;
    // Start is called before the first frame update
    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        FadeStartTime -= Time.deltaTime;
        if (FadeStartTime <= 0)
        {
           
            FadeColor -= Time.deltaTime * 0.3f;
            Renderer.color = new Color(0, 0, 0, FadeColor);
            if (FadeColor < 0)
                Destroy(gameObject);
        }


    }
}
