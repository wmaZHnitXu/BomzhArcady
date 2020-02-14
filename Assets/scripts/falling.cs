using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class falling : MonoBehaviour
{
    private Vector3 yey = new Vector3(0,-0.1f,0);
    void Update()
    {
        transform.position = transform.position + yey;
    }
}
