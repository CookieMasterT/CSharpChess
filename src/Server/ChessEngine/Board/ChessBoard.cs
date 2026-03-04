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

                return true;
            }
            return false;
        }
        public static bool MovePiece(int start_x, int start_y, int end_x, int end_y)
        {
            foreach (var coord in new int[4] { start_x, start_y, end_x, end_y })
            {
                if (!(coord is >= 0 and < 8))
                    return false;
            }
            return MovePiece(ChessBoard.Board[start_x, start_y], ChessBoard.Board[end_x, end_y]);
        }
    }
}
