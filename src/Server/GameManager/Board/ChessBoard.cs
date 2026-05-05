using CSharpChess.Game;
using CSharpChess.Pieces;
using System.Collections.ObjectModel;
using System.Security.Cryptography;

namespace CSharpChess.Board
{
    public enum GameState { Ongoing, WhiteWins, BlackWins, Tie }

    /// <summary>
    /// Chessboard defined as an instance of a game.
    /// Contains BoardContainer - the board state, the move history and all other game state info.
    /// Actions performed on the chessboard are done by the class with the specific instance injected into the functions.
    /// </summary>
    public class ChessBoard(ITeamTurnProvider turnProvider)
    {
        public const int BoardSize = 8;
        private readonly BoardContainer _board = new(BoardSize);

        private readonly ITeamTurnProvider _turnProvider = turnProvider;

        public GameState CurrentGameState => _currentGameState;

        private GameState _currentGameState = GameState.Ongoing;

        public BoardSquare? this[int x, int y]
        {
            get
            {
                if (x is >= 0 and < BoardSize && y is >= 0 and < BoardSize)
                    return _board[x, y];
                return null;
            }
        }

        public Collection<string> MoveHistory { get; } = [];
        private Collection<BoardContainer> BoardHistory { get; } = [];

        public BoardSquare? GetSquare(int x, int y)
        {
            if (x is >= 0 and < BoardSize && y is >= 0 and < BoardSize)
                return _board[x, y];
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
                    if (tile?.Content is not null && tile.Content.Team == byTeam)
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
                    if (tile?.Content is not null && tile.Content.Team == team)
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
            var start = targetBoard[startX, startY];
            var end = targetBoard[endX, endY];

            if (start is null || end is null)
            {
                throw new InvalidOperationException("The provided coordinates are out of bounds, you cannot move a piece from / to outside the board.");
            }

            return MovePiece(start, end, targetBoard, ignoreLegality, promotionPiece);
        }

        public static bool MovePiece(BoardSquare start, BoardSquare end, ChessBoard targetBoard, bool ignoreLegality = false, string? promotionPiece = null)
        {
            ArgumentNullException.ThrowIfNull(start);
            ArgumentNullException.ThrowIfNull(end);
            ArgumentNullException.ThrowIfNull(targetBoard);

            if (targetBoard._currentGameState != GameState.Ongoing)
                return false;
            if (start.Content is null)
                return false;
            Collection<string> legalPromotions = [ChessNotation.Queen, ChessNotation.Rook, ChessNotation.Bishop, ChessNotation.Knight];
            if ((start.Content is Pawn) && (((end.Y == 0 && start.Content.Team == Team.Black) || (end.Y == 7 && start.Content.Team == Team.White)) && (promotionPiece is null || !legalPromotions.Contains(promotionPiece))))
                return false;
            if ((ignoreLegality || start.Content.GetLegalMoves(start, targetBoard).Contains(end)) && start.Content.Team == targetBoard._turnProvider.Team)
            {
                // Before the first move, put the initial board state in the history, for the purposes of threefold move repetition detection.
                if (targetBoard.BoardHistory.Count == 0)
                    targetBoard.BoardHistory.Add(FastCloner.FastCloner.DeepClone(targetBoard._board) ?? throw new InvalidOperationException("Failed to clone the board. (Is FastCloner NuGet package installed?)"));

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
                        if (tile?.Content is not null && tile.Content.Team == CurrentTeam)
                        {
                            tile.Content.TurnStartCallback();
                        }
                    }
                }

                if (!ignoreLegality)
                {
                    targetBoard.MoveHistory.Add(ChessNotation.CreateNotation(end.Content, end, start, wasCapturing, HasLegalMoves(CurrentTeam, targetBoard), KingInDanger(CurrentTeam, targetBoard), MoveType));
                    targetBoard.BoardHistory.Add(FastCloner.FastCloner.DeepClone(targetBoard._board) ?? throw new InvalidOperationException("Failed to clone the board. (Is FastCloner NuGet package installed?)"));

                    UpdateGameState(targetBoard, wasCapturing, end.Content is Pawn);
                }
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
                    if (tile?.Content is not null && tile.Content.Team == CurrentTeam)
                    {
                        tile.Content.TurnStartCallback();
                    }
                }
            }
            targetBoard.MoveHistory.Add("-||-");
        }

        private int _fiftyMoveCounter; // if this is -1, it means that the current turn did have a capture or a pawn move.

        private static void UpdateGameState(ChessBoard targetBoard, bool wasCapturing, bool isPawnMove)
        {
            ArgumentNullException.ThrowIfNull(targetBoard);

            var currentTeam = targetBoard._turnProvider.Team == Team.White ? Team.Black : Team.White;

            // If the current team is in check and has no legal moves, they lose. if they are not in check but have no legal moves, it's a stalemate.
            if (KingInDanger(currentTeam, targetBoard) && !HasLegalMoves(currentTeam, targetBoard))
            {
                targetBoard._currentGameState = currentTeam == Team.White ? GameState.BlackWins : GameState.WhiteWins;
            }
            else if (!KingInDanger(currentTeam, targetBoard) && !HasLegalMoves(currentTeam, targetBoard))
            {
                targetBoard._currentGameState = GameState.Tie;
            }

            // If the move was a capture or a pawn move, reset the fifty-move counter.
            if (wasCapturing || isPawnMove)
            {
                // -1 makes it so that the next increment will set it to 0
                targetBoard._fiftyMoveCounter = -1;
            }

            // Increment the fifty-move counter. if it reaches 50, it's a tie.
            if (currentTeam == Team.Black)
            {
                targetBoard._fiftyMoveCounter++;
                if (targetBoard._fiftyMoveCounter >= 50)
                {
                    targetBoard._currentGameState = GameState.Tie;
                }
            }

            // Check the current board state history for threefold repetition.
            Dictionary<BoardContainer, int> whiteBoardStates = [];
            Dictionary<BoardContainer, int> blackBoardStates = [];

            for (int i = 0; i < targetBoard.BoardHistory.Count; i++)
            {
                var states = (i % 2 == 0) ? whiteBoardStates : blackBoardStates;
                var key = targetBoard.BoardHistory[i];

                states.TryGetValue(key, out int currentCount);
                int newCount = currentCount + 1;

                states[key] = newCount;

                if (newCount == 3)
                {
                    targetBoard._currentGameState = GameState.Tie;
                }
            }
        }
    }
}
