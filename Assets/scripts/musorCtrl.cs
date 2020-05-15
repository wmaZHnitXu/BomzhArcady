using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musorCtrl : hpBase
{
    public int musorCount;
    public float randomiser;
    public GameObject[] musori;
    [SerializeField]
    private Animator a;
    private propBlastCtrl c;
    private bool dead;
   public override void AddHit(int hit) {
       if (!dead) {
       hp -= hit;
       a.SetTrigger("hit");
       if (hp <= 0) {
           Death();
           }
       }
   }
   public override void Death() {
       for (int i = 0; i != musorCount; i++) {
           c = Instantiate(musori[Random.Range(musori.Length - 1,0)],transform.position,transform.rotation).GetComponent<propBlastCtrl>();
           c.Blast();
       }
        a.SetTrigger("death");
        Destroy(gameObject,1f);
        dead = true;
   }
}
