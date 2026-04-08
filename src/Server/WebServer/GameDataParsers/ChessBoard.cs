using CSharpChess.Game;
using Newtonsoft.Json.Linq;

namespace WebServer.GameDataParsers
{
    internal static class ChessBoard
    {
        public static string GetJson()
        {
            var json = new JObject();
            var board = new JArray();
            for (int x = 0; x < CSharpChess.Board.ChessBoard.BoardSize; x++)
            {
                var rank = new JArray();
                for (int y = 0; y < CSharpChess.Board.ChessBoard.BoardSize; y++)
                {
                    var str = string.Empty;
                    var tile = GameLogic.ChessBoard[x, y];
                    switch (tile.Content?.Team)
                    {
                        case CSharpChess.Game.Team.White:
                            str += ChessNotation.WhiteTeam;
                            break;
                        case CSharpChess.Game.Team.Black:
                            str += ChessNotation.BlackTeam;
                            break;
                    }
                    str += (tile.Content?.Name ?? ChessNotation.EmptySquare);
                    rank.Add(str);
                }
                board.Add(rank);
            }
            json.Add("board", board);
            json.Add("currentTeam", GameLogic.CurrentTurnTeam == Team.White ? "White" : "Black");
            json.Add("moveHistory", new JArray(GameLogic.ChessBoard.MoveHistory));
            return json.ToString();
        }
    }
}
