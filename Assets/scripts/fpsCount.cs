using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fpsCount : MonoBehaviour
{
    public Text targ;
    void Update()
    {
        targ.text = (1f / Time.unscaledDeltaTime).ToString();
    }
}
