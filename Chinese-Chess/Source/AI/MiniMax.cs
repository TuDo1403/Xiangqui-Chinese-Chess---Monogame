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
        private readonly int _player;

        public int BoardEvaluator(int[][] board, int player)
        {
            if (board == null) throw new ArgumentNullException(nameof(board));

            var score = 0;
            for (int i = 0; i < 10; ++i)
                for (int j = 0; j < 9; ++j)
                    score += board[i][j];
            if (Losing(board, player))
                score = player == 0 ? score + 100000 : score - 100000;
            else if (Winning(board, player))
                score = player == 0 ? score - 100000 : score + 100000;
            
            return score;
        }

        public MiniMax(int player)
        {
            _player = player;
        }

        public (int, Point, Point) Minimax(int[][] state, int depth)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            var alpha = -100000;
            var beta = 100000;

            if (_player == 1)
                return Max(state, _player, depth, ref alpha, ref beta);
            else
                return Min(state, _player, depth, ref alpha, ref beta);

        }

        private (int, Point, Point) Min(int[][] state, int player, int depth, ref int alpha, ref int beta)
        {
            if (depth == 0)
                return (BoardEvaluator(state, player), new Point(-1, -1), new Point(-1, -1));

            var bestMove = (100000, new Point(-1, -1), new Point(-1, -1));
            foreach (var pieceIdx in GetTeamPieceIndices(state, player))
                foreach (var move in GetMoves(state, pieceIdx))
                {
                    var successor = MakeMove(state, pieceIdx, move);
                    var maxMove = Max(successor, SwitchTeam(player), depth - 1,
                                           ref alpha, ref beta);
                    if (maxMove.Item1 < bestMove.Item1)
                        bestMove = (maxMove.Item1, pieceIdx, move);
                    if (maxMove.Item1 <= alpha) return bestMove;
                    beta = maxMove.Item1 < beta ? maxMove.Item1 : beta;
                }

            return bestMove;
        }

        private (int, Point, Point) Max(int[][] state, int player, int depth, ref int alpha, ref int beta)
        {
            if (depth == 0)
                return (BoardEvaluator(state, player), new Point(-1, -1), new Point(-1, -1));

            var bestMove = (-100000, new Point(-1, -1), new Point(-1, -1));
            foreach (var pieceIdx in GetTeamPieceIndices(state, player))
                foreach (var move in GetMoves(state, pieceIdx))
                {
                    var successor = MakeMove(state, pieceIdx, move);
                    var minMove = Min(successor, SwitchTeam(player), depth - 1,
                                           ref alpha, ref beta);

                    if (minMove.Item1 > bestMove.Item1)
                        bestMove = (minMove.Item1, pieceIdx, move);
                    if (minMove.Item1 >= beta) return bestMove;
                    alpha = minMove.Item1 > alpha ? minMove.Item1 : alpha;
                }
            return bestMove;
        }

        private static List<Point> GetTeamPieceIndices(int[][] board, int player)
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

        private static List<Point> GetMoves(int[][] board, Point pieceIndx)
        {
            var key = Math.Abs(board[pieceIndx.Y][pieceIndx.X]);
            return PieceMoveFactory.CreatePieceMove(key, pieceIndx).FindLegalMoves(board);
        }

        private static int[][] MakeMove(int[][] board, Point oldIdx, Point newIdx)
        {
            var value = board[oldIdx.Y][oldIdx.X];
            var newBoard = CopyBoard(board);
            newBoard[oldIdx.Y][oldIdx.X] = 0;
            newBoard[newIdx.Y][newIdx.X] = value;
            return newBoard;
        }

        private static int[][] CopyBoard(int[][] board)
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

        private static int SwitchTeam(int team) => -team + 1;

        private bool Losing(int[][] state, int player)
        {
            var isLosing = true;
            for (int i = 0; i < 10; ++i)
                for (int j = 0; j < 9; ++j)
                    if (player == 0 && state[i][j] == -100 ||
                        player == 1 && state[i][j] == 100)
                        return false;
            return isLosing;
        }

        private bool Winning(int[][] state, int player)
        {
            var isWinning = true;
            for (int i = 0; i < 10; ++i)
                for (int j = 0; j < 9; ++j)
                    if (player == 0 && state[i][j] == -100 ||
                        player == 1 && state[i][j] == 100)
                        return false;
            return isWinning;

        }

        

        //private bool IsGameOver(int[][] state)
        //{
        //    var result = true;
        //    for (int i = 0; i < 10; ++i)
        //        for (int j = 0; j < 9; ++j)
        //            if (_player == 1 && state[i][j] == 100 ||
        //                _player == 0 && state[i][j] == -100)
        //            {
        //                result = false;
        //                break;
        //            }
        //    return result;
        //}
    }
}
