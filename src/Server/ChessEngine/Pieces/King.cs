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

        public override List<BoardSquare> LegalMoves => throw new NotImplementedException();
    }
}
