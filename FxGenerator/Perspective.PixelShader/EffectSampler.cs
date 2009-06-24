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

namespace Perspective.PixelShader
{
    /// <summary>
    /// Represents an effect input (a HLSL sampler, a WPF brush).
    /// </summary>
    public class EffectSampler : EffectEntry
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="filter">A filter object.</param>
        public EffectSampler(FilterBase filter) : base(filter) { }
    }
}
