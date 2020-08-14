using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class witch : bossBase
{
    private Coroutine runningRoutine;
    [SerializeField] private float routinesTime = 20;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform target;
    [SerializeField] private hpBase targetHp;
    [SerializeField] private float groundAttackDistance = 2f;
    [SerializeField] private float speed;
    [SerializeField] private Transform groundSpikeTransform;
    [SerializeField] private ParticleSystem groundSpike;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool onAir;
    [SerializeField] private ParticleSystem airCharge;
    [SerializeField] private ParticleSystem crowStorm;
    [SerializeField] private Transform crowStormTransform;
    private float DamageMultipler = 1;
    private Color startLightColor;
    [SerializeField] private Color myLightColor;
    [SerializeField] private AudioSource source;
    protected new void Start () {
        base.Start();
        startLightColor = timeCtrl.me.sunLight.color;
        timeCtrl.me.sunLight.color = myLightColor;
        fxHub.me.ShakeMe(10);
        targetHp = characterctrl.it;
        target = characterctrl.me;
        StartCoroutine(Lifetime());
    }
    private IEnumerator Lifetime () {
        while (true) {
            runningRoutine = StartCoroutine(OnGround());
            yield return new WaitForSeconds(routinesTime + Random.Range(0, 5));
            StopCoroutine(runningRoutine);
            yield return runningRoutine = StartCoroutine(OnAir());
        }
    }
    private IEnumerator OnGround () {
        animator.SetBool("idem", true);
        animator.SetBool("letim", false);
        if (onAir) {
            rb.gravityScale = 1;
            onAir = false;
            yield return new WaitForSeconds(1f);
        }
        while (true) {
            Right = target.position.x > transform.position.x;
            if (Mathf.Abs(target.position.x - transform.position.x) > groundAttackDistance) {
                rb.velocity = new Vector3(Right ? speed : -speed, 0, 0);
            }
            else {
                yield return StartCoroutine(GroundAttack());
            }
            yield return new WaitForFixedUpdate();

        }
    }
    private IEnumerator GroundAttack () {
        animator.SetBool("idem", false);
        animator.SetTrigger("groundAttack");
        yield return new WaitForSeconds(0.5f);
        groundSpikeTransform.position = new Vector3(target.transform.position.x, -2, 0);
        groundSpike.Play();
        yield return new WaitForSeconds(0.2f);
        fxHub.me.GimmeParticles(particleType.SmallSmoke, target.position);
        targetHp.AddHit(15);
        yield return new WaitForSeconds(1f);
        if (!onAir) animator.SetBool("idem", true);
    }
    private IEnumerator OnAir () {
        onAir = true;
        animator.SetBool("idem", false);
        animator.SetBool("letim", true);
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        while (transform.position.y < 2.6f) {
            yield return new WaitForFixedUpdate();
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 2.7f, transform.position.z), 0.1f);
        }
        airCharge.Play();
        DamageMultipler = 2;
        yield return new WaitForSeconds(airCharge.main.duration);
        DamageMultipler = 1;
        for (int i = 0; i < 3; i++) { //airattacks
            yield return StartCoroutine(AirAttack());
        }
    }
    private IEnumerator AirAttack () {
        Debug.Log("airAttack");
        animator.SetTrigger("airAttack");
        Right = target.position.x > transform.position.x;
        yield return new WaitForSeconds(0.5f);
        crowStormTransform.position = target.position;
        crowStormTransform.localScale = new Vector3(Random.Range(0,1f) > 0.5f ? 1 : -1, 1, 1);
        crowStorm.Play();
        yield return new WaitForSeconds(5f);
    }
    public override void Death() {
        base.Death();
        airCharge.Stop();
        StopAllCoroutines();
        timeCtrl.me.sunLight.color = startLightColor;
    }
    public override void AddHit (int hit) {
        hit = (int)(DamageMultipler * hit);
        base.AddHit(hit);
    }
}
