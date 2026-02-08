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

        public override List<BoardSquare> LegalMoves => throw new NotImplementedException();
    }
}
