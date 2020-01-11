using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System;
using Microsoft.Xna.Framework.Media;

namespace Mors_Arcium
{
    public interface IPlatformOutlet
    {
        Rectangle GameViewport { get; }
        MenuButtonState ProcessMenuButton(MenuButton button);
    }
}
