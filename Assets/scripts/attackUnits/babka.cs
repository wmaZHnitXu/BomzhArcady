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
    [SerializeField] private Transform target;
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float screechDistance;
    [SerializeField] private float offencePeriod;
    [SerializeField] private Animator animator;
    private int HashedFlight;
    private int HashedIdem;
    private int HashedPunch;
    [SerializeField] private int healAmmount = 10;
    new private void Start() {
        base.Start();
        HashedFlight = Animator.StringToHash("flight");
        HashedIdem = Animator.StringToHash("idem");
        HashedPunch = Animator.StringToHash("punch");
        target = characterctrl.me;
        targetHp = characterctrl.it;
        StartCoroutine(Walking());
    }
    private IEnumerator Walking () {
        animator.SetBool(HashedIdem, true);
        while (target != null && Mathf.Abs(target.position.x - transform.position.x) > screechDistance) {
            yield return new WaitForFixedUpdate();
            Right = transform.position.x < target.position.x;
            rb.velocity = new Vector2(Right ? walkingSpeed : -walkingSpeed, rb.velocity.y);
        }
        if (target == null) {
            
        }
        else if (targetHp.hp <= 0) {
            //Тут дроп.
        }
        else {
            StartCoroutine(Screech());
            animator.SetBool(HashedIdem, false);
        }
    }
    private IEnumerator Screech () {
        while (targetHp != null && targetHp.hp > 0) {
            yield return new WaitForSeconds(offencePeriod);
            if (Mathf.Abs(transform.position.x - target.position.x) <= screechDistance) Offence();
            else { StartCoroutine(Walking()); yield break; }
        }
    }
    public override void OnGrab () {
        animator.SetBool(HashedFlight, true);
        hp = 0;
        StopAllCoroutines();
    }
    public override void OnSpit () {
        animator.SetBool(HashedFlight, false);
        StartCoroutine(Walking());
    }
    private void Offence () {
        animator.SetTrigger(HashedPunch);
        foreach (EnemyHp hp in EnemyHp.us) {
            hp.Heal(healAmmount);
        }
    }
}
