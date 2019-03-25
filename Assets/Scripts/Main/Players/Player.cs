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

        InitPieces();
        startPosition = (index * game.board.roadSize / game.board.maxPlayers) % game.board.roadSize;
        lastPosition = startPosition - 1 + ((startPosition - 1) < 0 ? game.board.roadSize : 0);
    }

    private void InitPieces() {
        pieces = new Piece[game.board.pieceForEachPlayer];
        for(int i = 0; i < pieces.Length; i++) {
            pieces[i] = new Piece(this, i);
        }
    }

    public virtual void DoDice() {
        
    }

    public virtual void DoMove(int diceNumber) {

    }

}