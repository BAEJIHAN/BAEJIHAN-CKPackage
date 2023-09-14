using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class MonGroupScript : MonoBehaviour
{  
   
    MonRootScript[] Mons;
   
    bool IsAwaken = false;
    // Start is called before the first frame update
    void Start()
    {
        Mons=GetComponentsInChildren<MonRootScript>(); 
             
    }

    // Update is called once per frame
    void Update()
    {
        
    }

 

    public void AwakeMons()
    {
        if (IsAwaken == true)
            return;

        IsAwaken=false;

        for(int i=0; i<Mons.Length; i++) 
        {
            Mons[i].AwakeMon();
        }
    }
}
