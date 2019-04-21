using System.Collections.Generic;
using System;

[Serializable]
public abstract class Player {

    public Piece[] pieces;
    public int inPieces;
    
    public int index;
    public Game game;
    public int startPosition;

    public Player(Game game, int index) {
        this.game = game;
        this.index = index;

        InitPieces();
        startPosition = (index * game.board.roadSize / game.board.maxPlayers) % game.board.roadSize;
    }

    private void InitPieces() {
        pieces = new Piece[game.board.pieceForEachPlayer];
        for(int i = 0; i < pieces.Length; i++) {
            pieces[i] = new Piece(this, i);
        }
    }

    public abstract bool Dice();
    public abstract Piece Choose(int diceNumber);

    public bool CanMove(int diceNumber) {
        for(int i = 0; i < pieces.Length; i++) {
            Block hitted = pieces[i].GetBlock(diceNumber);
            if(pieces[i].CanMove(hitted.type)) {
                return true;
            }
        }
        return false;
    }

}