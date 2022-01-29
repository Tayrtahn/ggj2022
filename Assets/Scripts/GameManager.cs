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
    private GoalRegion[] _goals;

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
            CreateGoalRegions();
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

    private void CreateGoalRegions()
    {
        int numPlayers = Mathf.Max(2, Locator.PlayerManager.JoinedPlayerCount);
        _goals = new GoalRegion[numPlayers];
        float sliceSize = (Mathf.PI * 2) / numPlayers;
        for (int i = 0; i < numPlayers; ++i)
        {
            float midpoint = i * sliceSize + sliceSize * 0.5f;
            GoalRegion goal = CreateGoalRegion(i, midpoint, sliceSize);
            _goals[i] = goal;
        }
    }

    public GoalRegion CreateGoalRegion(int playerIndex, float midpoint, float width)
    {
        GameObject goalGO = Instantiate(_goalPrefab);
        GoalRegion goal = goalGO.GetRequiredComponent<GoalRegion>();
        goal.Owner = playerIndex;
        goal.MidPoint = midpoint;
        goal.Width = width;
        goal.SetColor(Locator.PlayerManager.GetPlayerColor(playerIndex));
        return goal;
    }

    private void AdjustGoalRegions(PaddleController paddle)
    {
        int numPlayers = Mathf.Max(2, Locator.PlayerManager.JoinedPlayerCount);
        float sliceSize = (Mathf.PI * 2) / numPlayers;

        GoalRegion hitterGoal = GetGoalForPlayer(paddle.PlayerIndex);
        hitterGoal.MidPoint = paddle.CurrentAngle;
        hitterGoal.Width = sliceSize;

        int otherPlayerCount = numPlayers - 1;
        for (int i = 0; i < _goals.Length; ++i)
        {
            if (i == paddle.PlayerIndex)
                continue;
            
            _goals[i].Width = sliceSize;
            float offset = sliceSize * i;
            _goals[i].MidPoint = paddle.CurrentAngle + offset;
        }
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
        AdjustGoalRegions(paddle);
    }

    private GoalRegion GetGoalForPlayer(int playerIndex)
    {
        if (_goals == null)
            return null;
        return _goals[playerIndex];
    }

    public GoalRegion[] GetGoalRegions()
    {
        return _goals;
    }
}
