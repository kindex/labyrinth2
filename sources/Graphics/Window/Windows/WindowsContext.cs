using System;
using System.Runtime.InteropServices;

namespace Game.Graphics.Window.Windows
{
    internal sealed class WindowsContext : IPlatformContext
    {
        public WindowsContext(IntPtr dc, int samples)
        {
            this.dc = dc;
            this.VSync = false;
            this.Samples = samples;

            NativeAPI.PixelFormatDescriptor pfd = new NativeAPI.PixelFormatDescriptor();
            pfd.Size = (ushort)Marshal.SizeOf(pfd);
            pfd.Version = 1;
            pfd.Flags = NativeAPI.PixelFormatDescriptorFlags.DrawToWindow
                      | NativeAPI.PixelFormatDescriptorFlags.SupportOpenGL
                      | NativeAPI.PixelFormatDescriptorFlags.DoubleBuffer;
            pfd.PixelType = NativeAPI.PixelType.RGBA;
            pfd.ColorBits = 24;
            pfd.DepthBits = 24;
            pfd.StencilBits = 8;

            // TODO: why does this function must be called even if OpenGL context is not created yet?
            // without it context creation fails! But why???
            NativeAPI.wglSwapBuffers(dc);

            int pixelFormat = ChoosePixelFormat(ref pfd);
            NativeAPI.SetPixelFormat(dc, pixelFormat, ref pfd);

            rc = NativeAPI.wglCreateContext(dc);
            NativeAPI.wglMakeCurrent(dc, rc);
        }

