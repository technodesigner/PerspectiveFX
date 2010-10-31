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
#include "PixelShaderCompiler.h"

namespace Perspective{ namespace PixelShader{ namespace Compiler
{
	PixelShaderCompiler::PixelShaderCompiler(void)
	{
		SetProfile( PixelShaderProfile::ps_2_0 );
	}

	PixelShaderCompiler::PixelShaderCompiler(PixelShaderProfile profile)
	{
		SetProfile(profile);
	}

	// bool PixelShaderCompiler::CompileFromFile(String^ fxFileName, [Out]String^ %errorMessage)
	bool PixelShaderCompiler::CompileFromFile(String^ fxFileName)
	{
		bool result = false;
		marshal_context^ context = gcnew marshal_context();
		const int maxVersion = 60;
		const int minVersion = 32; // DirectX SDK December 2006
		int i = maxVersion;

		for (int i = maxVersion; i >= minVersion; i--)
		{
			String^ dllName = "d3dx9_" + i.ToString() + ".dll";
			HMODULE hD3DX = LoadLibrary(context->marshal_as<LPCWSTR>(dllName));
			if (hD3DX != NULL)
			{
				//FARPROC proc = GetProcAddress(hD3DX, "D3DXCompileShaderFromFileW");
				//D3DXCompileShaderFromFileFunction pfD3DXCompileShader = (D3DXCompileShaderFromFileFunction)proc;
				D3DXCompileShaderFromFileFunction pfD3DXCompileShader = 
					(D3DXCompileShaderFromFileFunction)GetProcAddress(hD3DX, "D3DXCompileShaderFromFileA");        
				if (!pfD3DXCompileShader)
				{
					_message = String::Format("{0} - {1} : D3DXCompileShaderFromFile GetProcAddress error", fxFileName, dllName);
				}
				else
				{
					LPD3DXBUFFER compiledShaderBuffer = NULL;
					LPD3DXBUFFER errorBuffer = NULL;
					//LPCSTR fileName = context->marshal_as<LPCSTR>(fxFileName);
					//LPCSTR profile = context->marshal_as<LPCSTR>(_profileString);

					//HRESULT hResult = 
					//	pfD3DXCompileShader(
					//		fileName, 
					//		NULL, // pDefines
					//		NULL, // pIncludes
					//		"main", // entrypoint
					//		profile,
					//		0, // compiler flags
					//		&compiledShaderBuffer,
					//		&errorBuffer,
					//		NULL   // constant table output
					//		);

					HRESULT hResult = 
						pfD3DXCompileShader(
							context->marshal_as<LPCSTR>(fxFileName), 
							NULL, // pDefines
							NULL, // pIncludes
							"main", // entrypoint
							context->marshal_as<LPCSTR>(_profileString),
							0, // compiler flags
							&compiledShaderBuffer,
							&errorBuffer,
							NULL   // constant table output
							);
					result = (hResult == D3D_OK);
					if (result)
					{
						if (!compiledShaderBuffer)
						{
							result = false;
							_message = String::Format("{0} - {2} : empty compilation buffer", fxFileName, dllName);
						}
						else
						{
		    				byte *compiledShader = (byte *)(compiledShaderBuffer->GetBufferPointer());
							// Array<Byte>^ psData = context->marshal_as<Array<Byte>^>(compiledShader);
							DWORD size = compiledShaderBuffer->GetBufferSize();
							array<byte>^ psData = gcnew array<byte>(size);
							Marshal::Copy(IntPtr(const_cast<byte*>(compiledShader)), psData, 0, size); 
							String^ psFileName = Path::ChangeExtension(fxFileName, "ps");
							File::WriteAllBytes(psFileName, psData);
							_message = String::Format("{0} compiled as {1} by {2}", fxFileName, psFileName, dllName);
						}
					}
					else
					{
						String^ errorCode = Convert::ToString(hResult);
						switch (hResult)
						{
							case D3DERR_INVALIDCALL :
								errorCode = "D3DERR_INVALIDCALL";
								break;
							case D3DXERR_INVALIDDATA :
								errorCode = "D3DXERR_INVALIDDATA";
								break;
							case E_OUTOFMEMORY :
								errorCode = "E_OUTOFMEMORY";
								break;
						}
						if (errorBuffer)
						{
							char *error = (char *)(errorBuffer->GetBufferPointer());
							_message = String::Format("{0} - {1} : Error {2} {3}", fxFileName, dllName, errorCode, context->marshal_as<String^>(error));
						}
						else
						{
							_message = String::Format("{0} - {1} : Error {2}", fxFileName, dllName, errorCode);
						}
					}
				}
				break;
			}
		}
		delete context;
		return result;
	}

}}}