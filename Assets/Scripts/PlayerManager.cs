using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private PaddleController[] paddleControllers;

    [SerializeField]
    private int _maxPlayers = 2;

    [SerializeField]
    [Tooltip("Minimum number of players needed to start game")]
    private int _minPlayers = 2;

    private void OnValidate()
    {
        if (paddleControllers == null)
        {
            paddleControllers = new PaddleController[_maxPlayers];
        }
        else
        {
            System.Array.Resize(ref paddleControllers, _maxPlayers);
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        PaddleController controller = playerInput.GetComponent<PaddleController>();

        paddleControllers[playerInput.playerIndex] = controller;

        controller.Setup(playerInput);
    }

    public int JoinedPlayerCount
    {
        get
        {
            int count = 0;
            for (int i = 0; i < paddleControllers.Length; ++i)
            {
                if (paddleControllers[i] != null)
                    ++count;
            }
            return count;
        }
    }

    public bool HasEnoughPlayers()
    {
        return JoinedPlayerCount >= _minPlayers;
    }
}
