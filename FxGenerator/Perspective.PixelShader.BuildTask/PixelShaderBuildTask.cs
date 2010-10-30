using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using System.IO;

namespace Perspective.PixelShader.BuildTask
{
    public class PixelShaderBuildTask : Task
    {
        public string PixelShaderFileName { get; set; }

        public override bool Execute()
        {
            string CompiledPixelShaderFileName = Path.ChangeExtension(PixelShaderFileName, "ps");
            // File.WriteAllText(CompiledPixelShaderFileName, DateTime.Now.ToString());
            return true;
        }
    }
}
