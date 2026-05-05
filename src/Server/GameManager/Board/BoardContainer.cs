using System;
using System.Collections.Generic;
using System.Text;
using CSharpChess.Pieces;

namespace CSharpChess.Board
{
    /// <summary>
    /// BoardContainer defined as the 8x8 field of board squares.
    /// This class is used to seperate storing the board from the ChessBoard logic, which allows for state history.
    /// This will never be used by anything other than the ChessBoard class.
    /// </summary>
    internal class BoardContainer : IEquatable<BoardContainer>
    {
        private readonly int _boardSize;

        public BoardContainer(int size)
        {
            _boardSize = size;

            _board = new BoardSquare[_boardSize][];
            for (int i = 0; i < _boardSize; i++)
            {
                _board[i] = new BoardSquare[_boardSize];
                for (int k = 0; k < _boardSize; k++)
                {
                    _board[i][k] = new BoardSquare(i, k);
                }
            }
        }

        public BoardSquare? this[int x, int y]
        {
            get
            {
                if (x is >= 0 && x < _boardSize && y is >= 0 && y < _boardSize)
                    return _board[x][y];
                return null;
            }
        }

        private readonly BoardSquare[][] _board;

        public override bool Equals(object? obj)
        {
            return obj is BoardContainer other && Equals(other);
        }

        public bool Equals(BoardContainer? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (_boardSize != other._boardSize) return false;

            // Compare all squares: piece type, color, and move state (for castling and en passant).
            for (int x = 0; x < _boardSize; x++)
            {
                for (int y = 0; y < _boardSize; y++)
                {
                    var thisSquare = _board[x][y];
                    var otherSquare = other._board[x][y];

                    var thisPiece = thisSquare?.Content;
                    var otherPiece = otherSquare?.Content;

                    if ((thisPiece is null) != (otherPiece is null))
                        return false;

                    // If both the squares are empty, continue to next square.
                    if (thisPiece is null)
                        continue;

                    if (thisPiece.GetType() != otherPiece!.GetType() || thisPiece.Team != otherPiece.Team)
                        return false;

                    // Compare HasMoved, but only for relevant pieces, king and rooks, needed for castling rights.
                    if (thisPiece.HasMoved != otherPiece.HasMoved && (thisPiece is King || thisPiece is Rook))
                        return false;

                    // Compare en passant state, only for pawns.
                    if (thisPiece is Pawn && (thisPiece as Pawn)?.DoubleMove != (otherPiece as Pawn)?.DoubleMove)
                        return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(_boardSize);

            // Hash the board content based on what Equals() compares
            for (int x = 0; x < _boardSize; x++)
            {
                for (int y = 0; y < _boardSize; y++)
                {
                    var piece = _board[x][y]?.Content;

                    if (piece is not null)
                    {
                        hash.Add(piece.GetType());
                        hash.Add(piece.Team);

                        // Hash castling rights (HasMoved for King and Rook)
                        if (piece is King || piece is Rook)
                        {
                            hash.Add(piece.HasMoved);
                        }

                        // Hash en passant state (DoubleMove for Pawn)
                        if (piece is Pawn pawn)
                        {
                            hash.Add(pawn.DoubleMove);
                        }
                    }
                    else
                    {
                        // Add a marker for empty squares to distinguish from pieces
                        hash.Add(0);
                    }
                }
            }

            return hash.ToHashCode();
        }
    }
}
