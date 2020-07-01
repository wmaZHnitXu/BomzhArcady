using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guardian : hpBase
{
    public hpBase enemyHp;
    private Transform enemyTransform;
    public bool reloaded;
    public Vector3 targetPos;
    [SerializeField]
    private bool attacking;
    private Vector3 chillPos;
    private Coroutine chillingPosRoutine;
    [SerializeField]
    private float basePosX;
    [SerializeField]
    private bool right;
    private Vector3 startScale;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float triggerDistance;
    public float punchCd;
    public float distanceToTarget;
    public int punch;
    private bool isWalking; //Для анимации
    [SerializeField] private Animator animator;
    private int hashedIdem;
    private int hashedPunch;
    void OnEnable()
    {
        hashedPunch = Animator.StringToHash("punch");
        hashedIdem = Animator.StringToHash("idem");
        startScale = transform.localScale;
        basePosX = transform.position.x;
        reloaded = true;
        Chill();
        StartCoroutine(Lifetime());
    }

    private IEnumerator Lifetime() {
        while (hp > 0) {
            if (enemyHp != null) {
                if (!attacking) {
                    attacking = true;
                    StopCoroutine(chillingPosRoutine);
                }

                targetPos = enemyTransform.position;

                if (distanceToTarget < 0.2f && reloaded) {
                    StartCoroutine(Punch());
                }
                if (enemyHp.hp <= 0) {
                    Chill();
                }
            }
            //Движение к поинту
            if (right != targetPos.x > transform.position.x) {
                    right = targetPos.x > transform.position.x;
                    transform.localScale = right ? startScale : new Vector3(-startScale.x,startScale.y,startScale.z);
                }
                if (walkingCheck((distanceToTarget = Mathf.Abs(targetPos.x - transform.position.x)) > 0.5f)) {
                    rb.velocity = new Vector2(right ? speed : -speed, rb.velocity.y);
            }
            yield return null;
        }
    }
    private IEnumerator ChillingPosUpdate () {
        Debug.Log("drop");
        while (true) {
            targetPos = chillPos = new Vector3(basePosX + Random.Range(-10f,10f),0,0); //Меняй на рандом с привязкой к дефпосикс P.s. Заменил, и че дальше?
            yield return new WaitForSeconds(Random.Range(2f,5f));
        }
    }
    private IEnumerator Searching () {
        chillingPosRoutine = StartCoroutine(ChillingPosUpdate());
        while (enemyHp == null) {
                float dist = triggerDistance; //not bruh anymore
                foreach (hpBase target in characterctrl.NearNpcs) {
                    float newDist = Vector3.SqrMagnitude(target.transform.position - transform.position);
                    if (dist > newDist && target.hp != 0) {
                        dist = newDist;
                        enemyHp = target;
                        enemyTransform = enemyHp.transform;
                    }
               }
            yield return new WaitForSeconds(0.1f);
        }
    }
    private IEnumerator Punch () {
        reloaded = false;
        enemyHp.AddHit(punch);
        animator.SetTrigger(hashedPunch);
        yield return new WaitForSeconds(punchCd);
        reloaded = true;
    }
    public void Chill () {
        enemyHp = null;
        enemyTransform = null;
        StartCoroutine(Searching());
        attacking = false;
    }
    private bool walkingCheck (bool walk) { //АХАХХАХАХАХ
        if (isWalking == walk) return walk;
        animator.SetBool(hashedIdem, walk);
        isWalking = walk;
        return walk;
    }
}
