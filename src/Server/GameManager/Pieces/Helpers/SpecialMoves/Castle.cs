using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess.Pieces.Helpers.SpecialMoves
{
    public class Castle(CastleSide castleSide) : SpecialMoveInfo
    {
        public CastleSide CastleSide { get; private set; } = castleSide;
    }

    public enum CastleSide
    {
        KingSide,
        QueenSide
    }
}
