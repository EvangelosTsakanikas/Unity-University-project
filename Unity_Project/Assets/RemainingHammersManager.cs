using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainingHammersManager : MonoBehaviour
{
    private Text numberOfRemainingHammers;

    void Awake()
    {
        numberOfRemainingHammers = GetComponent<Text>();
    }

    void FixedUpdate()
    {
        numberOfRemainingHammers.text = "Hammers: " + LoadMazeFromTxt.K;
    }
}
