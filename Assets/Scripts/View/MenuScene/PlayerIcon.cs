using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour {

    public PlayerType type;
    public Color color;
    private PlayerType[] types;
    private int currentIndex;
    private Image image;
    private Image icon;
    public Sprite humanImage;
    public Sprite botImage;

    void Start() {
        image = GetComponent<Image>();
        image.color = color;
        icon = transform.GetChild(0).GetComponent<Image>();
        types = new PlayerType[] {PlayerType.HUMAN, PlayerType.AI, PlayerType.NOTHING};
        currentIndex = Array.IndexOf(types, type);
        SetImage(type);
    }

    public void OnClick() {
        type = types[(++currentIndex) % types.Length];
        SetImage(type);
    }

    private void SetImage(PlayerType type) {
        switch (type) {
            case PlayerType.NOTHING:
                icon.sprite = null;
                break;
            case PlayerType.HUMAN:
                icon.sprite = humanImage;
                break;
            case PlayerType.AI:
                icon.sprite = botImage;
                break;
        }
    }

    public void RemoveButton() {
        Destroy(GetComponent<Button>());
    }

    public void SetActive(bool b) {
        image.color = new Color(color.r, color.g, color.b, b ? 1 : .5f);
    }

}
