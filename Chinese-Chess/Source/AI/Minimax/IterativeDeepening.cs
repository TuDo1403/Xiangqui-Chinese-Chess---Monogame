using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.GameRule;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.AI
{
    public class IterativeDeepening : IMoveStrategy
    {
        public Team Player { get; protected set; }
        public string Name { get; protected set; }
        public int PositionsEvaluated { get; protected set; }

        public IterativeDeepening(Team player)
        {
            Player = player;
            Name = "IterativeDeepening";
        }

        public (Point, Point) Search(BoardState state, int depth, GameTime gameTime)
        {
            PositionsEvaluated = 0;
            return MinimaxRoot(state, depth);
        }

        protected virtual (Point, Point) MinimaxRoot(BoardState state, int depth)
        {
            var isMaximizingPlayer = Player != Team.BLACK;
            var bestVal = Player == Team.BLACK ? int.MaxValue : int.MinValue;
            var bestMoveFound = (Point.Zero, Point.Zero);

            foreach (var pieceIdx in state.GetPieces(isMaximizingPlayer))
                foreach (var move in state.GetLegalMoves(pieceIdx))
                {
                    state.SimulateMove(pieceIdx, move);
                    var value = Minimax(state, !isMaximizingPlayer, depth - 1);
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
                           int depth)
        {
            if (isMaximizingPlayer)
                return MaxValue(state, isMaximizingPlayer, depth);
            else
                return MinValue(state, isMaximizingPlayer, depth);
        }

        private int MinValue(BoardState state, bool isMaximizingPlayer,
                             int depth)
        {
            ++PositionsEvaluated;
            if (depth == 0 || GameOver(state)) return BoardEvaluator(state);

            var bestMove = int.MaxValue;
            foreach (var pieceIdx in state.GetPieces(isMaximizingPlayer))
                foreach (var move in state.GetLegalMoves(pieceIdx))
                {
                    state.SimulateMove(pieceIdx, move);
                    bestMove = Math.Min(bestMove, MaxValue(state, !isMaximizingPlayer,
                                                           depth - 1));
                    state.Undo();
                }

            return bestMove;
        }

        private int MaxValue(BoardState state, bool isMaximizingPlayer,
                             int depth)
        {
            ++PositionsEvaluated;
            if (depth == 0 || GameOver(state)) return BoardEvaluator(state);

            var bestMove = int.MinValue;
            foreach (var pieceIdx in state.GetPieces(isMaximizingPlayer))
                foreach (var move in state.GetLegalMoves(pieceIdx))
                {
                    state.SimulateMove(pieceIdx, move);
                    bestMove = Math.Max(bestMove, MinValue(state, !isMaximizingPlayer,
                                                           depth - 1));
                    state.Undo();
                }

            return bestMove;
        }


        protected int BoardEvaluator(BoardState board)
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

        protected static bool GameOver(BoardState state) => RedWins(state) || BlackWins(state);

        protected static bool RedWins(BoardState state)
        {
            for (int i = 0; i <= (int)Rule.FB_CASTLE; ++i)
                for (int j = (int)Rule.L_CASTLE; j <= (int)Rule.R_CASTLE; ++j)
                    if (state[i, j] == (int)Pieces.B_General)
                        return false;

            return true;
        }

        protected static bool BlackWins(BoardState state)
        {
            for (int i = (int)Rule.FR_CASTLE; i <= (int)Rule.COL; ++i)
                for (int j = (int)Rule.L_CASTLE; j <= (int)Rule.R_CASTLE; ++j)
                    if (state[i, j] == (int)Pieces.R_General)
                        return false;

            return true;
        }
    }
}
