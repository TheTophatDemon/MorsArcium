using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mors_Arcium
{
    public class Gameplay
    {
        public const int TYPE_PLAYER = 6;
        public const int TYPE_PROJECTILE = 4;
        public const int TYPE_PROP = 1;
        public const int TYPE_ITEM = 2;
        public const int TYPE_BEAM = 7;

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
        bool terrainModified = false;
        Vector2 deathThingy;
        Rectangle deathThingyRect = new Rectangle(0, 32, 120, 65);
        Rectangle hbRect = new Rectangle(0, 97, 1, 1);
        Rectangle mbRect = new Rectangle(0, 98, 1, 1);
        Rectangle mbhbRect = new Rectangle(0, 0, 117, 32);
        Rectangle pauseThingyRect = new Rectangle(0, 99, 124, 25);
        public int healthPackFrequency = 500;
        public Player player;
        public float fadeIn;
        public float fadeOut;
        public int numCPUs = 20;
        public int numPlayers = 11;
        public int wave = 0;
        public int waveTimer = 0;
        private float waveAlpha = 1.0f;
        private int healthPackTimer = 500;
        string cheatString = "";
        int nearestEnemy = 0;

        public Gameplay(MorsArcium g)
        {
            game = g;
        }
        public void Initialize()
        {
            deathThingy = new Vector2(100, 240);
            entities = new Entity[8, 128];
            particles = new Particle[128];
            tilemap = new Tilemap(this, game.textures[5], 197, 24);
            player = new WizardPlayer(this);
            player.position = new Vector2(game.random.Next(32, (tilemap.width * 16) - 32), 0.0f);
            //SpawnEnemies();
            AddEntity(player);
            fadeOut = 0f;
            fadeIn = 1.0f;
            wave = 0;
            waveTimer = 0;
            waveAlpha = 1.0f;
            numPlayers = 1;
        }
        private void SpawnEnemies()
        {
            /*for (int i = 0; i < numCPUs; i++)
            {
                int t = game.random.Next(0, 3);
                switch (t)
                {
                    case 0:
                        MrBPlayer p = new MrBPlayer(this);
                        p.position.X = game.random.Next(32, (tilemap.width * 16) - 32);
                        AddEntity(p);
                        p = null;
                        break;
                    case 1:
                        WizardPlayer w = new WizardPlayer(this);
                        w.position.X = game.random.Next(32, (tilemap.width * 16) - 32);
                        AddEntity(w);
                        w = null;
                        break;
                    case 2:
                        EliPlayer e = new EliPlayer(this);
                        e.position.X = game.random.Next(32, (tilemap.width * 16) - 32);
                        AddEntity(e);
                        e = null;
                        break;
                }
            }*
            numPlayers = numCPUs + 1;*/
            for (int i = 0; i < 4; i++)
            {
                MrBPlayer p = new MrBPlayer(this);
                p.position.X = game.random.Next(32, (tilemap.width * 16) - 32);
                p.position.Y = -64.0f;
                AddEntity(p);
                p = null;
            }
            for (int i = 0; i < 4; i++)
            {
                WizardPlayer w = new WizardPlayer(this);
                w.position.X = game.random.Next(32, (tilemap.width * 16) - 32);
                w.position.Y = -64.0f;
                AddEntity(w);
                w = null;
            }
            for (int i = 0; i < 4; i++)
            {
                EliPlayer e = new EliPlayer(this);
                e.position.X = game.random.Next(32, (tilemap.width * 16) - 32);
                e.position.Y = -64.0f;
                AddEntity(e);
                e = null;
            }
        }
        public void Update(GameTime gt)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) game.Exit();
            if (!player.dead)
            {
#if DEBUG
                if (Keyboard.GetState().IsKeyDown(Keys.U)) player.health += 5;
                if (Keyboard.GetState().IsKeyDown(Keys.T))
                {
                    for (int i = 0; i < entities.GetLength(1); i++)
                    {
                        if (entities[TYPE_PLAYER, i] != null)
                        {
                            Player p = (Player)entities[TYPE_PLAYER, i];
                            p.target = player;
                            p = null;
                        }
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Y))
                {
                    for (int i = 0; i < entities.GetLength(1); i++)
                    {
                        if (entities[TYPE_PLAYER, i] != null && entities[TYPE_PLAYER, i] != player)
                        {
                            Player p = (Player)entities[TYPE_PLAYER, i];
                            p.health = 0;
                            p = null;
                        }
                    }
                }
#endif
                Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();
                if (pressedKeys.Length > 0)
                {
                    if (cheatString.Length > 0)
                    {
                        if (pressedKeys[0].ToString() != cheatString.Substring(cheatString.Length - 1))
                        {
                            cheatString += pressedKeys[0].ToString();
                        }
                    }
                    else
                    {
                        cheatString += pressedKeys[0].ToString();
                    }
                }

                
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
                    player.aimDirection = -1;
                }
                else if (Keyboard.GetState().IsKeyDown(game.DOWN))
                {
                    player.aimDirection = 1;
                }
                else
                {
                    player.aimDirection = 0;
                }
                if (Keyboard.GetState().IsKeyDown(game.ATTACK))
                {
                    player.Attack();
                }
                if (Keyboard.GetState().IsKeyDown(game.SPECIAL))
                {
                    player.Special();
                }
            }
            else
            {
                deathThingy.Y -= (deathThingy.Y - 72f) / 4;
                if (deathThingy.Y < 73) deathThingy.Y = 72;
                player.deathTimer += 1;
            }
            if (healthPackTimer > 0)
            {
                healthPackTimer -= 1;
                if (healthPackTimer == 0)
                {
                    AddEntity(new HealthPack(this, new Vector2(game.random.Next(0, tilemap.width * 16), -64.0f)));
                    healthPackTimer = healthPackFrequency;
                }
            }
            numPlayers = 0;
            float minDist = 10000;
            nearestEnemy = 0;
            for (int x = 0; x < entities.GetLength(0); x++)
            {
                for (int y = 0; y < entities.GetLength(1); y++)
                {
                    if (entities[x, y] != null)
                    {
                        if (entities[x, y].type == TYPE_PLAYER && entities[x, y] != player)
                        {
                            Player p = (Player)entities[x, y];
                            p.UpdateCPU();
                            if (Vector2.Distance(p.position, player.position) < minDist)
                            {
                                minDist = Vector2.Distance(p.position, player.position);
                                if (p.position.X > player.position.X)
                                {
                                    nearestEnemy = 1;
                                }
                                else
                                {
                                    nearestEnemy = -1;
                                }
                            }
                            p = null;
                        }
                        if (entities[x, y].type == TYPE_PLAYER)
                        {
                            numPlayers += 1;
                        }
                        entities[x, y].Update(gt);
                        if (entities[x, y].collisions)
                        {
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
                        if (entities[x, y].killMe)
                        {
                            RemoveEntity(entities[x, y]);
                        }
                    }
                }
            }
            if (cheatString.Contains("TDGMH"))
            {
                cheatString = "";
                Console.WriteLine("Give me hell!!!");
                for (int i = 0; i < entities.GetLength(1); i++)
                {
                    if (entities[TYPE_PLAYER, i] != null && entities[TYPE_PLAYER, i] != player)
                    {
                        Player p = (Player)entities[TYPE_PLAYER, i];
                        p.ChangeInto(64);
                        p = null;
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
            if (terrainModified)
            {
                terrainModified = false;
                tilemap.RefreshTiles();
            }
            if (player.deathTimer == 0 && numPlayers == 1 && waveTimer == 0)
            {
                wave += 1;
                waveTimer = 200;
                waveAlpha = 0.0f;
            }

            if (waveTimer > 0)
            {
                waveTimer -= 1;
                if (waveTimer == 0)
                {
                    SpawnEnemies();
                }
            }
            if (player.deathTimer > 200)
            {
                Initialize();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D1) && !(player is MrBPlayer))
            {
                player.ChangeInto(0);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D2) && !(player is WizardPlayer))
            {
                player.ChangeInto(32);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D3) && !(player is EliPlayer))
            {
                player.ChangeInto(64);
            }
            if (time == DateTime.Now.Second)
            {
                ticks += 1;
            }
            else
            {
                fps = ticks;
                ticks = 0;
                time = DateTime.Now.Second;
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
           
            
            sp.End();
            sp.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Matrix.CreateTranslation(new Vector3(-cameraPosition, 0f)));
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i] != null) particles[i].Draw(sp);
            }
            sp.End();
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);
            sp.DrawString(game.font1, "FPS: " + fps, new Vector2(0, 120), Color.White);
            sp.Draw(game.textures[2], Vector2.Zero, mbhbRect, Color.White);
            sp.Draw(game.textures[2], new Rectangle(12, 2,  (int)(((float)player.health / player.maxHealth) * 104.0f), 12), hbRect, Color.White);
            sp.Draw(game.textures[2], new Rectangle(12, 18, (int)(((float)player.magic / player.maxMagic) * 104.0f), 12), mbRect, Color.White);
            sp.DrawString(game.font1, "WAVE " + wave, new Vector2(240, 0), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            sp.DrawString(game.font1, "ENEMIES LEFT: " + (numPlayers - 1), new Vector2(0, 36), Color.White);
            if (nearestEnemy != 0)
            {
                if (nearestEnemy == 1)
                {
                    sp.DrawString(game.font1, "TO THE RIGHT!", new Vector2(0, 52), Color.White);
                }
                else
                {
                    sp.DrawString(game.font1, "TO THE LEFT!", new Vector2(0, 52), Color.White);
                }
            }
            if (waveTimer > 0)
            {
                if (waveTimer >= 100 && waveAlpha < 1.0f)
                {
                    waveAlpha += 0.1f;
                }
                else if (waveTimer <= 10 && waveAlpha > 0.0f)
                {
                    waveAlpha -= 0.1f;
                }
                if (wave == 1)
                {
                    sp.DrawString(game.font1, "GET READY!!!", new Vector2(112, 72), Color.White * waveAlpha);
                }
                else
                {
                    sp.DrawString(game.font1, "WAVE " + (wave - 1) + " COMPLETED", new Vector2(80, 72), Color.White * waveAlpha);
                }
            }
            if (player.dead)
            {
                sp.Draw(game.textures[2], deathThingy, deathThingyRect, Color.White);
                if (player.deathTimer == 150)
                {
                    fadeOut = 1.0f;
                }
            }
            if (fadeOut > 0.0f)
            {
                fadeOut -= 0.025f;
                if (fadeOut < 0.025f) fadeOut = 0.025f;
                sp.Draw(game.textures[2], sp.GraphicsDevice.Viewport.Bounds, hbRect, Color.Black * (1.0f - fadeOut));
            }
            if (fadeIn > 0.0f)
            {
                fadeIn -= 0.025f;
                sp.Draw(game.textures[2], sp.GraphicsDevice.Viewport.Bounds, hbRect, Color.Black * fadeIn);
            }
            if (game.pause)
            {
                sp.Draw(game.textures[2], new Vector2(100, 72), pauseThingyRect, Color.White);
            }
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
        public void Explode(float x, float y, float radius, int damage, bool hurtTerrain = false, Entity perpetrator = null)
        {
            int tx = (int)Math.Round(x / 16);
            int ty = (int)Math.Round(y / 16);
            int tr = (int)Math.Ceiling(radius / 16);
            Vector2 pos = new Vector2(x, y);
            if (hurtTerrain)
            {
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
                terrainModified = true;
            }
            for (int i = 0; i < entities.GetLength(1); i++)
            {
                if (entities[TYPE_PLAYER, i] != null)
                {
                    if (Vector2.Distance(pos, entities[TYPE_PLAYER, i].position) < radius + entities[TYPE_PLAYER, i].hitboxSize.X)
                    {
                        Player p = (Player)entities[TYPE_PLAYER, i];
                        p.Damage(damage, perpetrator);
                    }
                }
            }
            
            AddParticle(new Particle(this, pos - new Vector2(radius), Vector2.Zero, 3, 32, 2));
        }
    }
}
