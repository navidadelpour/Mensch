using System.Collections.Generic;

public class Player {

    public List<Piece> inPieces;
    public List<Piece> outPieces;
    
    public int piecesCount;
    public int index;

    public Game game;
    public int lastPosition;

    public Player(Game game, int index) {
        this.game = game;
        this.index = index;

        piecesCount = game.board.pieceForEachPlayer;
        outPieces = new List<Piece>();
        inPieces = new List<Piece>();
        
        for(int i = 0; i < piecesCount; i++) {
            outPieces.Add(new Piece(this));
        }

        // 0: 39
        // 1: 9
        // 2: 19
        // 3: 29
        // example: (2 * 40 / 4 - 1) % 40 = 19
        lastPosition = (index * game.board.roadSize / game.board.maxPlayers - 1) % game.board.roadSize;
    }

    public virtual void DoDice() {
        
    }

    public virtual void DoMove(int diceNumber) {

    }

    public bool CanMove(int diceNumber) {
        bool returnValue = inPieces.Capacity != 0;
        if (diceNumber == 6)
            returnValue |= outPieces.Capacity != 0;
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

}