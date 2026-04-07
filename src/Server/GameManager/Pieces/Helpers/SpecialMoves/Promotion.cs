namespace CSharpChess.Pieces.Helpers.SpecialMoves
{
    internal class Promotion(PromotionPiece promotionPiece) : SpecialMoveInfo
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
