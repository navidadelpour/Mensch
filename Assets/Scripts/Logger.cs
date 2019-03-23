using UnityEngine;

public class Logger : MonoBehaviour {

    Game game;

    public void Start() {
        game = GetComponent<Visualizer>().game;
        Subscribtion();
    }

    public void Subscribtion() {
        game.RolledDiceEvent += new Game.RollDiceHandler(OnRolledDice);
        game.SetNextTurnEvent += new Game.SetNextTurnHandler(OnSetNextPlayer);
        game.GetInPieceEvent += new Game.GetInPieceHandler(OnGetInPiece);
        game.GetOutPieceEvent += new Game.GetOutPieceHandler(OnGetOutPiece);
        game.MovePieceEvent += new Game.MovePieceHandler(OnMovePiece);
    }


    public void OnRolledDice(RollDiceEventArgs e) {
        if(e.canMove)
        Debug.LogFormat("DICE: player {0} dice {1}",
            e.player.index, e.diceNumber);
    }

    public void OnSetNextPlayer(SetNextTurnEventArgs e) {
        Debug.LogFormat("ACTIVE: player {0}",
            e.player.index);
    }

    public void OnGetInPiece(GetInPieceEventArgs e) {
        Debug.LogFormat("GET IN: player {0} piece {1} position {2}",
            e.piece.player.index, e.piece.index, e.piece.position);
    }

    public void OnGetOutPiece(GetOutPieceEventArgs e) {
        Debug.LogFormat("GET OUT: player {0} piece {1} attacks player {2} piece {3}",
            e.piece.player.index, e.piece.index, e.enemy.player.index, e.enemy.index);
    }

    public void OnMovePiece(MovePieceEventArgs e) {
        Debug.LogFormat("MOVE:player {0} piece {1} position {2}",
            e.piece.player.index, e.piece.index, e.piece.position);
    }

}