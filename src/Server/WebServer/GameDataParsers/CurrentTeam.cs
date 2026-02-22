using CSharpChess;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using WebServer.RequestTypes;

namespace WebServer.GameDataParsers
{
    internal class CurrentTeam
    {
        public static string GetJson()
        {
            var json = new JObject();
            json.Add("team", GameLogic.CurrentTurnTeam == Team.White ? "White" : "Black");
            return json.ToString();
        }
    }
}
