using System;
using System.Runtime.InteropServices;

namespace Game.Graphics.Window.Windows
{
    public sealed class DummyWindow : IDisposable
    {
        public DummyWindow(ref NativeAPI.PixelFormatDescriptor pfd)
        {
            hwnd = NativeAPI.CreateWindowEx(
                0, "STATIC", "Dymmy OpenGL Window", 0, 
                NativeAPI.CW_USEDEFAULT, NativeAPI.CW_USEDEFAULT,
                NativeAPI.CW_USEDEFAULT, NativeAPI.CW_USEDEFAULT,
                IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

            if (hwnd == IntPtr.Zero)
            {
                throw new ApplicationException();
            }

            try
            {
                dc = NativeAPI.GetDC(hwnd);
                if (dc == IntPtr.Zero)
                {
                    throw new ApplicationException();
                }

                try
                {
                    int format = NativeAPI.ChoosePixelFormat(dc, ref pfd);
                    if (format == 0)
                    {
                        throw new ApplicationException();
                    }

                    if (!NativeAPI.SetPixelFormat(dc, format, ref pfd))
                    {
                        throw new ApplicationException();
                    }

                    rc = NativeAPI.wglCreateContext(dc);
                    if (rc == IntPtr.Zero)
                    {
                        throw new ApplicationException();
                    }

                    try
                    {
                        NativeAPI.wglMakeCurrent(dc, rc);

                        NativeAPI.wglGetExtensionsStringARB wglGetExtensionsString;
                        GetNativeProc("wglGetExtensionsStringARB", out wglGetExtensionsString);
                        Extensions = new HashSet<string>(Marshal.PtrToStringAnsi(wglGetExtensionsString(dc)).Split(' '));
                    }
                    catch
                    {
                        NativeAPI.wglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
                        NativeAPI.wglDeleteContext(rc);
                    }
                }
                catch
                {
                    NativeAPI.ReleaseDC(hwnd, dc);
                }
            }
            catch
            {
                NativeAPI.DestroyWindow(hwnd);
            }
        }

        public void Dispose()
        {
            NativeAPI.wglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
            NativeAPI.wglDeleteContext(rc);
            NativeAPI.ReleaseDC(hwnd, dc);
            NativeAPI.DestroyWindow(hwnd);
        }

        public void GetNativeProc<T>(string name, out T proc)
        {
            proc = (T)Convert.ChangeType(Marshal.GetDelegateForFunctionPointer(NativeAPI.wglGetProcAddress(name), typeof(T)), typeof(T));
        }

        public HashSet<string> Extensions { get; private set; }

        public IntPtr dc { get; private set; }

        IntPtr hwnd;
        IntPtr rc;

    }
}
