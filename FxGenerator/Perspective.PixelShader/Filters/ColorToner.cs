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

namespace Perspective.PixelShader.Filters
{
    /// <summary>
    /// A toner filter for grayscale images. 
    /// Generates the HLSL code for a WPF or Silverlight effect.
    /// </summary>
    public class ColorToner : FilterBase
    {
        EffectProperty _redRatio, _greenRatio, _blueRatio;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ColorToner(): base()
        {
            _redRatio = new EffectProperty(this);
            _redRatio.Name = "RedRatio";
            _redRatio.WrapperType = typeof(double);
            _redRatio.DefaultValue = "1.0";
            Properties.Add(_redRatio);

            _greenRatio = new EffectProperty(this);
            _greenRatio.Name = "GreenRatio";
            _greenRatio.WrapperType = typeof(double);
            _greenRatio.DefaultValue = "0.888";
            Properties.Add(_greenRatio);

            _blueRatio = new EffectProperty(this);
            _blueRatio.Name = "BlueRatio";
            _blueRatio.WrapperType = typeof(double);
            _blueRatio.DefaultValue = "0.692";
            Properties.Add(_blueRatio);
        }

        /// <summary>
        /// Generates the HLSL body.
        /// </summary>
        /// <param name="output">The writer object for code generation.</param>
        public override void MainHlsl(System.CodeDom.Compiler.IndentedTextWriter output)
        {
            //output.WriteLine("color.r = grayscale * {0};",
            //    _redRatio.FullName);
            //output.WriteLine("color.g = grayscale * {0};",
            //    _greenRatio.FullName);
            //output.WriteLine("color.b = grayscale * {0};",
            //    _blueRatio.FullName);
            output.WriteLine("color.r *= {0};",
                _redRatio.FullName);
            output.WriteLine("color.g *= {0};",
                _greenRatio.FullName);
            output.WriteLine("color.b *= {0};",
                _blueRatio.FullName);
        }
    }
}
