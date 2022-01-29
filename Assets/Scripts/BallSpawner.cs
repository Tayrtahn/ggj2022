using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] _spawnPoints;
    [SerializeField]
    private float _launchSpeed;

    private Animator _animator;

    private void Awake()
    {
        _animator = this.GetRequiredComponent<Animator>();
    }

    public void Spawn(GameObject ballPrefab)
    {
        StartCoroutine(DoSpawnSequence(ballPrefab));
    }

    private IEnumerator DoSpawnSequence(GameObject ballPrefab)
    {
        Extend();
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("Done"));
        Fire(ballPrefab);
        Retract();
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("Done"));
    }

    private void Extend()
    {
        _animator.SetTrigger("Extend");
    }

    private void Fire(GameObject ballPrefab)
    {
        // This should probably be moved to the ball launcher
        GameObject ballGO = Instantiate(ballPrefab);
        Transform spawnPoint = GetRandomSpawnpoint();
        ballGO.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0);
        Ball ball = ballGO.GetComponent<Ball>();
        Locator.GameManager.RegisterBall(ball);

        Vector3 launcherDirection = (spawnPoint.position - transform.position);
        Vector3 launchDirection = new Vector3(launcherDirection.x, launcherDirection.y, 0).normalized;
        ball.SetVelocity(launchDirection * _launchSpeed);
    }

    private void Retract()
    {
        _animator.SetTrigger("Retract");
    }

    private Transform GetRandomSpawnpoint()
    {
        int index = Random.Range(0, _spawnPoints.Length);
        return _spawnPoints[index];
    }
}
