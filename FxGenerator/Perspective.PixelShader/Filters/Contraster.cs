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
    /// A contrast filter. 
    /// The default level (1.0) doesn't affect the image.
    /// Generates the HLSL code for a WPF or Silverlight effect.
    /// </summary>
    public class Contraster : FilterBase
    {
        EffectProperty _level;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Contraster()
        {
            _level = new EffectProperty(this);
            _level.Name = "Level";
            _level.WrapperType = typeof(double);
            _level.DefaultValue = "1.0";
            Properties.Add(_level);
        }

        /// <summary>
        /// Generates the HLSL body.
        /// </summary>
        /// <param name="output">The writer object for code generation.</param>
        public override void MainHlsl(System.CodeDom.Compiler.IndentedTextWriter output)
        {
            output.WriteLine("color.rgb = color.rgb - {0} * (color.rgb - 1.0f) * color.rgb * (color.rgb - 0.5f);", this._level.FullName);
        }
    }
}
