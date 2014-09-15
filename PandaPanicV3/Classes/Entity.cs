using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PandaPanicV3
{
    public abstract partial class Entity
    {
        public const int SIZE = 100;
        public readonly static Dictionary<Color,string> colors;
        public readonly static Color fillColor;
        protected readonly static int[] xset, yset;
        
        public Vector2      position;
        // the panal is the oval that the entity is displayed on top of 
        public Rectangle    bound, panal;
        public bool         alive;
        public int          type;
        Animate             ani;

        public Animate Ani
        {
            get { return ani; }
        }

        static Entity()
        {
            xset = new int[]{ 0, 2, 1, 3 };
            yset = new int[]{ 1, 3, 0, 2 };
            fillColor = Color.Black;
            fillColor.A = 127;
            colors = new Dictionary<Color,string>();
            colors.Add(Color.Pink, "Pink");
            colors.Add(Color.Green,"Green");
            colors.Add(Color.Blue,"Blue");
            colors.Add(Color.White,"White");
        }

        public Entity(Vector2 _position)
        {
            position = _position;
            alive = true;
            ani = new Animate();
            bound = new Rectangle((int)position.X, (int)position.Y, SIZE, SIZE);
            panal = new Rectangle(bound.X + bound.Width / 4,bound.Y + (bound.Width * 2) / 3 + 10,bound.Width / 2,bound.Height / 3);
        }

        public void updateBound()
        {
            bound.X = (int)position.X;
            bound.Y = (int)position.Y;
        }

        public virtual void update()
        {
            // wraps the moveable object around the screen
            if (position.X > 840)
                position = new Vector2(-SIZE, position.Y);
            else if (position.X < -SIZE)
                position = new Vector2(840, position.Y);

            if (position.Y > 480)
                position = new Vector2(position.X, -SIZE);
            else if (position.Y < -SIZE)
                position = new Vector2(position.X, 480);

            panal.X = bound.X + bound.Width / 4;
            panal.Y = bound.Y + (bound.Width * 2) / 3 + 10;
        }

        public virtual void draw()
        {
            if (!alive)
                Game1.batch.Draw(Artist.textures["skull"], bound, Color.White);
            else
            {
               // Game1.batch.Draw(Artist.textures["oval"], panal, colors.ElementAt(type).Key);
                //Game1.batch.Draw(Artist.textures["fillOval"], panal, fillColor);

                if (ani.direction == 1 || ani.direction == 5)
                    Game1.batch.Draw(Artist.textures["Pandas"], bound, ani.source, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                else
                    Game1.batch.Draw(Artist.textures["Pandas"], bound, ani.source, Color.White);
            }
        }

        public abstract void reset();
    }
}
