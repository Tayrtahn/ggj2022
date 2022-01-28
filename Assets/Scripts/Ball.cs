using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private float _startSpeed = 1;

    private Vector3 _velocity;
    private SphereCollider _sphereCollider;

    public float Speed
    {
        get
        {
            return _velocity.magnitude;
        }
        set
        {
            _velocity = _velocity.normalized * value;
        }
    }
    public float Radius => _sphereCollider.radius;

    private void Awake()
    {
        _sphereCollider = this.GetRequiredComponent<SphereCollider>();
    }

    private void Start()
    {
        _velocity = Random.insideUnitCircle.normalized * _startSpeed;
    }

    private void FixedUpdate()
    {
        Move();

        UpdateRolling();
    }

    private void Move()
    {
        Vector3 movement = _velocity * Time.deltaTime;
        Vector3 center = Locator.Arena.CenterPosition;
        float radius = Locator.Arena.Radius;
        Vector3 rayStart = transform.position;
        Vector3 rayEnd = transform.position + movement;
        
        // Find the point we'll hit the circle, eventually
        Vector3 hitPoint = MathHelper.LineCircleIntersection(rayStart, rayEnd, center, radius);

        // Make sure we'll reach the point this update
        float distanceToHit = Vector3.Distance(rayStart, hitPoint);
        if (distanceToHit < movement.magnitude)
        {
            float hitAngle = Mathf.Atan2(hitPoint.y, hitPoint.x);
            PaddleController[] paddles = Locator.PlayerManager.GetPaddleControllers();
            bool isHit = false;
            foreach (PaddleController paddle in paddles)
            {
                if (paddle == null)
                    continue;

                if (paddle.AngleIsCovered(hitAngle))
                {
                    isHit = true;
                    float remainingMovement = movement.magnitude - distanceToHit;
                    Vector3 paddleLocation = MathHelper.PointOnCircle(paddle.CurrentAngle, Locator.Arena.Radius);
                    Vector3 paddleNormal = (paddleLocation - center).normalized;
                    Vector3 wallNormal = (hitPoint - center).normalized;
                    Vector3 normalDiff = paddleNormal - wallNormal;
                    Vector3 reflectNormal = wallNormal + normalDiff * paddle.BounceAngleExaggeration;
                    Vector3 reflectedDirection = Vector3.Reflect(movement, reflectNormal);
                    Vector3 movementAfterHit = reflectedDirection * remainingMovement;
                    transform.position = hitPoint + movementAfterHit;
                    _velocity = Vector3.Reflect(_velocity, reflectNormal) * paddle.BounceSpeedMultiplier;

                    SFXManager.PlaySound(SoundType.Ping, transform.position);
                }
            }
            if (!isHit)
            {
                // Missed the paddle
                transform.position += movement;
            }
        }
        else
        {
            // Moving within the arena
            transform.position += movement;
        }
    }

    private void UpdateRolling()
    {
        Vector3 movement = _velocity * Time.deltaTime;
        float distance = movement.magnitude;
        float angle = distance * (180f / Mathf.PI) / _sphereCollider.radius;
        Vector3 rotationAxis = Vector3.Cross(Vector3.back, movement).normalized;
        transform.localRotation = Quaternion.Euler(rotationAxis * angle) * transform.localRotation;
    }
}
