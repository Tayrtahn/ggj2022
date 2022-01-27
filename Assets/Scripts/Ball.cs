using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector3 _velocity;
    private SphereCollider _sphereCollider;

    public float Speed
    {
        get
        {
            return _velocity.magnitude;
        }
        set
        {
            _velocity = _velocity.normalized * value;
        }
    }

    private void Awake()
    {
        _sphereCollider = this.GetRequiredComponent<SphereCollider>();
    }

    private void Start()
    {
        _velocity = Random.insideUnitCircle.normalized;
    }

    private void FixedUpdate()
    {
        Vector3 movement = _velocity * Time.deltaTime;
        transform.position += movement;

        float distance = movement.magnitude;
        float angle = distance * (180f / Mathf.PI) / _sphereCollider.radius;
        Vector3 rotationAxis = Vector3.Cross(Vector3.back, movement).normalized;
        transform.localRotation = Quaternion.Euler(rotationAxis * angle) * transform.localRotation;
    }
}
