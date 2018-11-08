using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public bool isCoOpMode = false;
    public bool gameOver;
    private bool isLoading = false;
    public GameObject player;
    public GameObject coOpPlayerOne;
    public GameObject coOpPlayerTwo;

    private GameObject pauseMenuPanel;
    private UIManager UIManager;
    private SpawnManager spawnManager;
    private Animator pauseMenuAnimator;
    
    // Find the UIManager script on start()
    private void Start()
    {
        UIManager = GameObject.Find(Constants.GO_CANVAS_NAME).GetComponent<UIManager>();
        spawnManager = GameObject.Find(Constants.GO_SPAWN_MANAGER_NAME).GetComponent<SpawnManager>();
        pauseMenuPanel = GameObject.Find(Constants.GO_PAUSE_MENU_NAME);
        pauseMenuAnimator = pauseMenuPanel.GetComponent<Animator>();
        pauseMenuAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        pauseMenuPanel.SetActive(false);
        gameOver = true;
        Time.timeScale = 1;
    }

    private void Update() {
        // If gameOver is true show the title screen
        if (gameOver) {
            if (UIManager != null && spawnManager != null && !isLoading)
            {
                UIManager.ShowTitleScreen(true);
                spawnManager.SetEnemySpeed(3.0f);
                
            }
            // If 'esc' is pressed, go back to main menu
            if(Input.GetKeyDown(KeyCode.Escape)) {
                UIManager.ShowTitleScreen(false);
                BackToMainMenu();
            }

            // If 'space' is pressed, hide the title screen, instantiate the player and set gameOver to false
            if (Input.GetKeyDown(KeyCode.Space)) {
                StartGame();
            }
        } else {
            // If game is not over and we press escape
            // pause the game
            if(Input.GetKeyDown(KeyCode.Escape)) {
                if(pauseMenuPanel.activeInHierarchy) {
                    ResumeGame();
                } else {
                    PauseGame();
                }
            }
        }
    }

    // Method to load main menu scene
    public void BackToMainMenu() {
        isLoading = true;
        StartCoroutine(LoadAsyncScene(Constants.SCENE_MAIN_MENU));
    }
    private IEnumerator LoadAsyncScene(int sceneIndex) {
        UIManager.ShowTitleScreen(false);
        UIManager.LoadingScreen();
        // The Application loads the Scene in the background as the current Scene runs.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

    // Method to resume game
    public void ResumeGame() {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }
    
    // Method to pause game
    public void PauseGame() {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0;
        pauseMenuAnimator.SetTrigger(Constants.PAUSED);
    }
    public void StartGame() {
        // Disable Title Screen
        UIManager.ShowTitleScreen(false);
        // Instantiate player(s) depending on game mode (single or co-op)
        switch (SceneManager.GetActiveScene().buildIndex) {
            case Constants.SCENE_SINGLE_PLAYER:
                isCoOpMode = false;
                Instantiate(player, Vector3.zero, Quaternion.identity);
                break;
            case Constants.SCENE_CO_OP:
                isCoOpMode = true;
                Instantiate(coOpPlayerOne);
                Instantiate(coOpPlayerTwo);
                break;
            default:
                // do nothing;
                break;
        }

        gameOver = false;
    }
}
