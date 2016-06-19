using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mors_Arcium
{
    public class MrBPlayer : Player
    {
        Animation idleAnimation;
        Animation walkAnimation;
        Animation jumpAnimation;
        Animation attackAnimation;
        Animation walkAttackAnimation;
        Animation aboutToAttackAnimation;
        Animation teleportAnimation;
        Animation deathAnimation;
        Particle eyeFlash;
        static Vector2 flashOffset = new Vector2(-7, -7);
        static Vector2 flashOffset2 = new Vector2(-2, -7);
        bool desperate = false;
        public MrBPlayer(Gameplay g) : base(g)
        {

            attackSpeed = 35;
            //Room for optimization if neeeded
            idleAnimation.frames = new int[] { 0, 1, 2, 1 };
            idleAnimation.speed = 5;
            idleAnimation.looping = true;
            walkAnimation.frames = new int[] { 3, 4, 5, 6 };
            walkAnimation.speed = 3;
            walkAnimation.looping = true;
            jumpAnimation.frames = new int[] { 7, 8, 9, 10 };
            jumpAnimation.speed = 3;
            jumpAnimation.looping = false;
            attackAnimation.frames = new int[] { 11, 11 };
            attackAnimation.looping = false;
            attackAnimation.speed = 3;
            walkAttackAnimation.frames = new int[] { 12, 13, 14, 15 };
            walkAttackAnimation.looping = true;
            walkAttackAnimation.speed = 3;
            aboutToAttackAnimation.frames = new int[] { 23 };
            aboutToAttackAnimation.looping = false;
            aboutToAttackAnimation.speed = 3;
            aboutToAttackAnimation.transition = true;
            teleportAnimation.frames = new int[] { 16, 17, 18, 19, 18, 17, 16 };
            teleportAnimation.looping = false;
            teleportAnimation.speed = 3;
            deathAnimation.frames = new int[] { 20, 21, 22 };
            deathAnimation.looping = false;
            deathAnimation.speed = 5;
            animation = idleAnimation;
            maxHealth = 80;
            maxMagic = 100;
            health = 80;
            walkSpeed = 3.0f;
        }
        public override void Update(GameTime gt)
        {
            if (magic < maxMagic) magic += 1;
            /*if (health < 20 && aiState == "attack" && target != null && !desperate)
            {
                
                if (Math.Abs(target.position.X - position.X) > 176.0f)
                {
                    aiState = "chase";
                }
                else
                {
                    if (position.X < 64.0f || position.X > (game.tilemap.width * 16) - 64.0f)
                    {
                        desperate = true;
                        aiState = "attack";
                        
                    }
                    else
                    {
                        aiState = "run";
                        runOrigin = position.X;
                        if (target.position.X > position.X)
                        {
                            runDistance = 300;
                            spriteEffects = SpriteEffects.FlipHorizontally;
                        }
                        else if (target.position.X < position.X)
                        {
                            runDistance = 300;
                            spriteEffects = SpriteEffects.None;
                        }
                    }
                }
            }*/
            if (position.X < 0.0f)
            {
                spriteEffects = SpriteEffects.None;
                Special();
            }
            else if (position.X > game.tilemap.width * 16)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
                Special();
            }
            if (animationState == "teleport")
            {
                walk = 0.0f;
                gravity = -0.15f;
                jump = 0.0f;
                if (frame == 2 && anim == 0)
                {
                    if (spriteEffects == SpriteEffects.None)
                    {
                        Vector2 newPos = position + new Vector2(128.0f, 0.0f);
                        while (game.tilemap.CollideRect(newPos.X + hitboxOffset.X, newPos.Y + hitboxOffset.Y, hitboxSize.X, hitboxSize.Y))
                        {
                            newPos.Y -= 32.0f;
                        }/*
                        for (int i = 0; i < 20; i++)
                        {
                            Vector2 shit = newPos - position;
                            shit.Normalize();
                            game.AddParticle(new Particle(game, position + new Vector2(game.game.random.Next(-16, 16), game.game.random.Next(-16, 16)), shit, 3, 8, 1));
                        }*/
                        position = newPos;
                    }
                    else
                    {
                        Vector2 newPos = position + new Vector2(-128.0f, 0.0f);
                        while (game.tilemap.CollideRect(newPos.X + hitboxOffset.X, newPos.Y + hitboxOffset.Y, hitboxSize.X, hitboxSize.Y))
                        {
                            newPos.Y -= 32.0f;
                        }
                        position = newPos;
                    }
                }
                else if (frame == 6 && anim == 2)
                {
                    ChangeAnimationState("jump");
                }
            }
            base.Update(gt);
            
            if (eyeFlash != null)
            {
                if (eyeFlash.killMe)
                {
                    eyeFlash = null;
                }
                else
                {
                    eyeFlash.position = position + ((spriteEffects == SpriteEffects.None) ? flashOffset : flashOffset2);
                }
            }
        }
        public override void Attack()
        {
            base.Attack();
            if (cooldown == 0 && animationState != "teleport" && deathTimer == 0)
            {
                if (spriteEffects == SpriteEffects.None)
                {
                    game.AddEntity(new Crab(game, position, new Vector2(2f, aimDirection * 4f), this));
                }
                else
                {
                    game.AddEntity(new Crab(game, position, new Vector2(-2f, aimDirection * 4f), this));
                }
                eyeFlash = new Particle(game, position + flashOffset, Vector2.Zero, 5, 8, 0);
                game.AddParticle(eyeFlash);
                attacking = true;
                cooldown = attackSpeed;
                if (animation.frames == walkAnimation.frames)
                {
                    animation = walkAttackAnimation;
                    frame = 0;
                }
                else if (animation.frames == idleAnimation.frames)
                {
                    animation = attackAnimation;
                    frame = 0;
                }
            }
        }
        public override void Special()
        {
            if (magic >= 75 && animationState != "teleport" && deathTimer == 0)
            {
                magic -= 75;
                ChangeAnimationState("teleport");
            }
        }
        protected override void ChangeAnimationState(string st)
        {
            string lastState = animationState;
            base.ChangeAnimationState(st);
            if ((lastState == "idle" || lastState == "walk") && (st == "idle_attack" || st == "walk_attack"))
            {
                animation = aboutToAttackAnimation;
            }
            else if ((lastState == "idle_attack" || lastState == "walk_attack") && (st == "idle" || st == "walk"))
            {
                animation = aboutToAttackAnimation;
            }
            else
            {
                SyncAnimationWithState();
            }
        }
        protected override void UpdateAnimationState()
        {
            base.UpdateAnimationState();
            if (animationState != "teleport" && animationState != "dead")
            {
                if (jump != 0.0f)
                {
                    ChangeAnimationState("jump");
                }
                else
                {
                    if (Math.Abs(walk) > 0.5f)
                    {
                        if (!attacking)
                        {
                            ChangeAnimationState("walk");
                        }
                        else
                        {
                            ChangeAnimationState("walk_attack");
                        }
                    }
                    else
                    {
                        if (!attacking)
                        {
                            ChangeAnimationState("idle");
                        }
                        else
                        {
                            ChangeAnimationState("idle_attack");
                        }
                    }
                }
            }
        }
        protected override void SyncAnimationWithState()
        {
            base.SyncAnimationWithState();
            switch (animationState)
            {
                case "idle":
                    animation = idleAnimation;
                    break;
                case "walk":
                    animation = walkAnimation;
                    break;
                case "jump":
                    animation = jumpAnimation;
                    break;
                case "idle_attack":
                    animation = attackAnimation;
                    break;
                case "walk_attack":
                    animation = walkAttackAnimation;
                    break;
                case "teleport":
                    animation = teleportAnimation;
                    break;
                case "dead":
                    animation = deathAnimation;
                    break;
            }
        }
        public override void CPUDodge()
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
                            else if (p is Crab)
                            {
                                aiState = "run";
                                runOrigin = position.X;
                                runDistance = 64.0f;
                                if (p.position.X > position.X)
                                {
                                    spriteEffects = SpriteEffects.None;
                                }
                                else
                                {
                                    spriteEffects = SpriteEffects.FlipHorizontally;
                                }
                                Special();
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
                        if (Vector2.Distance(game.entities[Gameplay.TYPE_PLAYER, i].position, position) < 64.0f - game.game.random.Next(0, 14))
                        {
                            aiState = "run";
                            runOrigin = position.X;
                            runDistance = 128.0f;
                            if (game.entities[Gameplay.TYPE_PLAYER, i].position.X > position.X)
                            {
                                spriteEffects = SpriteEffects.FlipHorizontally;
                            }
                            else
                            {
                                spriteEffects = SpriteEffects.None;
                            }
                            Special();
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
    }
}
