using System;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;

namespace Game.Graphics.Window.Windows
{
    public static class NativeAPI
    {
        const string OPENGL = "opengl32.dll";
        const string GDI = "gdi32.dll";
        const string USER = "user32.dll";
        const string KERNEL = "kernel32.dll";

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct Rectangle
        {
            public Rectangle(int left, int top, int right, int bottom) : this()
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            [FieldOffset(0)]
            public int Left;
            
            [FieldOffset(4)]
            public int Top;
            
            [FieldOffset(8)]
            public int Right;

            [FieldOffset(12)]
            public int Bottom;

            [FieldOffset(0)]
            public Point LeftTop;

            [FieldOffset(8)]
            public Point RightBottom;
        }

        public enum WindowMessage : uint
        {
            NULL                         = 0x0000,
            CREATE                       = 0x0001,
            DESTROY                      = 0x0002,
            MOVE                         = 0x0003,
            SIZE                         = 0x0005,
            ACTIVATE                     = 0x0006,
            SETFOCUS                     = 0x0007,
            KILLFOCUS                    = 0x0008,
            ENABLE                       = 0x000A,
            SETREDRAW                    = 0x000B,
            SETTEXT                      = 0x000C,
            GETTEXT                      = 0x000D,
            GETTEXTLENGTH                = 0x000E,
            PAINT                        = 0x000F,
            CLOSE                        = 0x0010,
            QUERYENDSESSION              = 0x0011,
            QUERYOPEN                    = 0x0013,
            ENDSESSION                   = 0x0016,
            QUIT                         = 0x0012,
            ERASEBKGND                   = 0x0014,
            SYSCOLORCHANGE               = 0x0015,
            SHOWWINDOW                   = 0x0018,
            WININICHANGE                 = 0x001A,
            SETTINGCHANGE                = WININICHANGE,
            DEVMODECHANGE                = 0x001B,
            ACTIVATEAPP                  = 0x001C,
            FONTCHANGE                   = 0x001D,
            TIMECHANGE                   = 0x001E,
            CANCELMODE                   = 0x001F,
            SETCURSOR                    = 0x0020,
            MOUSEACTIVATE                = 0x0021,
            CHILDACTIVATE                = 0x0022,
            QUEUESYNC                    = 0x0023,
            GETMINMAXINFO                = 0x0024,
            PAINTICON                    = 0x0026,
            ICONERASEBKGND               = 0x0027,
            NEXTDLGCTL                   = 0x0028,
            SPOOLERSTATUS                = 0x002A,
            DRAWITEM                     = 0x002B,
            MEASUREITEM                  = 0x002C,
            DELETEITEM                   = 0x002D,
            VKEYTOITEM                   = 0x002E,
            CHARTOITEM                   = 0x002F,
            SETFONT                      = 0x0030,
            GETFONT                      = 0x0031,
            SETHOTKEY                    = 0x0032,
            GETHOTKEY                    = 0x0033,
            QUERYDRAGICON                = 0x0037,
            COMPAREITEM                  = 0x0039,
            GETOBJECT                    = 0x003D,
            COMPACTING                   = 0x0041,
            WINDOWPOSCHANGING            = 0x0046,
            WINDOWPOSCHANGED             = 0x0047,
            POWER                        = 0x0048,
            COPYDATA                     = 0x004A,
            CANCELJOURNAL                = 0x004B,
            NOTIFY                       = 0x004E,
            INPUTLANGCHANGEREQUEST       = 0x0050,
            INPUTLANGCHANGE              = 0x0051,
            TCARD                        = 0x0052,
            HELP                         = 0x0053,
            USERCHANGED                  = 0x0054,
            NOTIFYFORMAT                 = 0x0055,
            CONTEXTMENU                  = 0x007B,
            STYLECHANGING                = 0x007C,
            STYLECHANGED                 = 0x007D,
            DISPLAYCHANGE                = 0x007E,
            GETICON                      = 0x007F,
            SETICON                      = 0x0080,
            NCCREATE                     = 0x0081,
            NCDESTROY                    = 0x0082,
            NCCALCSIZE                   = 0x0083,
            NCHITTEST                    = 0x0084,
            NCPAINT                      = 0x0085,
            NCACTIVATE                   = 0x0086,
            GETDLGCODE                   = 0x0087,
            SYNCPAINT                    = 0x0088,
            NCMOUSEMOVE                  = 0x00A0,
            NCLBUTTONDOWN                = 0x00A1,
            NCLBUTTONUP                  = 0x00A2,
            NCLBUTTONDBLCLK              = 0x00A3,
            NCRBUTTONDOWN                = 0x00A4,
            NCRBUTTONUP                  = 0x00A5,
            NCRBUTTONDBLCLK              = 0x00A6,
            NCMBUTTONDOWN                = 0x00A7,
            NCMBUTTONUP                  = 0x00A8,
            NCMBUTTONDBLCLK              = 0x00A9,
            NCXBUTTONDOWN                = 0x00AB,
            NCXBUTTONUP                  = 0x00AC,
            NCXBUTTONDBLCLK              = 0x00AD,
            INPUT_DEVICE_CHANGE          = 0x00FE,
            INPUT                        = 0x00FF,
            KEYFIRST                     = 0x0100,
            KEYDOWN                      = 0x0100,
            KEYUP                        = 0x0101,
            CHAR                         = 0x0102,
            DEADCHAR                     = 0x0103,
            SYSKEYDOWN                   = 0x0104,
            SYSKEYUP                     = 0x0105,
            SYSCHAR                      = 0x0106,
            SYSDEADCHAR                  = 0x0107,
            IME_STARTCOMPOSITION         = 0x010D,
            IME_ENDCOMPOSITION           = 0x010E,
            IME_COMPOSITION              = 0x010F,
            IME_KEYLAST                  = 0x010F,
            INITDIALOG                   = 0x0110,
            COMMAND                      = 0x0111,
            SYSCOMMAND                   = 0x0112,
            TIMER                        = 0x0113,
            HSCROLL                      = 0x0114,
            VSCROLL                      = 0x0115,
            INITMENU                     = 0x0116,
            INITMENUPOPUP                = 0x0117,
            MENUSELECT                   = 0x011F,
            MENUCHAR                     = 0x0120,
            ENTERIDLE                    = 0x0121,
            MENURBUTTONUP                = 0x0122,
            MENUDRAG                     = 0x0123,
            MENUGETOBJECT                = 0x0124,
            UNINITMENUPOPUP              = 0x0125,
            MENUCOMMAND                  = 0x0126,
            CHANGEUISTATE                = 0x0127,
            UPDATEUISTATE                = 0x0128,
            QUERYUISTATE                 = 0x0129,
            CTLCOLORMSGBOX               = 0x0132,
            CTLCOLOREDIT                 = 0x0133,
            CTLCOLORLISTBOX              = 0x0134,
            CTLCOLORBTN                  = 0x0135,
            CTLCOLORDLG                  = 0x0136,
            CTLCOLORSCROLLBAR            = 0x0137,
            CTLCOLORSTATIC               = 0x0138,
            MOUSEFIRST                   = 0x0200,
            MOUSEMOVE                    = 0x0200,
            LBUTTONDOWN                  = 0x0201,
            LBUTTONUP                    = 0x0202,
            LBUTTONDBLCLK                = 0x0203,
            RBUTTONDOWN                  = 0x0204,
            RBUTTONUP                    = 0x0205,
            RBUTTONDBLCLK                = 0x0206,
            MBUTTONDOWN                  = 0x0207,
            MBUTTONUP                    = 0x0208,
            MBUTTONDBLCLK                = 0x0209,
            MOUSEWHEEL                   = 0x020A,
            XBUTTONDOWN                  = 0x020B,
            XBUTTONUP                    = 0x020C,
            XBUTTONDBLCLK                = 0x020D,
            MOUSEHWHEEL                  = 0x020E,
            PARENTNOTIFY                 = 0x0210,
            ENTERMENULOOP                = 0x0211,
            EXITMENULOOP                 = 0x0212,
            NEXTMENU                     = 0x0213,
            SIZING                       = 0x0214,
            CAPTURECHANGED               = 0x0215,
            MOVING                       = 0x0216,
            POWERBROADCAST               = 0x0218,
            DEVICECHANGE                 = 0x0219,
            MDICREATE                    = 0x0220,
            MDIDESTROY                   = 0x0221,
            MDIACTIVATE                  = 0x0222,
            MDIRESTORE                   = 0x0223,
            MDINEXT                      = 0x0224,
            MDIMAXIMIZE                  = 0x0225,
            MDITILE                      = 0x0226,
            MDICASCADE                   = 0x0227,
            MDIICONARRANGE               = 0x0228,
            MDIGETACTIVE                 = 0x0229,
            MDISETMENU                   = 0x0230,
            ENTERSIZEMOVE                = 0x0231,
            EXITSIZEMOVE                 = 0x0232,
            DROPFILES                    = 0x0233,
            MDIREFRESHMENU               = 0x0234,
            IME_SETCONTEXT               = 0x0281,
            IME_NOTIFY                   = 0x0282,
            IME_CONTROL                  = 0x0283,
            IME_COMPOSITIONFULL          = 0x0284,
            IME_SELECT                   = 0x0285,
            IME_CHAR                     = 0x0286,
            IME_REQUEST                  = 0x0288,
            IME_KEYDOWN                  = 0x0290,
            IME_KEYUP                    = 0x0291,
            MOUSEHOVER                   = 0x02A1,
            MOUSELEAVE                   = 0x02A3,
            NCMOUSEHOVER                 = 0x02A0,
            NCMOUSELEAVE                 = 0x02A2,
            WTSSESSION_CHANGE            = 0x02B1,
            TABLET_FIRST                 = 0x02c0,
            TABLET_LAST                  = 0x02df,
            CUT                          = 0x0300,
            COPY                         = 0x0301,
            PASTE                        = 0x0302,
            CLEAR                        = 0x0303,
            UNDO                         = 0x0304,
            RENDERFORMAT                 = 0x0305,
            RENDERALLFORMATS             = 0x0306,
            DESTROYCLIPBOARD             = 0x0307,
            DRAWCLIPBOARD                = 0x0308,
            PAINTCLIPBOARD               = 0x0309,
            VSCROLLCLIPBOARD             = 0x030A,
            SIZECLIPBOARD                = 0x030B,
            ASKCBFORMATNAME              = 0x030C,
            CHANGECBCHAIN                = 0x030D,
            HSCROLLCLIPBOARD             = 0x030E,
            QUERYNEWPALETTE              = 0x030F,
            PALETTEISCHANGING            = 0x0310,
            PALETTECHANGED               = 0x0311,
            HOTKEY                       = 0x0312,
            PRINT                        = 0x0317,
            PRINTCLIENT                  = 0x0318,
            APPCOMMAND                   = 0x0319,
            THEMECHANGED                 = 0x031A,
            CLIPBOARDUPDATE              = 0x031D,
            DWMCOMPOSITIONCHANGED        = 0x031E,
            DWMNCRENDERINGCHANGED        = 0x031F,
            DWMCOLORIZATIONCOLORCHANGED  = 0x0320,
            DWMWINDOWMAXIMIZEDCHANGE     = 0x0321,
            GETTITLEBARINFOEX            = 0x033F,
            HANDHELDFIRST                = 0x0358,
            HANDHELDLAST                 = 0x035F,
            AFXFIRST                     = 0x0360,
            AFXLAST                      = 0x037F,
            PENWINFIRST                  = 0x0380,
            PENWINLAST                   = 0x038F,
            APP                          = 0x8000,
            USER                         = 0x0400,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            public IntPtr hwnd;
            public WindowMessage message;
            public uint wParam;
            public uint lParam;
            public uint time;
            public Point pt;
        }

