using CSharpChess.Board;
using CSharpChess.Game;

namespace CSharpChess.Pieces
{
    public class Bishop : Piece
    {
        private Bishop() : base(Team.White) { }
        public Bishop(Team team) : base(team)
        {
        }

        public override string Name { get => ChessNotation.Bishop; }

        public override List<BoardSquare> GetLegalMoves(BoardSquare ContainingSquare)
        {
            var MV = MoveConstructor.GetMoveConstructor(this, ContainingSquare);

            MV.LineAdd(1, 1); // move in a line north east
            MV.LineAdd(1, -1); // move in a line south east
            MV.LineAdd(-1, -1); // move in a line south west
            MV.LineAdd(-1, 1); // move in a line north west

            return MV.GetMoves();
        }
    }
}
