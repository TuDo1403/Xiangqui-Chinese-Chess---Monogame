using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
