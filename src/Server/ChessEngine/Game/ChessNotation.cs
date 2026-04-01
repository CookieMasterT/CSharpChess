using CSharpChess.Board;
using CSharpChess.Pieces;

namespace CSharpChess.Game
{
    public class ChessNotation
    {
        public static readonly char[] LegalRankNames = ['1', '2', '3', '4', '5', '6', '7', '8'];
        public static readonly char[] LegalFileNames = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];

        public const string King = "K";
        public const string Queen = "Q";
        public const string Rook = "R";
        public const string Bishop = "B";
        public const string Knight = "N";
        public const string Pawn = "";

        public const string PiecePlaceHolder = "P";

        public const string WhiteTeam = "w";
        public const string BlackTeam = "b";

        public const string EmptySquare = "U";

        public const string KingCastle = "O-O";
        public const string QueenCastle = "O-O-O";

        public const string Capture = "x";
        public const string Check = "+";
        public const string Checkmate = "#";

        public static string CreateNotation(Piece movingPiece, BoardSquare destination, bool capturingMove)
        {
            return $"{movingPiece.Name}{(capturingMove ? Capture : "")}{destination}";
        }
    }
}
