﻿using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class characterctrl : hpBase
{
   public static Transform me;
   public static float globalTime;
   [SerializeField]
   private float[] camBorders;
   public static bool freeze;
   public static int level;
   public static int exp;
   private int expMax;
   private static int _health;
   public static int Health {
       get => _health;
       set {
           _health = value;
           it.hp = value; //Я еблан
           it.CheckHp();
       }
   }
   public  static int money;
   public static byte eat;
   public static byte water;
    [SerializeField]
    private Text[] texts = new Text[5];
    public Animator[] nogiAnim = new Animator[2];
    [SerializeField]
    private bool idem;
    [SerializeField]
    private bool animNog;
    [SerializeField]
    public static Rigidbody2D rb;
    public bool[] directions = new bool[2]; //0 - право 1 - лево
    [SerializeField]
    private Vector2 force;
    public float speed;
    [SerializeField]
    private float acceleration;
    public float camspeed;
    [SerializeField]
    private Transform camtrns;
    public Transform caminterp;
    private Transform camTarget;
    [SerializeField] private bool camOnTransform;
    private Vector3 camHere;
    private Vector3 pos2;
    [SerializeField]
    public static bool rotation; //true - повернут вправо, false - повернут влево
    private byte counter;
    private bool rand;
    [SerializeField]
    private Image s;
    [SerializeField]
    private Image s2;
    [SerializeField]
    private ParticleSystem pLevelUp;
    public Animator arm;
    public byte weapon;
    public float[] attackCd; //Если огнестрел, то 0 - скорость перезарядки, 1 - интервал между выстрелами.
    private bool canAttack;
    public float meleDistance;
    public Transform melePos;
    public static float meleForce;
    public static characterctrl it;
    public GameObject answerObj;
    public Text[] answerTexts;
    public int answerNumber;
    public talkCtrl talker;
    public Transform hand;
    public Quaternion handRot;
    [SerializeField]
    private float zAngle;
    private Vector2 w;
    [SerializeField]
    private Transform crossHair;
    public bool withCross;
    public float plusZAngle;
    public GameObject[] weaponCanv;
    [SerializeField]
    private Transform bulletPos;
    public GameObject bullet;
    public ParticleSystem[] shParticles;
    public int[] magazine; //[1] == -1, при перезарядке, я так убрал одну лишнюю переменную
    public Sprite[] wsprites;
    public GameObject magObj;
    private readonly bool phone;
    private bool stopped;
    public Vector3[] npcSpawnPos;
    public static readonly List<hpBase> NearNpcs = new List<hpBase>();
    private int index;
    public static byte wantedLvl;
    public Transform[] towns;
    [SerializeField]
    private BoxCollider2D nogiColl;
    public GameObject[] nogiObjs; // 0 - дефолт 1 - сидя
    public bool siting;
    public bool shoot;
    private bool intwn;
    [SerializeField]
    private touchCtrl tCtrl;
    public bool onPc;
    public bool armBuff;
    private Coroutine attackCdRout;
    public ParticleSystem boostParticles;
    public TrailRenderer trail;
    public int meleeDamage = 5;
    public bool dead;
    public Animator youDied;
    public Vector2 spawnPoint;
    private float normalZRot;
    private bool _inBox;
    public UnityEvent detachAllNpcs;
    public bool busy;
    public Transform worldCanvas;
    private bool inBox;
    public bool InBox {
        get => _inBox;
        set {
            _inBox = value;
            if (value) {
               
            }
        }
    }
    [SerializeField]
    private ParticleSystem brakeParticles;
    [SerializeField]
    private bool brake;
    public bool gameStarted;
    [SerializeField]
    private GameObject mainCanvas;
    [SerializeField]
    private buttonWithDrag[] arrowButtons = new buttonWithDrag[2];

    private static readonly int Death1 = Animator.StringToHash("death");
    private static readonly int Hide = Animator.StringToHash("hide");
    private static readonly int Attack1 = Animator.StringToHash("attack");
    private static readonly int Walk = Animator.StringToHash("walk");
    private Camera myCam;
    private Coroutine zoomRoutine;
    private string crowStormName;

    public characterctrl(bool phone, float normalZRot)
    {
        this.phone = phone;
        this.normalZRot = normalZRot;
    }
    
    private void Awake()
    {
        crowStormName = "crowStorm";
        //Application.targetFrameRate = 60;
        myCam = Camera.main;
        onPc = SystemInfo.deviceType == DeviceType.Desktop;
        it = this;
        ResetCam();
        meleForce = 50;
        me = transform;
        rotation = true;
        rb = gameObject.GetComponent<Rigidbody2D>();
        Defaultinit();
        StartCoroutine(SvoistvaIteration());
        TextUpdate();
        expMax = level * level * 10;
        canAttack = true;
        freeze = true;
        siting = true;
    }
    public void Init () {
        transform.rotation = Quaternion.Euler(0,0,normalZRot);
        dead = false;
        brakeParticles.Stop();
        rb.freezeRotation = true;
        StopAllCoroutines();
        StartCoroutine(SvoistvaIteration());
        gameObject.layer = 12;
        rb.velocity = Vector3.zero;
        rb.drag = 0.5f;
        speed = 7;
        acceleration = 2;
        ResetCam();
    }
    private void FixedUpdate()
    {
        if (!dead)  {//ctrl
           if (!phone) {
               directions[0] = arrowButtons[0].clicked;
               directions[1] = arrowButtons[1].clicked;
                if (onPc) {
                    directions[0] = Input.GetKey(KeyCode.D) & !freeze;
                    directions[1] = Input.GetKey(KeyCode.A) & !freeze;
                }
           }
           idem = directions[0] ^ directions[1];  
           //anim
           if(idem & !animNog) {
               SwitchAnim(true);
               animNog = true;
           }
           if (!idem & animNog) {
               SwitchAnim(false);
               animNog = false;
           }
           //force
           force = rb.velocity;
           if (directions[0])
               force.x = Mathf.MoveTowards(force.x,speed,acceleration);
           if (directions[1])
               force.x = Mathf.MoveTowards(force.x,-speed,acceleration);
           rb.velocity = force;
           pos2 = Vector3.Lerp(camtrns.position,camTarget.position,camspeed);
           pos2.z = -10;
           if (pos2.x < camBorders[0]) {
               pos2.x = camBorders[0];
           }
           else if (pos2.x > camBorders[1]) {
               pos2.x = camBorders[1];
           }
           camtrns.position = pos2;
           if (!withCross) {
               if (directions[0] & !rotation & !directions[1]) {
                   rotation = true;
                   transform.localScale = new Vector3(10,10,10);
                   if (!canAttack & weapon == 0) {
                       StopCoroutine(attackCdRout);
                       canAttack = true;
                       //armBuff = true;
                   }
                   trail.emitting = false;
               }
               if (directions[1] & rotation & !directions[0]) {
                   rotation = false;
                   transform.localScale = new Vector3(-10,10,10);
                   if (!canAttack & weapon == 0) {
                       StopCoroutine(attackCdRout);
                       canAttack = true;
                       //armBuff = true;
                   }
                   trail.emitting = false;
               }
           }
           else {
               if (zAngle > 176 || zAngle < 0) {
                   if (rotation) {
                       rotation = false;
                       transform.localScale = new Vector3(-10,10,10);
                       plusZAngle = 94;
                   }
               }
               else {
                   if (!rotation) {
                       rotation = true;
                       transform.localScale = new Vector3(10,10,10);
                       plusZAngle = 86;
                   }
               }
               w = tCtrl.rightStick;
               if (onPc) w = -transform.position + crossHair.position;
               if (shoot) {
                   if (w.magnitude < 1 || onPc || !tCtrl.down)
                       shoot = false;
               }
               else {
                   if (w.magnitude >= 1 && onPc || tCtrl.down)
                       shoot = true;
               }
               zAngle = Mathf.Atan2(w.y, w.x) * Mathf.Rad2Deg;
               zAngle += plusZAngle;
               handRot = Quaternion.Euler(0,0,zAngle);
               hand.rotation = Quaternion.Lerp(hand.rotation,handRot,0.1f);
               if (magazine[0] > 0) { //Стрельба - перезарядка
                   if (shoot & canAttack) {
                       Shot();
                   }
               }
               else
               if (magazine[0] != -1) {
                   magazine[0] = -1;
                   StartCoroutine(Reloading());
               }
           }
        }
        if (rb.velocity.magnitude > 100f) {
            fxHub.me.ShakeMe(rb.velocity.magnitude / 5000); //Тряска
        }
        if (Mathf.Abs(rb.velocity.x) > speed * 2f & !brake) { //Тормоза эпичные
            StartCoroutine(Braking());
        }
    }
    void SwitchAnim (bool s) {
        nogiAnim[0].enabled = s;
        nogiAnim[1].enabled = s;
        nogiAnim[2].enabled = s;
        arm.SetBool(Walk,s);
    }
    IEnumerator SvoistvaIteration() {
        while(true) {
             yield return new  WaitForSeconds(0.5f);
            if (!freeze) {
                if (counter == 10) {
                    if (eat != 0)
                        eat -= 1;
                    else
                    {
                        Health--;
                    }
                    counter = 0;
                }
                intwn = InTown();
                if (!intwn & wantedLvl > 0)
                        wantedLvl-=1;
                rand = (Random.Range(0,1f) > 0.5f);
                if (counter == 4 | (counter == 9 & !rand)) {
                    if (water != 0) {
                        water -=1 ;
                    }
                    else
                    {
                       Health--; 
                    }
                }
                if (Health < 100) {
                    if (water > 74 & eat > 74)
                        Health++;
                }
                TextUpdate();
                counter++;
                if ((Random.Range(0,10) == 1 || Random.Range(0,10) == 2) & intwn & timeCtrl.me.time < 1300 & timeCtrl.me.time > 400)
                    SpawnRandomNpc();
            }
            if (eat == 0) {

            }
        }
    }
    void Defaultinit () {
        Health = 90;
        water = 100;
        eat = 100;
        level = 1;
        money = 100;
    }
    void TextUpdate () {
        texts[0].text = Health.ToString();
        texts[1].text = money.ToString();
        texts[2].text = water.ToString();
        texts[3].text = eat.ToString();
    }
    void LevelUp () {
        level++;
        exp -= expMax;
        expMax = level * level * 10;
        pLevelUp.Play();
    }
    public void Attack () {
        if (canAttack) {
        switch (weapon) {
            case 0:
                StartCoroutine(PreAttackCd());
            break;
        }
        attackCdRout = StartCoroutine(AttackCdEnum());
        }
    }

    private IEnumerator AttackCdEnum () {
        canAttack = false;
        yield return new WaitForSeconds(attackCd[1]);
        canAttack = true;
        trail.emitting = false;
    }

    private IEnumerator PreAttackCd () {
        arm.SetTrigger(Attack1);
        yield return new WaitForSeconds(0.1f);
        foreach (var b in NearNpcs) {//For юзай, псина
                    if (Vector3.Distance(((MonoBehaviour)b).transform.position,melePos.position) < meleDistance) { 
                        b.AddHit(meleeDamage);
                        if (armBuff) {
                            b.AddHit(meleeDamage * 2);
                        }
                    }
                    if (armBuff) {
                        Boost(rotation ? new Vector2 (1000,0) : new Vector2 (-1000,0));
                        armBuff = false;
                        trail.emitting = true;
                    }
        }
    }
    public void GetAnswer (int number) {
        answerNumber = number;
        talker.Answer();
    }
    public void SetWeapon(bool b) {
        withCross = b;
        weaponCanv[0].SetActive(!b);
        weaponCanv[1].SetActive(b && onPc);
        weaponCanv[2].SetActive(b);
    }

    private void Shot () {
        TriggerimNpc();
        Instantiate(bullet,bulletPos.position,bulletPos.rotation,bulletPos).GetComponent<bulletCtrl>().forPlayer = false;
        magazine[0]--;
        shParticles[weapon].Play();
        canAttack = false;
        attackCdRout = StartCoroutine(AttackCdEnum());
    }

    private IEnumerator Reloading() {
        if (wsprites[1] != null) {
           inventoryCtrl.me.inHandWeapon.sprite = wsprites[1];
           Destroy(Instantiate(magObj,bulletPos.position,bulletPos.rotation),2f);
        }
        yield return new WaitForSeconds(attackCd[0]);
        magazine[0] = magazine[1];
        if (wsprites[1] != null) {
            inventoryCtrl.me.inHandWeapon.sprite = wsprites[0];
        }
    }
    public override void AddHit (int hp)
    {
        Health -= hp;
        if (hp >= 15) {
            fxHub.GiveMeBlood(transform.position);
            StartCoroutine(Stopping());
        }
    }
    public override void AddHit(int hit, Vector3 punchPos) {
        AddHit(hit);
        rb.velocity = rb.velocity + (Vector2)(transform.position - punchPos).normalized;
    }
    private IEnumerator Stopping () {
        if (stopped != true) {
            speed = speed * 0.5f;
            stopped = true;
        }
        yield return new WaitForSeconds(0.5f);
        if (stopped != false) {
            speed = speed * 2;
            stopped = false;
        }
    }
    IEnumerator Braking () {
        brake = true;
        while (Mathf.Abs(rb.velocity.x) > speed * 4f) {
            rb.velocity = new Vector2(rb.velocity.x * 0.9f,0);
            transform.rotation = Quaternion.Euler(0,0,rb.velocity.x);
            brakeParticles.Play();
            fxHub.me.ShakeMe(rb.velocity.x * 0.005f);
            yield return new WaitForFixedUpdate();
        }
        while (transform.rotation.eulerAngles.z != 0) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.identity,0.5f);
            yield return new WaitForFixedUpdate();
        }
        brake = false;
    }

    private void SpawnRandomNpc () {
        index = Random.Range(0,npcSpawnPos.Length);
        masterPool.EjectNpc(npcSpawnPos[index] + transform.position);
    }

    private void TriggerimNpc ()
    {
        foreach (var ctrl in NearNpcs.OfType<npcCtrl>())
        {
            ctrl.PlayerPizdesTvorit();
        }
    }

    private bool InTown ()
    {
        return towns.Any(t => Vector2.Distance(t.position, me.position) < 50);
    }
    public void SwitchSiting (bool b) {
        StopAllCoroutines();
        StartCoroutine(SvoistvaIteration());
        nogiColl.enabled = !b;
        nogiObjs[0].SetActive(!b);
        nogiObjs[1].SetActive(b);
        siting = b;
        Debug.Log("bebra");
        if (b) {
            speed = speed / 2;
        }
        else {
            speed = 7;
        }
    }
    public void Boost(Vector2 bst) {
        rb.AddForce(bst);
        fxHub.me.ShakeMe(0.2f);
        boostParticles.Play();
        //boostParticles.transform.rotation = Quaternion.Euler(Vector2.Angle(rb.velocity,new Vector2(1,0)),90,0); т.к теперь кружок
    }
    public void CheckHp () {
         texts[0].text = Health.ToString();
         if (Health < 25) {
             texts[0].color = Color.red;
         }
         else
         {
             texts[0].color = Color.white;
         }
        if (Health < 0) {
            Health = 0;
            Death();
        }
    }

    public override void Death() {
        if (dead) return;
        dead = true;
        rb.freezeRotation = false;
        gameObject.layer = 15;
        StopAllCoroutines();
        startScreenCtrl.me.EnableCanvas();
        startScreenCtrl.me.SwitchScreenState(3);
    }
    public void Revive () {
        Health = 100;
        eat = 100;
        water = 100;
        transform.position = spawnPoint;
        youDied.SetBool("pull", false);
        startScreenCtrl.me.StartCoroutine(startScreenCtrl.me.canvasScaling(true));
        Init();
    }
    public void GetInTransport () {
        rb.simulated = false;
        busy = true;

    }
    public void GetOutTransport () {
        rb.simulated = true;
        transform.SetParent(null);
        busy = false;
    }
    public void SetWantedLevel (byte wanted) {
        if (wantedLvl < wanted) {
            wantedLvl = wanted;
        }
    }
    public void Exit() {
        Application.Quit();
    }
    public void SetCamTarget (Transform target) {
        camTarget = target;
        camOnTransform = true;
    }
    public void SetCamTarget (Vector3 pos) {
        camHere = pos;
        camOnTransform = false;
    }

    private void ResetCam () {
        camTarget = caminterp;
        camOnTransform = true;
        camspeed = 0.1f;
        SetCamZoom(5);
    }
    public void StartCutScene (bool start) {
        mainCanvas.SetActive(!start);
        rb.isKinematic = start;
        ResetCam();
        freeze = start;
    }
    public void SetCamZoom (float zoom) {
        if (zoomRoutine != null) StopCoroutine(zoomRoutine);
        zoomRoutine = StartCoroutine(Zooming(zoom));
    }
    private IEnumerator Zooming (float zoom) {
        while (Mathf.Abs(myCam.orthographicSize - zoom) > 0.1f) {
            yield return null;
            myCam.orthographicSize = Mathf.Lerp(myCam.orthographicSize, zoom, 0.1f);
        }
         myCam.orthographicSize = zoom;
    }
    public Vector3 GetNpcSpawnPosition()
    {
        return transform.position + (Random.Range(0, 100) > 50 ? npcSpawnPos[1] : npcSpawnPos[0]);
    }
    public void SetStatsFromSave (int hunger, int water, int health, int money) {
        characterctrl.eat = (byte)hunger;
        characterctrl.water = (byte)water;
        characterctrl.money = money;
        Health = health;
    }
    public void InstantPosChange (Vector3 pos) {
        transform.position = pos;
        camtrns.position = camTarget.position;
    }
    public void CrowStrike(Vector3 crowPos) {
        fxHub.me.GimmeParticles(particleType.SmallSmoke, transform.position);
        rb.AddForce((crowPos.x > transform.position.x ? -250 : 250) * Vector3.right);
        AddHit(5);
    }
    private void OnParticleCollision (GameObject collidedObj) {
        if (collidedObj.name.Equals(crowStormName)) {
            CrowStrike(collidedObj.transform.position);
        }
        Debug.Log(collidedObj.name + crowStormName);
    }
}
