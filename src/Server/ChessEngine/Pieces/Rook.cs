using CSharpChess.Board;
using CSharpChess.Game;

namespace CSharpChess.Pieces
{
    public class Rook : Piece
    {
        private Rook() : base(Team.White) { }
        public Rook(Team team) : base(team)
        {
        }

        public override string Name { get => ChessNotation.Rook; }

        public override List<BoardSquare> GetLegalMoves(BoardSquare ContainingSquare)
        {
            var MV = new MoveConstructor(this, ContainingSquare);

            MV.LineAdd(0, 1); // move in a line north
            MV.LineAdd(1, 0); // move in a line east
            MV.LineAdd(0, -1); // move in a line south
            MV.LineAdd(-1, 0); // move in a line west

            return MV.GetMoves();
        }
    }
}
