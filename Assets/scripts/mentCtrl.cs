using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mentCtrl : npcCtrl
{
    public int[] ammo;
    [SerializeField]
    private SpriteRenderer weapon;
    [SerializeField]
    private Transform armTrns;
    private Vector3 crossHair;
    private Quaternion startQuatArm;
    private float zAngle;
    private Vector3 w;
    private Quaternion handRot;
    [SerializeField]
    private float plusZAngle;
    private bool weaponEjected;
    private bool canShoot;
    [SerializeField]
    private float shotDelay;
    [SerializeField]
    private GameObject bullet;
    private bool firstIter = true;
    private Transform weaponPos;
    private bool investigation;
    protected override void OnEnable() {
        zAngle = 0;
        w.Set(0,0,0);
        canShoot = true;
        weaponPos = weapon.transform;
        if (!bornByScript)
            AllInit();
        if (characterctrl.wantedLvl >= 5)
            PlayerPizdesTvorit();
    }
    void Start () {
        startQuatArm = armTrns.rotation;
    }
    public override void Attacking() {
        crossHair = destination.position;
        w = -transform.position + crossHair;
        zAngle = Mathf.Atan2(w.y, w.x) * Mathf.Rad2Deg;
        zAngle += plusZAngle;
        handRot = Quaternion.Euler(0,0,zAngle);
        armTrns.rotation = Quaternion.Lerp(armTrns.rotation,handRot,0.1f);
        if (!weaponEjected) {
            weapon.enabled = true;
            weaponEjected = true;
        }
        if (Quaternion.Angle(armTrns.rotation,handRot) < 0.1f) {
            if (reloaded) {
                if (canShoot) {
                    Instantiate(bullet,weaponPos.position,weaponPos.rotation,weaponPos).GetComponent<bulletCtrl>().right = this.right;
                    StartCoroutine(AttackCd());
                    ammo[0]--;
                }
                if (ammo[0] == 0)
                    StartCoroutine(Reloading());
            }
        }
    }
    protected override void UpdatePlus() {
        if (!attack) {
            if (weaponEjected) {
                weaponEjected = false;
                weapon.enabled = false;
                StartCoroutine(Neponyal());
                StartCoroutine(HandSetDef());
            }
        }
        if (investigation) {
            if (Mathf.Abs(dist) < 20f) {
                investigation = false;
                StartCoroutine(Neponyal());
            }
        }
    }
    private IEnumerator AttackCd() {
        canShoot = false;
        yield return new WaitForSeconds(shotDelay);
        canShoot = true;
    }
    private IEnumerator Reloading() {
        reloaded = false;
        ammo[0] = -1;
        yield return new WaitForSeconds(reloadTime);
        ammo[0] = ammo[1];
        reloaded = true;
    }
    protected override void OnDisable() { //Пока что только чтобы привести руку в норм. положение, если внесение в пул произошло во время незаконченного HandSetDef
        base.OnDisable();
        armTrns.rotation = startQuatArm;
    }
    void OnDestroy () {
        mentiManagement.me.MentRip(this);
    }
    private IEnumerator HandSetDef () {
        while (armTrns.rotation != startQuatArm) {
            armTrns.rotation = Quaternion.RotateTowards(armTrns.rotation,startQuatArm,3f);
            yield return new WaitForFixedUpdate();
        }
    }
    public void SetInvestigationPos (Vector3 pos) {
        if (!attack) {
            if (reserveDest == null) {
                reserveDest = new GameObject(gameObject.name + "_destRes").transform;
            }
            destination = reserveDest;
            reserveDest.position = pos;
            investigation = true;
        }
    }
}
