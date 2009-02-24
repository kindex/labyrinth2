using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Game.Graphics.Window.Windows
{
    internal sealed class WindowsWindow : IPlatformWindow
    {
        public WindowsWindow(int width, int height, int samples, string title, bool fullscreen, bool resizable)
        {
            this.title = title;
            this.resizable = resizable;
            this.Fullscreen = fullscreen;
            this.Height = height;
            this.Width = width;

            this.hInstance = NativeAPI.GetModuleHandle(null);
            this.className = "GameWindowClass_" + this.GetHashCode().ToString("x");

            if (this.Fullscreen)
            {
                this.Fullscreen = SetFullscreen(this.Fullscreen);
            }

            this.windowProcDelegate = new NativeAPI.WindowProc(WindowProc);

            NativeAPI.WindowClass wc = new NativeAPI.WindowClass();
            wc.cbSize = (uint)Marshal.SizeOf(wc);
            wc.style = NativeAPI.ClassStyle.HRedraw | NativeAPI.ClassStyle.VRedraw | NativeAPI.ClassStyle.OwnDC;
            wc.lpfnWndProc = Marshal.GetFunctionPointerForDelegate(windowProcDelegate);
            wc.cbClsExtra = 0;
            wc.cbWndExtra = 0;
            wc.hInstance = hInstance;
            wc.hIcon = IntPtr.Zero;
            wc.hCursor = NativeAPI.LoadCursor(IntPtr.Zero, NativeAPI.IDC_ARROW);
            wc.hbrBackground = IntPtr.Zero;
            wc.lpszMenuName = null;
            wc.lpszClassName = className;
            wc.hIconSm = IntPtr.Zero;

            NativeAPI.WindowStyle style;
            NativeAPI.WindowStyleEx styleEx;
            int x, y;
            int w = width;
            int h = height;
            GetWindowStyle(this.Fullscreen, resizable, ref w, ref h, out x, out y, out style, out styleEx);

            wndclass = NativeAPI.RegisterClassEx(ref wc);

            handle = NativeAPI.CreateWindowEx(
                styleEx,
                wc.lpszClassName,
                title,
                style,
                x, y,
                w, h,
                IntPtr.Zero,
                IntPtr.Zero,
                hInstance,
                IntPtr.Zero);

            dc = NativeAPI.GetDC(handle);

            this.Context = new WindowsContext(dc, samples);
            Renderer.OpenGL.Loader.Load();
        }

        uint WindowProc(IntPtr hwnd, NativeAPI.WindowMessage uMsg, uint wParam, uint lParam)
        {
            switch (uMsg)
            {
                case NativeAPI.WindowMessage.DESTROY:
                    NativeAPI.PostQuitMessage(0);
                    break;

                case NativeAPI.WindowMessage.CLOSE:
                    OnClose();
                    return 0;

                case NativeAPI.WindowMessage.SIZE:
                    NativeAPI.Rectangle rect = new NativeAPI.Rectangle();
                    NativeAPI.GetClientRect(handle, out rect);
                    if (!Fullscreen && (Width != rect.Right || Height != rect.Bottom))
                    {
                        Width = rect.Right;
                        Height = rect.Bottom;
                        OnResize(Width, Height);
                    }
                    break;

                case NativeAPI.WindowMessage.ACTIVATE:
                    if (wParam == 0)
                    {
                        Focused = false;
                        OnFocusLost();

                        if (mouseCaptured)
                        {
                            NativeAPI.ClipCursor(IntPtr.Zero);
                            NativeAPI.ShowCursor(true);
                        }
                    }
                    else
                    {
                        Focused = true;
                        OnFocusGained();

                        if (mouseCaptured)
                        {
                            NativeAPI.ShowCursor(false);

                            lastX = Width / 2;
                            lastY = Height / 2;

                            NativeAPI.Rectangle crect = new NativeAPI.Rectangle(0, 0, Width, Height);
                            NativeAPI.ClientToScreen(handle, ref crect.LeftTop);
                            NativeAPI.ClientToScreen(handle, ref crect.RightBottom);

                            MoveMouseCursorToMiddle(crect);

                            NativeAPI.ClipCursor(ref crect);
                        }
                    }
                    break;
                    
                case NativeAPI.WindowMessage.CHAR:
                    OnEvent(new InputEvents.TextEvent((char)wParam));
                    break;

                case NativeAPI.WindowMessage.KEYDOWN:
                case NativeAPI.WindowMessage.SYSKEYDOWN:
                    if ((lParam & (1 << 30)) == 0)
                    {
                        InputEvents.Key keyDown = (wParam == NativeAPI.VK_SHIFT ? GetShiftState(true) : FromVirtualKeyCode(wParam, lParam));
                        bool altDown = (NativeAPI.GetAsyncKeyState(NativeAPI.VK_MENU) >> 16) != 0;
                        bool ctrlDown = (NativeAPI.GetAsyncKeyState(NativeAPI.VK_CONTROL) >> 16) != 0;
                        bool shiftDown = (NativeAPI.GetAsyncKeyState(NativeAPI.VK_SHIFT) >> 16) != 0;
                        OnEvent(new InputEvents.KeyPressedEvent(keyDown, altDown, ctrlDown, shiftDown));
                    }
                    break;

                case NativeAPI.WindowMessage.KEYUP:
                case NativeAPI.WindowMessage.SYSKEYUP:
                    InputEvents.Key keyUp = (wParam == NativeAPI.VK_SHIFT ? GetShiftState(false) : FromVirtualKeyCode(wParam, lParam));
                    bool altUp = (NativeAPI.GetAsyncKeyState(NativeAPI.VK_MENU) >> 16) != 0;
                    bool ctrlUp = (NativeAPI.GetAsyncKeyState(NativeAPI.VK_CONTROL) >> 16) != 0;
                    bool shiftUp = (NativeAPI.GetAsyncKeyState(NativeAPI.VK_SHIFT) >> 16) != 0;
                    OnEvent(new InputEvents.KeyReleasedEvent(keyUp, altUp, ctrlUp, shiftUp));
                    break;

                case NativeAPI.WindowMessage.MOUSEWHEEL:
                    OnEvent(new InputEvents.MouseWheelEvent((int)(wParam >> 16) / 120));
                    break;

                case NativeAPI.WindowMessage.LBUTTONDOWN:
                    NativeAPI.SetCapture(hwnd);
                    OnEvent(new InputEvents.MouseButtonPressedEvent(InputEvents.MouseButton.Left, (int)(lParam & 0xFFFF), (int)(lParam >> 16)));
                    break;

                case NativeAPI.WindowMessage.LBUTTONUP:
                    OnEvent(new InputEvents.MouseButtonReleasedEvent(InputEvents.MouseButton.Left, (int)(lParam & 0xFFFF), (int)(lParam >> 16)));
                    NativeAPI.ReleaseCapture();
                    break;

                case NativeAPI.WindowMessage.RBUTTONDOWN:
                    NativeAPI.SetCapture(hwnd);
                    OnEvent(new InputEvents.MouseButtonPressedEvent(InputEvents.MouseButton.Right, (int)(lParam & 0xFFFF), (int)(lParam >> 16)));
                    break;

                case NativeAPI.WindowMessage.RBUTTONUP:
                    OnEvent(new InputEvents.MouseButtonReleasedEvent(InputEvents.MouseButton.Right, (int)(lParam & 0xFFFF), (int)(lParam >> 16)));
                    NativeAPI.ReleaseCapture();
                    break;

                case NativeAPI.WindowMessage.MBUTTONDOWN:
                    NativeAPI.SetCapture(hwnd);
                    OnEvent(new InputEvents.MouseButtonPressedEvent(InputEvents.MouseButton.Middle, (int)(lParam & 0xFFFF), (int)(lParam >> 16)));
                    break;

                case NativeAPI.WindowMessage.MBUTTONUP:
                    OnEvent(new InputEvents.MouseButtonReleasedEvent(InputEvents.MouseButton.Middle, (int)(lParam & 0xFFFF), (int)(lParam >> 16)));
                    NativeAPI.ReleaseCapture();
                    break;

                case NativeAPI.WindowMessage.XBUTTONDOWN:
                    NativeAPI.SetCapture(hwnd);
                    InputEvents.MouseButton xButtonDown = (wParam >> 16 == NativeAPI.VK_XBUTTON1 ? InputEvents.MouseButton.XButton1 : InputEvents.MouseButton.XButton2);
                    OnEvent(new InputEvents.MouseButtonPressedEvent(xButtonDown, (int)(lParam & 0xFFFF), (int)(lParam >> 16)));
                    break;

                case NativeAPI.WindowMessage.XBUTTONUP:
                    InputEvents.MouseButton xButtonUp = (wParam >> 16 == NativeAPI.VK_XBUTTON1 ? InputEvents.MouseButton.XButton1 : InputEvents.MouseButton.XButton2);
                    OnEvent(new InputEvents.MouseButtonReleasedEvent(xButtonUp, (int)(lParam & 0xFFFF), (int)(lParam >> 16)));
                    NativeAPI.ReleaseCapture();
                    break;

                case NativeAPI.WindowMessage.MOUSEMOVE:
                    if (ignoreMouseMove)
                    {
                        ignoreMouseMove = false;
                    }
                    else if (Focused)
                    {
                        int newX = (int)(lParam & 0xFFFF);
                        int newY = (int)(lParam >> 16);

                        if (lastX != newX || lastY != newY)
                        {
                            OnEvent(new InputEvents.MouseMoveEvent(newX, newY));

                            lastX = newX;
                            lastY = newY;
                        }

                        if (mouseCaptured)
                        {
                            NativeAPI.Rectangle mrect = new NativeAPI.Rectangle(0, 0, Width, Height);

                            NativeAPI.ClientToScreen(handle, ref mrect.LeftTop);
                            NativeAPI.ClientToScreen(handle, ref mrect.RightBottom);

                            MoveMouseCursorToMiddle(mrect);
                        }
                    }

                    break;
            }

            return NativeAPI.DefWindowProc(hwnd, uMsg, wParam, lParam);
        }

        public void GetMousePosition(out int x, out int y)
        {
            if (mouseCaptured)
            {
                x = lastX - Width / 2;
                y = lastY - Height / 2;

                lastX = Width / 2;
                lastY = Height / 2;
            }
            else
            {
                x = lastX;
                y = lastY;
            }
        }

        public void Dispose()
        {
            Renderer.OpenGL.Loader.Unload();
            Context.Dispose();
            NativeAPI.ReleaseDC(handle, dc);
            NativeAPI.DestroyWindow(handle);
            NativeAPI.UnregisterClass(className, IntPtr.Zero);
        }

        public bool ProcessEvents()
        {
            if (!alive)
            {
                return false;
            }

            NativeAPI.MSG msg;
            while (NativeAPI.PeekMessage(out msg, handle, 0, 0, NativeAPI.PeekRemoveMessage.Remove))
            {
                NativeAPI.TranslateMessage(ref msg);
                NativeAPI.DispatchMessage(ref msg);
            }

            return true;
        }

        public void Close()
        {
            alive = false;
        }

        public void Show()
        {
            NativeAPI.ShowWindow(handle, NativeAPI.ShowWindowCommand.ShowNormal);
        }

        public void SetPositionSameAs(IPlatformWindow existingWindow)
        {
            WindowsWindow w = (WindowsWindow)existingWindow;
            NativeAPI.Rectangle rect = new NativeAPI.Rectangle();
            NativeAPI.GetWindowRect(w.handle, out rect);
            NativeAPI.SetWindowPos(handle, IntPtr.Zero, rect.Left, rect.Top, 0, 0, NativeAPI.WindowPosFlags.NoSize);
        }

        public void SetTitle(string title)
        {
            NativeAPI.SetWindowText(handle, title);
        }
        
        public void SetSize(int width, int height)
        {
            if (this.Fullscreen == false)
            {
                this.Width = width;
                this.Height = height;

                NativeAPI.WindowStyle style;
                NativeAPI.WindowStyleEx styleEx;
                int x, y;
                int w = width;
                int h = height;
                GetWindowStyle(Fullscreen, resizable, ref w, ref h, out x, out y, out style, out styleEx);

                NativeAPI.SetWindowPos(handle, Fullscreen ? NativeAPI.HWND_TOPMOST : NativeAPI.HWND_TOP, 0, 0, w, h,
                    NativeAPI.WindowPosFlags.NoReposition | NativeAPI.WindowPosFlags.NoMove |
                    NativeAPI.WindowPosFlags.NoCopyBits);
            }
        }
    
        public bool SetFullscreen(bool fullscreen)
        {
            if (fullscreen)
            {
                NativeAPI.DevMode dm = new NativeAPI.DevMode();
                dm.Size = (ushort)Marshal.SizeOf(dm.GetType());
                dm.Fields = NativeAPI.DevModeFields.BitsPerPel | NativeAPI.DevModeFields.PelsWidth | NativeAPI.DevModeFields.PelsHeight;
                dm.BitsPerPel = 32;
                dm.PelsWidth = Width;
                dm.PelsHeight = Height;

                if (NativeAPI.ChangeDisplaySettings(ref dm, NativeAPI.CDSFlags.Fullscreen) != 0)
                {
                    return false;
                }
            }
            else
            {
                if (handle != IntPtr.Zero)
                {
                    NativeAPI.ChangeDisplaySettings(IntPtr.Zero, 0);
                }
            }

            this.Fullscreen = fullscreen;

            if (handle != IntPtr.Zero)
            {
                NativeAPI.WindowStyle style;
                NativeAPI.WindowStyleEx styleEx;
                int x, y;
                int w = Width;
                int h = Height;
                GetWindowStyle(this.Fullscreen, resizable, ref w, ref h, out x, out y, out style, out styleEx);

                if (this.Fullscreen == false)
                {
                    x = 0;
                    y = 0;
                }
                
                NativeAPI.SetWindowLong(handle, NativeAPI.WindowLongIndex.Style, (int)style);
                NativeAPI.SetWindowLong(handle, NativeAPI.WindowLongIndex.ExStyle, (int)styleEx);

                NativeAPI.SetWindowPos(handle, fullscreen ? NativeAPI.HWND_TOPMOST : NativeAPI.HWND_TOP, x, y, w, h,
                    NativeAPI.WindowPosFlags.NoReposition | NativeAPI.WindowPosFlags.NoCopyBits |
                    NativeAPI.WindowPosFlags.FrameChanged | NativeAPI.WindowPosFlags.ShowWindow);
            }

            return fullscreen;
        }

        public void SetResizable(bool resizable)
        {
            this.resizable = resizable;

            if (this.Fullscreen == false)
            {
                NativeAPI.WindowStyle style;
                NativeAPI.WindowStyleEx styleEx;
                int x, y;
                int w = Width;
                int h = Height;
                GetWindowStyle(Fullscreen, resizable, ref w, ref h, out x, out y, out style, out styleEx);

                NativeAPI.SetWindowLong(handle, NativeAPI.WindowLongIndex.Style, (int)style);
                NativeAPI.SetWindowLong(handle, NativeAPI.WindowLongIndex.ExStyle, (int)styleEx);

                NativeAPI.SetWindowPos(handle, Fullscreen ? NativeAPI.HWND_TOPMOST : NativeAPI.HWND_TOP, 0, 0, w, h,
                      NativeAPI.WindowPosFlags.NoReposition | NativeAPI.WindowPosFlags.NoMove |
                      NativeAPI.WindowPosFlags.NoCopyBits | NativeAPI.WindowPosFlags.FrameChanged |
                      NativeAPI.WindowPosFlags.ShowWindow);
            }
        }

        void MoveMouseCursorToMiddle(NativeAPI.Rectangle screenRect)
        {
            ignoreMouseMove = true;
            NativeAPI.SetCursorPos((screenRect.Left + screenRect.Right) / 2, (screenRect.Top + screenRect.Bottom) / 2);
        }

        public void SetMouseCaptured(bool capture)
        {
            if (capture)
            {
                if (Focused)
                {
                    NativeAPI.ShowCursor(false);
                }

                NativeAPI.GetCursorPos(out mousePos);

                lastX = Width / 2;
                lastY = Height / 2;

                if (Focused)
                {
                    NativeAPI.Rectangle rect = new NativeAPI.Rectangle(0, 0, Width, Height);
                    NativeAPI.ClientToScreen(handle, ref rect.LeftTop);
                    NativeAPI.ClientToScreen(handle, ref rect.RightBottom);

                    MoveMouseCursorToMiddle(rect);

                    NativeAPI.ClipCursor(ref rect);
                }
            }
            else
            {
                if (Focused)
                {
                    NativeAPI.ClipCursor(IntPtr.Zero);
                    ignoreMouseMove = true;
                    NativeAPI.SetCursorPos(mousePos.x, mousePos.y);
                    NativeAPI.ShowCursor(true);
                }

                NativeAPI.ScreenToClient(handle, ref mousePos);

                lastX = mousePos.x;
                lastY = mousePos.y;
            }

            mouseCaptured = capture;
        }

        InputEvents.Key GetShiftState(bool down)
        {
            bool LShiftDown = (NativeAPI.GetAsyncKeyState(NativeAPI.VK_LSHIFT) >> 16) != 0;
            bool RShiftDown = (NativeAPI.GetAsyncKeyState(NativeAPI.VK_RSHIFT) >> 16) != 0;

            InputEvents.Key key = (InputEvents.Key)0;
            if (down)
            {
                if      (!LShiftPrevDown && LShiftDown) key = InputEvents.Key.LShift;
                else if (!RShiftPrevDown && RShiftDown) key = InputEvents.Key.RShift;
            }
            else
            {
                if      (LShiftPrevDown && !LShiftDown) key = InputEvents.Key.LShift;
                else if (RShiftPrevDown && !RShiftDown) key = InputEvents.Key.RShift;
            }

            LShiftPrevDown = LShiftDown;
            RShiftPrevDown = RShiftDown;

            return key;
        }

        InputEvents.Key FromVirtualKeyCode(uint wParam, uint lParam)
        {
            switch (wParam)
            {
                case NativeAPI.VK_MENU:        return ((lParam & (1 << 24)) != 0) ? InputEvents.Key.RAlt : InputEvents.Key.LAlt;
                case NativeAPI.VK_CONTROL:     return ((lParam & (1 << 24)) != 0) ? InputEvents.Key.RControl : InputEvents.Key.LControl;
                case NativeAPI.VK_LWIN :       return InputEvents.Key.LSystem;
                case NativeAPI.VK_RWIN :       return InputEvents.Key.RSystem;
                case NativeAPI.VK_APPS :       return InputEvents.Key.Menu;
                case NativeAPI.VK_OEM_1 :      return InputEvents.Key.SemiColon;
                case NativeAPI.VK_OEM_2 :      return InputEvents.Key.Slash;
                case NativeAPI.VK_OEM_PLUS :   return InputEvents.Key.Equal;
                case NativeAPI.VK_OEM_MINUS :  return InputEvents.Key.Dash;
                case NativeAPI.VK_OEM_4 :      return InputEvents.Key.LBracket;
                case NativeAPI.VK_OEM_6 :      return InputEvents.Key.RBracket;
                case NativeAPI.VK_OEM_COMMA :  return InputEvents.Key.Comma;
                case NativeAPI.VK_OEM_PERIOD : return InputEvents.Key.Period;
                case NativeAPI.VK_OEM_7 :      return InputEvents.Key.Quote;
                case NativeAPI.VK_OEM_5 :      return InputEvents.Key.BackSlash;
                case NativeAPI.VK_OEM_3 :      return InputEvents.Key.Tilde;
                case NativeAPI.VK_ESCAPE :     return InputEvents.Key.Escape;
                case NativeAPI.VK_SPACE :      return InputEvents.Key.Space;
                case NativeAPI.VK_RETURN :     return InputEvents.Key.Return;
                case NativeAPI.VK_BACK :       return InputEvents.Key.Back;
                case NativeAPI.VK_TAB :        return InputEvents.Key.Tab;
                case NativeAPI.VK_PRIOR :      return InputEvents.Key.PageUp;
                case NativeAPI.VK_NEXT :       return InputEvents.Key.PageDown;
                case NativeAPI.VK_END :        return InputEvents.Key.End;
                case NativeAPI.VK_HOME :       return InputEvents.Key.Home;
                case NativeAPI.VK_INSERT :     return InputEvents.Key.Insert;
                case NativeAPI.VK_DELETE :     return InputEvents.Key.Delete;
                case NativeAPI.VK_ADD :        return InputEvents.Key.Add;
                case NativeAPI.VK_SUBTRACT :   return InputEvents.Key.Subtract;
                case NativeAPI.VK_MULTIPLY :   return InputEvents.Key.Multiply;
                case NativeAPI.VK_DIVIDE :     return InputEvents.Key.Divide;
                case NativeAPI.VK_PAUSE :      return InputEvents.Key.Pause;
                case NativeAPI.VK_F1 :         return InputEvents.Key.F1;
                case NativeAPI.VK_F2 :         return InputEvents.Key.F2;
                case NativeAPI.VK_F3 :         return InputEvents.Key.F3;
                case NativeAPI.VK_F4 :         return InputEvents.Key.F4;
                case NativeAPI.VK_F5 :         return InputEvents.Key.F5;
                case NativeAPI.VK_F6 :         return InputEvents.Key.F6;
                case NativeAPI.VK_F7 :         return InputEvents.Key.F7;
                case NativeAPI.VK_F8 :         return InputEvents.Key.F8;
                case NativeAPI.VK_F9 :         return InputEvents.Key.F9;
                case NativeAPI.VK_F10 :        return InputEvents.Key.F10;
                case NativeAPI.VK_F11 :        return InputEvents.Key.F11;
                case NativeAPI.VK_F12 :        return InputEvents.Key.F12;
                case NativeAPI.VK_F13 :        return InputEvents.Key.F13;
                case NativeAPI.VK_F14 :        return InputEvents.Key.F14;
                case NativeAPI.VK_F15 :        return InputEvents.Key.F15;
                case NativeAPI.VK_LEFT :       return InputEvents.Key.Left;
                case NativeAPI.VK_RIGHT :      return InputEvents.Key.Right;
                case NativeAPI.VK_UP :         return InputEvents.Key.Up;
                case NativeAPI.VK_DOWN :       return InputEvents.Key.Down;
                case NativeAPI.VK_NUMPAD0 :    return InputEvents.Key.Numpad0;
                case NativeAPI.VK_NUMPAD1 :    return InputEvents.Key.Numpad1;
                case NativeAPI.VK_NUMPAD2 :    return InputEvents.Key.Numpad2;
                case NativeAPI.VK_NUMPAD3 :    return InputEvents.Key.Numpad3;
                case NativeAPI.VK_NUMPAD4 :    return InputEvents.Key.Numpad4;
                case NativeAPI.VK_NUMPAD5 :    return InputEvents.Key.Numpad5;
                case NativeAPI.VK_NUMPAD6 :    return InputEvents.Key.Numpad6;
                case NativeAPI.VK_NUMPAD7 :    return InputEvents.Key.Numpad7;
                case NativeAPI.VK_NUMPAD8 :    return InputEvents.Key.Numpad8;
                case NativeAPI.VK_NUMPAD9 :    return InputEvents.Key.Numpad9;
                case 'A' :            return InputEvents.Key.A;
                case 'Z' :            return InputEvents.Key.Z;
                case 'E' :            return InputEvents.Key.E;
                case 'R' :            return InputEvents.Key.R;
                case 'T' :            return InputEvents.Key.T;
                case 'Y' :            return InputEvents.Key.Y;
                case 'U' :            return InputEvents.Key.U;
                case 'I' :            return InputEvents.Key.I;
                case 'O' :            return InputEvents.Key.O;
                case 'P' :            return InputEvents.Key.P;
                case 'Q' :            return InputEvents.Key.Q;
                case 'S' :            return InputEvents.Key.S;
                case 'D' :            return InputEvents.Key.D;
                case 'F' :            return InputEvents.Key.F;
                case 'G' :            return InputEvents.Key.G;
                case 'H' :            return InputEvents.Key.H;
                case 'J' :            return InputEvents.Key.J;
                case 'K' :            return InputEvents.Key.K;
                case 'L' :            return InputEvents.Key.L;
                case 'M' :            return InputEvents.Key.M;
                case 'W' :            return InputEvents.Key.W;
                case 'X' :            return InputEvents.Key.X;
                case 'C' :            return InputEvents.Key.C;
                case 'V' :            return InputEvents.Key.V;
                case 'B' :            return InputEvents.Key.B;
                case 'N' :            return InputEvents.Key.N;
                case '0' :            return InputEvents.Key.Num0;
                case '1' :            return InputEvents.Key.Num1;
                case '2' :            return InputEvents.Key.Num2;
                case '3' :            return InputEvents.Key.Num3;
                case '4' :            return InputEvents.Key.Num4;
                case '5' :            return InputEvents.Key.Num5;
                case '6' :            return InputEvents.Key.Num6;
                case '7' :            return InputEvents.Key.Num7;
                case '8' :            return InputEvents.Key.Num8;
                case '9' :            return InputEvents.Key.Num9;
            }

            return (InputEvents.Key)0;
        }

        void GetWindowStyle(bool fullscreen, bool resizable, ref int width, ref int height, out int x, out int y, out NativeAPI.WindowStyle style, out NativeAPI.WindowStyleEx styleEx)
        {
            style = NativeAPI.WindowStyle.ClipChildren | NativeAPI.WindowStyle.ClipSiblings;
            styleEx = 0;

            if (fullscreen)
            {
                style |= NativeAPI.WindowStyle.Popup;
                styleEx |= NativeAPI.WindowStyleEx.Topmost | NativeAPI.WindowStyleEx.ApplicationWindow;
                x = 0;
                y = 0;
            }
            else
            {
                style |= NativeAPI.WindowStyle.Caption | NativeAPI.WindowStyle.SystemMenu | NativeAPI.WindowStyle.MinimizeBox;
                if (resizable)
                {
                    style |= NativeAPI.WindowStyle.SizeBox | NativeAPI.WindowStyle.MaximizeBox;
                }
                styleEx |= NativeAPI.WindowStyleEx.WindowEdge;

                NativeAPI.Rectangle rect = new NativeAPI.Rectangle(0, 0, width, height);
                NativeAPI.AdjustWindowRectEx(ref rect, style, false, styleEx);
                x = NativeAPI.CW_USEDEFAULT;
                y = NativeAPI.CW_USEDEFAULT;
                width = rect.Right - rect.Left;
                height = rect.Bottom - rect.Top;
            }
        }

        string title;
        bool resizable;

        int lastX = -1;
        int lastY = -1;

        NativeAPI.Point mousePos;
        bool mouseCaptured = false;
        bool ignoreMouseMove = false;

        bool LShiftPrevDown = false;
        bool RShiftPrevDown = false;

        bool alive = true;

        IntPtr handle;
        IntPtr dc;
        IntPtr wndclass;
        IntPtr hInstance;
        string className;

        NativeAPI.WindowProc windowProcDelegate;

        public IPlatformContext Context { get; private set;  }
        
        public ReadOnlyCollection<DisplayMode> SupportedModes
        {
            get
            {
                Collection<DisplayMode> modes = new Collection<DisplayMode>();

                NativeAPI.DevMode dm = new NativeAPI.DevMode();
                dm.Size = (ushort)Marshal.SizeOf(dm.GetType());

                uint mode = 0;
                while (NativeAPI.EnumDisplaySettings(IntPtr.Zero, mode++, ref dm))
                {
                    if (dm.BitsPerPel == 32 && dm.PelsWidth >= 640)
                    {
                        DisplayMode m = new DisplayMode() { Width = dm.PelsWidth, Height = dm.PelsHeight };
                        if (modes.Contains(m) == false)
                        {
                            modes.Add(m);
                        }
                    }
                }
                
                return new ReadOnlyCollection<DisplayMode>(modes);
            }
        }

        public event OnEventCallback OnEvent;
        public event OnResizeCallback OnResize;
        public event OnFocusGainedCallback OnFocusGained;
        public event OnFocusLostCallback OnFocusLost;
        public event OnCloseCallback OnClose;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool Fullscreen { get; private set; }
        public bool Focused { get; private set; }
    }
}
