using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showName : MonoBehaviour
{
    public Transform parent;
    public Transform player;
    [SerializeField]
    private Animator anim;
    private bool on;
    void Update()
    {
        if (Vector3.Distance(player.position,parent.position) < 3f) {
            if (!on) {
                anim.SetBool("on",true);
                on = true;
            } 
        }
        else {
            if (on) {
                anim.SetBool("on",false);
                on = false;
            }
        }
    }
}
