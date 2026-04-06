using CSharpChess.Game;
using CSharpChess.Board;
using WebServer.RequestTypes;

namespace WebServer.GameActions
{
    internal static class MovePieceWithPromotion
    {
        public static void Execute(MovePieceWithPromotionParams info)
        {
            if (info.startX == null || info.startY == null || info.endX == null || info.endY == null || 
                !(info.promotionPiece == ChessNotation.Queen || info.promotionPiece == ChessNotation.Rook || info.promotionPiece == ChessNotation.Bishop || info.promotionPiece == ChessNotation.Knight))
                return;
            ChessBoard.MovePiece(int.Parse(info.startX), int.Parse(info.startY), int.Parse(info.endX), int.Parse(info.endY), GameLogic.ChessBoard, promotionPiece:info.promotionPiece);
        }
    }
}
