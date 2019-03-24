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
    public float heuristic;
    public int index;

    public Piece(Player player, int index) {
        this.player = player;
        this.index = index;
        board = player.game.board;
        isIn = false;
        inGoal = false;

        position = index;
    }

    public KeyValuePair<Piece, BlockType> GetBlock(int diceNumber) {
        int nextPosition = 0;
        BlockType blockType = BlockType.NOTHING;
        if(isIn) {
            nextPosition = pacesGone  + diceNumber - board.roadSize;
            if(nextPosition >= 0) {
                // reached the goal
                if(nextPosition < board.pieceForEachPlayer) {
                    // moving inside of the goal situation
                    blockType = BlockType.INGOAL;
                } else {
                    // moving outside of the goal situation
                    blockType = BlockType.OUTGOAL;
                    return new KeyValuePair<Piece, BlockType>(null, blockType);
                }
            } else {
                // not reached the goal
                nextPosition = (position + diceNumber) % board.roadSize;
            }
        } else if(diceNumber == 6){
            // get in proccess
            nextPosition = player.startPosition;
        } else {
            // not allowed
            blockType = BlockType.OUTGOAL;
            return new KeyValuePair<Piece, BlockType>(null, blockType);
        }

        foreach (Piece piece in board.inPieces)
            if(nextPosition == piece.position && inGoal == piece.inGoal)
                return new KeyValuePair<Piece, BlockType>(piece, player == piece.player ? BlockType.ALLY : BlockType.ENEMY);

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
        board.inPieces.Add(this);
        player.inPieces ++;
        isIn = true;
        position = player.startPosition;
    }

    public void GetOut() {
        board.inPieces.Remove(this);
        player.inPieces --;
        isIn = false;
        position = index;
        pacesGone = 0;
    }

    public int CompareTo(Piece b) {
        int compareValue = b.heuristic.CompareTo(heuristic);
        if(compareValue == 0)
            compareValue = index.CompareTo(b.index);

        return compareValue;
    }

    public override string ToString() {
        return String.Format("piece {0} of player {1} in position {2}", index, player.index, position);
    }
}

