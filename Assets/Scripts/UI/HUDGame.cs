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

    public void AccumulateScore(int playerId)
    {
        iTween.PunchScale(scoreTexts[playerId].gameObject, Vector3.one * 1.00001f, 1f);
        scores[playerId] += 1;
        scoreTexts[playerId].text = scores[playerId].ToString();
    }
}
