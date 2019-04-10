using System.Collections.Generic;
using System.Collections;
using System;

public class AIPlayer : Player {

    public AIPlayer(Game game, int index) : base(game, index) {

    }

    public override void DoDice() {
        game.TryThrowDice();
    }

    public override void DoMove(int diceNumber) {
        game.TryMovePiece(GetBestPieceByHeuristic(diceNumber));
    }

    private Piece GetBestPieceByHeuristic(int diceNumber) {
        string cause = "";
        for (int i = 0; i < pieces.Length; i++)
            pieces[i].heuristic = Heuristic(pieces[i], diceNumber, ref cause);
        
        Array.Sort(pieces);
        return pieces[0];
    }

    private float Heuristic(Piece piece, int diceNumber, ref string cause) {
        float heuristic = 0;
        KeyValuePair<Piece, BlockType> hitted = piece.GetBlock(diceNumber);
        cause = "";
        
        // -1. negative condition
        if(!piece.CanMove(hitted.Value)){
            cause += "negative condition " + hitted.Value + ", ";
            return -1;
        }

        // 0. no other choice
        if((inPieces == 1 && diceNumber != 6 && piece.isIn) || (inPieces == 0 && diceNumber == 6)) {
            cause += "no other choice, ";
            return 13;
        }

        // 1. get out of the blocking piece in a base.
        if(!piece.inGoal && piece.position % ((game.board.roadSize / game.board.maxPlayers)) == 0) {
            if(piece.position == startPosition){
                if(inPieces != game.board.pieceForEachPlayer) {
                    heuristic += 11;
                    cause += "our base, ";
                }
            } else {
                heuristic += 10;
                cause += "blocking in a base, ";
            }
        }

        // 2. if you had 6 then you should get in your piece
        if(diceNumber == 6 && inPieces != pieces.Length && !piece.isIn){
            heuristic += 5;
            cause += "6 get in, ";
        }
            
        // 3. try to hit someone
        if(hitted.Value == BlockType.ENEMY) {
            heuristic += 2;
            cause += "hits enemy, ";
        }

        // 3.5. if not in goal
        if(!piece.inGoal){
            heuristic += 1;
            cause += "not in goal, ";
        }
        
        // 4. nearest piece to goal
        if(piece.isIn){
            heuristic += ((float) piece.pacesGone) / ((float) (game.board.roadSize + pieces.Length));
            cause += "near, ";
        }

        return heuristic;
    }
}