        public enum PixelType : byte
        {
            RGBA = 0,
            Indexed = 1
        }

        [Flags]
        public enum PixelFormatDescriptorFlags : uint
        {
            DoubleBuffer = 0x01,
            Stereo = 0x02,
            DrawToWindow = 0x04,
            DrawToBitmap = 0x08,
            SupportGDI = 0x10,
            SupportOpenGL = 0x20,
            GenericFormat= 0x40,
            NeedPalette = 0x80,
            NeedSystemPalette = 0x100,
            SwapExchange = 0x200,
            SwapCopy = 0x400,
            SwapLayerBuffers = 0x800,
            GenericAccelerated = 0x1000,
            SupportDirectDraw = 0x2000,

            DepthDontCare = 0x20000000,
            DoubleBufferDontCare = 0x40000000,
            StereoDontCare = 0x80000000,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PixelFormatDescriptor
        {
            public ushort Size;
            public ushort Version;
            public PixelFormatDescriptorFlags Flags;
            public PixelType PixelType;
            public byte ColorBits;
            public byte RedBits;
            public byte RedShift;
            public byte GreenBits;
            public byte GreenShift;
            public byte BlueBits;
            public byte BlueShift;
            public byte AlphaBits;
            public byte AlphaShift;
            public byte AccumBits;
            public byte AccumRedBits;
            public byte AccumGreenBits;
            public byte AccumBlueBits;
            public byte AccumAlphaBits;
            public byte DepthBits;
            public byte StencilBits;
            public byte AuxBuffers;
            public byte LayerType;
            public byte Reserved;
            public uint LayerMask;
            public uint VisibleMask;
            public uint DamageMask;
        }

