using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess
{
    public abstract class Piece
    {
        public Piece(Team team)
        {
            Team = team;
        }

        public virtual string Name { get => ChessNotation.PiecePlaceHolder; }

        public bool hasMoved = false;

        public readonly Team Team;

        abstract public List<BoardSquare> GetLegalMoves(BoardSquare ContainingSquare);
    }
}
