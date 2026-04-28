using CSharpChess.Board;
using CSharpChess.Game;
using CSharpChess.Pieces;
using GameManager.Tests.Helpers;

namespace GameManager.Tests.Pieces
{
    [TestClass]
    public class KingTests
    {
        [TestMethod]
        [DataRow(-1, -1)]
        [DataRow(0, -1)]
        [DataRow(1, -1)]
        [DataRow(-1, 0)]
        [DataRow(1, 0)]
        [DataRow(-1, 1)]
        [DataRow(0, 1)]
        [DataRow(1, 1)]
        public void KingCanMoveOneSquareInAnyDirection(int dx, int dy)
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.White));
            var king = new King(Team.White);

            cb[4, 4].Content = king; // 4, 4 so that the king has space to move in all directions

            // Act
            var result = ChessBoard.MovePiece(4, 4, 4 + dx, 4 + dy, cb); // move the king

            // Assert
            Assert.IsTrue(result); // the move should be successful beacuse the king can move to any adjacent square
            Assert.AreEqual(king, cb[4 + dx, 4 + dy].Content);
        }

        [TestMethod]
        public void KingCanCastleKingSide()
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.White));
            var king = new King(Team.White);
            var rook = new Rook(Team.White);

            cb[4, 0].Content = king; // place the king and rook on their initial squares
            cb[7, 0].Content = rook;

            // Act
            var result = ChessBoard.MovePiece(4, 0, 6, 0, cb); // attempt to castle kingside

            // Assert
            Assert.IsTrue(result); // the move should be successful because the king can castle
            Assert.AreEqual(king, cb[6, 0].Content); // the king should be on g1
            Assert.AreEqual(rook, cb[5, 0].Content); // the rook should be on f1
        }

        [TestMethod]
        public void KingCanCastleQueenSide()
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.White));
            var king = new King(Team.White);
            var rook = new Rook(Team.White);

            cb[4, 0].Content = king; // place the king and rook on their initial squares
            cb[0, 0].Content = rook;

            // Act
            var result = ChessBoard.MovePiece(4, 0, 2, 0, cb); // attempt to castle queenside

            // Assert
            Assert.IsTrue(result); // the move should be successful because the king can castle
            Assert.AreEqual(king, cb[2, 0].Content); // the king should be on c1
            Assert.AreEqual(rook, cb[3, 0].Content); // the rook should be on d1
        }

        [TestMethod]
        public void KingCannotCastleAfterMovingThemselves()
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.White, true));
            var king = new King(Team.White);
            var rook = new Rook(Team.White);

            cb[4, 0].Content = king; // place the king and rook on their initial squares
            cb[7, 0].Content = rook;

            // Act
            ChessBoard.MovePiece(4, 0, 5, 0, cb); // move the king back and forth
            ChessBoard.MovePiece(5, 0, 4, 0, cb);
            var result = ChessBoard.MovePiece(4, 0, 6, 0, cb); // attempt to castle kingside

            // Assert
            Assert.IsFalse(result); // the move should fail because the king has moved before and cannot castle after moving
        }

        [TestMethod]
        public void KingCannotCastleWithAnMovingRook()
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.White, true));
            var king = new King(Team.White);
            var rook = new Rook(Team.White);

            cb[4, 0].Content = king; // place the king and rook on their initial squares
            cb[7, 0].Content = rook;

            // Act
            ChessBoard.MovePiece(7, 0, 6, 0, cb); // move the rook back and forth
            ChessBoard.MovePiece(6, 0, 7, 0, cb);
            var result = ChessBoard.MovePiece(4, 0, 6, 0, cb); // attempt to castle kingside

            // Assert
            Assert.IsFalse(result); // the move should fail because the rook has moved before and cannot castle after moving
        }

        [TestMethod]
        public void KingCannotCastleWhileInCheck()
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.White, true));
            var king = new King(Team.White);
            var rook = new Rook(Team.White);
            var enemyRook = new Rook(Team.Black);

            cb[4, 0].Content = king; // place the king and rook on their initial squares
            cb[7, 0].Content = rook;
            cb[4, 7].Content = enemyRook; // place an enemy rook so that the king is in check

            // Act
            var result = ChessBoard.MovePiece(4, 0, 6, 0, cb); // attempt to castle kingside

            // Assert
            Assert.IsFalse(result); // the move should fail because castling to get out of check is not allowed
        }

        [TestMethod]
        public void KingCannotCastleThroughCheck()
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.White, true));
            var king = new King(Team.White);
            var rook = new Rook(Team.White);
            var enemyRook = new Rook(Team.Black);

            cb[4, 0].Content = king; // place the king and rook on their initial squares
            cb[7, 0].Content = rook;
            cb[5, 7].Content = enemyRook; // place an enemy rook so that the square the king would pass through is in check

            // Act
            var result = ChessBoard.MovePiece(4, 0, 6, 0, cb); // attempt to castle kingside

            // Assert
            Assert.IsFalse(result); // the move should fail because castling through a square that is in check is not allowed
        }

        [TestMethod]
        public void KingCanCastleWithAnAttackedRook()
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.White, true));
            var king = new King(Team.White);
            var rook = new Rook(Team.White);
            var enemyRook = new Rook(Team.Black);

            cb[4, 0].Content = king; // place the king and rook on their initial squares
            cb[7, 0].Content = rook;
            cb[7, 7].Content = enemyRook; // place an enemy rook so that the rook is attacked

            // Act
            var result = ChessBoard.MovePiece(4, 0, 6, 0, cb); // attempt to castle kingside

            // Assert
            Assert.IsTrue(result); // the move should be successful because castling with an attacked rook is still allowed
        }

        [TestMethod]
        public void KingCannotMoveIntoCheck()
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.White));
            var king = new King(Team.White);
            var enemyRook = new Rook(Team.Black);

            cb[0, 0].Content = king;
            cb[1, 1].Content = enemyRook; // place an enemy rook so that the square the king would move to is in check

            // Act
            var result = ChessBoard.MovePiece(0, 0, 1, 0, cb); // attempt to move the king into check

            // Assert
            Assert.IsFalse(result); // the move should fail because putting your king in danger is not allowed
        }

        [TestMethod]
        public void KingCannotBeRevealChecked()
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.White));
            var king = new King(Team.White);
            var friendlyPiece = new Rook(Team.White);
            var enemyRook = new Rook(Team.Black);

            cb[0, 0].Content = king;
            cb[1, 0].Content = friendlyPiece; // place a friendly piece in front of the king
            cb[2, 0].Content = enemyRook; // place an enemy rook so that if the friendly piece moves out of the way, the king would be put in check

            // Act
            var result = ChessBoard.MovePiece(1, 0, 1, 1, cb); // attempt to move the friendly piece out of the way

            // Assert
            Assert.IsFalse(result); // the move should fail because putting your king in danger is not allowed
        }
    }
}
