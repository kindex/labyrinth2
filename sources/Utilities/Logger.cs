using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Game
{
    static class Logger
    {
        static FileStream log_file;
        static Logger()
        {
            System.Diagnostics.Trace.AutoFlush = true;
            System.Diagnostics.Trace.Listeners.Add(new TextWriterTraceListener(System.Console.Out));
            log_file = File.OpenWrite("labyrinth2.log");
            System.Diagnostics.Trace.Listeners.Add(new TextWriterTraceListener(log_file));
            write("Logger started");

            System.AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            write("Logger flushing");
            System.Diagnostics.Trace.Listeners.Clear();
        }

        public static void write(string message)
        {
            System.Diagnostics.Trace.WriteLine(DateTime.Now.ToString() + "\t" + message);
        }
    }
}
