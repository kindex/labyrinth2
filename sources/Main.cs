using System.Diagnostics;
using System.IO;
using System;

[assembly: CLSCompliant(false)]


namespace Game
{
    public static class EntryPoint
    {
        static void Main()
        {
            Logger.write("Loading native libraries");
            NativeLoader.AutoLoad();
            Logger.write("Done loading native libraries");

            using (Game game = new Game())
            {
                game.Run();
            }
        }
    }
}
