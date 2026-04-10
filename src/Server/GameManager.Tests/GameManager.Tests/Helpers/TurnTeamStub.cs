using CSharpChess.Board;
using CSharpChess.Game;

namespace GameManager.Tests.Helpers
{
    internal sealed class TurnTeamStub(Team team) : ITeamTurnProvider
    {
        public Team Team => _team;

        private Team _team = team;

        public bool TrySwitchTurnTeam(ChessBoard chessBoard)
        {
            _team = _team == Team.White ? Team.Black : Team.White;
            return true;
        }
    }
}
