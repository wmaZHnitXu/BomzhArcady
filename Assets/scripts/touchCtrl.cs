using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class touchCtrl : MonoBehaviour
{
    public static touchCtrl me;
    public List<Vector2> touchStart = new List<Vector2>();
    public float scrW, scrH;
     public RectTransform[] joystickRight; // 0 - основа, 1 - стик
     public Text textUp;
    public Vector2 rightStick;
    public Vector2 chistilishe = new Vector2(9999,9999);
    //public bool shoot;
    public bool down;
    public bool right;
    Touch t;
    public List<Transform> itemTrns = new List<Transform>();
    public List <itemCtrl> itemCtrls = new List<itemCtrl>();
    public Vector2 pickRad;

    private void Awake () {
        me = this;
    }

    private void Start () {
        scrW = Screen.width;
        scrH = Screen.height;
    }

    private void LateUpdate()
    {
        foreach (var t in Input.touches) {
            right = false;
             if (t.phase == TouchPhase.Began & !touchStart.Contains(t.position)) //Можно убрать второе условие
                touchStart.Add(t.position);
            if (touchStart[System.Array.IndexOf(Input.touches,t)].x >= scrW * 0.65f & characterctrl.it.withCross) {
                right = true;
            }
            else if (touchStart[System.Array.IndexOf(Input.touches,t)].x <= scrW * 0.33f) {
            }
            //Порядок соблюдать.
            if (t.phase == TouchPhase.Moved) {
                if (right)
                {
                    //Правый (больше трети ширины экрана)
                    rightStick = t.position - touchStart[System.Array.IndexOf(Input.touches, t)];
                    if (rightStick.magnitude > 10f)
                    {
                        joystickRight[0].anchoredPosition = touchStart[System.Array.IndexOf(Input.touches, t)];
                        joystickRight[1].anchoredPosition = t.position;
                        down = true;
                    }
                }
            }
            if (!right) {
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
                    down = false;
                    joystickRight[0].position = chistilishe;
                    joystickRight[1].position = chistilishe;
                }
            }
            //+кастыль с искоренением мертвых тачпозишнов, лень было ковыряться    p.s. commented
            /*while (Input.touchCount < touchStart.Count) {
                touchStart.RemoveAt(0);
            }*/
        }
        if (Input.touchCount == 0 && touchStart.Count > 0 ) {
            touchStart.Clear();
            down = false;
            joystickRight[0].position = chistilishe;
            joystickRight[1].position = chistilishe;
        }
        textUp.text = touchStart.Count.ToString() + Input.touchCount.ToString() + "_" + scrH.ToString(CultureInfo.InvariantCulture) + "x" + scrW.ToString(CultureInfo.InvariantCulture) + " stick:" + rightStick.ToString() + " Fps:" + (1f / Time.fixedUnscaledDeltaTime).ToString(CultureInfo.InvariantCulture) + " magnitude:" + rightStick.magnitude.ToString(CultureInfo.InvariantCulture);
    }
}
