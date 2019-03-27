public class PlayerFactory {
    public static Player Create(PlayerType type, Game game, int index) {
        switch (type) {
            case PlayerType.HUMAN:
                return new Player(game, index);

            case PlayerType.AI:
                return new AIPlayer(game, index);
                
            default:
                return null;
        }
    }
}