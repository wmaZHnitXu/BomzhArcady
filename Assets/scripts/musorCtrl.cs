using UnityEngine;
using UnityEngine.Events;

public class musorCtrl : hpBase
{
    public int musorCount;
    public float randomiser;
    public GameObject[] musori;
    [SerializeField]
    private Animator a;
    private propBlastCtrl c;
    private bool dead;
    public UnityAction deathEvent;
    private void OnEnable() {
        characterctrl.NearNpcs.Add(this);
    }
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
        if (deathEvent != null)
            deathEvent();    
   }
   private void OnDestroy () {
       characterctrl.NearNpcs.Remove(this);
   }
}
