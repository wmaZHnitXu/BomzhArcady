using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemCtrl : MonoBehaviour
{
    public bool autoPick;
    private Transform player;
    public float distance;
    [SerializeField]
    private bool buttonon;
    public byte type;
    public int money;
    public ClickCallback cb;
    public byte id;
    public bool dead;
    [SerializeField]
    private Rigidbody2D rb;
    public float speed = 4f;
    public Material mat;
    public static List<Transform> itemTransforms = new List<Transform>();
    public static List<itemCtrl> controls = new List<itemCtrl>();
    void Awake () {
        if (mat == null) {
            mat = transform.GetChild(0).GetComponent<SpriteRenderer>().material;
        }
    }
    void Start()
    {
        itemTransforms.Add(transform);
        controls.Add(this);
        distance = 4;
        if (rb == null) {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }
        player = characterctrl.me;
        buttonon = false;
        mat.SetFloat("_OutlineThickness",0);
        speed = 10f;
    }
    void Update()
    {
        if (!dead) {
            if (Vector3.Distance(player.position,transform.position) <= distance) {
                if (!autoPick) {
                    ButtonOn(true);
                }
                else
                    {
                        buttonon = true;
                        CallBack();  
                    }
            }
            else
                if (!autoPick)
                    ButtonOn(false);
        }
        else {
            if (Vector3.Distance(player.position,transform.position) > 0.2f)
            transform.position = Vector3.Lerp(transform.position,player.position,speed * Time.deltaTime);
            else {
                if (type != 1) {
                        inventoryCtrl.me.AddItem(id);
                    }
                    else
                        characterctrl.money += money;
                Destroy(gameObject);
            }
        }
    }
    void ButtonOn(bool b) {
        if (!dead) {
            if (b & !buttonon ) {
                buttonon = true;
                mat.SetFloat("_OutlineThickness",1);
                if (cb != null) {
                    cb.enabled = true;
                }
            }
            if (!b & buttonon) {
                buttonon = false;
                mat.SetFloat("_OutlineThickness",0);
                if (cb != null) {
                    cb.enabled = false;
                }
            }
        }
    }
    public void CallBack () {
        if (!dead & !characterctrl.it.busy) {
            if (buttonon) {
                Pull();
            }
        }
    }
    public void Pull () {
        dead = true;
        if (rb!=null) rb.simulated = false;
        buttonon = false;
        mat.SetFloat("_OutlineThickness",0);
        if (cb != null) {
            cb.enabled = false;
        }
    }
    public void Pull(bool p) {
        if (p) {
            GetComponent<BoxCollider2D>().enabled = false;
        }
        Pull();
    } 
    void OnDestroy () {
        itemTransforms.Remove(transform);
    }
}
