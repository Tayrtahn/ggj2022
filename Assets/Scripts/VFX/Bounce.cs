using UnityEngine;


public class Bounce : MonoBehaviour
{
    public float magnitude = 0.2f;
    public float duration = 0.75f;

    public void Play()
    {
        iTween.Stop(gameObject);
        transform.localScale = Vector3.one;
        iTween.PunchScale(gameObject, Vector3.one * magnitude, duration);
    }
}