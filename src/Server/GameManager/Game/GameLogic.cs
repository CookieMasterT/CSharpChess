using CSharpChess.Board;
using CSharpChess.Pieces;

namespace CSharpChess.Game
{
    public class GameLogic : ITeamTurnProvider
    {
        public Team Team => CurrentTurnTeam;

        private Team CurrentTurnTeam { get; set; }

        public ChessBoard ChessBoard => _chessBoard;

        private ChessBoard _chessBoard = null!;

        public static Piece GetStartRowPiece(int index, Team team)
        {
            {
                index %= 8;
                Piece[] pieces = [new Rook(team), new Knight(team), new Bishop(team), new Queen(team), new King(team), new Bishop(team), new Knight(team), new Rook(team)];
                return pieces[index];
            }
        }

        public void SetupBoard()
        {
            _chessBoard = new ChessBoard(this);
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

        public bool TrySwitchTurnTeam(ChessBoard chessBoard)
        {
            if (chessBoard == _chessBoard)
            {
                this.CurrentTurnTeam = this.CurrentTurnTeam == Team.White ? Team.Black : Team.White;
                return true;
            }
            return false;
        }
    }
}
