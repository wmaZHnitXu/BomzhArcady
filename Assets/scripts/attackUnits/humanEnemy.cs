using UnityEngine;
using System.Collections;

public class humanEnemy : EnemyHp
{
    [SerializeField] private Transform target;
    [SerializeField] private hpBase targetHp;
    private bool right = true;
    [SerializeField] private float dist;
    [SerializeField] private float reloadTime = 1;
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
    [SerializeField] private Animator animator;
    private Vector3 startScale;
    public float walkingSpeed;
    [SerializeField] private float movingSpeed;
    private static readonly int Idem = Animator.StringToHash("idem");
    private bool landed;
    [SerializeField] private int punch;
    [SerializeField] private bool reloaded;
    private int HashedPunch;
    private int HashedIdem;
    private bool _isWalking;
    public bool isWalking {
        get => _isWalking;
        private set {
            if (value != _isWalking) {
                animator.SetBool(HashedIdem, value);
                _isWalking = value;
            }
        }
    }
    [SerializeField] private bool deadInside;
    
    private void OnEnable()
    {
        movingSpeed = movingSpeed + UnityEngine.Random.Range(0.5f, -0.5f) * movingSpeed;
        gameObject.layer = 13;
        characterctrl.NearNpcs.Add(this);
        if (target == null)
        {
            //Думаю, так быстрее, чем через .transform
            target = characterctrl.me;
            targetHp = characterctrl.it;
        }
    }

    private new void Start()
    {
        base.Start();
        HashedPunch = Animator.StringToHash("punch");
        HashedIdem = Animator.StringToHash("idem");
        if (rb == null)
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }
        startScale = transform.localScale;
        //StartCoroutine(LifeTime()) Эксперимент
    }

    private void FixedUpdate()
    {
        if (dead || deadInside) return;

        if (targetHp != null && targetHp.hp > 0) {
            dist = target.position.x - transform.position.x;
            Right = dist > 0;
            if (isWalking = Mathf.Abs(dist) > 0.5f) rb.velocity = new Vector3(Right ? walkingSpeed : -walkingSpeed ,rb.velocity.y,0);
            else {
                if (Mathf.Abs(rb.velocity.x) <= walkingSpeed) rb.velocity = new Vector3(0 ,rb.velocity.y,0);
                if (reloaded) StartCoroutine(Punch());
            }
        }
        else {
            SetTarget(FindObjectOfType<guardian>());
        }
    }

    private void NobodyWillSee() //???
    {
        landed = true;
    }

    private void OnDisable()
    {
        characterctrl.NearNpcs.Remove(this);
    }
    protected virtual IEnumerator Punch() {
        reloaded = false;
        animator.SetTrigger(HashedPunch);
        yield return new WaitForSeconds(0.5f);
        targetHp.AddHit(punch,transform.position);
        yield return new WaitForSeconds(reloadTime);
        reloaded = true;
    }

    public override void Death()
    {
        StopAllCoroutines();
        rb.AddTorque(UnityEngine.Random.Range(100,-100) * rb.mass);
        rb.AddForce(new Vector2(UnityEngine.Random.Range(100,-100) * rb.mass,UnityEngine.Random.Range(100,-100) * rb.mass));
        gameObject.layer = 15;
        leftBar.AddLine("Гопник издох");
        dead = true;
        timeCtrl.me.enemyCount--;
    }
    
    public void SetTarget (hpBase target) {
        if (target == null) {
            deadInside = true;
            return;
        }
        targetHp = target;
        this.target = target.transform;
    }
}
