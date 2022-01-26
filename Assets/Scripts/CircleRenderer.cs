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

    private void Redraw()
    {
        if (!_lineRenderer)
            _lineRenderer = this.GetRequiredComponent<LineRenderer>();

        float slice = (Mathf.PI * 2) / _pointCount;

        Vector3[] points = new Vector3[_pointCount];
        for (int i = 0; i < points.Length; ++i)
        {
            points[i] = Math.PointOnCircle(slice * i, _radius);
        }

        _lineRenderer.positionCount = _pointCount;
        _lineRenderer.SetPositions(points);
    }

    public LineRenderer GetLineRenderer() => _lineRenderer;
}
