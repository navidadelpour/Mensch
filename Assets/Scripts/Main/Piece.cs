using System;
using System.Collections.Generic;

[Serializable]
public class Piece : IComparable<Piece> {
    public Player player;
    public Board board;
    public bool isIn;
    public bool inGoal;
    public int position;
    public int pacesGone;
    public int index;

    public Piece(Player player, int index) {
        this.player = player;
        this.index = index;
        board = player.game.board;
        isIn = false;
        inGoal = false;

        position = -1;
    }

    public KeyValuePair<Piece, BlockType> GetBlock(int diceNumber) {
        int nextPosition;
        BlockType blockType = BlockType.NOTHING;
        if(isIn) {
            nextPosition = pacesGone  + diceNumber - board.roadSize;
            if(nextPosition >= 0) {
                // reached the goal
                // moving outside of the goal situation
                blockType = BlockType.OUTGOAL;
                if(nextPosition < board.pieceForEachPlayer) {
                    // moving inside of the goal situation
                    blockType = BlockType.INGOAL;
                    foreach (Piece piece in player.inPieces)
                        if(nextPosition == piece.position)
                            return new KeyValuePair<Piece, BlockType>(piece, BlockType.ALLY);
                }
            } else {
                // not reached the goal
                nextPosition = (position + diceNumber) % board.roadSize;
                foreach (Piece piece in board.inPieces)
                    if(nextPosition == piece.position)
                        return new KeyValuePair<Piece, BlockType>(piece, player == piece.player ? BlockType.ALLY : BlockType.ENEMY);
            }
        } else {
            Piece piece = board.GetPieceInPosition(player.startPosition);
            if(piece != null)
                blockType = player == piece.player ? BlockType.ALLY : BlockType.ENEMY;
            return new KeyValuePair<Piece, BlockType>(piece, blockType);
        }
        return new KeyValuePair<Piece, BlockType>(null, blockType);
    }

    public void GoForward(int diceNumber, bool inGoalArea = false) {
        int nextPosition;
        if(inGoalArea) {
            nextPosition = pacesGone  + diceNumber - board.roadSize;
            inGoal = true;
            player.CheckForWin();
        } else {
            nextPosition = (position + diceNumber) % board.roadSize;
        }
        position = nextPosition;
        pacesGone += diceNumber;
    }

    public void GetIn() {
        board.Swap(this);
        player.Swap(this);
        isIn = true;
        position = player.startPosition;
    }

    public void GetOut() {
        board.Swap(this);
        player.Swap(this);
        isIn = false;
        position = -1;
        pacesGone = 0;
    }

    public int CompareTo(Piece b) {
        return this.pacesGone.CompareTo(b.pacesGone);
    }

    public override string ToString() {
        return String.Format("piece {0} of player {1} in position {2}", index, player.index, position);
    }
}

