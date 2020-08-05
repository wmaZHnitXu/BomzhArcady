﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startScreenCtrl : MonoBehaviour
{
    [SerializeField] private GameObject GameCanvas;
    [SerializeField]
    private bool starting;
    [SerializeField]
    private CanvasScaler cslr;
    [SerializeField]
    private Animator[] menuAnim;
    [SerializeField]
    private GameObject[] canvases; //0 - меню 1 - интерфейс
    [SerializeField]
    private ParticleSystem particle;
    [SerializeField] private Animator settingsPanelAnim;
    [SerializeField] private Animator deadPanAnim;
    private readonly int HashedPull = Animator.StringToHash("pull");
    [SerializeField] private GameObject canvasObject;
    [SerializeField] private GameObject menu;
    [System.Serializable]
    public enum ScreenState : int {
        Main,
        Settings,
        Saves,
        Dead,
    }
    private bool menuPulled;
    [SerializeField] private ScreenState currentState;
    public static startScreenCtrl me;
    public void SwitchScreenState (int _stateToSwitch) { //Ссаный юнити не сериализует
        ScreenState stateToSwitch = (ScreenState)_stateToSwitch;
        Debug.Log(currentState.ToString() + " to " + stateToSwitch.ToString());
        if (stateToSwitch == currentState) return;
        #region cringe
        switch (currentState) {
            case ScreenState.Saves:
                SwitchSaves();
            break;
            case ScreenState.Settings:
                SwitchSettings();
            break;
            case ScreenState.Dead:
                SwitchDeadMenu();
            break;
        }
        switch (stateToSwitch) {
            case ScreenState.Saves:
                SwitchSaves();
            break;
            case ScreenState.Settings:
                SwitchSettings();
            break;
            case ScreenState.Dead:
                SwitchDeadMenu();
                Debug.Log("sw");
            break;
        }
        #endregion
        currentState = stateToSwitch;
    }

    void Start()
    {
        me = this;
        characterctrl.it.SwitchSiting(true);
    }

    void FixedUpdate()
    {
        if (starting) {
            cslr.scaleFactor = Mathf.Lerp(cslr.scaleFactor,1f,0.1f);
            if (cslr.scaleFactor > 0.99f) {
                starting = false;
                characterctrl.freeze = false;
                //canvases[0].SetActive(false);
                characterctrl.it.SwitchSiting(false);
                characterctrl.it.gameStarted = true;
                cslr.scaleFactor = 1f;
            }
        }
    }
    public void StartGame() {
        starting = true;
        SwitchScreenState(0);
        menuAnim[0].SetTrigger("disable");
        menuAnim[1].SetBool(HashedPull, false);
        canvases[1].SetActive(true);
        particle.Stop();
        Invoke("HashedPullCanvas", 2f);
    }
    public void SwitchSettings () {
        settingsPanelAnim.SetBool(HashedPull, !settingsPanelAnim.GetBool(HashedPull));
    }
    public void SwitchSaves () {
        SaveLoad.me.PullSavesPanel();
    }
    public void HashedPullCanvas () {
        canvasObject.SetActive(false);
    }
    public void SwitchDeadMenu () {
        bool b = !deadPanAnim.GetBool(HashedPull);
        menu.SetActive(!b);
        menuAnim[1].SetBool(HashedPull, !b);
        deadPanAnim.SetBool(HashedPull, b);
    }
    public void EnableCanvas () {
        canvasObject.SetActive(true);
        GameCanvas.SetActive(false);
    }
}
