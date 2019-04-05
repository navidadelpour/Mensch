using System;
using UnityEngine;

[Serializable]
public class PlayerData {
    public Color color;
    public PlayerType type;

    [HideInInspector] public Transform transform;
    [HideInInspector] public Transform dice;
    [HideInInspector] public Transform outsParent;
    [HideInInspector] public Transform goalsParent;
    [HideInInspector] public Transform piecesParent;

    public PlayerData(PlayerType type, Color color) {
        this.type = type;
        this.color = color;
    }

}