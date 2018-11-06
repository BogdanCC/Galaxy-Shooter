using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Prefabs
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShot;
    [SerializeField]
    private GameObject _playerExplosion;
    [SerializeField]
    private GameObject _playerShield;
    [SerializeField]
    private GameObject[] engineFires;
   
    // Physics variables
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _speedBoost = 1.5f;
    private float _calculatedSpeed;

    // Shooting cooldown variables
    [SerializeField]
    private const float _fireRate = 0.25f;
    private float _nextFire;

    // Player lives
    [SerializeField]
    private int _playerLives;
    private const int MAX_LIVES = 3;

    // Powerups booleans
    public bool canTripleShot = false;
    public bool hasSpeedBoost = false;
    public bool hasShield = false;

    // Powerups IDs
    private const int TRIPLE_SHOT_ID = 0;
    private const int SPEED_POWERUP_ID = 1;
    private const int SHIELD_POWERUP_ID = 2;

    private UIManager uiManager;
    private GameManager gameManager;
    private Coroutine shieldsCD;
    private Coroutine speedCD;
    private Coroutine tripleShotCD;
    private SpawnManager spawnManager;
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        audioSource = GetComponent<AudioSource>();
        _playerLives = 3;
        if (uiManager != null)
        {
            uiManager.UpdateLives(_playerLives);
        }
        spawnManager.StartSpawnRoutine();
    }

    // Update is called once per frame
    void Update()
    {
        // If space key is pressed
        // spawn laser at player position
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && Time.time > _nextFire)
        {
            Shoot();
        }
    }

    // Update method for physics calculations
    private void FixedUpdate()
    {
        // Call PlayerMovement function
        PlayerMovement();
    }

    // Method to create map constraints
    private void PlayerMovement()
    {
        if (hasSpeedBoost)
        {
            _calculatedSpeed = _speed * _speedBoost;
        } else
        {
            _calculatedSpeed = _speed;
        }
        // Get horizontal and vertical inputs from keyboard
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Translate the position accordingly
        transform.Translate(Vector3.right * _calculatedSpeed * horizontalInput * Time.deltaTime);
        transform.Translate(Vector3.up * _calculatedSpeed * verticalInput * Time.deltaTime);

        // Horizontal constraints (the player should reappear on the other side of the map)
        if (transform.position.x < -9.44f)
        {
            transform.position = new Vector3(9.5f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > 9.50f)
        {
            transform.position = new Vector3(-9.44f, transform.position.y, transform.position.z);
        }
        // Vertical constraints (movement should stop at these values)
        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        else if (transform.position.y < -4.0f)
        {
            transform.position = new Vector3(transform.position.x, -4.0f, transform.position.z);
        }
    }

    // Method for player fire functionality
    private void Shoot()
    {
        audioSource.Play();
        if (canTripleShot)
        {
            Instantiate(_tripleShot, transform.position + _tripleShot.transform.position, Quaternion.identity);
        } else
        {
            // spawn laser
            // Quaternion.identity means no rotation/default rotation
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
        }
        // Assign cooldown 
        // Time.time is the time passed since the game has started in seconds
        if (hasSpeedBoost)
        {
            _nextFire = Time.time + (_fireRate * 0);
        } else {
            _nextFire = Time.time + _fireRate;
        } 
    }

    // Method to enable triple shot and set power down countdown
    public void PowerupOn(int powerupID)
    {
        // Set the correct powerup boolean true given the powerupID (from Powerup.cs)
        switch (powerupID)
        {
            case TRIPLE_SHOT_ID:
                // If coroutine already active, restart it
                if (canTripleShot) {
                    StopCoroutine(tripleShotCD);
                    tripleShotCD = StartCoroutine(TripleShotPowerDownRoutine());
                }
                // Else set powerup active and start cooldown
                else {                 
                    canTripleShot = true;
                    tripleShotCD = StartCoroutine(TripleShotPowerDownRoutine());
                }
                break;

            case SPEED_POWERUP_ID:
                // If coroutine already active, restart it
                if (hasSpeedBoost) {
                    StopCoroutine(speedCD);
                    speedCD = StartCoroutine(SpeedBoostPowerDownRoutine());
                }
                // Else set powerup active and start cooldown
                else {
                    hasSpeedBoost = true;
                    speedCD = StartCoroutine(SpeedBoostPowerDownRoutine());
                }            
                break;

            case SHIELD_POWERUP_ID:
                // If coroutine already active, restart it
                if (hasShield) {
                    StopCoroutine(shieldsCD);
                    shieldsCD = StartCoroutine(ShieldPowerDownRoutine());
                }
                // Else set powerup active and start cooldown
                else {
                    EnableShields(true);
                    shieldsCD = StartCoroutine(ShieldPowerDownRoutine());
                }       
                break;
        }
    }

    // Method for power down countdown
    public IEnumerator TripleShotPowerDownRoutine()
    {
        // Wait 5 seconds
        yield return new WaitForSeconds(5.0f);
        canTripleShot = false;
    }
    public IEnumerator SpeedBoostPowerDownRoutine()
    {
        // Wait 5 seconds
        yield return new WaitForSeconds(5.0f);
        hasSpeedBoost = false;
    }
    public IEnumerator ShieldPowerDownRoutine()
    {
        // Wait 5 seconds
        yield return new WaitForSeconds(5.0f);
        EnableShields(false);
    }
    
    // Method to inflict damage to player
    public void InflictDamage(int damage)
    {
        // If player has shield, destroy it and return early
        if(hasShield) {
            EnableShields(false);
            return;
        }
        
        // Inflict damage
        _playerLives -= damage;

        // Damage engins
        switch (_playerLives) {
            case 2:
                engineFires[0].SetActive(true);
                break;
            case 1:
                engineFires[1].SetActive(true);
                break;
        };

        // Update lives image
        uiManager.UpdateLives(_playerLives);
        // If life is less than 1, destroy player
        if (_playerLives < 1)
        {
            OnPlayerDestroyed();
        }
    }

    // Method to destroy player & show the animation
    public void OnPlayerDestroyed()
    {
        gameManager.gameOver = true;
        Instantiate(_playerExplosion, transform.position, Quaternion.identity);
        spawnManager.StopSpawnRoutine();
        Destroy(this.gameObject);      
    }

    // Enable shields
    private void EnableShields(bool value)
    {
        hasShield = value;
        _playerShield.SetActive(value);
    }

}
