using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour {
    
    public bool log;
    public Button startButton;
    public Button backButton;
    private PlayerIconsHandler playerIconsHandler;
    private Visualizer visualizer;
    private GameGUIMaker gameGUIMaker;

    void Awake() {
        
    }

    public void Setup() {
        playerIconsHandler = GetComponent<PlayerIconsHandler>();
        visualizer = GetComponent<Visualizer>();
        backButton.gameObject.SetActive(false);
        visualizer.dice.gameObject.SetActive(false);

        visualizer.playersData = new PlayerData[4];
        SetPlayersData();
        gameGUIMaker = new GameGUIMaker(visualizer);
        // please choose your players
        visualizer.dialogBox.Popup(".ﻦﮐ ﺏﺎﺨﺘﻧﺍ ﻭﺭ ﻥﺎﻨﮑﯾﺯﺎﺑ");

    }

    public void OnStartButtonClick() {
        bool hasError = SetPlayersData();
        if(hasError) {
            // at least one human needed.
            visualizer.dialogBox.Popup(".ﻪﺷﺎﺑ ﯼﺯﺎﺑ ﻮﺗ ﺪﯾﺎﺑ ﻥﺎﺴﻧﺍ ﮏﯾ ﻞﻗﺍﺪﺣ");
            return;
        }

        if(log)
            gameObject.AddComponent(typeof(Logger));

        foreach (PlayerIcon playerIcon in playerIconsHandler.playerIcons) {
            playerIcon.RemoveButton();
            playerIcon.SetActive(false);
        }

        visualizer.Setup();
        gameGUIMaker.UpdateBoard();
        startButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
        visualizer.dice.gameObject.SetActive(true);
    }

    public void OnBackButtonClick() {
        visualizer.EndGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool SetPlayersData(bool first = false) {
        PlayerIcon[] playerIcons = playerIconsHandler.playerIcons;
        bool hasError = true;
        for (int i = 0; i < playerIcons.Length; i++) {
            PlayerData playerData = visualizer.playersData[i];
            if(playerData == null) {
                playerData = new PlayerData(playerIcons[i].type, playerIcons[i].color);
            } else {
                playerData = visualizer.playersData[i];
                playerData.type = playerIcons[i].type;
            }
            visualizer.playersData[i] = playerData;
            if(visualizer.playersData[i].type == PlayerType.HUMAN) {
                hasError = false;
            }
        }

        return hasError;
    }

    public void OnLinkButtonClick() {
        Application.OpenURL("www.navidadelpour.ir");
    }

}