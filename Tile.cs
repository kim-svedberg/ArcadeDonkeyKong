using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;

namespace MarioTest
{
    internal class Tile
    {
        Texture2D texture;
        public Vector2 position;
        public bool emptySpace;




        public Tile(Texture2D texture, Vector2 position, bool emptySpace)
        {
            this.texture = texture;
            this.position = position;
            this.emptySpace = emptySpace;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);

        }
    }
}
