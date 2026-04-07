using CSharpChess.Board;
using CSharpChess.Game;
using System.Globalization;
using WebServer.RequestTypes;

namespace WebServer.GameActions
{
    internal static class MovePieceWithPromotion
    {
        public static void Execute(MovePieceWithPromotionParams info)
        {
            if (info.startX == null || info.startY == null || info.endX == null || info.endY == null ||
                !(info.promotionPiece == ChessNotation.Queen || info.promotionPiece == ChessNotation.Rook || info.promotionPiece == ChessNotation.Bishop || info.promotionPiece == ChessNotation.Knight))
            {
                return;
            }

            ChessBoard.MovePiece(
                int.Parse(info.startX, CultureInfo.InvariantCulture),
                int.Parse(info.startY, CultureInfo.InvariantCulture),
                int.Parse(info.endX, CultureInfo.InvariantCulture),
                int.Parse(info.endY, CultureInfo.InvariantCulture),
                GameLogic.ChessBoard,
                promotionPiece: info.promotionPiece);
        }
    }
}
