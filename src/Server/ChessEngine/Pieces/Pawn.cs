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

        public override string Name { get => ChessNotation.Pawn; }

        public override List<BoardSquare> GetLegalMoves(BoardSquare ContainingSquare)
        {
            var MV = MoveConstructor.GetMoveConstructor(this, ContainingSquare);
            int direction = (this.Team == Team.White) ? 1 : -1; // Black pawns move down the y axis (-1) instead of up the y axis (+1)

            // move forward
            bool FirstMoveWorks = MV.TryAdd(0, direction, CapturingMove: false) == MoveConstructor.MoveCheckResult.Can_VacantTile;

            // if the pawn hasn't moved yet, they can move 2 forward
            if (FirstMoveWorks && !hasMoved)
                MV.TryAdd(0, 2 * direction, CapturingMove: false);

            // capture on diagonals in front of pawn
            MV.TryAdd(1, direction, MustCapture: true);
            MV.TryAdd(-1, direction, MustCapture: true);
            return MV.GetMoves();
        }
    }
}
