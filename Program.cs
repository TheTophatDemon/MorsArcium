using System;

namespace Mors_Arcium
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MorsArcium())
                game.Run();
        }
    }
}
