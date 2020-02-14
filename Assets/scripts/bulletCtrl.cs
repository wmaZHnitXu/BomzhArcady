﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletCtrl : MonoBehaviour
{
    private Vector3 v;
    public float speed;
    public Transform shell;
    public Transform bullet;
    public bool forPlayer = true;
    private int targLayer = 12;
    public byte hit = 2;
    public npcCtrl c;
    public bool laser;
    public ChildRbInfo cri;

    void Start () {
        if (c == null) {
            if (!characterctrl.rotation) {
                v.x = -speed;
            }
            else
                v.x = speed;
        }
        else {
            if (c.right) {
                v.x = speed;
            }
            else {
                v.x = -speed;
            }
        }
        if (forPlayer) {
            targLayer = 12;
        }
        else {
            targLayer = 13;
        }
        if (!laser) {
        shell.SetParent(null);
        bullet.SetParent(null);
        }
        Destroy(bullet.gameObject,3f);
        Destroy(shell.gameObject,3f);
        Destroy(gameObject,3.5f);
        cri.loaded = true;
        cri.isLaser = laser;
    }
    void FixedUpdate()
    {
        if (bullet != null)
        bullet.Translate(v);
    }
    public void stack(Collider2D coll) {
        if (coll.gameObject.layer == targLayer) {
            if (forPlayer) {
                characterctrl.health -= hit;
                characterctrl.it.addHit();
                characterctrl.rb.AddForce((characterctrl.me.position - bullet.position).normalized * 0.01f *  hit);
                if (!laser)
                fxHub.GiveMeBlood(bullet.position);
            }
            else {
                //Debug.Log(coll.name);
                if (!laser)
                    coll.GetComponent<hpBase>().addHit(hit,bullet.position);
                else
                    coll.GetComponent<hpBase>().addHit(hit);
            }
            if (!laser) {
                Destroy(bullet.gameObject);
                Destroy(shell.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
