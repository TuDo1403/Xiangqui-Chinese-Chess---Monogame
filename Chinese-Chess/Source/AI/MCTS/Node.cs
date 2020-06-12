using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.GameRule;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.AI.MCTS
{
    public class Node
    {
        public int TotalScore { get; set; }

        public int Visits { get; set; }

        public BoardState State { get; set; }

        public List<Node> Children { get; set; }

        public (Point, Point) FromTo { get; set; }

        public Node Parent { get; set; }

        public bool CurrentPlayer { get; set; }



        public Node(Node parent, BoardState state, (Point, Point) fromTo, bool currentPlayer)
        {
            Parent = parent;
            TotalScore = 0;
            Visits = 0;
            State = state;
            CurrentPlayer = currentPlayer;
            FromTo = fromTo;
        }

        public bool IsVisited()
        {
            return Visits > 0;
        }


        public void Expand()
        {
            Children = new List<Node>();
            if (GameOver()) return;

            foreach (var pieceIdx in State.GetPieces(CurrentPlayer))
            {
                foreach (var move in State.GetLegalMoves(pieceIdx))
                {
                    var node = new Node(this, State.SimulateMove(pieceIdx, move), 
                                        (pieceIdx, move), !CurrentPlayer);
                    Children.Add(node);
                    State.Undo();
                }
            }
        }

        public void BackPropagate()
        {
            if (Parent != null)
            {
                Parent.Visits += 1;
                Parent.TotalScore += TotalScore;
                Parent.BackPropagate();
            }
        }

        public Node FindRandomChild()
        {
            return Children[new Random().Next(Children.Count)];
        }

        private bool GameOver()
        {
            return RedWins(State) || BlackWins(State);
        }

        private static int BoardEvaluator(BoardState board)
        {
            if (RedWins(board)) return 500000;
            if (BlackWins(board)) return -500000;
            var score = 0;
            for (int i = 0; i < (int)Rule.ROW; ++i)
                for (int j = 0; j < (int)Rule.COL; ++j)
                    if (board[i, j] != 0)
                        score += board[i, j] + board.PosVal(i, j);

            return score;
            //if (RedWins(board) && Player == Team.RED ||
            //    BlackWins(board) && Player == Team.BLACK)
            //    return 1;
            //else if (RedWins(board) && Player == Team.BLACK ||
            //         BlackWins(board) && Player == Team.RED)
            //    return -1;
            //return 0;
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
    }
}
