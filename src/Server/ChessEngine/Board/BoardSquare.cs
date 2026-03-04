using CSharpChess.Game;
using CSharpChess.Pieces;

namespace CSharpChess.Board
{
    public class BoardSquare
    {
        private int _x; // The current file indexed from 0
        private int _y; // The current rank indexed from 0

        public Piece? content;

        private BoardSquare() { }
        public BoardSquare(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int X
        {
            get => _x;
            set
            {
                if (value is >= 0 and < 8)
                    _x = value;
                else
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
        public int Y
        {
            get => _y;
            set
            {
                if (value is >= 0 and < 8)
                    _y = value;
                else
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }

        public char File => ChessNotation.LegalFileNames[_x];
        public char Rank => ChessNotation.LegalRankNames[_y];

        public override string ToString() => $"{File}{Rank}";
        public override int GetHashCode() => HashCode.Combine(X, Y);
    }
}
