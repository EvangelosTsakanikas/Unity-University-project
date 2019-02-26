using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static float score;

    private Text ScoreText;

    void Awake()
    {
        ScoreText = GetComponent<Text>();
    }

    void FixedUpdate()
    {
        ScoreText.text = "Score: " + score;
    }
}
