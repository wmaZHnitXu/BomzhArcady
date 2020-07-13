using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : hpBase
{
    public bool dead;
    [SerializeField] private float statsOffset = 1;
    public static List<EnemyHp> us = new List<EnemyHp>();
    [SerializeField] protected Rigidbody2D rb; 
    protected void Awake () {
        us.Add(this);
    }
    private void OnEnable() {
        //characterctrl.it.allEnemys.Add(this);
        dead = false;
    }
    protected void OnDestroy () {
        us.Remove(this);
    }
    public override void AddHit(int hit)
    {
        hp -= hit;
        hp = hp > 0 ? hp : 0;
        fxHub.GiveMeBlood(new Vector3(transform.position.x,transform.position.y+statsOffset,0));
        enemyStats.me.ShowNpcStats(this);
        if (hp == 0 & !dead)
        {
            Death();
        }
    }
    public override void AddHit(int hit, Vector3 hitpos) {
        AddHit(hit);
    }
    public override void Death() {
        StopAllCoroutines();
        rb.AddTorque(UnityEngine.Random.Range(100,-100) * rb.mass);
        rb.AddForce(new Vector2(UnityEngine.Random.Range(100,-100) * rb.mass,UnityEngine.Random.Range(100,-100) * rb.mass));
        gameObject.layer = 15;
        leftBar.AddLine("Псина скончалась");
        dead = true;
        timeCtrl.me.enemyCount--;
    }
}
