using CSharpChess.Game;
using CSharpChess.Pieces;

namespace CSharpChess.Board
{
    public class BoardSquare : IEquatable<BoardSquare>
    {
        private int _x; // The current file indexed from 0
        private int _y; // The current rank indexed from 0

        public Piece? Content { get; set; }

        private BoardSquare() { }
        public BoardSquare(int x, int y)
        {
            _x = x;
            _y = y;
        }
        public BoardSquare(BoardSquare squareToCopy)
        {
            ArgumentNullException.ThrowIfNull(squareToCopy);

            _x = squareToCopy._x;
            _y = squareToCopy._y;
            Content = squareToCopy.Content;
        }

        public int X
        {
            get => _x;
            set
            {
                if (value is >= 0 and < ChessBoard.BoardSize)
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
                if (value is >= 0 and < ChessBoard.BoardSize)
                    _y = value;
                else
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }

        public char File => ChessNotation.LegalFileNames[_x];
        public char Rank => ChessNotation.LegalRankNames[_y];

        public override bool Equals(object? obj)
        {
            return obj is BoardSquare other && Equals(other);
        }

        public bool Equals(BoardSquare? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return X == other.X && Y == other.Y;
        }

        public static bool operator ==(BoardSquare? left, BoardSquare? right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(BoardSquare? left, BoardSquare? right)
        {
            return !(left == right);
        }

        public override string ToString() => $"{File}{Rank}";
        public override int GetHashCode() => HashCode.Combine(X, Y);
    }
}
