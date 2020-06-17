using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.GameRule;
using Microsoft.Xna.Framework;
using System;

namespace ChineseChess.Source.AI.Minimax
{
    public class MoveOrdering : AlphaBetaPruning
    {
        public MoveOrdering(Team player) : base(player) => Name = "MoveOrdering";

        protected override (Point, Point) MinimaxRoot(BoardState state, int depth)
        {
            var alpha = -10000;
            var beta = 10000;
            var isMaximizingPlayer = Player != Team.BLACK;
            var bestVal = Player == Team.BLACK ? int.MaxValue : int.MinValue;
            var bestMoveFound = (Point.Zero, Point.Zero);

            foreach (var pieceIdx in state.GetPieces(isMaximizingPlayer))
                foreach (var move in state.GetLegalMoves(pieceIdx, true))
                {
                    state.SimulateMove(pieceIdx, move);
                    var value = Minimax(state, !isMaximizingPlayer, depth - 1, alpha, beta);
                    state.Undo();

                    if (isMaximizingPlayer && value >= bestVal ||
                        !isMaximizingPlayer && value <= bestVal)
                    {
                        bestVal = value;
                        bestMoveFound = (pieceIdx, move);
                    }
                }

            return bestMoveFound;
        }

        private int Minimax(BoardState state, bool isMaximizingPlayer,
                           int depth, int alpha, int beta)
        {
            if (isMaximizingPlayer)
                return MaxValue(state, isMaximizingPlayer, depth, alpha, beta);
            else
                return MinValue(state, isMaximizingPlayer, depth, alpha, beta);
        }

        private int MinValue(BoardState state, bool isMaximizingPlayer,
                             int depth, int alpha, int beta)
        {
            ++PositionsEvaluated;
            if (depth == 0 || GameOver(state)) return BoardEvaluator(state);

            var bestMove = 10000;
            foreach (var pieceIdx in state.GetPieces(isMaximizingPlayer))
                foreach (var move in state.GetLegalMoves(pieceIdx, true))
                {
                    state.SimulateMove(pieceIdx, move);
                    bestMove = Math.Min(bestMove, MaxValue(state, !isMaximizingPlayer,
                                                           depth - 1, alpha, beta));
                    state.Undo();
                    beta = Math.Min(beta, bestMove);
                    if (beta <= alpha) return bestMove;
                }

            return bestMove;
        }

        private int MaxValue(BoardState state, bool isMaximizingPlayer,
                             int depth, int alpha, int beta)
        {
            ++PositionsEvaluated;
            if (depth == 0 || GameOver(state)) return BoardEvaluator(state);

            var bestMove = -10000;
            foreach (var pieceIdx in state.GetPieces(isMaximizingPlayer))
                foreach (var move in state.GetLegalMoves(pieceIdx, true))
                {
                    state.SimulateMove(pieceIdx, move);
                    bestMove = Math.Max(bestMove, MinValue(state, !isMaximizingPlayer,
                                                           depth - 1, alpha, beta));
                    state.Undo();
                    alpha = Math.Max(alpha, bestMove);
                    if (beta <= alpha) return bestMove;
                }

            return bestMove;
        }
    }
}
