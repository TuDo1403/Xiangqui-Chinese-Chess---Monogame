using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.GameRule;
using ChineseChess.Source.Players;
using Microsoft.Xna.Framework;

namespace ChineseChess.Source.AI
{
    public interface IMoveStrategy
    {
        (Point, Point) Search(BoardState state, int depth, GameTime gameTime);

        int PositionsEvaluated { get; }

        string Name { get; }

        Team Player { get; }
    }
}
