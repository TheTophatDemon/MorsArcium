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
        Particle eyeFlash;
        static Vector2 flashOffset = new Vector2(-7, -7);
        static Vector2 flashOffset2 = new Vector2(-2, -7);
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
            animation = idleAnimation;
        }
        public override void Update(GameTime gt)
        {
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
            if (cooldown == 0)
            {
                if (spriteEffects == SpriteEffects.None)
                {
                    game.AddEntity(new Crab(game, position, new Vector2(2f, -aimDirection * 4f), this));
                }
                else
                {
                    game.AddEntity(new Crab(game, position, new Vector2(-2f, -aimDirection * 4f), this));
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
            }
        }
    }
}
