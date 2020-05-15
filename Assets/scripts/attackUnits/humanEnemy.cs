using UnityEngine;

public class humanEnemy : hpBase
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
        fxHub.GiveMeBlood(transform.position);
        enemyStats.me.ShowNpcStats(this);
        if (hp <= 0 & !dead)
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

    private void Start()
    {
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

        //StartCoroutine(LifeTime());
    }

    private void FixedUpdate()
    {
        
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
        leftBar.AddLine("Гопник издох");
        dead = true;
    }
}
