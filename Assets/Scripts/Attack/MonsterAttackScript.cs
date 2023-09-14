using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int Damage;
    public GameObject HitEffect;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag=="Player")
        {
            
            GameObject tempPlayer= collision.gameObject;
            if(PLAYERSTATE.SLIDE!=tempPlayer.GetComponent<PlayerScript>().GetState())
            {
                GameObject EffectObj = Instantiate(HitEffect);
                EffectObj.transform.position = collision.gameObject.transform.position;
                
            }
            
        }
       

    }
}
