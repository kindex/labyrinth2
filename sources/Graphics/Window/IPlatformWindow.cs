using System;
using System.Collections.ObjectModel;

namespace Game.Graphics.Window
{
    public delegate void OnEventCallback(InputEvents.InputEvent ev);
    public delegate void OnResizeCallback(int width, int height);
    public delegate void OnFocusGainedCallback();
    public delegate void OnFocusLostCallback();
    public delegate void OnCloseCallback();

    public interface IPlatformWindow : IDisposable
    {
        bool ProcessEvents();
        void Close();
        
        void Show();

        void SetPositionSameAs(IPlatformWindow existingWindow);
        void SetTitle(string title);
        void SetSize(int width, int height);
        bool SetFullscreen(bool fullscreen);
        void SetResizable(bool resizable);
        void SetMouseCaptured(bool capture);

        void GetMousePosition(out int x, out int y);

        event OnEventCallback OnEvent;
        event OnResizeCallback OnResize;
        event OnFocusGainedCallback OnFocusGained;
        event OnFocusLostCallback OnFocusLost;
        event OnCloseCallback OnClose;

        int Width { get; }
        int Height { get; }
        bool Fullscreen { get; }
        bool Focused { get; }

        IPlatformContext Context { get; }

        ReadOnlyCollection<DisplayMode> SupportedModes { get; }
    }
}
