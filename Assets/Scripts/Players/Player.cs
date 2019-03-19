using System.Collections.Generic;

public class Player {

    List<Piece> inPieces;
    List<Piece> outPieces;

    Game game;

    public Player(Game game) {
        this.game = game;

        outPieces = new List<Piece>();
        inPieces = new List<Piece>();
        
        for(int i = 0; i < 4; i++) {
            outPieces.Add(new Piece(this));
        }
    }

    public virtual void DoDice() {
        
    }

    public virtual void DoMove(int diceNumber) {

    }

    public bool CanMove(int diceNumber) {
        bool returnValue = inPieces.Length != 0;
        if (diceNumber == 6)
            returnValue |= outPieces.Length != 0;
        return returnValue;
    }

}