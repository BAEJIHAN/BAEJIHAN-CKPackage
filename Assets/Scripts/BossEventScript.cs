using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEventScript : MonoBehaviour
{
    StageManagerRoot StageManager;
    // Start is called before the first frame update
    void Start()
    {
        StageManager = FindObjectOfType<StageManagerRoot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StageManager.BossEventOn();
            
        }
    }

}
