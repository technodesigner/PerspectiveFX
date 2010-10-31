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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using System.IO;
using Perspective.PixelShader.Compiler;
using Microsoft.Build.Framework;

namespace Perspective.PixelShader.BuildTask
{
    /// <summary>
    /// A custom MSBuild task to compile Silverlight and WPF pixel shaders.
    /// </summary>
    public class PixelShaderBuildTask : Task
    {
        [Required]
        public ITaskItem[] SourceFiles {get; set;}

        private List<ITaskItem> _outputFiles;

        [Output]
        public ITaskItem[] OutputFiles 
        {
            get
            {
                return _outputFiles.ToArray();
            }
            set
            {
                _outputFiles.AddRange(value);
            }
        }

        public PixelShaderBuildTask()
        {
            _outputFiles = new List<ITaskItem>();
        }

        public override bool Execute()
        {
            _outputFiles.Clear();
            bool result = false;
            PixelShaderCompiler compiler = new PixelShaderCompiler(PixelShaderProfile.ps_2_0);
            foreach (var item in SourceFiles)
            {
                result = compiler.CompileFromFile(item.ItemSpec);
                if (!result)
                {
                    Log.LogError(compiler.Message);
                    break;
                }
                else
                {
                    Log.LogMessageFromText(compiler.Message, MessageImportance.Normal);
                    _outputFiles.Add(new TaskItem(Path.ChangeExtension(item.ItemSpec, "ps")));
                }
            }
            return result;
        }
    }
}
