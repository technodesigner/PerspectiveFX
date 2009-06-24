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
using System.CodeDom.Compiler;

namespace Perspective.PixelShader.Filters
{
    /// <summary>
    /// A filter to transform the input. 
    /// Generates the HLSL code for a WPF or Silverlight effect.
    /// </summary>
    public class Transformer : ColorSampler
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Transformer(): base()
        {
        }

        /// <summary>
        /// Generates the HLSL body.
        /// </summary>
        /// <param name="output">The writer object for code generation.</param>
        public override void MainHlsl(IndentedTextWriter output)
        {
            output.WriteLine("float2 coord = uv;");
            output.WriteLine("coord.x *= 2;");
            output.WriteLine("float4 color = tex2D(Input1, coord);");
        }
    }
}
