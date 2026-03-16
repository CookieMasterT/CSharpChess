using CSharpChess.Game;

namespace CSharpChess.Board
{
    public class ChessBoard
    {
        public ChessBoard()
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
        BoardSquare[,] _board;
        public BoardSquare[,] Board { get => _board; private set => _board = value; }

        public BoardSquare? GetSquare(int x, int y)
        {
            if (x is >= 0 and < 8 && y is >= 0 and < 8)
                return _board[x, y];
            return null;
        }

        public bool IsSquareAttacked(BoardSquare square, Team byTeam, ChessBoard targetBoard)
        {
            foreach (var tile in _board)
            {
                if (tile.content is not null && tile.content.Team == byTeam)
                {
                    if (tile.content.GetAvailableTiles(tile, targetBoard).Contains(square))
                    {
                        return true;
                    }
                }
            }
            return false;
        } 

        public static bool MovePiece(int startX, int startY, int endX, int endY, ChessBoard targetBoard)
        {
            foreach (var coord in new int[4] { startX, startY, endX, endY })
            {
                if (!(coord is >= 0 and < 8))
                    return false;
            }
            return MovePiece(targetBoard.Board[startX, startY], targetBoard.Board[endX, endY], targetBoard);
        }

        public static bool MovePiece(BoardSquare start, BoardSquare end, ChessBoard targetBoard)
        {
            if (start.content is null)
                return false;
            if (start.content.GetLegalMoves(start, targetBoard).Contains(end) && start.content.Team == GameLogic.CurrentTurnTeam)
            {
                start.content.hasMoved = true;

                GameLogic.CurrentTurnTeam = GameLogic.CurrentTurnTeam == Team.White ? Team.Black : Team.White;

                end.content = start.content;
                start.content = null;
                end.content.SpecialMoveCallback(end);

                foreach (var tile in targetBoard.Board)
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
    }
}
