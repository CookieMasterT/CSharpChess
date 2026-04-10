using CSharpChess.Board;
using CSharpChess.Game;
using CSharpChess.Pieces.Helpers;
using System.Collections.ObjectModel;

namespace CSharpChess.Pieces
{
    public class Knight : Piece
    {
        private Knight() : base(Team.White) { }
        public Knight(Team team) : base(team)
        {
        }

        public override string Name { get => ChessNotation.Knight; }

        public override Collection<BoardSquare> GetAvailableTiles(BoardSquare containingSquare, ChessBoard containingBoard, bool onlyAttacks = false)
        {
            var MV = new MoveConstructor(this, containingSquare, containingBoard);

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
