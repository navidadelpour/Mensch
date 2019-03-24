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

}