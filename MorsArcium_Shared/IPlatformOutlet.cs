/*
Copyright (C) 2016-present Alexander Lunsford

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

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
