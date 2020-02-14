using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fxHub : MonoBehaviour
{
    public ParticleSystem[] particlesForInit;
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
    public bool shake {
        get {
            return _shake;
        }
        set {
            if (value) {
                if (!_shake) {
                    _shake = true;
                    StartCoroutine(shaker());
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

    void Start () {
        camTrns = transform;
        me = this;
        blood = particlesForInit[0];
        bloodTrns = blood.gameObject.transform;
        for (int i = 0; i != hitTexts.Length; i++) {
            hitTransforms[i] = hitTexts[i].gameObject.transform;
        }
        defCamPosition = camTrns.localPosition;
        shake = true;
    }
    public static void GiveMeBlood (Vector3 pos) {
        bloodTrns.position = pos;
        blood.Play();

    }
    public void EjectHitText (int hit,Vector3 pos) {
        hitTexts[hitCounter].text = hit.ToString();
        hitTransforms[hitCounter].position = new Vector3(Random.Range(hitTextSpread,-hitTextSpread) + pos.x, Random.Range(hitTextSpread,-hitTextSpread) + pos.y,0);
        hitAnims[hitCounter].SetTrigger("start");
        hitCounter++;
        if (hitCounter == hitTexts.Length)
            hitCounter = 0;
    }
    public void ShakeMe (float shk) {
        shakeRadius = shk;
        shake = true;
    }
    IEnumerator shaker () {
        while (_shake) {
            yield return new WaitForSeconds(0.02f);
            camVector = new Vector2(defCamPosition.x + Random.Range(shakeRadius,-shakeRadius),defCamPosition.x + Random.Range(shakeRadius,-shakeRadius));
            camTrns.localPosition = Vector2.Lerp(camVector,camTrns.localPosition,0.04f);
            shakeRadius -= shakeRadius / 10;   
            if (shakeRadius < 0.01f) {
                shake = true;
                shakeRadius = 0;
            }
        }
    }
}
