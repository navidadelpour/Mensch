using System.Collections.Generic;
public class Board {

    public int maxPlayers;
    public int roadSize;
    public int pieceForEachPlayer;

    public List<Piece> inPieces;

    public Board(int maxPlayers, int roadSize, int pieceForEachPlayer) {
        this.maxPlayers = maxPlayers;
        this.roadSize = roadSize;
        this.pieceForEachPlayer = pieceForEachPlayer;

        inPieces = new List<Piece>();
    }

    public Piece GetPieceInPosition(int position) {
        for(int i = 0; i < inPieces.Capacity; i++) {
            if(inPieces[i].position == position) {
                return inPieces[i];
            }
        }
        return null;
    }

    // TODO: modify inPieces list
    public void Swap(Piece piece) {
        if(piece.isIn) {
            inPieces.Remove(piece);
        } else {
            inPieces.Add(piece);
        }
    }


}