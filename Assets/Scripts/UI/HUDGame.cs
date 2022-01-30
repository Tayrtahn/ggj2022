using UnityEngine;
using UnityEngine.UI;

public class HUDGame : MonoBehaviour
{
    [SerializeField] Text[] scoreTexts;
    int[] scores;

    void Awake()
    {
        scores = new int[4];
    }

    public void AccumulateScore(PaddleController controller, int goalId)
    {
        if (!controller)
        {
            //SFXManager.PlaySound(SoundType.Laughter);
            return;
        }

        int scorer = controller.PlayerIndex;
        if (scorer == goalId)
        {
            SFXManager.PlaySound(SoundType.Laughter);
            scores[scorer] = Mathf.Max(scores[scorer] - 1, 0);
        }
        else
        {
            scores[scorer] += 1;
            if (scores[scorer] >= Constants.GAME_OVER_SCORE)
            {
                Object.FindObjectOfType<HUDGameOver>().enabled = true;
            }
            SFXManager.PlaySound(SoundType.Applause);
        }
        iTween.PunchScale(scoreTexts[scorer].gameObject, Vector3.one * 1.00001f, 1f);
        scoreTexts[scorer].text = scores[scorer].ToString();
        
    }
}
