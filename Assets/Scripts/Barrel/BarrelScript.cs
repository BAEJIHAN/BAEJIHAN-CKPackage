using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject BarrelShardRoot;
    public GameObject Cat;

    bool RumbleDirRight = false;
    float RumbleTime = 0;

    float IdleTime = 0;
    Vector3 OriginPos;

    AudioSource ASource;
    AudioClip AClip;
    void Start()
    {
        ASource = GetComponent<AudioSource>();
        ASource.volume = GValue.GSoundVolume;
        OriginPos = transform.position;
        RumbleTime = Random.Range(0.3f, 0.6f);
    }

    // Update is called once per frame
    void Update()
    {
        if(IdleTime>0)
        {
            IdleTime-=Time.deltaTime;
            RumbleTime = Random.Range(0.3f, 0.6f);
        }
       
        RumbleUpdate();
    }

    void RumbleUpdate()
    {
        if (IdleTime > 0)
            return;

        RumbleTime-= Time.deltaTime;
        if (RumbleTime < 0)
        {
            transform.position = OriginPos;
            IdleTime = Random.Range(4.0f, 7.0f);
            return;
        }

        if(RumbleDirRight)
        {
            transform.position = OriginPos + new Vector3(0.07f, 0, 0);
        }
        else
        {
            transform.position = OriginPos - new Vector3(0.07f, 0, 0);
        }

        RumbleDirRight = !RumbleDirRight;
            


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag=="PlayerAttack" || collision.tag=="FPlayerAttack")
        {
            
            GameObject ShardRootObj=Instantiate(BarrelShardRoot);
            GameObject CatObj=Instantiate(Cat);
            ShardRootObj.transform.position=  transform.position;
            CatObj.transform.position = transform.position;
            Destroy(gameObject);
        }
    }

   
}
