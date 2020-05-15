using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class extensionMethods
{
    public static float SquareDistance (this Vector3 v)
    {
        return Mathf.Abs(Mathf.Abs(v.x) > Mathf.Abs(v.y) ? v.x : v.y);
    }
}
