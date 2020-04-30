using ChineseChess.Source.GameObjects.Chess;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ChineseChess.Source.GameRule.MoveLogic
{
    public interface IMovable
    {
        List<Point> LegalMoves { get; }
        Point Index { get; set; }
        List<Point> FindLegalMoves(BoardState board);

        int Value { get; set; }
    }
}
