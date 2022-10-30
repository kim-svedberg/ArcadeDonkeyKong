using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;
using System.Collections.Generic;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using System.Threading;
using Microsoft.Xna.Framework.Media;

namespace MarioTest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        
        enum GameState { Menu, InGame, GameOver, Win }
        GameState state;

        static Tile[,] tileArray;
        Texture2D[] gameStateTex;

        SuperMario superMario;
        Enemy enemy;
        Pauline pauline;
        DonkeyKong donkeyKong;

        StreamReader reader;

        List<string> strings = new List<string>();
        List<Enemy> enemyList = new List<Enemy>();
        List<Pauline> paulineList = new List<Pauline>();
        List<DonkeyKong> donkeyKongList = new List<DonkeyKong>();
        List<ScoreItem> scoreItemList = new List<ScoreItem>();
        
        Random rnd = new Random();

        Vector2 enemyPos;
        Vector2 paulinePos;
        Vector2 donkeyKongPos;
        Vector2 scoreItemPos;
        Vector2 heartPos;

        bool hit = false;

        private const int texAdjustment = 40;
        private const int donkeyKongTexAdjustment = 80;
        int spaceBetweenHearts;




        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 595;
            graphics.PreferredBackBufferHeight = 440;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            state = GameState.Menu;
            //state = GameState.InGame;
            //state = GameState.GameOver;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Laddar in texturer, ljudfiler mm från TexManager klassen
            TexManager.LoadTextures(Content);

            //Hanterar musik
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(TexManager.themeSong);

            heartPos = new Vector2(10, 10);
            spaceBetweenHearts = 40;

            ReadFromTextFile();

            //Ger bakgrunder till gamestates
            TexBgForStates();


            //Kartan ritas ut
            Map();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

            switch(state)
            {
                case GameState.Menu: //Om spelaren trycker Enter --> man kommer in i spelet
                    {
                        if(Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            state = GameState.InGame;
                        }
                    }
                    break;

                case GameState.InGame:
                    {
                        superMario.Update(gameTime);
                        foreach (Enemy enemy in enemyList)
                        {
                            enemy.Update(gameTime);
                        }

                        superMario.GettingItem(scoreItemList);
                        foreach (ScoreItem scoreItem in scoreItemList)
                        {
                            scoreItem.Update();
                        }

                        superMario.EnemyCollision(enemyList, donkeyKongList);

                        if(SuperMario.CurrentLives == 0)
                        {
                            TexManager.donkeyKongSound.Play();
                            state = GameState.GameOver;
                        }

                       superMario.WithPauline(paulineList);
                       if (SuperMario.Reunited == true)
                       {
                          TexManager.winSound.Play();
                          state = GameState.Win;
                       }

                        

                    }
                    break;

                case GameState.GameOver:
                    {
                        MediaPlayer.Stop();

                    }
                    break;

                case GameState.Win:
                    break;

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(gameStateTex[(int)state],
                 new Vector2(0, 0),
                 null, Color.White);
            
            if(state == GameState.InGame)
            {

                GraphicsDevice.Clear(Color.Black);
                foreach (Tile tile in tileArray)
                {
                    tile.Draw(spriteBatch);

                }
           
                foreach (Enemy enemy in enemyList)
                {
                    enemy.Draw(spriteBatch);

                }

                foreach(ScoreItem scoreItem in scoreItemList)
                {
                    scoreItem.Draw(spriteBatch);

                }

                superMario.Draw(spriteBatch);
                pauline.Draw(spriteBatch);
                donkeyKong.Draw(spriteBatch);
                for (int i = 0; i < SuperMario.CurrentLives; i++)
                {
                    spriteBatch.Draw(TexManager.heartTex, new Vector2(heartPos.X + i * spaceBetweenHearts, heartPos.Y), Color.White);
                }

            }


            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void ReadFromTextFile()
        {
            reader = new StreamReader(@"C:\Users\kimas\OneDrive\Desktop\Projects_and_Solutions\MarioTest\Content\map.txt.txt");
            while (!reader.EndOfStream)
            {
                strings.Add(reader.ReadLine());
            }
            reader.Close();
        }

        public static bool GetTileAtPosition(Vector2 vec)
        {
            return tileArray[(int)vec.X / TexManager.bridgeTex.Width, (int)vec.Y / TexManager.bridgeTex.Height].emptySpace;
        }

        public void Map()
        {
            tileArray = new Tile[strings[0].Length, strings.Count];

            for (int i = 0; i < tileArray.GetLength(0); i++)
            {
                for (int j = 0; j < tileArray.GetLength(1); j++)
                {
                    if (strings[j][i] == 'B')
                    {
                        tileArray[i, j] = new Tile(TexManager.bridgeTex, new Vector2(TexManager.bridgeTex.Width * i, TexManager.bridgeTex.Height * j), false);
                    }
                    else if (strings[j][i] == '-')
                    {
                        tileArray[i, j] = new Tile(TexManager.emptyTex, new Vector2(TexManager.emptyTex.Width * i, TexManager.emptyTex.Height * j), true);
                    }
                    else if (strings[j][i] == 'P')
                    {
                        tileArray[i, j] = new Tile(TexManager.bridgeTex, new Vector2(TexManager.bridgeTex.Width * i, TexManager.bridgeTex.Height * j), false);
                        pauline = new Pauline(TexManager.paulineTex, paulinePos = new Vector2(TexManager.bridgeTex.Width * i, TexManager.bridgeTex.Height * j), new Rectangle((int)paulinePos.X, (int)paulinePos.Y - texAdjustment, TexManager.
                            paulineTex.Width, TexManager.paulineTex.Height));
                        paulineList.Add(pauline);
                    }
                    else if (strings[j][i] == 'M')
                    {
                        tileArray[i, j] = new Tile(TexManager.bridgeTex, new Vector2(TexManager.bridgeTex.Width * i, TexManager.bridgeTex.Height * j), false);
                        superMario = new SuperMario(TexManager.marioFrontTex, new Vector2(TexManager.bridgeTex.Width * i, TexManager.bridgeTex.Height * j), hit);
                    }
                    else if (strings[j][i] == 'L')
                    {
                        tileArray[i, j] = new Tile(TexManager.ladderTex, new Vector2(TexManager.ladderTex.Width * i, TexManager.ladderTex.Height * j), false);
                    }
                    else if (strings[j][i] == 'Y')
                    {
                        tileArray[i, j] = new Tile(TexManager.bridgeLadderTex, new Vector2(TexManager.bridgeLadderTex.Width * i, TexManager.bridgeLadderTex.Height * j), false);
                    }
                    else if (strings[j][i] == 'E')
                    {
                        tileArray[i, j] = new Tile(TexManager.bridgeTex, new Vector2(TexManager.bridgeTex.Width * i, TexManager.bridgeTex.Height * j), false);
                        float enemySpeed = (float)rnd.NextDouble() * 90;
                        enemy = new Enemy(TexManager.enemyTex, enemyPos = new Vector2(TexManager.bridgeTex.Width * i, TexManager.bridgeTex.Height * j), new Rectangle((int)enemyPos.X, (int)enemyPos.Y - texAdjustment, TexManager.
                            enemyTex.Width, TexManager.enemyTex.Height), enemySpeed);
                        enemyList.Add(enemy);
                    }
                    else if (strings[j][i] == 'D')
                    {
                        tileArray[i, j] = new Tile(TexManager.bridgeTex, new Vector2(TexManager.bridgeTex.Width * i, TexManager.bridgeTex.Height * j), false);
                        donkeyKong = new DonkeyKong(TexManager.donkeyKongTex, donkeyKongPos = new Vector2(TexManager.bridgeTex.Width * i, TexManager.bridgeTex.Height * j), new Rectangle((int)donkeyKongPos.X, (int)donkeyKongPos.Y - donkeyKongTexAdjustment, TexManager.
                            donkeyKongTex.Width, TexManager.donkeyKongTex.Height));
                        donkeyKongList.Add(donkeyKong);
                    }
                    else if (strings[j][i] == 'c')
                    {
                        tileArray[i, j] = new Tile(TexManager.bridgeTex, new Vector2(TexManager.bridgeTex.Width * i, TexManager.bridgeTex.Height * j), false);
                        scoreItemList.Add(new ScoreItem(TexManager.coinTex, scoreItemPos = new Vector2(TexManager.bridgeTex.Width * i, TexManager.bridgeTex.Height * j), new Rectangle((int)scoreItemPos.X, (int)scoreItemPos.Y - 20, TexManager.
                            coinTex.Width, TexManager.coinTex.Height)));

                    }



                }
            }
        }

        public void TexBgForStates()
        {
            gameStateTex = new Texture2D[4];
            gameStateTex[(int)GameState.InGame] = TexManager.emptyTex;
            gameStateTex[(int)GameState.Menu] = TexManager.menuScreenTex;
            gameStateTex[(int)GameState.GameOver] = TexManager.gameOverScreenTex;
            gameStateTex[(int)GameState.Win] = TexManager.winScreenTex;
        }
      
        
       
    }
}