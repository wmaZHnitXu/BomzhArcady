using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nemosh : hpBase
{
    protected bool _right = true;
    public bool Right {
        get => _right;
        set {
            if (_right != value) {
                transform.localScale = new Vector3(startScale.x, startScale.y, startScale.z);
                _right = value;
            }
        }
    }
    protected Vector3 startScale;
    public static List<hpWithTransform> nemoshi = new List<hpWithTransform>();
    public nemosh() {
        nemoshi.Add(new hpWithTransform(transform, this));
        startScale = transform.localScale;
    }
    public virtual void OnGrab () {
        
    }
    public virtual void OnSpit () {

    }
}
public struct hpWithTransform {
    Transform transform;
    hpBase hp;
    public hpWithTransform(Transform t, hpBase h) {
        transform = t;
        hp = h;
    }
}
