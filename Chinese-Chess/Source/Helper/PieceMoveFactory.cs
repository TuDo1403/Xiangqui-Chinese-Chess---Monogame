using ChineseChess.Source.AI.MoveLogic;
using ChineseChess.Source.GameRule;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.Helper
{
    public static class PieceMoveFactory
    {
        public static IMovable CreatePiece(int key, Point idx)
        {
            key = Math.Abs(key);
            if (key == (int)Pieces.R_Chariot)
                return new ChariotMove(idx);
            if (key == (int)Pieces.R_Horse)
                return new HorseMove(idx);
            if (key == (int)Pieces.R_Advisor)
                return new AdvisorMove(idx);
            if (key == (int)Pieces.R_Cannon)
                return new CannonMove(idx);
            if (key == (int)Pieces.R_General)
                return new GeneralMove(idx);
            if (key == (int)Pieces.R_Soldier)
                return new SoldierMove(idx);
            if (key == (int)Pieces.R_Elephant)
                return new ElephantMove(idx);

            throw new ArgumentException($"Create piecemove error: key values {key}");
        }
    }
}
