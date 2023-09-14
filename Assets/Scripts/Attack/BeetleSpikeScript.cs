using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleSpikeScript : MonoBehaviour
{

    float Speed = 6;
    float CurDist = 0;
    float MaxDist = 15;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CurDist += Speed * Time.deltaTime;

        transform.position += -transform.right * Speed * Time.deltaTime;

        if(CurDist>MaxDist)
        {
            Destroy(gameObject);
        }
    }
}
