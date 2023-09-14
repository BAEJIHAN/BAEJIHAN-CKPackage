using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MonRootScript : MonoBehaviour
{
    [HideInInspector] public Texture PortraitTexture;

    protected GameObject PlayerPivot;
    protected Animator Ani;
    protected SpriteRenderer render;
    protected GameObject Pivot;

    public int Damage;
    protected int MaxHP;
    protected int HP;
        
    protected float PlayerDetectDist;

    protected float DeathVipTime = 5;
    protected float CurDeathVipTime = 0;
    protected float FadeValue = 1;

    [HideInInspector]public bool IsFadeOn=false;

    protected float ObstacleCollisionTime = 0;
    protected float MaxObstacleCollisionTime = 1;
    protected bool IsStuck = false;

    protected MonGroupScript MonGroup;
    protected StageManagerRoot StageManager;

    [HideInInspector]public AudioClip AClip;
    [HideInInspector] public AudioSource ASource;
    protected void Awake()
    {
        ASource = GetComponent<AudioSource>();
        ASource.volume = GValue.GSoundVolume;
    }
    // Start is called before the first frame update
    protected void Start()
    {
        PlayerPivot = GameObject.Find("PlayerPivot");
        Ani = GetComponentInChildren<Animator>();
        render = GetComponentInChildren<SpriteRenderer>();
        StageManager=FindObjectOfType<StageManagerRoot>();
        Pivot = this.gameObject;
        
        if(transform.parent!=null)
            MonGroup = transform.parent.gameObject.GetComponent<MonGroupScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void DeathBip()
    {
        if (IsFadeOn==false)
            return;

        FadeValue -= Time.deltaTime * 0.3f;
        render.color = new Color(1, 1, 1, FadeValue);
        if (FadeValue < 0)
            Destroy(gameObject);
    }

    protected void TakeDamage(int Damage)
    {
        HP -= Damage;
    }

    public virtual void AwakeMon()
    {
        
    }

   

    public void MonHPOne()
    {
        HP = 1;
    }

    public int GetHP()
    {
        return HP;
    }

}
