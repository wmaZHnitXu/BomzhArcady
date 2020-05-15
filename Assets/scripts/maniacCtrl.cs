using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maniacCtrl : npcCtrl
{
    public bool triggered;
    public float lostDistance;
    public float detectionDistance;
    public bool neponyatki;
    [SerializeField]
    private ParticleSystem smoke;
    private Transform particleTrns;
    Coroutine _neponyal;
    protected override void UpdatePlus() { 
        if (toPlayerDistance <= detectionDistance & (right == transform.position.x < characterctrl.me.position.x) & !triggered) {
            triggered = true;
            attackAnim.SetBool("triggered",triggered);
            TriggerToPlayer();
            if (neponyatki) {
                StopCoroutine(_neponyal);
                neponyatki = false;
            }
        }
        if ((triggered & toPlayerDistance >= lostDistance) & !neponyatki) {
            neponyatki = true;
            _neponyal = StartCoroutine(Neponyal());
        }
    }
    protected override void OnEnable() {
        AllInit();
        particleTrns = smoke.transform;
        particleTrns.SetParent(null);
        PlayParticle();
    }

    protected override void IdemSwitch(bool b) {
        if (idet != b) {
            if (hasForwardFacing & b) {
                ToSide();
            }
            foreach (Animator a in forIdem) {
               a.SetBool("idem",b);
            }
            idet = b;
        }
    }
    protected override IEnumerator Neponyal() {
        //neponyatki = true;
        triggered = false;
        destination = reserveDest; //Дописать как он ходит рядом и растворяется
        attack = false;
        reserveDest.position = characterctrl.me.position;
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 5; i++) {
            yield return new WaitForSeconds(2f);
            destination.position = new Vector3(transform.position.x + Random.Range(-10,10),transform.position.y,transform.position.z);
        }
        attackAnim.SetBool("triggered",triggered);
    }
    
    public override void Death() {
        gameObject.SetActive(false);
    }
    void PlayParticle () {
        smoke.Play();
        particleTrns.position = transform.position;
    }
    protected override void OnDisable() {
        PlayParticle();
    }
}
