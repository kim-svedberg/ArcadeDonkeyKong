using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;
using Microsoft.Xna.Framework.Content;
using System.Drawing.Drawing2D;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace MarioTest
{
    internal class TexManager
    {
        public static Texture2D bridgeTex; //Tile
        public static Texture2D bridgeLadderTex;
        public static Texture2D ladderTex;
        public static Texture2D emptyTex;
        public static Texture2D marioFrontTex;
        public static Texture2D marioBackTex;
        public static Texture2D paulineTex;
        public static Texture2D enemyTex;
        public static Texture2D donkeyKongTex;
        public static Texture2D heartTex;
        public static Texture2D menuScreenTex;
        public static Texture2D gameOverScreenTex;
        public static Texture2D winScreenTex;
        public static Texture2D coinTex;
       
        public static Song themeSong;
        public static SoundEffect coinSound;
        public static SoundEffect winSound;
        public static SoundEffect marioHitSound;
        public static SoundEffect donkeyKongSound;

        public static SpriteFont font;

        public static void LoadTextures(ContentManager content)
        {
            bridgeTex = content.Load<Texture2D>("bridge");
            bridgeLadderTex = content.Load<Texture2D>("bridgeLadder");
            ladderTex = content.Load<Texture2D>("ladder");
            emptyTex = content.Load<Texture2D>("empty");
            marioFrontTex = content.Load<Texture2D>("SuperMarioFront");
            marioBackTex = content.Load<Texture2D>("SuperMarioBack");
            donkeyKongTex = content.Load<Texture2D>("DonkeyKongis_");
            paulineTex = content.Load<Texture2D>("pauline");
            enemyTex = content.Load<Texture2D>("enemy");
            heartTex = content.Load<Texture2D>("pixelheart");
            menuScreenTex = content.Load<Texture2D>("MenuScreen");
            gameOverScreenTex = content.Load<Texture2D>("GameOver");
            winScreenTex = content.Load<Texture2D>("winscreen");
            coinTex = content.Load<Texture2D>("coin_2");
          
            themeSong = content.Load<Song>("themeSong");
            coinSound = content.Load<SoundEffect>("coinSound");
            winSound = content.Load<SoundEffect>("winSound");
            marioHitSound = content.Load<SoundEffect>("marioHitSound");
            donkeyKongSound = content.Load<SoundEffect>("donkeySound");


            font = content.Load<SpriteFont>("font");





        }
    }
}
