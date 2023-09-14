using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimScript : MonoBehaviour
{
    CatScript Pivot;
    Animator Ani;

    // Start is called before the first frame update
    void Start()
    {
        Pivot = transform.parent.gameObject.GetComponent<CatScript>();
        Ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StretchingEndFunc()
    {
        Pivot.SelAct();
    }

    public void MeowEndFunc()
    {
        Pivot.SelAct();
    }
}
