using System;

public class WinEventArgs : EventArgs {

    public readonly Player player;

    public WinEventArgs(Player player) {
        this.player = player;
    }

}