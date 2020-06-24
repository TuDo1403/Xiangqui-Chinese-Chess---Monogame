using ChineseChess.Source.GameObjects.Chess;
using ChineseChess.Source.GameRule;
using ChineseChess.Source.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChineseChess.Source.Players
{
    public class Player
    {
        public List<Piece> Pieces { get; } = new List<Piece>();

        public bool IsDraw { get; set; }

        public PlayerTag Tag { get; protected set; }

        public event EventHandler GameOver;


        public void AddPiece(Piece piece) => Pieces.Add(piece);

        public void RemovePiece(ChessBoard board, Point index)
        {
            var piece = Pieces.Where(p => p.Index == index)
                              .SingleOrDefault();
            if (piece != null)
            {
                piece.RemoveBoardUpdatedEvent(board);
            }

            Pieces.RemoveAll(p => p.Index == index);
        }


        protected void OnGameOver() => GameOver?.Invoke(this, EventArgs.Empty);

        public virtual void Update(MouseState mouseState, GameTime gameTime) { }

        public virtual void Update(BoardState board, GameTime gameTime) { }

        public virtual void DrawPieces(SpriteBatch spriteBatch)
        {
            foreach (var piece in Pieces)
            {
                piece.Draw(spriteBatch);
            }
        }
    }
}
