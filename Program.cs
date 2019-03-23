using System;

namespace Mors_Arcium
{
    public static class Program
    {
        
        static void Main()
        {
            MorsArcium m = new MorsArcium(new AndroidOutlet());
            m.android.game = m;
            m.Run();
        }
    }
}
