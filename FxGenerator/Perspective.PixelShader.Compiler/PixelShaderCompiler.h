//------------------------------------------------------------------
//
//  For licensing information and to get the latest version go to:
//  http://www.codeplex.com/perspectivefx
//
//  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY
//  OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
//  LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
//  FITNESS FOR A PARTICULAR PURPOSE.
//
//------------------------------------------------------------------
#pragma once

#include <d3d9.h>
#include <d3dx9.h>

#include <msclr/marshal.h>
using namespace msclr::interop;

using namespace System;
using namespace System::IO;
using namespace System::Diagnostics;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;

namespace Perspective{ namespace PixelShader{ namespace Compiler
{
	public enum class PixelShaderProfile{ps_2_0, ps_3_0};

	public ref class PixelShaderCompiler
	{
	private:
		PixelShaderProfile _profile;
		String^ _profileString;
		String^ _message;
		void SetProfile(PixelShaderProfile value)
		{
			_profile = value;
			switch(_profile)
			{
				case PixelShaderProfile::ps_2_0 :
					_profileString = "ps_2_0";
					break;
				case PixelShaderProfile::ps_3_0 :
					_profileString = "ps_3_0";
					break;
			}
		}
	public:
		PixelShaderCompiler(void);
		PixelShaderCompiler(PixelShaderProfile profile);
		// bool CompileFromFile(String^ fxFileName, [Out]String^ %errorMessage);
		bool CompileFromFile(String^ fxFileName);
		property PixelShaderProfile Profile
		{
			PixelShaderProfile get()
			{
				return _profile;
			}
		}
		property String^ Message
		{
			String^ get()
			{
				return _message;
			}
		}
	};
}}}

typedef HRESULT (WINAPI *D3DXCompileShaderFromFileFunction)(
	LPCSTR pSrcFile,
    CONST D3DXMACRO* pDefines,
    LPD3DXINCLUDE pInclude,
    LPCSTR pFunctionName,
    LPCSTR pProfile,
    DWORD Flags,
    LPD3DXBUFFER* ppShader,
    LPD3DXBUFFER* ppErrorMsgs,
    LPD3DXCONSTANTTABLE* ppConstantTable);

