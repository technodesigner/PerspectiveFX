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

namespace Perspective.PixelShader.Filters
{
   public class ColorContrast:FilterBase
    {
        EffectProperty _factor;

        public ColorContrast()
        {
            _factor = new EffectProperty(this);
            _factor.Name = "Factor";
            _factor.WrapperType = typeof(double);
            _factor.DefaultValue = "0.5";
            Properties.Add(_factor);
        }

        public override void MainHlsl(System.CodeDom.Compiler.IndentedTextWriter output)
        {
            output.WriteLine("color.rgb = 1.0 - exp(-(color.rgb * {0}));",this._factor.FullName);
        }
    }
}
