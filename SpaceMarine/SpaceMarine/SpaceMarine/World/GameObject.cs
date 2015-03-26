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
    class GameObject
    {
        public Rectangle area;
        public bool solid;
        public static bool active = false;
        //=========================================================================================
        //  --- PROPERTIES ---
        //=========================================================================================
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }
        //=========================================================================================
        //  --- BASE FUNCTIONS ---
        //=========================================================================================
        //------------------------
        //  --- Initialize
        //------------------------
        public virtual void Initialize()
        { }
        //------------------------
        //  --- Load Content
        //------------------------
        public virtual void LoadContent(ContentManager content)
        { }
        //------------------------
        //  --- Update
        //------------------------
        public virtual void Update(GameTime gameTime)
        { }
        //------------------------
        //  --- Draw
        //------------------------
        public virtual void Draw(SpriteBatch spriteBatch)
        { }
        //=========================================================================================
        //  --- MOVEMENT FUNCTIONS ---
        //=========================================================================================
        //----------------------------------
        //  --- Check For Object Collisions
        //----------------------------------
        public virtual Type Collision(Vector2 objPos, Rectangle area)
        {
            Rectangle nextArea = new Rectangle(area.X, area.Y, area.Width, area.Height); 
            nextArea.X += (int)objPos.X;
            nextArea.Y += (int)objPos.Y;

            Type collisionObj;

            foreach (GameObject obj in WorldItems.objList)
            {
                if (obj.area.Intersects(nextArea) && obj.solid == true)
                    {
                        collisionObj = obj.GetType();
                        return collisionObj;
                    }
            }
            return null;
        }
        //=========================================================================================
        //  --- CLASS SPECIFIC FUNCTIONS ---
        //=========================================================================================
        //-----------------------------------------------
        //  --- Create Object (Add to World Items List)
        //-----------------------------------------------
        public virtual void Create()
        {
            WorldItems.toCreateObjList.Add(this);
        }
        //-----------------------------------------------
        //  --- Destroy Object (Remove from World Items)
        //-----------------------------------------------
        public virtual void Destroy()
        {
            area = new Rectangle(0, 0, 0, 0);
            WorldItems.objList.Remove(this);
        }
        //=========================================================================================
    }
}
