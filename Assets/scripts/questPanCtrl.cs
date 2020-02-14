using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questPanCtrl : MonoBehaviour
{
    [SerializeField]
    private Animator text;
    [SerializeField]
    private Animator pan;
    void OnMouseDown () {
        pan.SetBool("on",true);
        text.SetBool("on",true);
    }
    void OnMouseUp () {
        pan.SetBool("on",false);
        text.SetBool("on",false);
    }
    public IEnumerator quickShow () {
        OnMouseDown();
        yield return new WaitForSeconds(2f);
        OnMouseUp();
    }
    public void qs () {
        StartCoroutine(quickShow());
    }
}
