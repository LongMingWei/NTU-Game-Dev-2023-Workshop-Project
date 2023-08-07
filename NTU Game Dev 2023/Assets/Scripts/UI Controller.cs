using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI speed;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI distance;
    public TextMeshProUGUI distanceText;

    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;

    public GameObject gameOverPanel;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameOver(string value)
    {
        //Make the game over panel appear
        gameOverPanel.SetActive(true);

        //Hide the top GUI
        speed.gameObject.SetActive(false);
        speedText.gameObject.SetActive(false);
        distance.gameObject.SetActive(false); ;
        distanceText.gameObject.SetActive(false);

        //Set the final score value
        SetFinalScoreText(value);
    }

    public void SetSpeedText(string value)
    {
        speedText.text = value;
    }

    public void SetDistanceText(string value)
    {
        distanceText.text = value;
    }

    public void SetFinalScoreText(string value)
    {
        finalScoreText.text = value;
    }

    public void SetHighScoreText(string value)
    {
        Debug.Log(value);
        highScoreText.text = value;
    }
}
