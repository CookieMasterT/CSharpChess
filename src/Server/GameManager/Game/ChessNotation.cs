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

        public static string CreateNotation(Piece movingPiece, BoardSquare destination, BoardSquare departure, bool capturingMove, bool opponentHasLegalMoves, bool kingInDanger)
        {
            string notation = string.Empty;

            // The piece name via single letter or empty string for pawn
            notation += movingPiece.Name;

            // If the move is a pawn move and it's a capture, identify the pawn by its departure file
            if (movingPiece is Pawn && capturingMove)
            {
                notation += departure.File;
            }

            // Add capture symbol if it's a capturing move
            notation += capturingMove ? Capture : "";

            // Add destination square in algebraic notation (e.g., e4)
            notation += destination;

            // Add check or checkmate symbol if applicable
            if (kingInDanger && !opponentHasLegalMoves)
            {
                notation += Checkmate;
            }
            else if (kingInDanger)
            {
                notation += Check;
            }

            return notation;
        }
    }
}
