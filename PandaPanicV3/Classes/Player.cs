using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace PandaPanicV3
{
    public class Player : Entity
    {
        public enum                 STATE { NORMAL, PAUSED, ATTACK, DEAD }
        readonly static int         SCORE = 0, SPEED = 1;
        readonly static Keys[][]    KEYS;
        readonly static int[]       RESET_POSITIONS = new int[]{0, 840 - SIZE};
        
        int                         score, targetType;
        public STATE                state;
        Keys[]                      control;
        Vector2[]                   speeds;

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        static Player()
        {
            KEYS = new Keys[][] 
            { 
                new Keys[] { Keys.W, Keys.A, Keys.S, Keys.D, Keys.Space }, 
                new Keys[] { Keys.I, Keys.J, Keys.K, Keys.L, Keys.RightShift } 
            };
        }

        public Player(Vector2 _position, int _type, int _target)
            : base(_position)
        {
            type = _type;
            control = KEYS[type];
            score = SCORE;
            speeds = new Vector2[] { new Vector2(0, -SPEED), new Vector2(-SPEED, 0), new Vector2(0, SPEED), new Vector2(SPEED, 0) };
        }

        public override void update()
        {
            if (!alive) return;

            if (Keyboard.GetState().IsKeyDown(control.Last()))
            {
                state = STATE.ATTACK;

                foreach (int i in xset)
                {
                    if (Keyboard.GetState().IsKeyDown(control[i]))
                    {
                        Ani.update(i + 4);
                        break;
                    }
                }
            }
            else
            {
                state = STATE.NORMAL;

                // prioritize the horizontal animations
                // updates the animation
                foreach (int i in xset)
                {
                    if (Keyboard.GetState().IsKeyDown(control[i]))
                    {
                        Ani.update(i);
                        break;
                    }
                }
            }

            for (int i = 0; i < control.Length - 1; i++)
            {
                if (Keyboard.GetState().IsKeyDown(control[i]))
                    position += speeds[i];
            }

            base.update();
            updateBound();
        }

        public void die()
        {
            alive = false;
            Ani.die();
        }

        public override void reset()
        {
            alive = true;
            state = STATE.NORMAL;
            position = new Vector2(
                RESET_POSITIONS[type], 
                Game1.random.Next(0, 480 - SIZE)
                );
            Ani.reset();
        }

        public void resetGame()
        {
            score = 0;
            reset();
        }
    }
}
