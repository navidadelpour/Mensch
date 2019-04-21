using System;
using UnityEngine;

public class PlayerData {
    public Color color;
    public PlayerType type;

    public Transform outsParent;
    public Transform goalsParent;
    public Transform piecesParent;

    public PlayerData(PlayerType type, Color color) {
        this.type = type;
        this.color = color;
    }

}