using UnityEngine;

public class LightAnimator : MonoBehaviour
{
    [SerializeField] Light _light;
    Color defaultColor;
    float intensityMultiplier = 1.0f;

    private void Awake()
    {
        defaultColor = _light.color;
    }

    void Update()
    {
        intensityMultiplier = Mathf.SmoothStep(intensityMultiplier, 1.0f, Time.deltaTime);
        _light.intensity = intensityMultiplier * (1.0f + Mathf.PingPong(Time.time, 1.0f));
        _light.color = Color.Lerp(_light.color, defaultColor, Time.deltaTime);
    }

    public void FlashPlayerColor(int id)
    {
        Color c = Locator.PlayerManager.GetPlayerColor(id);
        _light.color = c;
        intensityMultiplier = 2.0f;
        //iTween.ColorFrom(gameObject, c, 1.0f);
    }
}
