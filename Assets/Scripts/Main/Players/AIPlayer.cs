using System.Collections.Generic;
using System.Collections;
using System;

public class AIPlayer : Player {

    public AIPlayer(Game game, int index) : base(game, index) {

    }

    public override void DoDice() {
        game.RollDice();
    }

    public override void DoMove(int diceNumber, Piece piece = null) {
        base.DoMove(diceNumber, piece);
        string cause = "";
        // Piece bestPiece = GetBestPiece(diceNumber, ref cause);
        Piece bestPiece = GetBestPieceByHeuristic(diceNumber);
        UnityEngine.Debug.Log("BEST PIECE: " + bestPiece + ", Cause: " + cause);
        game.MovePiece(bestPiece);
    }

    private Piece GetBestPieceByHeuristic(int diceNumber) {
        for (int i = 0; i < pieces.Length; i++) {
            pieces[i].heuristic = Heuristic(pieces[i], diceNumber);
        }
        UnityEngine.Debug.Log("before");
        for (int i = 0; i < pieces.Length; i++) {
            UnityEngine.Debug.Log(pieces[i] + " H: " + pieces[i].heuristic);
        }

        Array.Sort(pieces);

        UnityEngine.Debug.Log("after");
        for (int i = 0; i < pieces.Length; i++) {
            UnityEngine.Debug.Log(pieces[i] + " H: " + pieces[i].heuristic);
        }
        return pieces[0];
    }
    private float Heuristic(Piece piece, int diceNumber) {
        // 0. no other choice
        // 1. get out of the blocking piece in a base.
        // 2. if you had 6 then you should get in your piece
        // 3. try to hit someone
        // 4. nearest piece to goal

        float heuristic = 0;
        KeyValuePair<Piece, BlockType> hitted = piece.GetBlock(diceNumber);

        // -1. negative condition
        if(hitted.Value == BlockType.OUTGOAL || hitted.Value == BlockType.ALLY)
            return -1;

        // 0. no other choice
        if((inPieces == 1 && diceNumber != 6 && piece.isIn) ||
            (inPieces == 0 && diceNumber == 6))
            return 13;

        // 1. get out of the blocking piece in a base.
        if(!piece.inGoal && piece.position % ((game.board.roadSize / game.board.maxPlayers)) == 0)
            heuristic += 10;

        // 2. if you had 6 then you should get in your piece
        if(diceNumber == 6 && inPieces != pieces.Length && !piece.isIn)
            heuristic += 5;
            
        // 3. try to hit someone
        if(hitted.Value == BlockType.ENEMY)
            heuristic += 2;

        // 3.5. if not in goal
        if(!piece.inGoal)
            heuristic += 1;
        
        // 4. nearest piece to goal
        if(piece.isIn)
            heuristic += ((float) piece.pacesGone) / ((float) (game.board.roadSize + pieces.Length));

        return heuristic;
    }


    // private Piece GetBestPiece(int diceNumber, ref string cause) {
    //     // priorities:

    //     // 0. no other choice
    //     if(inPieces.Count == 1 && diceNumber != 6) {
    //         KeyValuePair<Piece, BlockType> hitted = inPieces[0].GetBlock(diceNumber);
    //         if(hitted.Value != BlockType.ALLY && hitted.Value != BlockType.OUTGOAL) {
    //             cause = "no other choice";
    //             return inPieces[0];
    //         } else
    //             return null;
    //     }

    //     // will be sorted by position
    //     inPieces.Sort();
    //     inPieces.Reverse();

    //     // 1. get out of the blocking piece in a base.
    //     for(int i = 0; i < inPieces.Count; i++) {
    //         KeyValuePair<Piece, BlockType> hitted = inPieces[i].GetBlock(diceNumber);
    //         if(!inPieces[i].inGoal && inPieces[i].position % ((game.board.roadSize / game.board.maxPlayers)) == 0 && hitted.Value != BlockType.ALLY) {
    //             cause = "in a base";
    //             return inPieces[i];
    //         }
    //     }

    //     // 2. if you had 6 then you should get in your piece
    //     if(diceNumber == 6 && outPieces.Count > 0) {
    //         KeyValuePair<Piece, BlockType> hitted = outPieces[0].GetBlock(diceNumber);
    //         if(hitted.Value != BlockType.ALLY){
    //             cause = "six dice";
    //             return outPieces[0];
                
    //         }
    //     }
        

    //     // 3. try to hit someone
    //     for(int i = 0; i < inPieces.Count; i++) {
    //         KeyValuePair<Piece, BlockType> hitted = inPieces[i].GetBlock(diceNumber);
    //         if(hitted.Value == BlockType.ENEMY) {
    //             cause = "hit someone";
    //             return inPieces[i];
    //         }
    //     }

    //     // 4. nearest piece to goal
    //     for(int i = 0; i < inPieces.Count; i++) {
    //         KeyValuePair<Piece, BlockType> hitted = inPieces[i].GetBlock(diceNumber);
    //         if(hitted.Value != BlockType.ALLY && hitted.Value != BlockType.OUTGOAL) {
    //             cause = "nearest";
    //             return inPieces[i];
    //         }
    //     }

    //     return null;

    // }

}