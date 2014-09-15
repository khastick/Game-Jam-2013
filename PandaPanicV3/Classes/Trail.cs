using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PandaPanicV3
{
    public class Trail
    {
        public const int SIZE = 3, DELAY = 50;
    
        List<Vector2>    positions;
        List<Rectangle>  sources;
        List<int>        directions;
        Rectangle        bound;
        Counter          timer;
        Player           mPlayer;

        public Trail(ref Player player)
        {
            mPlayer = player;
            bound = new Rectangle(0, 0, Entity.SIZE, Entity.SIZE);

            positions = new List<Vector2>();
            sources = new List<Rectangle>();
            directions = new List<int>();

            for (int i = 0; i < SIZE; i++)
            {
                positions.Add(Vector2.Zero);
                sources.Add(player.Ani.source);
                directions.Add(0);
            }

            timer = new Counter(DELAY);
        }

        public void updateBound(int index)
        {
            bound.X = (int)(positions[index].X);
            bound.Y = (int)(positions[index].Y);
        }

        public void update()
        {
            if (!timer.isReady()) return;

            for (int i = SIZE - 1; i >= 1; i--)
            {
                positions[i] = positions[i - 1];
                sources[i] = sources[i - 1];
                directions[i] = directions[i - 1];
            }

            positions[0] = mPlayer.position;
            sources[0] = mPlayer.Ani.source;
            directions[0] = mPlayer.Ani.direction;
        }

        public void draw()
        {
            Color _color = Color.White;

            for (int i = SIZE - 1; i >= 0; i--)
            {
                _color.A = (byte)((positions.Count - i) * (255 / positions.Count));
                updateBound(i);
                if (directions[i] == 1 || directions[i] == 5)
                    Game1.batch.Draw(Artist.textures["Pandas"], bound, sources[i], Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                else
                    Game1.batch.Draw(Artist.textures["Pandas"], bound, sources[i], Color.White);
            }
        }
    }
}
