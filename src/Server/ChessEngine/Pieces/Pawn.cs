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

        public override List<BoardSquare> LegalMoves => throw new NotImplementedException();
    }
}
