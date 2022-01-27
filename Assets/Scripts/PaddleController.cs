using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleController : MonoBehaviour
{
    private Vector2 _moveInput;

    private float _desiredAngle;
    private float _actualAngle;
    private bool _shouldMove;

    private PlayerInput _playerInput;

    [SerializeField]
    private float _maxSpeed = 1;
    [SerializeField]
    private float _deadzone = 0.1f;

    private void Update()
    {
        _shouldMove = _moveInput.sqrMagnitude > _deadzone;
        if (_shouldMove)
        {
            _desiredAngle = Mathf.Atan2(_moveInput.y, _moveInput.x);
        }
    }

    private void FixedUpdate()
    {
        if (!_shouldMove)
            return;

        float diff = Mathf.DeltaAngle(_actualAngle * Mathf.Rad2Deg, _desiredAngle * Mathf.Rad2Deg) * Mathf.Deg2Rad;
        if (Mathf.Abs(diff) > _maxSpeed)
        {
            _actualAngle += _maxSpeed * Mathf.Sign(diff);
        }
        else
        {
            _actualAngle += diff;
        }

        transform.localRotation = Quaternion.Euler(0, 0, _actualAngle * Mathf.Rad2Deg);
    }

    public void HandleMoveInput(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void Setup(PlayerInput playerInput)
    {
        _playerInput = playerInput;
        Debug.LogFormat("Player #{0} has joined", playerInput.playerIndex + 1);

    }
}
