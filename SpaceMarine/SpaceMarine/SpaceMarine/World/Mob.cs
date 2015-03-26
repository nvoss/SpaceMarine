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
    class Mob : GameObject
    {
        Animation mobAnimation = new Animation();
        Vector2 tempCurrentFrame;
        Rectangle sourceRectangle;
        //-----------------------------
        Vector2 position;
        string spriteName;
        Texture2D spriteIndex;
        float rotation;
        float speed;
        int health;
        //=========================================================================================
        //  --- CONSTRUCTOR(S) ---
        //=========================================================================================
        public Mob(Vector2 pos)
        {
            position = pos;
            spriteName = "balloon";
            solid = true;
            speed = 2;
            health = 10;
        }
        //=========================================================================================
        //  --- BASE FUNCTIONS ---
        //=========================================================================================
        //------------------------
        //  --- Initialize
        //------------------------
        public override void Initialize()
        { 
            mobAnimation.Initialize(position, new Vector2(4, 1));
            tempCurrentFrame = Vector2.Zero;
            tempCurrentFrame.Y = 0;
            mobAnimation.CurrentFrame = tempCurrentFrame;
            mobAnimation.FrameSpeed = 40; // the higher the FrameSpeed, the slower the animation (60)
        }
        //------------------------
        //  --- Load Content
        //------------------------
        public override void LoadContent(ContentManager content)
        {
            spriteIndex = content.Load<Texture2D>("sprites\\" + this.spriteName);
            area = new Rectangle(0, 0, spriteIndex.Width/4, spriteIndex.Height);
            mobAnimation.AnimationImage = spriteIndex;
        }
        //------------------------
        //  --- Update
        //------------------------
        public override void Update(GameTime gameTime)
        {
            if (!active) return;


            UpdateArea();
            rotation = point_direction(position.X, position.Y, CharacterBase.BodyPosition.X-475, CharacterBase.BodyPosition.Y-100);
            pushTo(speed, rotation);


            if (health <= 0)
            {
                mobAnimation.Position = position;
                mobAnimation.Active = true;
                mobAnimation.Update(gameTime);
                sourceRectangle = mobAnimation.SourceRectangle;
                if (mobAnimation.CurrentFrame.X == 150)
                  this.Destroy();
            }
            else
            {
                mobAnimation.Active = false;
                mobAnimation.Position = position;
                mobAnimation.Update(gameTime);
                sourceRectangle = mobAnimation.SourceRectangle;
            }
        }
        //------------------------
        //  --- Draw
        //------------------------
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!active) return;
            
            Vector2 center = new Vector2(spriteIndex.Width / 8, spriteIndex.Height / 2);
            spriteBatch.Draw(spriteIndex, new Vector2(position.X-75, position.Y), sourceRectangle, Color.White, MathHelper.ToRadians(rotation), center, 1f, SpriteEffects.None, 0);
            

        }
        //=========================================================================================
        //  --- MOVEMENT FUNCTIONS ---
        //=========================================================================================
        //--------------------------------------
        //  --- Point Mob in Specific Direction
        //--------------------------------------
        private float point_direction(float x, float y, float dirx, float diry)
        {
            float adj = x - dirx;
            float opp = y - diry;
            float res = MathHelper.ToDegrees((float)Math.Atan2(opp, adj));
            res = (res - 180) % 360;

            if (res < 0)
                res += 360;

            return res;
        }
        //------------------------
        //  --- Push To Point
        //------------------------
        public virtual void pushTo(float pix, float dir)
        {
            float newX = (float)Math.Cos(MathHelper.ToRadians(dir));
            float newY = (float)Math.Sin(MathHelper.ToRadians(dir));

            Vector2 newPosition = new Vector2();
            newPosition.X = position.X + (pix * newX);
            newPosition.Y = position.Y + (pix * newY);

            if (Collision(newPosition, area) == null || Collision(newPosition, area) == typeof(Mob))
            {
                position.X += pix * newX;
                position.Y += pix * newY;
            }
        }
        //=========================================================================================
        //  --- CLASS SPECIFIC FUNCTIONS ---
        //=========================================================================================
        //------------------------
        //  --- Update Area
        //------------------------
        public void UpdateArea()
        {
            area.X = (int)position.X - (spriteIndex.Width / 2);
            area.Y = (int)position.Y - (spriteIndex.Height / 2);
        }
        //--------------------------------------
        //  --- Damage Mob / Reduce health
        //--------------------------------------       
        public void Damage(int dmg)
        {
            health = health -dmg;
        }
        //----------------------------------
        //  --- Check For Object Collisions
        //----------------------------------
        public override Type Collision(Vector2 objPos, Rectangle area)
        {
            Type collisionObj;

            foreach (GameObject obj in WorldItems.objList)
            {
                if (obj.area.Intersects(area) && obj.solid == true)
                {
                    collisionObj = obj.GetType();
                    return collisionObj;
                }
            }
            return null;
        }
        //-----------------------------------------------
        //  --- Destroy Object (Remove from World Items)
        //-----------------------------------------------
        public override void Destroy()
        {
            area = new Rectangle(0, 0, 0, 0);
            WorldItems.mobList.Remove(this);
        }
        //=========================================================================================
    }
}
