using CSharpChess.Board;
using CSharpChess.Game;
using CSharpChess.Pieces.Helpers;
using CSharpChess.Pieces.Helpers.SpecialMoves;
using System.Collections.ObjectModel;

namespace CSharpChess.Pieces
{
    internal sealed class King : Piece
    {
        private King() : base(Team.White) { }
        public King(Team team) : base(team)
        {
        }

        public override string Name { get => ChessNotation.King; }

        public override Collection<BoardSquare> GetAvailableTiles(BoardSquare containingSquare, ChessBoard containingBoard, bool onlyAttacks = false)
        {
            ArgumentNullException.ThrowIfNull(containingSquare);
            ArgumentNullException.ThrowIfNull(containingBoard);

            var MV = new MoveConstructor(this, containingSquare, containingBoard);
            _specialMoveActions.Clear();

            // king moves in a 3x3 on top of itself (any adjacent square)
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    MV.TryAdd(i, j);
                }
            }

            // check both sides for a rook that hasn't moved, and if the tiles between them are empty, add the castle move
            if (!onlyAttacks)
            {
                if (!HasMoved && !ChessBoard.KingInDanger(this.Team, containingBoard))
                {
                    int[] directions = [-1, 1];
                    foreach (var dir in directions)
                    {
                        int X = containingSquare.X;
                        int Y = containingSquare.Y;
                        List<BoardSquare> tilesToCheck = [];
                        while (true)
                        {
                            X += dir;
                            var currentSquare = containingBoard.GetSquare(X, Y);
                            if (currentSquare != null)
                            {
                                if (currentSquare.Content is Rook rook && !rook.HasMoved)
                                {
                                    if (CheckTilesForCastle(tilesToCheck[..1], this.Team, containingBoard))
                                    {
                                        var castleMoveSquare = tilesToCheck[1];
                                        MV.TryAdd(dir * 2, 0);
                                        _specialMoveActions.Add((castleMoveSquare, (containingBoard) =>
                                        {
                                            currentSquare = Piece.CurrentBoardLookup(containingBoard, currentSquare);
                                            containingBoard.GetSquare(castleMoveSquare.X - dir, castleMoveSquare.Y)?.Content = currentSquare?.Content;
                                            currentSquare?.Content = null;

                                            if (dir == -1)
                                                return new Castle(CastleSide.QueenSide);
                                            else
                                                return new Castle(CastleSide.KingSide);
                                        }
                                        ));
                                    }
                                    break;
                                }
                                tilesToCheck.Add(currentSquare);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return MV.GetMoves();
        }

        private static bool CheckTilesForCastle(List<BoardSquare> tiles, Team team, ChessBoard containingBoard)
        {
            foreach (var tile in tiles)
            {
                if (tile.Content is not null || ChessBoard.IsSquareAttacked(tile, team == Team.White ? Team.Black : Team.White, containingBoard))
                {
                    return false;
                }
            }
            return true;
        }

        private readonly List<(BoardSquare, Func<ChessBoard, SpecialMoveInfo>)> _specialMoveActions = [];
        public override SpecialMoveInfo SpecialMoveCallback(BoardSquare tile, ChessBoard board, string? promotionPiece = null)
        {
            var specialMove = _specialMoveActions.FirstOrDefault(move => move.Item1 == tile);
            if (specialMove != default)
            {
                return specialMove.Item2(board);
            }
            return new NormalMove();
        }
    }
}
