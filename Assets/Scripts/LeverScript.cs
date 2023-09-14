using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour
{
    Animator Ani;
    public GameObject[] Spikes;
    bool OnLever = false;
    AudioSource ASource;
    
    // Start is called before the first frame update
    void Start()
    {
        Ani = GetComponentInChildren<Animator>();
        ASource = GetComponent<AudioSource>();
        ASource.volume = GValue.GSoundVolume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (OnLever)
            return;
        if (collision.tag == "PlayerAttack" || collision.tag == "FPlayerAttack")
        {
            ASource.Play();
            Ani.SetTrigger("LeverOn");
            if(UIMgrScript.Inst)
                UIMgrScript.Inst.OffGuideText();
            for(int i=0; i< Spikes.Length; i++)
            {
                Spikes[i].GetComponent<GSpikeScript>().SpikeDown();
            }
            
        }
    }

   
}
