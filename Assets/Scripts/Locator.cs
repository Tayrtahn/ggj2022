using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locator : MonoBehaviour
{
    private static Arena _arena;
    private static PlayerManager _playerManager;
    private static GameManager _gameManager;

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
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void ClearReferences()
    {
        _arena = null;
        _playerManager = null;
        _gameManager = FindObjectOfType<GameManager>();
    }

    public static Arena Arena => _arena;
    public static PlayerManager PlayerManager => _playerManager;
    public static GameManager GameManager => _gameManager;
}
