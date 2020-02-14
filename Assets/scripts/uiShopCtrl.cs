using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiShopCtrl : kioskCtrl
{   
    public GameObject shop;
    public override void Update()
    {
       if (Vector3.Distance(player.position,KioskPosition.position) < dist & !buttonOn) {
            buttonOn = true;
            button.SetActive(true);
        }
        if (!(Vector3.Distance(player.position,KioskPosition.position) < dist) & buttonOn) {
            buttonOn = false;
            button.SetActive(false);
        } 
    }
    public override void CallBack() {
        shop.SetActive(true);
    }
}
