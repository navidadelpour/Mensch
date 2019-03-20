using System.Collections.Generic;

public class AIPlayer : Player {

    public AIPlayer(Game game, int index) : base(game, index) {

    }

    public override void DoDice() {
        game.RollDice();
    }

    public override void DoMove(int diceNumber) {
        base.DoMove(diceNumber);
        Piece bestPiece = GetBestPiece(diceNumber);
        game.MovePiece(bestPiece);
    }

    private Piece GetBestPiece(int diceNumber) {
        // priorities:

        // 1. get out of the blocking piece in a base.
        for(int i = 0; i < inPieces.Capacity; i++) {
            if(inPieces[i].position % (game.board.roadSize / game.board.maxPlayers) == 0 && inPieces[i].CheckForward(diceNumber).player != this) {
                return inPieces[i];
            }
        }

        // 2. if you had 6 then you should get in your piece
        if(diceNumber == 6 && outPieces.Capacity > 0) {

            Piece possibleHittedPiece = outPieces[0].CheckForward(diceNumber);
            if(possibleHittedPiece == null){
                return outPieces[0];
            } else if(possibleHittedPiece.player != this){
                return outPieces[0];
            }

        }
        

        inPieces.Sort();

        // 3. try to hit someone and nearest piece to goal
        for(int i = 0; i < inPieces.Capacity; i++) {
            Piece possibleHittedPiece = inPieces[i].CheckForward(diceNumber);
            if(possibleHittedPiece != null && possibleHittedPiece.player != this) {
                return inPieces[i];
            }
        }

        // nearest piece to goal
        for(int i = 0; i < inPieces.Capacity; i++) {
            if(inPieces[i].CheckForward(diceNumber) == null) {
                return inPieces[i];
            }
        }

        return null;

    }

}