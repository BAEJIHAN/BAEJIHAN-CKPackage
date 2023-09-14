using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FAttackScript : MonoBehaviour
{
    float ResStamina = 50;
    public GameObject HitEffect;

    AudioSource ASource;
    AudioClip AClip;

    private void Awake()
    {
        ASource = GetComponent<AudioSource>();
        AClip = Resources.Load<AudioClip>("Sound/Monster/HitFAttack");
        ASource.volume = GValue.GSoundVolume;
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject.tag=="Monster" || collision.gameObject.tag == "Boss" || collision.tag == "FlyMon")
        {
            ASource.PlayOneShot(AClip);
            GameObject temp = GameObject.Find("PlayerPivot");
            temp.GetComponent<PlayerScript>().AddStamina(ResStamina);

            GameObject EffectObj = Instantiate(HitEffect);
            EffectObj.transform.position = collision.gameObject.transform.position;
            EffectObj.transform.localScale = collision.transform.localScale;
        }
    }
}
