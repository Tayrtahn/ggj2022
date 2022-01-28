using UnityEngine;

public class PaddleNoise : MonoBehaviour
{
    [SerializeField] float fadeSpeed = 5.0f;

    private AudioSource audioSource;
    private PaddleController paddleController;

    void Start()
    {
        paddleController = GetComponent<PaddleController>();
        audioSource = GetComponent<AudioSource>();

        if (!paddleController || !audioSource)
            enabled = false;
    }

    void Update()
    {
        audioSource.volume = Mathf.Lerp(audioSource.volume, Mathf.Abs(paddleController.DeltaAngle / paddleController.MaxSpeed), fadeSpeed * Time.deltaTime);
    }
}
