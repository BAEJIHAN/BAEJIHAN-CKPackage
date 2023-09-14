using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleAnimScript : MonoBehaviour
{
    public GameObject MonstetAttackObj;
    AudioSource ASource;
    AudioClip AClip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveAttackFunc()
    {
        MonstetAttackObj.SetActive(true);
        ASource = GetComponent<AudioSource>();
        ASource.volume = GValue.GSoundVolume;
        AClip = Resources.Load<AudioClip>("Sound/Monster/Tentacle");
        ASource.PlayOneShot(AClip);
    }

    public void DeActiveAttackFunc()
    {
        MonstetAttackObj.SetActive(false);
    }
    public void EndFunc()
    {
        Destroy(transform.parent.gameObject);
    }

}
