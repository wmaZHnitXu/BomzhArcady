using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static float squareDistance (this Vector3 v) {
        return Mathf.Abs(v.x > v.y ? v.x : v.y);
    }
}
