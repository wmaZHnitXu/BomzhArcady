using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fridge : MonoBehaviour
{
    [SerializeField]
    private bool grounded;
    [SerializeField]
    private Sprite[] openClose;
    [SerializeField]
    private float minY;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private float fallingSpeed;
    private void OnEnable() {
        StartCoroutine(Descent());
    }

    private IEnumerator Descent () {
        transform.rotation = Quaternion.Euler(0,0,Random.Range(45,-45));
        while (transform.position.y > minY) {
            transform.position -= transform.up * fallingSpeed;
            yield return new WaitForFixedUpdate();
        }
        spriteRenderer.sprite = openClose[1];
        fxHub.me.FridgeGrounded(transform.position);
        grounded = true;
    }
}
