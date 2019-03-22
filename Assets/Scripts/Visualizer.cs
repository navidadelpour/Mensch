using UnityEngine;
using System.Threading;

public class Visualizer : MonoBehaviour {

    public bool turnBased;
    public PlayerType[] playersTypes;
    public Player[] players;
    Board board;
    Game game;
    Thread gameThread;

    void Start() {
        board = new Board(4, 40, 4);
        game = new Game(board, playersTypes, turnBased);
        players = game.players;

        Subscribtion();
        gameThread = game.Start();
    }

    void Update() {
        Debug.Log(game.paused);
        if(Input.GetKeyDown(KeyCode.B)) {
            if(!game.paused)
                game.Pause();
            else
                game.Resume();
        }
        if(Input.GetKeyDown(KeyCode.C)) {
            game.winnedPlayer = game.players[0];
        }
    }

    private void OnApplicationQuit() {
        game.Pause();
    }

    void OnApplicationPause(bool pauseStatus) {
        if(gameThread == null)
            return;
        if(!pauseStatus)
            game.Pause();
        else
            game.Resume();
    }

    public void OnPieceClick() {
        // TODO: fetch piece data
        game.MovePiece(null);
    }

    public void OnDiceClick() {
        game.RollDice();
    }

    // a way to instantiate 4 player board game
    
    // events called in game logic and need to be handled on UI:
    // get piece in     done
    // get piece out    done
    // moving pieces    done
    // go to goal       
    // dice roll        done

    // onclick events comming from users:
    // dice
    // pieces

    // event listeners

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