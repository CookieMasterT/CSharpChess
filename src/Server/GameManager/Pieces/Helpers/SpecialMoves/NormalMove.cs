namespace CSharpChess.Pieces.Helpers.SpecialMoves
{
    /// <summary>
    /// This class is used to signify that a move is a normal move with no special properties.
    /// It is used as the default return value for the SpecialMoveCallback method.
    /// </summary>
    internal sealed class NormalMove() : SpecialMoveInfo
    {
    }
}
