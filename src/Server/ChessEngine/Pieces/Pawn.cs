using CSharpChess.Board;
using CSharpChess.Game;

namespace CSharpChess.Pieces
{
    public class Pawn : Piece
    {
        private Pawn() : base(Team.White) { }
        public Pawn(Team team) : base(team)
        {
        }

        public override string Name { get => ChessNotation.Pawn; }

        public override List<BoardSquare> GetAvailableTiles(BoardSquare ContainingSquare, ChessBoard ContainingBoard)
        {
            var MV = new MoveConstructor(this, ContainingSquare, ContainingBoard);
            int direction = (this.Team == Team.White) ? 1 : -1; // Black pawns move down the y axis (-1) instead of up the y axis (+1)

            // move forward
            bool FirstMoveWorks = MV.TryAdd(0, direction, CapturingMove: false) == MoveConstructor.MoveCheckResult.Can_VacantTile;

            // if the pawn hasn't moved yet, they can move 2 forward
            if (FirstMoveWorks && !hasMoved)
                if (MV.TryAdd(0, 2 * direction, CapturingMove: false) == MoveConstructor.MoveCheckResult.Can_VacantTile)
                {
                    SpecialMoveActions.Add((MV.GetMoves().Last(), () => { doubleMove = true; }));
                }

            // capture on diagonals in front of pawn
            MV.TryAdd(1, direction, MustCapture: true);
            MV.TryAdd(-1, direction, MustCapture: true);

            // if there is an adjacent enemy pawn that just did a double move, we can capture it en passant
            BoardSquare? leftSquare = ContainingBoard.GetSquare(ContainingSquare.X - 1, ContainingSquare.Y);
            if (leftSquare is not null && leftSquare.content is Pawn pawnL && pawnL.doubleMove && pawnL.Team != this.Team)
            {
                MV.TryAdd(-1, direction);
                SpecialMoveActions.Add((MV.GetMoves().Last(), () => { leftSquare.content = null; }));
            }
            BoardSquare? rightSquare = ContainingBoard.GetSquare(ContainingSquare.X + 1, ContainingSquare.Y);
            if (rightSquare is not null && rightSquare.content is Pawn pawnR && pawnR.doubleMove && pawnR.Team != this.Team)
            {
                MV.TryAdd(1, direction);
                SpecialMoveActions.Add((MV.GetMoves().Last(), () => { rightSquare.content = null; }));
            }
            return MV.GetMoves();
        }
        private bool doubleMove = false;
        private readonly List<(BoardSquare, Action)> SpecialMoveActions = [];
        public override void SpecialMoveCallback(BoardSquare tile)
        {
            var specialMove = SpecialMoveActions.FirstOrDefault(move => move.Item1 == tile);
            if (specialMove != default)
            {
                specialMove.Item2();
            }
        }

        public override void TurnStartCallback()
        {
            doubleMove = false;
            SpecialMoveActions.Clear();
        }
    }
}