        int ChoosePixelFormat(ref NativeAPI.PixelFormatDescriptor pfd)
        {
            try
            {
                using (DummyWindow dummyWindow = new DummyWindow(ref pfd))
                {
                    EnumerateSupportedSamples(dummyWindow);

                    if (dummyWindow.Extensions.Contains("WGL_EXT_swap_control"))
                    {
                        dummyWindow.GetNativeProc("wglSwapIntervalEXT", out SwapInterval);
                    }

                    if (dummyWindow.Extensions.Contains("WGL_ARB_pixel_format"))
                    {
                        NativeAPI.wglChoosePixelFormatARB ChoosePixelFormatARB;

                        dummyWindow.GetNativeProc("wglChoosePixelFormatARB", out ChoosePixelFormatARB);

                        if (dummyWindow.Extensions.Contains("WGL_ARB_multisample") == false)
                        {
                            Samples = 0;
                        }

                        int[] attrib = 
                        {
                            NativeAPI.WGL_DRAW_TO_WINDOW_ARB, 1,
                            NativeAPI.WGL_ACCELERATION_ARB, NativeAPI.WGL_FULL_ACCELERATION_ARB,
                            NativeAPI.WGL_SUPPORT_OPENGL_ARB, 1,
                            NativeAPI.WGL_DOUBLE_BUFFER_ARB, 1,
                            NativeAPI.WGL_COLOR_BITS_ARB, 24,
                            NativeAPI.WGL_RED_BITS_ARB, 8,
                            NativeAPI.WGL_GREEN_BITS_ARB, 8,
                            NativeAPI.WGL_BLUE_BITS_ARB, 8,
                            NativeAPI.WGL_ALPHA_BITS_ARB, 0,
                            NativeAPI.WGL_DEPTH_BITS_ARB, 24,
                            NativeAPI.WGL_STENCIL_BITS_ARB, 8,
                            0, 0,
                            0, 0,
                            0
                        };

                        if (Samples > 0)
                        {
                            attrib[attrib.Length - 5 + 0] = NativeAPI.WGL_SAMPLE_BUFFERS_ARB;
                            attrib[attrib.Length - 5 + 1] = 1;
                            attrib[attrib.Length - 5 + 2] = NativeAPI.WGL_SAMPLES_ARB;
                            attrib[attrib.Length - 5 + 3] = Samples;
                        }

                        int count;
                        bool validpf = false;
                        int[] piFormats = new int[1];
                        do
                        {
                            if (!ChoosePixelFormatARB(dummyWindow.dc, attrib, null, 1, piFormats, out count))
                            {
                                return 0;
                            }
                            if (count == 0)
                            {
                                if (Samples != 0)
                                {
                                    Samples--;
                                    attrib[attrib.Length - 5 + 3] = Samples;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                validpf = true;
                            }

                        }
                        while (!validpf && count == 0);

                        if (validpf)
                        {
                            return piFormats[0];
                        }
                    }
                }
            }
            catch (ApplicationException)
            {
            }

            return NativeAPI.ChoosePixelFormat(this.dc, ref pfd);
        }

        void EnumerateSupportedSamples(DummyWindow dummyWindow)
        {
            NativeAPI.wglGetPixelFormatAttribiARB GetPixelFormatAttribi;
            NativeAPI.wglGetPixelFormatAttribivARB GetPixelFormatAttribiv;
            dummyWindow.GetNativeProc("wglGetPixelFormatAttribivARB", out GetPixelFormatAttribi);
            dummyWindow.GetNativeProc("wglGetPixelFormatAttribivARB", out GetPixelFormatAttribiv);

            int num_format_attr = NativeAPI.WGL_NUMBER_PIXEL_FORMATS_ARB;
            int num_formats;
            if (GetPixelFormatAttribi(dummyWindow.dc, 0, 0, 1, ref num_format_attr, out num_formats))
            {
                int[] attr =
                {
                    NativeAPI.WGL_DRAW_TO_WINDOW_ARB,     // 0
                    NativeAPI.WGL_ACCELERATION_ARB,       // 1
                    NativeAPI.WGL_SUPPORT_OPENGL_ARB,     // 2
                    NativeAPI.WGL_DOUBLE_BUFFER_ARB,      // 3
                    NativeAPI.WGL_PIXEL_TYPE_ARB,         // 4
                    NativeAPI.WGL_COLOR_BITS_ARB,         // 5
                    NativeAPI.WGL_ALPHA_BITS_ARB,         // 6
                    NativeAPI.WGL_DEPTH_BITS_ARB,         // 7
                    NativeAPI.WGL_STENCIL_BITS_ARB,       // 8
                    NativeAPI.WGL_SAMPLE_BUFFERS_ARB,     // 9
                    NativeAPI.WGL_SAMPLES_ARB,            // 10
                };

                int[] values = new int[attr.Length];

                for (int i = 1; i <= num_formats; i++)
                {
                    if (GetPixelFormatAttribiv(dummyWindow.dc, i, 0, attr.Length, attr, values))
                    {
                        if (values[0] == 1 &&                                     // WGL_DRAW_TO_WINDOW_ARB
                            values[1] == NativeAPI.WGL_FULL_ACCELERATION_ARB &&   // WGL_ACCELERATION_ARB
                            values[2] == 1 &&                                     // WGL_SUPPORT_OPENGL_ARB
                            values[3] == 1 &&                                     // WGL_DOUBLE_BUFFER_ARB
                            values[4] == NativeAPI.WGL_TYPE_RGBA_ARB &&           // WGL_PIXEL_TYPE_ARB
                            values[5] >= 24 &&                                    // WGL_COLOR_BITS_ARB
                            values[6] >= 8 &&                                     // WGL_ALPHA_BITS_ARB
                            values[7] == 24 &&                                    // WGL_DEPTH_BITS_ARB
                            values[8] == 8 &&                                     // WGL_STENCIL_BITS_ARB
                            values[9] == 1)                                       // WGL_SAMPLE_BUFFERS_ARB
                        {
                            SupportedSamples |= (long)1 << values[10];            // WGL_SAMPLES_ARB
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            NativeAPI.wglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
            NativeAPI.wglDeleteContext(rc);
        }

        public void ShareWith(IPlatformContext newContext)
        {
            WindowsContext context = (WindowsContext)newContext;
            NativeAPI.wglShareLists(rc, context.rc);
        }

        public void MakeCurrent()
        {
            NativeAPI.wglMakeCurrent(dc, rc);
        }

        public void SwapBuffers()
        {
            NativeAPI.wglSwapBuffers(dc);
        }

        public bool VSync
        {
            get
            {
                return vsync;
            }

            set
            {
                vsync = value;
                if (SwapInterval != null)
                {
                    SwapInterval(value ? 1 : 0);
                }
            }
        }

        public int Samples { get; private set; }
        public long SupportedSamples { get; private set; }

        bool vsync;
        IntPtr dc;
        IntPtr rc;
        NativeAPI.wglSwapIntervalEXT SwapInterval;
    }
}
