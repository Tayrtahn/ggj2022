using UnityEngine;
using UnityEngine.UI;

public class UITitleScreen : MonoBehaviour
{
    [SerializeField] Text titleText;

    const float MAX_SHIFT = 2.0f;

    void Start()
    {
        // bounce that title!
        //iTween.PunchScale(titleText.gameObject, Vector3.one * 1.2f, 1.0f);
    }

    private void Update()
    {
        transform.localPosition = MAX_SHIFT * Vector3.right * Mathf.PingPong(Time.time, 1.0f);
    }

    public void Play()
    {
        SFXManager.PlaySound(SoundType.MenuConfirm);
        SceneManager.Goto(Constants.GAMEPLAY_SCENE);
    }

    public void Quit()
    {
        SFXManager.PlaySound(SoundType.MenuConfirm);
        Invoke("DelayedQuit", 1.0f);
    }

    void DelayedQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
