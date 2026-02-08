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

        public override List<BoardSquare> LegalMoves => throw new NotImplementedException();
    }
}
