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
                    _chessBoard[x, y].Content = null;
                }
            }

            for (int x = 0; x < ChessBoard.BoardSize; x++)
            {
                // white pieces
                _chessBoard[x, 0].Content = GetStartRowPiece(x, Team.White);

                // white pawns
                _chessBoard[x, 1].Content = new Pawn(Team.White);

                // black pawns
                _chessBoard[x, 6].Content = new Pawn(Team.Black);

                // black pieces
                _chessBoard[x, 7].Content = GetStartRowPiece(x, Team.Black);
            }
            CurrentTurnTeam = Team.White;
        }
    }
}
