using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScore : MonoBehaviour
{
    public static float finalScore;

    private Text text;

    void Awake()
    {
        text = GetComponent<Text>();
        finalScore = -1;
    }

    void Update()
    {
        if (finalScore >= 0)
        {
            text.text = "Final Score: " + finalScore;
        }
        else
        {
            text.text = "";
        }
    }
}
