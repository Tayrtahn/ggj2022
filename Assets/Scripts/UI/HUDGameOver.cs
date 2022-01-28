using UnityEngine;

public class HUDGameOver : MonoBehaviour
{
    void Start()
    {
        SFXManager.PlaySound(SoundType.GameOver);
    }
}
