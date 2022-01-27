using UnityEngine;
using UnityEngine.UI;

public class UITitleScreen : MonoBehaviour
{
    [SerializeField] Text titleText;

    void Start()
    {
        // bounce that title!
        //iTween.PunchScale(titleText.gameObject, Vector3.one * 1.2f, 1.0f);
    }


    public void Play()
    {
        SFXManager.PlaySound(SoundType.MenuConfirm);
        SceneManager.Goto(Constants.GAMEPLAY_SCENE);
    }

    public void Quit()
    {
        // quit cuts off sound
        //SFXManager.PlaySound(SoundType.MenuConfirm);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
