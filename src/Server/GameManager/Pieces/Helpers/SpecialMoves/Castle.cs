namespace CSharpChess.Pieces.Helpers.SpecialMoves
{
    internal class Castle(CastleSide castleSide) : SpecialMoveInfo
    {
        public CastleSide CastleSide { get; } = castleSide;
    }

    internal enum CastleSide
    {
        KingSide,
        QueenSide
    }
}
