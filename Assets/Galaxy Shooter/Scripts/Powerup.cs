using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {
    [SerializeField]
    private float _speed;
    [SerializeField]
    private int powerupID;
    [SerializeField]
    private AudioClip powerupSound;

    // Use this for initialisation
    private void Start()
    {
        switch (powerupID)
        {
            case 0:
                _speed = 3.0f;
                break;
            case 1:
                _speed = 1.5f;
                break;
            case 2:
                _speed = 2.0f;
                break;
            default:
                _speed = 3.0f;
                break;
        }
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -6.0f)
        {
            Destroy(this.gameObject);
        }
	}
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If we collided with the Player
        if(other.tag == "Player")
        {
            // Access the player
            Player player = other.GetComponent<Player>();
            // If the player handle is not null
            if(player != null)
            {
                player.PowerupOn(powerupID);
            }
            if(powerupSound.loadState == AudioDataLoadState.Loaded)
            {
                powerupSound.UnloadAudioData();
                AudioSource.PlayClipAtPoint(powerupSound, Camera.main.transform.position, 1f);
            }
            
            // Destroy this game object
            Destroy(this.gameObject);
        }
    }
}
