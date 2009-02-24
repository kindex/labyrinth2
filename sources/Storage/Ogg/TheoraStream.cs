using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Game.Storage.Ogg
{
    public sealed class TheoraStream : IDisposable
    {
        public TheoraStream(Stream input)
        {
            this.input = input;
            this.input_handle = GCHandle.Alloc(input, GCHandleType.Weak);
            this.decoder = NativeAPI.theora_create(GCHandle.ToIntPtr(input_handle), read_function);

            if (this.decoder == IntPtr.Zero)
            {
                throw new ApplicationException("Invalid Ogg Theora stream");
            }

            this.VideoWidth =  NativeAPI.theora_video_width(decoder);
            this.VideoHeight = NativeAPI.theora_video_height(decoder);
            this.VideoFramerate = (decimal)NativeAPI.theora_video_fps(decoder) / 1000;

            this.AudioRate = NativeAPI.theora_audio_frequency(decoder);
            this.AudioChannels = NativeAPI.theora_audio_channels(decoder);
            this.AudioBitrate = NativeAPI.theora_audio_bitrate(decoder);
        }

        ~TheoraStream()
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
                NativeAPI.theora_destroy(decoder);

                decoder = IntPtr.Zero;
            }
        }

        public bool Update(int audio_time_ms)
        {
            return NativeAPI.theora_update(decoder, audio_time_ms) != 0;
        }

        public int DecodeAudio(short[] buffer)
        {
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                return NativeAPI.theora_audio_decode(decoder, handle.AddrOfPinnedObject(), buffer.Length);
            }
            finally
            {
                handle.Free();
            }
        }

        public void DecodeVideo(uint[] buffer)
        {
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                NativeAPI.theora_video_decode(decoder, handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }

        public int VideoWidth { get; private set; }
        public int VideoHeight { get; private set; }
        public decimal VideoFramerate { get; private set; }

        public int AudioRate { get; private set; }
        public int AudioChannels { get; private set; }
        public int AudioBitrate { get; private set; }

        Stream input;
        GCHandle input_handle;
        IntPtr decoder;

        static NativeAPI.ReadFunction read_function = ReadFunction;

        static int ReadFunction(byte[] buffer, int size, int count, IntPtr arg)
        {
            Stream stream = (Stream)GCHandle.FromIntPtr(arg).Target;
            return stream.Read(buffer, 0, count);
        }
    }
}
