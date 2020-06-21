using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildRbInfo : MonoBehaviour
{
    public bulletCtrl b;
    public bool loaded;
    public bool isLaser;
    void OnTriggerEnter2D(Collider2D coll) {
        if (loaded) {
        b.Stack(coll);
        }
    }
    public void OnTriggerExit2D (Collider2D coll) {
        /* if (loaded) {
        b.stack(coll);
        }*/
    }
    public void OnTriggerStay2D (Collider2D coll) {
        if (loaded) {
            b.Stack(coll);
        } 
    }
}
