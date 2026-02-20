using CSharpChess;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using WebServer.RequestTypes;

namespace WebServer.GameDataParsers
{
    internal class PieceMoves
    {
        public static string GetJson(CoordinateInfo position)
        {
            var json = new JArray();
            BoardSquare? tile = CSharpChess.ChessBoard.Board[position.x, position.y];
            Piece? piece = tile?.content;
            if (piece is null || tile is null)
                return "";
            foreach (var item in piece.GetLegalMoves(tile))
            {
                json.Add(new JArray(new int[] {item.X, item.Y}));
            }
            return json.ToString();
        }
    }
}
