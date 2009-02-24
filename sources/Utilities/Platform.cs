using System;
using System.IO;
using System.Diagnostics;

namespace Game
{
    public static class Platform
    {
        public static bool IsWindows { get; private set; }
        public static bool IsLinux { get; private set; }
        public static bool IsMacOSX { get; private set; }

        public static Graphics.Window.IPlatformWindow CreateWindow(int width, int height, int samples, string title, bool fullscreen, bool resizable)
        {
            if (IsWindows)
            {
                return new Graphics.Window.Windows.WindowsWindow(width, height, samples, title, fullscreen, resizable);
            }
            else if (IsLinux)
            {
                //return new Linux.LinuxWindow(width, height, samples, title, fullscreen, resizable);
            }
            else if (IsMacOSX)
            {
                //return new MacOSX.MacOSXWindow(width, height, samples, title, fullscreen, resizable);
            }

            return null;
        }
        
        static Platform()
        {
            PlatformID p = Environment.OSVersion.Platform;

            if (p == PlatformID.Win32NT || p == PlatformID.Win32S || p == PlatformID.Win32Windows || p == PlatformID.WinCE)
            {
                IsWindows = true;
            }
            else if (p == PlatformID.Unix || p == (PlatformID)4)
            {
                switch (UnixKernel())
                {
                    case "Unix":
                    case "Linux":
                        IsLinux = true;
                        break;

                    case "Darwin":
                        IsMacOSX = true;
                        break;

                    default:
                        throw new ApplicationException("Unknown Unix platform");
                }
            }
            else
            {
                throw new ApplicationException("Unknown .NET platform");
            }
        }

        private static string UnixKernel()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = "-s";
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            foreach (string unameprog in new string[] { "/usr/bin/uname", "/bin/uname", "uname" })
            {
                try
                {
                    startInfo.FileName = unameprog;
                    Process process = Process.Start(startInfo);
                    StreamReader stdout = process.StandardOutput;
                    return stdout.ReadLine().Trim();
                }
                catch (System.IO.FileNotFoundException)
                {
                    continue;
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    continue;
                }
            }
            return null;
        }
    }
}
