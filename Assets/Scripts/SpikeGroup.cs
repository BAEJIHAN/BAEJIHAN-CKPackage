using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeGroup : MonoBehaviour
{
    GSpikeScript[] Spikes;
    // Start is called before the first frame update
    void Start()
    {
        Spikes = GetComponentsInChildren<GSpikeScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpikesUp()
    {
        for(int i=0; i<Spikes.Length; i++)
        {
            Spikes[i].SpikeUp();
        }
    }

    public void SpikesDown()
    {
        for (int i = 0; i < Spikes.Length; i++)
        {
            Spikes[i].SpikeDown();
        }
    }


}
