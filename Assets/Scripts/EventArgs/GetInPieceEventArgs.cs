using System;

public class GetInPieceEventArgs : EventArgs {
    public readonly Piece piece;

    public GetInPieceEventArgs(Piece piece) {
        this.piece = piece;
    }

}