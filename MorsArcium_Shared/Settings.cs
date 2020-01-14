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
