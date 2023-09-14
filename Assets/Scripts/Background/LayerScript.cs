using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] public float Pivot;
    public float Width;
    [HideInInspector] public LAYERXORDER Order;
    // Start is called before the first frame update
    void Start()
    {
        Pivot = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
