using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.GameRule;
using Microsoft.Xna.Framework;
using System;

namespace ChineseChess.Source.AI
{
    public class MiniMax : IMoveStrategy
    {
        private readonly Team _player;

        public int BoardEvaluator(BoardState board)
        {
            if (RedWins(board)) return int.MaxValue;
            if (BlackWins(board)) return int.MinValue;

            var score = 0;
            for (int i = 0; i < (int)Rule.ROW; ++i)
                for (int j = 0; j < (int)Rule.COL; ++j)
                    if (board[i, j] != 0)
                        score += board[i, j] + board.PosVal(i, j);

            return score;
        }


        public MiniMax(Team player) => _player = player;

        public (Point, Point) MinimaxRoot(BoardState state, int depth, bool moveOrder)
        {
            var alpha = int.MinValue;
            var beta = int.MaxValue;
            var isMaximizingPlayer = _player == Team.BLACK ? false : true;
            var bestVal = _player == Team.BLACK ? int.MaxValue : int.MinValue;
            var bestMoveFound = (Point.Zero, Point.Zero);

            foreach (var pieceIdx in state.GetPieces(isMaximizingPlayer))
                foreach (var move in state.GetLegalMoves(pieceIdx, moveOrder))
                {
                    state.SimulateMove(pieceIdx, move);
                    var value = Minimax(state, !isMaximizingPlayer, depth - 1, alpha, beta, moveOrder);
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

        public int Minimax(BoardState state, bool isMaximizingPlayer, 
                           int depth, int alpha, int beta, bool moveOrder)
        {
            if (isMaximizingPlayer)
                return MaxValue(state, isMaximizingPlayer, depth, alpha, beta, moveOrder);
            else
                return MinValue(state, isMaximizingPlayer, depth, alpha, beta, moveOrder);
        }

        private int MinValue(BoardState state, bool isMaximizingPlayer, 
                             int depth, int alpha, int beta, bool moveOrder)
        {
            if (depth == 0 || GameOver(state)) return BoardEvaluator(state);

            var bestMove = int.MaxValue;
            foreach (var pieceIdx in state.GetPieces(isMaximizingPlayer))
                foreach (var move in state.GetLegalMoves(pieceIdx, moveOrder))
                {
                    state.SimulateMove(pieceIdx, move);
                    bestMove = Math.Min(bestMove, MaxValue(state, !isMaximizingPlayer, 
                                                           depth - 1, alpha, beta, moveOrder));
                    state.Undo();
                    beta = Math.Min(beta, bestMove);
                    if (beta <= alpha) return bestMove;
                }

            return bestMove;
        }

        private int MaxValue(BoardState state, bool isMaximizingPlayer, 
                             int depth, int alpha, int beta, bool moveOrder)
        {
            if (depth == 0 || GameOver(state)) return BoardEvaluator(state);

            var bestMove = int.MinValue;
            foreach (var pieceIdx in state.GetPieces(isMaximizingPlayer))
                foreach (var move in state.GetLegalMoves(pieceIdx, moveOrder))
                {
                    state.SimulateMove(pieceIdx, move);
                    bestMove = Math.Max(bestMove, MinValue(state, !isMaximizingPlayer, 
                                                           depth - 1, alpha, beta, moveOrder));
                    state.Undo();
                    alpha = Math.Max(alpha, bestMove);
                    if (beta <= alpha) return bestMove;
                }

            return bestMove;
        }

        private static bool RedWins(BoardState state)
        {
            for (int i = 0; i <= (int)Rule.FB_CASTLE; ++i)
                for (int j = (int)Rule.L_CASTLE; j <= (int)Rule.R_CASTLE; ++j)
                    if (state[i, j] == (int)Pieces.B_General)
                        return false;

            return true;
        }

        private static bool BlackWins(BoardState state)
        {
            for (int i = (int)Rule.FR_CASTLE; i <= (int)Rule.COL; ++i)
                for (int j = (int)Rule.L_CASTLE; j <= (int)Rule.R_CASTLE; ++j)
                    if (state[i, j] == (int)Pieces.R_General)
                        return false;

            return true;
        }

        private static bool GameOver(BoardState state) => RedWins(state) || BlackWins(state);
    }
}
