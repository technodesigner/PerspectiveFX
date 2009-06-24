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
using System.Collections.ObjectModel;
using System.CodeDom.Compiler;

namespace Perspective.PixelShader
{
    /// <summary>
    /// Base class for a filter.
    /// Generates the HLSL code for a WPF or Silverlight effect.
    /// </summary>
    public abstract class FilterBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        protected FilterBase()
        {
        }

        /// <summary>
        /// Gets or sets the name of the filter.
        /// </summary>
        // public string Name { get; private set; }
        public string FilterName { get; set; }

        private Collection<EffectProperty> _properties = new Collection<EffectProperty>();

        /// <summary>
        /// Gets the collection of the properties of the generated effect.
        /// </summary>
        public Collection<EffectProperty> Properties
        {
            get { return _properties; }
        }

        private Collection<EffectSampler> _samplers = new Collection<EffectSampler>();

        /// <summary>
        /// Gets the collection of the brushes (HLSL samplers) of the generated effect.
        /// </summary>
        public Collection<EffectSampler> Samplers
        {
            get { return _samplers; }
        }

        /// <summary>
        /// Generates the HLSL functions region.
        /// </summary>
        /// <param name="output">The writer object for code generation.</param>
        public virtual void HlslFunctions(IndentedTextWriter output)
        {
            return;
        }

        /// <summary>
        /// Generates the HLSL body.
        /// </summary>
        /// <param name="output">The writer object for code generation.</param>
        public abstract void MainHlsl(IndentedTextWriter output);
    }
}
