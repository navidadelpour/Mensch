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

    public int CompareTo(Piece b) {
        int compareValue = b.heuristic.CompareTo(heuristic);
        if(compareValue == 0)
            compareValue = index.CompareTo(b.index);

        return compareValue;
    }

    public override string ToString() {
        return String.Format("piece {0} of player {1} in position {2}", index, player.index, position);
    }

    public Block GetBlock(int diceNumber) {
        int nextPosition = 0;
        Piece hitted = null;
        BlockType blockType = BlockType.NOTHING;
        CalculateNextPositionAndBlockType(diceNumber, ref nextPosition, ref blockType);
        hitted = CalculateHittedPiece(nextPosition, ref blockType);
        return new Block(hitted, blockType);
    }

    private void CalculateNextPositionAndBlockType(int diceNumber, ref int nextPosition, ref BlockType blockType) {
        if(isIn) {
            nextPosition = CalculateInGoalMovement(diceNumber);
            if(nextPosition >= 0) {
                // reached the goal
                if(nextPosition < board.pieceForEachPlayer) {
                    // moving inside of the goal situation
                    blockType = BlockType.INGOAL;
                } else {
                    // moving outside of the goal situation
                    blockType = BlockType.OUTGOAL;
                }
            } else {
                // not reached the goal
                nextPosition = CalculateInRoadMovement(diceNumber);
            }
        } else if(diceNumber == 6){
            // get in proccess
            nextPosition = player.startPosition;
        } else {
            // not allowed
            blockType = BlockType.OUTGOAL;
        }
    }

    private Piece CalculateHittedPiece(int nextPosition, ref BlockType blockType) {
        if(blockType != BlockType.OUTGOAL) {
            foreach (Piece piece in board.inPieces) {
                if(nextPosition == piece.position) {
                    if(piece.Belongs(player)) {
                        if((blockType == BlockType.INGOAL) == piece.inGoal) {
                            blockType = BlockType.ALLY;
                            return piece;
                        }
                    } else {
                        if(blockType != BlockType.INGOAL && !piece.inGoal) {
                            blockType = BlockType.ENEMY;
                            return piece;
                        }
                    }
                }
            }
        }
        return null;
    }

    private int CalculateInGoalMovement(int diceNumber) {
        return pacesGone + diceNumber - board.roadSize;
    }

    private int CalculateInRoadMovement(int diceNumber) {
        return (position + diceNumber) % board.roadSize;
    }

    private int calculatePacesGone(int diceNumber) {
        return pacesGone + diceNumber;
    }

    public Steps Go(BlockType blockType, int diceNumber) {
        Func<int, int> calculateMovement = CalculateInRoadMovement;
        int[] positions = new int[diceNumber];
        int inGoalIndex = -1;
        int nextPacesGone = pacesGone;
        inGoal = inGoal || blockType == BlockType.INGOAL;

        for(int i = 0; i < diceNumber; i++) { 
            nextPacesGone = calculatePacesGone(i + 1);
            if(inGoalIndex == -1 && inGoal && nextPacesGone >= board.roadSize) {
                inGoalIndex = i;
                calculateMovement = CalculateInGoalMovement;
            }
            positions[i] = calculateMovement(i + 1);
        }

        position = calculateMovement(diceNumber);
        pacesGone = calculatePacesGone(diceNumber);

        return new Steps(positions, inGoalIndex);
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
        pacesGone = calculatePacesGone(-pacesGone);
    }

    public bool CanMove(BlockType blockType) {
        return blockType != BlockType.OUTGOAL && blockType != BlockType.ALLY;
    }

    public bool Belongs(Player player) {
        return this.player == player;
    }

}

