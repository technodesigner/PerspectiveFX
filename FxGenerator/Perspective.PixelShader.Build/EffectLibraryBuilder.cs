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
using System.IO;
using System.Windows.Markup;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using System.Reflection;
using Microsoft.Win32;
using Microsoft.Build.Framework;
using System.Collections.Generic;

namespace Perspective.PixelShader.Build
{
    /// <summary>
    /// A class to build an effect library assembly.
    /// </summary>
    public static class EffectLibraryBuilder
    {
        /// <summary>
        /// Builds an effect library assembly from a XAML file description.
        /// </summary>
        /// <param name="filename">XAML file description containing an EffectBuilderCollection object and its children.</param>
        /// <param name="wpf">Indicates if a WPF assembly shall be built.</param>
        /// <param name="sl">Indicates if a Silverlight assembly shall be built.</param>
        /// <param name="debug">Indicates if the compilation shall be in debug mode.</param>
        /// <param name="loggers">Collection of loggers used for build.</param>
        public static void BuildFromXaml(string filename, bool wpf, bool sl, bool debug, IEnumerable<ILogger> loggers)
        {
            StreamReader sr = new StreamReader(filename);
            try
            {
                EffectBuilderCollection builders = (EffectBuilderCollection)XamlReader.Load(sr.BaseStream);
                BuildFromBuilders(builders, wpf, sl, debug, loggers);
            }
            finally
            {
                sr.Close();
            }
        }

        /// <summary>
        /// Builds an effect library assembly from an EffectBuilderCollection object and its children.
        /// </summary>
        /// <param name="builders">An EffectBuilderCollection object.</param>
        /// <param name="wpf">Indicates if a WPF assembly shall be built.</param>
        /// <param name="sl">Indicates if a Silverlight assembly shall be built.</param>
        /// <param name="debug">Indicates if the compilation shall be in debug mode.</param>
        public static void BuildFromBuilders(EffectBuilderCollection builders, bool wpf, bool sl, bool debug, IEnumerable<ILogger> loggers)
        {
            foreach (EffectBuilder builder in builders)
            {
                builder.Execute();
            }
            if (wpf)
            {
                Compile(builders, TargetFramework.Wpf, debug, loggers);
            }
            if (sl)
            {
                Compile(builders, TargetFramework.Silverlight, debug, loggers);
            }
        }

        private static bool RunAs64BitsProcess()
        {
            return IntPtr.Size == 8;
        }

