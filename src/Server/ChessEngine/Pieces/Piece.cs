using CSharpChess.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess
{
    public enum Team { white, black }

    public abstract class Piece
    {
        protected Piece(BoardSquare position, Team team)
        {
            Team = team;
        }

        public string Name { get => ChessNotation.PiecePlaceHolder; }
        public string FullName { get => $"{Team}:{ChessNotation.PiecePlaceHolder}"; }

        public readonly Team Team;

        abstract public List<BoardSquare> LegalMoves { get; } // list of moves the piece can do
    }
}
