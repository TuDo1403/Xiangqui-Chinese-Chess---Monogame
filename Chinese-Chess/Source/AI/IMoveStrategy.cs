using ChineseChess.Source.GameObjects.Chess;

namespace ChineseChess.Source.AI
{
    public interface IMoveStrategy
    {
        int BoardEvaluator(BoardState board);
    }
}
