using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    [SerializeField]
    private float _speed;
    public float Speed { get { return _speed; } set { _speed = value; } }
    private const int LIFE_DAMAGE = 1;
    private const int ENEMY_DESTROYED_POINTS = 10;

    [SerializeField]
    private GameObject _enemyExplosion;
    private UIManager uiManager;
    private GameManager gameManager;
    private SpawnManager spawnManager;

    [SerializeField]
    private AudioClip explosionSound;

    // Initialise things
    void Start() {
        uiManager = GameObject.Find(Constants.GO_CANVAS_NAME).GetComponent<UIManager>();
        gameManager = GameObject.Find(Constants.GO_GAME_MANAGER_NAME).GetComponent<GameManager>();
        spawnManager = GameObject.Find(Constants.GO_SPAWN_MANAGER_NAME).GetComponent<SpawnManager>();
    }
    // Update is called once per frame
    void Update () {
       
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -6.4f)
        {
            float randomX = Random.Range(-7.92f, 7.9f);
            transform.position = new Vector3(randomX, 6.35f, transform.position.z);
        }
        if(gameManager.gameOver == true)
        {
            OnEnemyDestroyed();
        }
	}

    // Enemy behaviour when collided with an object
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If collision with player - inflict damage, destroy this object
        if(other.tag == Constants.TAG_PLAYER)
        {
            // Get a Player.cs script handle
            Player player = other.GetComponent<Player>();
            if(player != null)
            {
                // Use Player.cs's InflictDamage method to inflict damage to player
                player.InflictDamage(LIFE_DAMAGE);
                OnEnemyDestroyed();
            }
        }
        // If collision with laser - destroy laser, destroy this object
        if(other.tag == Constants.TAG_LASER)
        {
            // Update score and increase difficulty if it's the case
            if (uiManager != null && spawnManager != null)
            {
                uiManager.UpdateScore(ENEMY_DESTROYED_POINTS);
                CheckScoreAndIncreaseSpeed(uiManager.score);
            }
            
            // Destroy laser and this enemy object
            Destroy(other.transform.gameObject);
            OnEnemyDestroyed();
        }
    }

    // Method to destroy & animate enemy game object
    public void OnEnemyDestroyed()
    {
        if(explosionSound.loadState == AudioDataLoadState.Loaded)
        {
            explosionSound.UnloadAudioData();
            AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, 1f);
        }
        Instantiate(_enemyExplosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    // Method to increase enemy speed based on player score
    private void CheckScoreAndIncreaseSpeed(int score)
    {
        switch (score)
        {
            case 50:
                spawnManager.SetEnemySpeed(4.0f);
                break;
            case 100:
                spawnManager.SetEnemySpeed(5.0f);
                break;
            case 200:
                spawnManager.SetEnemySpeed(6.0f);
                break;
            case 300:
                spawnManager.SetEnemySpeed(8.0f);
                break;
            case 500:
                spawnManager.SetEnemySpeed(10.0f);
                break;
            case 600:
                spawnManager.SetEnemySpeed(12.0f);
                break;
            case 800:
                spawnManager.SetEnemySpeed(15.0f);
                break;
        }
    }
}
