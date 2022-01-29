using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _ballPrefab;

    [SerializeField]
    private GameObject _goalPrefab;

    private PlayerManager _playerManager;
    private Arena _arena;

    private bool _gameStarted;

    private List<Ball> _balls = new List<Ball>();
    private List<GoalRegion> _goals = new List<GoalRegion>();

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
        ball.OnHitPaddle.AddListener(OnBallHitPaddle);
    }

    public void CreateGoalRegion(PaddleController owner)
    {
        GameObject goalGO = Instantiate(_goalPrefab);
        GoalRegion goal = goalGO.GetRequiredComponent<GoalRegion>();
        goal.Owner = owner.PlayerIndex;
        goal.MidPoint = owner.CurrentAngle;
        goal.Width = Mathf.PI;
        _goals.Add(goal);
    }

    public void OnBallExitedArena(Ball ball)
    {
        float angle = Mathf.Atan2(ball.transform.position.y, ball.transform.position.x);
        foreach (GoalRegion goal in _goals)
        {
            if (goal.CheckAngleIsInRegion(angle))
            {
                Debug.LogFormat("Hit goal for player {0}", goal.Owner + 1);
            }
        }
        _balls.Remove(ball);
        LaunchBall();
    }

    public void OnBallHitPaddle(Ball ball, PaddleController paddle)
    {
        GoalRegion goal = GetGoalForPlayer(paddle.PlayerIndex);
        if (goal != null)
        {
            goal.MidPoint = paddle.CurrentAngle;
        }
    }

    private GoalRegion GetGoalForPlayer(int playerIndex)
    {
        foreach (GoalRegion goal in _goals)
        {
            if (goal.Owner == playerIndex)
                return goal;
        }
        return null;
    }

    public List<GoalRegion> GetGoalRegions()
    {
        return _goals;
    }
}
