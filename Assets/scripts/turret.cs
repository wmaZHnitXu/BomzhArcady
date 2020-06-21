using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class turret : MonoBehaviour
{
    [System.Serializable]
    enum turretState {
        shooting,
        searching,
        reloading
    }
    [SerializeField] private UnityEvent shootEffects;
    [System.Serializable]
    public struct bolvanka {
        public Transform transform;
        public SpriteRenderer renderer;
        public Rigidbody2D rigidbody;
    }


    public storage ammoStorage;
    [SerializeField] private turretState currentState;
    [SerializeField] private bool right;
    [SerializeField] private float shootingSpeed;
    [SerializeField] private List<bolvanka> bolvankiInit = new List<bolvanka>();
    private Queue<bolvanka> bolvanki;
    [SerializeField] private Transform headTransform;
    [SerializeField] private Transform target;
    [SerializeField] private hpBase targetHp;
    [SerializeField] private float range;
    [SerializeField] private float updatePeriod;
    private Quaternion gunRotation;
    [SerializeField] private float rotationSpeed;
    private bool active; 
    [SerializeField] private SpriteRenderer indicatorRenderer;
    [SerializeField] private Color[] indicatorColors = new Color[3];
    [SerializeField] private bool panelActivated;
    private bool Active
    {
        get => active;

        set
        {
            active = value;
        }
    }
    public static Transform actPanelTransform;
   // public 

    private void OnEnable()
    {
        if (actPanelTransform == null) {
            actPanelTransform = GameObject.Find("turretCanvas").transform;
        }
        Active = true;
        bolvanki = new Queue<bolvanka>(bolvankiInit);
        if (ammoStorage == null) ammoStorage = GetComponent<storage>();
    }
    private void Update () {
        if (Mathf.Abs(characterctrl.me.position.x - transform.position.x) < 2) {
            if (!panelActivated) { actPanelTransform.position = transform.position;
                storageCanvas.me.store = ammoStorage;
                panelActivated = true;
            }
        }
        else if (panelActivated) {
            actPanelTransform.position = new Vector3(-999,-999,-999);
            panelActivated = false;
        }
        if (active) {
            if (currentState == turretState.shooting) {
                if (target == null || targetHp == null || (target.position - headTransform.position).sqrMagnitude > range || targetHp.hp <= 0) {
                    StartCoroutine(Searching());
                    return;
                }
                right = target.position.x > transform.position.x;
                Vector2 CrossHair = target.position - transform.position;
                var euler = headTransform.rotation.eulerAngles;
                euler.y = right ? 0 : 180;
                headTransform.rotation = Quaternion.Euler(euler);
                var zAngle = right ? Mathf.Atan2(CrossHair.y, CrossHair.x) * Mathf.Rad2Deg : -Mathf.Atan2(CrossHair.y, CrossHair.x) * Mathf.Rad2Deg + 180;
                gunRotation = Quaternion.Euler(0,euler.y, zAngle);//zAngle = Mathf.Atan2(w.y, w.x) * Mathf.Rad2Deg;
                headTransform.rotation = Quaternion.RotateTowards(headTransform.rotation, gunRotation, rotationSpeed);
                if (Quaternion.Angle(headTransform.rotation, gunRotation) < 3) {
                    //##################################
                    int item = ammoStorage.PullAnyItem();
                    if (item != 0 && currentState != turretState.reloading) {
                        var readyBolv = bolvanki.Dequeue();
                        readyBolv.transform.position = headTransform.position;
                        readyBolv.renderer.sprite = inventoryCtrl.me.sprites[item];
                        ammoStorage.RemoveItem(item);
                        readyBolv.rigidbody.velocity = headTransform.right * 25;
                        shootEffects.Invoke();
                        StartCoroutine(Reloading());
                        bolvanki.Enqueue(readyBolv);
                        if (panelActivated) storageCanvas.me.RenderStorage();
                    }
                }
            }
        }
    }
    public void Activate () {

    }
    public void Deactivate () {

    }
    private IEnumerator OutOfAmmo () {
        yield return null;
    }
    private IEnumerator Searching () {
        currentState = turretState.searching;
        indicatorRenderer.color = indicatorColors[(int)currentState];
        target = null;
        targetHp = null;
        var allEnemys = characterctrl.NearNpcs;
        while (currentState == turretState.searching) {
            yield return new WaitForSeconds(0.5f);
            foreach(hpBase h in allEnemys) {
                float distToEnemy = range;
                float newDistToEnemy = (headTransform.position - h.transform.position).sqrMagnitude;
                if (newDistToEnemy <= range && distToEnemy > newDistToEnemy && h.hp > 0) {
                    distToEnemy = newDistToEnemy;
                    target = h.transform;
                    targetHp = h;
                }
            }
            currentState = targetHp != null ? turretState.shooting : turretState.searching;
        }
        indicatorRenderer.color = indicatorColors[(int)currentState];
    }
    private IEnumerator Reloading() {
        currentState = turretState.reloading;
        indicatorRenderer.color = indicatorColors[(int)currentState];
        yield return new WaitForSeconds(shootingSpeed);
        currentState = turretState.shooting;
        indicatorRenderer.color = indicatorColors[(int)currentState];
    }
}
