using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour {
    
    public bool log;
    public Button startButton;
    public Button backButton;
    public Button dice;
    private PlayerIconsHandler playerIconsHandler;
    private Visualizer visualizer;

    void Awake() {
        playerIconsHandler = GetComponent<PlayerIconsHandler>();
        visualizer = GetComponent<Visualizer>();
        backButton.gameObject.SetActive(false);
        dice.gameObject.SetActive(false);
    }

    public void OnStartButtonClick() {
        PlayerIcon[] playerIcons = playerIconsHandler.playerIcons;
        foreach (PlayerIcon playerIcon in playerIcons) {
            playerIcon.RemoveButton();
            playerIcon.SetActive(false);
        }

        bool hasError = true;
        for (int i = 0; i < playerIcons.Length; i++) {
            visualizer.playersData[i] = new PlayerData(playerIcons[i].type, playerIcons[i].color);
                
            if(visualizer.playersData[i].type == PlayerType.HUMAN || visualizer.playersData[i].type == PlayerType.AI) {
                hasError = false;
            }
        }

        if(hasError) {
            Debug.Log("at least one human or AI is required");
            return;
        }

        if(log)
            gameObject.AddComponent(typeof(Logger));

        visualizer.Setup();
        startButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
        dice.gameObject.SetActive(true);
    }

    public void OnBackButtonClick() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnDiceClick() {
        visualizer.game.AttemptThrowDiceFromUser();
    }

}