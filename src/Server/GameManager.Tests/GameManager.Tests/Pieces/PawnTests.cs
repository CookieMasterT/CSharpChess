using CSharpChess.Board;
using CSharpChess.Game;
using CSharpChess.Pieces;
using GameManager.Tests.Helpers;

namespace GameManager.Tests.Pieces
{
    [TestClass]
    public class PawnTests
    {
        [TestMethod]
        public void PawnCanDoubleMove()
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.White));
            var pawn = new Pawn(Team.White);

            cb[0, 0].Content = pawn;

            // Act
            ChessBoard.MovePiece(0, 0, 0, 2, cb); // move from start to 2 squares up the y axis
            // Assert
            Assert.AreEqual(pawn, cb[0, 2].Content); // the pawn should be at the new location
        }

        [TestMethod]
        public void PawnJumpOverAnotherPieceWithDoubleMove()
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.White));
            var pawn = new Pawn(Team.White);
            var blockingPiece = new Pawn(Team.White);

            cb[0, 0].Content = pawn;
            cb[0, 1].Content = blockingPiece; // place a piece directly in front of the pawn

            // Act
            var result = ChessBoard.MovePiece(0, 0, 0, 2, cb); // attempt to move from start to 2 squares up the y axis

            // Assert
            Assert.IsFalse(result); // the move should fail because the pawn cannot jump over the blocking piece
        }


        [TestMethod]
        public void PawnCannotDoubleMoveAfterFirstMove()
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.White));
            var pawn = new Pawn(Team.White);

            cb[0, 1].Content = pawn;

            // Act
            ChessBoard.MovePiece(0, 1, 0, 2, cb); // move from start to 1 square up the y axis
            var result = ChessBoard.MovePiece(0, 2, 0, 4, cb); // attempt to move 2 squares up the y axis again

            // Assert
            Assert.IsFalse(result); // the move should fail because the pawn has already moved and cannot double move afterwards
        }

        [TestMethod]
        public void WhitePawnCannotMoveDownward()
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.White));
            var pawn = new Pawn(Team.White);

            cb[0, 1].Content = pawn;

            // Act
            var result = ChessBoard.MovePiece(0, 1, 0, 0, cb); // attempt to move downward

            // Assert
            Assert.IsFalse(result); // the move should fail beacuse white pawns don't move downwards
        }

        [TestMethod]
        public void WhitePawnCanMoveUpwardss()
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.White));
            var pawn = new Pawn(Team.White);

            cb[0, 1].Content = pawn;

            // Act
            var result = ChessBoard.MovePiece(0, 1, 0, 2, cb); // move upward

            // Assert
            Assert.IsTrue(result); // the move should succeed because white pawns move upward
            Assert.AreEqual(pawn, cb[0, 2].Content);
        }

        [TestMethod]
        public void BlackPawnCannotMoveUpwards()
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.Black));
            var pawn = new Pawn(Team.Black);

            cb[0, 6].Content = pawn;
            // Act
            var result = ChessBoard.MovePiece(0, 6, 0, 7, cb); // attempt to move upward

            // Assert
            Assert.IsFalse(result); // the move should fail because black pawns don't move upward
        }

        [TestMethod]
        public void BlackPawnCanMoveDownwards()
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.Black));
            var pawn = new Pawn(Team.Black);

            cb[0, 6].Content = pawn;

            // Act
            var result = ChessBoard.MovePiece(0, 6, 0, 5, cb); // attempt to move downward

            // Assert
            Assert.IsTrue(result); // the move should succeed because black pawns move downward
            Assert.AreEqual(pawn, cb[0, 5].Content);
        }

        [TestMethod]
        public void PawnCannotCaptureWithForwardMove()
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.White));
            var pawn = new Pawn(Team.White);
            var EnemyPiece = new Pawn(Team.Black);

            cb[0, 1].Content = pawn;
            cb[0, 2].Content = EnemyPiece;

            // Act
            var result = ChessBoard.MovePiece(0, 1, 0, 2, cb); // attempt to capture upward

            // Assert
            Assert.IsFalse(result); // the move should fail because pawns cannot capture pieces directly in front of them
        }

        [TestMethod]
        public void PawnCannotMoveDiagonally()
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.White));
            var pawn = new Pawn(Team.White);

            cb[0, 1].Content = pawn;

            // Act
            var result = ChessBoard.MovePiece(0, 1, 1, 2, cb); // attempt to move diagonally

            // Assert
            Assert.IsFalse(result); // the move should fail because pawns cannot move diagonally unless they are capturing a piece
        }

        [TestMethod]
        public void PawnCanCaptureDiagonally()
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.White));
            var pawn = new Pawn(Team.White);
            var EnemyPiece = new Pawn(Team.Black);

            cb[0, 1].Content = pawn;
            cb[1, 2].Content = EnemyPiece;

            // Act
            var result = ChessBoard.MovePiece(0, 1, 1, 2, cb); // attempt to capture diagonally

            // Assert
            Assert.IsTrue(result); // the move should succeed because pawns can capture pieces diagonally
            Assert.AreEqual(pawn, cb[1, 2].Content);
        }

        [TestMethod]
        public void PawnUsesEnPassantProperly()
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.Black));
            var attackingPawn = new Pawn(Team.White);
            var enemyPawn = new Pawn(Team.Black);

            cb[0, 4].Content = attackingPawn;
            cb[1, 6].Content = enemyPawn;

            // Act
            ChessBoard.MovePiece(1, 6, 1, 4, cb); // move black pawn 2 squares upward
            var result = ChessBoard.MovePiece(0, 4, 1, 5, cb); // attempt to en passant capture

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(attackingPawn, cb[1, 5].Content);
            Assert.IsNull(cb[1, 4].Content);
        }

        [TestMethod]
        public void PawnCannotUseEnPassantAfterOneTurn()
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.Black));
            var attackingPawn = new Pawn(Team.White);
            var enemyPawn = new Pawn(Team.Black);

            cb[0, 4].Content = attackingPawn;
            cb[1, 6].Content = enemyPawn;

            // Act
            ChessBoard.MovePiece(1, 6, 1, 4, cb); // move black pawn 2 squares forward
            ChessBoard.PassTurn(cb); // pass turn twice
            ChessBoard.PassTurn(cb);
            var result = ChessBoard.MovePiece(0, 4, 1, 5, cb); // attempt to en passant capture

            // Assert
            Assert.IsFalse(result); // the move should fail because en passant can only be used immediately after the opponent's pawn makes the double move
            Assert.AreEqual(enemyPawn, cb[1, 4].Content);
        }

        [TestMethod]
        public void PawnPromotionWithoutChoiceShouldFail() {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.White));
            var pawn = new Pawn(Team.White);
            cb[0, 6].Content = pawn;
            // Act
            var result = ChessBoard.MovePiece(0, 6, 0, 7, cb); // attempt to move to the promotion rank without specifying a promotion piece
            // Assert
            Assert.IsFalse(result); // the move should fail because a promotion piece must be specified when a pawn reaches the promotion rank
        }

        [TestMethod]
        [DataRow(ChessNotation.Queen, typeof(Queen))]
        [DataRow(ChessNotation.Rook, typeof(Rook))]
        [DataRow(ChessNotation.Bishop, typeof(Bishop))]
        [DataRow(ChessNotation.Knight, typeof(Knight))]
        public void PawnPromotionShouldCreateCorrectPiece(string promotion, Type expectedType)
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.White));
            var pawn = new Pawn(Team.White);
            cb[0, 6].Content = pawn;

            // Act
            ChessBoard.MovePiece(0, 6, 0, 7, cb, promotionPiece: promotion);

            // Assert
            Assert.IsInstanceOfType(cb[0, 7].Content, expectedType);
        }

        [TestMethod]
        [DataRow(ChessNotation.Pawn)]
        [DataRow(ChessNotation.King)]
        public void PawnPromotionWithIllegalPiecesShouldFail(string promotion)
        {
            // Arrange
            var cb = new ChessBoard(new TurnTeamStub(Team.White));
            var pawn = new Pawn(Team.White);
            cb[0, 6].Content = pawn;

            // Act
            var result = ChessBoard.MovePiece(0, 6, 0, 7, cb, promotionPiece: promotion);

            // Assert
            Assert.IsFalse(result); // the move should fail because you cannot promote a pawn to a king or another pawn
        }
    }
}
