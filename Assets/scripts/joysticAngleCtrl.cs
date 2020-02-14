using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joysticAngleCtrl : MonoBehaviour
{
    public Transform arcady;
    [SerializeField]
    private Transform stick;
    [SerializeField]
    private Transform parent;
    [SerializeField]
    private Transform cross;
    private bool down;
    private Vector3 correct;
    [SerializeField]
    private float radius;
    private Quaternion q;
    public static bool shoot;
    void Start()
    {
        parent = stick.parent;
    }

    void FixedUpdate()
    {
        if (down) {
            correct = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            correct.z = stick.position.z;
            stick.position = Vector3.MoveTowards(parent.position, correct, radius);
            cross.position = stick.localPosition + arcady.position;
            if (Vector3.Distance(parent.position,correct) > 1.5) {
                shoot = true;    
            }
            else
                shoot = false;
        }
    }
    void OnMouseDown () {
        down = true;
    }
    void OnMouseUp () {
        down = false;
        shoot = false;
        stick.localPosition = new Vector2(0,0);
        cross.position = arcady.position;
    }
    void OnDisable () {
        shoot = false;
    }
}
