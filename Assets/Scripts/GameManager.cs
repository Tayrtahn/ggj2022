using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _ballPrefab;

    private PlayerManager _playerManager;
    private Arena _arena;

    private bool _gameStarted;

    private List<Ball> _balls = new List<Ball>();

    public UnityEvent OnGameStarted;

    private void Start()
    {
        _playerManager = Locator.PlayerManager;
        _arena = Locator.Arena;
    }

    private void Update()
    {
        if (!_gameStarted && _playerManager.HasEnoughPlayers())
        {
            _gameStarted = true;
            // Everyone's excited
            SFXManager.PlaySound(SoundType.Applause);
            OnGameStarted.Invoke();

            LaunchBall();
        }
    }

    private void LaunchBall()
    {
        // This should probably be moved to the ball launcher
        GameObject ballGO = Instantiate(_ballPrefab);
        Ball ball = ballGO.GetComponent<Ball>();
        _balls.Add(ball);
        ball.OnExitArena.AddListener(OnBallExitedArena);
    }

    public void OnBallExitedArena(Ball ball)
    {
        _balls.Remove(ball);
        Debug.Log("Ball out!");
        LaunchBall();
    }
}
