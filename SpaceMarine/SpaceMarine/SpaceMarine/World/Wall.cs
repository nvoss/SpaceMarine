using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceMarine 
{
    class Wall : GameObject
    {
        Vector2 position;
        Texture2D spriteIndex;
        string spriteName;

        //=========================================================================================
        //  --- CONSTRUCTOR(S) ---
        //=========================================================================================
        public Wall(Vector2 pos)
        {
            position = pos;
        }
        //=========================================================================================
        //  --- PROPERTIES ---
        //=========================================================================================
        public bool Visible
        {
            get { return active; }
        }
        //=========================================================================================
        //  --- BASE FUNCTIONS ---
        //=========================================================================================
        //------------------------
        //  --- Initialize
        //------------------------
        public override void Initialize()
        {
            spriteName = "metalblock";
            solid = true;
            active = true;
        }
        //------------------------
        //  --- Load Content
        //------------------------
        public override void LoadContent(ContentManager content)
        {
            spriteIndex = content.Load<Texture2D>("Sprites\\" + this.spriteName);
            area = new Rectangle((int)position.X, (int)position.Y, spriteIndex.Width, spriteIndex.Height);
        }
        //------------------------
        //  --- Draw
        //------------------------
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!active) return;
            spriteBatch.Draw(spriteIndex, position, null, Color.White);
        }
        //=========================================================================================
    }
}
