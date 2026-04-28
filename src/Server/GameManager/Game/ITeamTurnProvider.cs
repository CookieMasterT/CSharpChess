using CSharpChess.Board;

namespace CSharpChess.Game
{
    public enum Team { White, Black }

    public interface ITeamTurnProvider
    {
        public Team Team { get; }

        public bool TrySwitchTurnTeam(ChessBoard chessBoard);
    }
}
