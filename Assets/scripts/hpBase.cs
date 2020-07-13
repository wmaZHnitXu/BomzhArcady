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
        addHitEvent.Invoke();
        hp -= hit;
    }

    protected void Start()
    {
        if (maxHp == 0) maxHp = hp;
    }

    public virtual void AddHit(int hit, Vector3 punchPos) {
        AddHit(hp);
    }
    public virtual void Death() {
    }
    void OnParticleCollision (GameObject other) {
        AddHit(25);
    }
    public virtual void Heal (int healAmmount) {
        if (hp == maxHp) return;
        fxHub.me.GimmeParticles(particleType.Regen, transform.position);
        int oldhp = hp;
        hp += healAmmount;
        if (hp > maxHp) hp = maxHp;
        fxHub.me.EjectHitText(hp - oldhp, transform.position, true);
    }
}
