using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class metalCtrl : kioskCtrl

{
    public byte needId;
    public override void Update() {
        if (Vector3.Distance(player.position,KioskPosition.position) < dist & !buttonOn) {
            buttonOn = true;
            button.SetActive(true);
        }
        if (!(Vector3.Distance(player.position,KioskPosition.position) < dist) & buttonOn) {
            buttonOn = false;
            button.SetActive(false);
        }
        if (buttonOn) {
            if (buttonActive & !(hasItem(needId))) {
                buttonActive = false;
                buttonImg.color = colors[1];
                Debug.Log("button false");
            }
            if (!buttonActive & hasItem(needId)) {
                buttonActive = true;
                buttonImg.color = colors[0];
                Debug.Log("button true");
            }
        }
    }
    public bool hasItem(byte id) {
        bool h = false;
        for (int i = 0; i < 4; i++) {
            for (int i2 = 0; i2 < 4; i2++) {
                if (inventoryCtrl.me.items[i2,i,0] == id) h = true;
            }
        }
        return h;
    }
    public override void CallBack() {
       // Debug.LogWarning("пиздос attempt" + " " + buttonActive.ToString() + hasItem(needId).ToString());
        if (buttonActive) {
            //Debug.LogWarning("пиздос successfull");
            Instantiate(tovar,tovarpoint.position,tovarpoint.rotation);
            inventoryCtrl.me.removeItem(needId);
        }
        else {
            //Debug.LogAssertion("пиздос error" + " " + buttonActive.ToString() + " " + hasItem(needId) + needId.ToString());
        }
    }   
}
