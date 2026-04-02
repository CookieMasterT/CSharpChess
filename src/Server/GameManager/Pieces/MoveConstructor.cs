using CSharpChess.Board;

namespace CSharpChess.Pieces
{
    internal class MoveConstructor(Piece MoveInitiator, BoardSquare SourceTile, ChessBoard TargetBoard)
    {
        private readonly Piece _moveInitiator = MoveInitiator;
        private readonly BoardSquare _sourceTile = SourceTile;
        public readonly ChessBoard _targetBoard = TargetBoard;
        private readonly List<BoardSquare> _moves = [];

        public void LineAdd(int deltaX, int deltaY)
        {
            if (deltaX == 0 && deltaY == 0) throw new InvalidOperationException($"You cannot create a line without a direction");

            var x = deltaX;
            var y = deltaY;
            while (TryAdd(x, y) == MoveCheckResult.Can_VacantTile)
            {
                x += deltaX;
                y += deltaY;
            }
        }

        public enum MoveCheckResult
        {
            Can_VacantTile = 01, // you can move onto the tile, the tile is empty
            Can_WillCapture = 02, // you can move onto the tile, this move will capture an enemy piece

            Cant_NonCapturing = 11, // you can't move onto the tile, blocked by enemy and you can't capture like this
            Cant_OutOfBounds = 12, // you can't move onto the tile, the move would land you outside the board
            Cant_BlockedByFriend = 13, // you can't move onto the tile, occupied by a piece that is on the same team
        }

        public MoveCheckResult TryAdd(int x, int y, bool CapturingMove = true, bool MustCapture = false)
        {
            if (CapturingMove == false && MustCapture == true)
                throw new InvalidOperationException("A move that must capture cannot be a non-capturing move");

            x += _sourceTile.X;
            y += _sourceTile.Y;
            if (!(x is >= 0 and < 8 && y is >= 0 and < 8))
                return MoveCheckResult.Cant_OutOfBounds;

            BoardSquare square = _targetBoard.Board[x, y];

            if (square.content is null && !MustCapture)
            {
                Add(x, y);
                return MoveCheckResult.Can_VacantTile;
            }

            if (square.content is not null && _moveInitiator.Team == square.content.Team)
                return MoveCheckResult.Cant_BlockedByFriend;

            if (CapturingMove && (!MustCapture || square.content is not null))
            {
                Add(x, y);
                return MoveCheckResult.Can_WillCapture;
            }
            else
            {
                return MoveCheckResult.Cant_NonCapturing;
            }
        }

        private void Add(int x, int y)
        {
            _moves.Add(_targetBoard.Board[x, y]);
        }

        public List<BoardSquare> GetMoves()
        {
            return _moves;
        }
    }
}
