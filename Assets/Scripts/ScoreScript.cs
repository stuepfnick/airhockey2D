using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{

    public TMP_Text aiScoreText, playerScoreText;
    private int _aiScore, _playerScore;

    public void Increment(bool didAiScore)
    {
        if (didAiScore)
        {
            aiScoreText.text = (++_aiScore).ToString();
        }
        else
        {
            playerScoreText.text = (++_playerScore).ToString();
        }
    }
}
