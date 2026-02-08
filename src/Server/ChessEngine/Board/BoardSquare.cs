namespace CSharpChess
{
    public struct BoardSquare : IEquatable<BoardSquare>
    {
        private int _x; // The current file indexed from 0
        private int _y; // The current rank indexed from 0

        public Piece? content;
        public string ContentStr
        {
            get
            {
                if (content != null)
                {
                    return content.FullName;
                }
                return ChessNotation.EmptySquare;
            }
        }

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

        public char File => ChessNotation.LegalFileNames[_x];  // Display as file letters a..h
        public char Rank => ChessNotation.LegalRankNames[_y]; // Display as rank numbers 1..8

        public bool Equals(BoardSquare other) => this.X == other.X && this.Y == other.Y;
        public override string ToString() => $"{File}{Rank}";
        public override int GetHashCode() => HashCode.Combine(X, Y);
    }
}
