﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class defaultPigeon : hpBase
{
    private float startX;
    [SerializeField] private bool _right = true;
    public bool Right {
        get => _right;
        set {
            if (value != _right) {
                _right = value;
                transform.localScale = new Vector3(_right ? startScale.x : -startScale.x, startScale.y, startScale.z);
            }
        }
    }
    private Vector3 startScale;
    [SerializeField] private float flightSpeed;
    [SerializeField] private bool inFlight;
    public float finalFlightHeight;
    [SerializeField] private float flightSpeedMax;
    [SerializeField] private Vector3 jumpVector;
    [SerializeField] private float acceleration = 0.1f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    private Transform characterTransform;
    [SerializeField] private float criticalDistanceToPlayer = 5;
    private int HashedFlight;
    [SerializeField] private GameObject drop;
    protected new void Start() {
        base.Start();
        startX = transform.position.x;
        characterTransform = characterctrl.me;
        HashedFlight = Animator.StringToHash("flight");
        
        startScale = transform.localScale;
        StartCoroutine(Walking());
        StartCoroutine(ApproximationChecking());
    }
    protected IEnumerator Flight () {
        Vector2 flyingVector = new Vector3(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
        flyingVector.x = Right ? flyingVector.x : -flyingVector.x;
        while (inFlight) {
            rb.velocity = (flyingVector * (flightSpeed = Mathf.Lerp(flightSpeed,flightSpeedMax,acceleration)));
            yield return null;
        }
    }
    protected IEnumerator Walking () {
        while (!inFlight) {
            yield return new WaitForSeconds(0.4f);
            yield return StartCoroutine(Jump());
        }
    }
    protected IEnumerator ApproximationChecking () {
        float distanceToPlayer = 0;
        do {
            distanceToPlayer = Mathf.Abs(characterTransform.position.x - transform.position.x);
            if (distanceToPlayer < criticalDistanceToPlayer * 1.5) Right = characterTransform.position.x < transform.position.x;
            else { 
                var dist = transform.position.x - startX;
                if (Mathf.Abs(dist) > 5) {
                    Right = dist < 0;
                }
            }
            yield return null;
        } while (distanceToPlayer > criticalDistanceToPlayer);
        StartFlight();
    }
    public void StartFlight () {
        animator.SetBool(HashedFlight,true);
        inFlight = true;
        StartCoroutine(Flight());
    }
    protected IEnumerator Jump () {
        rb.AddForce(new Vector3(Right ? jumpVector.x : -jumpVector.x, jumpVector.y, jumpVector.z));
        yield return new WaitForSeconds(0.5f); //Можно усовершенствовать проверкой стояния на земле.
        if (Random.Range(0,1f) > 0.9f && !inFlight) Right = !Right;
    }
    public override void AddHit(int hit) {
        if (hp == 0) return;
        hp -= hit;
        fxHub.me.GimmeParticles(particleType.Blood, transform.position);
        if (hp <= 0) Death();
    }
    public override void Death() {
        hp = 0;
        fxHub.me.GimmeParticles(particleType.SmallSmoke, transform.position);
        StopAllCoroutines();
        Instantiate(drop, transform.position, transform.rotation).GetComponent<Rigidbody2D>().velocity = rb.velocity + Random.insideUnitCircle;
        Destroy(gameObject);
    }
}
