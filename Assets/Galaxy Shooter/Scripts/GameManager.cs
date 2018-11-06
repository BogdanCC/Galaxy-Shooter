using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public bool gameOver;
    public GameObject player;
    private UIManager UIManager;
    private SpawnManager spawnManager;
    
    // Find the UIManager script on start()
    private void Start()
    {
        UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        gameOver = true;
    }

    private void Update() {
        // If gameOver is true show the title screen
        if (gameOver) {
            if (UIManager != null && spawnManager != null)
            {
                UIManager.ShowTitleScreen(true);
                spawnManager.SetEnemySpeed(3.0f);
            }
            

            // If 'space' is pressed, hide the title screen, instantiate the player and set gameOver to false
            if (Input.GetKeyDown(KeyCode.Space)) {

                UIManager.ShowTitleScreen(false);
                Instantiate(player, Vector3.zero, Quaternion.identity);
                gameOver = false;
            }
        }
    }

    
}
