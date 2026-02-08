using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess
{
    public class ChessNotation
    {
        public static readonly char[] LegalRankNames = ['1', '2', '3', '4', '5', '6', '7', '8'];
        public static readonly char[] LegalFileNames = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];

        // the first value is the notation Text, the second value will be used when fetching the ImageFileName.

        public static readonly string[] King = ["K", "king"];
        public static readonly string[] Queen = ["Q", "queen"];
        public static readonly string[] Rook = ["R", "rook"];
        public static readonly string[] Bishop = ["B", "bishop"];
        public static readonly string[] Knight = ["N", "knight"];
        public static readonly string[] Pawn = ["", "pawn"];

        public static readonly string[] PiecePlaceHolder = ["P", "unknown"];

        public const string WhiteTeam = "w";
        public const string BlackTeam = "b";

        public const string EmptySquare = "N";
    }
}
