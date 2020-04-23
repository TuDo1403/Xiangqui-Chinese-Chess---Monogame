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
            var score = 0;
            for (int i = 0; i < 10; ++i)
                for (int j = 0; j < 9; ++j)
                    score += board[i][j];
            return score;
        }

        public MiniMax(int player)
        {
            _player = player;
        }

        public (int, Point, Point) Minimax(int[][] state, int depth)
        {
            if (IsGameOver(state))
                return (BoardEvaluator(state, _player), new Point(-1, -1), new Point(-1, -1)); 
            //var alpha = int.MinValue;
            //var beta = int.MaxValue;
            var alpha = -100000;
            var beta = 100000;

            var result = MinValue(state, _player, depth, ref alpha, ref beta);
            return result;
            //if (player == _player)
            //    return MaxValue(state, player, depth, ref alpha, ref beta);
            //else
            //    return MinValue(state, player, depth, ref alpha, ref beta);

        }

        private (int, Point, Point) MinValue(int[][] state, int player, int depth, ref int alpha, ref int beta)
        {
            if (depth == 0 || IsGameOver(state))
                return (BoardEvaluator(state, player), new Point(-1, -1), new Point(-1, -1));

            //var value = int.MaxValue;
            var bestMove = (100000, new Point(-1, -1), new Point(-1, -1));
            var pieces = GetTeamPieceIndices(state, player);
            foreach (var pieceIdx in GetTeamPieceIndices(state, player))
                foreach (var move in GetMoves(state, pieceIdx))
                {
                    var successor = MakeMove(state, pieceIdx, move);
                    //var maxVal = MaxValue(successor, SwitchTeam(player), depth - 1,
                    //                      ref alpha, ref beta);
                    var maxMove = MaxValue(successor, SwitchTeam(player), depth - 1,
                                           ref alpha, ref beta);
                    if (maxMove.Item1 < bestMove.Item1)
                        bestMove = (maxMove.Item1, pieceIdx, move);

                    if (maxMove.Item1 <= alpha) return bestMove;
                    beta = beta < maxMove.Item1 ? beta : maxMove.Item1;
                    

                    //if (maxVal < value)
                    //{
                    //    value = maxVal;
                    //    //MinMove = (pieceIdx, move);
                    //}
                    

                    //if (maxVal <= alpha) return value;
                    //beta = beta < maxVal ? beta : maxVal;
                }

            //return value;
            return bestMove;
        }

        private (int, Point, Point) MaxValue(int[][] state, int player, int depth, ref int alpha, ref int beta)
        {
            if (depth == 0 || IsGameOver(state))
                return (BoardEvaluator(state, player), new Point(-1, -1), new Point(-1, -1));

            //var value = int.MinValue;
            var bestMove = (-100000, new Point(-1, -1), new Point(-1, -1));
            foreach (var pieceIdx in GetTeamPieceIndices(state, player))
                foreach (var move in GetMoves(state, pieceIdx))
                {
                    var successor = MakeMove(state, pieceIdx, move);
                    //var minVal = MinValue(successor, SwitchTeam(player), depth - 1,
                    //                           ref alpha, ref beta);
                    var minMove = MinValue(successor, SwitchTeam(player), depth - 1,
                                           ref alpha, ref beta);

                    if (minMove.Item1 > bestMove.Item1)
                        bestMove = (minMove.Item1, pieceIdx, move);
                    if (minMove.Item1 >= beta) return bestMove;

                    alpha = minMove.Item1 > alpha ? minMove.Item1 : alpha;

                    //value = minVal > value ? minVal : value;
                    //if (minVal > value)
                    //{
                    //    value = minVal;
                    //    //MaxMove = (pieceIdx, move);
                    //}

                    //if (minVal >= beta) return value;
                    //alpha = minVal > alpha ? minVal : alpha;
                }

            //return value;
            return bestMove;
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

        

        private bool IsGameOver(int[][] state)
        {
            var result = true;
            for (int i = 0; i < 10; ++i)
                for (int j = 0; j < 9; ++j)
                    if (_player == 1 && state[i][j] == 100 ||
                        _player == 0 && state[i][j] == -100)
                    {
                        result = false;
                        break;
                    }
            return result;
        }
    }
}
