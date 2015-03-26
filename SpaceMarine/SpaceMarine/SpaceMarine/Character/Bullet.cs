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
    class Bullet : GameObject
    {
        Vector2 position;
        string spriteName;
        Texture2D spriteIndex;
        float rotation;
        float speed;

        //=========================================================================================
        //  --- CONSTRUCTOR(S) ---
        //=========================================================================================
        public Bullet(Vector2 pos)
        {
            active = true;
            position = pos;
            rotation = 0f;
            speed = 0f;
            spriteName = "bullet";
        }
        //=========================================================================================
        //  --- PROPERTIES ---
        //=========================================================================================
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        //=========================================================================================
        //  --- BASE FUNCTIONS ---
        //=========================================================================================
        //------------------------
        //  --- LoadContent
        //------------------------
        public override void LoadContent(ContentManager content)
        {
            spriteIndex = content.Load<Texture2D>("sprites\\" + this.spriteName);
            area = new Rectangle((int)position.X, (int)position.Y, spriteIndex.Width, spriteIndex.Height);
        }
        //------------------------
        //  --- Update
        //------------------------
        public override void Update(GameTime gameTime)
        {
            if (!active) return;

            // if out of game window
            if (position.X < 0 || position.Y < 0 || position.X > Game1.gameWindow.Width || position.Y > Game1.gameWindow.Height)
                this.Destroy();

            UpdateArea();
            Type colObj = Collision(position, area);
            if (colObj != null && colObj != typeof(Bullet))
            {
                this.Destroy();
            }

            pushTo(speed, rotation);
        }
        //------------------------
        //  --- Draw
        //------------------------
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!active) return;

            Vector2 center = new Vector2(spriteIndex.Width / 2, spriteIndex.Height / 2);

            spriteBatch.Draw(spriteIndex, position, null, Color.White, MathHelper.ToRadians(rotation), center, 1f, SpriteEffects.None, 0);
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
        //------------------------
        //  --- Push To Point
        //------------------------
        public virtual void pushTo(float pix, float dir)
        {
            float newX = (float)Math.Cos(MathHelper.ToRadians(dir));
            float newY = (float)Math.Sin(MathHelper.ToRadians(dir));
            position.X += pix * newX;
            position.Y += pix * newY;
        }
        //------------------------
        //  --- Collision
        //------------------------
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
            for (int i = 0; i < WorldItems.mobList.Count(); i++)
            {
                Mob enemy = WorldItems.mobList[i];
                if (enemy.area.Intersects(area))
                {
                    collisionObj = enemy.GetType();
                    enemy.Damage(10);
                    return collisionObj;
                }
            }
            return null;
        }
        //=========================================================================================
    }
}
