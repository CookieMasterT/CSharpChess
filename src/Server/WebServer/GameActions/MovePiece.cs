using System;
using System.Collections.Generic;
using System.Text;
using WebServer.RequestTypes;

namespace WebServer.GameActions
{
    internal static class MovePiece
    {
        public static void Execute(MovePieceParams info)
        {
            if (info.startX == null || info.startY == null || info.endX == null || info.endY == null)
                return;
            CSharpChess.ChessBoard.MovePiece(int.Parse(info.startX), int.Parse(info.startY), int.Parse(info.endX), int.Parse(info.endY));
        }
    }
}
