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
    class Cursor : GameObject
    {
        Vector2 position;
        Texture2D spriteIndex;
        string spriteName;

        MouseState mouse;
        
        //=========================================================================================
        //  --- CONSTRUCTOR(S) ---
        //=========================================================================================
        public Cursor()
        {            
        }
        //=========================================================================================
        //  --- BASE FUNCTIONS ---
        //=========================================================================================
        //------------------------
        //  --- Initialize
        //------------------------
        public override void Initialize()
        {
            active = true;
            mouse = Mouse.GetState();
            position = new Vector2(mouse.X, mouse.Y);
            spriteName = "Cursor";
        }
        //------------------------
        //  --- Load Content
        //------------------------
        public override void LoadContent(ContentManager content)
        {
            spriteIndex = content.Load<Texture2D>("Sprites\\" + this.spriteName);
        }
        //------------------------
        //  --- Update
        //------------------------
        public override void Update(GameTime gameTime)
        {
            mouse = Mouse.GetState();
            position = new Vector2(mouse.X, mouse.Y);
        }
        //------------------------
        //  --- Draw
        //------------------------
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!active) return;
            Vector2 center = new Vector2(position.X - spriteIndex.Width / 2, position.Y - spriteIndex.Height);
            spriteBatch.Draw(spriteIndex, center, null, Color.White);            
        }
        //=========================================================================================
    }
}
