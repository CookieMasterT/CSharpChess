namespace CSharpChess.Pieces.Helpers.SpecialMoves
{
    internal sealed class Castle(CastleSide castleSide) : SpecialMoveInfo
    {
        public CastleSide CastleSide { get; } = castleSide;
    }

    internal enum CastleSide
    {
        KingSide,
        QueenSide
    }
}
