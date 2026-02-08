using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess
{
    public class Knight : Piece
    {
        public Knight(Team team) : base(team)
        {
        }

        public override string[] Name { get => ChessNotation.Knight; }

        public override List<BoardSquare> LegalMoves => throw new NotImplementedException();
    }
}
