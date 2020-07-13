using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : hpBase
{
    [SerializeField] private bool _right = true;
    [SerializeField] private float flyingSpeed;
    [SerializeField] private Transform target;
    [SerializeField] private nemosh targetNem;
    private Vector3 startScale;
    private Vector3 awayVector;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private float catchDistance = 5;
    private int HashedFlight;
    [SerializeField] private GameObject drop;
    [SerializeField] private bool grabbed;
    public bool Right {
        get => _right;
        set {
            if (value != _right) {
                transform.localScale = new Vector3(value ? startScale.x : -startScale.x, startScale.y, startScale.z);
                _right = value;
            }
        }
    } 
    private new void Start () {
        base.Start();
        HashedFlight = Animator.StringToHash("flight");
        startScale = transform.localScale;
        StartCoroutine(Chilling());
    }
    private void Grab () {
        target.SetParent(transform);
        Rigidbody2D targRb = null;
        if (targetNem != null) targetNem.OnGrab();
        if ((targRb = target.GetComponent<Rigidbody2D>()) != null) targRb.simulated = false;
        StartCoroutine(FlyingAway());
        grabbed = true;
    }
    private void Spit () {
        target.SetParent(null);
        Rigidbody2D targRb = null;
        if ((targRb = target.GetComponent<Rigidbody2D>()) != null) targRb.simulated = true;
        if (targetNem != null) targetNem.OnSpit();
    }
    private IEnumerator Chilling() {
        Transform arkadyFast = characterctrl.me;
        while (target == null) {
            //Повороты живые
            if (Mathf.Abs(arkadyFast.position.x - transform.position.x) < 2) {
                Right = arkadyFast.position.x > transform.position.x;
            }
            else Right = Random.Range(0f,1f) > 0.95f ? !Right : Right;
            //Поиск цели
            foreach(Transform itemTrns in itemCtrl.itemTransforms) {
                if (catchDistance >= (itemTrns.position - transform.position).sqrMagnitude) {
                    target = itemTrns;
                    StartCoroutine(FlyingToTarget());
                    yield break;
                }
            }
            foreach (nemoshWithTransform nem in nemosh.nemoshi) {
                if (catchDistance >= (nem.transform.position - transform.position).sqrMagnitude) {
                    target = nem.transform;
                    targetNem = nem.nemosh;
                    StartCoroutine(FlyingToTarget());
                    yield break;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    private IEnumerator FlyingToTarget () {
        Right = target.position.x > transform.position.x;
        animator.SetBool(HashedFlight, true);
        while ((transform.position - target.position).sqrMagnitude > 0.5f) {
            rb.velocity = ((target.position - transform.position).normalized * flyingSpeed);
            yield return new WaitForFixedUpdate();
        }
        Grab();
    }
    public override void AddHit(int hit) {
        if (hp == 0) return;
        hp -= hit;
        fxHub.me.GimmeParticles(particleType.Blood, transform.position);
        if (hp <= 0) Death();
    }
    public override void Death() {
        hp = 0;
        StopAllCoroutines();
        if (grabbed) Spit();
        animator.SetBool(HashedFlight, false);
        rb.gravityScale = 1;
        var dropInstance = Instantiate(drop, transform.position, transform.rotation);
        fxHub.me.GimmeParticles(particleType.SmallSmoke, transform.position);
        dropInstance.GetComponent<Rigidbody2D>().velocity = rb.velocity + Random.insideUnitCircle;
        rb.AddTorque(Random.Range(-1,1f));
        Destroy(gameObject);
    }
    private IEnumerator FlyingAway () {
        animator.SetBool(HashedFlight, true);
        awayVector = rb.velocity.normalized * flyingSpeed;
        awayVector.y = Mathf.Abs(awayVector.y);
        Right = awayVector.x > 0;
        while (transform.position.y < 100) {
            rb.AddForce(awayVector);
            yield return new WaitForFixedUpdate();
        }
    }
}
