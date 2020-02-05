﻿using ChineseChess.Source.GameRule;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.GameObjects.Chess
{
    public sealed class Soldier : Piece
    {
        public Soldier(Texture2D texture, Vector2 position, int type) : base(texture, position, type)
        {
        }


        protected override void FindNextMoves()
        {
            base.FindNextMoves();
            FindVerticalMoves();
            if (RiverCrossed())
            {
                FindHorizontalMoves();
            }
        }

        private bool RiverCrossed()
        {
            return Type < 0 && MatrixPos.Y > (int)BoardRule.BlackBorder ||
                   Type > 0 && MatrixPos.Y < (int)BoardRule.RedBorder;
        }

        protected override void FindHorizontalMoves()
        {
            if (MatrixPos.X + 1 < (int)BoardRule.Columns)
            {
                StillHasValidMoves(MatrixPos.Y, MatrixPos.X + 1);
            }
            if (MatrixPos.X - 1 >= 0)
            {
                StillHasValidMoves(MatrixPos.Y, MatrixPos.X - 1);
            }
        }

        protected override void FindVerticalMoves()
        {
            var step = 1;
            if (Type > 0)
            {
                step = -step;
            }
            StillHasValidMoves(MatrixPos.Y + step, MatrixPos.X);
        }
    }
}
