using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mors_Arcium
{
    public class WizardPlayer : Player
    {
        Animation idleAnimation;
        Animation walkAnimation;
        Animation jumpAnimation;
        Animation attackAnimation;
        Animation attackUpAnimation;
        Animation attackDownAnimation;
        Animation walkAttackAnimation;
        Animation walkAttackUpAnimation;
        Animation walkAttackDownAnimation;
        Animation jumpAttackUpAnimation;
        Animation jumpAttackDownAnimation;
        Animation jumpAttackAnimation;
        Animation deathAnimation;
        public WizardPlayer(Gameplay g) : base(g)
        {
            attackSpeed = 100;
            maxHealth = 100;
            maxMagic = 150;
            health = 100;
            magic = 150;
            walkSpeed = 2.0f;
            jumpHeight = 4.5f;
            //29 + frame
            idleAnimation.frames = new int[] { 0, 1 };
            idleAnimation.speed = 10;
            idleAnimation.looping = true;
            walkAnimation.frames = new int[] { 0, 2, 3, 2 };
            walkAnimation.speed = 3;
            walkAnimation.looping = true;
            jumpAnimation.frames = new int[] { 4, 5, 6 };
            jumpAnimation.speed = 3;
            jumpAnimation.looping = false;
            deathAnimation.frames = new int[] { 22, 23, 24 };
            deathAnimation.looping = false;
            deathAnimation.speed = 5;
            attackAnimation.frames = new int[] { 7, 8 };
            attackUpAnimation.frames = new int[] { 9, 10 };
            attackDownAnimation.frames = new int[] { 11, 12 };
            walkAttackAnimation.frames = new int[] { 13, 14 };
            walkAttackUpAnimation.frames = new int[] { 15, 16 };
            walkAttackDownAnimation.frames = new int[] { 17, 18 };
            jumpAttackAnimation.frames = new int[] { 25, 26 };
            jumpAttackUpAnimation.frames = new int[] { 27, 28 };
            jumpAttackDownAnimation.frames = new int[] { 29, 30 };
            jumpAttackAnimation.speed = jumpAttackUpAnimation.speed = jumpAttackDownAnimation.speed = walkAttackAnimation.speed = walkAttackUpAnimation.speed = walkAttackDownAnimation.speed = attackAnimation.speed = attackUpAnimation.speed = attackDownAnimation.speed = 3;
            jumpAttackAnimation.looping = jumpAttackUpAnimation.looping = jumpAttackDownAnimation.looping = walkAttackAnimation.looping = walkAttackUpAnimation.looping = walkAttackDownAnimation.looping = attackAnimation.looping = attackUpAnimation.looping = attackDownAnimation.looping = true;
            sheetOffset = 32;
            animation = idleAnimation;
        }
        public override void Update(GameTime gt)
        {
            base.Update(gt);
        }
        public override void Attack()
        {
            base.Attack();
            if (cooldown == 0 && deathTimer == 0)
            {
                attacking = true;
                cooldown = attackSpeed;
                /*float spdx = 8.0f;
                if (spriteEffects == SpriteEffects.FlipHorizontally) spdx = -8.0f;
                Bullet b = new Bullet(game, position + new Vector2(spdx * 1.25f, (aimDirection * 8.0f) + 4), new Vector2(spdx, aimDirection * 8), this);
                game.AddEntity(b);
                b = null;
                Particle p = new Particle(game, position, new Vector2(((float)game.game.random.NextDouble() * 2.5f) * -Math.Sign(spdx), -0.5f), 100, 8, 5);
                game.AddParticle(p);
                p = null;*/
                //Check a square of tiles around the ray being cast
                //Collect the tiles that hit the ray into a list
                //Find the closest tile and get its collision x,y
                Vector2 rayEnd = new Vector2(position.X, position.Y);
                float rot = 0.0f;
                if (spriteEffects == SpriteEffects.None)
                {
                    rayEnd.X += 160.0f;
                    if (aimDirection == -1)
                    {
                        rayEnd.Y -= 160.0f;
                        rot = 315.0f;
                    }
                    else if (aimDirection == 1)
                    {
                        rayEnd.Y += 160.0f;
                        rot = 45.0f;
                    }
                }
                else
                {
                    rayEnd.X -= 160.0f;
                    if (aimDirection == -1)
                    {
                        rayEnd.Y -= 160.0f;
                        rot = 225.0f;
                    }
                    else if (aimDirection == 1)
                    {
                        rayEnd.Y += 160.0f;
                        rot = 135.0f;
                    }
                    else
                    {
                        rot = 180.0f;
                    }
                }
                
                float slope = (rayEnd.Y - position.Y) / (rayEnd.X - position.X);
                int l = (int)Math.Floor(Math.Min(rayEnd.X, position.X) / 16.0f) - 1;
                int r = (int)Math.Ceiling(Math.Max(rayEnd.X, position.X) / 16.0f) + 1;
                int t = (int)Math.Floor(Math.Min(rayEnd.Y, position.Y) / 16.0f) - 1;
                int b = (int)Math.Ceiling(Math.Max(rayEnd.Y, position.Y) / 16.0f) + 1;
                if (l < 0) l = 0; if (r < 0) r = 0;
                if (t < 0) t = 0; if (b < 0) b = 0;
                if (r >= game.tilemap.width) r = game.tilemap.width - 1;
                if (b >= game.tilemap.height) b = game.tilemap.height - 1;
                if (l >= game.tilemap.width) l = game.tilemap.width - 1;
                if (t >= game.tilemap.height) t = game.tilemap.height - 1;
                float minDistance = 215.0f;
                for (int y = t; y < b; y++)
                {
                    for (int x = l; x < r; x++)
                    {
                        if (game.tilemap.data[x, y] != -1)
                        {
                            float tx = x * 16;
                            float ty = y * 16;
                            float y0 = position.Y + ((tx - position.X) * slope);
                            float y1 = position.Y + ((tx + 16 - position.X) * slope);
                            if ((ty < y0 || ty < y1) && (ty + 16 > y0 || ty + 16 > y1))
                            {
                                
                                //We have hit!
                                float dist = Vector2.Distance(new Vector2(tx + 8, ty + 8), position);
                                if (dist < minDistance)
                                {
                                    minDistance = dist;
                                }
                            }
                        }
                    }
                }
                for (int i = 0; i < game.entities.GetLength(1); i++)
                {
                    if (game.entities[Gameplay.TYPE_PLAYER, i] != null)
                    {
                        Player p = (Player)game.entities[Gameplay.TYPE_PLAYER, i];
                        if (Vector2.Distance(p.position, position) <= minDistance && ((p.position.X < position.X && spriteEffects == SpriteEffects.FlipHorizontally) || (p.position.X > position.X && spriteEffects == SpriteEffects.None)))
                        {
                            float y0 = position.Y + ((p.position.X + p.hitboxOffset.X - p.hitboxSize.X - position.X) * slope);
                            float y1 = position.Y + ((p.position.X + p.hitboxOffset.X + p.hitboxSize.X - position.X) * slope);
                            float top = p.position.Y + p.hitboxOffset.Y - p.hitboxSize.Y;
                            float bottom = p.position.Y + p.hitboxOffset.Y + p.hitboxSize.Y;
                            if ((top < y0 + 4 || top < y1 + 4) && (bottom > y0 - 4 || bottom > y1 - 4))
                            {
                                p.Damage(13, this);
                            }
                        }
                        p = null;
                    }
                }
                Beam beam = new Beam(game, minDistance, MathHelper.ToRadians(rot));
                beam.position = position;
                game.AddEntity(beam);
                beam = null;
            }
        }
        protected override void ChangeAnimationState(string st)
        {
            string lastState = animationState;
            base.ChangeAnimationState(st);
            //Process transition animations
            /*if (animationState == "attack" && lastState == "jump")
            {
                animation = jumpShootTransition;
            }
            else
            {*/
                SyncAnimationWithState();
            //}
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
                case "attack":
                    if (aimDirection == -1)
                    {
                        animation = attackUpAnimation;
                    }
                    else if (aimDirection == 1)
                    {
                        animation = attackDownAnimation;
                    }
                    else
                    {
                        animation = attackAnimation;
                    }
                    break;
                case "walk_attack":
                    if (aimDirection == -1)
                    {
                        animation = walkAttackUpAnimation;
                    }
                    else if (aimDirection == 1)
                    {
                        animation = walkAttackDownAnimation;
                    }
                    else
                    {
                        animation = walkAttackAnimation;
                    }
                    break;
                case "jump_attack":
                    if (aimDirection == -1)
                    {
                        animation = jumpAttackUpAnimation;
                    }
                    else if (aimDirection == 1)
                    {
                        animation = jumpAttackDownAnimation;
                    }
                    else
                    {
                        animation = jumpAttackAnimation;
                    }
                    break;
                case "dead":
                    animation = deathAnimation;
                    break;
            }
            
        }
        protected override void UpdateAnimationState()
        {
            base.UpdateAnimationState();
            //Actually determine what action triggers which state
            if (animationState != "dead")
            {
                if (jump != 0.0f)
                {
                    if (attacking)
                    {
                        ChangeAnimationState("jump_attack");
                    }
                    else
                    {
                        ChangeAnimationState("jump");
                    }
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
                            ChangeAnimationState("attack");
                        }
                    }
                }
            }
        }
        public override void CPUAttack()
        {
            float ang2targ = MathHelper.ToDegrees((float)Math.Atan2(target.position.Y - position.Y, target.position.X - position.X));
            if (ang2targ >= 360) ang2targ -= 360;
            if (ang2targ < 0) ang2targ += 360;
            if (cooldown < 25)
            {
                if ((ang2targ < 315 && aimDirection == -1 && spriteEffects == SpriteEffects.None) ||
                    (ang2targ > 225 && aimDirection == -1 && spriteEffects == SpriteEffects.FlipHorizontally))
                {
                    Jump();
                }
            }
            base.CPUAttack();
        }
    }
}
