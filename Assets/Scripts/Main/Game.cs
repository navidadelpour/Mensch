using System;

public class Game {

    GameStates state;
    Board board;
    Player[] players;
    int currentPlayerIndex;

    int diceNumber;

    public Game(Board board, PlayerTypes[] playerTypes) {
        // instantiate players
        players = new Player[playerTypes.Length];
        for(int i = 0; i < playerTypes.Length; i++) {
            switch (type) {
                case PlayerTypes.HUMAN:
                    players[i] = new Player(this);
                    break;
                case PlayerTypes.AI:
                    players[i] = new AIPlayer(this);
                    break;
                default:
                    players[i] = null;
                    break;
            }
        }

        // default values for game
        state = GameStates.DICE;
        currentPlayerIndex = 0;
        players[currentPlayerIndex].Act();
    }

    // onclick listener for piece clicking
    public void MovePiece(Piece piece) {
        if(piece == null) {
            // means when we cant move any piece
            return;
            NextPlayer();
        }

        if(players[currentPlayerIndex] == piece.player) {
            if(piece.isIn){
                // check our forward step
                Piece possibleHittedPiece = piece.CheckForward(board);
                if(possibleHittedPiece == null){
                    // no one is in the way
                    piece.GoForward(diceNumber);
                    NextPlayer();
                } else if(possibleHittedPiece.player != piece.player){
                    // hit the enemy
                    possibleHittedPiece.GetOut();
                    piece.GoForward(diceNumber);
                    NextPlayer();
                } else {
                    // another piece of ours is blocking the way
                    return;
                }
            } else {
                if(diceNumber == 6) {
                    // check our forward step
                    Piece possibleHittedPiece = piece.CheckForward(board);
                    if(possibleHittedPiece == null){
                        // no one is in the way
                        piece.GetIn();
                        NextPlayer();
                    } else if(possibleHittedPiece.player != piece.player){
                        // hit the enemy
                        possibleHittedPiece.GetOut();
                        piece.GetIn();
                        NextPlayer();
                    } else {
                        // another piece of ours is blocking the way
                        return;
                    }
                }
            }
        } else {
            // cant play because it's not your turn
        }
    }

    public void RollDice() {
        diceNumber = Random.Range(1, 7);
        state = state = GameStates.MOVE;

        Player player = players[currentPlayerIndex];
        if(player.CanMove(diceNumber)) {
            player.DoMove(diceNumber);
        } else {
            NextPlayer();
        }
    }

    public void NextPlayer() {
        currentPlayerIndex ++;
        state = GameStates.DICE;
        
        players[currentPlayerIndex].DoDice();
    }

}