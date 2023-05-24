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

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Mors_Arcium
{
    public class Settings
    {
        public Binding aimUp;
        public Binding aimDown;
        public Binding moveRight;
        public Binding moveLeft;
        public Binding jump;
        public Binding attack;
        public Binding special;
        public Binding pause;

        public bool musicEnabled;
        public bool soundEnabled;
        public bool fullScreen;
        public bool vSync;
        public bool jumpFly;
        public bool playedBefore;
    }
}
