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
    class WorldItems
    {
        //=========================================================================================
        //  --- LIST(s) OF OBJECTS IN GAME ---
        //=========================================================================================
        public static List<GameObject> objList = new List<GameObject>();
        public static List<GameObject> toCreateObjList = new List<GameObject>();
        public static List<Mob> mobList = new List<Mob>();
        public static List<Mob> toCreateMobList = new List<Mob>();
        //=========================================================================================
        //  --- INITIALIZE ---
        //=========================================================================================
        public static void Initialize()
        {
            for (int i = 0; i < 16; i++) // Floor
                objList.Add(new Wall(new Vector2(i*50, 430))); 

            for (int i = 0; i < 3; i++) // Platform
                objList.Add(new Wall(new Vector2((i * 50)+600, 200)));

            objList.Add(new Wall(new Vector2(400, 380))); 



            objList.Add(new CharacterBase());
            objList.Add(new Cursor());

            mobList.Add(new Mob(new Vector2(100, 100)));
            mobList.Add(new Mob(new Vector2(200, 100)));
            mobList.Add(new Mob(new Vector2(300, 50)));
            mobList.Add(new Mob(new Vector2(400, 25)));
        }
        //=========================================================================================
        //  --- CLASS SPECIFIC FUNCTIONS ---
        //=========================================================================================
        // N/A
        //=========================================================================================
    }
}
