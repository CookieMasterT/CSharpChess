using System;
using System.Collections.Generic;
using System.Text;
using CSharpChess;
using Newtonsoft.Json.Linq;

namespace WebServer.GameDataParsers
{
    internal static class ChessBoard
    {
        public static string json { 
            get 
            {
                var json = new JObject();
                for (int x = 0; x < 8; x++)
                {
                    // todo: ensure that this is actually a rank
                    var rank = new JArray();
                    for (int y = 0; y < 8; y++)
                    {
                        var str = string.Empty;
                        var tile = CSharpChess.ChessBoard.Board[x, y];
                        str += tile.content?.Team;
                        str += (tile.content?.Name ?? "N");
                        rank.Add(str);
                    }
                    json.Add(ChessNotation.LegalRankNames[x].ToString(), rank);
                }
                return json.ToString();
            } 
        }
    }
}
