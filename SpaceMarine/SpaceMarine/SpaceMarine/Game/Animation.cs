using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace SpaceMarine
{
    public class Animation
    {
        int frameCounter;
        int switchFrame = 60;// default to 60
        bool active;
        Vector2 position, amtofFrames, currentFrame;
        Texture2D Image;
        Rectangle sourceRect;

        //=========================================================================================
        //  --- PROPERTIES ---
        //=========================================================================================
        public Vector2 CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public int FrameWidth
        {
            get { return Image.Width / (int)amtofFrames.X; }
        }
        public int FrameHeight
        {
            get { return Image.Height / (int)amtofFrames.Y; }
        }
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }
        public Texture2D AnimationImage
        {
            set { Image = value; }
        }
        public Rectangle SourceRectangle
        {
            get { return sourceRect; }
        }
        public int FrameSpeed
        {
            set { switchFrame = value; }
        }
        //=========================================================================================
        //  --- BASE FUNCTIONS ---
        //=========================================================================================
        //------------------------
        //  --- Initialize
        //------------------------
        public void Initialize(Vector2 position, Vector2 Frames)
        {
            active = false;
            this.position = position;
            this.amtofFrames = Frames;
        }
        //------------------------
        //  --- Update
        //------------------------
        public void Update(GameTime gameTime)
        {
            if (active)
                frameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            else
                frameCounter = 0;
            if (frameCounter > switchFrame)
            {
                frameCounter = 0;
                currentFrame.X += FrameWidth;
                if (currentFrame.X >= Image.Width)
                    currentFrame.X = 0;
            }
            sourceRect = new Rectangle((int)currentFrame.X, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }
        //------------------------
        //  --- Draw
        //------------------------
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, position, sourceRect, Color.White);
        }
        //=========================================================================================
    }
}
