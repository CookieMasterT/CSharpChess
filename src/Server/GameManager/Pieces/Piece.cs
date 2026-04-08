using CSharpChess.Board;
using CSharpChess.Game;
using CSharpChess.Pieces.Helpers.SpecialMoves;
using System.Collections.ObjectModel;

namespace CSharpChess.Pieces
{
    public abstract class Piece
    {
        private Piece() { }
        protected Piece(Team team)
        {
            Team = team;
        }

        public virtual string Name { get => ChessNotation.PiecePlaceHolder; }

        public bool HasMoved { get; set; }

        public Team Team { get; }

        public Collection<BoardSquare> GetLegalMoves(BoardSquare containingSquare, ChessBoard containingBoard)
        {
            ArgumentNullException.ThrowIfNull(containingSquare);

            Collection<BoardSquare> PossibleMoves = GetAvailableTiles(containingSquare, containingBoard);
            Collection<BoardSquare> LegalMoves = [];

            foreach (var Move in PossibleMoves)
            {
                // if after doing the move your king will be in danger, then the move is not legal
                ChessBoard tempBoard = FastCloner.FastCloner.DeepClone(containingBoard) ?? new();
                ChessBoard.MovePiece(containingSquare.X, containingSquare.Y, Move.X, Move.Y, tempBoard, true);
                if (ChessBoard.KingInDanger(this.Team, tempBoard))
                {
                    continue;
                }
                LegalMoves.Add(Move);
            }

            return LegalMoves;
        }

        abstract public Collection<BoardSquare> GetAvailableTiles(BoardSquare containingSquare, ChessBoard containingBoard, bool onlyAttacks = false);

        public virtual SpecialMoveInfo SpecialMoveCallback(BoardSquare tile, ChessBoard board, string? promotionPiece = null) { return new NormalMove(); }

        public static BoardSquare? CurrentBoardLookup(ChessBoard containingBoard, BoardSquare neededSquare)
        {
            ArgumentNullException.ThrowIfNull(containingBoard);

            for (int x = 0; x < ChessBoard.BoardSize; x++)
            {
                for (int y = 0; y < ChessBoard.BoardSize; y++)
                {
                    var Square = containingBoard[x, y];
                    if (Square == neededSquare)
                    {
                        return Square;
                    }
                }
            }
            return null;
        }

        public virtual void TurnStartCallback() { }
    }
}
