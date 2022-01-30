using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper
{
    public static Vector3 PointOnCircle(float radians, float radius)
    {
        return new Vector3(Mathf.Cos(radians) * radius, Mathf.Sin(radians) * radius, 0);
    }

    public static Vector3 LineCircleIntersection(Vector3 lineStart, Vector3 lineEnd, Vector3 circleCenter, float circleRadius)
    {
        Vector3 directionRay = lineEnd - lineStart;
        Vector3 centerToRayStart = lineStart - circleCenter;

        float a = Vector3.Dot(directionRay, directionRay);
        float b = 2 * Vector3.Dot(centerToRayStart, directionRay);
        float c = Vector3.Dot(centerToRayStart, centerToRayStart) - (circleRadius * circleRadius);

        float discriminant = (b * b) - (4 * a * c);
        discriminant = Mathf.Sqrt(discriminant);

        //How far on ray the intersections happen
        float t2 = (-b + discriminant) / (2 * a);
        Debug.Assert(discriminant > 0, "A strange collision happened. Tell Chris about it!");

        return lineStart + (directionRay * t2);
    }
}
