using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    // An array of Sprite objects to hold the lives image sprites
    public Sprite[] livesSprites;
    public Image livesImage;
    public Text scoreText;
    public Text highScoreText;
    public GameObject TitleScreen;
    public int score;
    public int highScore;

    // Initialising scores and UI text on start()
    private void Start()
    {
        highScore = 0;
        highScoreText.text = "High Score: " + highScore;
        ResetScore();
    }

    // Method to reset the score
    public void ResetScore()
    {
        score = 0;
        scoreText.text = "Score: " + score;
    }

    // Method to update the lives image given the player life (value)
    public void UpdateLives(int value)
    {
        livesImage.sprite = livesSprites[value];
    }

    // Method to update the current score and high score
    public void UpdateScore(int points)
    {
        // Increase the score by the points passed in, update the UI text
        score += points;
        scoreText.text = "Score: " + score;

        // If current score is higher than highscore, set it to highscore
        if(score > highScore) {
            highScore = score;
            highScoreText.text = "High Score: " + highScore;
        }
    }

    // Method to show or hide the title screen
    public void ShowTitleScreen(bool value)
    {
        if(value) {
            // Show title screen
            TitleScreen.SetActive(true);       
        } else {
            // Hide title screen
            TitleScreen.SetActive(false);
            ResetScore();
        }
    }
}
