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
        Animation specialAnimation;
        Animation deathAnimation;
        int inaccuracy = 0;
        bool missed = false;
        int framesSinceLastAttack = 0;
        public WizardPlayer(Gameplay g, int hhh = 0) : base(g, hhh)
        {
            attackSpeed = 75;
            maxHealth = 100 + healthHandicap;
            maxMagic = 150;
            health = 100 + healthHandicap;
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
            specialAnimation.frames = new int[] { 19, 20, 21, 20, 19 };
            specialAnimation.speed = 5;
            specialAnimation.looping = false;
            sheetOffset = 32;
            sourceRect = new Rectangle(0, 32, 32, 32);
            animation = idleAnimation;
            inaccuracy = game.game.random.Next(-15, 15);
        }
        public override void Update(GameTime gt)
        {
            /*if (chargeUp > 0)
            {
                chargeUp += 1;
                if (chargeUp == 5)
                {
                    chargeUp = 0;
                    FireBeam();
                    color = Color.White;
                }
            }*/
            if (cooldown > 20 && deathTimer == 0)
            {
                color = Color.CornflowerBlue;
            }
            else
            {
                color = Color.White;
            }
            framesSinceLastAttack += 1;
            if (magic < maxMagic) magic += 1;
            if (animationState == "special")
            {
                walk = 0.0f;
                if (frame == specialAnimation.frames.Length - 1)
                {
                    ChangeAnimationState("idle");
                }
            }
            base.Update(gt);
        }
        public override void Attack()
        {
            base.Attack();
            if (cooldown == 0 && deathTimer == 0)
            {
                attacking = true;
                cooldown = attackSpeed + game.reloadOffset;
                game.PlaySound(7, position);
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
                //chargeUp = 1;
                //color = Color.Red;
                FireBeam();
            }
        }
        private void FireBeam()
        {
            framesSinceLastAttack = 0;
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
            int numHits = 0;
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
                        float ty2 = ty + 16;
                        float slopeOffs = 0.0f;
                        if (game.tilemap.data[x, y] == 0)
                        {
                            ty += 16;
                            slopeOffs = -16.0f;
                        }
                        else if (game.tilemap.data[x, y] == 2)
                        {
                            slopeOffs = 16.0f;
                        }
                        float y0 = position.Y + ((tx - position.X) * slope);
                        float y1 = position.Y + ((tx + 16 - position.X) * slope);
                        if ((tx < position.X || tx + 16 < position.X) && spriteEffects == SpriteEffects.None) { y0 = 1000.0f; y1 = 1000.0f; }
                        if ((tx + 16 > position.X || tx > position.X) && spriteEffects == SpriteEffects.FlipHorizontally) { y1 = 1000.0f; y0 = 1000.0f; }
                        if ((ty <= y0 || ty + slopeOffs <= y1) && (ty2 >= y0 || ty2 >= y1))
                        {
                            //We have hit!
                            float dist = Vector2.Distance(new Vector2((x * 16) + 8, (y * 16) + 8), position);
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
                    float dist = Vector2.Distance(p.position, position);
                    if (dist <= minDistance && ((p.position.X < position.X && spriteEffects == SpriteEffects.FlipHorizontally) || (p.position.X > position.X && spriteEffects == SpriteEffects.None)))
                    {
                        float left = p.position.X + p.hitboxOffset.X - p.hitboxSize.X;
                        float right = p.position.X + p.hitboxOffset.X + p.hitboxSize.X;
                        float y0 = position.Y + ((left - position.X) * slope);
                        float y1 = position.Y + ((right - position.X) * slope);
                        float top = p.position.Y + p.hitboxOffset.Y - p.hitboxSize.Y;
                        float bottom = p.position.Y + p.hitboxOffset.Y + p.hitboxSize.Y;

                        if ((left < position.X || right < position.X) && spriteEffects == SpriteEffects.None) { y0 = 1000.0f; y1 = 1000.0f; }
                        if ((right > position.X || left > position.X) && spriteEffects == SpriteEffects.FlipHorizontally) { y1 = 1000.0f; y0 = 1000.0f; }
                        if ((top < y0 + 4 || top < y1 + 4) && (bottom > y0 - 4 || bottom > y1 - 4))
                        {
                            if (dist > 176)
                            {
                                p.Damage(8, this);
                            }
                            else if (dist > 130)
                            {
                                p.Damage(10, this);
                            }
                            else
                            {
                                p.Damage(13, this);
                            }
                            p.target = this;
                            numHits += 1;
                        }
                    }
                    p = null;
                }
            }
            Beam beam = new Beam(game, minDistance, MathHelper.ToRadians(rot));
            beam.position = position;
            if (aimDirection == 0)
            {
                beam.position.Y += 4;
            }
            else if (aimDirection == -1)
            {
                beam.position.Y += 12;
            }
            missed = false;
            if (numHits == 0) missed = true;
            game.AddEntity(beam);
            beam = null;
        }
        public override void Special()
        {
            if (magic == maxMagic && collision_bottom && deathTimer == 0)
            {
                game.PlaySound(8, position);
                magic = 0;
                ChangeAnimationState("special");
                walk = 0.0f;
                Particle p = new Particle(game, position, Vector2.Zero, 25, 64, 5);
                game.AddParticle(p);
                p = null;
                for (int i = 0; i < game.entities.GetLength(1); i++)
                {
                    if (game.entities[Gameplay.TYPE_PLAYER, i] != null && game.entities[Gameplay.TYPE_PLAYER, i] != this)
                    {
                        if (Vector2.Distance(game.entities[Gameplay.TYPE_PLAYER, i].position, position) < 72.0f)
                        {
                            Player pl = (Player)game.entities[Gameplay.TYPE_PLAYER, i];
                            if (pl.position.X > position.X)
                            {
                                pl.knockback = new Vector2(11.0f, -5.0f);
                            }
                            else
                            {
                                pl.knockback = new Vector2(-11.0f, -5.0f);
                            }
                            pl = null;
                        }
                    }
                    if (game.entities[Gameplay.TYPE_PROJECTILE, i] != null)
                    {
                        if (Vector2.Distance(game.entities[Gameplay.TYPE_PROJECTILE, i].position, position) < 72.0f)
                        {
                            Projectile pr = (Projectile)game.entities[Gameplay.TYPE_PROJECTILE, i];
                            if (pr.position.X > position.X)
                            {
                                pr.knockback = new Vector2(11.0f, -5.0f);
                            }
                            else
                            {
                                pr.knockback = new Vector2(-11.0f, -5.0f);
                            }
                            pr = null;
                        }
                    }
                }
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
                case "special":
                    animation = specialAnimation;
                    break;
            }
            
        }
        protected override void UpdateAnimationState()
        {
            base.UpdateAnimationState();
            //Actually determine what action triggers which state
            if (animationState != "dead" && animationState != "special")
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
            float ang2targ = MathHelper.ToDegrees((float)Math.Atan2(target.position.Y - position.Y, target.position.X - position.X)) + inaccuracy;
            if (ang2targ >= 360) ang2targ -= 360;
            if (ang2targ < 0) ang2targ += 360;
            if (cooldown < 25)
            {
                if ((ang2targ < 315 && aimDirection == -1 && spriteEffects == SpriteEffects.None) ||
                    (ang2targ > 225 && aimDirection == -1 && spriteEffects == SpriteEffects.FlipHorizontally) || (missed == true && framesSinceLastAttack < 100))
                {
                    Jump();
                }
            }
            if (spriteEffects == SpriteEffects.None)
            {
                if (ang2targ > 40 && ang2targ < 90)
                {
                    aimDirection = 1;
                }
                else if (ang2targ < 330 && ang2targ > 270)
                {
                    aimDirection = -1;
                }
                else
                {
                    aimDirection = 0;
                }
            }
            else
            {
                if (ang2targ < 135 && ang2targ > 90)
                {
                    aimDirection = 1;
                }
                else if (ang2targ > 225 && ang2targ < 270)
                {
                    aimDirection = -1;
                }
                else
                {
                    aimDirection = 0;
                }
            }
            base.CPUAttack();
            
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
                        if (Vector2.Distance(p.position, position) < p.dodgeDistance - game.game.random.Next(0, 14))
                        {
                            if (p.position.Y > position.Y + hitboxOffset.Y - hitboxSize.Y)
                            {
                                Jump();
                            }
                            p = null;
                        }
                    }
                    p = null;
                }
                else if (game.entities[Gameplay.TYPE_PLAYER, i] != null)
                {
                    //if (game.entities[Gameplay.TYPE_PLAYER, i] is EliPlayer)
                    //{
                    float dist = Vector2.Distance(game.entities[Gameplay.TYPE_PLAYER, i].position, position);
                    if (dist < 64.0f && game.game.random.Next(0, 14) == 1)
                    {
                        if (game.entities[Gameplay.TYPE_PLAYER, i].position.Y > position.Y + hitboxOffset.Y - hitboxSize.Y)
                        {
                            if (game.entities[Gameplay.TYPE_PLAYER, i] is EliPlayer)
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
                            }
                                
                                
                        }
                        if (dist < 32.0f) Special();
                        break;
                    }
                    //}
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
