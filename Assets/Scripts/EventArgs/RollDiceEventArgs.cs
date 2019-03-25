using System;

public class RollDiceEventArgs : EventArgs {
    public readonly int diceNumber;
    public readonly Player player;

    public RollDiceEventArgs(int diceNumber, Player player) {
        this.diceNumber = diceNumber;
        this.player = player;
    }

}