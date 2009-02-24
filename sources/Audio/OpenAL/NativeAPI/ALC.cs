using System;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;

namespace Game.Audio.OpenAL.NativeAPI
{
    [NativeLibrary("openal32.dll")]
    public static class ALC
    {
        public enum Error : int
        {
            NoError = 0,
            InvalidDevice = 0xA001,
            InvalidContext = 0xA002,
            InvalidEnum = 0xA003,
            InvalidValue = 0xA004,
            OutOfMemory = 0xA005,
        }

        public enum StringParam : int
        {
            DefaultDeviceSpecifier = 0x1004,
            DeviceSpecifier = 0x1005,
            Extensions = 0x1006,
        }

        public enum IntegerParam : int
        {
            MajorVersion = 0x1000,
            MinorVersion = 0x1001,

        }

        // Device Management
        [SuppressUnmanagedCodeSecurity]
        public delegate IntPtr alcOpenDevice([In, MarshalAs(UnmanagedType.LPStr)] string devicename);
        public static alcOpenDevice OpenDevice;

        [SuppressUnmanagedCodeSecurity]
        public delegate bool alcCloseDevice(IntPtr device);
        public static alcCloseDevice CloseDevice;

        // Context Management
        [SuppressUnmanagedCodeSecurity]
        public delegate IntPtr alcCreateContext(IntPtr device, IntPtr attrlist);
        public static alcCreateContext CreateContext;

        [SuppressUnmanagedCodeSecurity]
        public delegate bool alcMakeContextCurrent(IntPtr context);
        public static alcMakeContextCurrent MakeContextCurrent;

        [SuppressUnmanagedCodeSecurity]
        public delegate void alcDestroyContext(IntPtr context);
        public static alcDestroyContext DestroyContext;

        // Error support.
        [SuppressUnmanagedCodeSecurity]
        public delegate Error alcGetError(IntPtr device);
        public static alcGetError GetError;

        // Extension support.
        [SuppressUnmanagedCodeSecurity]
        public delegate bool alcIsExtensionPresent(IntPtr device, [In, MarshalAs(UnmanagedType.LPStr)] string extname);
        public static alcIsExtensionPresent IsExtensionPresent;

        [SuppressUnmanagedCodeSecurity]
        public delegate IntPtr alcGetProcAddress(IntPtr device, [In, MarshalAs(UnmanagedType.LPStr)] string funcname);
        public static alcGetProcAddress GetProcAddress;

        public static partial class Private
        {
            // Query functions
            [SuppressUnmanagedCodeSecurity]
            public delegate IntPtr alcGetString(IntPtr device, StringParam param);
            public static alcGetString GetString;
        }

        public static string GetString(IntPtr device, StringParam name)
        {
            return Marshal.PtrToStringAnsi(Private.GetString(device, name));
        }

        [SuppressUnmanagedCodeSecurity]
        public delegate void alcGetIntegerv(IntPtr device, IntegerParam param, int size, out int data);
        public static alcGetIntegerv GetInteger;

        const int AttributesSize = 0x1002;
        const int AllAttributes = 0x1003;

        public enum DeviceAttributes : int
        {
            Frequency = 0x1007,
            Refresh = 0x1008,
            Sync = 0x1009,
            MonoSources = 0x1010,
            StereoSources = 0x1011,
        }

        public static void GetAllAttributes(IntPtr device, out int[] attributes)
        {
            int size;
            GetInteger(device, (IntegerParam)AttributesSize, 1, out size);
            attributes = new int[size];
            GetInteger(device, (IntegerParam)AllAttributes, size, out attributes[0]);
        }
    }

}
