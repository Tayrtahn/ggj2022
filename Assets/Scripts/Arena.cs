using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField]
    private float _radius = 20;
    [SerializeField]
    private float _spinSpeed = 1.0f;

    public float Radius => _radius;

    public Vector3 CenterPosition => transform.position;

    public float DistanceFromCenter(Vector3 position)
    {
        return Vector3.Distance(CenterPosition, position);
    }

    public void Update()
    {
        transform.Rotate(Vector3.forward * _spinSpeed * Time.deltaTime, Space.World);
    }
}
