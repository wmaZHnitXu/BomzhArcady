using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mentiManagement : MonoBehaviour
{
    public static mentiManagement me;
    [SerializeField]
    private GameObject mentPref;
    [SerializeField]
    private GameObject ladaPref;
    [SerializeField]
    private Transform spawnPoint;
    private List<mentCtrl> moiMenti = new List<mentCtrl>();
    private bool viehali;
    void Awake()
    {
        me = this;
    }
    public void SendNaryad (int naryadLevel, Vector3 naryadPos) {
        Debug.Log("musornulis");
        if (!viehali) {
            mentCtrl ctrl;
            switch (naryadLevel) {
                case 0:
                    ctrl = Instantiate(mentPref,spawnPoint.position,spawnPoint.rotation).GetComponent<mentCtrl>();
                    ctrl.bornByScript = true;
                    ctrl.SetInvestigationPos(naryadPos);
                    moiMenti.Add(ctrl);
                break;
                default:
                    for (int i = 0; i < naryadLevel+1; i++) {
                        moiMenti.Add(Instantiate(mentPref,spawnPoint.position,spawnPoint.rotation).GetComponent<mentCtrl>());
                    }
                    foreach(mentCtrl m in moiMenti) {
                        m.bornByScript = true;
                        m.SetInvestigationPos(naryadPos);
                    }
                break;
            }
            viehali = true;
        }
        else {
            foreach(mentCtrl m in moiMenti) {
                m.SetInvestigationPos(naryadPos);
            }
        }
    }
    public void SendNaryad (int naryadLevel, Vector3 naryadPos, bool immediately) {
        Debug.Log("musornulis immediately");
            mentCtrl ctrl;
            switch (naryadLevel) {
                case 0:
                    ctrl = Instantiate(mentPref,spawnPoint.position,spawnPoint.rotation).GetComponent<mentCtrl>();
                    ctrl.bornByScript = true;
                    ctrl.SetInvestigationPos(naryadPos);
                    moiMenti.Add(ctrl);
                break;
                default:
                    for (int i = 0; i < naryadLevel+1; i++) {
                        moiMenti.Add(Instantiate(mentPref,spawnPoint.position,spawnPoint.rotation).GetComponent<mentCtrl>());
                    }
                    foreach(mentCtrl m in moiMenti) {
                        m.bornByScript = true;
                        m.SetInvestigationPos(naryadPos);
                    }
                break;
            }
    }
    public void mentRip (mentCtrl ment) {
        Debug.Log("F, " + ment.gameObject.name);
        moiMenti.Remove(ment);
        if (moiMenti.Count == 0) {
            SendNaryad(2,characterctrl.me.position,true);
        }
        else {
            foreach(mentCtrl m in moiMenti) {
                m.bornByScript = true;
                m.SetInvestigationPos(characterctrl.me.position);
            }
        }
    }
}
