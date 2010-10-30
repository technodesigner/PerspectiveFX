using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Perspective.PixelShader.Compiler
{
    public class Interop
    {
        [DllImport("kernel32", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string dllFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        public static extern UIntPtr GetProcAddress(IntPtr hModule, string procName);



        //[DllImport("kernel32")]
        //public extern static int LoadLibrary(string libraryName);

        //[DllImport("kernel32", CharSet = CharSet.Unicode)]
        //public extern static int GetProcAddress(int hWnd, string functionName);

        [DllImport("Invoke", CharSet = CharSet.Unicode)]
        public extern static int InvokeFunc(int funcptr, int hwnd,
                                            string message, string title, int flags);

    }


}
