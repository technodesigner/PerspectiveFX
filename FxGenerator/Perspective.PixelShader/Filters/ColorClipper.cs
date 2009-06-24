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
    /// A filter that clips the input using an opacity mask and a threshold value. 
    /// Generates the HLSL code for a WPF or Silverlight effect.
    /// </summary>
    public class ColorClipper : FilterBase
    {
        EffectSampler _sampler2;
        EffectProperty _property;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ColorClipper(): base()
        {
            _sampler2 = new EffectSampler(this);

            _sampler2.Name = "Mask";
            _sampler2.Comment = "The opacity mask brush";
            Samplers.Add(_sampler2);

            _property = new EffectProperty(this);
            _property.Name = "Threshold";
            _property.WrapperType = typeof(double);
            _property.DefaultValue = "0.3";
            Properties.Add(_property);
        }

        /// <summary>
        /// Generates the HLSL functions region.
        /// </summary>
        /// <param name="output">The writer object for code generation.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public override void HlslFunctions(IndentedTextWriter output)
        {
            output.WriteLine("float4 {0}ApplyThreshold(float f)",
                this.FilterName);
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine("if (f > {0})", _property.FullName);
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine("f = 0.0;");
            output.Indent--;
            output.WriteLine("}");
            output.WriteLine("else");
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine("f = 1.0;");
            output.Indent--;
            output.WriteLine("}");
            output.WriteLine("return f;");
            output.Indent--;
            output.WriteLine("}");
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
            output.WriteLine("color.a = {0}ApplyThreshold(color.r);",
                this.FilterName);
        }
    }
}
