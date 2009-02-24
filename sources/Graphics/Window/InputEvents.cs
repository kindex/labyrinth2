using System;

namespace Game.Graphics.Window.InputEvents
{
    public enum Key
    {
        A = 'a',
        B = 'b',
        C = 'c',
        D = 'd',
        E = 'e',
        F = 'f',
        G = 'g',
        H = 'h',
        I = 'i',
        J = 'j',
        K = 'k',
        L = 'l',
        M = 'm',
        N = 'n',
        O = 'o',
        P = 'p',
        Q = 'q',
        R = 'r',
        S = 's',
        T = 't',
        U = 'u',
        V = 'v',
        W = 'w',
        X = 'x',
        Y = 'y',
        Z = 'z',
        Num0 = '0',
        Num1 = '1',
        Num2 = '2',
        Num3 = '3',
        Num4 = '4',
        Num5 = '5',
        Num6 = '6',
        Num7 = '7',
        Num8 = '8',
        Num9 = '9',
        Escape = 256,
        LControl,
        LShift,
        LAlt,
        LSystem,
        RControl,
        RShift,
        RAlt,
        RSystem,
        Menu,
        LBracket,
        RBracket,
        SemiColon,
        Comma,
        Period,
        Quote,
        Slash,
        BackSlash,
        Tilde,
        Equal,
        Dash,
        Space,
        Return,
        Back,
        Tab,
        PageUp,
        PageDown,
        End,
        Home,
        Insert,
        Delete,
        Add,
        Subtract,
        Multiply,
        Divide,
        Left,
        Right,
        Up,
        Down,
        Numpad0,
        Numpad1,
        Numpad2,
        Numpad3,
        Numpad4,
        Numpad5,
        Numpad6,
        Numpad7,
        Numpad8,
        Numpad9,
        F1,
        F2,
        F3,
        F4,
        F5,
        F6,
        F7,
        F8,
        F9,
        F10,
        F11,
        F12,
        F13,
        F14,
        F15,
        Pause,
    };

    public enum MouseButton
    {
        Left,
        Right,
        Middle,
        XButton1,
        XButton2,
    }

    public interface InputEvent
    {
    }

    public class KeyEvent : InputEvent
    {
        public KeyEvent(Key key, bool alt, bool ctrl, bool shift)
        {
            this.Key = key;
            this.Alt = alt;
            this.Ctrl = ctrl;
            this.Shift = shift;
        }

        public Key Key;
        public bool Alt;
        public bool Ctrl;
        public bool Shift;
    }

    public class KeyPressedEvent : KeyEvent
    {
        public KeyPressedEvent(Key key, bool alt, bool ctrl, bool shift)
            : base(key, alt, ctrl, shift)
        {
        }
    }

    public class KeyReleasedEvent : KeyEvent
    {
        public KeyReleasedEvent(Key key, bool alt, bool ctrl, bool shift)
            : base(key, alt, ctrl, shift)
        {
        }
    }

    public class TextEvent : InputEvent
    {
        public TextEvent(char ch)
        {
            this.Char = ch;
        }

        public char Char;
    }

    public class MouseMoveEvent : InputEvent
    {
        public MouseMoveEvent(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X;
        public int Y;
    }

    public class MouseButtonEvent : InputEvent
    {
        public MouseButtonEvent(MouseButton button, int x, int y)
        {
            this.Button = button;
            this.X = x;
            this.Y = y;
        }

        public MouseButton Button;
        public int X;
        public int Y;
    }

    public class MouseButtonPressedEvent : MouseButtonEvent
    {
        public MouseButtonPressedEvent(MouseButton button, int x, int y)
            : base(button, x, y)
        {
        }
    }

    public class MouseButtonReleasedEvent : MouseButtonEvent
    {
        public MouseButtonReleasedEvent(MouseButton button, int x, int y)
            : base(button, x, y)
        {
        }
    }

    public class MouseWheelEvent : InputEvent
    {
        public MouseWheelEvent(int delta)
        {
            this.Delta = delta;
        }
        public int Delta;
    }

}
