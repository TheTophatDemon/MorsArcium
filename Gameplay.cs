using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mors_Arcium
{
    public class Gameplay
    {
        public const int TYPE_PLAYER = 0;

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

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                player.Jump();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                player.spriteEffects = SpriteEffects.None;
                player.Walk();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                player.spriteEffects = SpriteEffects.FlipHorizontally;
                player.Walk();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.J))
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
                    }
                }
            }
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i] != null) particles[i].Draw(sp);
            }
            sp.End();
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);
            sp.DrawString(game.font1, "FPS: " + fps, Vector2.Zero, Color.White);
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
    }
}
