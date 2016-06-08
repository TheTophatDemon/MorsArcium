using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Mors_Arcium
{
    public class Prop : Entity
    {
        public static Rectangle cyberCactus = new Rectangle(0, 48, 16, 32);
        int ty, tx, th, tw;
        public Prop(Gameplay g, Rectangle src, Vector2 pos) : base(g)
        {
            type = Gameplay.TYPE_PROP;
            collisions = false;
            origin = Vector2.Zero;
            texture = g.game.textures[5];
            sourceRect = src;
            position = pos;
            tx = (int)Math.Floor(pos.X / 16.0f);
            ty = (int)Math.Floor(pos.Y / 16.0f);
            th = (int)Math.Floor(src.Height / 16.0f);
            tw = (int)Math.Floor(src.Width / 16.0f);
        }
        public override void Update(GameTime gt)
        {
            //Make_Money();
            for (int x = tx; x < tx + tw; x++)
            {
                if (game.tilemap.data[x, ty + th] == 0 || game.tilemap.data[x, ty + th] == 2)
                {
                    killMe = true;
                    //Console.WriteLine(game.tilemap.data[x, ty + th]);
                    break;
                }
            }
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, position, sourceRect, Color.White);
        }
        public override void Collide(Entity perpetrator)
        {
            
        }
    }
}
