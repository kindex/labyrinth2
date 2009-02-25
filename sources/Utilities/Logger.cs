using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    static class Logger
    {
        public static void write(string message)
        {
            System.Diagnostics.Trace.WriteLine(DateTime.Now.ToString() + "\t" + message);
        }
    }
}
