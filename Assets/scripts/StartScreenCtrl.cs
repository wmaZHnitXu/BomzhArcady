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
    private bool SettingsPulled = false;

    private static readonly int Disable = Animator.StringToHash("disable");

    void Start()
    {
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
        SettingsPulled = !SettingsPulled;
        settingsPanelAnim.SetBool("pull", SettingsPulled);
    }
}
