using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    public GameObject HitEffect;
    AudioSource ASource;
    AudioClip AClip;
    private void Awake()
    {
        ASource = GetComponent<AudioSource>();
        AClip = Resources.Load<AudioClip>("Sound/Monster/HitAttack");
        ASource.volume = GValue.GSoundVolume;
    }
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag=="Monster" || collision.tag=="Boss" || collision.tag == "FlyMon")
        {
            
            ASource.PlayOneShot(AClip);
            GameObject EffectObj = Instantiate(HitEffect);
            EffectObj.transform.position = collision.gameObject.transform.position;
            EffectObj.transform.localScale = collision.transform.localScale;
        }
        
        
    }
}
