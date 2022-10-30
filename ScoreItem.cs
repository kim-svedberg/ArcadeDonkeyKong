using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;
using MonoGame.Extended;
using MonoGame.Extended.Timers;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace MarioTest
{
    internal class ScoreItem
    {
        Texture2D texture;
        Vector2 position;
        public Rectangle hitBox;

        public ScoreItem(Texture2D texture, Vector2 position, Rectangle hitBox)
        {
            this.texture = texture;
            this.position = position;
            this.hitBox = hitBox;
        }   

        public void Update()
        {
            hitBox.Location = new Vector2(position.X, position.Y - 20).ToPoint();
            Game1.GetTileAtPosition(position);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(position.X,
                position.Y - TexManager.coinTex.Height),
                Color.White);


            //spriteBatch.DrawRectangle(hitBox,
            //   Color.Red,
            //   2f,
            //   0);
        }
    }
}
