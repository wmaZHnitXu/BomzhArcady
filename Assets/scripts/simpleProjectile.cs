using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleProjectile : MonoBehaviour
{
    public int hit;
    void OnCollisionEnter2D (Collision2D col) {
        if (col.gameObject.layer == 13) {
            var hp = col.gameObject.GetComponent<hpBase>();
            hp.AddHit(hit);
        }
        transform.position = new Vector2(-999,-999);
    }
}
