namespace WebServer.RequestTypes
{
#pragma warning disable CS8618 // Non-nullable field is uninitialized
    internal class MovePieceWithPromotionParams
    {
        public string? startX;
        public string? startY;
        public string? endX;
        public string? endY;
        public string? promotionPiece;
    }
#pragma warning restore CS8618
}
