using System.Collections.Generic;
using System;

[Serializable]
public class Player {

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

    public virtual void DoDice() {
        game.waitingForUserToDice = true;
        while(game.waitingForUserToDice) {
            game.internalDelay.WaitOne(200);
        }
        game.waitingForUserToDice = true;
        game.TryThrowDice();
    }

    public virtual void DoMove(int diceNumber) {
        game.waitingForUserToMove = true;
        while(game.waitingForUserToMove) {
            game.internalDelay.WaitOne(200);
        }
        game.waitingForUserToMove = true;
        game.TryMovePiece(game.playerSelectedPiece);
    }

    public bool CanMove(int diceNumber) {
        for(int i = 0; i < pieces.Length; i++) {
            KeyValuePair<Piece, BlockType> hitted = pieces[i].GetBlock(diceNumber);
            if(pieces[i].CanMove(hitted.Value)) {
                return true;
            }
        }
        return false;
    }

}