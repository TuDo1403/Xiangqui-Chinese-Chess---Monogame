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

        private int _simulations = 3000;

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
            return MCTS(state, simulations, gameTime);
        }

        //private void ResetTimer() => _startTime = 0;

        //private void ClearTree() => _rootNode = null;

        //private (Point, Point) MCTSRoot(BoardState state, int simulations, GameTime gameTime)
        //{
        //    var turn = Player != Team.BLACK;
        //    var bestVal = 0d;
        //    var bestMoveFound = (Point.Zero, Point.Zero);
        //    foreach (var pieceIdx in state.GetPieces(turn))
        //    {
        //        foreach (var move in state.GetLegalMoves(pieceIdx))
        //        {
        //            state.SimulateMove(pieceIdx, move);
        //            var value = MCTS(state, !turn, simulations, gameTime);
        //            state.Undo();

        //            if (bestVal < value)
        //            {
        //                bestVal = value;
        //                bestMoveFound = (pieceIdx, move);
        //            }
        //        }
        //    }
        //    return bestMoveFound;
        //}

        private (Point, Point) MCTS(BoardState state, int simulations, GameTime gameTime)
        {
            var turn = Player != Team.BLACK;
            var rootNode = CreateTree(state, turn);
            for (int i = 0; i < _simulations; ++i)
            {
                var current = Traverse(rootNode);

                var simulationResult = RollOut(current);

                current.NumberOfSimulations += 1;
                current.TotalScore += simulationResult;
                current.BackPropagate();
            }

            return BestChild(rootNode);
        }

        //private void RollOut(Node node, int simulations)
        //{
        //    var reward = Simulate(node, simulations);
        //    node.TotalScore += reward;
        //    node.NumberOfSimulations += 1;
        //    node.BackPropagate();
        //}

        private int RollOut(Node node)
        {
            if (GameOver(node.State))
                return BoardEvaluator(node.State);

            var maxVal = int.MinValue;
            var simulateMove = (Point.Zero, Point.Zero);
            foreach (var piece in node.State.GetPieces(node.NextTurn))
            {
                foreach (var move in node.State.GetLegalMoves(piece))
                {
                    simulateMove = maxVal > Math.Abs(node.State[move.Y, move.X]) ? simulateMove : (piece, move);
                    maxVal = Math.Max(maxVal, Math.Abs(node.State[move.Y, move.X]));
                }
            }

            var successorState = node.State.SimulateMove(simulateMove.Item1, simulateMove.Item2);
            var simulationNode = new Node(node, successorState, simulateMove, !node.NextTurn);
            if (node.Children == null)
                node.Children = new List<Node>();
            node.Children.Add(simulationNode);
            var eval = BoardEvaluator(node.State);
            node.State.Undo();

            return eval;
        }

        private (Point, Point) BestChild(Node rootNode)
        {
            var bestChild = rootNode.Children[0];
            foreach (var child in rootNode.Children)
            {
                bestChild = UBC(child) > UBC(bestChild) ? child : bestChild;
            }
            return bestChild.FromTo;
        }

        private Node CreateTree(BoardState state, bool nextTurn)
        {
            var rootNode = new Node(null, state, (Point.Zero, Point.Zero), nextTurn);
            rootNode.Expand();
            return rootNode;
        }

        private Node Traverse(Node node)
        {
            //var bestUBCNode = node.Children[0];
            //foreach (var child in node.Children)
            //{
            //    if (child.Children != null)
            //    {
            //        bestUBCNode = UBC(Traverse(child)) > UBC(bestUBCNode) ? Traverse(child) : bestUBCNode;
            //    }
            //    else
            //    {
            //        bestUBCNode = UBC(child) > UBC(bestUBCNode) ? child : bestUBCNode;
            //    }
            //}
            //return bestUBCNode;
            
            foreach (var child in node.Children)
                if (!child.IsVisited())
                    return child;

            var bestUBCNode = node.Children[0];
            foreach (var child in node.Children)
            {
                if (child.Children != null)
                {
                    return Traverse(child);
                }
                bestUBCNode = UBC(child) > UBC(bestUBCNode) ? child : bestUBCNode;
            }
            bestUBCNode.Expand();
            return Traverse(bestUBCNode);
        }

        private static double UBC(Node node)
        {
            var w = node.TotalScore;
            var n = node.NumberOfSimulations;
            var v = w / n;
            var N = node.Parent.NumberOfSimulations;
            return v + 2 * Math.Sqrt(Math.Log(N) / n);
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
            var invertReward = Player == Team.BLACK ? -1 : 1;
            if (RedWins(board)) return 500000 * invertReward;
            if (BlackWins(board)) return -500000 * invertReward;
            var score = 0;
            for (int i = 0; i < (int)Rule.ROW; ++i)
                for (int j = 0; j < (int)Rule.COL; ++j)
                    if (board[i, j] != 0)
                        score += board[i, j] + board.PosVal(i, j);

            return score * invertReward;
            //if (RedWins(board) && Player == Team.RED ||
            //    BlackWins(board) && Player == Team.BLACK)
            //    return 1;
            //else if (RedWins(board) && Player == Team.BLACK ||
            //         BlackWins(board) && Player == Team.RED)
            //    return -1;
            //return 0;
        }


        //private double Simulate(ref Node node, int simulations)
        //{
        //    if (simulations == 0 || GameOver(node.State))
        //    {
        //        PositionsEvaluated += 2;
        //        var score = BoardEvaluator(node.State);
        //        return Player == Team.BLACK ? 1 - score : score;
        //    }

        //    var pieces = node.State.GetPieces(nexTurn);
        //    Point randomFrom = Point.Zero;
        //    List<Point> moves = null;
        //    var outOfMove = true;
        //    while (outOfMove)
        //    {
        //        randomFrom = pieces[new Random().Next(pieces.Count)];
        //        moves = node.State.GetLegalMoves(randomFrom);
        //        if (moves.Count != 0) outOfMove = false;
        //    }
        //    var randomTo = moves[new Random().Next(moves.Count)];

        //    node.State.SimulateMove(randomFrom, randomTo);
        //    var reward = Rollout(node, simulations - 1, !nexTurn);
        //    node.State.Undo();

        //    return reward;

        //    //while (simulations > 0)
        //    //{
        //    //    --simulations;
        //    //    if (GameOver(node.State))
        //    //    {
        //    //        return node.NextTurn == true ? 0 : 1;
        //    //    }
        //    //    node.Expand();
        //    //    node = node.FindRandomChild();
        //    //}
        //    //var rw = BoardEvaluator(node.State);
        //    //return node.NextTurn == true ? 1 - rw : rw;
        //}




    }
}
