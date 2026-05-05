#pragma warning disable CA1812, CS8618, CS0649 // Non-nullable field is uninitialized, field is never assigned to, and will always have its default value null
namespace WebServer.RequestTypes
{
    internal class MovePieceWithPromotionParams
    {
        public string? startX;
        public string? startY;
        public string? endX;
        public string? endY;
        public string? promotionPiece;
    }
}
#pragma warning restore CA1812, CS8618, CS0649