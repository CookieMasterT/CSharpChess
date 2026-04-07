using CSharpChess.Board;
using CSharpChess.Pieces;
using CSharpChess.Pieces.Helpers.SpecialMoves;

namespace CSharpChess.Game
{
    public static class ChessNotation
    {
        // IMPORTANT: Some of the variables here are used to name data sent over to the client, so changing them may break client functionality if not updated on the client as well.

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

        public const string Promotion = "=";

        public static string CreateNotation(Piece movingPiece, BoardSquare destination, BoardSquare departure, bool capturingMove, bool opponentHasLegalMoves, bool opponentKingInDanger, SpecialMoveInfo extras)
        {
            ArgumentNullException.ThrowIfNull(movingPiece);
            ArgumentNullException.ThrowIfNull(departure);

            // If the move is a castle, return the appropriate castle notation
            if (extras is Castle)
            {
                if (extras is Castle { CastleSide: CastleSide.KingSide })
                {
                    return ChessNotation.KingCastle;
                }
                else
                {
                    return ChessNotation.QueenCastle;
                }
            }

            // If the move is a promotion, then the piece being moved is technically still a pawn.
            if (extras is Promotion)
            {
                movingPiece = new Pawn(movingPiece.Team);
            }

            string notation = string.Empty;

            // The piece name via single letter or empty string for pawn
            notation += movingPiece.Name;

            // If the move is a pawn move and it's a capture, identify the pawn by its departure file
            if (movingPiece is Pawn && capturingMove)
            {
                notation += departure.File;
            }

            // Add capture symbol if it's a capturing move
            notation += capturingMove || extras is EnPassant ? Capture : "";

            // Add destination square in algebraic notation (e.g., e4)
            notation += destination;

            // If the move includes a promotion, add the promoted piece type
            if (extras is Promotion)
            {
                notation += Promotion;
                switch (extras)
                {
                    case Promotion { PromotionPiece: PromotionPiece.Queen }:
                        notation += Queen;
                        break;

                    case Promotion { PromotionPiece: PromotionPiece.Rook }:
                        notation += Rook;
                        break;

                    case Promotion { PromotionPiece: PromotionPiece.Bishop }:
                        notation += Bishop;
                        break;

                    case Promotion { PromotionPiece: PromotionPiece.Knight }:
                        notation += Knight;
                        break;
                }
            }

            // Add check or checkmate symbol if applicable
            if (opponentKingInDanger && !opponentHasLegalMoves)
            {
                notation += Checkmate;
            }
            else if (opponentKingInDanger)
            {
                notation += Check;
            }

            return notation;
        }
    }
}
