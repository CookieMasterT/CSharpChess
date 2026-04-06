namespace WebServer.RequestTypes
{
    internal class MovePieceWithPromotionParams
    {
        public string? startX = null!;
        public string? startY = null!;
        public string? endX = null!;
        public string? endY = null!;
        public string? promotionPiece = null!;
    }
}
