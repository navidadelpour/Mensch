using System.Collections.Generic;
using System;

[Serializable]
public class Player {

    public Piece[] pieces;
    public int inPieces;
    
    public int index;
    public Game game;
    public int lastPosition;
    public int startPosition;

    public Player(Game game, int index) {
        this.game = game;
        this.index = index;

        pieces = new Piece[game.board.pieceForEachPlayer];
        for(int i = 0; i < pieces.Length; i++) {
            pieces[i] = new Piece(this, i);
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
        bool returnValue = inPieces != 0;
        if (diceNumber == 6)
            returnValue |= inPieces != pieces.Length;
        return returnValue;
    }

    public void CheckForWin() {
        bool win = false;
        if(inPieces == pieces.Length) {
            win = true;
            for(int i = 0; i < pieces.Length; i++) {
                if(!pieces[i].inGoal)
                    win = false;
            }
        }
        if(win) {
            game.winnedPlayer = this;
        }
    }

}