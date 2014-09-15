using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PandaPanicV3
{
    public partial class Enemy : Entity
    {
        const double MAX_SPEED = 1.5, MIN_SPEED = 0.5;
        static readonly Func<Vector2,bool>[] speeds;

        // variables
        Vector2         speed, savedSpeed;
        Counter         counter;
        int             selectedMove;
        Counter         moveSwitch, pause;
        bool            stop;
        int[]           imageSet;
        List<Action>    movement;

        static Enemy()
        {
            speeds = new Func<Vector2,bool>[]
            {
                (Vector2 speed) => speed.Y < 0,
                (Vector2 speed) => speed.X < 0,
                (Vector2 speed) => speed.Y > 0,
                (Vector2 speed) => speed.X > 0
            };
        }

        // constructor
        public Enemy(int _type)
            : base(Vector2.Zero)
        {
            type = _type;
            position = new Vector2
            (
                Game1.random.Next(0, (int)Game1.WIDTH),
                Game1.random.Next(0, (int)Game1.HEIGHT)
            );
            speed = new Vector2
            (
                (float)GenerateSpeed(),
                (float)GenerateSpeed()
            );

            alive = true;
            selectMovement();

            moveSwitch = new Counter(Game1.random.Next(5 * Game1.SECOND, 10 * Game1.SECOND));
            counter = new Counter(Game1.random.Next(1 * Game1.SECOND, 3 * Game1.SECOND));
            pause = new Counter(Game1.random.Next(100, 200));

            movement = new List<Action>
            {
                MoveLine,
                MoveDiagonal,
                MoveStaircase,
                MoveWaveX,
                MoveWaveY,
                MovePause,
            };

            imageSet = xset;
        }

        public override void update()
        {
            if (!alive) return;

            foreach (int i in imageSet)
            {
                if (speeds[i](speed))
                {
                    Ani.update(i);
                    break;
                }
            }

            if (moveSwitch.isReady()) selectMovement();

            movement[selectedMove]();
            position += speed;

            updateBound();
            base.update();
        }

        public void selectMovement()
        {
            selectedMove = Game1.random.Next(6);
            imageSet = selectedMove == 4 ? yset : xset;
        }

        public void die()
        {
            alive = false;
            speed = Vector2.Zero;
            Ani.die();
        }

        public override void reset()
        {
            alive = true;
            Ani.reset();
        }

    }
}
