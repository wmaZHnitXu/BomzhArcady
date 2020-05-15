using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class npcGoCtrl : MonoBehaviour
{
    [SerializeField]
    private float speed;
    public float Target {
        get {
            return target;
        }
        set {
            target = value;
            targetVector3 = new Vector3(value,transform.position.y,transform.position.z);
        }
    }
    private float target;
    [SerializeField]
    private bool right;
    [SerializeField]
    private Animator[] legs;
    private bool idem;
    public Vector3 targetVector3;
    private Vector3 startScale;
    private bool stopped;
    public UnityEvent imHere;
    private Transform mother; //Если катаются на машинках.
    private bool going;
    private Vector3 motherRelativePos;

    void Awake()
    {
        mother = transform.parent;
        if (mother != null)
            motherRelativePos = transform.localPosition;
        startScale = transform.localScale;
        stopped = true;
        Target = 120;
    }

    void FixedUpdate()
    {   if (going) {
            if (targetVector3 != transform.position) {
                if (stopped) {
                    stopped = false;
                    legs[0].enabled = true;
                    legs[1].enabled = true;
                }
                if (targetVector3.x < transform.position.x) {
                    if (!right) {
                        right = true;
                        transform.localScale = startScale;
                    }
                }
                else if (right) {
                    right = false;
                    transform.localScale = new Vector3 (-startScale.x,startScale.y,startScale.z);
                }
                transform.position = Vector3.MoveTowards(transform.position,targetVector3,speed);
            }
            else if (!stopped) {
                stopped = true;
                imHere.Invoke();
                legs[0].enabled = false;
                legs[1].enabled = false;
            }
        }
    }
    public void StartStopGoing (bool b) {
        going = b;
        if (b) {
            transform.SetParent(null);
        }
        else {
            transform.SetParent(mother);
            legs[0].enabled = false;
            legs[1].enabled = false;
            stopped = true;
        }
    }
    public void ReturnMe () {
        StartCoroutine(_returnToMother());
    }
    IEnumerator _returnToMother () {
        targetVector3 = new Vector3(mother.position.x,transform.position.y,transform.position.z);

        while (targetVector3 != transform.position) {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.5f);
        targetVector3 = mother.TransformPoint(motherRelativePos);

        while (targetVector3 != transform.position) {
            yield return new WaitForFixedUpdate();
        }
        StartStopGoing(false);
    }
}
