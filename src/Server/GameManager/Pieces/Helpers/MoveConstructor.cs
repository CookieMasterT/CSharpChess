using CSharpChess.Board;
using System.Collections.ObjectModel;


namespace CSharpChess.Pieces.Helpers
{
    internal class MoveConstructor(Piece moveInitiator, BoardSquare sourceTile, ChessBoard targetBoard)
    {
        private readonly Piece _moveInitiator = moveInitiator;
        private readonly BoardSquare _sourceTile = sourceTile;
        public readonly ChessBoard _targetBoard = targetBoard;
        private readonly Collection<BoardSquare> _moves = [];

        public void LineAdd(int deltaX, int deltaY)
        {
            if (deltaX == 0 && deltaY == 0) throw new InvalidOperationException("You cannot create a line without a direction");

            var x = deltaX;
            var y = deltaY;
            while (TryAdd(x, y) == MoveCheckResult.Can_VacantTile)
            {
                x += deltaX;
                y += deltaY;
            }
        }

        internal enum MoveCheckResult
        {
            Can_VacantTile = 01, // you can move onto the tile, the tile is empty
            Can_WillCapture = 02, // you can move onto the tile, this move will capture an enemy piece

            Cant_NonCapturing = 11, // you can't move onto the tile, blocked by enemy and you can't capture like this
            Cant_OutOfBounds = 12, // you can't move onto the tile, the move would land you outside the board
            Cant_BlockedByFriend = 13, // you can't move onto the tile, occupied by a piece that is on the same team
        }

        public MoveCheckResult TryAdd(int x, int y, bool capturingMove = true, bool mustCapture = false)
        {
            if (!capturingMove && mustCapture)
                throw new InvalidOperationException("A move that must capture cannot be a non-capturing move");

            x += _sourceTile.X;
            y += _sourceTile.Y;
            if (!(x is >= 0 and < ChessBoard.BoardSize && y is >= 0 and < ChessBoard.BoardSize))
                return MoveCheckResult.Cant_OutOfBounds;

            BoardSquare square = _targetBoard[x, y];

            if (square.Content is null && !mustCapture)
            {
                Add(x, y);
                return MoveCheckResult.Can_VacantTile;
            }

            if (square.Content is not null && _moveInitiator.Team == square.Content.Team)
                return MoveCheckResult.Cant_BlockedByFriend;

            if (capturingMove && (!mustCapture || square.Content is not null))
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
            _moves.Add(_targetBoard[x, y]);
        }

        public Collection<BoardSquare> GetMoves()
        {
            return _moves;
        }
    }
}
