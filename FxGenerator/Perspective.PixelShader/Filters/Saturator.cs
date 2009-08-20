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
    /// A filter to change the saturation of an image.
    /// The default level (1.0) doesn't affect the image.
    /// A level of 0.0 gives a greyscale image.
    /// A negative level gives a negative image.
    /// Generates the HLSL code for a WPF or Silverlight effect.
    /// </summary>
    public class Saturator : FilterBase
    {
        EffectProperty _level;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Saturator()
        {
            _level = new EffectProperty(this);
            _level.Name = "Level";
            _level.WrapperType = typeof(double);
            _level.DefaultValue = "1.0";
            Properties.Add(_level);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void MainHlsl(System.CodeDom.Compiler.IndentedTextWriter output)
        {
            // see http://en.wikipedia.org/wiki/Luma_(video)
            output.WriteLine("float luminance = dot(color.rgb, float3(0.2126, 0.7152, 0.0722));");
            
            // The result is the interpolation between luminance and chromatic (rgb) image
            output.WriteLine("color.rgb = lerp(luminance.xxx, color.rgb, {0});", this._level.FullName);
        }
    }
}
