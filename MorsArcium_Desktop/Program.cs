using System;
using System.Threading;
using Mors_Arcium;

namespace MorsArcium_Desktop
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
