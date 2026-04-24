using CSharpChess.Board;
using CSharpChess.Game;

namespace GameManager.Tests.Helpers
{
    internal sealed class TurnTeamStub(Team team, bool ignoreTeamSwitching = false) : ITeamTurnProvider
    {
        public Team Team => _team;
        private Team _team = team;

        public bool IgnoreTeamSwitching { get; } = ignoreTeamSwitching;

        public bool TrySwitchTurnTeam(ChessBoard chessBoard)
        {
            if (IgnoreTeamSwitching)
                return false;

            _team = _team == Team.White ? Team.Black : Team.White;
            return true;
        }
    }
}
