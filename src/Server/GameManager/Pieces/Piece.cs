using CSharpChess.Board;
using CSharpChess.Game;
using CSharpChess.Pieces.Helpers.SpecialMoves;

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
            List<BoardSquare> PossibleMoves = GetAvailableTiles(ContainingSquare, ContainingBoard);
            List<BoardSquare> LegalMoves = [];

            foreach (var Move in PossibleMoves)
            {
                // if after doing the move your king will be in danger, then the move is not legal
                ChessBoard tempBoard = FastCloner.FastCloner.DeepClone(ContainingBoard) ?? new();
                ChessBoard.MovePiece(ContainingSquare.X, ContainingSquare.Y, Move.X, Move.Y, tempBoard, true);
                if (ChessBoard.KingInDanger(this.Team, tempBoard))
                {
                    continue;
                }
                LegalMoves.Add(Move);
            }

            return LegalMoves;
        }

        abstract public List<BoardSquare> GetAvailableTiles(BoardSquare ContainingSquare, ChessBoard ContainingBoard);

        public virtual SpecialMoveInfo SpecialMoveCallback(BoardSquare tile, ChessBoard board) { return new NormalMove(); }

        public static BoardSquare? CurrentBoardLookup(ChessBoard ContainingBoard, BoardSquare NeededSquare)
        {
            foreach (var Square in ContainingBoard.Board)
            {
                if (Square == NeededSquare)
                {
                    return Square;
                }
            }
            return null;
        }

        public virtual void TurnStartCallback() { }
    }
}
