using CSharpChess.Board;
using CSharpChess.Game;
using CSharpChess.Pieces.Helpers;
using CSharpChess.Pieces.Helpers.SpecialMoves;

namespace CSharpChess.Pieces
{
    public class King : Piece
    {
        private King() : base(Team.White) { }
        public King(Team team) : base(team)
        {
        }

        public override string Name { get => ChessNotation.King; }

        public override List<BoardSquare> GetAvailableTiles(BoardSquare ContainingSquare, ChessBoard ContainingBoard)
        {
            var MV = new MoveConstructor(this, ContainingSquare, ContainingBoard);

            // king moves in a 3x3 on top of itself (any adjacent square)
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    MV.TryAdd(i, j);
                }
            }

            // check both sides for a rook that hasn't moved, and if the tiles between them are empty, add the castle move
            if (!hasMoved)
            {
                int[] directions = [-1, 1];
                foreach (var dir in directions)
                {
                    int X = ContainingSquare.X;
                    int Y = ContainingSquare.Y;
                    List<BoardSquare> tilesToCheck = [];
                    while (true)
                    {
                        X += dir;
                        var currentSquare = ContainingBoard.GetSquare(X, Y);
                        if (currentSquare != null)
                        {

                            if (currentSquare.content is Rook rook && !rook.hasMoved)
                            {
                                if (CheckTilesForCastle(tilesToCheck))
                                {
                                    var castleMoveSquare = tilesToCheck[1];
                                    MV.TryAdd(dir * 2, 0);
                                    SpecialMoveActions.Add((castleMoveSquare, (ContainingBoard) =>
                                    {
                                        currentSquare = Piece.CurrentBoardLookup(ContainingBoard, currentSquare);
                                        ContainingBoard.GetSquare(castleMoveSquare.X - dir, castleMoveSquare.Y)?.content = currentSquare?.content;
                                        currentSquare?.content = null;

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
                            break;
                    }
                }
            }

            return MV.GetMoves();
        }

        private static bool CheckTilesForCastle(List<BoardSquare> tiles)
        {
            foreach (var tile in tiles)
            {
                if (tile.content is not null)
                {
                    return false;
                }
            }
            return true;
        }

        private readonly List<(BoardSquare, Func<ChessBoard, SpecialMoveInfo>)> SpecialMoveActions = [];
        public override SpecialMoveInfo SpecialMoveCallback(BoardSquare tile, ChessBoard board)
        {
            var specialMove = SpecialMoveActions.FirstOrDefault(move => move.Item1 == tile);
            if (specialMove != default)
            {
                return specialMove.Item2(board);
            }
            return new NormalMove();
        }
    }
}
