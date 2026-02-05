using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess
{
    internal abstract class PieceAbstract
    {
        private BoardSquare _position;
        public BoardSquare Position { get { return _position; } }

        abstract public bool DoMove(); // Attempt to move somewhere
        abstract public List<BoardSquare> LegalMoves { get; } // list of moves the piece can do
    }
}
