using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class trueButton : MonoBehaviour
{
    public UnityEvent onClick;
    private bool clicked;
    void OnMouseDown () {
        clicked = true;
    }
    void OnMouseUp () {
        if (clicked == true) {
            onClick.Invoke();
        }
        clicked = false;
    }
}
