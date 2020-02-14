using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenCtrl : MonoBehaviour
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
    void Start()
    {
        characterctrl.it.switchSiting(true);
    }

    void FixedUpdate()
    {
        if (starting) {
            cslr.scaleFactor = Mathf.Lerp(cslr.scaleFactor,1f,0.1f);
            if (cslr.scaleFactor > 0.99f) {
                starting = false;
                characterctrl.freeze = false;
                //canvases[0].SetActive(false);
                characterctrl.it.switchSiting(false);
                cslr.scaleFactor = 1f;
            }
        }
    }
    public void StartGame() {
        starting = true;
        foreach (Animator a in menuAnim)
            a.SetTrigger("disable");
        canvases[1].SetActive(true);
        particle.Stop();
    } 
}
