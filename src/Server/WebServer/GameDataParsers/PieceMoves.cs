using CSharpChess.Board;
using CSharpChess.Game;
using CSharpChess.Pieces;
using Newtonsoft.Json.Linq;
using WebServer.RequestTypes;

namespace WebServer.GameDataParsers
{
    internal static class PieceMoves
    {
        public static string GetJson(CoordinateInfo position)
        {
            var json = new JArray();
            BoardSquare? tile = Program.GameLogicMain.ChessBoard[position.X, position.Y];
            Piece? piece = tile?.Content;
            if (piece is null || tile is null || piece.Team != Program.GameLogicMain.Team)
                return "{}";
            foreach (var item in piece.GetLegalMoves(tile, Program.GameLogicMain.ChessBoard))
            {
                json.Add(new JArray(new int[] { item.X, item.Y }));
            }
            return json.ToString();
        }
    }
}
