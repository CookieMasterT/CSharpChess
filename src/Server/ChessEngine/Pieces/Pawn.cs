using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess
{
    public class Pawn : Piece
    {
        public Pawn(Team team) : base(team)
        {
        }

        public override string[] Name { get => ChessNotation.Pawn; }

        public override List<BoardSquare> LegalMoves => throw new NotImplementedException();
    }
}
