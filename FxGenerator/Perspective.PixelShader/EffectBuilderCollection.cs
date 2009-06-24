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

namespace Perspective.PixelShader
{
    public class EffectBuilderCollection : Collection<EffectBuilder>
    {
        /// <summary>
        /// Gets or sets the .NET namespace of the wrapper.
        /// </summary>
        public string EffectNamespace { get; set; }

        /// <summary>
        /// Gets or sets the directory of the code files.
        /// </summary>
        public string EffectDirectory { get; set; }

        protected override void InsertItem(int index, EffectBuilder item)
        {
            base.InsertItem(index, item);
            item.ParentCollection = this;
        }

        protected override void RemoveItem(int index)
        {
            this[index].ParentCollection = null;
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, EffectBuilder item)
        {
            base.SetItem(index, item);
            item.ParentCollection = this;
        }

        protected override void ClearItems()
        {
            foreach (EffectBuilder item in this)
            {
                item.ParentCollection = null;
            }
            base.ClearItems();
        }
    }
}
