using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayerCanv : MonoBehaviour
{
    public Transform destPoint;
    private Transform mytrns;
    public bool smooth;
    void Start () {
        mytrns = gameObject.transform;
    }
    void FixedUpdate()
    {
        if (smooth)
            mytrns.position = Vector3.Lerp(mytrns.position,destPoint.position,0.2f);
        else
            mytrns.position = destPoint.position;
    }
}
