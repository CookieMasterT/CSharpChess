using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess
{
    public enum Team { white, black }

    internal abstract class Piece
    {
        protected Piece(BoardSquare position, Team team)
        {
            _position = position;
            Team = team;
        }

        private BoardSquare _position;
        public BoardSquare Position { get { return _position; } }

        public readonly Team Team;

        abstract public bool DoMove(); // Attempt to move somewhere
        abstract public List<BoardSquare> LegalMoves { get; } // list of moves the piece can do
    }
}
