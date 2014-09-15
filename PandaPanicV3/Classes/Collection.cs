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

namespace PandaPanicV3
{
    public class Collection
    {
        public readonly static int NUM_OF_PLAYERS = 2, NUM_OF_ENEMIES = 10;

        // constants
        Vector2[] initialPositions;

        // variables
        List<Player>    players;
        List<Enemy>     enemies;
        List<Entity>    entities;
        List<Trail>     trails;

        public List<Trail> Trails
        {
            get { return trails; }
        }

        public List<Player> Players
        {
            get { return players; }
        }

        public List<Enemy> Enemies
        {
            get { return enemies; }
        }

        public List<Entity> Entities
        {
            get { return entities; }
        }
        
        // constructor
        public Collection()
        {
            initialPositions = new Vector2[] { 
                new Vector2(0, 0), 
                new Vector2(Game1.WIDTH - Entity.SIZE, 0), 
                new Vector2(0, Game1.HEIGHT - Entity.SIZE), 
                new Vector2(Game1.WIDTH - Entity.SIZE, Game1.HEIGHT - Entity.SIZE) 
            };
           
            entities = new List<Entity>();
            players = new List<Player>();
            enemies = new List<Enemy>();
            trails = new List<Trail>();

            List<Texture2D> sprites = new List<Texture2D>();
            Player player;
            for (int i = 0; i < NUM_OF_PLAYERS; i++)
            {
                player = new Player(initialPositions[i], i, (i + 1) % NUM_OF_PLAYERS);
                entities.Add(player);
                trails.Add(new Trail(ref player));
            }

            players = entities.Where(i => i.GetType() == typeof(Player)).ToList().Cast<Player>().ToList();
        
            // adds the AI objects
            for (int i = 0; i < NUM_OF_ENEMIES; i++)
            {
                Enemy enemy = new Enemy(Game1.random.Next(4));
                entities.Add(enemy);
            }

            enemies = entities.Where(i => i.GetType() == typeof(Enemy)).ToList().Cast<Enemy>().ToList();
        }

        public void update()
        {
            entities.ForEach(entity => entity.update());
            collisionPlayer();
            collisionEnemy();
            entities = entities.OrderBy(o => o.position.Y).ToList();
        }

        void collisionPlayer()
        {
            foreach (Player player1 in players)
            {
                if (!player1.alive) continue;

                foreach (Player player2 in players)
                {

                    if (!(player1.state == Player.STATE.ATTACK
                       && player2.alive
                          && !player1.Equals(player2)
                        && player1.bound.Intersects(player2.bound)))
                        continue;
                    {
                            //player1.Score += (int)time.getTime();
                            player2.die();
                            /*sounds["hit"].Play();
                            sounds["death"].Play();
                            sounds["stab"].Play();*/
                        player1.Score += 50;
                    }
                }
            }
        }

        void collisionEnemy()
        {
            foreach (Player player in players)
            {
                if (!player.alive) continue;

                foreach (Enemy enemy in enemies)
                {

                    if (!(player.state == Player.STATE.ATTACK
                        && enemy.alive
                        && player.bound.Intersects(enemy.bound)))
                        continue;

                    player.Score -= 5;
                    enemy.die();
                    /*sounds["hit"].Play();
                    sounds["death"].Play();*/
                }
            }
        }

        public void resetRound()
        {
            setEntities(Player.STATE.NORMAL);
            entities.ForEach(entity => entity.reset());
        }

        public void resetGame()
        {
            players.ForEach(player => player.resetGame());
            enemies.ForEach(enemy => enemy.reset());
        }

        public void actionPause()
        {
            players.ForEach(player => player.update());
            trails.ForEach(trail => trail.update());     
        }

         public void setEntities(Player.STATE state)
        {
            players.ForEach(player => player.state = state);
        }
                                      
    }
}
