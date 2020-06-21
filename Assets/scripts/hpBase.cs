using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class hpBase : MonoBehaviour
{
    public string nameOfNpc;
    public int hp = 100;
    public int maxHp;
    [SerializeField] public UnityEvent addHitEvent;
    public virtual void AddHit(int hit) {
        hp -= hit;
    }

    protected void Start()
    {
        if (maxHp == 0) maxHp = hp;
    }

    public virtual void AddHit(int hit, Vector3 punchPos) {
        
    }
    public virtual void Death() {
    }
    void OnParticleCollision (GameObject other) {
        AddHit(25);
    }
}
