using System;
using System.Security;
using System.Runtime.InteropServices;

namespace Game.Storage.Ogg
{
    [NativeLibrary("ogg.dll")]
    public static class NativeAPI
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int ReadFunction([Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] buffer, int size, int count, IntPtr arg);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int SeekFunction(IntPtr arg, int offset, int origin);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int TellFunction(IntPtr arg);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate IntPtr vorbis_decoder_create(IntPtr arg, ReadFunction read, SeekFunction seek, TellFunction tell);
        public static vorbis_decoder_create vorbis_create;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int vorbis_decoder_decode(IntPtr decoder, IntPtr buffer, int size);
        public static vorbis_decoder_decode vorbis_decode;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int vorbis_decoder_samples(IntPtr decoder);
        public static vorbis_decoder_samples vorbis_samples;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int vorbis_decoder_frequency(IntPtr decoder);
        public static vorbis_decoder_frequency vorbis_frequency;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int vorbis_decoder_channels(IntPtr decoder);
        public static vorbis_decoder_channels vorbis_channels;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int vorbis_decoder_bitrate(IntPtr decoder);
        public static vorbis_decoder_bitrate vorbis_bitrate;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void vorbis_decoder_destroy(IntPtr decoder);
        public static vorbis_decoder_destroy vorbis_destroy;



        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate IntPtr theora_decoder_create(IntPtr arg, ReadFunction read);
        public static theora_decoder_create theora_create;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int theora_decoder_update(IntPtr decoder, int audio_time_ms);
        public static theora_decoder_update theora_update;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int theora_decoder_audio_decode(IntPtr decoder, IntPtr buffer, int size);
        public static theora_decoder_audio_decode theora_audio_decode;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void theora_decoder_video_decode(IntPtr decoder, IntPtr buffer);
        public static theora_decoder_video_decode theora_video_decode;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int theora_decoder_audio_frequency(IntPtr decoder);
        public static theora_decoder_audio_frequency theora_audio_frequency;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int theora_decoder_audio_channels(IntPtr decoder);
        public static theora_decoder_audio_channels theora_audio_channels;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int theora_decoder_audio_bitrate(IntPtr decoder);
        public static theora_decoder_audio_bitrate theora_audio_bitrate;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int theora_decoder_video_width(IntPtr decoder);
        public static theora_decoder_video_width theora_video_width;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int theora_decoder_video_height(IntPtr decoder);
        public static theora_decoder_video_height theora_video_height;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int theora_decoder_video_fps(IntPtr decoder);
        public static theora_decoder_video_fps theora_video_fps;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int theora_decoder_finished(IntPtr decoder);
        public static theora_decoder_finished theora_finished;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void theora_decoder_destroy(IntPtr decoder);
        public static theora_decoder_destroy theora_destroy;
    }
}
