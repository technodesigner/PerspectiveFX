﻿//------------------------------------------------------------------
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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System;
using System.Xaml;
using System.Windows.Markup;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Perspective.PixelShader")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(Perspective.PixelShader.LibraryInfo.Company)]
[assembly: AssemblyProduct("Perspective.PixelShader")]
[assembly: AssemblyCopyright(Perspective.PixelShader.LibraryInfo.Copyright)]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("9d1002b8-ca12-4ed5-86b3-67fc8ff4ad99")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
// [assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyVersion(Perspective.PixelShader.LibraryInfo.GlobalVersion)]
// [assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyFileVersion(Perspective.PixelShader.LibraryInfo.GlobalVersion)]
[assembly: CLSCompliant(true)]

[assembly: XmlnsDefinition(Perspective.PixelShader.LibraryInfo.XmlNamespace, "Perspective.PixelShader")]
[assembly: XmlnsDefinition(Perspective.PixelShader.LibraryInfo.XmlNamespace, "Perspective.PixelShader.Filters")]

namespace Perspective.PixelShader
{
    /// <summary>
    /// A class to handle the default assembly attribute values.
    /// </summary>
    public sealed class LibraryInfo
    {
        private LibraryInfo() { }

        /// <summary>
        /// The default XML namespace.
        /// </summary>
        public const string XmlNamespace = "http://www.codeplex.com/perspectivefx";

        /// <summary>
        /// Perspective global version number.
        /// 2.0.0.1 : 2.0 alpha
        /// 2.0.0.2 : 2.0 final
        /// 2.1.0.0 : 2.1 final (32-64 bits)
        /// 2.2.0.0 : WPF 4.0 - Silverlight 5/5.1 (32-64 bits)
        /// </summary>
        public const string GlobalVersion = "2.2.0.0";

        /// <summary>
        /// Global company name.
        /// </summary>
        public const string Company = "perspectivefx.codeplex.com";

        /// <summary>
        /// Global copyright information.
        /// </summary>
        public const string Copyright = "Copyright © http://perspectivefx.codeplex.com 2009-2012";
    }
}
