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
    /// A solarization filter. 
    /// Generates the HLSL code for a WPF or Silverlight effect.
    /// </summary>
    public class Solarizer : FilterBase
    {
        EffectProperty _property;
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Solarizer(): base()
        {
            _property = new EffectProperty(this);
            _property.Name = "Threshold";
            //_property.Name = "solarizerThreshold";
            //_property.WrapperName = "SolarizerThreshold";
            _property.WrapperType = typeof(double);
            _property.DefaultValue = "0.7";
            Properties.Add(_property);
        }

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
            output.WriteLine("f = 1.0 - f;");
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
            output.WriteLine("color.r = {0}ApplyThreshold(color.r);",
                this.FilterName);
            output.WriteLine("color.g = {0}ApplyThreshold(color.g);",
                this.FilterName);
            output.WriteLine("color.b = {0}ApplyThreshold(color.b);",
                this.FilterName);
        }
    }
}
