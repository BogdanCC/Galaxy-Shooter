using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    private Animator _animator;
    private Player _player;
	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {

        if (_player.isPlayerOne) {
            Animate(KeyCode.A, KeyCode.D);
        }

        if (_player.isPlayerTwo) {
            Animate(KeyCode.LeftArrow, KeyCode.RightArrow);
        }
    }

    private void Animate(KeyCode left, KeyCode right) {

        // If "A" key is pressed down or left arrow key is down -> Left animation
        if (Input.GetKeyDown(left)) {
            _animator.SetBool(Constants.TURN_LEFT, true);
            _animator.SetBool(Constants.TURN_RIGHT, false);
        }

        // If "A" key is lifted up or left arrow key is up -> Idle animation
        else if (Input.GetKeyUp(left)) {
            _animator.SetBool(Constants.TURN_LEFT, false);
            _animator.SetBool(Constants.TURN_RIGHT, false);
        }

        // If "D" key is pressed down or right arrow key is down -> Right animation
        if (Input.GetKeyDown(right)) {
            _animator.SetBool(Constants.TURN_RIGHT, true);
            _animator.SetBool(Constants.TURN_LEFT, false);
        }

        // If "D" key is lifted up or right arrow key is up -> Idle animation
        else if (Input.GetKeyUp(right)) {
            _animator.SetBool(Constants.TURN_RIGHT, false);
            _animator.SetBool(Constants.TURN_LEFT, false);
        }
    }
}
