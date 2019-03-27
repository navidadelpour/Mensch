using System;

public class MovePieceEventArgs : EventArgs {
    public readonly Piece piece;
    public readonly int diceNumber;

    public MovePieceEventArgs(Piece piece, int diceNumber) {
        this.piece = piece;
        this.diceNumber = diceNumber;
    }

}