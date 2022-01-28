using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locator : MonoBehaviour
{
    private static Arena _arena;

    private void Awake()
    {
        FindSceneReferences();
    }

    private void OnDestroy()
    {
        ClearReferences();
    }

    private void FindSceneReferences()
    {
        _arena = FindObjectOfType<Arena>();
    }

    private void ClearReferences()
    {
        _arena = null;
    }

    public static Arena Arena => _arena;
}
