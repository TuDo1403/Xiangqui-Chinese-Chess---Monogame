using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.GameRule;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace ChineseChess.Source.AI
{
    public class MiniMax : IMoveStrategy
    {
        private readonly Team _player;

        public int BoardEvaluator(BoardState board)
        {
            if (board == null) throw new ArgumentNullException(nameof(board));

            if (RedWins(board)) return int.MaxValue;
            if (BlackWins(board)) return int.MinValue;

            var score = 0;
            var redVal = board.GetPieces(true)
                              .Sum(pieceIdx => board[pieceIdx.Y, pieceIdx.X] + board.PosVal(pieceIdx));
            
            var blackVal = board.GetPieces(false)
                                .Sum(pieceIdx => board[pieceIdx.Y, pieceIdx.X] + board.PosVal(pieceIdx));
            score = redVal + blackVal;

            return score;
        }


        public MiniMax(Team player) => _player = player;

        public (Point, Point) MinimaxRoot(BoardState state, int depth)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            var alpha = int.MinValue;
            var beta = int.MaxValue;
            var isMaximizingPlayer = _player == Team.BLACK ? false : true;
            var bestVal = _player == Team.BLACK ? int.MaxValue : int.MinValue;
            var bestMoveFound = (Point.Zero, Point.Zero);

            foreach (var pieceIdx in state.GetPieces(isMaximizingPlayer, true))
                foreach (var move in state.GetLegalMoves(pieceIdx))
                {
                    var newGameMove = (pieceIdx, move);
                    state.SimulateMove(pieceIdx, move);
                    var value = Minimax(state, !isMaximizingPlayer, depth - 1, alpha, beta);
                    state.Undo();

                    if (isMaximizingPlayer && value >= bestVal ||
                        !isMaximizingPlayer && value <= bestVal)
                    {
                        bestVal = value;
                        bestMoveFound = newGameMove;
                    }
                }
            return bestMoveFound;
        }

        public int Minimax(BoardState state, bool isMaximizingPlayer, int depth, int alpha, int beta)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            if (isMaximizingPlayer)
                return MaxValue(state, isMaximizingPlayer, depth, alpha, beta);
            else
                return MinValue(state, isMaximizingPlayer, depth, alpha, beta);
            
        }

        private int MinValue(BoardState state, bool isMaximizingPlayer, int depth, int alpha, int beta)
        {
            if (depth == 0 || IsGameOver(state)) return BoardEvaluator(state);

            var bestMove = int.MaxValue;
            foreach (var pieceIdx in state.GetPieces(isMaximizingPlayer, true))
                foreach (var move in state.GetLegalMoves(pieceIdx))
                {
                    state.SimulateMove(pieceIdx, move);
                    bestMove = Math.Min(bestMove, MaxValue(state, !isMaximizingPlayer, depth - 1, 
                                                           alpha, beta));
                    state.Undo();
                    beta = Math.Min(beta, bestMove);
                    if (beta <= alpha) return bestMove;
                }

            return bestMove;
        }

        private int MaxValue(BoardState state, bool isMaximizingPlayer, int depth, int alpha, int beta)
        {
            if (depth == 0 || IsGameOver(state)) return BoardEvaluator(state);

            var bestMove = int.MinValue;
            foreach (var pieceIdx in state.GetPieces(isMaximizingPlayer, true))
                foreach (var move in state.GetLegalMoves(pieceIdx))
                {
                    state.SimulateMove(pieceIdx, move);
                    bestMove = Math.Max(bestMove, MinValue(state, !isMaximizingPlayer, depth - 1,
                                                           alpha, beta));
                    state.Undo();
                    alpha = Math.Max(alpha, bestMove);
                    if (beta <= alpha) return bestMove;
                }
            return bestMove;
        }

        private bool RedWins(BoardState state)
        {
            for (int i = 0; i <= (int)BoardRule.FB_CASTLE; ++i)
                for (int j = (int)BoardRule.L_CASTLE; j <= (int)BoardRule.R_CASTLE; ++j)
                    if (state[i, j] == (int)Pieces.B_General)
                        return false;
            return true;
        }

        private bool BlackWins(BoardState state)
        {
            for (int i = (int)BoardRule.FR_CASTLE; i <= (int)BoardRule.COL; ++i)
                for (int j = (int)BoardRule.L_CASTLE; j <= (int)BoardRule.R_CASTLE; ++j)
                    if (state[i, j] == (int)Pieces.R_General)
                        return false;
            return true;
        }

        private static bool IsGameOver(BoardState state)
        {
            var generalCount = 0;
            for (int i = (int)BoardRule.FB_CASTLE; i <= (int)BoardRule.COL; ++i)
                for (int j = (int)BoardRule.L_CASTLE; j <= (int)BoardRule.R_CASTLE; ++j)
                    if (state[i, j] == (int)Pieces.R_General)
                        ++generalCount;
            for (int i = 0; i <= (int)BoardRule.FB_CASTLE; ++i)
                for (int j = (int)BoardRule.L_CASTLE; j <= (int)BoardRule.R_CASTLE; ++j)
                    if (state[i, j] == (int)Pieces.B_General)
                        ++generalCount;

            return generalCount != 2;
        }
    }
}
