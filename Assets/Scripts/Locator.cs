using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locator : MonoBehaviour
{
    private static Arena _arena;
    private static PlayerManager _playerManager;

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
        _playerManager = FindObjectOfType<PlayerManager>();
    }

    private void ClearReferences()
    {
        _arena = null;
        _playerManager = null;
    }

    public static Arena Arena => _arena;
    public static PlayerManager PlayerManager => _playerManager;
}
