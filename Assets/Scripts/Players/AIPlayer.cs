using System.Collections.Generic;

public class AIPlayer : Player {

    public AIPlayer(Game game, int index) : base(game, index) {

    }

    public override void DoDice() {
        game.RollDice();
    }

    public override void DoMove(int diceNumber, Piece piece = null) {
        base.DoMove(diceNumber, piece);
        string cause = "";
        Piece bestPiece = GetBestPiece(diceNumber, ref cause);
        UnityEngine.Debug.Log("BEST PIECE: " + bestPiece + ", Cause: " + cause);
        game.MovePiece(bestPiece);
    }

    private Piece GetBestPiece(int diceNumber, ref string cause) {
        // TODO: heuristic function.
        // priorities:

        // 0. no other choice
        if(inPieces.Count == 1 && diceNumber != 6) {
            KeyValuePair<Piece, BlockType> tuple = inPieces[0].GetBlock(diceNumber);
            if(tuple.Value != BlockType.ALLY && tuple.Value != BlockType.OUTGOAL) {
                cause = "no other choice";
                return inPieces[0];
            } else
                return null;
        }

        // will be sorted by position
        inPieces.Sort();
        inPieces.Reverse();

        // 1. get out of the blocking piece in a base.
        for(int i = 0; i < inPieces.Count; i++) {
            KeyValuePair<Piece, BlockType> tuple = inPieces[i].GetBlock(diceNumber);
            if(!inPieces[i].inGoal && inPieces[i].position % ((game.board.roadSize / game.board.maxPlayers)) == 0 && tuple.Value != BlockType.ALLY) {
                cause = "in a base";
                return inPieces[i];
            }
        }

        // 2. if you had 6 then you should get in your piece
        if(diceNumber == 6 && outPieces.Count > 0) {
            KeyValuePair<Piece, BlockType> tuple = outPieces[0].GetBlock(diceNumber);
            if(tuple.Value != BlockType.ALLY){
                cause = "six dice";
                return outPieces[0];
                
            }
        }
        

        // 3. try to hit someone
        for(int i = 0; i < inPieces.Count; i++) {
            KeyValuePair<Piece, BlockType> tuple = inPieces[i].GetBlock(diceNumber);
            if(tuple.Value == BlockType.ENEMY) {
                cause = "hit someone";
                return inPieces[i];
            }
        }

        // nearest piece to goal
        for(int i = 0; i < inPieces.Count; i++) {
            KeyValuePair<Piece, BlockType> tuple = inPieces[i].GetBlock(diceNumber);
            if(tuple.Value != BlockType.ALLY && tuple.Value != BlockType.OUTGOAL) {
                cause = "nearest";
                return inPieces[i];
            }
        }

        return null;

    }

}