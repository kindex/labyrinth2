using System.Diagnostics;
using System.IO;

namespace Game
{
    public static class EntryPoint
    {
        static void Main()
        {
            System.Diagnostics.Trace.Listeners.Add(new TextWriterTraceListener(System.Console.Out));

            using (FileStream fs = File.OpenWrite("labyrinth2.log"))
            {
                System.Diagnostics.Trace.AutoFlush = true;
                System.Diagnostics.Trace.Listeners.Add(new TextWriterTraceListener(fs));

                Logger.write("Loading native libraries");
                NativeLoader.AutoLoad();
                Logger.write("Done loading native libraries");

                using (Game game = new Game())
                {
                    game.Run();
                }

                System.Diagnostics.Trace.Listeners.Clear();
            }
        }
    }
}
