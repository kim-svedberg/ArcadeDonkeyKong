using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MarioTest
{
    internal class SuperMario
    {
       
        //Variabler initeraras och deklareras 

        CooldownTimer cooldownTimer = new CooldownTimer();
        enum MarioState { Default, IsHit }
        
        Rectangle hitBox;
        
        Texture2D texture;

        Vector2 position;
        Vector2 destination;
        Vector2 direction;

        private const float speed = 100.0f;
        public static int CurrentLives = 5;
        public static int Score = 0;
        private const int texAdjustment = 40;
        
        bool moving = false;
        bool hit = false;
        public static bool Reunited;
        

        public SuperMario(Texture2D texture, Vector2 position, bool hit)
        {
            this.texture = texture;
            this.position = position;
            this.hit = hit;

        }

        public void Update(GameTime gameTime) 
        {
            hitBox = new Rectangle((int)position.X, (int)position.Y - texAdjustment, texture.Width, texture.Height);
            cooldownTimer.Update(gameTime.ElapsedGameTime.TotalSeconds);

            if (!moving)
            {
                NotMoving();
            }
            else
            {
                Moving(gameTime);
            }
           
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            //spriteBatch.DrawRectangle(hitBox,
            //    Color.Red,
            //    2f,
            //    0);


            spriteBatch.Draw(texture, new Vector2(position.X,
            position.Y - TexManager.marioFrontTex.Height),
            Color.White);

            spriteBatch.Draw(TexManager.coinTex, new Vector2(550, 10), Color.White);
            spriteBatch.DrawString(TexManager.font, "" + Score, new Vector2(570, 10), Color.Gold);

        }

        public void ChangeDirection(Vector2 dir)
        {
            direction = dir;
            Vector2 newDestination = position + direction * TexManager.bridgeTex.Width;

            if (!Game1.GetTileAtPosition(newDestination))
            {
                destination = newDestination;
                moving = true;
            }

        }

        public void Moving(GameTime gameTime)
        {
            position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Vector2.Distance(position, destination) < 1)
            {
                position = destination;
                moving = false;
            }
        }

        public void NotMoving()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                ChangeDirection(new Vector2(-1, 0));

            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                ChangeDirection(new Vector2(1, 0));
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                ChangeDirection(new Vector2(0, 1));
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                ChangeDirection(new Vector2(0, -1));
            }
        }

        public void EnemyCollision(List<Enemy>enemyList, List<DonkeyKong>donkeyKongList)
        {
            foreach(Enemy enemy in enemyList)
            {

                if (hitBox.Intersects(enemy.hitBox))
                {

                    if (hit && cooldownTimer.IsDone())
                    {
                        CurrentLives--;
                        TexManager.marioHitSound.Play();
                    }

                    if (cooldownTimer.IsDone())
                    {
                        cooldownTimer.ResetAndStart(2.0);
                        hit = false;
                    }
                    else if(!cooldownTimer.IsDone())
                    {
                        hit = true;
                    }
                }

                foreach(DonkeyKong donkeyKong in donkeyKongList)
                {
                    if (hitBox.Intersects(donkeyKong.hitBox))
                    {

                        if (hit && cooldownTimer.IsDone())
                        {
                            CurrentLives--;
                            TexManager.marioHitSound.Play();

                        }

                        if (cooldownTimer.IsDone())
                        {
                            cooldownTimer.ResetAndStart(2.0);
                            hit = false;
                        }
                        else if (!cooldownTimer.IsDone())
                        {
                            hit = true;
                        }
                    }
                }
            
            }

        }

        public void WithPauline(List<Pauline> paulineList)
        {
            foreach(Pauline pauline in paulineList)
            {
                if (hitBox.Intersects(pauline.hitBox))
                {
                    Reunited = true;
                }
                else
                {
                    Reunited = false;
                }
            }
        }

        public void GettingItem(List<ScoreItem> scoreItemList)
        {
            foreach(ScoreItem scoreItem in scoreItemList)
            {
                if (hitBox.Intersects(scoreItem.hitBox))
                {
                    Score += 10;
                    scoreItemList.Remove(scoreItem);
                    TexManager.coinSound.Play();
                    break;

                    
                }
            }
        }
    }
}
