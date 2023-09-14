using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelShardScript : MonoBehaviour
{
    public Sprite[] Sprites = new Sprite[12];
    // Start is called before the first frame update
    private void Awake()
    {
        int idx=Random.Range(0,Sprites.Length);
        GetComponent<SpriteRenderer>().sprite = Sprites[idx];
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
