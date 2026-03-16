using CSharpChess.Board;
using CSharpChess.Game;

namespace CSharpChess.Pieces
{
    public class Knight : Piece
    {
        private Knight() : base(Team.White) { }
        public Knight(Team team) : base(team)
        {
        }

        public override string Name { get => ChessNotation.Knight; }

        public override List<BoardSquare> GetAvailableTiles(BoardSquare ContainingSquare, ChessBoard ContainingBoard)
        {
            var MV = new MoveConstructor(this, ContainingSquare, ContainingBoard);

            // Jump north
            MV.TryAdd(-1, 2);
            MV.TryAdd(1, 2);

            // Jump east
            MV.TryAdd(2, 1);
            MV.TryAdd(2, -1);

            // Jump south
            MV.TryAdd(1, -2);
            MV.TryAdd(-1, -2);

            // Jump west
            MV.TryAdd(-2, -1);
            MV.TryAdd(-2, 1);

            return MV.GetMoves();
        }
    }
}
