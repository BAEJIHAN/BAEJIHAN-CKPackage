using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpAnimScript : MonoBehaviour
{
    GameObject PlayerPivot;
    ImpScript Pivot;
    Animator Ani;

    //Spear
    public GameObject SpearObj;
    public GameObject SpearPos;
    Vector3 P1, P2, P3, P4;
 

    // Start is called before the first frame update
    private void Awake()
    {
        Pivot = transform.parent.gameObject.GetComponent<ImpScript>();
        Ani = GetComponent<Animator>();
    }
    void Start()
    {

        PlayerPivot = GameObject.Find("PlayerPivot");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AttackPsFunc()
    {
        P1 = SpearPos.transform.position;
        float Ranx = Random.Range(-1.0f, 1.0f);
        float Rany = Random.Range(-1.0f, 1.0f);
        P4 = PlayerPivot.transform.position;
        P4 += new Vector3(Ranx, Rany, 0);

        float MaxSpearHeight = Mathf.Abs(P1.x - P4.x) * 0.2f;
        P2 = Vector3.zero;
        P3 = Vector3.zero;
        P2.y = P1.y + MaxSpearHeight;
        P3.y = P4.y + MaxSpearHeight;
        P2.x = ((P1.x + P4.x) / 2 + P1.x) / 2;
        P3.x = ((P1.x + P4.x) / 2 + P4.x) / 2;
    }
    public void AttackSpawnFunc()
    {
        GameObject tempArrow = Instantiate(SpearObj);
        tempArrow.transform.parent=Pivot.transform.parent;
        tempArrow.transform.position = P1;
        Pivot.AClip = Resources.Load<AudioClip>("Sound/Monster/SpearThrow");
        Pivot.ASource.PlayOneShot(Pivot.AClip);
        tempArrow.GetComponent<SpearScript>().InitSpear(P1, P2, P3, P4);

    }

    public void AttackEndFunc()
    {
        Ani.SetTrigger("Walk");
        Pivot.SetImpState(IMPSTATE.COMBAT);
        Pivot.SetDest();
        Pivot.SetPreAttackNum();
    }

    public void HitEndFunc()
    {
        Ani.SetTrigger("Walk");
        Pivot.SetImpState(IMPSTATE.COMBAT);
        Pivot.SetDest();
        Pivot.SetPreAttackNum();
    }

    public void DeathEndFunc()
    {
        Destroy(Pivot.gameObject);
    }
}
