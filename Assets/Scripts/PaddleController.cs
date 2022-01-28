using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleController : MonoBehaviour
{
    private Vector2 _moveInput;

    private float _desiredAngle;
    private float _actualAngle;
    private float _deltaAngle;
    private bool _shouldMove;

    private PlayerInput _playerInput;

    [SerializeField]
    private float _maxSpeed = 1;
    [SerializeField]
    private float _deadzone = 0.1f;
    [SerializeField]
    private float _width = 2;

    private void Update()
    {
        _shouldMove = _moveInput.sqrMagnitude > _deadzone;
        if (_shouldMove)
        {
            _desiredAngle = Mathf.Atan2(_moveInput.y, _moveInput.x);
        }

        Debug.DrawLine(MathHelper.PointOnCircle(_actualAngle - _width * 0.5f, Locator.Arena.Radius), MathHelper.PointOnCircle(_actualAngle + _width * 0.5f, Locator.Arena.Radius), Color.magenta);
    }

    private void FixedUpdate()
    {
        if (!_shouldMove)
            return;

        float oldAngle = _actualAngle;

        float diff = Mathf.DeltaAngle(_actualAngle * Mathf.Rad2Deg, _desiredAngle * Mathf.Rad2Deg) * Mathf.Deg2Rad;
        if (Mathf.Abs(diff) > _maxSpeed)
        {
            _actualAngle += _maxSpeed * Mathf.Sign(diff);
        }
        else
        {
            _actualAngle += diff;
        }

        _deltaAngle = _actualAngle - oldAngle;

        transform.localPosition = MathHelper.PointOnCircle(_actualAngle, Locator.Arena.Radius);
        transform.localRotation = Quaternion.Euler(0, 0, _actualAngle * Mathf.Rad2Deg);
    }

    public void HandleMoveInput(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        if (_playerInput.currentControlScheme == "Keyboard&Mouse")
        {
            Vector2 centroid = Camera.main.WorldToScreenPoint(Vector3.zero);
            _moveInput = _moveInput - centroid;
        }
    }

    public void Setup(PlayerInput playerInput)
    {
        _playerInput = playerInput;
        Debug.LogFormat("Player #{0} has joined", playerInput.playerIndex + 1);
    }

    public bool AngleIsCovered(float angle)
    {
        return Mathf.Abs(Mathf.DeltaAngle(angle * Mathf.Rad2Deg, _actualAngle * Mathf.Rad2Deg)) < _width * 0.5f * Mathf.Rad2Deg;
    }

    public float MaxSpeed => _maxSpeed;
    public float CurrentAngle => _actualAngle;
    public float DeltaAngle => _deltaAngle;
}
