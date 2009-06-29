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
    /// A filter to warp the input. 
    /// Generates the HLSL code for a WPF or Silverlight effect.
    /// </summary>
    public class WaveWarper : FilterBase
    {
        EffectProperty _x1, _y1, _x2, _y2;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WaveWarper(): base()
        {
            _x1 = new EffectProperty(this);
            _x1.Name = "X1";
            _x1.WrapperType = typeof(double);
            _x1.DefaultValue = "10.0";
            Properties.Add(_x1);

            _x2 = new EffectProperty(this);
            _x2.Name = "X2";
            _x2.WrapperType = typeof(double);
            _x2.DefaultValue = "0.1";
            Properties.Add(_x2);

            _y1 = new EffectProperty(this);
            _y1.Name = "Y1";
            _y1.WrapperType = typeof(double);
            _y1.DefaultValue = "10.0";
            Properties.Add(_y1);

            _y2 = new EffectProperty(this);
            _y2.Name = "Y2";
            _y2.WrapperType = typeof(double);
            _y2.DefaultValue = "0.05";
            Properties.Add(_y2);
        }

        /// <summary>
        /// Generates the HLSL body.
        /// </summary>
        /// <param name="output">The writer object for code generation.</param>
        public override void MainHlsl(IndentedTextWriter output)
        {
            output.WriteLine("float2 coord = uv;");
            output.WriteLine("coord.x = coord.x + (sin(coord.y * {0}) * {1});",
                _y1.FullName,
                _y2.FullName);
            output.WriteLine("coord.y = coord.y + (sin(coord.x * {0}) * {1});",
                _x1.FullName,
                _x2.FullName);
            output.WriteLine("color = tex2D(Input1, coord);");
        }
    }
}
