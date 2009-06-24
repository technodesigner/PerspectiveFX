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
using System.IO;

namespace Perspective.PixelShader
{
    /// <summary>
    /// A code generator which produces a WPF or Silverlight HLSL effect from filters.
    /// </summary>
    [System.Windows.Markup.ContentProperty("Filters")]
    public class EffectBuilder
    {
        private IndentedTextWriter _shaderOut;
        private IndentedTextWriter _effectOut;

        /// <summary>
        /// Gets or sets the parent collection.
        /// </summary>
        internal EffectBuilderCollection ParentCollection { get; set; }

        private string _effectNamespace;
        /// <summary>
        /// Gets or sets the .NET namespace of the wrapper.
        /// </summary>
        public string EffectNamespace 
        {
            get
            {
                if ((String.IsNullOrEmpty(_effectNamespace))
                    && (ParentCollection != null))
                {
                    return ParentCollection.EffectNamespace;
                }
                return _effectNamespace;
            }

            set
            {
                _effectNamespace = value;
            }
        }

        private string _effectDirectory;
        /// <summary>
        /// Gets or sets the directory of the code files.
        /// </summary>
        public string EffectDirectory 
        {
            get
            {
                if ((String.IsNullOrEmpty(_effectDirectory))
                    && (ParentCollection != null))
                {
                    return ParentCollection.EffectDirectory;
                }
                return _effectDirectory;

            }
            set
            {
                _effectDirectory = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the effect.
        /// </summary>
        public string EffectName { get; set; }

        /// <summary>
        /// Gets or sets the default color sampler. Set it to null to generate the code from custom filters.
        /// </summary>
        public ColorSampler DefaultSampler { get; set; }

        private Collection<FilterBase> _filters = new Collection<FilterBase>();

        /// <summary>
        /// Gets the filter collection.
        /// </summary>
        public Collection<FilterBase> Filters
        {
            get { return _filters; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public EffectBuilder()
        {
            // DefaultSampler = new ColorSampler("DefaultSampler");
            DefaultSampler = new ColorSampler();
            DefaultSampler.FilterName = "DefaultSampler";
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        private void GenerateHlsl()
        {
            int propertyNumber = 0;
            foreach (FilterBase filter in _filters)
            {
                foreach (EffectProperty property in filter.Properties)
                {
                    if (!String.IsNullOrEmpty(property.Comment))
                    {
                        _shaderOut.WriteLine(
                            "//{0}",
                            property.Comment);
                    }
                    _shaderOut.WriteLine(
                        "{0} {1} : register(C{2});",
                        property.Type,
                        property.FullName,
                        propertyNumber);
                    _shaderOut.WriteLineNoTabs("");
                    propertyNumber++;
                    CheckPropertyCount(propertyNumber);
                }

                filter.HlslFunctions(_shaderOut);
                // _shaderOut.WriteLineNoTabs("");
            }

            _shaderOut.WriteLine("sampler2D Input1 : register(S0);");
            _shaderOut.WriteLineNoTabs("");

            int samplerNumber = 1;
            foreach (FilterBase filter in _filters)
            {
                foreach (EffectSampler sampler in filter.Samplers)
                {
                    if (!String.IsNullOrEmpty(sampler.Comment))
                    {
                        _shaderOut.WriteLine(
                            "//{0}",
                            sampler.Comment);
                    }
                    _shaderOut.WriteLine(
                        "sampler2D {0} : register(S{1});",
                        sampler.FullName,
                        samplerNumber);
                    _shaderOut.WriteLineNoTabs("");
                    samplerNumber++;
                    CheckSamplerCount(samplerNumber);
                }
            }

            _shaderOut.WriteLine("float4 main(float2 uv : TEXCOORD) : COLOR");
            _shaderOut.WriteLine("{");
            _shaderOut.Indent++;

            //_shaderOut.WriteLine("float4 color = tex2D(Input1, uv);");
            if (DefaultSampler != null)
            {
                DefaultSampler.MainHlsl(_shaderOut);
            }
            foreach (FilterBase filter in _filters)
            {
                filter.MainHlsl(_shaderOut);
            }
            _shaderOut.WriteLine("return color;");

            _shaderOut.Indent--;
            _shaderOut.Write("}");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        private void GenerateCs()
        {
            _effectOut.WriteLine("using System;");
            _effectOut.WriteLine("using System.Text;");
            _effectOut.WriteLine("using System.Windows;");
            _effectOut.WriteLine("using System.Windows.Media;");
            _effectOut.WriteLine("using System.Windows.Media.Effects;");
            _effectOut.WriteLineNoTabs("#if SILVERLIGHT");
            _effectOut.WriteLine("using UIPropertyMetadata = System.Windows.PropertyMetadata;");
            _effectOut.WriteLineNoTabs("#endif");
            _effectOut.WriteLineNoTabs("");
            _effectOut.WriteLine("namespace {0}", this.EffectNamespace);
            _effectOut.WriteLine("{");
            _effectOut.Indent++;

            _effectOut.WriteLine("public class {0} : ShaderEffect", this.EffectName);
            _effectOut.WriteLine("{");
            _effectOut.Indent++;

            _effectOut.WriteLine("private static PixelShader _pixelShader = new PixelShader();");
            _effectOut.WriteLineNoTabs("");

            _effectOut.WriteLine("static {0}()", this.EffectName);
            _effectOut.WriteLine("{");
            _effectOut.Indent++;
            _effectOut.WriteLine("StringBuilder uriString = new StringBuilder();");
            _effectOut.WriteLineNoTabs("#if !SILVERLIGHT");
            _effectOut.WriteLine("uriString.Append(\"pack://application:,,,\");");
            _effectOut.WriteLineNoTabs("#endif");
            _effectOut.WriteLine("uriString.Append(\"/{0};component/{1}.ps\");", 
                this.EffectNamespace, 
                this.EffectName);
            _effectOut.WriteLine("_pixelShader.UriSource = new Uri(uriString.ToString(), UriKind.RelativeOrAbsolute);");
            _effectOut.Indent--;
            _effectOut.WriteLine("}");
            _effectOut.WriteLineNoTabs("");

            _effectOut.WriteLine("public {0}()", this.EffectName);
            _effectOut.WriteLine("{");
            _effectOut.Indent++;
            _effectOut.WriteLine("this.PixelShader = _pixelShader;");
            foreach (FilterBase filter in _filters)
            {
                _effectOut.WriteLine("UpdateShaderValue(Input1Property);");
                foreach (EffectSampler sampler in filter.Samplers)
                {
                    _effectOut.WriteLine("UpdateShaderValue({0}Property);", sampler.FullName);
                }
                foreach (EffectProperty property in filter.Properties)
                {
                    _effectOut.WriteLine("UpdateShaderValue({0}Property);", property.FullName);
                }
            }
            _effectOut.Indent--;
            _effectOut.WriteLine("}");
            _effectOut.WriteLineNoTabs("");

            _effectOut.WriteLine("///<summary>");
            _effectOut.WriteLine("///Gets or sets explicitly the main input sampler.");
            _effectOut.WriteLine("///</summary>");
            _effectOut.WriteLineNoTabs("#if !SILVERLIGHT");
            _effectOut.WriteLine("[System.ComponentModel.BrowsableAttribute(false)]");
            _effectOut.WriteLineNoTabs("#endif");
            _effectOut.WriteLine("public Brush Input1");
            _effectOut.WriteLine("{");
            _effectOut.Indent++;
            _effectOut.WriteLine("get { return (Brush)GetValue(Input1Property); }");
            _effectOut.WriteLine("set { SetValue(Input1Property, value); }");
            _effectOut.Indent--;
            _effectOut.WriteLine("}");
            _effectOut.WriteLineNoTabs("");

            _effectOut.WriteLine("///<summary>");
            _effectOut.WriteLine("///Identifies the Input1 dependency property.");
            _effectOut.WriteLine("///</summary>");
            _effectOut.WriteLine("public static readonly DependencyProperty Input1Property = ShaderEffect.RegisterPixelShaderSamplerProperty(\"Input1\", typeof({0}), 0);", 
                this.EffectName);
            _effectOut.WriteLineNoTabs("");

            int samplerNumber = 1;
            foreach (FilterBase filter in _filters)
            {
                foreach (EffectSampler sampler in filter.Samplers)
                {
                    if (!String.IsNullOrEmpty(sampler.Comment))
                    {
                        _effectOut.WriteLine("///<summary>");
                        _effectOut.WriteLine(
                            "///{0}",
                            sampler.Comment);
                        _effectOut.WriteLine("///</summary>");
                    }

                    _effectOut.WriteLine("public Brush {0}",
                        sampler.FullName);
                    _effectOut.WriteLine("{");
                    _effectOut.Indent++;
                    _effectOut.WriteLine("get {{ return (Brush)GetValue({0}Property); }}",
                        sampler.FullName);
                    _effectOut.WriteLine("set {{ SetValue({0}Property, value); }}",
                        sampler.FullName);
                    _effectOut.Indent--;
                    _effectOut.WriteLine("}");
                    _effectOut.WriteLineNoTabs("");

                    _effectOut.WriteLine("///<summary>");
                    _effectOut.WriteLine("///Identifies the {0} dependency property.",
                        sampler.FullName);
                    _effectOut.WriteLine("///</summary>");
                    _effectOut.WriteLine("public static readonly DependencyProperty {0}Property = ShaderEffect.RegisterPixelShaderSamplerProperty(\"{0}\", typeof({1}), {2});",
                        sampler.FullName,
                        this.EffectName,
                        samplerNumber);
                    _effectOut.WriteLineNoTabs("");

                    samplerNumber++;
                    CheckSamplerCount(samplerNumber);
                }
            }

            int propertyNumber = 0;
            foreach (FilterBase filter in _filters)
            {
                foreach (EffectProperty property in filter.Properties)
                {
                    if (!String.IsNullOrEmpty(property.Comment))
                    {
                        _effectOut.WriteLine("///<summary>");
                        _effectOut.WriteLine(
                            "///{0}",
                            property.Comment);
                        _effectOut.WriteLine("///</summary>");
                    }
                    _effectOut.WriteLine("public {0} {1}",
                        property.WrapperType,
                        property.FullName);
                    _effectOut.WriteLine("{");
                    _effectOut.Indent++;
                    _effectOut.WriteLine("get {{ return ({0})GetValue({1}Property); }}",
                        property.WrapperType,
                        property.FullName);
                    _effectOut.WriteLine("set {{ SetValue({0}Property, value); }}",
                        property.FullName);
                    _effectOut.Indent--;
                    _effectOut.WriteLine("}");
                    _effectOut.WriteLineNoTabs("");

                    _effectOut.WriteLine("///<summary>");
                    _effectOut.WriteLine("///Identifies the {0} dependency property.",
                        property.FullName);
                    _effectOut.WriteLine("///</summary>");
                    _effectOut.WriteLine("public static readonly DependencyProperty {0}Property = DependencyProperty.Register(\"{0}\", typeof({1}), typeof({2}), new UIPropertyMetadata({3}, PixelShaderConstantCallback({4})));",
                        property.FullName,
                        property.WrapperType,
                        this.EffectName,
                        property.DefaultValue,
                        propertyNumber);
                    _effectOut.WriteLineNoTabs("");
                    propertyNumber++;
                    CheckPropertyCount(propertyNumber);
                }
            }

            _effectOut.Indent--;
            _effectOut.WriteLine("}");

            _effectOut.Indent--;
            _effectOut.Write("}");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        private static void CheckPropertyCount(int number)
        {
            if (number > 32)
            {
                // see http://msdn.microsoft.com/en-us/library/bb172918(VS.85).aspx
                throw new IndexOutOfRangeException("Property count must be <= 32.");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        private static void CheckSamplerCount(int number)
        {
            if (number > 15)
            {
                // see http://msdn.microsoft.com/en-us/library/bb172918(VS.85).aspx
                throw new IndexOutOfRangeException("Sampler count must be <= 16.");
            }
        }

        /// <summary>
        /// Generates the code.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)")]
        public void Execute()
        {
            System.IO.Directory.CreateDirectory(EffectDirectory);
            string fullName = System.IO.Path.Combine(EffectDirectory, EffectName);
            using (StreamWriter swShader = new StreamWriter(String.Format("{0}.fx", fullName)))
            {
                _shaderOut = new IndentedTextWriter(swShader);
                GenerateHlsl();
            }
            using (StreamWriter swEffect = new StreamWriter(String.Format("{0}.cs", fullName)))
            {
                _effectOut = new IndentedTextWriter(swEffect);
                GenerateCs();
            }
        }
    }
}