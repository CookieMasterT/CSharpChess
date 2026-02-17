using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess
{
    public class ChessNotation
    {
        public static readonly char[] LegalRankNames = ['1', '2', '3', '4', '5', '6', '7', '8'];
        public static readonly char[] LegalFileNames = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];

        public static readonly string King = "K";
        public static readonly string Queen = "Q";
        public static readonly string Rook = "R";
        public static readonly string Bishop = "B";
        public static readonly string Knight = "N";
        public static readonly string Pawn = "";

        public static readonly string PiecePlaceHolder = "P";

        public const string WhiteTeam = "w";
        public const string BlackTeam = "b";

        public const string EmptySquare = "N";
    }
}
