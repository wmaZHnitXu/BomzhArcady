using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fxHub : MonoBehaviour
{
    public ParticleSystem[] particlesForInit;
    [SerializeField]
    private Transform[] transforms;
    public static ParticleSystem blood;
    public static Transform bloodTrns;
    public Text[] hitTexts;
    public Transform[] hitTransforms;
    public Animator[] hitAnims;
    public int hitCounter;
    public static fxHub me;
    public float hitTextSpread;
    private bool _shake;
    [SerializeField]
    public bool Shake {
        get {
            return _shake;
        }
        set {
            if (value) {
                if (!_shake) {
                    _shake = true;
                    StartCoroutine(Shaker());
                }
                else
                {
                   //shakeRadius = 1; 
                }
            }
            _shake = value;
        }
    }
    private Transform camTrns;
    private Vector2 camVector;
    private Vector3 defCamPosition;
    [SerializeField]
    private float shakeRadius;

    private static readonly int Start1 = Animator.StringToHash("start");

    void Start () {
        camTrns = transform;
        me = this;
        blood = particlesForInit[0];
        bloodTrns = blood.gameObject.transform;
        for (int i = 0; i != hitTexts.Length; i++) {
            hitTransforms[i] = hitTexts[i].gameObject.transform;
        }
        defCamPosition = camTrns.localPosition;
        Shake = true;
        for (int i = 0; i < particlesForInit.Length; i++) {
            transforms[i] = particlesForInit[i].transform;
        }
    }
    public static void GiveMeBlood (Vector3 pos) {
        bloodTrns.position = pos;
        blood.Play();

    }
    public static void GiveMeBloodyExplosion (Vector3 pos) {
        me.transforms[1].position = pos;
        me.particlesForInit[1].Play();
    }
    public void EjectHitText (int hit,Vector3 pos) {
        hitTexts[hitCounter].text = hit.ToString();
        hitTransforms[hitCounter].position = new Vector3(Random.Range(hitTextSpread,-hitTextSpread) + pos.x, Random.Range(hitTextSpread,-hitTextSpread) + pos.y,0);
        hitAnims[hitCounter].SetTrigger(Start1);
        hitCounter++;
        if (hitCounter == hitTexts.Length)
            hitCounter = 0;
    }
    public void ShakeMe (float shk) {
        shakeRadius = shk;
        Shake = true;
    }
    IEnumerator Shaker () {
        while (_shake) {
            yield return new WaitForSeconds(0.02f);
            camVector = new Vector2(defCamPosition.x + Random.Range(shakeRadius,-shakeRadius),defCamPosition.x + Random.Range(shakeRadius,-shakeRadius));
            camTrns.localPosition = Vector2.Lerp(camVector,camTrns.localPosition,0.04f);
            shakeRadius -= shakeRadius / 10;   
            if (shakeRadius < 0.01f) {
                Shake = true;
                shakeRadius = 0;
            }
        }
    }
    public void FridgeGrounded (Vector3 particlePos) {
        transforms[2].position = particlePos;
        particlesForInit[2].Play();
    }
}
