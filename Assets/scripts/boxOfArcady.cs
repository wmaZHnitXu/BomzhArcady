using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxOfArcady : MonoBehaviour
{
    [SerializeField]
    private GameObject interior;
    [SerializeField]
    private GameObject enterButton;
    [SerializeField]
    private GameObject exitButton;
    [SerializeField]
    private float enterPosition;
    private bool nearDoor;
    private Transform player;
    private bool isPlayerInside;
    void Start () {
        player = characterctrl.me;
    }

    public void Enter () {
        interior.SetActive(true);
        enterButton.SetActive(false);
        isPlayerInside = true;
        nearDoor = false;
        characterctrl.it.InBox = true;
        characterctrl.it.detachAllNpcs.Invoke();
    }
    public void Exit () {
        interior.SetActive(false);
        exitButton.SetActive(false);
        isPlayerInside = false;
        nearDoor = false;
        characterctrl.it.InBox = false;
    }
    void Update () {
        if (Mathf.Abs(player.position.x - enterPosition) < 1f) {
            if (!nearDoor) {
                nearDoor = true;
                if (isPlayerInside) {
                    exitButton.SetActive(true);
                }
                else
                {
                    enterButton.SetActive(true);
                }
            }
        }
        else if (nearDoor)
        {
            nearDoor = false;
            if (isPlayerInside) {
                exitButton.SetActive(false);
            }
            else
            {
                enterButton.SetActive(false);
            }
        }
    }
}
