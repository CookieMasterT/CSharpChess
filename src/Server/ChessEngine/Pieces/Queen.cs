using CSharpChess.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess
{
    public class Queen : Piece
    {
        public Queen(Team team) : base(team)
        {
        }

        public override string Name { get => ChessNotation.Queen; }

        public override List<BoardSquare> GetLegalMoves(BoardSquare ContainingSquare)
        {
            var MV = MoveConstructor.GetMoveConstructor(this, ContainingSquare);

            // move in a line in all directions
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i != 0 || j != 0)
                        MV.LineAdd(i, j);
                }
            }

            return MV.GetMoves();
        }
    }
}
