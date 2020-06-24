using ChineseChess.Source.GameRule;
using ChineseChess.Source.GameRule.MoveLogic;
using Microsoft.Xna.Framework;
using System;

namespace ChineseChess.Source.Helper
{
    public static class PieceMoveFactory
    {
        public static IMovable CreatePieceMove(int key, Point idx)
        {
            key = Math.Abs(key);
            if (key == (int)Pieces.R_Chariot) return new ChariotMove(idx);
            if (key == (int)Pieces.R_Horse) return new HorseMove(idx);
            if (key == (int)Pieces.R_Advisor) return new AdvisorMove(idx);
            if (key == (int)Pieces.R_Cannon) return new CannonMove(idx);
            if (key == (int)Pieces.R_General) return new GeneralMove(idx);
            if (key == (int)Pieces.R_Soldier) return new SoldierMove(idx);
            if (key == (int)Pieces.R_Elephant) return new ElephantMove(idx);
            throw new ArgumentException($"Create piecemove error: key values {key}");
        }
    }
}
