using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeScript : MonoBehaviour
{
    public GameObject ShardRoot;
    // Start is called before the first frame update
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyBari()
    {
        GameObject tempObj = Instantiate(ShardRoot);
        tempObj.transform.position = transform.position;
        Destroy(gameObject);
    }
   
}
