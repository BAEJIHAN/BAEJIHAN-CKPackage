using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualAnimScript : MonoBehaviour
{
    ManualModelScript Pivot;
    // Start is called before the first frame update
    void Start()
    {
        Pivot = transform.parent.gameObject.GetComponent<ManualModelScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Jump2EndFunc()
    {
        Pivot.SetModelState(MODELSTATE.JUMP3);
    }
}
