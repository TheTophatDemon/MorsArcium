﻿/*
Copyright (C) 2016-present Alexander Lunsford

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mors_Arcium
{
    public class Player : Entity
    {
        public int maxHealth = 100;
        public int maxMagic = 100;
        public int health = 100;
        public int magic = 100;
        public int healthHandicap = 0;
        
        public SpriteEffects spriteEffects;
        protected int sheetOffset;
        protected Color color = Color.White;

        protected float gravity = 0.0f;
        protected float jump = 0.0f;
        protected float walk = 0.0f;

        protected float jumpHeight = 5.0f;
        protected float acceleration = 0.25f;
        protected float walkSpeed = 2.5f;
        protected int attackSpeed = 70;

        protected bool tryingToJump = false;
        protected bool tryingToWalk = false;
        protected bool attacking = false;
        protected int cooldown = 0;
        public int aimDirection = 0;

        public bool controllable = true;

        protected float maxGravity = 8.0f;

        protected string animationState = "idle";
        public bool dead = false;
        public int deathTimer = 0;

        public int hurtTimer = 0;

        public Entity target;
        protected string aiState = "chase";
        protected int giveUpTimer = 0;
        protected float lastX = 0.0f;
        protected float runDistance;
        protected float runOrigin;
        public Vector2 knockback = Vector2.Zero;

        public static int[] PlayerCollisionMask = new int[] { Gameplay.TYPE_PROJECTILE, Gameplay.TYPE_PLAYER };
        public Player(Gameplay g, int hhh) : base(g)
        {
            healthHandicap = hhh;
            collisionMask = PlayerCollisionMask;
            texture = g.game.textures["characters"];
            type = Gameplay.TYPE_PLAYER;
            
            hitboxSize = new Vector2(8, 16); //Half of the actual hitbox's size
            hitboxOffset = new Vector2(0, 0);
            origin = new Vector2(16, 16);
            /*
            switch (pt)
            {
                case TYPE_MR_B:
                    
                    break;
                case TYPE_WIZARD:

                    break;
            }
            animation = idleAnimation;*/
        }
        public void Jump()
        {
            tryingToJump = true;
            if (collision_bottom)
            {
                jump = -jumpHeight;
                gravity = 0.0f;
                onSlope = -1;
            }
            
        }
        public void Walk()
        {
            if (deathTimer == 0)
            {
                if (spriteEffects == SpriteEffects.None)
                {
                    walk += acceleration;
                    if (walk > walkSpeed) walk = walkSpeed;
                }
                else
                {
                    walk += -acceleration;
                    if (walk < -walkSpeed) walk = -walkSpeed;
                }
                tryingToWalk = true;
            }
        }
        public virtual void Attack()
        {
            
        }
        public virtual void Special()
        {
            //yep. that's right. it's literally nothing. and it's not going to be anything, either. heh heh heh... ya get it?
        }
        public virtual void CPUAttack()
        {
            walk = 0.0f;
            if (target.position.X < position.X)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            else
            {
                spriteEffects = SpriteEffects.None;
            }
            Attack();
            if (target.position.Y + target.hitboxOffset.Y + target.hitboxSize.Y < position.Y)
            {
                aimDirection = -1;
            }
            else if (target.position.Y + target.hitboxOffset.Y - target.hitboxSize.Y > position.Y)// + hitboxOffset.Y + hitboxSize.Y
            {
                aimDirection = 1;
            }
            else
            {
                aimDirection = 0;
            }
            float disp = Math.Abs(target.position.X - position.X);
            if (disp > 160.0f)
            {
                aiState = "chase";
            }
            if (disp < 32.0f)
            {
                aiState = "run";
                runOrigin = position.X;
                runDistance = 64.0f;
            }
        }
        public virtual void CPUChase()
        {
            if (Vector2.Distance(target.position, position) < 96.0f && position.X < (game.tilemap.width * 16) - 64.0f && position.X > 64.0f && target is Player)
            {
                aiState = "attack";
            }
            if (target.position.X < position.X)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            else
            {
                spriteEffects = SpriteEffects.None;
            }
            if (position.X < 0.0f || position.X > game.tilemap.width * 16)
            {
                aiState = "run";
                runOrigin = position.X;
                runDistance = 8;
            }
            Walk();
            if (position.X == lastX)
            {
                //giveUpTimer += 1;
                if (giveUpTimer > 100)
                {
                    giveUpTimer = 0;
                    aiState = "run";
                    runOrigin = position.X;

                    if (spriteEffects == SpriteEffects.FlipHorizontally)
                    {
                        spriteEffects = SpriteEffects.None;
                        runDistance = ((game.tilemap.width * 16) - 64) - position.X;
                    }
                    else
                    {
                        spriteEffects = SpriteEffects.FlipHorizontally;
                        runDistance = position.X - 64.0f;
                    }
                }
            }
            else
            {
                giveUpTimer = 0;
            }
        }
        public virtual void CPURun()
        {
            Walk();
            if (Math.Abs(position.X - runOrigin) >= runDistance)
            {
                aiState = "chase";
            }
            if (position.X < 64.0f || position.X > (game.tilemap.width * 16) - 64.0f)
            {
                aiState = "run";
                if (position.X < 64.0f)
                {
                    runOrigin = position.X;
                    spriteEffects = SpriteEffects.None;
                    runDistance = 64.0f;
                }
                else if (position.X > (game.tilemap.width * 16) - 64.0f)
                {
                    runOrigin = position.X;
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    runDistance = 64.0f;
                }
            }
        }
        public virtual void CPUDodge()
        {
            for (int i = 0; i < game.entities.GetLength(1); i++)
            {
                if (game.entities[Gameplay.TYPE_PROJECTILE, i] != null)
                {
                    Projectile p = (Projectile)game.entities[Gameplay.TYPE_PROJECTILE, i];
                    if (p.owner != this)
                    {
                        if (Vector2.Distance(p.position, position) < p.dodgeDistance && game.game.random.Next(0, 14) == 1)
                        {
                            if (p.position.Y > position.Y + hitboxOffset.Y - hitboxSize.Y)
                            {
                                Jump();
                            }
                            p = null;
                            break;
                        }
                    }
                    p = null;
                }
                else if (game.entities[Gameplay.TYPE_PLAYER, i] != null)
                {
                    if (game.entities[Gameplay.TYPE_PLAYER, i] is EliPlayer)
                    {
                        if (Vector2.Distance(game.entities[Gameplay.TYPE_PLAYER, i].position, position) < 64.0f && game.game.random.Next(0, 14) == 1)
                        {
                            if (game.entities[Gameplay.TYPE_PLAYER, i].position.Y > position.Y + hitboxOffset.Y - hitboxSize.Y)
                            {
                                Jump();
                            }
                            break;
                        }
                    }
                }
                else if (game.entities[Gameplay.TYPE_ITEM, i] != null)
                {
                    if (game.entities[Gameplay.TYPE_ITEM, i] is HealthPack)
                    {
                        if (Math.Abs(game.entities[Gameplay.TYPE_ITEM, i].position.X - position.X) < 160 && health < maxHealth / 2)
                        {
                            target = game.entities[Gameplay.TYPE_ITEM, i];
                            aiState = "chase";
                        }
                    }
                }
            }
        }
        public virtual void UpdateCPU()
        {
            if (deathTimer == 0)
            {
                if (target != null)
                {
                    bool imTheTrashManIEatGarbage = false;
                    if (target.killMe) imTheTrashManIEatGarbage = true;
                    if (target.position.X < 0 || target.position.X > game.tilemap.width * 16)
                    {
                        imTheTrashManIEatGarbage = true;
                    }
                    if (imTheTrashManIEatGarbage)
                    {
                        target = null;
                    }
                }
                if (target != null && target != this)
                {
                    if (game.lavaHeight != game.defaultLavaHeight && position.Y > (game.tilemap.height * 8) - 32.0f)
                    {
                        aiState = "run";
                        runOrigin = position.X;
                        runDistance = 128.0f;
                    }
                    if (aiState == "chase")
                    {
                        CPUChase();
                    }
                    else if (aiState == "run")
                    {
                        CPURun();
                    }
                    else if (aiState == "attack")
                    {
                        CPUAttack();
                    }

                    //DODGE!!!!
                    CPUDodge();
                    if (target is Player)
                    {
                        Player p = (Player)target;
                        if (p.deathTimer != 0)
                        {
                            target = null;
                        }
                        p = null;
                    }
                    if (target is HealthPack)
                    {
                        target = null;
                    }
                }
                else
                {
                    Walk();
                    if (position.X < 64.0f)
                    {
                        spriteEffects = SpriteEffects.None;
                    }
                    else if (position.X > (game.tilemap.width - 4) * 16)
                    {
                        spriteEffects = SpriteEffects.FlipHorizontally;
                    }
                    for (int i = 0; i < game.entities.GetLength(1); i++)
                    {
                        if (game.entities[Gameplay.TYPE_PLAYER, i] != null && game.entities[Gameplay.TYPE_PLAYER, i] != this)
                        {
                            Player p = (Player)game.entities[Gameplay.TYPE_PLAYER, i];
                            if (Vector2.Distance(game.entities[Gameplay.TYPE_PLAYER, i].position, position) < 160.0f && p.deathTimer == 0)
                            {
                                target = game.entities[Gameplay.TYPE_PLAYER, i];
                                break;
                            }
                            p = null;
                        }
                    }
                }
                //Jump at jump nodes
                if (aiState == "run" || aiState == "chase" || aiState == "attack")
                {
                    for (int i = 0; i < game.tilemap.jumpNodeCount; i++)
                    {
                        if (position.X + hitboxOffset.X + hitboxSize.X > game.tilemap.jumpNodes[i].X - 24.0f && position.X + hitboxOffset.X - hitboxSize.X < game.tilemap.jumpNodes[i].X + 24.0f)
                        {
                            bool fnickel = false;
                            if (target != null)
                            {
                                if (position.Y + hitboxOffset.Y - hitboxSize.Y <= game.tilemap.jumpNodes[i].Y + 16)
                                {
                                    fnickel = true;
                                }
                                else if (target.position.Y < position.Y)
                                {
                                    fnickel = true;
                                }
                            }
                            else
                            {
                                fnickel = true;
                            }
                            if (fnickel) Jump();
                        }
                    }
                }
            }
        }
        protected virtual void ChangeAnimationState(string st)
        {
            string lastState = animationState;
            animationState = st;
            if (lastState != st)
            {
                frame = 0;
                anim = 0;
            }
        }
        protected virtual void UpdateAnimationState()
        {
            
        }
        public override void Update(GameTime gt)
        {
            if (hurtTimer > 0) hurtTimer -= 1;
            UpdateAnimationState();
            if (cooldown > 0) cooldown -= 1;
            if (cooldown < attackSpeed - 20 || cooldown == 0) attacking = false;
            
            if (!tryingToWalk)
            {
                walk *= 0.9f;
                if (Math.Abs(walk) < 0.05f) walk = 0.0f;
            }
            if (collision_left && walk < 0.0f) walk = 0.0f;
            if (collision_right && walk > 0.0f) walk = 0.0f;
            if (knockback.X != 0.0f)
            {
                knockback.X *= 0.93f;
                if (Math.Abs(knockback.X) < 0.1f) knockback.X = 0.0f;
            }
            if (knockback.Y != 0.0f)
            {
                knockback.Y *= 0.93f;
                if (Math.Abs(knockback.Y) < 0.1f) knockback.Y = 0.0f;
            }
            tryingToWalk = false;
            gravity += game.gravityAcceleration;
            if (gravity > maxGravity) gravity = maxGravity;
            if ((collision_bottom && onSlope == -1) || (onSlope != -1 && wasOnSlope == -1))
            {
                if (gravity >= 7.0f)
                {
                    game.AddParticle(new Particle(game, new Vector2(position.X - 4, position.Y + hitboxSize.Y + hitboxOffset.Y), Vector2.Zero, 5, 8, 1));
                    //game.PlaySound(4, position);
                }
            }
            if ((collision_bottom && onSlope == -1) || (onSlope == -1 && wasOnSlope != -1))
            {
                //Console.WriteLine("BEPIS");
                gravity = 0.0f;    
            }
            if (collision_bottom)
            {
                if (!tryingToJump) jump = 0.0f;
            }
            if (!tryingToJump || gravity + jump > 0.0f)
            {
                jump = jump + 0.15f;
                if (Math.Abs(jump) <= 0.15f) jump = 0.0f;
            }
            if (collision_top)
            {
                jump = 0.0f;
            }
            if (deathTimer > 0)
            {
                //walk = 0.0f;
                jump = 0.0f;
            }
            speed = new Vector2(walk, gravity + jump) + knockback;
            /*
            speed = Vector2.Zero;
            if (Keyboard.GetState().IsKeyDown(Keys.S)) speed.Y = 2.5f;
            if (Keyboard.GetState().IsKeyDown(Keys.A)) speed.X = -2.5f;
            if (Keyboard.GetState().IsKeyDown(Keys.D)) speed.X = 2.5f;
            if (Keyboard.GetState().IsKeyDown(Keys.W)) speed.Y = -2.5f;*/
            if (position.Y + hitboxOffset.Y + hitboxSize.Y > game.lavaHeight + 64)
            {
                speed.Y = 0.0f;
            }
            lastX = position.X;
            TryMove(speed);
            
            tryingToJump = false;
            
            if (health <= 0 && deathTimer == 0)
            {
                health = 0;
                deathTimer = 1;
                ChangeAnimationState("dead");
                game.game.audio.Play3DSound("freeze", position);
            }
            if (position.Y + hitboxOffset.Y + hitboxSize.Y > game.lavaHeight && deathTimer == 0)
            {
                if (game.tutorial && game.IsHuman(this) && game.tutorialPhase != 11)
                {
                    knockback.Y = -64.0f;
                }
                else
                {
                    health = 0;
                }
                //deathTimer = 99;
                //Console.WriteLine("Player fell into pit: " + sourceRect.Y);
            }
            
            Animate();
            if (deathTimer >= 1)
            {
                deathTimer += 1;
                if (deathTimer == 100)
                {
                    dead = true;
                    game.Explode(position.X, position.Y, 16f, 15);
                    game.game.audio.Play3DSound("die", position);
                    for(int i = 0; i < 5; i++)
                    {
                        Particle p = new Particle(game, position, new Vector2(((float)game.game.random.NextDouble() * 5f) - 2.5f, -((float)game.game.random.NextDouble() * 2.5f) - 1.5f), 100, 8, 3);
                        p.baseX = game.game.random.Next(9, 13) * 8;
                        game.AddParticle(p);
                        p = null;
                    }
                    killMe = true;
                    game.numPlayers -= 1;
                }
            }
            if (health > maxHealth) health = maxHealth;
            if (magic > maxMagic) magic = maxMagic;
            if (magic < 0) magic = 0;
        }
        public override void Draw(SpriteBatch sp)
        {
            if (hurtTimer == 0 || hurtTimer % 4 == 0)
            {
                sp.Draw(texture, position, sourceRect, color, rotation, origin, scale, spriteEffects, 0);
            }
        }
        public virtual void Damage(int amount, Entity perpetrator = null)
        {
            if (hurtTimer == 0)
            {
                if (perpetrator is EliPlayer) game.game.audio.Play3DSound("jump", position);
                if (game.reloadOffset >= 0)
                {
                    health -= amount;
                }
                else
                {
                    health -= (amount / 2);
                }
                hurtTimer = 30;
                if (perpetrator is EliPlayer)
                {
                    target = perpetrator;
                }
            }
            if (perpetrator != null && health <= 0 && deathTimer == 0)
            {
                if (perpetrator is Player)
                {
                    Player p = (Player)perpetrator;
                    game.ChangePlayerType(p, sourceRect.Y);
                    p = null;
                }
            }
        }
        public override void Collide(Entity perpetrator)
        {
            if (perpetrator is Projectile)
            {
                Projectile p = (Projectile)perpetrator;
                if (p.owner != this)
                {
                    target = (Player)p.owner;
                }
                p = null;
            }
            if (perpetrator is EliPlayer)
            {
                target = perpetrator;
            }
        }
        private void Animate()
        {
            if (animation.frames != null)
            {
                anim += 1;
                if (anim > animation.speed)
                {
                    anim = 0;
                    frame += 1;
                    if (frame > animation.frames.Length - 1)
                    {
                        frame = 0;
                        if (!animation.looping)
                        {
                            frame = animation.frames.Length - 1;
                        }
                        if (animation.transition)
                        {
                            SyncAnimationWithState();
                        }
                    }
                }
                if (frame >= animation.frames.Length)
                {
                    frame = 0;
                }
                //if (frame != lastFrame)
                //{
                    sourceRect.X = (animation.frames[frame] % (texture.Width / sourceRect.Width)) * sourceRect.Width;
                    sourceRect.Y = sheetOffset;
                //}
                //lastFrame = frame;
            }
        }
        protected virtual void SyncAnimationWithState()
        {
            
        }
    }
}
