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
using Microsoft.Build.BuildEngine;
using System.Reflection;
using Perspective.PixelShader.Filters.WpfFx;

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
        public static void BuildFromXaml(string filename, bool wpf, bool sl, bool debug)
        {
            // To enforce the reference on Perspective.PixelShader.Filters.WpfFx
            // for further XAML reading
            Bloom bloom = null;

            StreamReader sr = new StreamReader(filename);
            try
            {
                EffectBuilderCollection builders = (EffectBuilderCollection)XamlReader.Load(sr.BaseStream);
                BuildFromBuilders(builders, wpf, sl, debug);
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
        public static void BuildFromBuilders(EffectBuilderCollection builders, bool wpf, bool sl, bool debug)
        {
            foreach (EffectBuilder builder in builders)
            {
                builder.Execute();
            }
            if (wpf)
            {
                Compile(builders, TargetFramework.Wpf, debug);
            }
            if (sl)
            {
                Compile(builders, TargetFramework.Silverlight, debug);
            }
        }

        private static void Compile(
            EffectBuilderCollection builders,
            TargetFramework target,
            bool debug)
        {
            string assemblyName = builders.EffectNamespace;
            string oldDir = Directory.GetCurrentDirectory();
            try
            {
                // change of current directory
                // for getting an exact match betwwen source path and .ps resource path
                Directory.SetCurrentDirectory(builders.EffectDirectory);

                Engine engine = new Engine();
                engine.DefaultToolsVersion = "3.5";

                Project project = engine.CreateNewProject();
                project.DefaultTargets = "Build";
                project.AddNewUsingTaskFromAssemblyName("ShaderBuildTask.PixelShaderCompile", "ShaderBuildTask, Version=1.0.3072.18169, Culture=neutral, PublicKeyToken=44e467d1687af125");

                BuildPropertyGroup propertyGroup = project.AddNewPropertyGroup(false);
                propertyGroup.AddNewProperty("ProductVersion", "9.0.30729");
                propertyGroup.AddNewProperty("TargetFrameworkVersion", "v3.5");
                propertyGroup.AddNewProperty("SchemaVersion", "2.0");
                propertyGroup.AddNewProperty("ProjectGuid", Guid.NewGuid().ToString());
                propertyGroup.AddNewProperty("OutputType", "Library");
                propertyGroup.AddNewProperty("RootNamespace", assemblyName);
                propertyGroup.AddNewProperty("AssemblyName", assemblyName);
                propertyGroup.AddNewProperty("WarningLevel", "4");
                propertyGroup.AddNewProperty("ErrorReport", "prompt");

                string targetFolder = target == TargetFramework.Wpf ? @"\WPF" : @"\Silverlight";
                string configFolder = debug ? @"\Debug" : @"\Release";
                propertyGroup.AddNewProperty("OutputPath", String.Format("bin{0}{1}", targetFolder, configFolder));

                BuildPropertyGroup configPropertyGroup = project.AddNewPropertyGroup(false);
                if (debug)
                {
                    configPropertyGroup.AddNewProperty("DebugSymbols", "true");
                    configPropertyGroup.AddNewProperty("DebugType", "full");
                    configPropertyGroup.AddNewProperty("Optimize", "false");
                    if (target == TargetFramework.Wpf)
                    {
                        configPropertyGroup.AddNewProperty("DefineConstants", "DEBUG;TRACE");
                    }
                    if (target == TargetFramework.Silverlight)
                    {
                        configPropertyGroup.AddNewProperty("DefineConstants", "DEBUG;TRACE;SILVERLIGHT");
                    }
                }
                else
                {
                    configPropertyGroup.AddNewProperty("DebugType", "pdbonly");
                    configPropertyGroup.AddNewProperty("Optimize", "true");
                    if (target == TargetFramework.Wpf)
                    {
                        configPropertyGroup.AddNewProperty("DefineConstants", "TRACE");
                    }
                    if (target == TargetFramework.Silverlight)
                    {
                        configPropertyGroup.AddNewProperty("DefineConstants", "TRACE;SILVERLIGHT");
                    }
                }

                if (target == TargetFramework.Silverlight)
                {
                    BuildPropertyGroup slPropertyGroup = project.AddNewPropertyGroup(false);
                    slPropertyGroup.AddNewProperty("SilverlightApplication", "false");
                    slPropertyGroup.AddNewProperty("NoStdLib", "true");
                    slPropertyGroup.AddNewProperty("NoConfig", "true");

                    BuildItemGroup slItemGroup = project.AddNewItemGroup();
                    slItemGroup.AddNewItem("Reference", "mscorlib");
                    slItemGroup.AddNewItem("Reference", "System");
                    slItemGroup.AddNewItem("Reference", "System.Core");
                    slItemGroup.AddNewItem("Reference", "System.Windows");
                }
                if (target == TargetFramework.Wpf)
                {
                    BuildItemGroup itemGroup = project.AddNewItemGroup();
                    itemGroup.AddNewItem("Reference", "System");
                    BuildItem item = itemGroup.AddNewItem("Reference", "System.Core");
                    item.SetMetadata("RequiredTargetFramework", "3.5");
                    BuildItem item2 = itemGroup.AddNewItem("Reference", "WindowsBase");
                    item2.SetMetadata("RequiredTargetFramework", "3.5");
                    BuildItem item3 = itemGroup.AddNewItem("Reference", "PresentationCore");
                    item3.SetMetadata("RequiredTargetFramework", "3.5");
                    BuildItem item4 = itemGroup.AddNewItem("Reference", "PresentationFramework");
                    item4.SetMetadata("RequiredTargetFramework", "3.5");
                }

                BuildItemGroup itemGroup2 = project.AddNewItemGroup();
                BuildItemGroup itemGroup3 = project.AddNewItemGroup();
                foreach (EffectBuilder builder in builders)
                {
                    itemGroup2.AddNewItem("Compile", builder.EffectName + ".cs");
                    itemGroup3.AddNewItem("Effect", builder.EffectName + ".fx");
                }

                if (target == TargetFramework.Silverlight)
                {
                    project.AddNewImport(@"$(MSBuildExtensionsPath)\Microsoft\Silverlight\v3.0\Microsoft.Silverlight.CSharp.targets", null);
                }
                if (target == TargetFramework.Wpf)
                {
                    project.AddNewImport(@"$(MSBuildToolsPath)\Microsoft.CSharp.targets", null);
                }

                Target effectTarget = project.Targets.AddNewTarget("EffectCompile");
                effectTarget.Condition = "'@(Effect)' != ''";

                BuildTask task = effectTarget.AddNewTask("PixelShaderCompile");
                task.SetParameterValue("Sources", "@(Effect)");
                task.AddOutputItem("Outputs", "Resource");

                BuildPropertyGroup propertyGroup2 = project.AddNewPropertyGroup(true);
                propertyGroup2.AddNewProperty("PrepareResourcesDependsOn", "EffectCompile;$(PrepareResourcesDependsOn)");

                // string s = project.Xml;

                ConsoleLogger logger = new ConsoleLogger();
                engine.RegisterLogger(logger);
                engine.BuildProject(project);
                engine.UnregisterAllLoggers();
            }
            finally
            {
                Directory.SetCurrentDirectory(oldDir);
            }
        }
    }
}
