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
    /// A convolution filter. 
    /// Generates the HLSL code for a WPF or Silverlight effect.
    /// </summary>
    public class Convolution3X3Filter : FilterBase
    {
        EffectProperty 
            _m00, _m01, _m02, 
            _m10, _m11, _m12, 
            _m20, _m21, _m22, 
            _horizontalPixelCount, _verticalPixelCount,
            _divisor;

        const string _defaultHorizontalPixelCount = "40.0";
        const string _defaultVerticalPixelCount = "30.0";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Convolution3X3Filter(): base()
        {
            _horizontalPixelCount = new EffectProperty(this);
            _horizontalPixelCount.Name = "HorizontalPixelCount";
            _horizontalPixelCount.WrapperType = typeof(double);
            _horizontalPixelCount.DefaultValue = _defaultHorizontalPixelCount;
            Properties.Add(_horizontalPixelCount);

            _verticalPixelCount = new EffectProperty(this);
            _verticalPixelCount.Name = "VerticalPixelCount";
            _verticalPixelCount.WrapperType = typeof(double);
            _verticalPixelCount.DefaultValue = _defaultVerticalPixelCount;
            Properties.Add(_verticalPixelCount);

            _m00 = new EffectProperty(this);
            _m00.Name = "M00";
            _m00.WrapperType = typeof(double);
            _m00.DefaultValue = "0.0";
            Properties.Add(_m00);

            _m01 = new EffectProperty(this);
            _m01.Name = "M01";
            _m01.WrapperType = typeof(double);
            _m01.DefaultValue = "0.0";
            Properties.Add(_m01);

            _m02 = new EffectProperty(this);
            _m02.Name = "M02";
            _m02.WrapperType = typeof(double);
            _m02.DefaultValue = "0.0";
            Properties.Add(_m02);

            _m10 = new EffectProperty(this);
            _m10.Name = "M10";
            _m10.WrapperType = typeof(double);
            _m10.DefaultValue = "0.0";
            Properties.Add(_m10);

            _m11 = new EffectProperty(this);
            _m11.Name = "M11";
            _m11.WrapperType = typeof(double);
            _m11.DefaultValue = "0.0";
            Properties.Add(_m11);

            _m12 = new EffectProperty(this);
            _m12.Name = "M12";
            _m12.WrapperType = typeof(double);
            _m12.DefaultValue = "0.0";
            Properties.Add(_m12);

            _m20 = new EffectProperty(this);
            _m20.Name = "M20";
            _m20.WrapperType = typeof(double);
            _m20.DefaultValue = "0.0";
            Properties.Add(_m20);

            _m21 = new EffectProperty(this);
            _m21.Name = "M21";
            _m21.WrapperType = typeof(double);
            _m21.DefaultValue = "0.0";
            Properties.Add(_m21);

            _m22 = new EffectProperty(this);
            _m22.Name = "M22";
            _m22.WrapperType = typeof(double);
            _m22.DefaultValue = "0.0";
            Properties.Add(_m22);

            _divisor = new EffectProperty(this);
            _divisor.Name = "Divisor";
            _divisor.WrapperType = typeof(double);
            _divisor.DefaultValue = "1.0";
            Properties.Add(_divisor);
        }

        /// <summary>
        /// Generates the HLSL body.
        /// </summary>
        /// <param name="output">The writer object for code generation.</param>
        public override void MainHlsl(IndentedTextWriter output)
        {
            output.WriteLine("#define MATRIX_WIDTH 3");
            output.WriteLine("#define MATRIX_HEIGHT 3");
            output.WriteLine("");

            output.WriteLine("if ({0} == 0.0)", _horizontalPixelCount.FullName);
            output.WriteLine("{");
            output.Indent++;

            // Generates error X3025: global variables are implicitly constant, enable compatibility mode to allow modification
            // output.WriteLine("{0} = {1};", _horizontalPixelCount.FullName, _defaultHorizontalPixelCount);
            output.WriteLine("return color;");

            output.Indent--;
            output.WriteLine("}");
            output.WriteLine("float hpx = 1.0 / {0};", _horizontalPixelCount.FullName);

            output.WriteLine("if ({0} == 0.0)", _verticalPixelCount.FullName);
            output.WriteLine("{");
            output.Indent++;
            
            // Generates error X3025: global variables are implicitly constant, enable compatibility mode to allow modification
            // output.WriteLine("{0} = {1};", _verticalPixelCount.FullName, _defaultVerticalPixelCount);
            output.WriteLine("return color;");

            output.Indent--;
            output.WriteLine("}");
            output.WriteLine("float vpx = 1.0 / {0};", _verticalPixelCount.FullName);

            output.WriteLine("float2 offset[MATRIX_WIDTH][MATRIX_HEIGHT] = ");
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine("{float2(-hpx, -vpx), float2(0.0, -vpx), float2(hpx, -vpx)},");
            output.WriteLine("{float2(-hpx, 0.0), float2(0.0, 0.0), float2(hpx, 0.0)},");
            output.WriteLine("{float2(-hpx, vpx), float2(0.0, vpx), float2(hpx, vpx)}");
            output.Indent--;
            output.WriteLine("};");

            output.WriteLine("if ({0} == 0.0)", _divisor.FullName);
            output.WriteLine("{");
            output.Indent++;
            
            // Generates error X3025: global variables are implicitly constant, enable compatibility mode to allow modification
            // output.WriteLine("{0} = 1.0;", _divisor.FullName);
            output.WriteLine("return color;");

            output.Indent--;
            output.WriteLine("}");

            output.WriteLine("");
            output.WriteLine("color.rgb = 0.0;");

            output.WriteLine("color.rgb += (tex2D(Input1, uv + offset[0][0])) * {0} / {1};", _m00.FullName, _divisor.FullName);
            output.WriteLine("color.rgb += (tex2D(Input1, uv + offset[0][1])) * {0} / {1};", _m01.FullName, _divisor.FullName);
            output.WriteLine("color.rgb += (tex2D(Input1, uv + offset[0][2])) * {0} / {1};", _m02.FullName, _divisor.FullName);
            output.WriteLine("color.rgb += (tex2D(Input1, uv + offset[1][0])) * {0} / {1};", _m10.FullName, _divisor.FullName);
            output.WriteLine("color.rgb += (tex2D(Input1, uv + offset[1][1])) * {0} / {1};", _m11.FullName, _divisor.FullName);
            output.WriteLine("color.rgb += (tex2D(Input1, uv + offset[1][2])) * {0} / {1};", _m12.FullName, _divisor.FullName);
            output.WriteLine("color.rgb += (tex2D(Input1, uv + offset[2][0])) * {0} / {1};", _m20.FullName, _divisor.FullName);
            output.WriteLine("color.rgb += (tex2D(Input1, uv + offset[2][1])) * {0} / {1};", _m21.FullName, _divisor.FullName);
            output.WriteLine("color.rgb += (tex2D(Input1, uv + offset[2][2])) * {0} / {1};", _m22.FullName, _divisor.FullName);
        }
    }
}

