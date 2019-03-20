using UnityEngine;

public class Visualizer : MonoBehaviour {

    PlayerType[] playersTypes;
    Board board;
    Game game;

    void Start() {
        playersTypes = new PlayerType[]{PlayerType.AI, PlayerType.AI, PlayerType.AI, PlayerType.AI};
        board = new Board(4, 40, 4);
        game = new Game(board, playersTypes);
        
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
    // get piece in, out
    // moving pieces
    // attack piece
    // go to goal
    // dice roll

    // onclick events comming from users:
    // dice
    // pieces

}