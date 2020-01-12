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
    /// <summary>
    /// Provides access to various platform-specific functions from within the shared code.
    /// </summary>
    public interface IPlatformOutlet
    {
        Rectangle GameViewport { get; }
        Settings GameSettings { get; }

        Mors_Arcium.GUI.Button.State ProcessMenuButton(Mors_Arcium.GUI.Button button);

        void ApplyVideoSettings();
        void SaveSettings();
        void Exit();
    }
}
