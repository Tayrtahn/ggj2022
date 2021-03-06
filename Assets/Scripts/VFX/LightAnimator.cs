using UnityEngine;

public class LightAnimator : MonoBehaviour
{
    [SerializeField] Light _light;
    Color defaultColor;
    float intensityMultiplier = 1.0f;

    [SerializeField]
    private float _rotationSpeed = 1;
    private Quaternion _targetDirection;

    private void Awake()
    {
        defaultColor = _light.color;
        _targetDirection = transform.rotation;
    }

    void Update()
    {
        intensityMultiplier = Mathf.SmoothStep(intensityMultiplier, 1.0f, 5.0f * Time.deltaTime);
        _light.intensity = intensityMultiplier * (1.0f + Mathf.PingPong(Time.time, 1.0f));
        _light.color = Color.Lerp(_light.color, defaultColor, Time.deltaTime);

        transform.rotation = Quaternion.Slerp(transform.rotation, _targetDirection, Time.deltaTime * _rotationSpeed);
    }

    public void FlashPlayerColor(PaddleController paddleController, int id)
    {
        if (!paddleController || paddleController.PlayerIndex == id)
        {
            intensityMultiplier = 0.0f;
            return;
        }

        int scorer = paddleController.PlayerIndex;
        Color c = Locator.PlayerManager.GetPlayerColor(scorer);
        _light.color = c;
        intensityMultiplier = 2.0f;
        //iTween.ColorFrom(gameObject, c, 1.0f);
    }

    public void OnBallHit(Ball ball, PaddleController paddleController)
    {
        Vector3 direction = (paddleController.transform.position - transform.position).normalized;
        _targetDirection = Quaternion.LookRotation(direction, Vector3.back);
    }
}
