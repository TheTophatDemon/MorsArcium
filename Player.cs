using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mors_Arcium
{
    public class Player : Entity
    {
        public const int TYPE_MR_B = 0;
        public const int TYPE_WIZARD = 1;
        public const int TYPE_ELI = 2;
        public const int TYPE_BUG = 3;

        public SpriteEffects spriteEffects;
        public int playerType = 0;

        Animation idleAnimation;
        Animation walkAnimation;
        Animation jumpAnimation;
        Animation attackAnimation;
        Animation walkAttackAnimation;
        Animation aboutToAttackAnimation;

        private float gravity = 0.0f;
        private float jump = 0.0f;
        private float walk = 0.0f;

        private float jumpHeight = 5.0f;
        private float acceleration = 0.25f;
        private float walkSpeed = 2.77f;
        private int attackSpeed = 70;

        private bool tryingToJump = false;
        private bool attacking = false;
        private int cooldown = 0;

        private string animationState = "idle";
        

        public Player(Gameplay g, int pt) : base(g)
        {
            texture = g.game.textures[0];
            playerType = pt;
            type = Gameplay.TYPE_PLAYER;
            sourceRect = new Rectangle(0, pt * 32, 32, 32);
            hitboxSize = new Vector2(8, 16); //Half of the actual hitbox's size
            hitboxOffset = new Vector2(0, 0);
            origin = new Vector2(16, 16);
            switch (pt)
            {
                case TYPE_MR_B:
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
                    break;
                case TYPE_WIZARD:

                    break;
            }
            animation = idleAnimation;
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
            if (cooldown == 0)
            {
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
                else
                {
                    
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
        protected virtual void UpdateAnimationState()
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
        public override void Update(GameTime gt)
        {
            UpdateAnimationState();
            if (cooldown > 0) cooldown -= 1;
            if (cooldown < attackSpeed - 20) attacking = false;
            walk *= 0.9f;
            if (Math.Abs(walk) < 0.1f) walk = 0.0f;
            gravity += 0.15f;
            if (gravity > 8.0f) gravity = 8.0f;
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
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, position, sourceRect, Color.White, rotation, origin, scale, spriteEffects, 0);
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
