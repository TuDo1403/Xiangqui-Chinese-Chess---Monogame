﻿using ChineseChess.Properties;
using ChineseChess.Source.GameRule;
using ChineseChess.Source.Helper;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess.Source.GameObjects.Chess
{
    public class BoardState
    {
        private int[][] _boardState = new int[10][];
        private readonly Stack<int[][]> _undoStates = new Stack<int[][]>();

        private readonly static int[][] _redCannonPosVal = new int[10][];
        private readonly static int[][] _redChariotPosVal = new int[10][];
        private readonly static int[][] _redHorsePosVal = new int[10][];
        private readonly static int[][] _redSoldierPosVal = new int[10][];

        private readonly static int[][] _blackCannonPosVal = new int[10][];
        private readonly static int[][] _blackChariotPosVal = new int[10][];
        private readonly static int[][] _blackHorsePosVal = new int[10][];
        private readonly static int[][] _blackSoldierPosVal = new int[10][];


        public BoardState()
        {
            LoadBoard(Resources.MatrixBoard, _boardState);
            LoadPosValBoards();
        }

        private void LoadPosValBoards()
        {
            LoadBoard(Resources.R_CannonPosVal, _redCannonPosVal);
            LoadBoard(Resources.R_ChariotPosVal, _redChariotPosVal);
            LoadBoard(Resources.R_HorsePosVal, _redHorsePosVal);
            LoadBoard(Resources.R_SoldierPosVal, _redSoldierPosVal);

            LoadBoard(Resources.B_CannonPosVal, _blackCannonPosVal);
            LoadBoard(Resources.B_ChariotPosVal, _blackChariotPosVal);
            LoadBoard(Resources.B_HorsePosVal, _blackHorsePosVal);
            LoadBoard(Resources.B_SoldierPosVal, _blackSoldierPosVal);
        }

        private static void LoadBoard(string boardStateData, int[][] boardState)
        {
            var board = boardStateData.Split('\n');
            var idx = 0;
            foreach (var line in board)
            {
                var row = line.Replace("\r", "").Split(' ').ToArray();
                boardState[idx++] = Array.ConvertAll(row, s => int.Parse(s));
            }
        }

        public int this[int idx1, int idx2]
        {
            get => _boardState[idx1][idx2];
            set => _boardState[idx1][idx2] = value;
        }

        public void MakeMove(Point oldIdx, Point newIdx)
        {
            var pieceVal = _boardState[oldIdx.Y][oldIdx.X];
            _boardState[oldIdx.Y][oldIdx.X] = 0;
            _boardState[newIdx.Y][newIdx.X] = pieceVal;
        }

        public void SimulateMove(Point oldIdx, Point newIdx)
        {
            _undoStates.Push(Clone(_boardState));
            MakeMove(oldIdx, newIdx);
        }

        public void Undo() => _boardState = _undoStates.Pop();


        private static int[][] Clone(int[][] board)
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

        public int PosVal(Point idx)
        {
            var pieceVal = _boardState[idx.Y][idx.X];
            if (pieceVal == (int)Pieces.B_Cannon)
                return _blackCannonPosVal[idx.Y][idx.X];
            if (pieceVal == (int)Pieces.B_Chariot)
                return _blackChariotPosVal[idx.Y][idx.X];
            if (pieceVal == (int)Pieces.B_Soldier)
                return _blackSoldierPosVal[idx.Y][idx.X];
            if (pieceVal == (int)Pieces.B_Horse)
                return _blackHorsePosVal[idx.Y][idx.X];

            if (pieceVal == (int)Pieces.R_Cannon)
                return _redCannonPosVal[idx.Y][idx.X];
            if (pieceVal == (int)Pieces.R_Chariot)
                return _redChariotPosVal[idx.Y][idx.X];
            if (pieceVal == (int)Pieces.R_Soldier)
                return _redSoldierPosVal[idx.Y][idx.X];
            if (pieceVal == (int)Pieces.R_Horse)
                return _redHorsePosVal[idx.Y][idx.X];

            return 0;
        }


        public List<Point> GetLegalMoves(Point pieceIdx)
        {
            var key = Math.Abs(_boardState[pieceIdx.Y][pieceIdx.X]);
            return PieceMoveFactory.CreatePieceMove(key, pieceIdx)
                                   .FindLegalMoves(this);
        }


        public List<Point> GetPieces(bool maxPlayer, bool moveOrder=false)
        {
            var pieces = new List<Point>();
            for (int i = 0; i < 10; ++i)
                for (int j = 0; j < 9; ++j)
                    if (maxPlayer == true && IsTeamRed(i, j) ||
                        maxPlayer == false && IsTeamBlack(i, j))
                        pieces.Add(new Point(j, i));

            // Simple move ordering
            if (moveOrder)
            {
                if (maxPlayer == false)
                    pieces = pieces.OrderBy(p => _boardState[p.Y][p.X])
                                   .ToList();
                else
                    pieces = pieces.OrderByDescending(p => _boardState[p.Y][p.X])
                                   .ToList();
            }
                         
            return pieces;
        }

        private bool IsTeamBlack(int i, int j) => _boardState[i][j] < 0;

        private bool IsTeamRed(int i, int j) => _boardState[i][j] > 0;
    }
}
