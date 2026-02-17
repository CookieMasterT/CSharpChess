using CSharpChess.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess
{
    public class Rook : Piece
    {
        public Rook(Team team) : base(team)
        {
        }

        public override string Name { get => ChessNotation.Rook; }

        public override List<BoardSquare> GetLegalMoves(BoardSquare ContainingSquare)
        {
            var MV = MoveConstructor.GetMoveConstructor(this, ContainingSquare);

            MV.LineAdd(0, 1); // move in a line north
            MV.LineAdd(1, 0); // move in a line east
            MV.LineAdd(0, -1); // move in a line south
            MV.LineAdd(-1, 0); // move in a line west

            return MV.GetMoves();
        }
    }
}
