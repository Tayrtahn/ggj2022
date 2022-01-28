using UnityEngine;

public class HUDGame : MonoBehaviour
{
    void Start()
    {
        // Everyone's excited
        SFXManager.PlaySound(SoundType.Applause);
    }
}
