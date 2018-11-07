using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
    /**
     * If you change game object names or order inside "MainMenuPanel"
     * make sure to change the constants as well
     */
    // Constants for child game object indeces of main menu panel game object
    private const int MENU_IMAGE_INDEX = 1;
    private const int SINGLE_PLAYER_BUTTON_INDEX = 2;
    private const int CO_OP_BUTTON_INDEX = 3;
    private const int LOADING_TEXT_INDEX = 4;
    // Constants for child game object names of main menu panel game object
    private const string MAIN_MENU_PANEL_NAME = "MainMenuPanel";
    private const string MENU_IMAGE_NAME = "MainMenuImage";
    private const string SINGLE_PLAYER_BUTTON_NAME = "SinglePlayerButton";
    private const string CO_OP_BUTTON_NAME = "CoOpButton";
    private const string NOT_AVAILABLE_MOBILE = "NotAvailable";

    private GameObject tempGameObject;
    private GameObject mainMenuPanel;
    private Button coOpButton;
    private GameObject notAvailable;
    
    // A list of game objects to disable when loading
    private List<KeyValuePair<string, int>> childsToDisableList = new List<KeyValuePair<string, int>>();

    private void Start() {
        // Find main menu panel to disable childs
        mainMenuPanel = GameObject.Find(MAIN_MENU_PANEL_NAME);
        notAvailable = GameObject.Find(NOT_AVAILABLE_MOBILE);
        // Add children of main menu panel as key value pairs (a name, child index)
        childsToDisableList.Add(new KeyValuePair<string, int>(MENU_IMAGE_NAME, MENU_IMAGE_INDEX));
        childsToDisableList.Add(new KeyValuePair<string, int>(SINGLE_PLAYER_BUTTON_NAME, SINGLE_PLAYER_BUTTON_INDEX));
        childsToDisableList.Add(new KeyValuePair<string, int>(CO_OP_BUTTON_NAME, CO_OP_BUTTON_INDEX));
#if UNITY_ANDROID
        notAvailable.SetActive(true);
        coOpButton = mainMenuPanel.transform.GetChild(CO_OP_BUTTON_INDEX).GetComponent<Button>();
        coOpButton.interactable = false;
#else
        notAvailable.SetActive(false);
#endif
    }

    // Uses a coroutine to load the Scene in the background, using the scene's index from build settings 
    public void LoadScene(int sceneIndex) {

        StartCoroutine(LoadAsyncScene(sceneIndex));
    }

    private IEnumerator LoadAsyncScene(int sceneIndex) {

        foreach(KeyValuePair<string, int> entry in childsToDisableList) {
            tempGameObject = mainMenuPanel.transform.GetChild(entry.Value).gameObject;
            tempGameObject.SetActive(false);
        }
        
        tempGameObject = mainMenuPanel.transform.GetChild(LOADING_TEXT_INDEX).gameObject;
        tempGameObject.SetActive(true);
        // The Application loads the Scene in the background as the current Scene runs.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        
        // Wait until the asynchronous scene fully loads
        while(!asyncLoad.isDone) {
            yield return null;
        }
    }
}
