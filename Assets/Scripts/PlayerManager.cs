using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        PaddleController controller = playerInput.GetComponent<PaddleController>();
        controller.Setup(playerInput);
    }
}
