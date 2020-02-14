using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class propBlastCtrl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 forseVector;
    public float multipler;
    void OnEnable()
    {
       
    }
    public void Blast() {
        forseVector = new Vector3();
        forseVector.x = Random.Range(-1f,1f);
        forseVector.y = Random.Range(0.25f,1f);
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.AddForce(forseVector * multipler);
        rb.AddTorque(Random.Range(10f,-10f));
    }
}
