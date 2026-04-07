using CSharpChess.Board;
using CSharpChess.Game;
using CSharpChess.Pieces.Helpers;
using CSharpChess.Pieces.Helpers.SpecialMoves;
using System.Collections.ObjectModel;

namespace CSharpChess.Pieces
{
    internal class Pawn : Piece
    {
        private Pawn() : base(Team.White) { }
        public Pawn(Team team) : base(team)
        {
        }

        public override string Name { get => ChessNotation.Pawn; }

        public override Collection<BoardSquare> GetAvailableTiles(BoardSquare containingSquare, ChessBoard containingBoard, bool onlyAttacks = false)
        {
            ArgumentNullException.ThrowIfNull(containingSquare);
            ArgumentNullException.ThrowIfNull(containingBoard);

            var MV = new MoveConstructor(this, containingSquare, containingBoard);
            _specialMoveActions.Clear();
            int direction = (this.Team == Team.White) ? 1 : -1; // Black pawns move down the y axis (-1) instead of up the y axis (+1)

            if (!onlyAttacks)
            {
                // move forward
                bool FirstMoveWorks = MV.TryAdd(0, direction, capturingMove: false) == MoveConstructor.MoveCheckResult.Can_VacantTile;

                // if the pawn hasn't moved yet, they can move 2 forward
                if (FirstMoveWorks && !HasMoved)
                {
                    if (MV.TryAdd(0, 2 * direction, capturingMove: false) == MoveConstructor.MoveCheckResult.Can_VacantTile)
                    {
                        _specialMoveActions.Add((MV.GetMoves().Last(), (containingBoard, movedToTile) =>
                        {
                            (Piece.CurrentBoardLookup(containingBoard, movedToTile)?.Content as Pawn)?._doubleMove = true;
                            return new NormalMove();
                        }
                        ));
                    }
                }
            }

            // capture on diagonals in front of pawn
            MV.TryAdd(1, direction, mustCapture: true);
            MV.TryAdd(-1, direction, mustCapture: true);

            // if there is an adjacent enemy pawn that just did a double move, we can capture it en passant
            BoardSquare? leftSquare = containingBoard.GetSquare(containingSquare.X - 1, containingSquare.Y);
            if (leftSquare?.Content is Pawn pawnL && pawnL._doubleMove && pawnL.Team != this.Team)
            {
                MV.TryAdd(-1, direction);
                _specialMoveActions.Add((MV.GetMoves().Last(), (containingBoard, _) =>
                {
                    Piece.CurrentBoardLookup(containingBoard, leftSquare)?.Content = null;
                    return new EnPassant();
                }
                ));
            }
            BoardSquare? rightSquare = containingBoard.GetSquare(containingSquare.X + 1, containingSquare.Y);
            if (rightSquare?.Content is Pawn pawnR && pawnR._doubleMove && pawnR.Team != this.Team)
            {
                MV.TryAdd(1, direction);
                _specialMoveActions.Add((MV.GetMoves().Last(), (containingBoard, _) =>
                {
                    Piece.CurrentBoardLookup(containingBoard, rightSquare)?.Content = null;
                    return new EnPassant();
                }
                ));
            }
            return MV.GetMoves();
        }
        private bool _doubleMove;
        private readonly List<(BoardSquare, Func<ChessBoard, BoardSquare, SpecialMoveInfo>)> _specialMoveActions = [];
        public override SpecialMoveInfo SpecialMoveCallback(BoardSquare tile, ChessBoard board, string? promotionPiece = null)
        {
            if ((tile?.Y == 0 && Team == Team.Black) || (tile?.Y == 7 && Team == Team.White))
            {
                switch (promotionPiece)
                {
                    case ChessNotation.Queen:
                        tile.Content = new Queen(this.Team);
                        return new Promotion(PromotionPiece.Queen);
                    case ChessNotation.Rook:
                        tile.Content = new Rook(this.Team);
                        return new Promotion(PromotionPiece.Rook);
                    case ChessNotation.Bishop:
                        tile.Content = new Bishop(this.Team);
                        return new Promotion(PromotionPiece.Bishop);
                    case ChessNotation.Knight:
                        tile.Content = new Knight(this.Team);
                        return new Promotion(PromotionPiece.Knight);
                }

            }

            var specialMove = _specialMoveActions.FirstOrDefault(move => move.Item1 == tile);
            if (specialMove != default)
            {
                return specialMove.Item2(board, tile!);
            }
            return new NormalMove();
        }

        public override void TurnStartCallback()
        {
            _doubleMove = false;
            _specialMoveActions.Clear();
        }
    }
}