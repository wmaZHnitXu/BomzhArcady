using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class areaBlocker : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] sprites;
    [SerializeField] private float[] bounds;
    private float dist;
    void Update()
    {
        dist = Mathf.Abs(characterctrl.me.position.x - bounds[0]);
        sprites[0].color = new Color(sprites[0].color.r, sprites[0].color.g, sprites[0].color.b, Mathf.Clamp(2 - dist * 0.5f, 0, 1));
        dist = Mathf.Abs(characterctrl.me.position.x - bounds[1]);
        sprites[1].color = new Color(sprites[1].color.r, sprites[1].color.g, sprites[1].color.b, Mathf.Clamp(2 -  dist * 0.5f, 0, 1));
    }
}
