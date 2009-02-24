using System;
using System.Security;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Game
{
    [AttributeUsageAttribute(AttributeTargets.Class, Inherited = false, AllowMultiple = false)] 
    public sealed class NativeLibraryAttribute : Attribute
    {
        public NativeLibraryAttribute(string dll)
        {
            this.DLL = dll;
        }

        public string DLL { get; private set; }
    }

    public static class NativeLoader
    {
        public static Dictionary<string, IntPtr> libraries = new Dictionary<string, IntPtr>();

        public static void AutoLoad()
        {
            foreach (Type type in Assembly.GetCallingAssembly().GetTypes())
            {
                NativeLibraryAttribute attr = (NativeLibraryAttribute)Attribute.GetCustomAttribute(type, typeof(NativeLibraryAttribute));
                if (attr != null)
                {
                    IntPtr lib;
                    if (libraries.ContainsKey(attr.DLL))
                    {
                        lib = libraries[attr.DLL];
                    }
                    else
                    {
                        Console.WriteLine("Loading {0}", attr.DLL);
                        lib = Load(attr.DLL);
                        libraries.Add(attr.DLL, lib);
                    }

                    LoadDelegates(lib, type, null);
                }
            }

            AppDomain.CurrentDomain.ProcessExit += OnExit;
        }

        static void OnExit(object sender, EventArgs e)
        {
            AppDomain.CurrentDomain.ProcessExit -= OnExit;

            foreach (var kv in libraries)
            {
                Console.WriteLine("Unloading {0}", kv.Key);
                Unload(kv.Value);
            }
        }

        public static IntPtr Load(string libname)
        {
            return LoadLibrary(libname);
        }

        public static void Unload(IntPtr handle)
        {
            FreeLibrary(handle);
        }

        public delegate IntPtr GetFunctionAddress(string name);

        public static void LoadDelegates(IntPtr handle, Type type, GetFunctionAddress addressGetter)
        {
            FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo f = fields[i];

                if (f.FieldType == typeof(int))
                {
                    continue;
                }

                string funcName = f.FieldType.Name;

                IntPtr address = GetProcAddress(handle, funcName);

                if (address == IntPtr.Zero && addressGetter != null)
                {
                    address = addressGetter(funcName);
                }

                if (address == IntPtr.Zero)
                {
                    f.SetValue(null, null);
                }
                else
                {
                    f.SetValue(null, Marshal.GetDelegateForFunctionPointer(address, f.FieldType));
                }
            }

            Type privateType = type.GetNestedType("Private");
            if (privateType != null)
            {
                LoadDelegates(handle, privateType, addressGetter);
            }
        }

        const string KERNEL = "kernel32.dll";

        [DllImport(KERNEL, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport(KERNEL, CallingConvention = CallingConvention.Winapi), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FreeLibrary(IntPtr hModule);

        [DllImport(KERNEL, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

    }
}
