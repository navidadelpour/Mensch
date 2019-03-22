using System.Collections.Generic;
using System;

[Serializable]
public class Player {

    public List<Piece> inPieces;
    public List<Piece> outPieces;
    
    public int piecesCount;
    public int index;

    public Game game;
    public int lastPosition;
    public int startPosition;

    public Player(Game game, int index) {
        this.game = game;
        this.index = index;

        piecesCount = game.board.pieceForEachPlayer;
        outPieces = new List<Piece>();
        inPieces = new List<Piece>();
        
        for(int i = 0; i < piecesCount; i++) {
            outPieces.Add(new Piece(this, i));
        }

        // 0: 39
        // 1: 9
        // 2: 19
        // 3: 29
        // example: (2 * 40 / 4 - 1) % 40 = 19
        startPosition = (index * game.board.roadSize / game.board.maxPlayers) % game.board.roadSize;
        lastPosition = startPosition - 1 + ((startPosition - 1) < 0 ? game.board.roadSize : 0);
    }

    public virtual void DoDice() {
        
    }

    public virtual void DoMove(int diceNumber, Piece piece = null) {
        if(piece == null)
            return;
        else {
            // check the validity
        }
    }

    public bool CanMove(int diceNumber) {
        bool returnValue = inPieces.Count != 0;
        if (diceNumber == 6)
            returnValue |= outPieces.Count != 0;
        return returnValue;
    }

    public void Swap(Piece piece) {
        if(piece.isIn) {
            inPieces.Remove(piece);
            outPieces.Add(piece);
        } else {
            outPieces.Remove(piece);
            inPieces.Add(piece);
        }
    }

    public void CheckForWin() {
        bool win = false;
        if(outPieces.Count == 0) {
            win = true;
            for(int i = 0; i < inPieces.Count; i++) {
                if(!inPieces[i].inGoal)
                    win = false;
            }
        }
        if(win) {
            game.winnedPlayer = this;
        }
    }

}