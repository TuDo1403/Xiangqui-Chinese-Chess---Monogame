using Microsoft.Xna.Framework;
using System;

namespace ChineseChess.Source.Helper
{
    public class PositionTransitionEventArgs : EventArgs
    {
        public Point CurrentIdx { get; set; }

        public Point NewIdx { get; set; }



        public PositionTransitionEventArgs(Point currentIdx, Point newIdx)
        {
            CurrentIdx = currentIdx;
            NewIdx = newIdx;
        }
    }
}
