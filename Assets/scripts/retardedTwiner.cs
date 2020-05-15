using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum AnimationType {
        classicLerp,
        linear,
        pizedc
    }
public class retardedTwiner : MonoBehaviour
{
    public static retardedTwiner me;
    private void Start() {
        me = this;    
    }
    public void CallAnimation(RectTransform animatedTrans, Vector2 localDestination, float speed, UnityAction OnEnded, float waitTime) {
        StartCoroutine(Animate(animatedTrans, localDestination, AnimationType.classicLerp, speed, OnEnded, waitTime));
    }
    private IEnumerator Animate (RectTransform animatedTrans, Vector2 localDestination, AnimationType animType, float speed, UnityAction OnEnded, float waitTime) {
        while (animatedTrans.anchoredPosition != localDestination) {
            switch (animType) {
                case AnimationType.classicLerp:
                    animatedTrans.anchoredPosition = Vector2.Distance(animatedTrans.anchoredPosition,localDestination) > 0.5f //Иначе это безумие не прекратится
                    ? Vector2.Lerp(animatedTrans.anchoredPosition, localDestination, speed)
                    : localDestination;
                    
                break;
                case AnimationType.linear:
                    animatedTrans.anchoredPosition = Vector2.MoveTowards(animatedTrans.anchoredPosition, localDestination, speed);
                break;
                case AnimationType.pizedc:
                 animatedTrans.anchoredPosition = Vector2.Lerp(animatedTrans.anchoredPosition, localDestination, speed);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(waitTime);
        if (OnEnded != null)
            OnEnded();
    }
}
