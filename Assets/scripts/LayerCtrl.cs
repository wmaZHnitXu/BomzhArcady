using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerCtrl : MonoBehaviour
{
    public Transform camTrns;
    private Vector3 startPos;
    private Vector3 startCamPos;
    private Vector3 newPos;
    public float slowness;
    void Start()
    {
        startCamPos = camTrns.position;
        startPos = transform.position;
    }

    void Update()
    {
        newPos = ((camTrns.position - startCamPos) / slowness + startPos);
        transform.position = newPos;
    }
}
