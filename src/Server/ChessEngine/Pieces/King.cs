using CSharpChess.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess
{
    public class King : Piece
    {
        public King(Team team) : base(team)
        {
        }

        public override string Name { get => ChessNotation.King; }

        public override List<BoardSquare> GetLegalMoves(BoardSquare ContainingSquare)
        {
            var MV = MoveConstructor.GetMoveConstructor(this, ContainingSquare);

            //todo: en passant
            //todo: king can't move on any enemy legal capturing moves

            // king moves in a 3x3 on top of itself (any adjacent square)
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    MV.TryAdd(i, j);
                }
            }

            return MV.GetMoves();
        }
    }
}
