using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class npcScenario : MonoBehaviour
{
    [SerializeField]
    private GameObject[] myBodys; //0 - side 1 - front
    [SerializeField]
    private float approximationDistance; //Используется для того, чтобы нпс смотрел на игрока при приближении.
    [SerializeField]
    private float maxApproximationDistance;
    [SerializeField]
    private bool state; //Смотрит вперед(true)/в сторону(false)
    [SerializeField]
    private GameObject defNpc;
    [SerializeField]
    private GameObject myText;
    private GameObject TextGameObjectInstance;
    private Text TextInstance;
    private Transform playerTrns;
    [SerializeField]
    private bool right = true;
    private Vector3 startScale;
    [SerializeField] [TextArea]
    private string greetings;
    [SerializeField]
    private talkCtrl talker;
    private bool saidGreetings;

    void Start()
    {
        startScale = transform.localScale;
        playerTrns = characterctrl.me;
        StartCoroutine(myUpdate());
        TextGameObjectInstance = Instantiate(myText,new Vector3(transform.position.x,transform.position.y + 2,transform.position.z),Quaternion.identity,characterctrl.it.worldCanvas);
        TextInstance = TextGameObjectInstance.GetComponent<Text>();
        talker.said = TextInstance;
    }

    IEnumerator myUpdate () {
        while (true) {
            yield return new WaitForSeconds(1);
            #region updateLogic
            //Крутим вертим непися
               if (approximationDistance != 0 && Mathf.Abs(playerTrns.position.x - transform.position.x) <= approximationDistance) {
                   //Влево / вправо
                   if (playerTrns.position.x > transform.position.x) {
                       if (!right) {
                            transform.localScale = new Vector3(startScale.x,startScale.y,startScale.z);
                            right = true;
                       }
                   }
                   else {
                       if (right) {
                           transform.localScale = new Vector3(-startScale.x,startScale.y,startScale.z);
                           right = false;
                       }
                   }
                   //Вперед / всторону
                   if (Mathf.Abs(playerTrns.position.x - transform.position.x) <= maxApproximationDistance) {
                        if (state) {
                            state = false;
                            sayGreetings();
                            myBodys[0].SetActive(!state);
                            myBodys[1].SetActive(state);
                        }
                    }
               }
               else {
                   if (!state) {
                        state = true;
                        myBodys[0].SetActive(!state);
                        myBodys[1].SetActive(state);
                        talker.Shutdown();
                   }
               }
            #endregion
        }
    }
    void OnDestroy () {
        if (TextGameObjectInstance != null) {
            Destroy(TextGameObjectInstance,3f);
        }
    }
    void sayGreetings () {
        Debug.Log("said");
        talker.SayThis(greetings);
    }
}
