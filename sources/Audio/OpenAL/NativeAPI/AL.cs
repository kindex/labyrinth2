using System;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;

namespace Game.Audio.OpenAL.NativeAPI
{
    [NativeLibrary("openal32.dll")]
    public static class AL
    {
        public enum Error : int
        {
            NoError = 0,
            InvalidName = 0xA001,
            InvalidEnum = 0xA002,
            InvalidValue = 0xA003,
            InvalidOperation = 0xA004,
            OutOfMemory = 0xA005,
        }

        public enum StringParam : int
        {
            Vendor = 0xB001,
            Version = 0xB002,
            Renderer = 0xB003,
            Extensions = 0xB004,
        }

        public enum ListenerFloatParam : int
        {
            Gain = 0x100A,
        }

        public enum ListenerVectorParam : int
        {
            Position = 0x1004,
            Velocity = 0x1006,
        }

        public enum ListenerOrientationParam : int
        {
            Orientation = 0x100F,
        }

        public enum SourceFloatParam : int
        {
            Gain = 0x100A,
            MinGain = 0x100D,
            MaxGain = 0x100E,
            ReferenceDistance = 0x1020,
            MaxDistance = 0x1023,
            RolloffFactor = 0x1021,
            ConeInnerAngle = 0x1001,
            ConeOuterAngle = 0x1002,
            ConeOuterGain = 0x1022,
            Pitch = 0x1003,
        }

        public enum SourceVectorParam : int
        {
            Position = 0x1004,
            Velocity = 0x1006,
            Direction = 0x1005,

        }

        public enum SourceIntParam : int
        {
            Relative = 0x202,
            Looping = 0x1007,
            SampleOffset = 0x1025,
            ByteOffset = 0x1026,
            Buffer = 0x1009,
            State = 0x1010,
            BuffersQueued = 0x1015,
            BuffersProcessed = 0x1016,
        }

        public enum SourceState : int
        {
            Initial = 0x1011,
            Playing = 0x1012,
            Paused = 0x1013,
            Stopped = 0x1014,
        }

        public enum BufferFormat : int
        {
            Mono8 = 0x1100,
            Mono16 = 0x1101,
            Stereo8 = 0x1102,
            Stereo16 = 0x1103,
        }

        public enum DistanceModelType : int
        {
            None = 0,
            InverseDistance = 0xD001,
            InverseDistanceClamped = 0xD002,
            LinearDistance = 0xD003,
            LinearDistanceClamped = 0xD004,
            ExponentDistance = 0xD005,
            ExponentDistanceClamped = 0xD006,
        }

        // State retrieval

        public static partial class Private
        {
            [SuppressUnmanagedCodeSecurity]
            public delegate IntPtr alGetString(StringParam param);
            public static alGetString GetString;
        }

        public static string GetString(StringParam name)
        {
            return Marshal.PtrToStringAnsi(Private.GetString(name));
        }

        // Error support.
        [SuppressUnmanagedCodeSecurity]
        public delegate Error alGetError();
        public static alGetError GetError;

        // Extension support.
        [SuppressUnmanagedCodeSecurity]
        public delegate bool alIsExtensionPresent([In, MarshalAs(UnmanagedType.LPStr)] string extname);
        public static alIsExtensionPresent IsExtensionPresent;

        [SuppressUnmanagedCodeSecurity]
        public delegate IntPtr alGetProcAddress([In, MarshalAs(UnmanagedType.LPStr)] string fname);
        public static alGetProcAddress GetProcAddress;

        // Listener

        public static partial class Private
        {
            [SuppressUnmanagedCodeSecurity]
            public delegate void alListenerf(ListenerFloatParam param, float value);
            public static alListenerf Listenerf;

            [SuppressUnmanagedCodeSecurity]
            public delegate void alListener3f(ListenerVectorParam param, float value1, float value2, float value3);
            public static alListener3f Listener3f;

            [SuppressUnmanagedCodeSecurity]
            public delegate void alListenerfv(ListenerOrientationParam param, [In, MarshalAs(UnmanagedType.LPArray, SizeConst = 6)] float[] values);
            public static alListenerfv Listenerfv;
        }

        public static void Listener(ListenerFloatParam param, float value)
        {
            Private.Listenerf(param, value);
        }

        public static void Listener(ListenerVectorParam param, Vector3 v)
        {
            Private.Listener3f(param, v.X, v.Y, v.Z);
        }

        public static void ListenerOrientation(Vector3 forward, Vector3 up)
        {
            float[] values = { forward.X, forward.Y, forward.Z, up.X, up.Y, up.Z };
            Private.Listenerfv(ListenerOrientationParam.Orientation, values);
        }

        public static partial class Private
        {
            [SuppressUnmanagedCodeSecurity]
            public delegate void alGenSources(int n, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] uint[] sids);
            public static alGenSources GenSources;

