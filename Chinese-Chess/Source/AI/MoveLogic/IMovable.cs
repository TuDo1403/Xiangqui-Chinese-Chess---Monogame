using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.AI.MoveLogic
{
    public interface IMovable
    {
        List<Point> LegalMoves { get; }
        Point Index { get; set; }
        List<Point> FindLegalMoves(int[][] board);

        int Value { get; set; }
    }
}
