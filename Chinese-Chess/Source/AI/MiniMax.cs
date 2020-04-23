using ChineseChess.Source.GameRule;
using ChineseChess.Source.Helper;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.AI
{
    public class MiniMax : IMoveStrategy
    {
        private int _player;

        public int BoardEvaluator(int[][] board, int player)
        {
            throw new NotImplementedException();
        }

        public MiniMax(int player)
        {
            _player = player;
        }

        public (int, Point, Point) Minimax(int[][] state, int depth, int player)
        {
            if (IsGameOver(state))
            {
                var score = BoardEvaluator(state, player);
                return (score, new Point(-1, -1), new Point(-1, -1));
            }
            var alpha = int.MaxValue;
            var beta = int.MinValue;
            if (player == _player)
                return MaxValue(state, player, depth, ref alpha, ref beta);
            else
                return MinValue(state, player, depth, ref alpha, ref beta);

        }

        private List<Point> GetTeamPieceIndices(int[][] board, int player)
        {
            var indices = new List<Point>();
            for (int i = 0; i < 10; ++i)
                for (int j = 0; j < 9; ++j)
                    if (board[i][j] != 0)
                        if (player == 1 && board[i][j] > 0 ||
                            player == 0 && board[i][j] < 0)
                            indices.Add(new Point(j, i));
                    
            return indices;
        }

        private List<Point> GetMoves(int[][] board, Point pieceIndx)
        {
            var key = Math.Abs(board[pieceIndx.Y][pieceIndx.X]);
            return PieceMoveFactory.CreatePieceMove(key, pieceIndx).FindLegalMoves(board);
        }

        private int[][] MakeMove(int[][] board, Point oldIdx, Point newIdx)
        {
            var value = board[oldIdx.Y][oldIdx.X];
            var newBoard = CopyBoard(board);
            newBoard[oldIdx.Y][oldIdx.X] = 0;
            newBoard[newIdx.Y][newIdx.X] = value;
            return newBoard;
        }

        private int[][] CopyBoard(int[][] board)
        {
            var newBoard = new int[10][];
            for (int i = 0; i < 10; ++i)
            {
                newBoard[i] = new int[9];
                for (int j = 0; j < 9; ++j)
                    newBoard[i][j] = board[i][j];
            }
            return newBoard;
        }

        private int SwitchTeam(int team) => -team + 1;

        private (int, Point, Point) MinValue(int[][] state, int player, int depth, ref int alpha, ref int beta)
        {
            if (depth == 0 || IsGameOver(state))
            {
                var score = BoardEvaluator(state, player);
                return (score, new Point(-1, -1), new Point(-1, -1));
            }

            var result = (int.MaxValue, new Point(-1, -1), new Point(-1, -1));
            foreach (var pieceIdx in GetTeamPieceIndices(state, player))
                foreach (var move in GetMoves(state, pieceIdx))
                {
                    var successor = MakeMove(state, pieceIdx, move);
                    var bestResult = MaxValue(successor, SwitchTeam(player), depth - 1, 
                                              ref alpha, ref beta);

                    var value = result.MaxValue < bestResult.Item1 ? result.MaxValue : bestResult.Item1;
                    if (value <= alpha) return (value, pieceIdx, move);
                    beta = beta < value ? beta : value;
                }

            return result;
        }

        private (int, Point, Point) MaxValue(int[][] state, int player, int depth, ref int alpha, ref int beta)
        {
            if (depth == 0 || IsGameOver(state))
            {
                var score = BoardEvaluator(state, player);
                return (score, new Point(-1, -1), new Point(-1, -1));
            }

            var result = (int.MinValue, new Point(-1, -1), new Point(-1, -1));
            foreach (var pieceIdx in GetTeamPieceIndices(state, player))
                foreach (var move in GetMoves(state, pieceIdx))
                {
                    var successor = MakeMove(state, pieceIdx, move);
                    var worstResult = MinValue(successor, SwitchTeam(player), depth - 1, 
                                               ref alpha, ref beta);
                    var value = result.MinValue > worstResult.Item1 ? result.MinValue : worstResult.Item1;
                    if (value >= beta) return (value, pieceIdx, move);
                    alpha = alpha > value ? alpha : value;
                }

            return result;
        }

        private bool IsGameOver(int[][] state)
        {
            return true;
        }
    }
}
