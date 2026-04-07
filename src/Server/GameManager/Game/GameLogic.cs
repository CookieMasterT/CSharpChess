using CSharpChess.Board;
using CSharpChess.Pieces;

namespace CSharpChess.Game
{
    public enum Team { White, Black }

    public static class GameLogic
    {
        public static Team CurrentTurnTeam { get; set; }

        public static ChessBoard ChessBoard => _chessBoard;

        private static ChessBoard _chessBoard = null!;

        public static void SwapSquareStruct(ref BoardSquare originalSquare, Piece? newContent)
        {
            if (originalSquare is null)
                return;
            originalSquare.Content = newContent;
        }

        public static Piece GetStartRowPiece(int index, Team team)
        {
            {
                index %= 8;
                Piece[] pieces = [new Rook(team), new Knight(team), new Bishop(team), new Queen(team), new King(team), new Bishop(team), new Knight(team), new Rook(team)];
                return pieces[index];
            }
        }

        public static void SetupBoard()
        {
            _chessBoard = new ChessBoard();
            for (int x = 0; x < ChessBoard.BoardSize; x++)
            {
                for (int y = 0; y < ChessBoard.BoardSize; y++)
                {
                    SwapSquareStruct(ref ChessBoard.Board[x][y], null);
                }
            }

            for (int x = 0; x < ChessBoard.BoardSize; x++)
            {
                // white pieces
                SwapSquareStruct(ref ChessBoard.Board[x][0], GetStartRowPiece(x, Team.White));

                // white pawns
                SwapSquareStruct(ref ChessBoard.Board[x][1], new Pawn(Team.White));

                // black pawns
                SwapSquareStruct(ref ChessBoard.Board[x][6], new Pawn(Team.Black));

                // black pieces
                SwapSquareStruct(ref ChessBoard.Board[x][7], GetStartRowPiece(x, Team.Black));
            }
            CurrentTurnTeam = Team.White;
        }
    }
}
