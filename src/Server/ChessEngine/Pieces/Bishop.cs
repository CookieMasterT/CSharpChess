using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess
{
    public class Bishop : Piece
    {
        public Bishop(Team team) : base(team)
        {
        }

        public override string[] Name { get => ChessNotation.Bishop; }

        public override List<BoardSquare> LegalMoves => throw new NotImplementedException();
    }
}
