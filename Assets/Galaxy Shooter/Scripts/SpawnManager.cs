using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    // Game objects to spawn
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private List<GameObject> _powerups;
    private EnemyAI enemyAI;
    private UIManager uiManager;

    // Random powerup and positions on X axis
    private int _randPowerup;
    private float _randPowerupX;
    private float _randEnemyX;

    // Variables to hold the coroutines
    private Coroutine enemies;
    private Coroutine powerups;

    // Spawn frequencies
    [SerializeField]
    private int enemySpawnFrequency = 2;
    [SerializeField]
    private int powerupSpawnFrequency = 5;

    private GameManager gameManager;

    private void Start() {
        enemyAI = _enemy.GetComponent<EnemyAI>();
        uiManager = GameObject.Find(Constants.GO_CANVAS_NAME).GetComponent<UIManager>();
        gameManager = GameObject.Find(Constants.GO_GAME_MANAGER_NAME).GetComponent<GameManager>();
        if (enemyAI != null) {
            enemyAI.Speed = 3.0f;
        }
        
    }

    public void SetEnemySpeed(float value)
    {
            enemyAI.Speed = value;
    }

    // Method to start the spawn routine
    public void StartSpawnRoutine()
    {
        enemies = StartCoroutine(SpawnEnemyRoutine());
        powerups = StartCoroutine(SpawnPowerupRoutine());
    }

    // Method to stop the spawn routine
    public void StopSpawnRoutine()
    {
        StopCoroutine(enemies);
        StopCoroutine(powerups);
    }

    // Method for spawning powerups
    private IEnumerator SpawnPowerupRoutine()
    {
        while (true)
        {
            // Get a random powerup and a random X position
            _randPowerup = Random.Range(0, _powerups.Count);
            _randPowerupX = Random.Range(-8.40f, 8.30f);

            // Instantiate the powerup
            Instantiate(_powerups[_randPowerup], new Vector3(_randPowerupX, 5.70f, 0.0f), Quaternion.identity);

            // If co-op mode, spawn more powerups
            if (gameManager != null) {
                if (gameManager.isCoOpMode) {
                    powerupSpawnFrequency = 3;
                }
            }

            // Wait 5 (or 3) seconds and do this all over again
            yield return new WaitForSeconds(powerupSpawnFrequency);
        }   
    }

    // Method for spawning enemies
    private IEnumerator SpawnEnemyRoutine()
    {
        // Infinite loop
        while (true)
        {
            // Get a random enemy position on the X axis
            _randEnemyX = Random.Range(-7.92f, 7.9f);  

            // Instantiate the enemy at the random position
            Instantiate(_enemy, new Vector3(_randEnemyX, 6.35f, 0.0f), Quaternion.identity);

            // Wait 2 seconds and do this all over again
            yield return new WaitForSeconds(enemySpawnFrequency);
        }
    }
}
