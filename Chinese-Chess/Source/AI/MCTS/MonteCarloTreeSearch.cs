using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.GameRule;
using ChineseChess.Source.Players;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.AI.MCTS
{
    class MonteCarloTreeSearch : IMoveStrategy
    {
        public int PositionsEvaluated { get; protected set; }

        private int w = 0;
        private int n = 0;

        private double C = Math.Sqrt(2);

        public string Name { get; protected set; }

        public Team Player { get; protected set; }

        public MonteCarloTreeSearch(Team player)
        {
            Player = player;
            Name = "MonteCarloTreeSearch";
        }

        public (Point, Point) Search(BoardState state, int depth)
        {
            throw new NotImplementedException();
        }

        private (Point, Point) MCTSRoot(BoardState state)
        {
            var isMaximizingPlayer = Player != Team.BLACK;
            var bestVal = Player == Team.BLACK ? int.MaxValue : int.MinValue;
            var bestMoveFound = (Point.Zero, Point.Zero);

            foreach (var pieceIndx in state.GetPieces(isMaximizingPlayer))
            {
                foreach (var move in state.GetLegalMoves(pieceIndx))
                {

                }
            }

            return bestMoveFound;
        }

        private float MCTS()
        {
            return 0;
        }

        private void RandomMove()
        {

        }
    }
}
