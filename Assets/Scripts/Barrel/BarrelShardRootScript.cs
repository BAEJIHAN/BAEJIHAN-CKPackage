using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelShardRootScript : MonoBehaviour
{
    public GameObject BarrelShard;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<10; i++)
        {
            GameObject temp=Instantiate(BarrelShard);
            Vector3 tempV=transform.position;
            tempV.x += Random.Range(-1.0f, 1.0f);
            tempV.y += Random.Range(-0.5f, 0.5f);
            temp.transform.position = tempV;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
