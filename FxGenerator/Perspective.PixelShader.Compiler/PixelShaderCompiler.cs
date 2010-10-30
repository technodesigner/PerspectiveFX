using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perspective.PixelShader.Compiler
{
    public static class PixelShaderCompiler
    {

        private const int _minD3DX9Version = 32; // DirectX SDK December 2006
        private const int _maxD3DX9Version = 43; // DirectX SDK June 2010

        // private void

        public static void Compile()
        {
            IntPtr hModule = IntPtr.Zero;
            // Microsoft.Win32.LoadLibrary
            for (int i = _maxD3DX9Version; i >= _minD3DX9Version; i--)
            {
                string dllFileName = String.Format("d3dx9_{0}.dll", i);
                hModule = Interop.LoadLibrary(dllFileName);
                if (hModule != IntPtr.Zero)
                {
                    break;
                }
            }
            if (hModule != IntPtr.Zero)
            {



                Interop.FreeLibrary(hModule);
            }
        }
    }
}
