using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startScreenCtrl : MonoBehaviour
{
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
    private static readonly int Disable = Animator.StringToHash("disable");
    [System.Serializable]
    public enum ScreenState : int {
        Main,
        Settings,
        Saves,
    }
    [SerializeField] private ScreenState currentState;
    public static startScreenCtrl me;
    public void SwitchScreenState (int _stateToSwitch) { //Ссаный юнити не сериализует
        ScreenState stateToSwitch = (ScreenState)_stateToSwitch;
        if (stateToSwitch == currentState) return;
        switch (currentState) {
            case ScreenState.Saves:
                SwitchSaves();
            break;
            case ScreenState.Settings:
                SwitchSettings();
            break;
        }
        switch (stateToSwitch) {
            case ScreenState.Saves:
                SwitchSaves();
            break;
            case ScreenState.Settings:
                SwitchSettings();
            break;
        }
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
        foreach (Animator a in menuAnim)
            a.SetTrigger(Disable);
        canvases[1].SetActive(true);
        particle.Stop();
        settingsPanelAnim.SetBool("pull", false);
    }
    public void SwitchSettings () {
        settingsPanelAnim.SetBool("pull", !settingsPanelAnim.GetBool("pull"));
    }
    public void SwitchSaves () {
        SaveLoad.me.PullSavesPanel();
    }
}
