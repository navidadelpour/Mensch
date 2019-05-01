using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour {

    public PlayerType type;
    [HideInInspector] public Color color;
    private PlayerType[] types;
    private int currentIndex;
    private Image image;
    private Image icon;
    public Sprite humanImage;
    public Sprite botImage;

    void Awake() {
        image = GetComponent<Image>();
        color = image.color;
        icon = transform.GetChild(0).GetComponent<Image>();
        types = new PlayerType[] {PlayerType.AI, PlayerType.HUMAN, PlayerType.NOTHING};
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
                icon.color = Color.clear;
                break;
            case PlayerType.HUMAN:
                icon.sprite = humanImage;
                icon.color = Color.white;
                break;
            case PlayerType.AI:
                icon.sprite = botImage;
                icon.color = Color.white;
                break;
        }
    }

    public void RemoveButton() {
        Destroy(GetComponent<Button>());
    }

    public void SetActive(bool b) {
        float alpha = b ? 1 : .5f;
        image.color = new Color(color.r, color.g, color.b, alpha);
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, icon.color.a == 0 ? 0 : alpha);
    }

}
