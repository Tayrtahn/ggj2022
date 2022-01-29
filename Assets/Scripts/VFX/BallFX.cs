using UnityEngine;

public class BallFX : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer _trailRenderer;

    [SerializeField] float ballBounceMultiplier = 1.1f;
    [SerializeField] float paddleBounceMultiplier = 1.1f; 

    public void OnBallHit(Ball ball, PaddleController paddleController)
    {
        iTween.PunchScale(ball.gameObject, Vector3.one * ballBounceMultiplier, 0.5f);
        iTween.PunchScale(paddleController.gameObject, Vector3.one * paddleBounceMultiplier, 0.5f);

        SoundType sound = paddleController.PlayerIndex == 0 ? SoundType.Ping : SoundType.Pong;
        SFXManager.PlaySound(sound, paddleController.transform.position);
        ParticleManager.Emit(ParticleType.Spark, paddleController.transform.position);

        if (_trailRenderer)
        {
            _trailRenderer.startColor = Locator.PlayerManager.GetPlayerColor(paddleController.PlayerIndex);
            _trailRenderer.endColor = Locator.PlayerManager.GetPlayerColor(paddleController.PlayerIndex);
        }
    }

    public void OnBallExit(Ball ball)
    {
        SFXManager.PlaySound(SoundType.Applause);
        ParticleManager.Emit(ParticleType.Starburst, ball.transform.position);
    }
}
