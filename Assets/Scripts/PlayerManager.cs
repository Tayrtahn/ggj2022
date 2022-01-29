using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private PaddleController[] _paddleControllers;

    [SerializeField]
    private int _maxPlayers = 2;

    [SerializeField]
    [Tooltip("Minimum number of players needed to start game")]
    private int _minPlayers = 2;

    private void OnValidate()
    {
        if (_paddleControllers == null)
        {
            _paddleControllers = new PaddleController[_maxPlayers];
        }
        else
        {
            System.Array.Resize(ref _paddleControllers, _maxPlayers);
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        PaddleController controller = playerInput.GetComponent<PaddleController>();

        _paddleControllers[playerInput.playerIndex] = controller;

        controller.Setup(playerInput);

        Locator.GameManager.CreateGoalRegion(controller);
    }

    public int JoinedPlayerCount
    {
        get
        {
            int count = 0;
            for (int i = 0; i < _paddleControllers.Length; ++i)
            {
                if (_paddleControllers[i] != null)
                    ++count;
            }
            return count;
        }
    }

    public bool HasEnoughPlayers()
    {
        return JoinedPlayerCount >= _minPlayers;
    }

    public PaddleController[] GetPaddleControllers() => _paddleControllers;
}
