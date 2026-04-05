using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpChess.Pieces.Helpers.SpecialMoves
{
    internal class Promotion(PromotionPiece promotionPiece) : SpecialMoveInfo
    {
        public PromotionPiece PromotionPiece { get; private set; } = promotionPiece;
    }
    
    enum PromotionPiece
    {
        Queen,
        Rook,
        Bishop,
        Knight
    }
}
