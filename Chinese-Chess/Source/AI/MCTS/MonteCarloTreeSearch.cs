using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.GameRule;
using ChineseChess.Source.Players;
using Microsoft.Win32.SafeHandles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.AI.MCTS
{
    public class MonteCarloTreeSearch : IMoveStrategy
    {
        public int PositionsEvaluated { get; protected set; }

        public string Name { get; protected set; }

        public Team Player { get; protected set; }

        private int _simulations = 4000;

        public MonteCarloTreeSearch(Team player)
        {
            Player = player;
            Name = "MonteCarloTreeSearch";
        }

        public (Point, Point) Search(BoardState state, int simulations, GameTime gameTime)
        {
            PositionsEvaluated = 0;
            //ClearTree();
            //ResetTimer();
            return MCTS(state, simulations);
            //return MCTSRoot(state, simulations, gameTime);
        }


        private (Point, Point) MCTS(BoardState state, int simulations)
        {
            var currentPlayer = Player == Team.RED;
            var rootNode = new Node(null, state, (Point.Zero, Point.Zero), currentPlayer);
            for (int i = 0; i < _simulations; ++i)
            {
                var current = Traverse(rootNode);

                var simulationNode = RollOut(current);

                if (simulationNode != null)
                {
                    simulationNode.BackPropagate();
                }
            }
            return PickMaxUBC(rootNode).FromTo;
        }

        private Node RollOut(Node node)
        {
            var invertReward = Player == Team.RED ? 1 : -1;
            if (GameOver(node.State))
            {
                node.Visits += 1;
                node.TotalScore += BoardEvaluator(node.State) * invertReward;
                return node;
            }


            var validMoves = from piece in node.State.GetPieces(node.CurrentPlayer)
                             from move in node.State.GetLegalMoves(piece, true)
                             select (piece, move);
            var greedyMove = validMoves.ToList()[0];

            var successorState = node.State.SimulateMove(greedyMove.Item1, greedyMove.Item2);
            var simulationNode = new Node(node, successorState, greedyMove, !node.CurrentPlayer);

            simulationNode.TotalScore += BoardEvaluator(node.State) * invertReward;
            simulationNode.Visits += 1;
            node.State.Undo();
            
            return simulationNode;
        }

        private Node Traverse(Node node)
        {   
            if (node.Parent != null && node.Visits == 0)
            {
                node.Visits = 1;
                return node;
            }
            if (node.Children == null)
                node.Expand();

            var unvisitNodes = node.Children.Where(n => n.Visits == 0).ToList();
            if (unvisitNodes.Count > 0)
                return unvisitNodes[0];
            Node bestUBCNode = null;
            var maxUBC = PickMaxUBC(node);
            if (bestUBCNode != null)
            {
                return Traverse(bestUBCNode);
            }
            else
            {
                return node;
            }
            
        }

        private Node PickMaxUBC(Node node)
        {
            Node selected = null;
            var maxUBC = double.MinValue;
            foreach (var child in node.Children)
            {
                selected = UBC(child) > maxUBC ? child : selected;
                maxUBC = Math.Max(UBC(child), maxUBC);
            }
            return selected;
        }

        private static double UBC(Node node)
        {
            var w = node.TotalScore;
            var n = node.Visits;
            var v = w / n;
            var N = node.Parent.Visits;
            return v + Math.Sqrt((12000 * Math.Log(N)) / n);
        }

        private static bool GameOver(BoardState state)
        {
            return RedWins(state) || BlackWins(state);
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

        private int BoardEvaluator(BoardState board)
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
    }
}
