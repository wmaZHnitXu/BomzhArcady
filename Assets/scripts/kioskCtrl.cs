using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class kioskCtrl : MonoBehaviour
{
    protected Transform player;
    public float dist;
    [SerializeField]
    protected GameObject button;
    public int price;
    protected ClickCallback call;
    [SerializeField]
    protected bool buttonOn;
    [SerializeField]
    protected bool buttonActive;
    [SerializeField]
    protected SpriteRenderer buttonImg;
    [SerializeField]
    protected Color[] colors = new Color[2];
    [FormerlySerializedAs("KioskPosition")] [SerializeField]
    protected Transform kioskPosition;
    [SerializeField]
    protected Transform tovarpoint;
    [SerializeField]
    protected GameObject tovar;
    protected void Start()
    {
        buttonActive = false;
        buttonOn = false;
        player = characterctrl.me;
        buttonImg.color = colors[1];
    }

    public virtual void Update()
    {
        if (Vector3.Distance(player.position,kioskPosition.position) < dist & !buttonOn) {
            buttonOn = true;
            button.SetActive(true);
        }
        if (!(Vector3.Distance(player.position,kioskPosition.position) < dist) & buttonOn) {
            buttonOn = false;
            button.SetActive(false);
        }
        if (buttonOn) {
            if (buttonActive & characterctrl.money < price) {
                buttonActive = false;
                buttonImg.color = colors[1];
            }
            if (!buttonActive & !(characterctrl.money < price)) {
                buttonActive = true;
                buttonImg.color = colors[0];
            }
        }
    }
    public virtual void CallBack () {
        if (buttonActive) {
            Instantiate(tovar,tovarpoint.position,tovarpoint.rotation);
            Debug.LogWarning("Купил в киоске чето kioskCtrl");
            characterctrl.money -= price;
        }
    }
}
