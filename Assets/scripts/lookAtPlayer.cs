using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtPlayer : MonoBehaviour
{
    private Transform player;
    private bool right;
    private Vector3 scale;
    private float startX;
    void Start()
    {
        player = characterctrl.me;
        scale = transform.localScale;
        startX = scale.x;
        right = true;
    }

    void Update()
    {
        if (!right & transform.position.x < player.position.x) {
            right = true;
            scale.x = startX;
            transform.localScale = scale;
        }
        if (right & transform.position.x > player.position.x) {
            right = false;
            scale.x = -startX;
            transform.localScale = scale;
        }
    }
}
