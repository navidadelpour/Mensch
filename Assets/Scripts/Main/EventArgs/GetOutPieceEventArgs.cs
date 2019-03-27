using System;

public class GetOutPieceEventArgs : EventArgs {
    public readonly Piece piece;

    public GetOutPieceEventArgs(Piece piece) {
        this.piece = piece;
    }

}