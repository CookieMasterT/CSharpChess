using CSharpChess.Game;
using CSharpChess.Board;
using WebServer.RequestTypes;

namespace WebServer.GameActions
{
    internal static class MovePiece
    {
        public static void Execute(MovePieceParams info)
        {
            if (info.startX == null || info.startY == null || info.endX == null || info.endY == null)
                return;
            ChessBoard.MovePiece(int.Parse(info.startX), int.Parse(info.startY), int.Parse(info.endX), int.Parse(info.endY), GameLogic.ChessBoard);
        }
    }
}
