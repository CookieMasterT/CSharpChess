using CSharpChess.Board;
using CSharpChess.Game;
using CSharpChess.Pieces.Helpers;

namespace CSharpChess.Pieces
{
    public class Bishop : Piece
    {
        private Bishop() : base(Team.White) { }
        public Bishop(Team team) : base(team)
        {
        }

        public override string Name { get => ChessNotation.Bishop; }

        public override List<BoardSquare> GetAvailableTiles(BoardSquare ContainingSquare, ChessBoard ContainingBoard)
        {
            var MV = new MoveConstructor(this, ContainingSquare, ContainingBoard);

            MV.LineAdd(1, 1); // move in a line north east
            MV.LineAdd(1, -1); // move in a line south east
            MV.LineAdd(-1, -1); // move in a line south west
            MV.LineAdd(-1, 1); // move in a line north west

            return MV.GetMoves();
        }
    }
}
