using CSharpChess.Board;
using CSharpChess.Game;
using CSharpChess.Pieces.Helpers;
using System.Collections.ObjectModel;

namespace CSharpChess.Pieces
{
    internal sealed class Rook : Piece
    {
        private Rook() : base(Team.White) { }
        public Rook(Team team) : base(team)
        {
        }

        public override string Name { get => ChessNotation.Rook; }

        public override Collection<BoardSquare> GetAvailableTiles(BoardSquare containingSquare, ChessBoard containingBoard, bool onlyAttacks = false)
        {
            var MV = new MoveConstructor(this, containingSquare, containingBoard);

            MV.LineAdd(0, 1); // move in a line north
            MV.LineAdd(1, 0); // move in a line east
            MV.LineAdd(0, -1); // move in a line south
            MV.LineAdd(-1, 0); // move in a line west

            return MV.GetMoves();
        }
    }
}
