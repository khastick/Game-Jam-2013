using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace PandaPanicV3
{
    public class Animate
    {
        static int MAX_FRAME = 8, DISPLACEMENT, DEAD_SOURCE = 1, ACTIVE_SOURCE = 0;
        public static Rectangle[] SOURCES;

        public Counter      counter;
        public int          frame, direction, index, displacement;
        public Rectangle    source;

        static Animate()
        {
            SOURCES = new Rectangle[] {
                new Rectangle(0,0,127,156),
                new Rectangle(0,0,70,70)
            };

            DISPLACEMENT = 3 * SOURCES[ACTIVE_SOURCE].Height;
        }

        internal Animate()
        {
            frame = 0;
            direction = 0;
            counter = new Counter(8);
            source = SOURCES[ACTIVE_SOURCE];
        }

        public void update(int i)
        {
            // change the image type
            if (direction != i)
            {
                direction = i;
                frame = 0;
                index = i % 4;
                displacement = i < 4 ? 0 : DISPLACEMENT;

                source.Y = displacement + (index < 3 ? index * SOURCES[ACTIVE_SOURCE].Height : SOURCES[0].Height);
                source.X = frame * SOURCES[0].Width;
            }

            // update the animation
            if (counter.isReady())
            {
                source.X = ++frame % MAX_FRAME * SOURCES[ACTIVE_SOURCE].Width;
            }
        }

        public void die()
        {
            source = SOURCES[DEAD_SOURCE];
        }

        public void reset()
        {
            frame = 0;
            direction = 0;
            counter = new Counter(10);
            source = SOURCES[0];
        }
    }
}
