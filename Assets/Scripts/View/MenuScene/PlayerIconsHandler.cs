using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIconsHandler : MonoBehaviour {

    public PlayerIcon[] playerIcons;
    private int currentPlayerIndex;

    public void NextPlayer(int index) {
        playerIcons[currentPlayerIndex].SetActive(false);
        currentPlayerIndex = index;
        playerIcons[index].SetActive(true);
    }

}
