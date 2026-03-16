using CSharpChess.Game;
using Newtonsoft.Json.Linq;

namespace WebServer.GameDataParsers
{
    internal static class ChessBoard
    {
        public static string GetJson()
        {
            var json = new JArray();
            for (int x = 0; x < 8; x++)
            {
                var rank = new JArray();
                for (int y = 0; y < 8; y++)
                {
                    var str = string.Empty;
                    var tile = GameLogic.ChessBoard.Board[x, y];
                    switch (tile.content?.Team)
                    {
                        case CSharpChess.Game.Team.White:
                            str += ChessNotation.WhiteTeam;
                            break;
                        case CSharpChess.Game.Team.Black:
                            str += ChessNotation.BlackTeam;
                            break;
                    }
                    str += (tile.content?.Name ?? ChessNotation.EmptySquare);
                    rank.Add(str);
                }
                json.Add(rank);
            }
            return json.ToString();
        }
    }
}
