using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class directButtons : MonoBehaviour
{
    public characterctrl ctrl;
    public int num;
    void Start () {
        ctrl = characterctrl.it;
    }
    void OnMouseDown () {
        ctrl.directions[num] = true;
    }
    void OnMouseUp () {
        ctrl.directions[num] = false;
    }
}
