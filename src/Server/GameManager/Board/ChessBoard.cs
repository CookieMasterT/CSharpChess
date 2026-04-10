using CSharpChess.Game;
using CSharpChess.Pieces;
using System.Collections.ObjectModel;

namespace CSharpChess.Board
{
    public class ChessBoard
    {
        public const int BoardSize = 8;

        public ChessBoard(ITeamTurnProvider turnProvider)
        {
            _board = new BoardSquare[BoardSize][];
            for (int i = 0; i < BoardSize; i++)
            {
                _board[i] = new BoardSquare[BoardSize];
                for (int k = 0; k < BoardSize; k++)
                {
                    _board[i][k] = new BoardSquare(i, k);
                }
            }

            _turnProvider = turnProvider;
        }
        private readonly BoardSquare[][] _board;

        private readonly ITeamTurnProvider _turnProvider;

        public BoardSquare this[int x, int y]
        {
            get => _board[x][y];
        }

        public Collection<string> MoveHistory { get; } = [];

        public BoardSquare? GetSquare(int x, int y)
        {
            if (x is >= 0 and < BoardSize && y is >= 0 and < BoardSize)
                return _board[x][y];
            return null;
        }

        public static bool IsSquareAttacked(BoardSquare square, Team byTeam, ChessBoard targetBoard)
        {
            ArgumentNullException.ThrowIfNull(square);
            ArgumentNullException.ThrowIfNull(targetBoard);

            for (int x = 0; x < BoardSize; x++)
            {
                for (int y = 0; y < BoardSize; y++)
                {
                    var tile = targetBoard[x, y];
                    if (tile.Content is not null && tile.Content.Team == byTeam)
                    {
                        if (tile.Content.GetAvailableTiles(tile, targetBoard, true).Contains(square))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool KingInDanger(Team team, ChessBoard targetBoard)
        {
            ArgumentNullException.ThrowIfNull(targetBoard);

            for (int x = 0; x < BoardSize; x++)
            {
                for (int y = 0; y < BoardSize; y++)
                {
                    var tile = targetBoard[x, y];
                    if (tile?.Content is King king && king.Team == team)
                    {
                        if (IsSquareAttacked(tile, team == Team.White ? Team.Black : Team.White, targetBoard))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool HasLegalMoves(Team team, ChessBoard targetBoard)
        {
            ArgumentNullException.ThrowIfNull(targetBoard);

            for (int x = 0; x < BoardSize; x++)
            {
                for (int y = 0; y < BoardSize; y++)
                {
                    var tile = targetBoard[x, y];
                    if (tile.Content is not null && tile.Content.Team == team)
                    {
                        if (tile.Content.GetLegalMoves(tile, targetBoard).Count > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool MovePiece(int startX, int startY, int endX, int endY, ChessBoard targetBoard, bool ignoreLegality = false, string? promotionPiece = null)
        {
            ArgumentNullException.ThrowIfNull(targetBoard);

            foreach (var coord in new int[4] { startX, startY, endX, endY })
            {
                if (!(coord is >= 0 and < BoardSize))
                    return false;
            }
            return MovePiece(targetBoard[startX, startY], targetBoard[endX, endY], targetBoard, ignoreLegality, promotionPiece);
        }

        public static bool MovePiece(BoardSquare start, BoardSquare end, ChessBoard targetBoard, bool ignoreLegality = false, string? promotionPiece = null)
        {
            ArgumentNullException.ThrowIfNull(start);
            ArgumentNullException.ThrowIfNull(end);
            ArgumentNullException.ThrowIfNull(targetBoard);

            if (start.Content is null)
                return false;
            if ((ignoreLegality || start.Content.GetLegalMoves(start, targetBoard).Contains(end)) && start.Content.Team == targetBoard._turnProvider.Team)
            {
                start.Content.HasMoved = true;

                Team CurrentTeam = targetBoard._turnProvider.Team == Team.White ? Team.Black : Team.White;
                targetBoard._turnProvider.TrySwitchTurnTeam(targetBoard);

                bool wasCapturing = false;
                if (end.Content is not null)
                    wasCapturing = true;

                end.Content = start.Content;
                start.Content = null;

                var MoveType = end.Content.SpecialMoveCallback(end, targetBoard, promotionPiece);

                for (int x = 0; x < BoardSize; x++)
                {
                    for (int y = 0; y < BoardSize; y++)
                    {
                        var tile = targetBoard[x, y];
                        if (tile.Content is not null && tile.Content.Team == CurrentTeam)
                        {
                            tile.Content.TurnStartCallback();
                        }
                    }
                }

                if (!ignoreLegality)
                    targetBoard.MoveHistory.Add(ChessNotation.CreateNotation(end.Content, end, start, wasCapturing, HasLegalMoves(CurrentTeam, targetBoard), KingInDanger(CurrentTeam, targetBoard), MoveType));

                return true;
            }
            return false;
        }

        public static void PassTurn(ChessBoard targetBoard)
        {
            ArgumentNullException.ThrowIfNull(targetBoard);

            Team CurrentTeam = targetBoard._turnProvider.Team == Team.White ? Team.Black : Team.White;
            targetBoard._turnProvider.TrySwitchTurnTeam(targetBoard);

            for (int x = 0; x < BoardSize; x++)
            {
                for (int y = 0; y < BoardSize; y++)
                {
                    var tile = targetBoard[x, y];
                    if (tile.Content is not null && tile.Content.Team == CurrentTeam)
                    {
                        tile.Content.TurnStartCallback();
                    }
                }
            }
            targetBoard.MoveHistory.Add("-||-");
        }
    }
}
