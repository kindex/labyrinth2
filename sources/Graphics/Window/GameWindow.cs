using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Game.Graphics.Window
{
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public abstract class GameWindow : IDisposable
    {
        public GameWindow(int width, int height, int samples, string title) : this(width, height, samples, title, false, false)
        {
        }

        public GameWindow(int width, int height, int samples, string title, bool fullscreen, bool resizable)
        {
            _title = title;
            _resizable = resizable;

            _window = Platform.CreateWindow(width, height, samples, title, fullscreen, resizable);
            _window.Context.VSync = false;
            _window.OnEvent += OnEvent;
            _window.OnResize += OnResize;
            _window.OnFocusGained += OnFocusGained;
            _window.OnFocusLost += OnFocusLost;
            _window.OnClose += OnClose;

            _window.Show();
        }

        public void OnEvent(InputEvents.InputEvent ev)
        {
            _events.Enqueue(ev);
        }

        public void Dispose()
        {
            if (_window != null)
            {
                _window.Dispose();
                _window = null;
            }
        }

        public void Close()
        {
            _running = false;
        }

        public InputEvents.InputEvent GetNextInputEvent()
        {
            if (_events.Count > 0)
            {
                return _events.Dequeue();
            }

            return null;
        }

        public void Run()
        {
                Logger.write("Done loading native libraries");
            OnLoad();
            try
            {
                double invFreq = 1.0 / (double)Stopwatch.Frequency;
                long time = Stopwatch.GetTimestamp();

                Logger.write("Entering main loop");
                while (_running)
                {
                    if (_window.ProcessEvents() == false)
                    {
                        break;
                    }

                    long current = Stopwatch.GetTimestamp();
                    long delta = current - time;
                    float deltaTime = (float)(delta * invFreq);
                    time = current;

                    OnUpdate(deltaTime);
                    OnRender(deltaTime);

                    _window.Context.SwapBuffers();
                }
                Logger.write("Leaving main loop");
            }
            finally
            {
                OnUnload();
            }
        }

        public virtual void OnUpdate(float deltaTime)
        {
        }

        public virtual void OnRender(float deltaTime)
        {
        }

        public virtual void OnLoad()
        {
        }

        public virtual void OnUnload()
        {
        }

        public virtual void OnResize(int width, int height)
        {
        }

        public virtual void OnFocusGained()
        {
        }

        public virtual void OnFocusLost()
        {
        }

        public virtual void OnClose()
        {
        }

        public void SetSize(int newWidth, int newHeight)
        {
            if (_window.Width != newWidth && _window.Height != newHeight)
            {
                _window.SetSize(newWidth, newHeight);
            }
        }

        public bool Fullscreen
        {
            get
            {
                return _window.Fullscreen;
            }
            set
            {
                if (_window.Fullscreen != value)
                {
                    _window.SetFullscreen(value);
                }

            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    _window.SetTitle(value);
                }
            }
        }

        public bool VSync
        {
            get
            {
                return _window.Context.VSync;
            }
            set
            {
                if (_window.Context.VSync != value)
                {
                    _window.Context.VSync = value;
                }
            }
        }

        public int Width
        {
            get
            {
                return _window.Width;
            }
        }

        public int Height
        {
            get
            {
                return _window.Height;
            }
        }

        public int Samples
        {
            get
            {
                return _window.Context.Samples;
            }
            set
            {
                if (_window.Context.Samples != value)
                {
                    IPlatformWindow w = Platform.CreateWindow(Width, Height, value, _title, false, _resizable);
                    
                    w.OnEvent += OnEvent;
                    w.OnResize += OnResize;
                    w.OnFocusGained += OnFocusGained;
                    w.OnFocusLost += OnFocusLost;
                    w.OnClose += OnClose;

                    bool fullscreen = Fullscreen;
                    if (fullscreen)
                    {
                        _window.SetFullscreen(false);
                    }
                    _window.Context.ShareWith(w.Context);
                    w.SetPositionSameAs(_window);
                    _window.Dispose();
                    if (fullscreen)
                    {
                        w.SetFullscreen(true);
                    }
                    w.Context.MakeCurrent();
                    w.Show();

                    _window = w;
                }
            }
        }

        public bool Resizable
        {
            get
            {
                return _resizable;
            }
            set
            {
                if (_resizable != value)
                {
                    _resizable = value;
                    _window.SetResizable(value);
                }
            }
        }

        public bool MouseCaptured
        {
            get
            {
                return _mouseCaptured;
            }
            set
            {
                if (_mouseCaptured != value)
                {
                    _mouseCaptured = value;
                    _window.SetMouseCaptured(value);
                }
            }
        }

        public bool Focused
        {
            get
            {
                return _window.Focused;
            }
        }

        public Point MousePosition
        {
            get
            {
                int x;
                int y;
                _window.GetMousePosition(out x, out y);
                return new Point(x, y);
            }
        }

        public long SupportedSamples
        {
            get
            {
                return _window.Context.SupportedSamples;
            }
        }

        public ReadOnlyCollection<DisplayMode> SupportedFullscreenModes
        {
            get
            {
                if (_fullscreenModes == null)
                {
                    _fullscreenModes = _window.SupportedModes;
                }

                return _fullscreenModes;
            }
        }

        bool _running = true;
        string _title;
        bool _resizable;
        bool _mouseCaptured;

        IPlatformWindow _window;

        ReadOnlyCollection<DisplayMode> _fullscreenModes;

        Queue<InputEvents.InputEvent> _events = new Queue<InputEvents.InputEvent>();
    }
}