            [SuppressUnmanagedCodeSecurity]
            public delegate void alDeleteSources(int n, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] uint[] sids);
            public static alDeleteSources DeleteSources;
        }

        public static uint[] GenSources(int n)
        {
            uint[] result = new uint[n];
            Private.GenSources(n, result);
            return result;
        }

        public static uint GenSource()
        {
            uint[] sid = new uint[1];
            Private.GenSources(1, sid);
            return sid[0];
        }

        public static void DeleteSources(uint[] sids)
        {
            Private.DeleteSources(sids.Length, sids);
        }

        public static void DeleteSource(uint sid)
        {
            uint[] sids = { sid };
            Private.DeleteSources(1, sids);
        }

        public static partial class Private
        {
            [SuppressUnmanagedCodeSecurity]
            public delegate void alSourcef(uint sid, SourceFloatParam param, float value);
            public static alSourcef Sourcef;

            [SuppressUnmanagedCodeSecurity]
            public delegate void alSourcefv(uint sid, SourceVectorParam param, [In] ref Vector3 value);
            public static alSourcefv Sourcefv;

            [SuppressUnmanagedCodeSecurity]
            public delegate void alSourcei(uint sid, SourceIntParam param, int value);
            public static alSourcei Sourcei;
        }

        public static void Source(uint sid, SourceFloatParam param, float value)
        {
            Private.Sourcef(sid, param, value);
        }

        public static void Source(uint sid, SourceVectorParam param, Vector3 v)
        {
            Private.Sourcefv(sid, param, ref v);
        }

        public static void Source(uint sid, SourceIntParam param, int value)
        {
            Private.Sourcei(sid, param, value);
        }

        [SuppressUnmanagedCodeSecurity]
        public delegate void alGetSourcei(uint sid, SourceIntParam param, out int value);
        public static alGetSourcei GetSource;

        public static partial class Private
        {
            [SuppressUnmanagedCodeSecurity]
            public delegate void alSourcePlayv(int ns, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] uint[] sids);
            public static alSourcePlayv SourcePlayv;

            [SuppressUnmanagedCodeSecurity]
            public delegate void alSourceStopv(int ns, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] uint[] sids);
            public static alSourceStopv SourceStopv;

            [SuppressUnmanagedCodeSecurity]
            public delegate void alSourceRewindv(int ns, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] uint[] sids);
            public static alSourceRewindv SourceRewindv;

            [SuppressUnmanagedCodeSecurity]
            public delegate void alSourcePausev(int ns, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] uint[] sids);
            public static alSourcePausev SourcePausev;

            [SuppressUnmanagedCodeSecurity]
            public delegate void alSourcePlay(uint sid);
            public static alSourcePlay SourcePlay;

            [SuppressUnmanagedCodeSecurity]
            public delegate void alSourceStop(uint sid);
            public static alSourceStop SourceStop;

            [SuppressUnmanagedCodeSecurity]
            public delegate void alSourceRewind(uint sid);
            public static alSourceRewind SourceRewind;

            [SuppressUnmanagedCodeSecurity]
            public delegate void alSourcePause(uint sid);
            public static alSourcePause SourcePause;
        }

        public static void SourcePlay(uint[] sids)
        {
            Private.SourcePlayv(sids.Length, sids);
        }

        public static void SourceStop(uint[] sids)
        {
            Private.SourceStopv(sids.Length, sids);
        }

        public static void SourceRewind(uint[] sids)
        {
            Private.SourceRewindv(sids.Length, sids);
        }

        public static void SourcePause(uint[] sids)
        {
            Private.SourcePausev(sids.Length, sids);
        }

        public static void SourcePlay(uint sid)
        {
            Private.SourcePlay(sid);
        }

        public static void SourceStop(uint sid)
        {
            Private.SourceStop(sid);
        }

        public static void SourceRewind(uint sid)
        {
            Private.SourceRewind(sid);
        }

        public static void SourcePause(uint sid)
        {
            Private.SourcePause(sid);
        }

        [SuppressUnmanagedCodeSecurity]
        public delegate void alSourceQueueBuffers(uint sid, int numEntries, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] uint[] bids);
        public static alSourceQueueBuffers SourceQueueBuffers;

        [SuppressUnmanagedCodeSecurity]
        public delegate void alSourceUnqueueBuffers(uint sid, int numEntries, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] uint[] bids);
        public static alSourceUnqueueBuffers SourceUnqueueBuffers;

        // Buffer

        public static partial class Private
        {
            [SuppressUnmanagedCodeSecurity]
            public delegate void alGenBuffers(int n, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] uint[] buffers);
            public static alGenBuffers GenBuffers;

            [SuppressUnmanagedCodeSecurity]
            public delegate void alDeleteBuffers(int n, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] uint[] buffers);
            public static alDeleteBuffers DeleteBuffers;
        }

        public static uint[] GenBuffers(int n)
        {
            uint[] result = new uint[n];
            Private.GenBuffers(n, result);
            return result;
        }

        public static uint GenBuffer()
        {
            uint[] buffer = new uint[1];
            Private.GenBuffers(1, buffer);
            return buffer[0];
        }

        public static void DeleteBuffers(uint[] buffers)
        {
            Private.DeleteBuffers(buffers.Length, buffers);
        }

        public static void DeleteBuffer(uint buffer)
        {
            uint[] buffers = { buffer };
            Private.DeleteBuffers(1, buffers);
        }

        [SuppressUnmanagedCodeSecurity]
        public delegate void alBufferData(uint bid, BufferFormat format, IntPtr data, int size, int freq);
        public static alBufferData BufferData;

        [SuppressUnmanagedCodeSecurity]
        public delegate void alDopplerFactor(float value);
        public static alDopplerFactor DopplerFactor;

        [SuppressUnmanagedCodeSecurity]
        public delegate void alDopplerVelocity(float value);
        public static alDopplerVelocity DopplerVelocity;

        [SuppressUnmanagedCodeSecurity]
        public delegate void alSpeedOfSound(float value);
        public static alSpeedOfSound SpeedOfSound;

        [SuppressUnmanagedCodeSecurity]
        public delegate void alDistanceModel(DistanceModelType model);
        public static alDistanceModel DistanceModel;
    }
}
