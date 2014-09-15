using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace PandaPanicV3
{
    /*
     * Description: A set of helper functions.
     * The methods set the speed of the enemy.
     * 
     */
    public partial class Enemy
    {
        public double GetRandomNumber(double minimum, double maximum)
        {
            return Game1.random.NextDouble() * (maximum - minimum) + minimum;
        }

        public double GenerateSpeed()
        {
            return Game1.random.Next(2) == 0 ? GetRandomNumber(MIN_SPEED, MAX_SPEED) : -GetRandomNumber(MIN_SPEED, MAX_SPEED);
        }

        // line movement
        private void MoveLine()
        {
            if (!counter.isReady()) return;

            if (Game1.random.Next(2) == 0)
            {
                speed.X = (float)GenerateSpeed();
                speed.Y = (float)GenerateSpeed();
            }
            else
            {
                speed.X = 0;
                speed.Y = (float)GenerateSpeed();
            }
        }

        // diagonal movement
        private void MoveDiagonal()
        {
            if (!counter.isReady()) return;
            speed.X = (float)GenerateSpeed();
            speed.Y = (float)GenerateSpeed();
        }

        private void MoveStaircase()
        {
            if (!counter.isReady()) return;
            if (speed.X != 0)
            {
                speed.Y = speed.X;
                speed.X = 0;
            }
            else
            {
                speed.X = speed.Y;
                speed.Y = 0;
            }
        }

        private void MoveWaveY()
        {
            speed.Y = (float)(MAX_SPEED * Math.Cos(position.X / 20));
            imageSet = yset;
        }

        private void MoveWaveX()
        {
            speed.X = (float)(MAX_SPEED * Math.Cos(position.Y / 20));
        }

        private void MovePause()
        {
            if (pause.isReady())
            {
                stop = !stop;
                if (stop)
                {
                    savedSpeed = speed;
                    speed = Vector2.Zero;
                }
                else
                    speed = savedSpeed;
            }
        }
    }
}
