using UnityEngine;

public class HUDGameOver : MonoBehaviour
{
    [SerializeField] GameObject activateGo;
    [SerializeField] float loadDelay = 3.0f;
    float timer;

    private void OnEnable()
    {
        activateGo.SetActive(true);
        TimeManager.AddTimescaleSource(this, 0.0f);
    }

    private void OnDisable()
    {
        activateGo.SetActive(false);
    }

    void Start()
    {
        timer = loadDelay;
        SFXManager.PlaySound(SoundType.GameOver);
    }

    private void Update()
    {
        if (timer <= 0)
            GotoTitle();
        else
            timer -= Time.unscaledDeltaTime;
    }

    void GotoTitle()
    {
        TimeManager.RemoveTimescaleSource(this);
        SceneManager.Goto(Constants.TITLE_SCENE);
    }
}
