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
    void Start()
    {
        distance = 4;
        if (mat == null) {
            mat = transform.GetChild(0).GetComponent<SpriteRenderer>().material;
        }
        if (rb == null) {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }
        player = characterctrl.me;
        buttonon = false;
        mat.SetFloat("_OutlineThickness",0);
        touchCtrl.me.itemCtrls.Add(this);
        touchCtrl.me.itemTrns.Add(transform);
        bomzhCtrl.itemTransforms.Add(transform);

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
                dead = true;
                if (rb!=null)
                    rb.simulated = false;
                buttonon = false;
                mat.SetFloat("_OutlineThickness",0);
                if (cb != null) {
                    cb.enabled = false;
                }
            }
        }
    }
    void OnDestroy () {
        bomzhCtrl.itemTransforms.Remove(transform);
    }
}
