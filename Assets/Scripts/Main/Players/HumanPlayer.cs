using System.Collections.Generic;
using System.Collections;
using System;

public class HumanPlayer : Player {

    public HumanPlayer(Game game, int index) : base(game, index) {

    }

    public override bool Dice() {
        wait(ref game.waitingForUserToDice);
        return true;
    }

    public override Piece Choose(int diceNumber) {
        wait(ref game.waitingForUserToChoose);
        return game.playerSelectedPiece;
    }

    private void wait(ref bool condition) {
        condition = true;
        while(condition) {
            game.Delay();
        }
    }

}