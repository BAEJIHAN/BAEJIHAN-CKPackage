using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSpikeScript : MonoBehaviour
{
    Animator Ani;
    // Start is called before the first frame update
    void Start()
    {
        Ani = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpikeDown()
    {
        Ani.SetTrigger("SpikeDown");
    }

    public void SpikeUp()
    {
        Ani.SetTrigger("SpikeUp");
    }

    void SpikeDownEndFunc()
    {
        BoxCollider2D temp = GetComponentInChildren<BoxCollider2D>();
        if(temp)
            temp.enabled = false;
    }

    void SpikeUpEndFunc()
    {
        BoxCollider2D temp = GetComponentInChildren<BoxCollider2D>();
        if (temp)
            temp.enabled = true;

    }
}
