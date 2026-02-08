using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess.Constants
{
    internal class ChessNotation
    {
        public static readonly char[] LegalRankNames = ['1', '2', '3', '4', '5', '6', '7', '8'];
        public static readonly char[] LegalFileNames = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];

        public const string King = "K";
        public const string Queen = "Q";
        public const string Rook = "R";
        public const string Bishop = "B";
        public const string Knight = "N";

        public const string PiecePlaceHolder = "P";

        public const string WhiteTeam = "W";
        public const string BlackTeam = "B";
    }
}
