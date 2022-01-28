using UnityEngine;

public class HUDGameOver : MonoBehaviour
{
    void Start()
    {
        SFXManager.PlaySound(SoundType.GameOver);
        Invoke("GotoTitle", 2f);
    }

    void GotoTitle()
    {
        SceneManager.Goto(Constants.TITLE_SCENE);
    }
}
