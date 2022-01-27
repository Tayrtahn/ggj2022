using UnityEngine;

/// <summary>
/// Plays a sound on Start
/// </summary>
public class PlaySound : MonoBehaviour
{
    [SerializeField] SoundType soundType;
    [SerializeField]
    [Range(0.5f, 2.0f)] float pitchMultiplier = 1.0f;

    void Start()
    {
        Play();
    }

    [ContextMenu("Play")]
    void Play()
    {
        SFXManager.PlaySound(soundType, Random.insideUnitCircle * 5, pitchMultiplier);
    }
}
