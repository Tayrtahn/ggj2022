using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleModelGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject _middle;
    [SerializeField]
    private GameObject _top;
    [SerializeField]
    private GameObject _bottom;

    [SerializeField]
    private float _width;

    public void SetWidth(float width)
    {
        _width = width;
        Redraw();
    }

    private void OnValidate()
    {
        Redraw();
    }

    private void Redraw()
    {
        _middle.transform.localScale = new Vector3(1, 1, _width);
        _top.transform.localPosition = new Vector3(0, -1 + _width, 0);
        _bottom.transform.localPosition = new Vector3(0, 1 -_width, 0);
    }
}
