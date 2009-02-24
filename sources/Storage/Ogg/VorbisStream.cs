using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Game.Storage.Ogg
{
    public sealed class VorbisStream : IDisposable
    {
        public VorbisStream(Stream input)
        {
            this.input = input;
            this.input_handle = GCHandle.Alloc(input, GCHandleType.Weak);
            this.decoder = NativeAPI.vorbis_create(GCHandle.ToIntPtr(input_handle),
                                                   read_function,
                                                   input.CanSeek ? seek_function : null,
                                                   input.CanSeek ? tell_function : null);
            if (decoder == IntPtr.Zero)
            {
                throw new ApplicationException("Invalid Ogg Vorbis stream");
            }

            this.Channels = NativeAPI.vorbis_channels(decoder);
            this.Bitrate = NativeAPI.vorbis_bitrate(decoder);
            this.Rate = NativeAPI.vorbis_frequency(decoder);
            this.Position = 0;
            this.Length = NativeAPI.vorbis_samples(decoder);
        }

        public int Decode(short[] buffer)
        {
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                int samples = NativeAPI.vorbis_decode(decoder, handle.AddrOfPinnedObject(), buffer.Length);
                Position += samples / Channels;
                return samples;
            }
            finally
            {
                handle.Free();
            }
        }

        ~VorbisStream()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (decoder != IntPtr.Zero)
            {
                input_handle.Free();
                NativeAPI.vorbis_destroy(decoder);

                decoder = IntPtr.Zero;
            }
        }

        public int Bitrate { get; private set; }
        public int Channels { get; private set; }
        public int Rate { get; private set; }
        public int Position { get; private set; }
        public int Length { get; private set; }

        Stream input;
        GCHandle input_handle;
        IntPtr decoder;

        static NativeAPI.ReadFunction read_function = ReadFunction;
        static NativeAPI.SeekFunction seek_function = SeekFunction;
        static NativeAPI.TellFunction tell_function = TellFunction;

        static int ReadFunction(byte[] buffer, int size, int count, IntPtr arg)
        {
            Stream stream = (Stream)GCHandle.FromIntPtr(arg).Target;
            return stream.Read(buffer, 0, count);
        }

        static int SeekFunction(IntPtr arg, int offset, int origin)
        {
            Stream stream = (Stream)GCHandle.FromIntPtr(arg).Target;
            if (origin == 0)
            {
                stream.Seek(offset, SeekOrigin.Begin);
            }
            else if (origin == 1)
            {
                stream.Seek(offset, SeekOrigin.Current);
            }
            else if (origin == 2)
            {
                stream.Seek(offset, SeekOrigin.End);
            }
            else
            {
                return 0;
            }
            return 1;
        }

        static int TellFunction(IntPtr arg)
        {
            Stream stream = (Stream)GCHandle.FromIntPtr(arg).Target;
            return (int)stream.Position;
        }
    }
}
