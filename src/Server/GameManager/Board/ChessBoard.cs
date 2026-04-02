using CSharpChess.Game;
using CSharpChess.Pieces;
using CSharpChess.Pieces.Helpers.SpecialMoves;

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
        public List<string> MoveHistory = [];

        public BoardSquare? GetSquare(int x, int y)
        {
            if (x is >= 0 and < 8 && y is >= 0 and < 8)
                return _board[x, y];
            return null;
        }

        public static bool IsSquareAttacked(BoardSquare square, Team byTeam, ChessBoard targetBoard)
        {
            foreach (var tile in targetBoard.Board)
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

        public static bool KingInDanger(Team team, ChessBoard targetBoard)
        {
            foreach (var tile in targetBoard.Board)
            {
                if (tile?.content is King king && king.Team == team)
                {
                    if (IsSquareAttacked(tile, team == Team.White ? Team.Black : Team.White, targetBoard))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool HasLegalMoves(Team team, ChessBoard targetBoard)
        {
            foreach (var tile in targetBoard.Board)
            {
                if (tile.content is not null && tile.content.Team == team)
                {
                    if (tile.content.GetLegalMoves(tile, targetBoard).Count > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool MovePiece(int startX, int startY, int endX, int endY, ChessBoard targetBoard, bool IgnoreLegality = false)
        {
            foreach (var coord in new int[4] { startX, startY, endX, endY })
            {
                if (!(coord is >= 0 and < 8))
                    return false;
            }
            return MovePiece(targetBoard.Board[startX, startY], targetBoard.Board[endX, endY], targetBoard, IgnoreLegality);
        }

        public static bool MovePiece(BoardSquare start, BoardSquare end, ChessBoard targetBoard, bool IgnoreLegality = false)
        {
            if (start.content is null)
                return false;
            if ((IgnoreLegality || start.content.GetLegalMoves(start, targetBoard).Contains(end)) && start.content.Team == GameLogic.CurrentTurnTeam)
            {
                start.content.hasMoved = true;

                Team CurrentTeam = GameLogic.CurrentTurnTeam == Team.White ? Team.Black : Team.White;
                if (GameLogic.ChessBoard == targetBoard)
                {
                    GameLogic.CurrentTurnTeam = CurrentTeam;
                }

                bool wasCapturing = false;
                if (end.content is not null)
                    wasCapturing = true;

                end.content = start.content;
                start.content = null;

                var MoveType = end.content.SpecialMoveCallback(end, targetBoard);

                foreach (var tile in targetBoard.Board)
                {
                    if (tile.content is not null && tile.content.Team == CurrentTeam)
                    {
                        tile.content.TurnStartCallback();
                    }
                }

                if (MoveType is EnPassant)
                {
                    wasCapturing = true;
                }

                if (MoveType is Castle)
                {
                    if (MoveType is Castle { CastleSide: CastleSide.KingSide })
                    {
                        targetBoard.MoveHistory.Add(ChessNotation.KingCastle);
                    }
                    else
                    {
                        targetBoard.MoveHistory.Add(ChessNotation.QueenCastle);
                    }
                }
                else
                    targetBoard.MoveHistory.Add(ChessNotation.CreateNotation(end.content, end, start, wasCapturing, HasLegalMoves(CurrentTeam, targetBoard), KingInDanger(CurrentTeam, targetBoard)));

                return true;
            }
            return false;
        }
    }
}
