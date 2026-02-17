using CSharpChess.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess
{
    public class Knight : Piece
    {
        public Knight(Team team) : base(team)
        {
        }

        public override string Name { get => ChessNotation.Knight; }

        public override List<BoardSquare> GetLegalMoves(BoardSquare ContainingSquare)
        {
            var MV = MoveConstructor.GetMoveConstructor(this, ContainingSquare);

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
