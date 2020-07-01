using System.Collections;
using UnityEngine;
public class Dog : hpBase
{
    private Transform target;
    private hpBase targetHp;
    private bool right = true;
    [SerializeField] private float dist;
    private Rigidbody2D rb;

    private bool Right
    {
        get => right;
        set
        {
            if (Right != value)
            {
                transform.localScale = new Vector3(value ? startScale.x : -startScale.x,startScale.y,startScale.z);
            }
            right = value;
        }
    }

    private bool walk;
    [SerializeField] private Animator legs;
    private bool dead;
    private Vector3 startScale;
    [SerializeField] private float movingSpeed;
    private static readonly int Idem = Animator.StringToHash("idem");
    private bool landed;
    [SerializeField] private int punch;
    

    public int GetHp()
    {
        return hp;
    }

    public override void AddHit(int hit)
    {
        hp -= hit;
        hp = hp > 0 ? hp : 0;
        fxHub.GiveMeBlood(transform.position);
        enemyStats.me.ShowNpcStats(this);
        if (hp == 0 & !dead)
        {
            Death();
        }
    }

    private void OnEnable()
    {
        movingSpeed = movingSpeed + UnityEngine.Random.Range(0.5f, -0.5f) * movingSpeed;
        gameObject.layer = 13;
        characterctrl.NearNpcs.Add(this);
    }

    private new void Start()
    {
        base.Start();
        if (rb == null)
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }
        startScale = transform.localScale;
        if (target == null)
        {
            target = characterctrl.me;
            targetHp = characterctrl.it;
        }

        StartCoroutine(LifeTime());
    }

    private IEnumerator LifeTime()
    {
        while (!dead || targetHp.hp > 0)
        {
            dist = (target.position - transform.position).SquareDistance();
            var position = transform.position;
            Right = target.position.x > position.x;
            walk = dist > 3;
            legs.speed = Mathf.Abs(rb.velocity.x * 0.3f);
            if (walk)
            {
                rb.velocity = new Vector3(Right ? movingSpeed : -movingSpeed, rb.velocity.y, 0); 
                transform.position = position;
            }
            else
            {
                var jumpVector = (target.position - transform.position);
                legs.speed = 0;
                landed = false;
                jumpVector.y += 2;
                jumpVector *= (120 * rb.mass);
                rb.AddForce(jumpVector);
                Invoke(nameof(NobodyWillSee),1f);
                while ((transform.position - target.position).sqrMagnitude > 1f & !landed)
                {
                    Right = target.position.x > position.x;
                    yield return new WaitForFixedUpdate();
                }
                if (!landed)
                {
                    target.GetComponent<Rigidbody2D>().velocity = rb.velocity;
                    rb.velocity = Vector2.zero;
                    Debug.Log(rb.velocity.magnitude);
                    targetHp.AddHit(punch);
                }

                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private void NobodyWillSee()
    {
        landed = true;
    }

    private void OnDisable()
    {
        characterctrl.NearNpcs.Remove(this);
    }

    public override void Death()
    {
        StopAllCoroutines();
        rb.AddTorque(UnityEngine.Random.Range(100,-100) * rb.mass);
        rb.AddForce(new Vector2(UnityEngine.Random.Range(100,-100) * rb.mass,UnityEngine.Random.Range(100,-100) * rb.mass));
        gameObject.layer = 15;
        leftBar.AddLine("Псина скончалась");
        dead = true;
    }
}
