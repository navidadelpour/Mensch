using System.Collections.Generic;
public class Piece : IComparer<Piece> {
    public Player player;
    public Board board;
    public bool isIn;
    public bool inGoal;
    public int position;

    public Piece(Player player) {
        this.player = player;
        board = player.game.board;
        isIn = false;
        inGoal = false;
    }

    public Piece CheckForward(int diceNumber) {
        if(isIn) {
            int nextPosition = (position + diceNumber) % board.roadSize;
            for(int i = 0; i < board.inPieces.Capacity; i++) {
                if(nextPosition == board.inPieces[i].position)
                    return board.inPieces[i];
            }
            if(nextPosition > player.lastPosition) {
                // goal positions start
                nextPosition -= player.lastPosition - 1;
                if(nextPosition < board.pieceForEachPlayer) {
                    // moving out of the goal situation
                    return this;
                }
            }
        } else {
            return board.GetPieceInPosition(player.lastPosition);
        }
        return null;
    }

    public void GoForward(int diceNumber) {
        int nextPosition = (position + diceNumber) % board.roadSize;
        if(nextPosition > player.lastPosition) {
            // goal positions start
            nextPosition -= player.lastPosition - 1;
            if(nextPosition < board.pieceForEachPlayer) {
                position = nextPosition;
                inGoal = true;
            }
        } else {
            position = nextPosition;
        }
    }

    // TODO: make an event for these.
    public void GetIn() {
        board.Swap(this);
        player.Swap(this);
        isIn = true;
    }

    public void GetOut() {
        board.Swap(this);
        player.Swap(this);
        isIn = false;
    }

    public int Compare(Piece a, Piece b) {
        if (a.position > b.position)
            return 1;
        return 0;
    }

}

