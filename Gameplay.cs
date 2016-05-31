﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mors_Arcium
{
    public class Gameplay
    {
        public const int TYPE_PLAYER = 0;
        public const int TYPE_PROJECTILE = 1;

        public Vector2 cameraPosition;
        public float cameraRotation;
        public MorsArcium game;

        public Entity[,] entities;
        public Tilemap tilemap;
        private Particle[] particles;

        int time;
        int ticks;
        int fps;
        Vector2 cameraOffset = new Vector2(160, 120);

        public Player player;

        public Gameplay(MorsArcium g)
        {
            game = g;
        }
        public void Initialize()
        {
            entities = new Entity[8, 128];
            particles = new Particle[128];
            tilemap = new Tilemap(this, game.textures[5], 78, 24);
            player = new MrBPlayer(this);
            player.position = new Vector2(game.random.Next(32, (tilemap.width * 16) - 32), 0.0f);
            AddEntity(player);
        }
        public void Update(GameTime gt)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) game.Exit();
            if (Keyboard.GetState().IsKeyDown(game.JUMP))
            {
                player.Jump();
            }
            if (Keyboard.GetState().IsKeyDown(game.RIGHT))
            {
                player.spriteEffects = SpriteEffects.None;
                player.Walk();
            }
            else if (Keyboard.GetState().IsKeyDown(game.LEFT))
            {
                player.spriteEffects = SpriteEffects.FlipHorizontally;
                player.Walk();
            }
            if (Keyboard.GetState().IsKeyDown(game.UP))
            {
                player.aimDirection = 1;
            }
            else if (Keyboard.GetState().IsKeyDown(game.DOWN))
            {
                player.aimDirection = -1;
            }
            else
            {
                player.aimDirection = 0;
            }
            if (Keyboard.GetState().IsKeyDown(game.ATTACK))
            {
                player.Attack();
            }

            for (int x = 0; x < entities.GetLength(0); x++)
            {
                for (int y = 0; y < entities.GetLength(1); y++)
                {
                    if (entities[x, y] != null)
                    {
                        entities[x, y].Update(gt);
                    }
                }
            }
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i] != null)
                {
                    particles[i].Update(gt);
                    if (particles[i].killMe)
                    {
                        particles[i] = null;
                    }
                }
            }
            
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                int x = (int)Math.Floor(((float)(Mouse.GetState().Position.X / game.scaleFactor) + cameraPosition.X) / 16);
                int y = (int)Math.Floor(((float)(Mouse.GetState().Position.Y / game.scaleFactor) + cameraPosition.Y) / 16);
                if (x < 0) x = 0;
                if (y < 0) y = 0;
                if (x >= tilemap.width) x = tilemap.width - 1;
                if (y >= tilemap.height) y = tilemap.height - 1;
                tilemap.data[x, y] = -1;
                tilemap.RefreshTiles();
            }
            cameraPosition = player.position - cameraOffset;
            if (time == gt.TotalGameTime.Seconds)
            {
                ticks += 1;
            }
            else
            {
                fps = ticks;
                ticks = 0;
                time = gt.TotalGameTime.Seconds;
            }
        }
        public void Draw(SpriteBatch sp)
        {
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Matrix.CreateTranslation(new Vector3(-cameraPosition, 0f)));
            tilemap.Draw(sp);
            for (int x = 0; x < entities.GetLength(0); x++)
            {
                for (int y = 0; y < entities.GetLength(1); y++)
                {
                    if (entities[x, y] != null)
                    {
                        entities[x, y].Draw(sp);
#if DEBUG
                        //sp.Draw(game.textures[6], new Rectangle((int)(entities[x, y].position.X - entities[x, y].hitboxSize.X), (int)(entities[x, y].position.Y - entities[x, y].hitboxSize.Y), (int)(entities[x, y].hitboxSize.X * 2.0f), (int)(entities[x, y].hitboxSize.Y * 2.0f)), Color.White);
#endif
                        for (int i = 0; i < entities[x, y].collisionMask.Length; i++)
                        {
                            int type = entities[x, y].collisionMask[i];
                            for (int j = 0; j < entities.GetLength(1); j++)
                            {
                                if (entities[type, j] != entities[x, y] && entities[type, j] != null)
                                {
                                    if (entities[x, y].position.X + entities[x, y].hitboxSize.X + entities[x, y].hitboxOffset.X > entities[type, j].position.X - entities[type, j].hitboxSize.X + entities[type, j].hitboxOffset.X
                                        && entities[x, y].position.X - entities[x, y].hitboxSize.X + entities[x, y].hitboxOffset.X < entities[type, j].position.X + entities[type, j].hitboxSize.X + entities[type, j].hitboxOffset.X
                                        && entities[x, y].position.Y + entities[x, y].hitboxSize.Y + entities[x, y].hitboxOffset.Y > entities[type, j].position.Y - entities[type, j].hitboxSize.Y + entities[type, j].hitboxOffset.Y
                                        && entities[x, y].position.Y - entities[x, y].hitboxSize.Y + entities[x, y].hitboxOffset.Y < entities[type, j].position.Y + entities[type, j].hitboxSize.Y + entities[type, j].hitboxOffset.Y
                                    )
                                    {
                                        entities[x, y].Collide(entities[type, j]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i] != null) particles[i].Draw(sp);
            }
            sp.End();
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);
            sp.DrawString(game.font1, "FPS: " + fps, new Vector2(0, 120), Color.White);
            sp.Draw(game.textures[2], Vector2.Zero, new Rectangle(0, 0, 117, 32), Color.White);
            sp.Draw(game.textures[2], new Rectangle(12, 2, 4 + player.health, 12), new Rectangle(0, 97, 1, 1), Color.White);
            sp.Draw(game.textures[2], new Rectangle(12, 18, 4 + player.magic, 12), new Rectangle(0, 98, 1, 1), Color.White);
            sp.End();
        }
        public void AddEntity(Entity e)
        {
            int index = -1;
            for (int i = 0; i < entities.GetLength(1); i++)
            {
                if (entities[e.type, i] == null)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                index = 0;
                Console.WriteLine("ENTITY LIMIT REACHED ON TYPE " + e.type);
            }
            e.index = index;
            entities[e.type, index] = e;
        }
        public void RemoveEntity(Entity e)
        {
            entities[e.type, e.index] = null;
            e = null;
        }
        public void AddParticle(Particle p)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i] == null)
                {
                    particles[i] = p;
                    break;
                }
            }
        }
        public void Explode(float x, float y, float radius, int damage)
        {
            int tx = (int)Math.Round(x / 16);
            int ty = (int)Math.Round(y / 16);
            int tr = (int)Math.Ceiling(radius / 16);
            Vector2 pos = new Vector2(x, y);
            for (int yy = -tr; yy < tr; yy++)
            {
                for (int xx = -tr; xx < tr; xx++)
                {
                    if (tx + xx > 0 && tx + xx < tilemap.width && ty + yy > 0 && ty + yy < tilemap.height)
                    {
                        if (tilemap.data[tx + xx, ty + yy] != -1)
                        {
                            float dist = Vector2.Distance(pos, new Vector2(((tx + xx) * 16) + 8, ((ty + yy) * 16) + 8));
                            if (dist < radius)
                            {
                                tilemap.data[tx + xx, ty + yy] = -1;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < entities.GetLength(1); i++)
            {
                if (entities[TYPE_PLAYER, i] != null)
                {
                    if (Vector2.Distance(pos, entities[TYPE_PLAYER, i].position) < radius + entities[TYPE_PLAYER, i].hitboxSize.X)
                    {
                        Player p = (Player)entities[TYPE_PLAYER, i];
                        p.Damage(damage);
                    }
                }
            }
            tilemap.RefreshTiles();
            AddParticle(new Particle(this, pos - new Vector2(radius), Vector2.Zero, 3, 32, 2));
        }
    }
}
