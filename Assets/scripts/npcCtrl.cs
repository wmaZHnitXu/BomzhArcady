using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class npcCtrl : hpBase, IHp
{
    public Rigidbody2D rb;
    public float speed;
    public bool mstit;
    protected Animator anim; //Для форвардфейсинга
    public bool idet;
    public Transform destination;
    [SerializeField]
    protected Vector2 direction;
    public bool right;
    protected Vector3 scale;
    protected float scaleFactor;
    [SerializeField]
    protected float dist;
    public Animator[] forIdem;
    public bool attack;
    public bool realisticPunchForce;
    public bool reloaded;
    public bool run;
    public float reloadTime;
    public byte punch;
    public Animator attackAnim;
    public Transform kickPos;
    public bool canIdti;
    public Animator hitAnim;
    public ParticleSystem hitP;
    public bool forward;
    public bool hasForwardFacing;
    protected bool stoped;
    public float criticalDist = 1.5f;
    [SerializeField]
    protected GameObject[] bodys; //0 - сторона 1 - вперед
    [SerializeField]
    protected Transform reserveDest;
    [SerializeField]
    protected float speedMultipl = 1f;
    protected float plusDestination;
    public bool finalDestination = true;
    public float toPlayerDistance;
    protected Transform playerTrns;
    protected bool addedRoNearList;
    public int myLayer;
    public GameObject[] lootSet;
    public byte courage;
    propBlastCtrl c;
    public bool dead;
    private bool tikaet;
    public byte wlPlus = 5;
    private UnityAction detachMe;
    public bool bornByScript;
    public static readonly List<npcCtrl> AllNpcs = new List<npcCtrl>();
    private Transform myCity;
    [SerializeField] private float maxCityDistance = 50;

    private static readonly int Attack = Animator.StringToHash("attack");
    private static readonly int Def = Animator.StringToHash("def");
    private static readonly int Hit = Animator.StringToHash("hit");
    private static readonly int Idem = Animator.StringToHash("idem");
    public int GetHp()
    {
        return hp;
    }

    //Все настолько нанотеч, что у меня есть пул для нпс, но нет пула для пуль.
    protected void Awake () { //Не допускать мертворождение(слой меняется во время смерти)
        timeCtrl.me.remindAboutNight += NigthAlarm;
        myLayer = gameObject.layer;
        scale = transform.localScale;
        detachMe += DetachTarget;
        scaleFactor = scale.x;
        if (reserveDest == null)
            reserveDest = new GameObject(gameObject.name + "_destRes").transform;
        gameObject.name += masterPool.howManyNpcWeHave.ToString();
        if (playerTrns == null) {
            playerTrns = characterctrl.me;
        }
        AllNpcs.Add(this);
    }
    protected virtual void OnEnable()
    {
        AllInit();
    }

    protected void FixedUpdate()
    {
        if (canIdti) {
            dist = destination.position.x - transform.position.x;
            toPlayerDistance = Math.Abs(transform.position.x - playerTrns.position.x);
            if (toPlayerDistance > (!bornByScript ? 50 : 200)) { //Возвращаем в пулл, дабы не перенаселять мир
                SilentDeath();
            }
            if (dist > 0) {
                if (!right) {
                    right = true;
                    scale.x = scaleFactor;
                    transform.localScale = scale;
                }
                direction.x = 1;
                IdemSwitch(true);
            }
            if (dist < 0) {
                if (right) {
                    right = false;
                    scale.x = -scaleFactor;
                    transform.localScale = scale;
                }
                direction.x = -1;
                IdemSwitch(true);
        
            }
            if (Mathf.Abs(dist) < criticalDist + plusDestination) {
                direction.x = 0;
                IdemSwitch(false);
                if (attack) {
                    if (!characterctrl.it.dead && Mathf.Abs(destination.position.x - myCity.position.x) < maxCityDistance) {
                        Attacking();
                    }
                    else
                    {
                        attack = false;
                        destination = reserveDest;
                    }
                }
                else {
                    if (!finalDestination)
                        reserveDest.position = new Vector3(transform.position.x + UnityEngine.Random.Range(30f,-30f),transform.position.y,transform.position.z);
                        if (Mathf.Abs(reserveDest.position.x - myCity.position.x) > maxCityDistance) 
                            reserveDest.position = new Vector3(myCity.position.x + UnityEngine.Random.Range(30f,-30f),transform.position.y,transform.position.z);
                }
            }
            if (!stoped)
                rb.AddForce(direction * (speed * speedMultipl));
        }
        if (toPlayerDistance < 10f) {
            if (!addedRoNearList) {
                characterctrl.NearNpcs.Add(this);
                if (characterctrl.wantedLvl >= 15) {
                    PlayerPizdesTvorit();
                } 
                addedRoNearList = true;
            }
        }
        else {
            if (addedRoNearList) {
                characterctrl.NearNpcs.Remove(this);
                addedRoNearList = false;
            }
        }
        UpdatePlus();
    }
    protected virtual void IdemSwitch (bool b) { //Анимация ходьбы вкл/выкл
        if (idet == b) return;
        if (hasForwardFacing & b) {
            ToSide();
        }
        foreach (var a in forIdem) {
            a.SetBool(Idem,b);
        }
        idet = b;
    }
    public override void AddHit(int hit) { //Получение урона.
        hp -= hit;
        fxHub.me.EjectHitText(hit,transform.position);
        if (hp <= -100) {
            fxHub.GiveMeBloodyExplosion(transform.position);
            SilentDeath();
        }
        rb.AddForce((transform.position - characterctrl.me.position).normalized * (10 * characterctrl.meleForce));
        StartCoroutine(Stoping());
        if (characterctrl.meleForce > 50) {
            StartCoroutine(Stun());
        }
        if (hitP != null)
            hitP.Play();
        if (courage > characterctrl.wantedLvl) {
            TriggerToPlayer();
        }
        else {
            Tikaem();
        }
        if (hitAnim!=null) hitAnim.SetTrigger(Hit);
        if (hp <= 0) {
            Death();
        }
    }
    public override void AddHit(int hit, Vector3 punchPos) { //Перегрузка метода сверху, благодаря которй нпс может отлетать в нужном направлении.
        AddHit(hit);
        rb.AddForce((transform.position - punchPos).normalized * (hit * 30));
    }
    public virtual void Attacking () { //Этот и последующие три метода отвечают за атаку, и там все страшно.
        Debug.Log("Я бью ребенка (C) " + gameObject.name);
        if (reloaded) {
            reloaded = false;
            attackAnim.SetTrigger(Attack);
                foreach (Animator a in forIdem) {
                    a.SetTrigger(Def);
                }
            StartCoroutine(Puncher());
        }
    }
    protected IEnumerator Reloading () {
        canIdti = false;
        yield return new WaitForSeconds(0.9f);
        canIdti = true;
        yield return new WaitForSeconds(reloadTime/4 * 3);
        reloaded = true;
    }
    public IEnumerator Puncher() {
        yield return new WaitForSeconds(0.2f);
        if (!stoped) {
            characterctrl.it.AddHit(punch);
            if (kickPos != null)
                characterctrl.rb.AddForce(realisticPunchForce ? (destination.position - transform.position).normalized * 10 * punch : new Vector3((destination.position.x - transform.position.x > 0 ? punch : -punch) * 100 ,0,0));
            Debug.Log("punch");
        }
        StartCoroutine(Reloading());
    }
    public override void Death() {
        if (!dead) {
            base.Death();
            /* */gameObject.layer = 15; //dead
            Debug.Log("death + " + gameObject.name);
            rb.freezeRotation = false;
            rb.AddTorque(UnityEngine.Random.Range(200,-200));
            rb.gravityScale = rb.gravityScale * 1.5f;
            c = Instantiate(lootSet[UnityEngine.Random.Range(0,lootSet.Length)],transform.position,transform.rotation).GetComponent<propBlastCtrl>();
            characterctrl.wantedLvl += wlPlus;
            c.Blast();
            StopAllCoroutines();
            dead = true;
            stoped = true;
            if (!bornByScript)
                masterPool.InsertNpc(gameObject,2f);
            else
                Destroy(gameObject,2f);
            //this.enabled = false; Хз, зачем, но пока закомментирую
        }
    }
    public void SilentDeath () {
        StopAllCoroutines();
        if (!bornByScript)
            masterPool.InsertNpc(gameObject);
        else
            Destroy(gameObject);
        //this.enabled = false;
    }
    public IEnumerator Stun() {
        canIdti = false;
        rb.freezeRotation = false; //Это для крутого откидывания
        rb.drag = 1;
        yield return new WaitForSeconds(2f);
        rb.freezeRotation = true;
        transform.rotation = Quaternion.Euler(0,0,0); //Возвращение в исходное положение
        rb.drag = 5; //Драг тоже, но его стоит пофиксить
        canIdti = true;
    }
    public IEnumerator Stoping() {
        stoped = true;
        yield return new WaitForSeconds(0.5f);
        stoped = false;
    }
    public void ToForward() { //Именно так...
        forward = true;
        bodys[0].SetActive(false);
        bodys[1].SetActive(true);
    }
    public void ToSide () { //... и никак больше. (нет)
        forward = false;
        bodys[0].SetActive(true);
        bodys[1].SetActive(false);
    }
    protected virtual void UpdatePlus () { //Вероятно, мне тогда было очень плохо, когда я это писал.

    }
    public void TriggerToPlayer() {
        destination = characterctrl.me;
        attack = true;
    }
    protected void Tikaem (bool callPol = true) {
        if (!tikaet) {
            tikaet = true;
            attack = false;
            destination = reserveDest;
            reserveDest.position = new Vector3((characterctrl.me.position.x < transform.position.x ? 10 : -10) + transform.position.x,transform.position.y,transform.position.z);
            speedMultipl = 1.5f;
            if (callPol) StartCoroutine(CallThePolice());
        }
    }
    protected virtual void OnDisable () {
        StopAllCoroutines(); //Дефолтные операции по завершению цикла жизни. Не путать с Death, он, как-никак, вызывается при нулевом хп, а не при отключении.
        characterctrl.it.detachAllNpcs.RemoveListener(detachMe);
        if (addedRoNearList) {
            characterctrl.NearNpcs.Remove(this);
            addedRoNearList = false;
        }
    }
    public virtual void PlayerPizdesTvorit() { //Тут все просто.
        if (courage > characterctrl.wantedLvl) {
            TriggerToPlayer();
        }
        else {
            Tikaem();
        }  
    }
    private IEnumerator CallThePolice () { //И тут.
        yield return new WaitForSeconds(4);
        mentiManagement.me.SendNaryad(0,characterctrl.me.position);
    }
    public void DetachTarget () {
        StartCoroutine(Neponyal());
    }
    protected virtual IEnumerator Neponyal() { //Заставляет непися побродить кругом, а потом идти по делам. (см. след комментарий)
        Debug.Log("neponyal");
        destination = reserveDest;
        attack = false;
        reserveDest.position = characterctrl.me.position;
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 5; i++) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(1f,3f));
            reserveDest.position = new Vector3(transform.position.x + UnityEngine.Random.Range(-10,10),transform.position.y,transform.position.z);
        }
        finalDestination = false; // Чтобы он пошел дальше гулять
        Debug.Log("ponyal");
    }
    public void AllInit() { //Инициализация, вызывается при каждом включении объекта. Много всего.
    //Возврат переменных в исходное состояние.
        if (myCity == null) {
            float dist = 100000;
            foreach (var g in GameObject.FindGameObjectsWithTag("city")) {
                var iterationDist = Mathf.Abs(g.transform.position.x - transform.position.x);
                if (iterationDist < dist) {
                    dist = iterationDist;
                    myCity = g.transform;
                }
            }
        }
        speedMultipl = 1;
        canIdti = true;
        dead = false;
        reloaded = true;
        rb.freezeRotation = true;
        rb.gravityScale = 10;
        transform.rotation = new Quaternion();
        right = true;
        stoped = false;
        scale.x = scaleFactor;
        hp = 100;
        transform.localScale = scale;
        gameObject.layer = myLayer;
        attack = false;
        if (anim != null)
            anim.Rebind();
        forIdem[0].Rebind();
        forIdem[1].Rebind();
        idet = false;
        characterctrl.it.detachAllNpcs.AddListener(detachMe);
        //Ген конечной точки
        //reserveDest.position.Set(transform.position.x + UnityEngine.Random.Range(30f,-30f),transform.position.y,transform.position.z);
        if (transform.position.x - characterctrl.me.position.x >= 0) {
            reserveDest.position = new Vector3(transform.position.x + UnityEngine.Random.Range(-25,-40f),transform.position.y,transform.position.z); //Чекай ренж на удаление, если будешь менять
        }
        else {
            reserveDest.position = new Vector3(transform.position.x + UnityEngine.Random.Range(25,40f),transform.position.y,transform.position.z);
        }
        destination = reserveDest;
    }
    private IEnumerator NightKills () {
        while (true) {
            yield return null;
            if (Mathf.Abs(characterctrl.me.position.x - transform.position.x) > 20) {
                SilentDeath();
                break;
            } 
        }
    }
    public void NigthAlarm () {
        if (dead) return;
        Tikaem(false);
        StartCoroutine(NightKills());
    }
}
