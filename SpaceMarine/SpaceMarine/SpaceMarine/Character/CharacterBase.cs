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
    class CharacterBase : GameObject
    {
        Animation characterAnimation = new Animation();
        Vector2 tempCurrentFrame;
        Rectangle sourceRectangle;
        // base vars --------------------------------
         Vector2 position;
         float rotation;
         float speed;
         float fallspd;
         float scale = 1.0f;
         bool jumping = false;
         int jumpStart = 0; // controller for jumping
         int currTime = 0; // jump controller
        //-------------------------------------------
        // texture vars -----------------------------
        //-----[HEAD]----------
         Texture2D headIndex;
         string headImg = "xMarine-Head";
         Vector2 headPos;
         float headRot = 0.0f;
         float newheadRot = 0.0f;
         float prevheadRot = 0.0f;
         public static Rectangle headArea;
        //-----[BODY]----------
         Texture2D bodyIndex;
         string bodyImg = "Marine-Body";
         public static Vector2 bodyPos;
         float bodyRot = 0.0f;
         public static Rectangle bodyArea;
        //-----[GUNS]----------
         Texture2D gunIndex;
         string gunImg = "Marine-Gun";
         Vector2 gunPos;
         float gunRot = 0.0f;
         float newgunRot = 0.0f;
         float prevgunRot = 0.0f;
         public static Rectangle gunArea;
        //-------------------------------------------
        // control vars -----------------------------
        KeyboardState keyboard;
        KeyboardState prevkeyboard;
        MouseState mouse;
        MouseState prevmouse;
        bool flip;
        // Shooting------------
        float bulletspd = 20;
        const int maxAmmo = 100;
        int ammo = 100;
        int rate = 10; // 60fps, so rate is 6 per sec
        int firingTimer = 0;
        public Vector2 gunBarrel;
        //---- Reloading ------
        int reloadTimer = 0;
        int reloadTime = 60 * 2; // reload takes 2 secs
        bool reloading = false;
        //--- Jet Pack Fuel -----
        int fuel = 500;
        int maxFuel = 500;
        //-------------------------------------------

        //=========================================================================================
        //  --- CONSTRUCTOR(S) ---
        //=========================================================================================
        public CharacterBase()
        {            
        }
        //=========================================================================================
        //  --- PROPERTIES ---
        //=========================================================================================
        public Rectangle HeadArea
        {
            get { return headArea; }
        }
        public Rectangle BodyArea
        {
            get { return bodyArea; }
        }
        public static Vector2 BodyPosition
        {
            get { return bodyPos; }
        }
        public Rectangle GunArea
        {
            get { return gunArea; }
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
            position = new Vector2(100, 200);
            solid = false;

            headPos = new Vector2(position.X, position.Y + 90 /*30*/);
            bodyPos = new Vector2(position.X + 492, position.Y + 320 /*190*/);
            gunPos = new Vector2(position.X + 10, position.Y + 145 /*85*/);
            speed = 4;
            fallspd = 6;
            characterAnimation.Initialize(bodyPos, new Vector2(9, 3));
            tempCurrentFrame = Vector2.Zero;
            tempCurrentFrame.Y = 0;
            characterAnimation.CurrentFrame = tempCurrentFrame;
            characterAnimation.FrameSpeed = 75; // the higher the FrameSpeed, the slower the animation (75)
        }
        //------------------------
        //  --- Load Content
        //------------------------
        public override void LoadContent(ContentManager content)
        {
            headIndex = content.Load<Texture2D>("sprites\\" + this.headImg);
            headArea = new Rectangle(0, 0, headIndex.Width, headIndex.Height);

            bodyIndex = content.Load<Texture2D>("sprites\\" + this.bodyImg);
            bodyArea = new Rectangle(0, 0, bodyIndex.Width/9, bodyIndex.Height/3);
            characterAnimation.AnimationImage = bodyIndex;

            gunIndex = content.Load<Texture2D>("sprites\\" + this.gunImg);
            gunArea = new Rectangle(0, 0, gunIndex.Width, gunIndex.Height);
        }
        //------------------------
        //  --- Update
        //------------------------
        public override void Update(GameTime gameTime)
        {
            if (!active) return;
            keyboard = Keyboard.GetState();
            prevmouse = mouse;
            mouse = Mouse.GetState();

            if (!jumping)
                checkMoveDirection(gameTime);
            else
                jump(gameTime);


            updateAnimation(gameTime);
            rotateCharacter();
                                    
            firingTimer++;
            if (mouse.LeftButton == ButtonState.Pressed && !reloading)
                checkShooting();
            if (keyboard.IsKeyDown(Keys.R))
                reloading = true;
            checkReload();

            prevkeyboard = keyboard;
            prevmouse = mouse;
            UpdateArea();
        }
        //------------------------
        //  --- Draw
        //------------------------
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!active) return; 

            // [HUD] ------------------------------------------------------------------------------------------------------------
            spriteBatch.DrawString(Game1.font, "Ammo: " + ammo + "/" + maxAmmo, Vector2.Zero, Color.White);
            spriteBatch.DrawString(Game1.font, "Fuel: " + fuel + "/" + maxFuel, new Vector2(0, Game1.font.LineSpacing), Color.BurlyWood);
            spriteBatch.DrawString(Game1.font, "Position: " + "(" + bodyPos.X.ToString() + ", " + bodyPos.Y.ToString() + ")", new Vector2(0, Game1.font.LineSpacing * 2), Color.WhiteSmoke);
            if (reloading)
                spriteBatch.DrawString(Game1.font, "RELOADING...", new Vector2(150, 0), Color.Yellow);
            //-------------------------------------------------------------------------------------------------------------------
            
            Vector2 headcenter = new Vector2(headIndex.Width / 2, headIndex.Height / 2);
            Vector2 bodycenter = new Vector2(bodyIndex.Width / 2, bodyIndex.Height / 2);
            Vector2 guncenter = new Vector2(gunIndex.Width / 2, gunIndex.Height / 2);

            if (flip == true)
            {
                spriteBatch.Draw(headIndex, new Vector2(headPos.X-5, headPos.Y), null, Color.White, MathHelper.ToRadians(headRot), headcenter, scale, SpriteEffects.FlipVertically, 0);
                spriteBatch.Draw(bodyIndex, new Vector2(bodyPos.X+3, bodyPos.Y), sourceRectangle, Color.White, MathHelper.ToRadians(bodyRot), bodycenter, scale, SpriteEffects.FlipHorizontally, 0);
                spriteBatch.Draw(gunIndex, new Vector2(gunPos.X-20, gunPos.Y), null, Color.White, MathHelper.ToRadians(gunRot), guncenter, scale, SpriteEffects.FlipVertically, 0);
            }
            else
            {
                spriteBatch.Draw(headIndex, headPos, null, Color.White, MathHelper.ToRadians(headRot), headcenter, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(bodyIndex, bodyPos, sourceRectangle, Color.White, MathHelper.ToRadians(bodyRot), bodycenter, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(gunIndex, gunPos, null, Color.White, MathHelper.ToRadians(gunRot), guncenter, scale, SpriteEffects.None, 0);
            }

        }
        //=========================================================================================
        //  --- MOVEMENT FUNCTIONS ---
        //=========================================================================================
        //----------------------------------
        //  --- Update Player's Area
        //----------------------------------
        private void UpdateArea()
        {
            headArea.X = (int)headPos.X - (headIndex.Width / 2);
            headArea.Y = (int)headPos.Y - (headIndex.Height / 2);
            bodyArea.X = (int)bodyPos.X - (bodyIndex.Width / 2);
            bodyArea.Y = (int)bodyPos.Y - (bodyIndex.Height / 2)-35; // jetpack flame extends 35 pixles when animated
            gunArea.X = (int)gunPos.X - (gunIndex.Width / 2);
            gunArea.Y = (int)gunPos.Y - (gunIndex.Height / 2);         
        }
        //----------------------------------
        //  --- Set Player's Direction
        //----------------------------------
        private float point_direction(float x, float y, float dirx, float diry)
        {
            float adj = x - dirx;
            float opp = y - diry;
            float tan = opp / adj;
            float res = MathHelper.ToDegrees((float)Math.Atan2(opp, adj));
            res = (res - 180) % 360;

            if (res < 0)
            {
                res += 360;
            }

            return res;
        }
        //----------------------------------
        //  --- Check For Player Collisions
        //----------------------------------
        private bool playerCollision(Vector2 newVector)
        {
            if (Collision(newVector, headArea) != null)
                return true;
            else if (Collision(newVector, bodyArea) != null)
                return true;
            else if (Collision(newVector, gunArea) != null)
                return true;
            else
                return false;
        }
        //----------------------------------
        //  --- Player Jump
        //----------------------------------
        private void jump(GameTime gameTime)
        {
            int jumpTime = 192; // time(in miliseconds) it takes to complete jump
            float jumpspd = 7;

            if (jumping == false) // first time into loop
            {
                jumping = true;
                jumpStart = (int)gameTime.ElapsedGameTime.Milliseconds;
                currTime = jumpStart;
                headPos.Y -= jumpspd;
                bodyPos.Y -= jumpspd;
                gunPos.Y -= jumpspd;
            }
            else 
            {
                currTime += (int)gameTime.ElapsedGameTime.Milliseconds;
                if (currTime < (jumpStart + jumpTime) && !isPlayerStanding()) // are we still jumping?
                {
                    if (!playerCollision(new Vector2(0, -jumpspd)))// if no collision, continue jumping
                    {
                        headPos.Y -= jumpspd;
                        bodyPos.Y -= jumpspd;
                        gunPos.Y -= jumpspd;
                    }
                }
                else // if not, stop jumping
                {
                    gravity(4);
                    jumping = false;
                }
            }
            // Move left/right while jumping
            if (keyboard.IsKeyDown(Keys.A) && !playerCollision(new Vector2(-speed, 0)))
            {
                headPos.X -= speed;
                bodyPos.X -= speed;
                gunPos.X -= speed;
            }
            if (keyboard.IsKeyDown(Keys.D) && !playerCollision(new Vector2(speed, 0)))
            {
                headPos.X += speed;
                bodyPos.X += speed;
                gunPos.X += speed;
            }

        }
        //-------------------------------------------------
        //  --- Is the player standing on a solid surface
        //-------------------------------------------------
        private bool isPlayerStanding()
        {
            return playerCollision(new Vector2(0, fallspd));
        }
        //-------------------------------------------------
        //  --- Which Direction to Move
        //-------------------------------------------------
        private void checkMoveDirection(GameTime gameTime)
        {
            if (keyboard.IsKeyDown(Keys.W) && !playerCollision(new Vector2(0, -speed)))
            {
                if (fuel > 0)
                {
                    characterAnimation.Active = true;
                    headPos.Y -= speed;
                    bodyPos.Y -= speed;
                    gunPos.Y -= speed;
                    tempCurrentFrame.Y = 1;
                    fuel--;
                }
                else
                {
                    if (prevkeyboard.IsKeyUp(Keys.W) && isPlayerStanding())
                    {
                        jump(gameTime);
                    }
                }
            }
            if (keyboard.IsKeyDown(Keys.Space) && !playerCollision(new Vector2(0, -speed)))
            {
                if (prevkeyboard.IsKeyUp(Keys.Space) && isPlayerStanding())
                {
                    jump(gameTime);
                }
            }
            if (keyboard.IsKeyDown(Keys.A) && !playerCollision(new Vector2(-speed, 0)))
            {
                characterAnimation.Active = true;
                headPos.X -= speed;
                bodyPos.X -= speed;
                gunPos.X -= speed;
                if (keyboard.IsKeyUp(Keys.W))
                    tempCurrentFrame.Y = 0;
            }
            if (keyboard.IsKeyDown(Keys.S) && !playerCollision(new Vector2(0, speed)))
            {
                headPos.Y += speed;
                bodyPos.Y += speed;
                gunPos.Y += speed;
                if (keyboard.IsKeyUp(Keys.W))
                    tempCurrentFrame.Y = 0;
            }
            if (keyboard.IsKeyDown(Keys.D) && !playerCollision(new Vector2(speed, 0)))
            {
                characterAnimation.Active = true;
                headPos.X += speed;
                bodyPos.X += speed;
                gunPos.X += speed;
                if (keyboard.IsKeyUp(Keys.W))
                    tempCurrentFrame.Y = 0;
            }

            gravity(fallspd);

        }
        //-------------------------------------------------
        //  --- Gravity moves character down at fallspd
        //-------------------------------------------------
        private void gravity(float gravSpd)
        {
            if ((keyboard.IsKeyUp(Keys.W) || fuel <= 0) && !isPlayerStanding())
            {
                headPos.Y += gravSpd;
                bodyPos.Y += gravSpd;
                gunPos.Y += gravSpd;
            }        
        }
        //-------------------------------------------------
        //  --- Update the SpriteIndex/SourceRectangle
        //-------------------------------------------------
        private void updateAnimation(GameTime gameTime)
        {
            if (jumping)
                tempCurrentFrame.Y = 2;
            else
            {
                if (keyboard.IsKeyUp(Keys.A) && keyboard.IsKeyUp(Keys.D) && keyboard.IsKeyUp(Keys.W))
                {
                    tempCurrentFrame.Y = 0;
                    characterAnimation.Active = false;
                }
                if (keyboard.IsKeyDown(Keys.W) && fuel == 0)
                    tempCurrentFrame.Y = 0;
            }
            tempCurrentFrame.X = characterAnimation.CurrentFrame.X;
            characterAnimation.CurrentFrame = tempCurrentFrame;
            characterAnimation.Position = bodyPos;
            characterAnimation.Update(gameTime);
            sourceRectangle = characterAnimation.SourceRectangle;
        }
        //-------------------------------------------------
        //  --- Point Character at Mouse
        //-------------------------------------------------
        private void rotateCharacter()
        {
            rotation = point_direction(headPos.X, headPos.Y, mouse.X, mouse.Y);
            newheadRot = point_direction(headPos.X, headPos.Y, mouse.X, mouse.Y);
            newgunRot = point_direction(gunPos.X, gunPos.Y, mouse.X, mouse.Y);

            if ((newheadRot > 40 && newheadRot < 140) || (newheadRot < 315 && newheadRot > 235))
                headRot = prevheadRot;
            else
            {
                prevheadRot = headRot;
                headRot = newheadRot;
            }
            if ((newgunRot > 40 && newgunRot < 140) || (newgunRot < 315 && newgunRot > 235))
                gunRot = prevgunRot;
            else
            {
                prevgunRot = gunRot;
                gunRot = newgunRot;
            }
            if (headRot < 40 || headRot > 315)
            {
                flip = false;
                gunBarrel = new Vector2(gunPos.X + 60, gunPos.Y - 10);
            }
            else
            {
                flip = true;
                gunBarrel = new Vector2(gunPos.X + 80, gunPos.Y + 10);
            }

            gunBarrel = gunBarrel.RotatePoint(gunRot, gunPos);
        }
        //=========================================================================================
        //  --- GUN/SHOOTING FUNCTIONS ---
        //=========================================================================================
        //-----------------------------
        //  --- Is Player Reloading?
        //-----------------------------
        private void checkReload()
        {
            if (reloading)
                reloadTimer++;
            if (reloadTimer > reloadTime)
            {
                ammo = maxAmmo;
                reloadTimer = 0;
                reloading = false;
            }
        }
        //-----------------------------
        //  --- Is Player Shooting?
        //-----------------------------
        private void checkShooting()
        {
            if (firingTimer > rate && ammo > 0)
            {
                firingTimer = 0;
                Shoot();
            }
        }
        //-----------------------------
        //  --- Shoot
        //-----------------------------
        private void Shoot()
        {
            ammo--;
           
            Bullet blt = new Bullet(gunBarrel);
            blt.Rotation = gunRot;
            blt.Speed = bulletspd;
            blt.Create();
        }
        //=========================================================================================
    }
}