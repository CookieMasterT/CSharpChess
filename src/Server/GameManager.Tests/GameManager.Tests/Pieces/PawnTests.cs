using CSharpChess.Board;
using CSharpChess.Game;
using CSharpChess.Pieces;

namespace GameManager.Tests.Pieces
{
    [TestClass]
    public class PawnTests
    {
        [TestMethod]
        public void PawnCanDoubleMove()
        {
            // Arrange
            var cb = new ChessBoard();
            var pawn = new Pawn(Team.White);
            cb[0, 0].Content = pawn;
            // Act
            ChessBoard.MovePiece(0,0,0,2,cb); // move from start to 2 squares up the y axis
            // Assert
            Assert.AreEqual(pawn, cb[0, 2].Content);
        }

        [TestMethod]
        public void PawnCannotDoubleMoveAfterFirstMove()
        {
            // Arrange
            var cb = new ChessBoard();
            var pawn = new Pawn(Team.White);
            cb[0, 0].Content = pawn;
            // Act
            ChessBoard.MovePiece(0, 0, 0, 1, cb); // move from start to 1 square up the y axis
            var result = ChessBoard.MovePiece(0, 1, 0, 3, cb); // attempt to move from 1 square up to 3 squares up
            // Assert
            Assert.IsFalse(result);
        }

        public void PawnCannotMoveBackward()
        {
            // Arrange
            var cb = new ChessBoard();
            var pawn = new Pawn(Team.White);
            cb[0, 1].Content = pawn;
            // Act
            var result = ChessBoard.MovePiece(0, 1, 0, 0, cb); // attempt to move backward
            // Assert
            Assert.IsFalse(result);
        }

        public void BlackPawnCanMoveBackwards()
        {
            // Arrange
            var cb = new ChessBoard();
            var pawn = new Pawn(Team.Black);
            cb[0, 6].Content = pawn;
            // Act
            var result = ChessBoard.MovePiece(0, 6, 0, 7, cb); // attempt to move backward
            // Assert
            Assert.AreEqual(pawn, cb[0, 7].Content);
        }
    }
}
