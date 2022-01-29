using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] _spawnPoints;
    [SerializeField]
    private float _launchSpeed;

    public Ball Spawn(GameObject ballPrefab)
    {
        // This should probably be moved to the ball launcher
        GameObject ballGO = Instantiate(ballPrefab);
        Transform spawnPoint = GetRandomSpawnpoint();
        ballGO.transform.position = spawnPoint.position;
        Ball ball = ballGO.GetComponent<Ball>();
        Locator.GameManager.RegisterBall(ball);

        Vector3 launcherDirection = (spawnPoint.position - transform.position);
        Vector3 launchDirection = Vector3.ProjectOnPlane(launcherDirection, Vector3.back);
        ball.SetVelocity(launchDirection * _launchSpeed);

        return ball;
    }

    private Transform GetRandomSpawnpoint()
    {
        int index = Random.Range(0, _spawnPoints.Length - 1);
        return _spawnPoints[index];
    }
}
