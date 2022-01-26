using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleRenderer : MonoBehaviour
{
    [SerializeField]
    private float _radius = 1;

    [SerializeField]
    private int _pointCount = 12;

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = this.GetRequiredComponent<LineRenderer>();
    }

    private void Start()
    {
        Redraw();
    }

    private void OnValidate()
    {
        _pointCount = Mathf.Max(_pointCount, 2);
        Redraw();
    }

    [ContextMenu("Redraw")]
    private void Redraw()
    {
        if (!_lineRenderer)
            _lineRenderer = this.GetRequiredComponent<LineRenderer>();


        float slice = (Mathf.PI * 2) / _pointCount;

        Vector3[] points = new Vector3[_pointCount];
        for (int i = 0; i < points.Length; ++i)
        {
            points[i] = PointOnCircle(slice * i);
        }

        _lineRenderer.positionCount = _pointCount;
        _lineRenderer.SetPositions(points);
    }

    private Vector3 PointOnCircle(float radians)
    {
        float x = Mathf.Cos(radians) * _radius;
        float y = Mathf.Sin(radians) * _radius;
        return new Vector3(x, y, 0);
    }

    public LineRenderer GetLineRenderer() => _lineRenderer;
}
