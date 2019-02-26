using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResult : MonoBehaviour
{
    private Text gameResult;

    void Awake()
    {
        gameResult = GetComponent<Text>();
    }

    void Update()
    {
        if (FinalScore.finalScore > 0)
        {
            gameResult.text = "You Won!";
        }
        else if (FinalScore.finalScore == 0)
        {
            gameResult.text = "Defeated";
        }
    }

}