        [Flags]
        public enum WindowStyleEx : uint
        {
            AcceptFiles = 0x00000010,
            ApplicationWindow = 0x00040000,
            ClientEdge = 0x00000200,
            Composited = 0x02000000,
            ContextHelp = 0x00000400,
            ControlParent = 0x00010000,
            DialogModalFrame = 0x00000001,
            Layered = 0x00080000,
            LayoutRTL = 0x00400000,
            Left = 0x00000000,
            LeftScrollBar = 0x00004000,
            LeftToRightReading = 0x00000000,
            MDIChild = 0x00000040,
            NoActive = 0x08000000,
            NoInheritLayout = 0x00100000,
            NoParentNotify = 0x00000004,
            OverlappedWindow = WindowEdge | ClientEdge,
            PaletteWindow = WindowEdge | ToolWindow | Topmost,
            Right = 0x00001000,
            RightScrollbar = 0x00000000,
            RightToLeftReading = 0x00002000,
            StaticEdge = 0x00020000,
            ToolWindow = 0x00000080,
            Topmost = 0x00000008,
            Transparent = 0x00000020,
            WindowEdge = 0x00000100,
        }

        [Flags]
        public enum WindowStyle : uint
        {
            Border = 0x00800000,
            Caption = 0x00C00000,
            Child = 0x40000000,
            ChildWindow = Child,
            ClipChildren = 0x02000000,
            ClipSiblings = 0x04000000,
            Disabled = 0x08000000,
            DialogFrame = 0x00400000,
            Group = 0x00020000,
            HScroll = 0x00100000,
            Iconic = Minimize,
            Maximize = 0x01000000,
            MaximizeBox = 0x00010000,
            Minimize = 0x20000000,
            MinimizeBox = 0x00020000,
            Overlapped = 0,
            OverlappedWindow = Overlapped | Caption | SystemMenu | ThickFrame | MinimizeBox | MaximizeBox,
            Popup = 0x80000000,
            PopupWindow = Popup | Border | SystemMenu,
            SizeBox = ThickFrame,
            SystemMenu = 0x00080000,
            TabStop = 0x00010000,
            ThickFrame = 0x00040000,
            Tiled = Overlapped,
            TiledWindow = OverlappedWindow,
            Visible = 0x10000000,
            VScroll = 0x00200000,
        }

