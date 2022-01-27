using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper
{
    public static Vector3 PointOnCircle(float radians, float radius)
    {
        return new Vector3(Mathf.Cos(radians) * radius, Mathf.Sin(radians) * radius, 0);
    }
}
