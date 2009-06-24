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

namespace Perspective.PixelShader
{
    /// <summary>
    /// Represents an effect property.
    /// </summary>
    public class EffectProperty : EffectEntry
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="filter">A filter object.</param>
        public EffectProperty(FilterBase filter) : base(filter) { }

        /// <summary>
        /// Gets or sets the default value for the property.
        /// </summary>
        public string DefaultValue { get; set; }
        
        private Type _wrapperType;

        /// <summary>
        /// Gets or sets the type of the .NET wrapper.
        /// </summary>
        public Type WrapperType
        {
            get { return _wrapperType; }
            set 
            { 
                _wrapperType = value;
                if (_wrapperType.IsAssignableFrom(typeof(Color)))
                {
                    Type = "float4";
                }
                else if (_wrapperType.IsAssignableFrom(typeof(double)))
                {
                    Type = "float";
                }
            }
        }

        /// <summary>
        /// Gets the HLSL type of the property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public string Type { get; private set; }
    }
}
