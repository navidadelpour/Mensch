using System;

public class RollDiceEventArgs : EventArgs {
    public readonly int diceNumber;
    public readonly Player player;
    public readonly bool canMove;

    public RollDiceEventArgs(int diceNumber, Player player, bool canMove) {
        this.diceNumber = diceNumber;
        this.player = player;
        this.canMove = canMove;
    }

}