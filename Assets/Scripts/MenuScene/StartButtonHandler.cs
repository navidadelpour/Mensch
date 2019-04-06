﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonHandler : MonoBehaviour {
    public PlayerHandler[] playerHandlers;
    public string sceneToLoadName;

    void Start() {
    }

    public void OnStartButtonClick() {
        bool hasError = true;
        PlayerData[] playersData = new PlayerData[4];
        for (int i = 0; i < playerHandlers.Length; i++) {
            playersData[i] = new PlayerData(playerHandlers[i].type, playerHandlers[i].color);
                
            if(playersData[i].type == PlayerType.HUMAN || playersData[i].type == PlayerType.AI) {
                hasError = false;
            }
        }

        if(hasError) {
            Debug.Log("at least one human or AI is required");
            return;
        }

        DataForGameScene.playersData = playersData;
        DataForGameScene.isValid = true;

        SceneManager.LoadScene(sceneToLoadName);
    }

}