using System.Collections.Generic;
using System.Collections;
using System;

public class HumanPlayer : Player {

    public HumanPlayer(Game game, int index) : base(game, index) {

    }

    public override void DoDice() {
        wait(ref game.waitingForUserToDice);
        game.TryThrowDice();
    }

    public override void DoMove(int diceNumber) {
        wait(ref game.waitingForUserToMove);
        game.TryMovePiece(game.playerSelectedPiece);
    }

    private void wait(ref bool condition) {
        condition = true;
        while(condition) {
            game.internalDelay.WaitOne(200);
        }
        condition = false;
    }

}