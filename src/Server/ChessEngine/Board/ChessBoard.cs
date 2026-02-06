using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess
{
    internal static class ChessBoard
    {
        public static List<Piece> Pieces = new List<Piece> { };

        public static bool IsTileOccupied(BoardSquare Tile)
        {
            foreach (var item in Pieces)
            {
                if (item.Position.Equals(Tile))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
