using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace Mors_Arcium
{
    public class Gameplay
    {
        public class GUI
        {
            public Vector2 cameraPosition;
            public Vector2 deathThingy = new Vector2(100, 200);
            public float fadeIn = 1.0f;
            public float fadeOut = 0.0f;
            public int nearestEnemy = -1;
        }
        public const int TYPE_PLAYER = 6;
        public const int TYPE_PROJECTILE = 4;
        public const int TYPE_PROP = 1;
        public const int TYPE_ITEM = 2;
        public const int TYPE_BEAM = 7;

        private static readonly string[] GAMEPLAY_SONGS = { "asinosfacio", "frozenhell", "gasconade", "unholywars", "welcometohell" };

        public GameManager game;
        public bool started = false;

        public Entity[,] entities;
        public Tilemap tilemap;
        private Particle[] particles;
        public bool eventsEnabled = true;
        private float eventThingy = 240.0f;
        private Rectangle eventRect = new Rectangle(256, 128, 128, 24);

        int time;
        int ticks;
        int fps;
        Vector2 cameraOffset = new Vector2(160, 120);
        bool terrainModified = false;
        Rectangle deathThingyRect = new Rectangle(0, 32, 120, 65);
        Rectangle hbRect = new Rectangle(0, 97, 1, 1);
        Rectangle mbRect = new Rectangle(0, 98, 1, 1);
        Rectangle mbhbRect = new Rectangle(0, 0, 117, 32);
        Rectangle pauseThingyRect = new Rectangle(0, 99, 124, 25);
        public int healthPackFrequency = 500;
        public Player humanPlayer;
        public GUI gui;
        
        public int numCPUs = 20;
        public int numPlayers = 11;
        public int wave = 0;
        public int waveTimer = 0;
        private float waveAlpha = 1.0f;
        private int healthPackTimer = 500;
        string cheatString = "";
        
        int eventSelectorIndex = 0;
        int eventSelectorSpeed = 1;
        int eventSelectorTimer = 0;
        Rectangle eventSelectorText;
        public float gravityAcceleration = 0.15f;
        /*bool shakeScreen = false;
        float cameraShake = 0.0f;*/
        public float lavaHeight = 0.0f;
        public int lavaAnim = 0;
        Rectangle lavaTop;
        Rectangle lavaBase;
        public float defaultLavaHeight = 0.0f;
        int lavaTimer = 0;
        public int reloadOffset = 0;
        public Satan satan;

        public bool tutorial = false;
        int tutorialTimer = 0;
        public int tutorialPhase = 0;

        public string difficulty = "hard";
        int num_mr_b = 3;
        int num_wizard = 3;
        int num_fingers = 3;
        int num_bugs = 3;
        public float projectileSpeedMultiplier = 1.0f;

        public Gameplay(GameManager gameMan)
        {
            game = gameMan;
        }
        
        public void Initialize(int playerClass)
        {
            gui = new GUI();

            int mapw = 129;
            int plhlha = 0;
            if (tutorial)
            {
                difficulty = "normal";
                AudioSystem.ChangeMusic("welcometohell", true);
            }
            if (difficulty != "????")
            {
                StreamReader ronaldMcDonald = new StreamReader(difficulty + "_difficulty.txt");
                string nmb = ronaldMcDonald.ReadLine(); num_mr_b = int.Parse(nmb.Substring(nmb.IndexOf(':') + 1));
                string nw = ronaldMcDonald.ReadLine(); num_wizard = int.Parse(nw.Substring(nw.IndexOf(':') + 1));
                string ne = ronaldMcDonald.ReadLine(); num_fingers = int.Parse(ne.Substring(ne.IndexOf(':') + 1));
                string nb = ronaldMcDonald.ReadLine(); num_bugs = int.Parse(nb.Substring(nb.IndexOf(':') + 1));
                string hpf = ronaldMcDonald.ReadLine(); healthPackFrequency = int.Parse(hpf.Substring(hpf.IndexOf(':') + 1));
                string phh = ronaldMcDonald.ReadLine(); plhlha = int.Parse(phh.Substring(phh.IndexOf(':') + 1));
                string ee = ronaldMcDonald.ReadLine(); eventsEnabled = bool.Parse(ee.Substring(ee.IndexOf(':') + 1));
                string pso = ronaldMcDonald.ReadLine(); projectileSpeedMultiplier = float.Parse(pso.Substring(pso.IndexOf(':') + 1));
                string mw = ronaldMcDonald.ReadLine(); mapw = int.Parse(mw.Substring(mw.IndexOf(':') + 1));
                ronaldMcDonald.Close();
                ronaldMcDonald.Dispose();
#if ANDROID
            game.android.LoadDifficulty(difficulty + "_difficulty.txt");
#endif
            }
            else
            {
                num_mr_b = game.random.Next(0, 10);
                num_wizard = game.random.Next(0, 10);
                num_fingers = game.random.Next(0, 10);
                num_bugs = game.random.Next(0, 10);
                healthPackFrequency = game.random.Next(50, 1000);
                plhlha = game.random.Next(-60, 60);
                projectileSpeedMultiplier = ((float)game.random.NextDouble() * 2.0f) - 1.0f;
                mapw = game.random.Next(4, 200);
            }
            
            eventSelectorIndex = 0; eventSelectorSpeed = 1; eventSelectorTimer = 0;
            eventSelectorText = new Rectangle(256, 152, 124, 11);
            entities = new Entity[8, 128];
            particles = new Particle[128];
            if (tutorial)
            {
                tilemap = new Tilemap(this, game.textures[5], 25, 24);
                int h = 0;
                for (int i = 0; i < tilemap.width; i++)
                {
                    if (i < 5)
                    {
                        h += 1;
                    }
                    if (i >= 20)
                    {
                        h -= 1;
                    }
                    for (int j = h; j < tilemap.height; j++)
                    {
                        tilemap.data[i, j] = 1;
                    }
                }
                tilemap.RefreshTiles();
            }
            else
            {
                tilemap = new Tilemap(this, game.textures[5], mapw, 24);
                tilemap.Generate();
            }
            
            lavaHeight = (tilemap.height * 16) - 32;
            defaultLavaHeight = lavaHeight;
            lavaTop = new Rectangle(0, 80, 16, 16);
            lavaBase = new Rectangle(48, 80, 16, 16);
            
            humanPlayer = SpawnPlayer(playerClass, plhlha);
            if (tutorial)
            {
                humanPlayer.maxHealth = 600;
                humanPlayer.health = 600;
                healthPackTimer = 0;
                humanPlayer.position.X = 200;
            }
            
            
            wave = 1;
            waveTimer = 100;
            waveAlpha = 1.0f;
            numPlayers = 1 + num_mr_b + num_fingers + num_bugs + num_wizard;
            started = true;
            tutorialPhase = 0;
            tutorialTimer = 0;
            //Satan satan = new Satan(this, new Vector2(256, 0));
            //AddEntity(satan);
        }
        Player SpawnPlayer(int playerClass, int plhlha)
        {
            Player p;
            switch (playerClass)
            {
                case 1:
                    p = new WizardPlayer(this, plhlha);
                    break;
                case 2:
                    p = new EliPlayer(this, plhlha);
                    break;
                case 3:
                    p = new BugPlayer(this, plhlha);
                    break;
                default:
                    p = new MrBPlayer(this, plhlha);
                    break;
            }
            p.position = new Vector2(game.random.Next(32, (tilemap.width* 16) - 32), 0.0f);
            AddEntity(p);
            return p;
        }
        private void SpawnEnemies()
        {
            if (!tutorial || tutorialPhase == 11)
            {
                for (int i = 0; i < num_mr_b; i++)
                {
                    MrBPlayer p = new MrBPlayer(this);
                    p.position.X = game.random.Next(64, (tilemap.width * 16) - 64);
                    p.position.Y = -96.0f;
                    AddEntity(p);
                    p = null;
                }
                for (int i = 0; i < num_wizard; i++)
                {
                    WizardPlayer w = new WizardPlayer(this);
                    w.position.X = game.random.Next(64, (tilemap.width * 16) - 64);
                    w.position.Y = -96.0f;
                    AddEntity(w);
                    w = null;
                }
                for (int i = 0; i < num_fingers; i++)
                {
                    EliPlayer e = new EliPlayer(this);
                    e.position.X = game.random.Next(64, (tilemap.width * 16) - 64);
                    e.position.Y = -96.0f;
                    AddEntity(e);
                    e = null;
                }
                for (int i = 0; i < num_bugs; i++)
                {
                    BugPlayer b = new BugPlayer(this);
                    b.position.X = game.random.Next(64, (tilemap.width * 16) - 64);
                    b.position.Y = -96.0f;
                    AddEntity(b);
                    b = null;
                }
            }
        }
        public void Update(GameTime gt)
        {
            if ((Keyboard.GetState().IsKeyDown(Keys.Escape)) && game.currentMenu == null)
            {
                game.ChangeMenuState(new MainMenu(game));
            }
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
            if (!Settings.playedBefore)
            {
                Settings.playedBefore = true;
                Settings.Save();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D6))
            {
                eventSelectorIndex = 1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D5))
            {
                eventSelectorIndex = 0;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D7))
            {
                eventSelectorIndex = 4;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D8))
            {
                eventSelectorIndex = 6;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D9))
            {
                eventSelectorIndex = 2;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D0)) //Sudden death
            {
                eventSelectorIndex = 3;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D1) && !(humanPlayer is MrBPlayer))
            {
                ChangePlayerType(humanPlayer, 0);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D2) && !(humanPlayer is WizardPlayer))
            {
                ChangePlayerType(humanPlayer, 32);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D3) && !(humanPlayer is EliPlayer))
            {
                ChangePlayerType(humanPlayer, 64);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D4) && !(humanPlayer is BugPlayer))
            {
                ChangePlayerType(humanPlayer, 96);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.U))
            {
                humanPlayer.health += 5;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Y))
            {
                humanPlayer.health -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.T))
            {
                for (int i = 0; i < entities.GetLength(1); i++)
                {
                    if (entities[TYPE_PLAYER, i] != null)
                    {
                        if (humanPlayer.index == i) continue;
                        Player p = (Player)entities[TYPE_PLAYER, i];
                        p.health = 0;
                    }
                }
            }

            if (!humanPlayer.dead)
            {
                if (humanPlayer.deathTimer == 0 && humanPlayer.controllable)
                {
                    if (Settings.bindings.jump.IsDown())
                    {
                        humanPlayer.Jump();
                    }

                    if (Settings.bindings.right.IsDown())
                    {
                        humanPlayer.spriteEffects = SpriteEffects.None;
                        humanPlayer.Walk();
                    }
                    else if (Settings.bindings.left.IsDown())
                    {
                        humanPlayer.spriteEffects = SpriteEffects.FlipHorizontally;
                        humanPlayer.Walk();
                    }
                    if (Settings.bindings.up.IsDown())
                    {
                        humanPlayer.aimDirection = -1;
                    }
                    else if (Settings.bindings.down.IsDown())
                    {
                        humanPlayer.aimDirection = 1;
                    }
                    else
                    {
                        humanPlayer.aimDirection = 0;
                    }
                    if (Settings.bindings.attack.IsDown())
                    {
                        humanPlayer.Attack();
                    }
                    if (Settings.bindings.special.IsDown())
                    {
                        humanPlayer.Special();
                    }
                }
            }
            else
            {
                gui.deathThingy.Y -= (gui.deathThingy.Y - 72f) / 4;
                if (gui.deathThingy.Y < 73) gui.deathThingy.Y = 72;
                humanPlayer.deathTimer += 1;
            }
            AudioSystem.ListenerPosition = humanPlayer.position;


            if (healthPackTimer > 0 && eventSelectorIndex != 3)
            {
                healthPackTimer -= 1;
                if (healthPackTimer == 0)
                {
                    AddEntity(new HealthPack(this, new Vector2(game.random.Next(0, tilemap.width * 16), -96.0f)));
                    healthPackTimer = healthPackFrequency;
                }
            }

            numPlayers = 0;
            gui.nearestEnemy = 0;
            for (int x = 0; x < entities.GetLength(0); x++)
            {
                for (int y = 0; y < entities.GetLength(1); y++)
                {
                    if (entities[x, y] != null)
                    {
                        if (entities[x, y].type == TYPE_PLAYER)
                        {
                            Player p = (Player)entities[x, y];
                            if (!IsHuman(p)) p.UpdateCPU();
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

            /*if (shakeScreen)
            {
                cameraShake = ((float)game.random.NextDouble() - 0.5f) * 5.0f;
            }
            else
            {
                cameraShake = 0.0f;
            }*/
            /*if (cheatString.Contains("TDGMH"))
            {
                cheatString = "";
                Console.WriteLine("Give me hell!!!");
                for (int i = 0; i < entities.GetLength(1); i++)
                {
                    if (entities[TYPE_PLAYER, i] != null && entities[TYPE_PLAYER, i] != player)
                    {
                        Player p = (Player)entities[TYPE_PLAYER, i];
                        ChangePlayerType(p, 64);
                        p = null;
                    }
                }
            }*/
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
#if DEBUG
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                /*int x = (int)Math.Floor(((float)(Mouse.GetState().Position.X / game.scaleFactor) + cameraPosition.X) / 16);
                int y = (int)Math.Floor(((float)(Mouse.GetState().Position.Y / game.scaleFactor) + cameraPosition.Y) / 16);
                if (x < 0) x = 0;
                if (y < 0) y = 0;
                if (x >= tilemap.width) x = tilemap.width - 1;
                if (y >= tilemap.height) y = tilemap.height - 1;
                tilemap.data[x, y] = -1;
                tilemap.RefreshTiles();*/
            }
#endif
            if (terrainModified)
            {
                terrainModified = false;
                tilemap.RefreshTiles();
            }

            gui.cameraPosition = humanPlayer.position - cameraOffset;
            if (gui.cameraPosition.Y + 240 > tilemap.height * 16)
            {
                gui.cameraPosition.Y = (tilemap.height * 16) - 240;
            }

            if (numPlayers == 1 && waveTimer == 0 && !tutorial)
            {
                wave += 1;
                waveTimer = 200;
                waveAlpha = 0.0f;
                if (humanPlayer.deathTimer == 0)
                {
                    humanPlayer.health = humanPlayer.maxHealth;
                }
                eventSelectorIndex = game.random.Next(0, 7); eventSelectorSpeed = 1; eventSelectorTimer = 0;
                eventSelectorText.Y = 152 + (eventSelectorIndex * 11);
            }
            if (wave <= 1) eventSelectorIndex = 0;
            if (waveTimer > 0)
            {
                if (!tutorial)
                {
                    AudioSystem.ChangeMusic("");
                }
                waveTimer -= 1;
                if (eventsEnabled && wave > 1)//96, 120
                {
                    eventThingy -= (eventThingy - 192) / 4.0f;
                    if (waveTimer > 20) eventSelectorTimer += 1;
                    if (eventSelectorTimer > eventSelectorSpeed)
                    {
                        eventSelectorTimer = 0;
                        eventSelectorIndex += 1;
                        if (eventSelectorIndex > 7) eventSelectorIndex = 0;
                        eventSelectorText.Y = 152 + (eventSelectorIndex * 11);
                        AudioSystem.Play2DSound("slotmachine");
                    }
                    if (waveTimer % 10 == 0)
                    {
                        eventSelectorSpeed += 2;
                    }
                    lavaTimer = 0;
                }
                if (waveTimer == 0 && !tutorial)
                {
                    SpawnEnemies();

                    if (humanPlayer.dead)
                    {
                        humanPlayer = SpawnPlayer(game.random.Next(4), humanPlayer.healthHandicap);
                        gui = new GUI();
                    }

                    for (int y = 0; y < entities.GetLength(1); y++)
                    {
                        Player p = (Player)entities[TYPE_PLAYER, y];
                        if (p != null)
                        {
                            if (eventSelectorIndex == 3)
                            {
                                p.health = 1;
                            }
                        }
                    }
                    eventThingy = 240.0f;
                    //Play random music track
                    if (Settings.musicEnabled)
                    {
                        AudioSystem.ChangeMusic(GAMEPLAY_SONGS[game.random.Next(0, GAMEPLAY_SONGS.Length)]);
                    }
                }
            }
            if (humanPlayer.deathTimer > 200)
            {
                if (tutorial)
                {
                    game.ChangeMenuState(new MainMenu(game));
                    started = false;
                }
                else
                {
                    Initialize(game.random.Next(0, 3));
                }
            }
            lavaAnim += 1;

            if (eventSelectorIndex == 4)
            {
                gravityAcceleration = 0.05f;
            }
            else
            {
                gravityAcceleration = 0.15f;
            }

            if (eventSelectorIndex == 6 && waveTimer == 0)
            {
                //Rapid Fire! Oh yeah!
                reloadOffset = -15;
            }
            else
            {
                reloadOffset = 0;
            }

            if (eventSelectorIndex == 2 && satan == null && waveTimer == 0 && wave > 1)
            {
                satan = new Satan(this, new Vector2(game.random.Next(32, (tilemap.width - 2) * 16), 0.0f));
                AddEntity(satan);
            }
            if (eventSelectorIndex != 2 && satan != null)
            {
                satan = null;
            }
            //shakeScreen = false;
            if ((eventSelectorIndex != 1 || lavaTimer > 200) && lavaHeight < defaultLavaHeight)
            {
                lavaHeight += 4;
                //shakeScreen = true;
            }
            if (lavaAnim > 5)
            {
                if (eventSelectorIndex == 1 && eventThingy == 240.0f && lavaHeight > tilemap.height * 8 && wave > 1)
                {
                    lavaHeight -= 1;
                    //shakeScreen = true;
                }
                else if (lavaHeight <= tilemap.height * 8)
                {
                    lavaTimer += 1;
                }
                
                if (lavaHeight > defaultLavaHeight)
                {
                    lavaHeight = defaultLavaHeight;
                }
                lavaAnim = 0;
                lavaTop.X += 16;
                if (lavaTop.X > 32)
                {
                    lavaTop.X = 0;
                }
                lavaBase.X = lavaTop.X + 48;
            }
            if (tutorial)
            {
                tutorialTimer += 1;
                if (tutorialPhase == 0 && tutorialTimer > 525)
                {
                    tutorialTimer = 0;
                    tutorialPhase += 1;
                }
                else if (tutorialPhase == 1 && tutorialTimer > 250)
                {
                    tutorialTimer = 0;
                    tutorialPhase += 1;
                    //Spawn an enemy
                    WizardPlayer w = new WizardPlayer(this);
                    w.position.X = game.random.Next(64, (tilemap.width * 16) - 64);
                    w.position.Y = -96.0f;
                    AddEntity(w);
                    w = null;
                }
                else if ((tutorialPhase == 3 && tutorialTimer > 250) || (tutorialPhase == 6 && tutorialTimer > 350))
                {
                    tutorialTimer = 0;
                    tutorialPhase += 1;
                }
                else if (tutorialPhase == 4 && tutorialTimer > 250)
                {
                    tutorialTimer = 0;
                    tutorialPhase += 1;
                    BugPlayer g = new BugPlayer(this);
                    g.position.X = game.random.Next(64, (tilemap.width * 16) - 64);
                    g.position.Y = -96.0f;
                    AddEntity(g);
                    g = null;
                }
                else if (tutorialPhase == 7 && tutorialTimer > 250)
                {
                    tutorialTimer = 0;
                    tutorialPhase += 1;
                    EliPlayer e = new EliPlayer(this);
                    e.position.X = game.random.Next(64, (tilemap.width * 16) - 64);
                    e.position.Y = -96.0f;
                    AddEntity(e);
                    e = null;
                }
                else if (tutorialPhase == 9 && tutorialTimer > 250)
                {
                    tutorialTimer = 0;
                    tutorialPhase += 1;
                    MrBPlayer b = new MrBPlayer(this);
                    b.position.X = 200;
                    b.position.Y = -96.0f;
                    AddEntity(b);
                    b = null;
                }
                if (tutorialPhase == 2 && numPlayers == 1 && tutorialTimer > 10)
                {
                    tutorialPhase += 1;
                    tutorialTimer = 0;
                    ChangePlayerType(humanPlayer, 32);
                    humanPlayer.health = 600;
                    humanPlayer.maxHealth = 600;
                }
                if (tutorialPhase == 5 && numPlayers == 1 && tutorialTimer > 10)
                {
                    tutorialPhase += 1;
                    tutorialTimer = 0;
                    ChangePlayerType(humanPlayer, 96);
                    humanPlayer.health = 600;
                    humanPlayer.maxHealth = 600;
                }
                if (tutorialPhase == 8 && numPlayers == 1 && tutorialTimer > 10)
                {
                    tutorialPhase += 1;
                    tutorialTimer = 0;
                    ChangePlayerType(humanPlayer, 64);
                    humanPlayer.health = 600;
                    humanPlayer.maxHealth = 600;
                }
                if (tutorialPhase == 10 && numPlayers == 1 && tutorialTimer > 10)
                {
                    tutorialPhase += 1;
                    tutorialTimer = 0;
                    ChangePlayerType(humanPlayer, 0);
                    SpawnEnemies();
                    SpawnEnemies();
                }
                if (tutorialPhase == 11 && numPlayers == 1 && tutorialTimer > 10)
                {
                    AudioSystem.ChangeMusic("skelesong", true);
                }
                if (tutorialPhase == 11 && numPlayers == 1 && tutorialTimer % 50 == 0 && tutorialTimer > 10)
                {
                    SpookyScarySkeleton s = new SpookyScarySkeleton(this);
                    s.position = new Vector2(game.random.Next(0, 400), -64.0f);
                    AddEntity(s);
                    s = null;
                    
                }
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
            sp.GraphicsDevice.Clear(Color.DarkBlue);
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Matrix.CreateTranslation(new Vector3(-gui.cameraPosition, 0f)));
            for (int y = 0; y < entities.GetLength(1); y++)
            {
                if (entities[TYPE_PROP, y] != null)
                {
                    entities[TYPE_PROP, y].Draw(sp);
                }
            }
            for (int y = 0; y < entities.GetLength(1); y++)
            {
                if (entities[TYPE_BEAM, y] != null)
                {
                    entities[TYPE_BEAM, y].Draw(sp);
                }
            }
            tilemap.Draw(sp, gui);
            if (tutorial)
            {
                switch (tutorialPhase)
                {
                    case 0:
                        sp.DrawString(game.font1, "USE " + Settings.bindings.left + " AND " + Settings.bindings.right + " TO WALK.", new Vector2(72, -16.0f), Color.White);
                        sp.DrawString(game.font1, "USE " + Settings.bindings.jump + " TO JUMP.", new Vector2(72, 0), Color.White);
                        sp.DrawString(game.font1, "USE " + Settings.bindings.attack + " TO ATTACK.", new Vector2(72, 16), Color.White);
                        sp.DrawString(game.font1, "USE " + Settings.bindings.up + " AND " + Settings.bindings.down + " TO AIM.", new Vector2(72, 32), Color.White);
                        sp.DrawString(game.font1, "USE " + Settings.bindings.special + " TO USE A SPECIAL ABILITY", new Vector2(72, 48), Color.White);
                        break;
                    case 9:
                    case 7:
                    case 4:
                    case 1:
                        sp.DrawString(game.font1, "AN ENEMY IS BEING SENT YOUR WAY...", new Vector2(52, 0.0f), Color.White);
                        break;
                    case 3:
                        sp.DrawString(game.font1, "IT FEELS GOOD TO WEAR THE SKIN", new Vector2(56, 0.0f), Color.White);
                        sp.DrawString(game.font1, "OF YOUR ENEMIES, DOESN'T IT?", new Vector2(56, 16.0f), Color.White);
                        break;
                    case 6:
                        sp.DrawString(game.font1, "YOU CAN FLY NOW.", new Vector2(56, 0.0f), Color.White);
#if WINDOWS
                        sp.DrawString(game.font1, "(USE YOUR SPECIAL ABILITY IN THE AIR)", new Vector2(56, 16.0f), Color.White);
#endif
#if ANDROID
                    sp.DrawString(game.font1, "(HOLD J WHILE IN THE AIR)", new Vector2(56, 16.0f), Color.White);
#endif
                        break;
                    case 11:
                        sp.DrawString(game.font1, "GOOD WORK. NOW DIE!!!", new Vector2(80, 0.0f), Color.White);
                        break;
                }

            }
            for (int x = 0; x < entities.GetLength(0); x++)
            {
                if (x != TYPE_BEAM && x != TYPE_PROP)
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
            }
            if (lavaHeight < gui.cameraPosition.Y + 240)
            {
                int cx = (int)Math.Floor(gui.cameraPosition.X / 16.0f);
                int ch = (int)Math.Ceiling((gui.cameraPosition.Y + 240 - lavaHeight) / 16.0f);
                for (int j = 0; j < 22; j++)
                {
                    sp.Draw(game.textures[5], new Vector2((cx + j) * 16, lavaHeight), lavaTop, Color.White);
                    for (int k = 1; k < ch; k++)
                    {
                        sp.Draw(game.textures[5], new Vector2((cx + j) * 16, lavaHeight + (k * 16)), lavaBase, Color.White);
                    }
                }
            }



            sp.End();
            sp.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Matrix.CreateTranslation(new Vector3(-gui.cameraPosition, 0f)));
            for (int j = 0; j < particles.Length; j++)
            {
                if (particles[j] != null) particles[j].Draw(sp);
            }
            sp.End();
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null); //HUD
            //if (!game.paused)
            {
#if DEBUG
            sp.DrawString(game.font1, "FPS: " + fps, new Vector2(0, 120), Color.White);
#endif
                sp.Draw(game.textures[2], Vector2.Zero, mbhbRect, Color.White);
                sp.Draw(game.textures[2], new Rectangle(12, 2, (int)(((float)humanPlayer.health / humanPlayer.maxHealth) * 104.0f), 12), hbRect, Color.White);
                sp.Draw(game.textures[2], new Rectangle(12, 18, (int)(((float)humanPlayer.magic / humanPlayer.maxMagic) * 104.0f), 12), mbRect, Color.White);
                if (!tutorial)
                {
                    sp.DrawString(game.font1, "WAVE " + wave, new Vector2(252, 0), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                    if (humanPlayer.deathTimer == 0 && humanPlayer.dead == false) sp.DrawString(game.font1, "ENEMIES LEFT: " + (numPlayers - 1), new Vector2(0, 36), Color.White);
                    if (gui.nearestEnemy != 0 && humanPlayer.deathTimer == 0 && humanPlayer.dead == false)
                    {
                        if (gui.nearestEnemy == 1)
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
                            if (wave != 1) sp.DrawString(game.font1, "WAVE " + (wave - 1) + " COMPLETED", new Vector2(80, 72), Color.White * waveAlpha);
                            sp.Draw(game.textures[2], new Vector2(96, eventThingy), eventRect, Color.White * waveAlpha);
                            sp.Draw(game.textures[2], new Vector2(103, eventThingy + 7), eventSelectorText, Color.White * waveAlpha);
                        }

                    }
                }
                if (humanPlayer.dead)
                {
                    sp.Draw(game.textures[2], gui.deathThingy, deathThingyRect, Color.White);
                    if (humanPlayer.deathTimer == 150)
                    {
                        gui.fadeOut = 1.0f;
                    }
                }
#if ANDROID
            game.android.DrawControls(sp);
#endif
            }
            if (gui.fadeOut > 0.0f)
            {
                gui.fadeOut -= 0.025f;
                if (gui.fadeOut < 0.025f) gui.fadeOut = 0.025f;
                sp.Draw(game.textures[2], sp.GraphicsDevice.Viewport.Bounds, hbRect, Color.Black * (1.0f - gui.fadeOut));
            }
            if (gui.fadeIn > 0.0f)
            {
                gui.fadeIn -= 0.025f;
                sp.Draw(game.textures[2], sp.GraphicsDevice.Viewport.Bounds, hbRect, Color.Black * gui.fadeIn);
            }
            /*if (game.paused)
            {
                sp.Draw(game.textures[2], new Vector2(100, 72), pauseThingyRect, Color.White);
                sp.DrawString(game.font1, "PRESS ESCAPE TO RETURN TO THE MENU", new Vector2(4, 32), Color.White);
            }*/
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
        public void ReplaceEntity(Entity e, int index)
        {
            entities[e.type, index] = null;
            e.index = index;
            entities[e.type, index] = e;
        }
        public void RemoveEntity(Entity e)
        {
            entities[e.type, e.index] = null;
        }
        public bool IsHuman(Player p)
        {
            return humanPlayer == p;
        }
        public void ChangePlayerType(Player p, int srcRctY)
        {
            if (srcRctY != p.sourceRect.Y)
            {
                p.killMe = true;
                Player newPlayer = null;
                switch (srcRctY)
                {
                    default:
                    case 0:
                        newPlayer = new MrBPlayer(this);
                        break;
                    case 32:
                        newPlayer = new WizardPlayer(this);
                        break;
                    case 64:
                        newPlayer = new EliPlayer(this);
                        break;
                    case 96:
                        newPlayer = new BugPlayer(this);
                        break;
                }
                if (IsHuman(p))
                {
                    humanPlayer = newPlayer;
                }
                newPlayer.position = p.position;
                newPlayer.health = p.health;
                newPlayer.magic = p.magic;
                newPlayer.maxHealth += p.healthHandicap;
                newPlayer.spriteEffects = p.spriteEffects;
                AddEntity(newPlayer);
            }
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
