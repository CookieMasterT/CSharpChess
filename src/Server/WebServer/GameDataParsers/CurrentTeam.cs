using CSharpChess.Game;
using Newtonsoft.Json.Linq;

namespace WebServer.GameDataParsers
{
    internal class CurrentTeam
    {
        public static string GetJson()
        {
            var json = new JObject
            {
                { "team", GameLogic.CurrentTurnTeam == Team.White ? "White" : "Black" }
            };
            return json.ToString();
        }
    }
}
