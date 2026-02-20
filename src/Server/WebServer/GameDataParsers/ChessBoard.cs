using System;
using System.Collections.Generic;
using System.Text;
using CSharpChess;
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
                // todo: ensure that this is actually a rank
                var rank = new JArray();
                for (int y = 0; y < 8; y++)
                {
                    var str = string.Empty;
                    var tile = CSharpChess.ChessBoard.Board[x, y];
                    switch (tile.content?.Team)
                    {
                        case CSharpChess.Team.white:
                            str += ChessNotation.WhiteTeam;
                            break;
                        case CSharpChess.Team.black:
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
