using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCallback : MonoBehaviour
{
    public bool iskiosk;
    public itemCtrl parent;
    public kioskCtrl kiosk;
    protected bool down;

   protected void OnMouseDown () {
       down = true; 
   }
   void Start() {
       iskiosk = kiosk;
       if (parent == null && kiosk == null) parent = GetComponent<itemCtrl>();
   }
   void OnMouseOver () {
       if (!iskiosk) {
           parent.CallBack();
       }
   }
   public virtual void OnMouseUp () {
       if (down) {
           if (iskiosk) {
               Debug.LogAssertion("стив хуйс");
               kiosk.CallBack();
           }
           else {
           if (characterctrl.it.onPc)
           parent.CallBack();
           }
       }
   }
}
