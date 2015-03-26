using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceMarine
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Rectangle gameWindow;
        public static SpriteFont font;
        CharacterBase Player = new CharacterBase();
        Cursor cursor = new Cursor();

        //=========================================================================================
        //  --- CONSTRUCTOR(S) ---
        //=========================================================================================
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        //=========================================================================================
        //  --- BASE FUNCTIONS ---
        //=========================================================================================
        //------------------------
        //  --- Initialize
        //------------------------
        protected override void Initialize()
        {
            gameWindow = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            WorldItems.Initialize();
            foreach (GameObject item in WorldItems.objList)
                item.Initialize();
            foreach (Mob enemy in WorldItems.mobList)
                enemy.Initialize();

            base.Initialize();
        }
        //------------------------
        //  --- Load Content
        //------------------------
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/HUDFont");

            foreach (GameObject item in WorldItems.toCreateObjList)
                item.LoadContent(this.Content);
            foreach (GameObject item in WorldItems.objList)
                item.LoadContent(this.Content);
            
            foreach (Mob enemy in WorldItems.toCreateMobList)
                enemy.LoadContent(this.Content);
            foreach (Mob enemy in WorldItems.mobList)
                enemy.LoadContent(this.Content);
        }
        //------------------------
        //  --- Unload Content
        //------------------------
        protected override void UnloadContent()
        {
        }
        //------------------------
        //  --- Update
        //------------------------
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // Add any new items to the World 
            foreach (GameObject item in WorldItems.toCreateObjList)
            {
                item.LoadContent(this.Content);
                WorldItems.objList.Insert(0, item);
            }
            WorldItems.toCreateObjList.Clear();

            // Update all items in the World 
            for (int i = 0; i < WorldItems.objList.Count(); i++)
            {
                GameObject item = WorldItems.objList[i];
                item.Update(gameTime);
            }

            // Add any new Enemies to the World 
            foreach (Mob enemy in WorldItems.toCreateMobList)
            {
                enemy.LoadContent(this.Content);
                WorldItems.mobList.Insert(0, enemy);
            }
            WorldItems.toCreateMobList.Clear();

            // Update all Enemies in the World 
            for (int i = 0; i < WorldItems.mobList.Count(); i++)
            {
                Mob enemy = WorldItems.mobList[i];
                enemy.Update(gameTime);
            }

            base.Update(gameTime);
        }
        //------------------------
        //  --- Draw
        //------------------------
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            foreach (Mob enemy in WorldItems.mobList)
                enemy.Draw(spriteBatch);                
            
            foreach (GameObject item in WorldItems.objList)
                item.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
        //=========================================================================================
    }
}
