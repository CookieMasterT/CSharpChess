namespace CSharpChess.Pieces.Helpers.SpecialMoves
{
    internal sealed class Promotion(PromotionPiece promotionPiece) : SpecialMoveInfo
    {
        public PromotionPiece PromotionPiece { get; } = promotionPiece;
    }

    enum PromotionPiece
    {
        Queen,
        Rook,
        Bishop,
        Knight
    }
}
