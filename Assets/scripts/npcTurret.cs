using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcTurret : hpBase
{
    private Transform targetTrns;
    private bool right = true;
    private Vector3 startScale;
    [SerializeField]
    private Transform armTrns;
    [SerializeField]
    private float coverageDistance;
    [SerializeField]
    private Transform weaponTransform;
    private Vector3 w;
    private float zAngle;
    [SerializeField]
    private float plusZAngle;
    private Quaternion handRot;
    private Vector3 crossHair;
    private bool targetInCoverage;
    private bool triggered;
    private bool openFire;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private int[] ammo;
    [SerializeField]
    private float shootDelay;
    [SerializeField]
    private float reloadingDelay;
    [SerializeField]
    private ParticleSystem shootParticles;
    void Start()
    {
        startScale = transform.localScale;
        TriggerToPlayer();
    }

    IEnumerator shooting () {
        triggered = true;
        while (triggered) {
            //Расстояние
            targetInCoverage = Mathf.Abs(targetTrns.position.x - transform.position.x) < coverageDistance;
            //Крутим нпс
            if (right != targetTrns.position.x >= transform.position.x) {
                right = !right;
                transform.localScale = new Vector3(right ? startScale.x : -startScale.x, startScale.y, startScale.z);
            }
            if (targetInCoverage) {
                //Целимся
                crossHair = targetTrns.position;
                w = -transform.position + crossHair;
                zAngle = Mathf.Atan2(w.y, w.x) * Mathf.Rad2Deg;
                zAngle += plusZAngle;
                handRot = Quaternion.Euler(0,0,zAngle);
                armTrns.rotation = Quaternion.Lerp(armTrns.rotation,handRot,0.1f);
                if (openFire && Quaternion.Angle(handRot,armTrns.rotation) < 4f) {
                    //Стреляем.
                    if (ammo[0] > 0) {
                        yield return new WaitForSeconds(shootDelay);
                        ammo[0]--;
                        shootParticles.Play();
                        Instantiate(bullet,weaponTransform.position,weaponTransform.rotation,weaponTransform).GetComponent<bulletCtrl>().right = this.right;
                    }
                    else {
                        yield return new WaitForSeconds(reloadingDelay);
                        ammo[0] = ammo[1];
                    }
                } 
            }
            yield return new WaitForFixedUpdate();
        }
    }
    public void TriggerToPlayer () {
        targetTrns = characterctrl.me;
        StartCoroutine(shooting());
    }
}
