using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class questManager : MonoBehaviour
{
    public static questManager me;
    public int quest;
    public int activePart;
    [SerializeField]
    private questStructure[] quest1;
    public questStructure[][] quests;
    public Text qdescText;
    [SerializeField]
    private Transform playerTrns;
    [SerializeField]
    private questPanCtrl qpc;
    void Awake() {
        me = this;
        quests = new questStructure[2][];
        quests[1] = quest1;
        quest = 1;
        qdescText.text = quests[quest][activePart].qdesc;
    }
    void Update()
    {
        if (quest != 0) {
            switch (quests[quest][activePart].type)
            {
                case 0:
                    if (Vector3.Distance(playerTrns.position,quests[quest][activePart].gotuda) < 0.5f) {
                        PartDone();
                    }
                break;
            }
        }
    }
    public void PartDone () {
        if (activePart+1 != quests[quest].Length) {
            activePart++;
            qdescText.text = quests[quest][activePart].qdesc;
            qpc.Qs();
        }
        else
        {
            QuestComplete();
        }
    }
    public void QuestComplete () {
        quest = 0;
        activePart = 0;
    }
    public void QuestStart () {

    }
}
