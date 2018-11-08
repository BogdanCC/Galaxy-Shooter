using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    // An array of Sprite objects to hold the lives image sprites
    public Sprite[] livesSprites;
    public Image livesImage;
    public Image livesImage2;
    public Text scoreText;
    public Text highScoreText;
    public GameObject TitleScreen;
    public GameObject spaceText;
    public GameObject escText;
    public GameObject pauseMenuPanel;

    public int score;
    public int highScore;

    private GameManager gameManager;
    private MeshRenderer galaxyMeshRenderer;
    private Player player;
    public GameObject loadingText;

    private const string HIGH_SCORE_KEY = "High Score";
    private const string HIGH_SCORE = "High Score: ";
    private const string SCORE = "Score: ";
    

    // Initialising scores and UI text on start()
    private void Start()
    {
        gameManager = GameObject.Find(Constants.GO_GAME_MANAGER_NAME).GetComponent<GameManager>();
#if UNITY_ANDROID
        galaxyMeshRenderer = GameObject.Find(Constants.GO_GALAXY).GetComponent<MeshRenderer>();
        Camera.main.backgroundColor = UnityEngine.Color.black;
        galaxyMeshRenderer.enabled = false;
#endif
        
        highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        highScoreText.text = HIGH_SCORE + highScore;
        ResetScore();
    }

    // Method to reset the score
    public void ResetScore()
    {
        score = 0;
        scoreText.text = SCORE + score;
    }

    // Method to update the lives image given the player life (value)
    public void UpdateLives(int value, int playerId)
    {
        switch (playerId) {
            case Constants.PLAYER_ONE_ID:
                livesImage2.sprite = livesSprites[value];
                break;
            case Constants.PLAYER_TWO_ID:
                livesImage.sprite = livesSprites[value];
                break;
            default:
                livesImage.sprite = livesSprites[value];
                break;
        }
    }

    // Method to update the current score and high score
    public void UpdateScore(int points)
    {
        // Increase the score by the points passed in, update the UI text
        score += points;
        scoreText.text = SCORE + score;

        // If current score is higher than highscore, set it to highscore
        if(score > highScore) {
            highScore = score;
            highScoreText.text = HIGH_SCORE + highScore;
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
        }
    }

    // Method to show or hide the title screen
    public void ShowTitleScreen(bool value) {

        TitleScreen.SetActive(value);
        spaceText.SetActive(value);
        escText.SetActive(value);

        if (!value) ResetScore();
    }

    public void LoadingScreen() {
        pauseMenuPanel.SetActive(false);
        loadingText.SetActive(true);
    }
}
