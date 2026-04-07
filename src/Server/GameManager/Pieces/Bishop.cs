using CSharpChess.Board;
using CSharpChess.Game;
using CSharpChess.Pieces.Helpers;
using System.Collections.ObjectModel;

namespace CSharpChess.Pieces
{
    internal class Bishop : Piece
    {
        private Bishop() : base(Team.White) { }
        public Bishop(Team team) : base(team)
        {
        }

        public override string Name { get => ChessNotation.Bishop; }

        public override Collection<BoardSquare> GetAvailableTiles(BoardSquare containingSquare, ChessBoard containingBoard, bool onlyAttacks = false)
        {
            var MV = new MoveConstructor(this, containingSquare, containingBoard);

            MV.LineAdd(1, 1); // move in a line north east
            MV.LineAdd(1, -1); // move in a line south east
            MV.LineAdd(-1, -1); // move in a line south west
            MV.LineAdd(-1, 1); // move in a line north west

            return MV.GetMoves();
        }
    }
}
