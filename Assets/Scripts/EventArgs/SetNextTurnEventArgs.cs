using System;

public class SetNextTurnEventArgs : EventArgs {
    public readonly Player player;

    public SetNextTurnEventArgs(Player player) {
        this.player = player;
    }

}