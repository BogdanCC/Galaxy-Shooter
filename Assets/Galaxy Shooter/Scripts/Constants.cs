using UnityEngine;

public class Constants : MonoBehaviour {
    // Scenes indeces
    public const int SCENE_MAIN_MENU = 0;
    public const int SCENE_SINGLE_PLAYER = 1;
    public const int SCENE_CO_OP = 2;
    // Powerups IDs
    public const int TRIPLE_SHOT_ID = 0;
    public const int SPEED_POWERUP_ID = 1;
    public const int SHIELD_POWERUP_ID = 2;
    // Player IDs
    public const int PLAYER_ONE_ID = 100;
    public const int PLAYER_TWO_ID = 101;
    // GameObject names
    public const string GO_CANVAS_NAME = "Canvas";
    public const string GO_GAME_MANAGER_NAME = "GameManager";
    public const string GO_SPAWN_MANAGER_NAME = "SpawnManager";
    public const string GO_PAUSE_MENU_NAME = "PauseMenuPanel";
    public const string GO_GALAXY = "Galaxy";
    // Prefab name
    public const string PLAYER_ONE_PREFAB = "Player1(Clone)";
    public const string PLAYER_TWO_PREFAB = "Player2(Clone)";
    // Tags
    public const string TAG_PLAYER = "Player";
    public const string TAG_LASER = "Laser";
    // Animation parameters
    public const string TURN_LEFT = "Turn_Left";
    public const string TURN_RIGHT = "Turn_Right";
    public const string PAUSED = "Paused";
    // Input Manager names
    public const string HORIZONTAL_ONE = "Horizontal1";
    public const string HORIZONTAL_TWO = "Horizontal2";
    public const string VERTICAL_ONE = "Vertical1";
    public const string VERTICAL_TWO = "Vertical2";
}
