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
    /// A filter that clips the input using an opacity mask. 
    /// Generates the HLSL code for a WPF or Silverlight effect.
    /// </summary>
    public class Clipper : FilterBase
    {
        EffectSampler _sampler2;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Clipper(): base()
        {
            _sampler2 = new EffectSampler(this);
            _sampler2.Name = "Mask";
            _sampler2.Comment = "The opacity mask brush";
            Samplers.Add(_sampler2);
        }

        /// <summary>
        /// Generates the HLSL body.
        /// </summary>
        /// <param name="output">The writer object for code generation.</param>
        public override void MainHlsl(IndentedTextWriter output)
        {
            output.WriteLine("float4 {0}MaskColor = tex2D({1}, uv);", 
                this.FilterName,
                _sampler2.FullName);
            output.WriteLine("color.a = {0}MaskColor.a;",
                this.FilterName);
        }
    }
}
