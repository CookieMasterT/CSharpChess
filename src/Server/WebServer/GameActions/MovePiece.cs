using CSharpChess.Board;
using CSharpChess.Game;
using System.Globalization;
using WebServer.RequestTypes;

namespace WebServer.GameActions
{
    internal static class MovePiece
    {
        public static void Execute(MovePieceParams info)
        {
            if (info.startX == null || info.startY == null || info.endX == null || info.endY == null)
                return;
            ChessBoard.MovePiece(
                int.Parse(info.startX, CultureInfo.InvariantCulture),
                int.Parse(info.startY, CultureInfo.InvariantCulture),
                int.Parse(info.endX, CultureInfo.InvariantCulture),
                int.Parse(info.endY, CultureInfo.InvariantCulture),
                Program.GameLogicMain.ChessBoard);
        }
    }
}
