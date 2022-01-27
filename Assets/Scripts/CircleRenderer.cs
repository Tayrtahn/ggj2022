using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleRenderer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Radius of the circle")]
    private float _radius = 1;

    [SerializeField]
    [Range(0, 360)]
    [Tooltip("Length of the arc segment in degrees")]
    private float _fill = 360;

    [SerializeField]
    [Tooltip("How many points should be used to draw a full circle")]
    private int _pointCount = 12;

    [SerializeField]
    [Tooltip("Method of smoothing the location of the last point on the circle")]
    private InterpolationMode _interpolationMode;

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
        _pointCount = Mathf.Max(_pointCount, 4);
        Redraw();
    }

    private void Redraw()
    {
        if (!_lineRenderer)
            _lineRenderer = this.GetRequiredComponent<LineRenderer>();

        float sliceSize = Mathf.PI * 2 / _pointCount;
        int sliceCount = Mathf.CeilToInt(_fill * Mathf.Deg2Rad / sliceSize);

        Vector3[] points = new Vector3[sliceCount + 1];
        for (int i = 0; i < sliceCount + 1; ++i)
        {
            points[i] = MathHelper.PointOnCircle(sliceSize * i, _radius);
        }

        switch (_interpolationMode)
        {
            case InterpolationMode.Circle:
                points[sliceCount] = MathHelper.PointOnCircle(_fill * Mathf.Deg2Rad, _radius);
                break;
            case InterpolationMode.Line:
                Vector3 before = points[sliceCount - 1];
                Vector3 after = points[sliceCount];
                float remainder = ((_fill * Mathf.Deg2Rad) % sliceSize) / sliceSize;
                points[sliceCount] = Vector3.Lerp(before, after, remainder);
                break;
        }

        _lineRenderer.positionCount = sliceCount + 1;
        _lineRenderer.SetPositions(points);

        _lineRenderer.loop = (_fill == 360);
    }

    public LineRenderer GetLineRenderer() => _lineRenderer;

    public enum InterpolationMode
    {
        Line,
        Circle
    }
}
