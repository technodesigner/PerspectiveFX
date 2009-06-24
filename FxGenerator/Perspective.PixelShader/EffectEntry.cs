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
    /// Abstract class for an effect entry.
    /// </summary>
    public abstract class EffectEntry
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="filter">A filter object.</param>
        protected EffectEntry(FilterBase filter)
        {
            Filter = filter;
        }

        /// <summary>
        /// Gets the filter object.
        /// </summary>
        public FilterBase Filter { get; private set; }

        /// <summary>
        /// Gets or sets the name of the entry.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the full name of the entry, including the filter name.
        /// </summary>
        public string FullName 
        {
            get
            {
                if (Filter != null)
                {
                    return Filter.FilterName + this.Name;
                }
                return this.Name;
            }
        }

        /// <summary>
        /// Gets or sets a documentation comment for the entry.
        /// </summary>
        public string Comment { get; set; }
    }
}
