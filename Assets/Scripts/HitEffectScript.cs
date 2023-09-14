using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        float rotZ = Random.Range(0, 180);
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
        Destroy(gameObject, 0.5f);
    }

    // Update is called once per frame
   
}
