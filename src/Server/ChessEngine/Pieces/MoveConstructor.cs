using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess.Pieces
{
    internal class MoveConstructor
    {
        private static MoveConstructor? _singleton;

        private MoveConstructor() { }
        public static MoveConstructor GetMoveConstructor(Piece MoveInitiator, BoardSquare SourceTile)
        {
            _singleton ??= new MoveConstructor();
            _singleton._moveInitiator = MoveInitiator;
            _singleton._sourceTile = SourceTile;
            _singleton._moves = new List<BoardSquare>();
            return _singleton;
        }

        private BoardSquare _sourceTile;
        private Piece _moveInitiator = null!;
        private List<BoardSquare> _moves = null!;

        public void LineAdd(int deltaX, int deltaY)
        {
            if (deltaX == 0 && deltaY == 0) throw new InvalidOperationException($"You cannot create a line without a direction");

            var x = deltaX;
            var y = deltaY;
            while (TryAdd(x,y) == MoveCheckResult.Can_VacantTile)
            {
                x += deltaX;
                y += deltaY;
            }
        }

        public enum MoveCheckResult
        {
            Can_VacantTile = 0, // you can move onto the tile, the tile is empty
            Can_WillCapture = 0, // you can move onto the tile, this move will capture an enemy piece

            Cant_NonCapturing = 1, // you can't move onto the tile, blocked by enemy and you can't capture like this
            Cant_OutOfBounds = 1, // you can't move onto the tile, the move would land you outside the board
            Cant_BlockedByFriend = 1, // you can't move onto the tile, occupied by a piece that is on the same team

            // todo: implement
            Cant_KingInDanger = 2, // you can't move beacuse your king is in check, and this move does not help with that
            Cant_DangerousToKing = 2 // you can't move beacuse the move would put your king in check 
        }

        public MoveCheckResult TryAdd(int x, int y, bool CapturingMove = true, bool MustCapture = false)
        {
            if (CapturingMove == false && MustCapture == true)
                throw new InvalidOperationException("A move that must capture cannot be a non-capturing move");

            x += _sourceTile.X;
            y += _sourceTile.Y;
            if (!(x is >= 0 and < 8 && y is >= 0 and < 8))
                return MoveCheckResult.Cant_OutOfBounds;

            BoardSquare square = ChessBoard.Board[x, y];

            if (square.content is null && !MustCapture)
            {
                Add(x, y);
                return MoveCheckResult.Can_VacantTile;
            }

            if (!(square.content is null) && _moveInitiator.Team == square.content.Team)
                return MoveCheckResult.Cant_BlockedByFriend;

            if (CapturingMove && (!MustCapture || !(square.content is null)))
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
            _moves.Add(ChessBoard.Board[x, y]);
        }

        public List<BoardSquare> GetMoves()
        {
            return _moves;
        }
    }
}
