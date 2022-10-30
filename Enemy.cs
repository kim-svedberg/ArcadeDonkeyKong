using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;
using MonoGame.Extended;

namespace MarioTest
{
    internal class Enemy
    {
        Texture2D texture;
        Vector2 position;
        Vector2 direction;
        Vector2 destination;
        Vector2 newDestination;
        public Rectangle hitBox;
        float speed = 100.0f;
        bool moving = false;
        bool goingLeft = false;

        public Enemy(Texture2D texture, Vector2 position, Rectangle hitBox, float speed)
        {
            this.texture = texture;
            this.position = position;
            this.hitBox = hitBox;
            this.speed = speed;

        }


        public void Update(GameTime gameTime)
        {
            hitBox.Location = new Vector2(position.X, position.Y - 40).ToPoint(); 

            if (!moving)
            {
                if (!goingLeft)
                {
                    ChangeDirection(new Vector2(1,0));

                }
                else if (goingLeft)
                {
                    ChangeDirection(new Vector2(-1,0));
                }

                if(Game1.GetTileAtPosition(newDestination) && !goingLeft)
                {
                    goingLeft = true;
                }
                else if(Game1.GetTileAtPosition(newDestination) && goingLeft)
                {
                    goingLeft = false;
                }


            }

            else
            {
                position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;


                if (Vector2.Distance(position, destination) < 1)
                {
                    position = destination;
                    moving = false;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(position.X,
                position.Y - TexManager.enemyTex.Height),
                Color.White);


            //spriteBatch.DrawRectangle(hitBox,
            //   Color.Red,
            //   2f,
            //   0);
        }
        public void ChangeDirection(Vector2 dir)
        {
            direction = dir;
            newDestination = position + direction * TexManager.bridgeTex.Width;

            if (!Game1.GetTileAtPosition(newDestination))
            {
                destination = newDestination;
                moving = true;
            }

        }
    }
}