        public enum PeekRemoveMessage : uint
        {
            NoRemove = 0,
            Remove = 1,
            NoYield = 2,
        }

        [Flags]
        public enum ClassStyle : uint
        {
            ByteAlignClient = 0x1000,
            ByteAlignWindow = 0x2000,
            ClassDC = 0x0040,
            DoubleClicks = 0x0008,
            DropShadow = 0x00020000,
            GlobalClass = 0x4000,
            HRedraw = 0x0002,
            NoClose = 0x0200,
            OwnDC = 0x0020,
            ParentDC = 0x0080,
            SaveBits = 0x0800,
            VRedraw = 0x0001,
        }

        [Flags]
        public enum DevModeFields : int
        {
            Orientation = 0x00000001,
            PaperSize = 0x00000002,
            PaperLength = 0x00000004,
            PaperWidth = 0x00000008,
            Scale = 0x00000010,
            Copies = 0x00000100,
            DefaultSource = 0x00000200,
            PrintQuality = 0x00000400,
            Position = 0x00000020,
            DisplayOrientation = 0x00000080,
            DisplayFixedOutput = 0x20000000,
            Color = 0x00000800,
            Duplex = 0x00001000,
            YResolution = 0x00002000,
            TTOption = 0x00004000,
            Collate = 0x00008000,
            FormName = 0x00010000,
            LogPixels = 0x00020000,
            BitsPerPel = 0x00040000,
            PelsWidth = 0x00080000,
            PelsHeight = 0x00100000,
            DisplayFlags = 0x00200000,
            Nup = 0x00000040,
            DisplayFrequency = 0x00400000,
            ICMMethod = 0x00800000,
            ICMIntent = 0x01000000,
            MediaType = 0x02000000,
            DitherType = 0x04000000,
            PanningWidth = 0x08000000,
            PanningHeight = 0x10000000,
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
        public struct DevMode
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)]
            public char[] DeviceName;
            public ushort SpecVersion;
            public ushort DriverVersion;
            public ushort Size;
            public ushort DriverExtra;
            public DevModeFields Fields;

            public short Orientation;
            public short PaperSize;
            public short PaperLength;
            public short PaperWidth;
            public short PaperScale;
            public short PaperCopies;
            public short PaperDefaultSource;
            public short PaperPrintQuality;

