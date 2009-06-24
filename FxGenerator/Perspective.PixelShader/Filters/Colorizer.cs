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
using System.Windows.Media;
using System.CodeDom.Compiler;

namespace Perspective.PixelShader.Filters
{
    /// <summary>
    /// A colored filter. 
    /// Generates the HLSL code for a WPF or Silverlight effect.
    /// </summary>
    public class Colorizer : FilterBase
    {
        EffectProperty _colorProperty;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Colorizer(): base()
        {
            _colorProperty = new EffectProperty(this);
            _colorProperty.Name = "Color";
            _colorProperty.WrapperType = typeof(Color);
            _colorProperty.DefaultValue = "Colors.Red";
            Properties.Add(_colorProperty);
        }

        /// <summary>
        /// Generates the HLSL body.
        /// </summary>
        /// <param name="output">The writer object for code generation.</param>
        public override void MainHlsl(IndentedTextWriter output)
        {
            output.WriteLine("color *= {0};", _colorProperty.FullName);
        }
    }
}
