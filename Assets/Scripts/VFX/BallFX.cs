using UnityEngine;

public class BallFX : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer _trailRenderer;

    private Material _ballMaterial;

    [SerializeField] float ballBounceMultiplier = 1.1f;
    [SerializeField] float paddleBounceMultiplier = 1.1f;

    private void Awake()
    {
        MeshRenderer meshRenderer = this.GetRequiredComponent<MeshRenderer>();
        _ballMaterial = meshRenderer.material;
    }

    public void OnBallHit(Ball ball, PaddleController paddleController)
    {
        // HACK:
        Debug.Assert(Jukebox.instance);
        Jukebox.instance.Flip();

        iTween.PunchScale(ball.gameObject, Vector3.one * ballBounceMultiplier, 0.5f);
        iTween.PunchScale(paddleController.gameObject, Vector3.one * paddleBounceMultiplier, 0.5f);

        SoundType sound = paddleController.PlayerIndex == 0 ? SoundType.Ping : SoundType.Pong;
        SFXManager.PlaySound(sound, paddleController.transform.position);
        ParticleManager.Emit(ParticleType.Spark, paddleController.transform.position);

        Color playerColor = Locator.PlayerManager.GetPlayerColor(paddleController.PlayerIndex);

        if (_trailRenderer)
        {
            _trailRenderer.startColor = playerColor;
            _trailRenderer.endColor = playerColor;
        }

        if (_ballMaterial)
        {
            _ballMaterial.SetColor("_EmissionColor", playerColor);
        }
    }

    public void OnBallExit(Ball ball)
    {
        SFXManager.PlaySound(SoundType.BallOut, ball.transform.position);
        ParticleManager.Emit(ParticleType.Starburst, ball.transform.position + Constants.CAMERA_DIR);
    }
}
