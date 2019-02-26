using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInsideLabyrinth : MonoBehaviour
{
    public GameObject gameManager;
    private int N;
    private float scalingFactor;

    void Start()
    {
        N = gameManager.gameObject.GetComponent<LoadMazeFromTxt>().getN();
        scalingFactor = gameManager.gameObject.gameObject.GetComponent<LoadMazeFromTxt>().getScalingFactor();
    }

    void Update()
    {
        transform.position = new Vector3
            (
                Mathf.Clamp(transform.position.x, 0 - 1.15f, scalingFactor * N - scalingFactor/2.0f),
                transform.position.y,
                Mathf.Clamp(transform.position.z, 0 - 1.15f, scalingFactor * N - scalingFactor/2.0f)
            );
    }
}
