using UnityEngine;
using System;

public class Logger : MonoBehaviour {

    Game game;

    public void Start() {
        game = GetComponent<Visualizer>().game;
        Subscribtion();
    }

    public void Subscribtion() {
        game.RolledDiceEvent += new EventHandler<RollDiceEventArgs>(OnRolledDice);
        game.SetNextTurnEvent += new EventHandler<SetNextTurnEventArgs>(OnSetNextPlayer);
        game.GetInPieceEvent += new EventHandler<GetInPieceEventArgs>(OnGetInPiece);
        game.GetOutPieceEvent += new EventHandler<GetOutPieceEventArgs>(OnGetOutPiece);
        game.MovePieceEvent += new EventHandler<MovePieceEventArgs>(OnMovePiece);
    }


    public void OnRolledDice(object obj, RollDiceEventArgs e) {
        Debug.LogFormat("DICE: dice {0}",
            e.diceNumber);
    }

    public void OnSetNextPlayer(object obj, SetNextTurnEventArgs e) {
        Debug.Log("----------------------------------------------------------------");
        Debug.LogFormat("ACTIVE: player {0}",
            e.player.index);
    }

    public void OnGetInPiece(object obj, GetInPieceEventArgs e) {
        Debug.LogFormat("GET IN: piece {0} position {1}",
            e.piece.index, e.piece.position);
    }

    public void OnGetOutPiece(object obj, GetOutPieceEventArgs e) {
        Debug.LogFormat("GET OUT: piece {0} position {1}",
            e.piece.index, e.piece.position);
    }

    public void OnMovePiece(object obj, MovePieceEventArgs e) {
        Debug.LogFormat("MOVE: piece {0} position {1}",
            e.piece.index, e.piece.position);
    }

}