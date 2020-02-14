using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class touchCtrl : MonoBehaviour
{
    public static touchCtrl me;
    public List<Vector2> touchStart = new List<Vector2>();
    public float scrW, scrH;
     public RectTransform[] joystickRight; // 0 - основа, 1 - стик
    public RectTransform[] joystickLeft;
    public Text textUp;
    public Vector2 rightStick;
    public Vector2 leftStick;
    public Vector2 chistilishe = new Vector2(9999,9999);
    //public bool shoot;
    public bool[] down;
    public bool right;
    public bool left;
    Touch t;
    public List<Transform> itemTrns = new List<Transform>();
    public List <itemCtrl> itemCtrls = new List<itemCtrl>();
    public Vector2 pickRad;
    void Awake () {
        me = this;
    }
    void Start () {
        scrW = Screen.width;
        scrH = Screen.height;
    }
    void LateUpdate()
    {
        foreach (Touch t in Input.touches) {
            left = false;
            right = false;
             if (t.phase == TouchPhase.Began & !touchStart.Contains(t.position)) //Можно убрать второе условие
                touchStart.Add(t.position);
            if (touchStart[System.Array.IndexOf(Input.touches,t)].x >= scrW * 0.65f & characterctrl.it.withCross) {
                right = true;
            }
            else if (touchStart[System.Array.IndexOf(Input.touches,t)].x <= scrW * 0.33f) {
                left = true;
            }
            //Порядок соблюдать.
            if (t.phase == TouchPhase.Moved) {
                if (right) { //Правый (больше трети ширины экрана)
                    rightStick = t.position - touchStart[System.Array.IndexOf(Input.touches,t)];
                    if (rightStick.magnitude > 10f) {
                      joystickRight[0].anchoredPosition = touchStart[System.Array.IndexOf(Input.touches,t)];
                      joystickRight[1].anchoredPosition = t.position;
                      down[1] = true;
                  }
                }
                else if (left) {
                  leftStick = t.position - touchStart[System.Array.IndexOf(Input.touches,t)];
                    if (leftStick.magnitude > 10f) {
                    joystickLeft[0].anchoredPosition = touchStart[System.Array.IndexOf(Input.touches,t)];
                    joystickLeft[1].anchoredPosition = t.position;
                    down[0] = true;
                  }
                }
            }
            if (!right & !left) {
                for (int i = 0; i < itemTrns.Count; i++) {
                    if (Mathf.Abs(itemTrns[i].position.x - Camera.main.ScreenToWorldPoint(t.position).x) <= pickRad.x & Mathf.Abs(itemTrns[i].position.y - Camera.main.ScreenToWorldPoint(t.position).y) <= pickRad.y)
                    itemCtrls[i].CallBack();
                    itemCtrls.RemoveAt(i);
                    itemTrns.RemoveAt(i);
                }
            }
            //Порядок соблюдать.
            if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) {
                touchStart.RemoveAt(System.Array.IndexOf(Input.touches,t));
                if (right) {
                    down[1] = false;
                    joystickRight[0].position = chistilishe;
                    joystickRight[1].position = chistilishe;
                }
                if (left) {
                    down[0] = false;
                    joystickLeft[0].position = chistilishe;
                    joystickLeft[1].position = chistilishe;
                }
            }
            //+кастыль с искоренением мертвых тачпозишнов, лень было ковыряться    p.s. commented
            /*while (Input.touchCount < touchStart.Count) {
                touchStart.RemoveAt(0);
            }*/
      }
        if (Input.touchCount == 0 && touchStart.Count > 0 ) {
            touchStart.Clear();
            down[0] = false;
            down[1] = false;
            joystickLeft[0].position = chistilishe;
            joystickLeft[1].position = chistilishe;
            joystickRight[0].position = chistilishe;
            joystickRight[1].position = chistilishe;
        }
        textUp.text = touchStart.Count.ToString() + Input.touchCount.ToString() + "_" + scrH.ToString() + "x" + scrW.ToString() + " stick:" + rightStick.ToString() + " Fps:" + (1f / Time.fixedUnscaledDeltaTime).ToString() + " magnitude:" + rightStick.magnitude.ToString();
    }
}
