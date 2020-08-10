using ChineseChess.Source.AI.Minimax;
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
    public class MinimaxUCT : IMoveStrategy
    {
        public int PositionsEvaluated { get; protected set; }

        public string Name { get; protected set; }

        public Team Player { get; protected set; }

        private readonly int _simulations = 300;

        private static int _depth;

        private static Node _rootNode;



        public MinimaxUCT(Team player)
        {
            Player = player;
            Name = "MinimaxUCT";
        }

        public (Point, Point) Search(BoardState state, int depth, GameTime gameTime)
        {
            PositionsEvaluated = 0;
            return UCTSearch(state, depth);
        }


        private (Point, Point) UCTSearch(BoardState state, int depth)
        {
            _depth = depth;
            var currentPlayer = Player == Team.RED;
            _rootNode = new Node(null, state, (Point.Zero, Point.Zero), currentPlayer);
            for (int i = 0; i < _simulations; ++i)
            {
                var v = TreePolicy(_rootNode);
                var vState = v.State.Clone();
                var reward = DefaultPolicy(vState, v.CurrentPlayer, depth);
                BackUp(v, reward);
            }
            var bestChild = BestChild(_rootNode);
            return bestChild.FromTo;
        }

        private Node TreePolicy(Node v)
        {
            while (!IsTerminal(v.State))
            {
                if (!IsFullyExpanded(v)) return Expand(v);
                else v = BestChild(v);
            }
            return v;
        }

        private static bool IsFullyExpanded(Node v)
        {
            if (v.Children == null) return false;
            return v.Children.Where(n => n.Visits == 0).ToList().Count == 0;
        }

        private Node Expand(Node v)
        {
            if (v.Children == null)
            {
                v.Children = new List<Node>();
                if (v.CurrentPlayer == _rootNode.CurrentPlayer)
                {
                    var untriedActions = (from piece in v.State.GetPieces(v.CurrentPlayer)
                                          from move in v.State.GetLegalMoves(piece)
                                          select (piece, move)).ToList();
                    foreach (var action in untriedActions)
                    {
                        var s = v.State.SimulateMove(action.piece, action.move);
                        var v_ = new Node(v, s, action, !v.CurrentPlayer);
                        v.State.Undo();
                        v.Children.Add(v_);
                    }
                }
                else
                {
                    var opponent = v.CurrentPlayer == true ? Team.RED : Team.BLACK;
                    var ai = new MoveOrdering(opponent);
                    var minimaxAction = ai.Search(v.State, _depth, null);
                    var s = v.State.SimulateMove(minimaxAction.Item1, minimaxAction.Item2);
                    var v_ = new Node(v, s, minimaxAction, !v.CurrentPlayer);
                    v.State.Undo();
                    v.Children.Add(v_);
                    PositionsEvaluated += ai.PositionsEvaluated;
                }
            }

            var unvisitNodes = v.Children.Where(n => n.Visits == 0).ToList();
            return unvisitNodes[new Random().Next(unvisitNodes.Count)];
        }

        private static Node BestChild(Node v)
        {
            var nodes = v.Children.OrderByDescending(n => UBC(n)).ToList();
            if (nodes.Count == 0) return null;
            return nodes[0];
        }

        private int DefaultPolicy(BoardState vState, bool turn, int depth)
        {
            if (!IsTerminal(vState) && !turn)
            {
                var actions = (from piece in vState.GetPieces(turn)
                               from move in vState.GetLegalMoves(piece)
                               select (piece, move)).ToList();
                var a = actions[new Random().Next(actions.Count)];

                vState.MakeMove(a.piece, a.move);
            }

            ++PositionsEvaluated;
            var reward = BoardEvaluator(vState);
            return reward;
        }

        private void BackUp(Node v, int reward)
        {
            var invertReward = Player == Team.RED ? 1 : -1;
            reward *= invertReward;
            while (v != null)
            {
                v.Visits += 1;
                v.TotalScore += reward;
                v = v.Parent;
            }
        }

        private static double UBC(Node node)
        {
            var w = node.TotalScore;
            var n = node.Visits;
            var v = w / n;
            var N = node.Parent.Visits;
            return v + Math.Sqrt((2 * Math.Log(N)) / n);
        }

        private static bool IsTerminal(BoardState state) => RedWins(state) || BlackWins(state);

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

        private static int BoardEvaluator(BoardState board)
        {
            var score = 0;
            for (int i = 0; i < (int)Rule.ROW; ++i)
                for (int j = 0; j < (int)Rule.COL; ++j)
                    if (board[i, j] != 0)
                        score += board[i, j] + board.PosVal(i, j);

            return score;
        }
    }
}
