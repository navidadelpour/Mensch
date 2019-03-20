public class Game {

    public GameState state;
    public Board board;
    public Player[] players;
    public int currentPlayerIndex;

    public int diceNumber;

    public Game(Board board, PlayerType[] playerTypes) {
        this.board = board;

        // instantiate players
        players = new Player[playerTypes.Length];
        for(int i = 0; i < playerTypes.Length; i++) {
            switch (playerTypes[i]) {
                case PlayerType.HUMAN:
                    players[i] = new Player(this, i);
                    break;
                case PlayerType.AI:
                    players[i] = new AIPlayer(this, i);
                    break;
                default:
                    players[i] = null;
                    break;
            }
        }

        // default values for game
        state = GameState.DICE;
        currentPlayerIndex = 0;

        // start the game
        players[currentPlayerIndex].DoDice();
    }

    // onclick listener for piece clicking
    public void MovePiece(Piece piece) {
        if(state != GameState.MOVE) {
            return;
        }

        if(piece == null) {
            // means when we cant move any piece
            return;
            NextPlayer();
        }

        if(players[currentPlayerIndex] == piece.player) {
            if(piece.isIn){
                // check our forward step
                Piece possibleHittedPiece = piece.CheckForward(diceNumber);
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
                    // or we are moving out of the goal area
                    return;
                }
            } else if(diceNumber == 6) {
                // check our forward step
                Piece possibleHittedPiece = piece.CheckForward(diceNumber);
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
                    // or we are moving out of the goal area
                    return;
                }
            }
        } else {
            // cant play because it's not your turn
        }
    }

    public void RollDice() {
        if(state != GameState.DICE) {
            return;
        }

        diceNumber = 4;
        state = GameState.MOVE;

        Player player = players[currentPlayerIndex];
        if(player.CanMove(diceNumber)) {
            player.DoMove(diceNumber);
        } else {
            NextPlayer();
        }
    }

    public void NextPlayer() {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
        state = GameState.DICE;
        
        players[currentPlayerIndex].DoDice();
    }

}