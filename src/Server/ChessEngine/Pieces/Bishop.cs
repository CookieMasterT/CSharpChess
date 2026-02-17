using CSharpChess.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess
{
    public class Bishop : Piece
    {
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
