﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class questPanCtrl : MonoBehaviour
{
    [SerializeField]
    private Animator text;
    [SerializeField]
    private Animator pan;
    [SerializeField]
    private Text description;
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
    public void SetQuestDesc (string desc) {
        description.text = desc;
        StartCoroutine(quickShow());
    }
}
