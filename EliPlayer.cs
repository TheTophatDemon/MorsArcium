using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mors_Arcium
{
    public class EliPlayer : Player
    {
        Animation idleAnimation;
        Animation walkAnimation;
        Animation jumpAnimation;
        Animation idleAttackAnimation;
        Animation walkAttackAnimation;
        Animation jumpAttackAnimation;
        Animation deathAnimation;
        public EliPlayer(Gameplay g) : base(g)
        {
            attackSpeed = 5;
            maxHealth = 60;
            maxMagic = 200;
            health = 60;
            magic = 200;
            walkSpeed = 3.0f;
            jumpHeight = 5.0f;
            acceleration = 0.15f;
            sheetOffset = 64;
            sourceRect = new Rectangle(0, 0, 40, 32);
            hitboxOffset = new Vector2(0, 0);
            hitboxSize = new Vector2(6, 16);
            origin = new Vector2(16, 16);
            idleAnimation.frames = new int[] { 0, 1, 2, 1 }; idleAnimation.speed = 5; idleAnimation.looping = true;
            walkAnimation.frames = new int[] { 3, 4, 5, 6, 7 }; walkAnimation.speed = 2; walkAnimation.looping = true;
            jumpAnimation.frames = new int[] { 5, 6, 7 }; jumpAnimation.speed = 3; jumpAnimation.looping = false;
            idleAttackAnimation.frames = new int[] { 8, 9, 10, 11, 12 }; idleAttackAnimation.speed = 2; idleAttackAnimation.looping = true;
            walkAttackAnimation.frames = new int[] { 13, 14, 15, 16, 17 }; walkAttackAnimation.speed = 2; walkAttackAnimation.looping = true;
            jumpAttackAnimation.frames = new int[] { 18, 19, 20, 21, 22 }; jumpAttackAnimation.speed = 2; jumpAttackAnimation.looping = true;
            deathAnimation.frames = new int[] { 23, 24, 25 };
            deathAnimation.speed = 5;
            deathAnimation.looping = false;
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
                for (int i = 0; i < game.entities.GetLength(1); i++)
                {
                    if (game.entities[Gameplay.TYPE_PLAYER, i] != null)
                    {
                        if (Vector2.Distance(game.entities[Gameplay.TYPE_PLAYER, i].position, position) < 32)
                        {
                            Player p = (Player)game.entities[Gameplay.TYPE_PLAYER, i];
                            if (spriteEffects == SpriteEffects.None && p.position.X > position.X)
                            {
                                p.knockback = 11.0f;
                                p.Damage(12, this);
                            }
                            else if (spriteEffects == SpriteEffects.FlipHorizontally && p.position.X < position.X)
                            {
                                p.knockback = -11.0f;
                                p.Damage(12, this);
                            }
                        }
                    }
                }
            }
        }
        public override void Special()
        {
            base.Special();
            
        }
        protected override void ChangeAnimationState(string st)
        {
            string lastState = animationState;
            base.ChangeAnimationState(st);
            SyncAnimationWithState();
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
                    animation = idleAttackAnimation;
                    break;
                case "walk_attack":
                    animation = walkAttackAnimation;
                    break;
                case "jump_attack":
                    animation = jumpAttackAnimation;
                    break;
                case "dead":
                    animation = deathAnimation;
                    break;
            }

        }
        public override void Draw(SpriteBatch sp)
        {
            if (hurtTimer == 0 || hurtTimer % 4 == 0)
            {
                if (spriteEffects == SpriteEffects.None)
                {
                    sp.Draw(texture, position - origin, sourceRect, Color.White, rotation, Vector2.Zero, scale, spriteEffects, 0);
                }
                else
                {
                    sp.Draw(texture, position - origin - new Vector2(8, 0), sourceRect, Color.White, rotation, Vector2.Zero, scale, spriteEffects, 0);
                }
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
            float dist = Math.Abs(target.position.X - position.X);
            if (target.position.X < position.X)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            else
            {
                spriteEffects = SpriteEffects.None;
            }
            if (dist > 16) Walk();
            Attack();
            
            if (dist > 160.0f)
            {
                aiState = "chase";
            }
        }
    }
}
