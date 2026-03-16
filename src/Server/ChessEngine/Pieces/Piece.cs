using CSharpChess.Board;
using CSharpChess.Game;

namespace CSharpChess.Pieces
{
    public abstract class Piece
    {
        private Piece() { }
        public Piece(Team team)
        {
            Team = team;
        }

        public virtual string Name { get => ChessNotation.PiecePlaceHolder; }

        public bool hasMoved = false;

        public readonly Team Team;

        public List<BoardSquare> GetLegalMoves(BoardSquare ContainingSquare, ChessBoard ContainingBoard)
        {
            return GetAvailableTiles(ContainingSquare, ContainingBoard);
        }

        abstract public List<BoardSquare> GetAvailableTiles(BoardSquare ContainingSquare, ChessBoard ContainingBoard);

        public virtual void SpecialMoveCallback(BoardSquare tile) { }

        public virtual void TurnStartCallback() { }
    }
}
