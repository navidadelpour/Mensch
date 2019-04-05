using System;

public class MovePieceEventArgs : EventArgs {
    public readonly Piece piece;
    public readonly int diceNumber;
    public readonly int[] steps;
    public readonly int inGoalIndex;

    public MovePieceEventArgs(Piece piece, int diceNumber, int[] steps, int inGoalIndex) {
        this.piece = piece;
        this.diceNumber = diceNumber;
        this.steps = steps;
        this.inGoalIndex = inGoalIndex;
    }

}