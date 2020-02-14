using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class hpBase : MonoBehaviour
{
    public int hp = 100;
    public virtual void addHit(int hit) {
        hp -= hit;
    }
    public virtual void addHit(int hit, Vector3 punchPos) {
        
    }
    public virtual void death() {

    }
    void OnParticleCollision () {
        addHit(25);
    }
}
