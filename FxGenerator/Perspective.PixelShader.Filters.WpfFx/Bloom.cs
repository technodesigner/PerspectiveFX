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

namespace Perspective.PixelShader.Filters.WpfFx
{
    /// <summary>
    /// A bloom filter. 
    /// Generates the HLSL code for a WPF or Silverlight effect.
    /// </summary>
    public class Bloom : FilterBase
    {
        EffectProperty _baseSaturation;
        EffectProperty _bloomIntensity;
        EffectProperty _baseIntensity;
        EffectProperty _bloomSaturation;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Bloom() : base()
        {
            _baseSaturation = new EffectProperty(this);
            _bloomIntensity = new EffectProperty(this);
            _baseIntensity = new EffectProperty(this);
            _bloomSaturation = new EffectProperty(this);

            _baseSaturation.Name = "BaseSaturation";
            _baseSaturation.WrapperType = typeof(double);
            _baseSaturation.DefaultValue = "0.5";
            Properties.Add(_baseSaturation);

            _bloomIntensity.Name = "BloomIntensity";
            _bloomIntensity.WrapperType = typeof(double);
            _bloomIntensity.DefaultValue = "0.5";
            Properties.Add(_bloomIntensity);

            _baseIntensity.Name = "BaseIntensity";
            _baseIntensity.WrapperType = typeof(double);
            _baseIntensity.DefaultValue = "0.5";
            Properties.Add(_baseIntensity);

            _bloomSaturation.Name = "BloomSaturation";
            _bloomSaturation.WrapperType = typeof(double);
            _bloomSaturation.DefaultValue = "0.5";
            Properties.Add(_bloomSaturation);
        }

        /// <summary>
        /// Generates the HLSL functions region.
        /// </summary>
        /// <param name="output">The writer object for code generation.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public override void HlslFunctions(IndentedTextWriter output)
        {
            output.WriteLine("float4 AdjustSaturation(float4 color2, float saturation)");
            output.WriteLine("{");
            output.Indent++;
            output.WriteLine("float grey = dot(color2, float3(0.3, 0.59, 0.11));");
            output.WriteLine("return lerp(grey, color2, saturation);");
            output.Indent--;
            output.WriteLine("}");
        }

        /// <summary>
        /// Generates the HLSL body.
        /// </summary>
        /// <param name="output">The writer object for code generation.</param>
        public override void MainHlsl(IndentedTextWriter output)
        {
            output.WriteLine("float BloomThreshold = 0.25f;");

            output.WriteLine("float4 bloom = saturate((color - BloomThreshold) / (1 - BloomThreshold));");

            output.WriteLine("bloom = AdjustSaturation(bloom, {0}) * {1};", _bloomSaturation.FullName, _bloomIntensity.FullName);
            output.WriteLine("color = AdjustSaturation(color, {0}) * {1};", _baseSaturation.FullName, _baseIntensity.FullName);
            output.WriteLine("color *= (1 - saturate(bloom));");
            output.WriteLine("color += bloom;");
        }
    }
}
