using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carNpcCtrl : hpBase
{
    [SerializeField]
    protected Rigidbody2D rb;
    public Vector3 destination;
    [SerializeField]
    protected bool right;
    [SerializeField]
    protected SpriteRenderer seats;
    [SerializeField]
    protected bool ai;
    public float maxSpeed;
    protected float speed;
    public float dist;
    protected Vector3 startScale;
    protected Vector2 forceMaster;
    [SerializeField]
    protected Transform[] wheels;
    protected bool needSideChange;
    [SerializeField]
    protected Transform[] doors;
    [SerializeField]
    protected GameObject[] npcs;
    [SerializeField]
    protected bool ejected;
    public GameObject deadBody;
    public Rigidbody2D[] deadRbs;
    protected void OnEnable()
    {
        startScale = transform.localScale;    
    }

    protected virtual void FixedUpdate() //Нужна коррекция, писал в полуотрубленном состоянии
    {
        if (ai) {
            dist = transform.position.x - destination.x;
            if (Mathf.Abs(dist) > 8f) {
                if (dist < 0 & !right) {
                    needSideChange = true;
                    if (speed < 0.05f) {
                        right = true;
                        needSideChange = false;
                        transform.localScale = startScale;
                    }
                }
                if (dist > 0 & right) {
                    needSideChange = true;
                    if (speed < 0.05f) {
                        right = false;
                        needSideChange = false;
                        transform.localScale = new Vector3(-startScale.x,startScale.y,startScale.z);
                    }
                }
            }
            if (Mathf.Abs(dist) > 8f & !needSideChange) {
                speed = Mathf.Lerp(speed,maxSpeed,0.03f);
            }
            else {
                speed = Mathf.Lerp(speed,0,0.2f);
                if (speed < 0.01f)
                    if (!ejected) {
                        StartCoroutine(DoEjection());
                        ejected = true;
                    }
            }
            forceMaster.x = right ? speed : -speed;
            rb.AddForce(forceMaster);
            wheels[0].Rotate(0,0,rb.velocity.magnitude);
            wheels[1].Rotate(0,0,rb.velocity.magnitude);
        }
    }
    protected IEnumerator DoEjection () {
        for (int i = 0; i != npcs.Length; i++) {
            yield return new WaitForSeconds(0.1f);
            Instantiate(npcs[Random.Range(0,npcs.Length-1)],doors[i].position,doors[i].rotation);
        }
    }
    public override void AddHit(int hit) {
        hp -= hit;
        if (hp <= 0) {
            Death();
        }
    }
    public override void Death() {
        deadBody.SetActive(true);
        deadBody.transform.SetParent(null);
        fxHub.me.ShakeMe(1);
        foreach (Rigidbody2D rb2d in deadRbs) {
            rb2d.velocity += rb.velocity;
        }
        Destroy(gameObject);
    }
}
