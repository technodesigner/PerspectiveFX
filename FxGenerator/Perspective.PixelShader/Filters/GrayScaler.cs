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
    /// A black and white filter. 
    /// Generates the HLSL code for a WPF or Silverlight effect.
    /// </summary>
    public class GrayScaler : FilterBase
    {
        EffectProperty _redRatio, _greenRatio, _blueRatio;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GrayScaler():base()
        {
            _redRatio = new EffectProperty(this);
            _redRatio.Name = "RedRatio";
            _redRatio.WrapperType = typeof(double);
            _redRatio.DefaultValue = "0.30";
            Properties.Add(_redRatio);

            _greenRatio = new EffectProperty(this);
            _greenRatio.Name = "GreenRatio";
            _greenRatio.WrapperType = typeof(double);
            _greenRatio.DefaultValue = "0.59";
            Properties.Add(_greenRatio);

            _blueRatio = new EffectProperty(this);
            _blueRatio.Name = "BlueRatio";
            _blueRatio.WrapperType = typeof(double);
            _blueRatio.DefaultValue = "0.11";
            Properties.Add(_blueRatio);
        }

        /// <summary>
        /// Generates the HLSL body.
        /// </summary>
        /// <param name="output">The writer object for code generation.</param>
        public override void MainHlsl(IndentedTextWriter output)
        {
            output.WriteLine("float3 grayscale = dot(color.rgb, float3({0}, {1}, {2}));",
                _redRatio.FullName,
                _greenRatio.FullName,
                _blueRatio.FullName);
        }
    }
}
