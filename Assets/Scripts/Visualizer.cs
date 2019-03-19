using UnityEngine;

public class Visualizer : MonoBehaviour {

    void Start() {
        Board board = new FourPlayerBoard();
        Game game = new Game();
    }
    
    // what if I add these functions to humanPlayer class?!
    public void OnPieceClick() {
        if(state == GameStates.MOVE) {
            game.MovePiece();
        }
    }

    public void OnDiceClick() {
        if(state == GameStates.DICE) {
            game.RollDice();
        }
    }

}