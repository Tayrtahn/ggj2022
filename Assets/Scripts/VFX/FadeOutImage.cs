using UnityEngine;
using UnityEngine.UI;

public class FadeOutImage : MonoBehaviour
{
    [SerializeField] Image image;

    int delay = 2;

    private void Awake()
    {
        Debug.Assert(image);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
        //image.CrossFadeAlpha(0.0f, 1.0f, true);
    }

    void Update()
    {
        // HACK: delta of the first played frame seems to be a large number, so we wait
        if (delay > 0)
        {
            delay--;
            return;
        }
            
        image.color -= new Color(0, 0, 0, Time.unscaledDeltaTime * 0.5f);
        if (image.color.a <= 0)
        {
            image.enabled = false;
            enabled = false;
        }
    }
}
