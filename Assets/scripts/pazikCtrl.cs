using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pazikCtrl : carNpcCtrl
{
    public float transportTime = 0.5f;
    private Transform arkadyTransform;
    private WaitForSeconds wait;
    [SerializeField]
    private Transform[] positions = new Transform[2];
    [SerializeField]
    private SpriteRenderer sprite;
    private Coroutine routine;
    [SerializeField]
    private Vector3[] busStops;
    private int currentBusStop;
    private bool pathDirection;
    private Coroutine stopRoutine;
    [SerializeField]
    private Animator door;
    [SerializeField]
    private int countDown;
    [SerializeField]
    private Button busStopButtonEnter;
    [SerializeField]
    private Button busStopButtonExit;
    [SerializeField]
    private Transform stopButtonsTrns;
    private bool stopped;
    void Start () {
        destination = transform.position;
        StartCoroutine(stop());
        SwitchEnterExit(true);
    }
    public void EjectArcady () {
        if (routine != null) {
            StopCoroutine(routine);
        }
        routine = StartCoroutine(Transporting(false));
        SwitchEnterExit(true);
    }
    public void InsertArkady () {
        arkadyTransform = characterctrl.me;
        characterctrl.it.GetInTransport();
        arkadyTransform.SetParent(transform);
        if (routine != null) {
            StopCoroutine(routine);
        }
        routine = StartCoroutine(Transporting(true));
        SwitchEnterExit(false);
        characterctrl.it.detachAllNpcs.Invoke();
    }
    IEnumerator Transporting (bool tudaSuda) {
        int i = tudaSuda ? 0 : positions.Length-1;
        while (i != (tudaSuda ? positions.Length : 0)) {
            while (Vector3.Distance(arkadyTransform.localPosition,positions[i].localPosition) > 0.1f) {
                arkadyTransform.localPosition = Vector3.Lerp(arkadyTransform.localPosition,positions[i].localPosition,0.1f);
                yield return new WaitForFixedUpdate();
                }
            if (i == 1) {
                sprite.sortingOrder = sprite.sortingOrder == 5 ? 4 : 5;
            }
            yield return wait;
            i += tudaSuda ? 1 : -1;
        }
        if (!tudaSuda)
            characterctrl.it.GetOutTransport();
        else
            countDown = countDown/3;
    }
    IEnumerator stop () {
        stopped = true;
        door.SetBool("IsOpen",true);
        for (countDown = 20; countDown >= 0; countDown--) {
            yield return new WaitForSeconds(1);
        }
        FindNextBusStop();
        door.SetBool("IsOpen",false);
        ejected = false;
        stopped = false;
    }
    protected override void FixedUpdate() //Нужна коррекция, писал в полуотрубленном состоянии
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
                        stopRoutine = StartCoroutine(stop());
                        ejected = true;
                    }
            }
            forceMaster.x = right ? speed : -speed;
            rb.AddForce(forceMaster);
            wheels[0].Rotate(0,0,rb.velocity.magnitude);
            wheels[1].Rotate(0,0,rb.velocity.magnitude);
            if (stopped & Mathf.Abs(characterctrl.me.position.x - transform.position.x) < 10)
                stopButtonsTrns.position = transform.position;
            else
                stopButtonsTrns.position = new Vector3 (9999,9999,0);
        }
    }
    void FindNextBusStop () {
        if (currentBusStop == 0 | currentBusStop == busStops.Length-1) {
            pathDirection = !pathDirection;
        }
        currentBusStop += pathDirection ? 1 : -1;
        destination = busStops[currentBusStop];
    }
    void SwitchEnterExit (bool enter) {
        busStopButtonEnter.gameObject.SetActive(enter);
        busStopButtonExit.gameObject.SetActive(!enter);
    }
}
