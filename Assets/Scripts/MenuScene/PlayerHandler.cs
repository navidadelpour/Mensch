using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour {
    public PlayerType type = PlayerType.NOTHING;
    public Color color;
    private const int humanIndex = 1;
    private const int AiIndex = 2;
    private const int nothingIndex = 3;
    private int currentIndex;

    private Color activeColor = Color.white;
    private Color disabledColor = new Color32(153, 153, 153, 255);

    void Start() {
        color = transform.GetChild(0).GetComponent<Text>().color;
        currentIndex = nothingIndex;
        SetButtonColor(currentIndex, activeColor);
    }

    public void OnTypeButtonClick() {
        int selectedIndex = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();

        SetButtonColor(currentIndex, disabledColor);
        SetButtonColor(selectedIndex, activeColor);
        currentIndex = selectedIndex;

        switch(selectedIndex) {
            case nothingIndex:
                type = PlayerType.NOTHING;
                break;
            case humanIndex:
                type = PlayerType.HUMAN;
                break;
            case AiIndex:
                type = PlayerType.AI;
                break;
        }
    }

    void SetButtonColor(int index, Color color) {
        transform.GetChild(index).GetComponent<Image>().color = color;
    }

}
