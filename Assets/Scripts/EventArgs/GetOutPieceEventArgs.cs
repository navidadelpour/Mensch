using System;

public class GetOutPieceEventArgs : EventArgs {
    public readonly Piece piece;
    public readonly Piece enemy;

    public GetOutPieceEventArgs(Piece piece, Piece enemy) {
        this.piece = piece;
        this.enemy = enemy;
    }

}