using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletCtrl : MonoBehaviour
{
    private Vector3 v;
    public float speed;
    public Transform shell;
    public Transform bullet;
    public bool forPlayer = true;
    private int targLayer = 12;
    public byte hit = 2;
    public bool right;
    public bool laser;
    public ChildRbInfo cri;
    [SerializeField]
    private SpriteRenderer bulletRenderer;
    [SerializeField]
    private bool needRenderDelay;

    private void Start () {
        bullet.gameObject.layer = forPlayer ? 19 : 20;
        if (!forPlayer)
            v.x = characterctrl.rotation ? speed : -speed;
        else
            v.x = right ? speed : -speed;
        targLayer = forPlayer ? 12 : 13;
        if (!laser) {
            shell.SetParent(null);
            bullet.SetParent(null);
        }
        Destroy(bullet.gameObject,3f);
        Destroy(shell.gameObject,3f);
        Destroy(gameObject,3.5f);
        cri.loaded = true;
        cri.isLaser = laser;
        if (needRenderDelay)
            StartCoroutine(StartRender());
    }

    private void FixedUpdate()
    {
        if (bullet != null)
            bullet.Translate(v);
    }
    public void Stack(Collider2D coll) {
        Rigidbody2D rb = coll.GetComponent<Rigidbody2D>();
        if (rb != null) rb.AddForce((bullet.position - characterctrl.me.position).normalized * (1f * hit));
        hpBase hp = coll.GetComponent<hpBase>();
        if (hp != null) {
            hp.AddHit(hit, bullet.position);
        }
        if (coll.gameObject.layer == 11) fxHub.me.GimmeParticles(particleType.DirtSplash, bullet.position);
        if (laser) return;
        Destroy(bullet.gameObject);
        Destroy(shell.gameObject);
        Destroy(gameObject);
    }
    IEnumerator StartRender () {
        bulletRenderer.enabled = false;
        yield return new WaitForSeconds(0.06f);
        bulletRenderer.enabled = true;
    }
}
