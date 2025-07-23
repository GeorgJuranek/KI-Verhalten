using System;
using TMPro;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    int highScoreCongestion;
   
    [SerializeField]
    TextMeshProUGUI highscoreText; 

    [SerializeField] int bonusTime = 10;

    public static event Action<int> OnNewMilestoneInHighscore;

    private void OnEnable()
    {
        CongestedCarsObserver.OnCountChanged += BonusHandlingCheck;
    }

    private void OnDisable()
    {
        CongestedCarsObserver.OnCountChanged -= BonusHandlingCheck;
    }

    void BonusHandlingCheck(int countCongestion)
    {
        if (countCongestion == 0)
        {
            highScoreCongestion = 0;
            highscoreText.text = highScoreCongestion.ToString();
            return;
        }

        if (countCongestion > highScoreCongestion)
        {
            highScoreCongestion = countCongestion;
            highscoreText.text = highScoreCongestion.ToString();
    
            if (highScoreCongestion % 10 == 0)
            {
                OnNewMilestoneInHighscore?.Invoke(bonusTime);
            }
        }
    
    }

}
