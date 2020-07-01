using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : hpBase
{
    [SerializeField] private bool _right = true;
    [SerializeField] private float flyingSpeed;
    [SerializeField] private Transform target;
    private Vector3 startScale;
    private Vector3 awayVector;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    private int HashedFlight;
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
        if ((targRb = target.GetComponent<Rigidbody2D>()) != null) targRb.simulated = false;
        StartCoroutine(FlyingAway());
    }
    private IEnumerator Chilling() {
        yield return new WaitForSeconds(5f);
        StartCoroutine(FlyingToTarget());
    }
    private IEnumerator FlyingToTarget () {
        animator.SetBool(HashedFlight, true);
        while ((transform.position - target.position).sqrMagnitude > 0.3f) {
            rb.AddForce((target.position - transform.position).normalized * flyingSpeed);
            yield return new WaitForFixedUpdate();
        }
        Grab();
    }
    private IEnumerator FlyingAway () {
        animator.SetBool(HashedFlight, true);
        awayVector = rb.velocity.normalized * flyingSpeed;
        awayVector.y = Mathf.Abs(awayVector.y);
        while (transform.position.y < 100) {
            rb.AddForce(awayVector);
            yield return new WaitForFixedUpdate();
        }
    }
}
