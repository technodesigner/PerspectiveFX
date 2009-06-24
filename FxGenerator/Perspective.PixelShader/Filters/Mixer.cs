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
    /// A filter to mix 2 inputs. 
    /// Generates the HLSL code for a WPF or Silverlight effect.
    /// </summary>
    public class Mixer : FilterBase
    {
        EffectProperty _gain1;
        EffectProperty _gain2;
        EffectSampler _sampler2;
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Mixer(): base()
        {
            _gain1 = new EffectProperty(this);
            _gain2 = new EffectProperty(this);
            _sampler2 = new EffectSampler(this);

            _sampler2.Name = "Input2";
            _sampler2.Comment = "A brush to mix with the original element.";
            Samplers.Add(_sampler2);

            _gain1.Name = "Gain1";
            _gain1.WrapperType = typeof(double);
            _gain1.DefaultValue = "0.5";
            Properties.Add(_gain1);

            _gain2.Name = "Gain2";
            _gain2.WrapperType = typeof(double);
            _gain2.DefaultValue = "0.5";
            Properties.Add(_gain2);
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
            output.WriteLine("color = {0} * color + {1} * {2}MaskColor;", 
                _gain1.FullName,
                _gain2.FullName,
                this.FilterName);
        }
    }
}
