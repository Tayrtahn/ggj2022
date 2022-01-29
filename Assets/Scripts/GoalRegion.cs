using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleRenderer))]
public class GoalRegion : MonoBehaviour
{
    private int _owner;
    private float _midPoint;
    private float _width;
    private CircleRenderer _circleRenderer;

    public int Owner
    {
        get => _owner;
        set => _owner = value;
    }
    public float MidPoint
    {
        get => _midPoint;
        set
        {
            _midPoint = value;
            transform.localRotation = Quaternion.Euler(0, 0, (MidPoint - _width * 0.5f) * Mathf.Rad2Deg);
        }
    }
    public float Width
    {
        get => _width;
        set
        {
            _width = value;
            _circleRenderer.Fill = _width * Mathf.Rad2Deg;
        }
    }

    private void Awake()
    {
        _circleRenderer = this.GetRequiredComponent<CircleRenderer>();
    }

    public bool CheckAngleIsInRegion(float angle)
    {
        return Mathf.Abs(Mathf.DeltaAngle(angle * Mathf.Rad2Deg, _midPoint * Mathf.Rad2Deg)) < _width * Mathf.Rad2Deg;
    }
}
