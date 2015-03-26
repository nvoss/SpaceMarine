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
    public static class Extensions
    {
        /// <summary>
        /// Rotates a point around another set point.
        /// </summary>
        /// <param name="PointToRotate">The point to rotate around the origin.</param>
        /// <param name="Origin">The point to rotate around.</param>
        /// <returns></returns>
        public static Vector2 RotatePoint(this Vector2 PointToRotate, float Angle, Vector2 Origin)
        {
            Vector2 retVal = Vector2.Zero;

            Angle = MathHelper.ToRadians(Angle);

            retVal.X = (float)Math.Cos(Angle) * (PointToRotate.X - Origin.X) - (float)Math.Sin(Angle) *
                (PointToRotate.Y - Origin.Y) + Origin.X;
            retVal.Y = (float)Math.Sin(Angle) * (PointToRotate.X - Origin.X) + (float)Math.Cos(Angle) *
                (PointToRotate.Y - Origin.Y) + Origin.Y;

            return retVal;
        }

        /// <summary>
        /// Quick method to draw a simple box on the screen.
        /// Used mainly for debugging hitboxes.
        /// </summary>
        /// <param name="Graphics">GrahpicsDeviceManager object from the main game.</param>
        /// <param name="Batch">SpriteBatch object from the main game.</param>
        /// <param name="Box">HitBox Rectangle</param>
        /// <param name="RectColor">The color to draw the rectangle.</param>
        public static void DrawBox(GraphicsDeviceManager Graphics, SpriteBatch Batch, Rectangle Box, Color RectColor)
        {
            Texture2D rect = new Texture2D(Graphics.GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.Red });
            Batch.Draw(rect, Box, null, Color.Red, 0.0f, Vector2.Zero, SpriteEffects.None, .99f);
        }

    }
}
