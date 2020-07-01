using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class babka : nemosh
{
    enum babkaState {
        screeching,
        walking,
        flying
    }
    [SerializeField] private babkaState currentState;
    [SerializeField] private bool isWalking;
    private hpBase targetHp;
    private Transform target;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float screechDistance;
    [SerializeField] private float offencePeriod;
    private IEnumerator Walking () {
        Right = transform.position.x < target.position.x;
        while (target != null && Mathf.Abs(target.position.x - transform.position.x) > screechDistance) {
            yield return new WaitForFixedUpdate();
        }
        if (target == null) {
            
        }
        else {
            StartCoroutine(Screech());
        }
    }
    private IEnumerator Screech () {
        while (targetHp != null && targetHp.hp > 0) {
            yield return new WaitForSeconds(Random.Range(offencePeriod,offencePeriod+1));
            Offence();
        }
    }
    public override void OnGrab () {

    }
    public override void OnSpit () {

    }
    private void Offence () {

    }
}
