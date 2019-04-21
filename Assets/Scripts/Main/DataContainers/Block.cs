public class Block {
    public BlockType type;
    public Piece piece;

    public Block (Piece piece, BlockType type) {
        this.type = type;
        this.piece = piece;
    }
}