using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelCatScript : MonoBehaviour
{
    GameObject Model;
    Animator Ani;
    float Speed=6;
    float RunTime = 0;
    bool IsRun = false;
    // Start is called before the first frame update
    void Start()
    {
        Ani = GetComponentInChildren<Animator>();
        Model = GameObject.Find("ManualModel");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (IsRun)
        {
            transform.position += Vector3.left * Time.unscaledDeltaTime * Speed;
            RunTime += Time.unscaledDeltaTime;
            if (RunTime > 3)
                Destroy(gameObject);
        }
        else
        {
            if((Model.transform.position-transform.position).magnitude<0.5f)
            {
                IsRun = true;
                Ani.SetTrigger("Run");
                transform.rotation = Quaternion.Euler(0, 180, 0);
                
            }
        }
    }

    
}
