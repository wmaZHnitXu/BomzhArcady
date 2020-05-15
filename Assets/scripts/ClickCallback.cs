using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickCallback : MonoBehaviour
{
    public bool iskiosk;
    public itemCtrl parent;
    public kioskCtrl kiosk;
    protected bool down;

   protected void OnMouseDown () {
       down = true; 
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
           parent.CallBack();
           }
       }
   }
}
