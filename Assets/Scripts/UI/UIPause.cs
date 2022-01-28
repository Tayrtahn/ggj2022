using UnityEngine;

public class UIPause : MonoBehaviour
{
    [SerializeField] GameObject activateGO;

    void OnEnable()
    {
        TimeManager.AddPauseSource(this);
        activateGO.SetActive(true);
    }

    void OnDisable()
    {
        TimeManager.RemovePauseSource(this);
        activateGO.SetActive(false);
    }

    public void ToggleMenu()
    {
        enabled = !enabled;
        if (enabled)
            SFXManager.PlaySound(SoundType.MenuConfirm);
        else
            SFXManager.PlaySound(SoundType.MenuCancel);
    }

    public void Resume()
    {
        if (enabled)
            ToggleMenu();
    }

    public void Quit()
    {
        SFXManager.PlaySound(SoundType.MenuCancel);
        SceneManager.Goto(Constants.TITLE_SCENE);
    }
}
