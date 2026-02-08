using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CSharpChess
{
    public static class ChessBoard
    {
        static ChessBoard()
        {
            Board = new BoardSquare[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int k = 0; k < 8; k++)
                {
                    Board[i, k] = new BoardSquare(i, k);
                }
            }
        }
        public static BoardSquare[,] Board;
    }
}
