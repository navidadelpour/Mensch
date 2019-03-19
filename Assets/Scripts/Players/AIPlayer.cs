public class AIPlayer : Player {

    public override void DoDice() {
        game.RollDice();
    }

    public override void DoMove(int diceNumber) {
        base.DoMove();
        Piece bestPiece = null;
        // priorities:
        // get out of the blocking piece in a base.
        // if you had 6 then you should get in your piece
        // try to hit someone
        // nearest piece to goal
        game.MovePiece(bestPiece);
    }

}