        private static void Compile(
            EffectBuilderCollection builders,
            TargetFramework target,
            bool debug,
            IEnumerable<ILogger> loggers)
        {
            string assemblyName = builders.EffectNamespace;
            string oldDir = Directory.GetCurrentDirectory();
            try
            {
                // change of current directory
                // for getting an exact match betwwen source path and .ps resource path
                Directory.SetCurrentDirectory(builders.EffectDirectory);

                var projectCollection = new ProjectCollection();
                projectCollection.DefaultToolsVersion = "4.0";

                Project project = new Project(projectCollection);

                if (RunAs64BitsProcess() && (target == TargetFramework.Silverlight))
                {
                    // On a 64 bits machine, the 64 version of MsBuild does not find the Silverlight 4 SDK...
                    // Thanks to Gary Hall, http://stackoverflow.com/questions/3001083/msbuild-command-line-error-silverlight-4-sdk-is-not-installed
                    // Here the SDK path is read from the registry
                    RegistryKey registryKey1 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Microsoft SDKs\Silverlight\v4.0\ReferenceAssemblies");
                    try
                    {
                        string slRuntimePath = registryKey1.GetValue("SLRuntimeInstallPath").ToString();
                        var propertyGroup0 = project.Xml.CreatePropertyGroupElement();
                        project.Xml.InsertAfterChild(propertyGroup0, project.Xml.LastChild);
                        propertyGroup0.AddProperty("_FullFrameworkReferenceAssemblyPaths", slRuntimePath);
                        propertyGroup0.AddProperty("TargetFrameworkDirectory", slRuntimePath);
                    }
                    finally
                    {
                        registryKey1.Close();
                    }
                }

                project.SetProperty("DefaultTargets", "Build");
                var usingTaskElement = project.Xml.CreateUsingTaskElement("Perspective.PixelShader.BuildTask.PixelShaderBuildTask", null, "Perspective.PixelShader.BuildTask");
                project.Xml.InsertAfterChild(usingTaskElement, project.Xml.LastChild);

                var propertyGroup = project.Xml.CreatePropertyGroupElement();
                project.Xml.InsertAfterChild(propertyGroup, project.Xml.LastChild);

                propertyGroup.AddProperty("TargetFrameworkVersion", "v4.0");
                propertyGroup.AddProperty("SchemaVersion", "2.0");
                propertyGroup.AddProperty("ProjectGuid", Guid.NewGuid().ToString());
                propertyGroup.AddProperty("OutputType", "Library");
                propertyGroup.AddProperty("RootNamespace", assemblyName);
                propertyGroup.AddProperty("AssemblyName", assemblyName);
                propertyGroup.AddProperty("WarningLevel", "4");
                propertyGroup.AddProperty("ErrorReport", "prompt");
                propertyGroup.AddProperty("GenerateResourceNeverLockTypeAssemblies", "true"); // to prevent MSB4018 error

                string targetFolder = target == TargetFramework.Wpf ? @"\WPF" : @"\Silverlight";
                string configFolder = debug ? @"\Debug" : @"\Release";
                propertyGroup.AddProperty("OutputPath", String.Format("bin{0}{1}", targetFolder, configFolder));

                var configPropertyGroup = project.Xml.CreatePropertyGroupElement();
                project.Xml.InsertAfterChild(configPropertyGroup, project.Xml.LastChild);
                if (debug)
                {
                    configPropertyGroup.AddProperty("DebugSymbols", "true");
                    configPropertyGroup.AddProperty("DebugType", "full");
                    configPropertyGroup.AddProperty("Optimize", "false");
                    if (target == TargetFramework.Wpf)
                    {
                        configPropertyGroup.AddProperty("DefineConstants", "DEBUG;TRACE");
                    }
                    if (target == TargetFramework.Silverlight)
                    {
                        configPropertyGroup.AddProperty("DefineConstants", "DEBUG;TRACE;SILVERLIGHT");
                    }
                }
                else
                {
                    configPropertyGroup.AddProperty("DebugType", "pdbonly");
                    configPropertyGroup.AddProperty("Optimize", "true");
                    if (target == TargetFramework.Wpf)
                    {
                        configPropertyGroup.AddProperty("DefineConstants", "TRACE");
                    }
                    if (target == TargetFramework.Silverlight)
                    {
                        configPropertyGroup.AddProperty("DefineConstants", "TRACE;SILVERLIGHT");
                    }
                }

                if (target == TargetFramework.Silverlight)
                {
                    var slPropertyGroup = project.Xml.CreatePropertyGroupElement();
                    project.Xml.InsertAfterChild(slPropertyGroup, project.Xml.LastChild);
                    slPropertyGroup.AddProperty("SilverlightApplication", "false");
                    slPropertyGroup.AddProperty("NoStdLib", "true");
                    slPropertyGroup.AddProperty("NoConfig", "true");

                    var slItemGroup = project.Xml.CreateItemGroupElement();
                    project.Xml.InsertAfterChild(slItemGroup, project.Xml.LastChild);
                    slItemGroup.AddItem("Reference", "mscorlib");
                    slItemGroup.AddItem("Reference", "System");
                    slItemGroup.AddItem("Reference", "System.Core");
                    slItemGroup.AddItem("Reference", "System.Windows");
                }
                if (target == TargetFramework.Wpf)
                {
                    var itemGroup = project.Xml.CreateItemGroupElement();
                    project.Xml.InsertAfterChild(itemGroup, project.Xml.LastChild);
                    itemGroup.AddItem("Reference", "System");
                    var item = itemGroup.AddItem("Reference", "System.Core");
                    item.AddMetadata("RequiredTargetFramework", "4.0");
                    var item2 = itemGroup.AddItem("Reference", "WindowsBase");
                    item2.AddMetadata("RequiredTargetFramework", "4.0");
                    var item3 = itemGroup.AddItem("Reference", "PresentationCore");
                    item3.AddMetadata("RequiredTargetFramework", "4.0");
                    var item4 = itemGroup.AddItem("Reference", "PresentationFramework");
                    item4.AddMetadata("RequiredTargetFramework", "4.0");
                }

                var itemGroup2 = project.Xml.CreateItemGroupElement();
                project.Xml.InsertAfterChild(itemGroup2, project.Xml.LastChild);
                var itemGroup3 = project.Xml.CreateItemGroupElement();
                project.Xml.InsertAfterChild(itemGroup3, project.Xml.LastChild);
                const string pixelShaderItemEntry = "PixelShader";
                foreach (EffectBuilder builder in builders)
                {
                    itemGroup2.AddItem("Compile", builder.EffectName + ".cs");
                    itemGroup3.AddItem(pixelShaderItemEntry, builder.EffectName + ".fx");
                }

                if (target == TargetFramework.Silverlight)
                {
                    var projectImportElement1 = project.Xml.CreateImportElement(@"$(MSBuildExtensionsPath32)\Microsoft\Silverlight\v4.0\Microsoft.Silverlight.CSharp.targets");
                    project.Xml.InsertAfterChild(projectImportElement1, project.Xml.LastChild);
                }
                if (target == TargetFramework.Wpf)
                {
                    var projectImportElement2 = project.Xml.CreateImportElement(@"$(MSBuildToolsPath)\Microsoft.CSharp.targets");
                    project.Xml.InsertAfterChild(projectImportElement2, project.Xml.LastChild);
                }

                const string buildTargetName = "PixelShaderCompile";

                var buildTarget = project.Xml.CreateTargetElement(buildTargetName);
                project.Xml.InsertAfterChild(buildTarget, project.Xml.LastChild);
                buildTarget.Condition = "'@(" + pixelShaderItemEntry + ")' != ''";

                var buildTask = buildTarget.AddTask("PixelShaderBuildTask");

                buildTask.SetParameter("SourceFiles", "@(" + pixelShaderItemEntry + ")");
                buildTask.AddOutputItem("OutputFiles", "Resource");

                var propertyGroup2 = project.Xml.CreatePropertyGroupElement();
                project.Xml.InsertAfterChild(propertyGroup2, project.Xml.LastChild);
                propertyGroup2.AddProperty("PrepareResourcesDependsOn", buildTargetName + ";$(PrepareResourcesDependsOn)");

                string projectFileName = (target == TargetFramework.Silverlight) ? "FxGenSl.proj" : "FxGenWpf.proj";
                project.Save(projectFileName);
                
                projectCollection.RegisterLoggers(loggers);
                try
                {
                    project.Build();
                }
                finally
                {
                    projectCollection.UnregisterAllLoggers();
                }
            }
            finally
            {
                Directory.SetCurrentDirectory(oldDir);
            }
        }
    }
}
