using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess
{
    public enum Team { white, black }

    public static class GameLogic
    {
        public static Team CurrentTurn;

        public static void SwapSquareStruct(ref BoardSquare originalSquare, Piece? newContent)
        {
            originalSquare.content = newContent;
        }

        public static Piece GetStartRowPiece(int index, Team team)
        {
            {
                Piece[] pieces = new Piece[]
                {new Rook(team), new Knight(team), new Bishop(team), new Queen(team), new King(team), new Bishop(team), new Knight(team), new Rook(team)};
                return pieces[index];
            }
        }

        public static void SetupBoard()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    SwapSquareStruct(ref ChessBoard.Board[x, y], null);
                }
            }

            for (int x = 0; x < 8; x++)
            {
                // white pieces
                SwapSquareStruct(ref ChessBoard.Board[x, 0], GetStartRowPiece(x, Team.white));

                // white pawns
                SwapSquareStruct(ref ChessBoard.Board[x, 1], new Pawn(Team.white));

                // black pawns
                SwapSquareStruct(ref ChessBoard.Board[x, 6], new Pawn(Team.black));

                // black pieces
                SwapSquareStruct(ref ChessBoard.Board[x, 7], GetStartRowPiece(x, Team.black));
            }
            CurrentTurn = Team.white;
        }
    }
}
