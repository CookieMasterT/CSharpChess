using CSharpChess.Game;

namespace CSharpChess.Board
{
    public static class ChessBoard
    {
        static ChessBoard()
        {
            _board = new BoardSquare[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int k = 0; k < 8; k++)
                {
                    _board[i, k] = new BoardSquare(i, k);
                }
            }
        }
        static BoardSquare[,] _board;
        public static BoardSquare[,] Board { get => _board; private set => _board = value; }

        public static BoardSquare? GetSquare(int x, int y)
        {
            if (x is >= 0 and < 8 && y is >= 0 and < 8)
                return _board[x, y];
            return null;
        }

        public static bool IsSquareAttacked(BoardSquare square, Team byTeam)
        {
            foreach (var tile in _board)
            {
                if (tile.content is not null && tile.content.Team == byTeam)
                {
                    if (tile.content.GetLegalMoves(tile).Contains(square))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool MovePiece(BoardSquare start, BoardSquare end)
        {
            if (start.content is null)
                return false;
            if (start.content.GetLegalMoves(start).Contains(end) && start.content.Team == GameLogic.CurrentTurnTeam)
            {
                start.content.hasMoved = true;

                GameLogic.CurrentTurnTeam = GameLogic.CurrentTurnTeam == Team.White ? Team.Black : Team.White;

                end.content = start.content;
                start.content = null;
                end.content.SpecialMoveCallback(end);

                foreach (var tile in _board)
                {
                    if (tile.content is not null && tile.content.Team == GameLogic.CurrentTurnTeam)
                    {
                        tile.content.TurnStartCallback();
                    }
                }

                return true;
            }
            return false;
        }
        public static bool MovePiece(int startX, int startY, int endX, int endY)
        {
            foreach (var coord in new int[4] { startX, startY, endX, endY })
            {
                if (!(coord is >= 0 and < 8))
                    return false;
            }
            return MovePiece(ChessBoard.Board[startX, startY], ChessBoard.Board[endX, endY]);
        }
    }
}
