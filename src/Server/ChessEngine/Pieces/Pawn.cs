using CSharpChess.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess
{
    public class Pawn : Piece
    {
        public Pawn(Team team) : base(team)
        {
        }

        public override string[] Name { get => ChessNotation.Pawn; }

        public override List<BoardSquare> GetLegalMoves(BoardSquare ContainingSquare)
        {
            var MV = MoveConstructor.GetMoveConstructor(this, ContainingSquare);
            int direction = (this.Team == Team.white) ? 1 : -1; // Black pawns move down the y axis (-1) instead of up the y axis (+1)

            // move forward
            MV.TryAdd(0, direction, false);

            // if the pawn hasn't moved yet, they can move 2 forward
            if (!hasMoved)
                MV.TryAdd(1, 2 * direction, false);

            // capture on diagonals in front of pawn
            MV.TryAdd(1, direction);
            MV.TryAdd(-1, direction);
            return MV.GetMoves();
        }
    }
}
