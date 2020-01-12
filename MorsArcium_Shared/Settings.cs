using System;
using System.Collections.Generic;
using System.Text;

namespace Mors_Arcium
{
    public class Settings
    {
        public IBinding aimUp;
        public IBinding aimDown;
        public IBinding moveRight;
        public IBinding moveLeft;
        public IBinding jump;
        public IBinding attack;
        public IBinding special;
        public IBinding pause;
        public bool musicEnabled;
        public bool soundEnabled;
        public bool fullScreen;
        public bool vSync;
        public bool jumpFly;
        public bool playedBefore;
    }
}
