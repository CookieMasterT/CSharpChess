using CSharpChess.Board;
using CSharpChess.Game;
using CSharpChess.Pieces;
using GameManager.Tests.Helpers;
using System.Collections.ObjectModel;

namespace GameManager.Tests.Pieces;

[TestClass]
public class RookBishopQueenTests
{
    [TestMethod]
    [DataRow(typeof(Queen))]
    [DataRow(typeof(Rook))]
    public void CardinalMoversCanMove(Type pieceType)
    {
        // Arrange
        var cb = new ChessBoard(new TurnTeamStub(Team.White));
        Piece? piece = (Piece?)Activator.CreateInstance(pieceType, Team.White);
        cb[0, 0].Content = piece;

        // Act
        var result = ChessBoard.MovePiece(cb[0, 0], cb[0, 7], cb); // Move horizontally across the board

        // Assert
        Assert.IsTrue(result); // The move should be successful
        Assert.AreEqual(piece, cb[0, 7].Content);
    }

    [TestMethod]
    [DataRow(typeof(Queen))]
    [DataRow(typeof(Rook))]
    public void CardinalMoversCanCapture(Type pieceType)
    {
        // Arrange
        var cb = new ChessBoard(new TurnTeamStub(Team.White));
        Piece? piece = (Piece?)Activator.CreateInstance(pieceType, Team.White);
        var enemyPiece = new Pawn(Team.Black);

        cb[0, 0].Content = piece;
        cb[0, 7].Content = enemyPiece;

        // Act
        var result = ChessBoard.MovePiece(cb[0, 0], cb[0, 7], cb); // Move horizontally across the board to capture

        // Assert
        Assert.IsTrue(result); // The move should be successful
        Assert.AreEqual(piece, cb[0, 7].Content);
    }

    [TestMethod]
    [DataRow(typeof(Queen))]
    [DataRow(typeof(Rook))]
    public void CardinalMoversCannotCaptureThroughPieces(Type pieceType)
    {
        // Arrange
        var cb = new ChessBoard(new TurnTeamStub(Team.White));
        Piece? piece = (Piece?)Activator.CreateInstance(pieceType, Team.White);
        var enemyPiece = new Pawn(Team.Black);
        var blockingPiece = new Pawn(Team.Black);

        cb[0, 0].Content = piece;
        cb[0, 6].Content = blockingPiece;
        cb[0, 7].Content = enemyPiece;

        // Act
        var result = ChessBoard.MovePiece(cb[0, 0], cb[0, 7], cb); // Attempt to move horizontally across the board to capture

        // Assert
        Assert.IsFalse(result); // The move should fail because the path is blocked by another piece
        Assert.AreEqual(enemyPiece, cb[0, 7].Content); // The enemy piece should still be there
    }

    [TestMethod]
    [DataRow(typeof(Queen))]
    [DataRow(typeof(Bishop))]
    public void DiagonalMoversCanMove(Type pieceType)
    {
        // Arrange
        var cb = new ChessBoard(new TurnTeamStub(Team.White));
        Piece? piece = (Piece?)Activator.CreateInstance(pieceType, Team.White);
        cb[0, 0].Content = piece;

        // Act
        var result = ChessBoard.MovePiece(cb[0, 0], cb[7, 7], cb); // Move diagonally across the board

        // Assert
        Assert.IsTrue(result); // The move should be successful
        Assert.AreEqual(piece, cb[7, 7].Content);
    }

    [TestMethod]
    [DataRow(typeof(Queen))]
    [DataRow(typeof(Bishop))]
    public void DiagonalMoversCanCapture(Type pieceType)
    {
        // Arrange
        var cb = new ChessBoard(new TurnTeamStub(Team.White));
        Piece? piece = (Piece?)Activator.CreateInstance(pieceType, Team.White);
        var enemyPiece = new Pawn(Team.Black);

        cb[0, 0].Content = piece;
        cb[7, 7].Content = enemyPiece;

        // Act
        var result = ChessBoard.MovePiece(cb[0, 0], cb[7, 7], cb); // Move diagonally across the board to capture

        // Assert
        Assert.IsTrue(result); // The move should be successful
        Assert.AreEqual(piece, cb[7, 7].Content);
    }

    [TestMethod]
    [DataRow(typeof(Queen))]
    [DataRow(typeof(Bishop))]
    public void DiagonalMoversCannotCaptureThroughPieces(Type pieceType)
    {
        // Arrange
        var cb = new ChessBoard(new TurnTeamStub(Team.White));
        Piece? piece = (Piece?)Activator.CreateInstance(pieceType, Team.White);
        var enemyPiece = new Pawn(Team.Black);
        var blockingPiece = new Pawn(Team.Black);
        cb[0, 0].Content = piece;
        cb[6, 6].Content = blockingPiece;
        cb[7, 7].Content = enemyPiece;
        // Act
        var result = ChessBoard.MovePiece(cb[0, 0], cb[7, 7], cb); // Attempt to move diagonally across the board to capture
        // Assert
        Assert.IsFalse(result); // The move should fail because the path is blocked by another piece
        Assert.AreEqual(enemyPiece, cb[7, 7].Content); // The enemy piece should still be there
    }

    [TestMethod]
    [DataRow(typeof(Queen))]
    [DataRow(typeof(Rook))]
    [DataRow(typeof(Bishop))]
    public void LongMoversCannotJumpOverOtherPieces(Type pieceType)
    {
        // Arrange
        var cb = new ChessBoard(new TurnTeamStub(Team.White));
        Piece? piece = (Piece?)Activator.CreateInstance(pieceType, Team.White);
        cb[3, 3].Content = piece;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue; // Skip the piece's own square
                cb[3 + i, 3 + j].Content = new Pawn(Team.White); // Place pawns around the piece
            }
        }

        if (piece is null)
            throw new InvalidOperationException("Failed to create piece instance.");

        // Act
        Collection<BoardSquare> legalMoves = piece.GetLegalMoves(cb[3, 3], cb);

        // Assert
        Assert.IsEmpty(legalMoves); // The piece should not have any legal moves, because it is obstructed from all sides
    }
}