            public short Color;
            public short Duplex;
            public short YResolution;
            public short TTOption;
            public short Collate; 
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)]
            public char[] FormName;
            public short LogPixels;
            public int BitsPerPel;
            public int PelsWidth;
            public int PelsHeight;
            public int DisplayFlags;
            public int DisplayFrequency;

            public int ICMMethod;
            public int ICMIntent;
            public int MediaType;
            public int DitherType;
            public int Reserved1;
            public int Reserved2;
            public int PanningWidth;
            public int PanningHeight;
        }

        [Flags]
        public enum CDSFlags : uint
        {
            Fullscreen = 4,
            Global = 8,
            NoReset = 0x10000000,
            Reset = 0x40000000,
            SetPrimary = 16,
            Test = 2,
            UpdateRegistry = 1, 
        }

        [UnmanagedFunctionPointerAttribute(CallingConvention.Winapi), SuppressUnmanagedCodeSecurity]
        public delegate uint WindowProc(IntPtr hwnd, WindowMessage uMsg, uint wParam, uint lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct WindowClass
        {
            public uint cbSize;
            public ClassStyle style;
            public IntPtr lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszMenuName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszClassName;
            public IntPtr hIconSm;
        };

        public enum WindowLongIndex : int
        {
            ExStyle = -20,
            Style = -16,
            HInstance = -6,
            ID = -12,
            UserData = -21,
        }

        [Flags]
        public enum WindowPosFlags : uint
        {
            AsyncWindowPos = 0x4000,
            DeferErase = 0x2000,
            DrawFrame = FrameChanged,
            FrameChanged = 0x0020,
            HideWindow = 0x0080,
            NoActivate = 0x0010,
            NoCopyBits = 0x0100,
            NoMove = 0x0002,
            NoOwnerZOrder = 0x0200,
            NoRedraw = 0x0008,
            NoReposition = NoOwnerZOrder,
            NoSendChanging = 0x0400,
            NoSize = 0x0001,
            NoZOrder = 0x0004,
            ShowWindow = 0x0040,
        }

        public enum ShowWindowCommand : int
        {
            ForceMinimize = 11,
            Hide = 0,
            Maximize = 3,
            Minimize = 6,
            Restore = 9,
            Show = 5,
            ShowDefault = 10,
            ShowMaximized = 3,
            ShowMinimized = 2,
            SjowMinimizedNoActive = 7,
            ShowNoActive = 4,
            ShowNA = 8,
            ShowNormal = 1,
        }

        public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        public static readonly IntPtr HWND_TOP = new IntPtr(0);
        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        public const int VK_LBUTTON        = 0x01;
        public const int VK_RBUTTON        = 0x02;
        public const int VK_CANCEL         = 0x03;
        public const int VK_MBUTTON        = 0x04;
        public const int VK_XBUTTON1       = 0x05;
        public const int VK_XBUTTON2       = 0x06;
        public const int VK_BACK           = 0x08;
        public const int VK_TAB            = 0x09;
        public const int VK_CLEAR          = 0x0C;
        public const int VK_RETURN         = 0x0D;
        public const int VK_SHIFT          = 0x10;
        public const int VK_CONTROL        = 0x11;
        public const int VK_MENU           = 0x12;
        public const int VK_PAUSE          = 0x13;
        public const int VK_CAPITAL        = 0x14;
        public const int VK_KANA           = 0x15;
        public const int VK_HANGUL         = 0x15;
        public const int VK_JUNJA          = 0x17;
        public const int VK_FINAL          = 0x18;
        public const int VK_HANJA          = 0x19;
        public const int VK_KANJI          = 0x19;
        public const int VK_ESCAPE         = 0x1B;
        public const int VK_CONVERT        = 0x1C;
        public const int VK_NONCONVERT     = 0x1D;
        public const int VK_ACCEPT         = 0x1E;
        public const int VK_MODECHANGE     = 0x1F;
        public const int VK_SPACE          = 0x20;
        public const int VK_PRIOR          = 0x21;
        public const int VK_NEXT           = 0x22;
        public const int VK_END            = 0x23;
        public const int VK_HOME           = 0x24;
        public const int VK_LEFT           = 0x25;
        public const int VK_UP             = 0x26;
        public const int VK_RIGHT          = 0x27;
        public const int VK_DOWN           = 0x28;
        public const int VK_SELECT         = 0x29;
        public const int VK_PRINT          = 0x2A;
        public const int VK_EXECUTE        = 0x2B;
        public const int VK_SNAPSHOT       = 0x2C;
        public const int VK_INSERT         = 0x2D;
        public const int VK_DELETE         = 0x2E;
        public const int VK_HELP           = 0x2F;
        public const int VK_LWIN           = 0x5B;
        public const int VK_RWIN           = 0x5C;
        public const int VK_APPS           = 0x5D;
        public const int VK_SLEEP          = 0x5F;
        public const int VK_NUMPAD0        = 0x60;
        public const int VK_NUMPAD1        = 0x61;
        public const int VK_NUMPAD2        = 0x62;
        public const int VK_NUMPAD3        = 0x63;
        public const int VK_NUMPAD4        = 0x64;
        public const int VK_NUMPAD5        = 0x65;
        public const int VK_NUMPAD6        = 0x66;
        public const int VK_NUMPAD7        = 0x67;
        public const int VK_NUMPAD8        = 0x68;
        public const int VK_NUMPAD9        = 0x69;
        public const int VK_MULTIPLY       = 0x6A;
        public const int VK_ADD            = 0x6B;
        public const int VK_SEPARATOR      = 0x6C;
        public const int VK_SUBTRACT       = 0x6D;
        public const int VK_DECIMAL        = 0x6E;
        public const int VK_DIVIDE         = 0x6F;
        public const int VK_F1             = 0x70;
        public const int VK_F2             = 0x71;
        public const int VK_F3             = 0x72;
        public const int VK_F4             = 0x73;
        public const int VK_F5             = 0x74;
        public const int VK_F6             = 0x75;
        public const int VK_F7             = 0x76;
        public const int VK_F8             = 0x77;
        public const int VK_F9             = 0x78;
        public const int VK_F10            = 0x79;
        public const int VK_F11            = 0x7A;
        public const int VK_F12            = 0x7B;
        public const int VK_F13            = 0x7C;
        public const int VK_F14            = 0x7D;
        public const int VK_F15            = 0x7E;
        public const int VK_F16            = 0x7F;
        public const int VK_F17            = 0x80;
        public const int VK_F18            = 0x81;
        public const int VK_F19            = 0x82;
        public const int VK_F20            = 0x83;
        public const int VK_F21            = 0x84;
        public const int VK_F22            = 0x85;
        public const int VK_F23            = 0x86;
        public const int VK_F24            = 0x87;
        public const int VK_NUMLOCK        = 0x90;
        public const int VK_SCROLL         = 0x91;
        public const int VK_OEM_NEC_EQUAL  = 0x92;
        public const int VK_OEM_FJ_JISHO   = 0x92;
        public const int VK_OEM_FJ_MASSHOU = 0x93;
        public const int VK_OEM_FJ_TOUROKU = 0x94;
        public const int VK_OEM_FJ_LOYA    = 0x95;
        public const int VK_OEM_FJ_ROYA    = 0x96;
        public const int VK_LSHIFT         = 0xA0;
        public const int VK_RSHIFT         = 0xA1;
        public const int VK_LCONTROL       = 0xA2;
        public const int VK_RCONTROL       = 0xA3;
        public const int VK_LMENU          = 0xA4;
        public const int VK_RMENU          = 0xA5;
        public const int VK_BROWSER_BACK        = 0xA6;
        public const int VK_BROWSER_FORWARD     = 0xA7;
        public const int VK_BROWSER_REFRESH     = 0xA8;
        public const int VK_BROWSER_STOP        = 0xA9;
        public const int VK_BROWSER_SEARCH      = 0xAA;
        public const int VK_BROWSER_FAVORITES   = 0xAB;
        public const int VK_BROWSER_HOME        = 0xAC;
        public const int VK_VOLUME_MUTE         = 0xAD;
        public const int VK_VOLUME_DOWN         = 0xAE;
        public const int VK_VOLUME_UP           = 0xAF;
        public const int VK_MEDIA_NEXT_TRACK    = 0xB0;
        public const int VK_MEDIA_PREV_TRACK    = 0xB1;
        public const int VK_MEDIA_STOP          = 0xB2;
        public const int VK_MEDIA_PLAY_PAUSE    = 0xB3;
        public const int VK_LAUNCH_MAIL         = 0xB4;
        public const int VK_LAUNCH_MEDIA_SELECT = 0xB5;
        public const int VK_LAUNCH_APP1         = 0xB6;
        public const int VK_LAUNCH_APP2         = 0xB7;
        public const int VK_OEM_1          = 0xBA;
        public const int VK_OEM_PLUS       = 0xBB;
        public const int VK_OEM_COMMA      = 0xBC;
        public const int VK_OEM_MINUS      = 0xBD;
        public const int VK_OEM_PERIOD     = 0xBE;
        public const int VK_OEM_2          = 0xBF;
        public const int VK_OEM_3          = 0xC0;
        public const int VK_OEM_4          = 0xDB;
        public const int VK_OEM_5          = 0xDC;
        public const int VK_OEM_6          = 0xDD;
        public const int VK_OEM_7          = 0xDE;
        public const int VK_OEM_8          = 0xDF;
        public const int VK_OEM_AX         = 0xE1;
        public const int VK_OEM_102        = 0xE2;
        public const int VK_ICO_HELP       = 0xE3;
        public const int VK_ICO_00         = 0xE4;
        public const int VK_PROCESSKEY     = 0xE5;
        public const int VK_ICO_CLEAR      = 0xE6;
        public const int VK_PACKET         = 0xE7;
        public const int VK_OEM_RESET      = 0xE9;
        public const int VK_OEM_JUMP       = 0xEA;
        public const int VK_OEM_PA1        = 0xEB;
        public const int VK_OEM_PA2        = 0xEC;
        public const int VK_OEM_PA3        = 0xED;
        public const int VK_OEM_WSCTRL     = 0xEE;
        public const int VK_OEM_CUSEL      = 0xEF;
        public const int VK_OEM_ATTN       = 0xF0;
        public const int VK_OEM_FINISH     = 0xF1;
        public const int VK_OEM_COPY       = 0xF2;
        public const int VK_OEM_AUTO       = 0xF3;
        public const int VK_OEM_ENLW       = 0xF4;
        public const int VK_OEM_BACKTAB    = 0xF5;
        public const int VK_ATTN           = 0xF6;
        public const int VK_CRSEL          = 0xF7;
        public const int VK_EXSEL          = 0xF8;
        public const int VK_EREOF          = 0xF9;
        public const int VK_PLAY           = 0xFA;
        public const int VK_ZOOM           = 0xFB;
        public const int VK_NONAME         = 0xFC;
        public const int VK_PA1            = 0xFD;
        public const int VK_OEM_CLEAR = 0xFE;

        public const int CW_USEDEFAULT = unchecked((int)0x80000000);

        [DllImport(KERNEL, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        


        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport(USER, CallingConvention=CallingConvention.Winapi, ExactSpelling=true), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReleaseDC(IntPtr hwnd, IntPtr DC);



        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr RegisterClassEx([In] ref WindowClass lpwcx);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnregisterClass(string lpClassName, IntPtr hInstance);



        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AdjustWindowRectEx(ref Rectangle lpRect, WindowStyle dwStyle, bool bMenu, WindowStyleEx dwExStyle);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr CreateWindowEx(WindowStyleEx dwExStyle, string lpClassName, string lpWindowName, WindowStyle dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowText(IntPtr hWnd, string lpString);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        public static extern int SetWindowLong(IntPtr hWnd, WindowLongIndex index, int dwNewLong);
        
        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, WindowPosFlags uFlags);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommand nCmdShow);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        public static extern int ShowCursor(bool show);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point point);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ScreenToClient(IntPtr hWnd, ref Point point);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ClipCursor([In] ref Rectangle lpRect);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ClipCursor(IntPtr lpRect);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out Point lpPoint);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCursorPos(int x, int y);


        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PeekMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, PeekRemoveMessage wRemoveMsg);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool TranslateMessage(ref MSG lpMsg);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        public static extern uint DispatchMessage([In] ref MSG lpmsg);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        public static extern uint DefWindowProc(IntPtr hWnd, WindowMessage Msg, uint wParam, uint lParam);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern void PostQuitMessage(int nExitCode);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        public static extern int GetKeyNameText(int lParam, StringBuilder lpString, int nSize);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern short GetAsyncKeyState(int vKey);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr SetCapture(IntPtr hWnd);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReleaseCapture();

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr LoadCursor(IntPtr hInstance, IntPtr lpCursorName);

        public static readonly IntPtr IDC_ARROW = new IntPtr(32512);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        public static extern int ChangeDisplaySettings([In] ref DevMode lpDevMode, CDSFlags dwflags);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        public static extern int ChangeDisplaySettings(IntPtr lpDevMode, CDSFlags dwflags);

        [DllImport(USER, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumDisplaySettings(IntPtr lpszDeviceName, uint iModeNum, ref DevMode lpDevMode);



        [DllImport(GDI, CallingConvention = CallingConvention.Winapi, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int ChoosePixelFormat(IntPtr hDc, [In] ref PixelFormatDescriptor pPfd);

        [DllImport(GDI, CallingConvention = CallingConvention.Winapi, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int DescribePixelFormat(IntPtr hdc, int ipfd, uint cjpfd, ref PixelFormatDescriptor ppfd);

        [DllImport(GDI, CallingConvention = CallingConvention.Winapi, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetPixelFormat(IntPtr hdc, int ipfd, [In] ref PixelFormatDescriptor ppfd);

        [DllImport(GDI, CallingConvention = CallingConvention.Winapi, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int GetPixelFormat(IntPtr hdc);



        [DllImport(OPENGL, CallingConvention = CallingConvention.Winapi, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr wglCreateContext(IntPtr hDc);

        [DllImport(OPENGL, CallingConvention = CallingConvention.Winapi, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern bool wglMakeCurrent(IntPtr hDc, IntPtr newContext);

        [DllImport(OPENGL, CallingConvention = CallingConvention.Winapi, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool wglDeleteContext(IntPtr hglrc);

        [DllImport(OPENGL, CallingConvention = CallingConvention.Winapi, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern bool wglSwapBuffers(IntPtr hdc);

        [DllImport(OPENGL, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr wglGetProcAddress(string lpszProc);

        [DllImport(OPENGL, CallingConvention = CallingConvention.Winapi, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern bool wglShareLists(IntPtr with, IntPtr from);



        [UnmanagedFunctionPointer(CallingConvention.Winapi), SuppressUnmanagedCodeSecurity]
        public delegate IntPtr wglGetExtensionsStringARB(IntPtr dc);

        [UnmanagedFunctionPointer(CallingConvention.Winapi), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool wglSwapIntervalEXT(int interval);

        [UnmanagedFunctionPointer(CallingConvention.Winapi), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool wglGetPixelFormatAttribiARB(IntPtr hdc, int iPixelFormat, int iLayerPlane, int nAttributes, [In] ref int piAttributes, out int piValues);

        [UnmanagedFunctionPointer(CallingConvention.Winapi), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool wglGetPixelFormatAttribivARB(IntPtr hdc, int iPixelFormat, int iLayerPlane, int nAttributes, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] int[] piAttributes, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] int[] piValues);

        [UnmanagedFunctionPointer(CallingConvention.Winapi), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool wglChoosePixelFormatARB(IntPtr hdc, [In] int[] piAttribIList, [In] float[] pfAttribFList, int nMaxFormats, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] int[] piFormats, out int nNumFormats);


        public const int WGL_NUMBER_PIXEL_FORMATS_ARB = 0x2000;
        public const int WGL_DRAW_TO_WINDOW_ARB = 0x2001;
        public const int WGL_DRAW_TO_BITMAP_ARB = 0x2002;
        public const int WGL_ACCELERATION_ARB = 0x2003;
        public const int WGL_NEED_PALETTE_ARB = 0x2004;
        public const int WGL_NEED_SYSTEM_PALETTE_ARB = 0x2005;
        public const int WGL_SWAP_LAYER_BUFFERS_ARB = 0x2006;
        public const int WGL_SWAP_METHOD_ARB = 0x2007;
        public const int WGL_NUMBER_OVERLAYS_ARB = 0x2008;
        public const int WGL_NUMBER_UNDERLAYS_ARB = 0x2009;
        public const int WGL_TRANSPARENT_ARB = 0x200A;
        public const int WGL_TRANSPARENT_RED_VALUE_ARB = 0x2037;
        public const int WGL_TRANSPARENT_GREEN_VALUE_ARB = 0x2038;
        public const int WGL_TRANSPARENT_BLUE_VALUE_ARB = 0x2039;
        public const int WGL_TRANSPARENT_ALPHA_VALUE_ARB = 0x203A;
        public const int WGL_TRANSPARENT_INDEX_VALUE_ARB = 0x203B;
        public const int WGL_SHARE_DEPTH_ARB = 0x200C;
        public const int WGL_SHARE_STENCIL_ARB = 0x200D;
        public const int WGL_SHARE_ACCUM_ARB = 0x200E;
        public const int WGL_SUPPORT_GDI_ARB = 0x200F;
        public const int WGL_SUPPORT_OPENGL_ARB = 0x2010;
        public const int WGL_DOUBLE_BUFFER_ARB = 0x2011;
        public const int WGL_STEREO_ARB = 0x2012;
        public const int WGL_PIXEL_TYPE_ARB = 0x2013;
        public const int WGL_COLOR_BITS_ARB = 0x2014;
        public const int WGL_RED_BITS_ARB = 0x2015;
        public const int WGL_RED_SHIFT_ARB = 0x2016;
        public const int WGL_GREEN_BITS_ARB = 0x2017;
        public const int WGL_GREEN_SHIFT_ARB = 0x2018;
        public const int WGL_BLUE_BITS_ARB = 0x2019;
        public const int WGL_BLUE_SHIFT_ARB = 0x201A;
        public const int WGL_ALPHA_BITS_ARB = 0x201B;
        public const int WGL_ALPHA_SHIFT_ARB = 0x201C;
        public const int WGL_ACCUM_BITS_ARB = 0x201D;
        public const int WGL_ACCUM_RED_BITS_ARB = 0x201E;
        public const int WGL_ACCUM_GREEN_BITS_ARB = 0x201F;
        public const int WGL_ACCUM_BLUE_BITS_ARB = 0x2020;
        public const int WGL_ACCUM_ALPHA_BITS_ARB = 0x2021;
        public const int WGL_DEPTH_BITS_ARB = 0x2022;
        public const int WGL_STENCIL_BITS_ARB = 0x2023;
        public const int WGL_AUX_BUFFERS_ARB = 0x2024;

        public const int WGL_NO_ACCELERATION_ARB = 0x2025;
        public const int WGL_GENERIC_ACCELERATION_ARB = 0x2026;
        public const int WGL_FULL_ACCELERATION_ARB = 0x2027;

        public const int WGL_SWAP_EXCHANGE_ARB = 0x2028;
        public const int WGL_SWAP_COPY_ARB = 0x2029;
        public const int WGL_SWAP_UNDEFINED_ARB = 0x202A;

        public const int WGL_TYPE_RGBA_ARB = 0x202B;
        public const int WGL_TYPE_COLORINDEX_ARB = 0x202C;

        public const int WGL_SAMPLE_BUFFERS_ARB = 0x2041;
        public const int WGL_SAMPLES_ARB = 0x2042;
    }
}
