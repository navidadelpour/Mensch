using System;

public class MovePieceEventArgs : EventArgs {
    public readonly Piece piece;
    public readonly int diceNumber;
    public readonly Steps steps;

    public MovePieceEventArgs(Piece piece, int diceNumber, Steps steps) {
        this.piece = piece;
        this.diceNumber = diceNumber;
        this.steps = steps;
    }

}