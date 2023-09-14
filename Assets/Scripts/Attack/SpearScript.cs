using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearScript : MonoBehaviour
{
    float Speed = 9;
    Vector3 P1, P2, P3, P4;
 
    float fvalue = 0;
    float Dist = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float temp = Time.deltaTime / Dist;

        fvalue += temp*Speed;


        if (fvalue >= 1f)
        {
            Destroy(gameObject);
        }


        transform.position = MMath.Lerp(P1, P2, P3, P4, fvalue);
        Vector3 RotPos = MMath.Lerp(P1, P2, P3, P4, fvalue + 0.1f);
        Vector2 Dir = new Vector2(transform.position.x - RotPos.x,
                                    transform.position.y - RotPos.y);

        float Angle = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(Angle, Vector3.forward);
        Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, fvalue);
        transform.rotation = rotation;
    }

    public void InitSpear(Vector3 _P1, Vector3 _P2, Vector3 _P3, Vector4 _P4)
    {
        P1 = _P1;
        P2= _P2;
        P3= _P3;
        P4 = _P4;
        SetInitRot();
        Dist = Mathf.Abs(P1.x - P4.x);
    }

    void SetInitRot()
    {
        Vector3 RotPos = MMath.Lerp(P1, P2, P3, P4, fvalue + 0.1f);
        Vector2 Dir = new Vector2(transform.position.x - RotPos.x,
                                    transform.position.y - RotPos.y);
        float Angle = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(Angle, Vector3.forward);
        transform.rotation = angleAxis;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
