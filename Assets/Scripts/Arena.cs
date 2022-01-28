using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField]
    private float _radius = 20;

    public float Radius => _radius;

    public Vector3 CenterPosition => transform.position;

    public float DistanceFromCenter(Vector3 position)
    {
        return Vector3.Distance(CenterPosition, position);
    }
}
