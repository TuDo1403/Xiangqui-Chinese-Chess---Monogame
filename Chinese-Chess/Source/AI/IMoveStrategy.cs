using ChineseChess.Source.GameObjects.Chess;
using Microsoft.Xna.Framework;

namespace ChineseChess.Source.AI
{
    public interface IMoveStrategy
    {
        (Point, Point) Search(BoardState state, int depth);

        int PositionsEvaluated { get; }

        string Name { get; }
    }
}
