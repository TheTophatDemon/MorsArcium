﻿using System;
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
        public SpriteEffects spriteEffects;

        protected float gravity = 0.0f;
        protected float jump = 0.0f;
        protected float walk = 0.0f;

        protected float jumpHeight = 5.0f;
        protected float acceleration = 0.25f;
        protected float walkSpeed = 2.77f;
        protected int attackSpeed = 70;

        protected bool tryingToJump = false;
        protected bool attacking = false;
        protected int cooldown = 0;
        public int aimDirection = 0;

        protected string animationState = "idle";
        public bool dead = false;
        public int deathTimer = 0;

        public int hurtTimer = 0;

        public Player target;
        private string aiState = "";

        public static int[] PlayerCollisionMask = new int[] { Gameplay.TYPE_PROJECTILE, Gameplay.TYPE_PLAYER };
        public Player(Gameplay g) : base(g)
        {
            collisionMask = PlayerCollisionMask;
            texture = g.game.textures[0];
            type = Gameplay.TYPE_PLAYER;
            sourceRect = new Rectangle(0, 0, 32, 32);
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
                //onSlope = -1;
            }
            
        }
        public void Walk()
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
        }
        public virtual void Attack()
        {
            
        }
        public virtual void Special()
        {
            //yep. that's right. it's literally nothing. and it's not going to be anything, either. heh heh heh... ya get it?
        }
        public virtual void UpdateCPU()
        {
            if (target != null)
            {
                if (target.position.X < position.X)
                {
                    spriteEffects = SpriteEffects.FlipHorizontally;
                }
                else
                {
                    spriteEffects = SpriteEffects.None;
                }
                Walk();
                for (int i = 0; i < game.tilemap.jumpNodeCount; i++)
                {
                    if (position.X + hitboxOffset.X + hitboxSize.X > game.tilemap.jumpNodes[i].X - 24.0f && position.X + hitboxOffset.X - hitboxSize.X < game.tilemap.jumpNodes[i].X + 24.0f)
                    {
                        bool fnickel = false;
                        if (position.Y + hitboxOffset.Y + hitboxSize.Y <= game.tilemap.jumpNodes[i].Y + 16)
                        {
                            fnickel = true;
                        }
                        else if (target.position.Y < position.Y)
                        {
                            fnickel = true;
                        }
                        if (fnickel) Jump();
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
            if (cooldown < attackSpeed - 20) attacking = false;
            if (magic < maxMagic) magic += 1;
            walk *= 0.9f;
            if (Math.Abs(walk) < 0.1f) walk = 0.0f;
            gravity += 0.15f;
            if (gravity > 8.0f) gravity = 8.0f;
            if ((collision_bottom && onSlope == -1) || (onSlope != -1 && wasOnSlope == -1))
            {
                if (gravity >= 7.0f)
                {
                    game.AddParticle(new Particle(game, new Vector2(position.X - 4, position.Y + hitboxSize.Y + hitboxOffset.Y), Vector2.Zero, 5, 8, 1));
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
                walk = 0.0f;
                jump = 0.0f;
            }
            speed = new Vector2(walk, gravity + jump);
            /*
            speed = Vector2.Zero;
            if (Keyboard.GetState().IsKeyDown(Keys.S)) speed.Y = 2.5f;
            if (Keyboard.GetState().IsKeyDown(Keys.A)) speed.X = -2.5f;
            if (Keyboard.GetState().IsKeyDown(Keys.D)) speed.X = 2.5f;
            if (Keyboard.GetState().IsKeyDown(Keys.W)) speed.Y = -2.5f;*/
            
            TryMove(speed);
            Animate();
            tryingToJump = false;
            if (health <= 0 && deathTimer == 0)
            {
                health = 0;
                deathTimer = 1;
                ChangeAnimationState("dead");
            }
            if (deathTimer >= 1)
            {
                deathTimer += 1;
                if (deathTimer == 100)
                {
                    dead = true;
                    game.Explode(position.X, position.Y, 16f, 15);
                    game.RemoveEntity(this);
                }
            }
        }
        public override void Draw(SpriteBatch sp)
        {
            if (hurtTimer == 0 || hurtTimer % 4 == 0)
            {
                sp.Draw(texture, position, sourceRect, Color.White, rotation, origin, scale, spriteEffects, 0);
            }
        }
        public void Damage(int amount)
        {
            health -= amount;
            hurtTimer = 30;
        }
        public override void Collide(Entity perpetrator)
        {
            
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
                //if (frame != lastFrame)
                //{
                    sourceRect.X = animation.frames[frame] * 32;
                //}
                //lastFrame = frame;
            }
        }
        protected virtual void SyncAnimationWithState()
        {
            
        }
    }
}
