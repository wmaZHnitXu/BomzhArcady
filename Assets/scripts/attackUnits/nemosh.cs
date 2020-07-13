using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nemosh : EnemyHp
{
    [SerializeField] protected bool _right = true;
    public bool Right {
        get => _right;
        set {
            if (_right != value) {
                transform.localScale = new Vector3(value ? startScale.x : -startScale.x , startScale.y, startScale.z);
                _right = value;
            }
        }
    }
    protected Vector3 startScale;
    public static List<nemoshWithTransform> nemoshi = new List<nemoshWithTransform>();
    new protected void Start() {
        base.Start();
        characterctrl.NearNpcs.Add(this);
        nemoshi.Add(new nemoshWithTransform(transform, this));
        startScale = transform.localScale;
    }
    public virtual void OnGrab () {
        
    }
    public virtual void OnSpit () {

    }
}
public struct nemoshWithTransform {
    public Transform transform;
    public nemosh nemosh;
    public nemoshWithTransform(Transform t, nemosh h) {
        transform = t;
        nemosh = h;
    }
}
