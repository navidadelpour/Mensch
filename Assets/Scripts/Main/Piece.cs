public class Piece {
    public Player player;
    public bool isIn;
    public int position;

    public Piece(Player player) {
        this.player = player;
        isIn = false;
    }

    public Piece CheckForward(Board board) {
        int nextPosition = position;
        nextPosition += diceNumber;
        for(int i = 0; i < board.inPieces.Length; i++) {
            if(nextPosition == board.inPieces[i].position)
                return board.inPieces[i];
        }
        return null;
    }

    public void GoForward(int diceNumber) {
        position += diceNumber;
    }

    public void GetIn() {
        isIn = true;
    }

    public void GetOut() {
        isIn = false;
    }

}