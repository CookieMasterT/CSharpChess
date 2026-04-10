using CSharpChess.Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess.Game
{
    public enum Team { White, Black }

    public interface ITeamTurnProvider
    {
        public Team Team { get; }

        public bool TrySwitchTurnTeam(ChessBoard chessBoard);
    }
}
