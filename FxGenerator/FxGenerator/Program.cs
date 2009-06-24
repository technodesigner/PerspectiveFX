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
using Microsoft.Build.BuildEngine;
using Perspective.PixelShader;
using Perspective.PixelShader.Filters;
using System.IO;
using System.Xml;
using System.Windows.Markup;
using System.Threading;

namespace FxGenerator
{
    class Program
    {
        [STAThread()]
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                if (Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName == "fr")
                {
                    Console.WriteLine("Génère un assembly d'effet pour WPF et/ou Silverlight");
                    Console.WriteLine("Arguments : fichier [/debug] [/wpf:0/1] [/sl:0/1]");
                    Console.WriteLine("  - fichier : fichier XAML de description des effets");
                    Console.WriteLine("  - /debug : génère un assembly en mode Debug (mode Release par défaut)");
                    Console.WriteLine("  - /wpf:1 : génère un assembly WPF (valeur par défaut wpf:1)");
                    Console.WriteLine("  - /sl:1 : génère un assembly Silverlight (valeur par défaut sl:0)");
                    Console.WriteLine("");
                    Console.WriteLine(@"Exemple : FxGenerator Fxtest.xaml /debug /wpf:0 /sl:1");
                    Console.WriteLine("");
                    Console.WriteLine("Dernière version et informations de licence : voir http://www.codeplex.com/perspectivefx");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine("Generates an effect assembly for WPF and/or Silverlight");
                    Console.WriteLine("Arguments : filename [/debug] [/wpf:0/1] [/sl:0/1]");
                    Console.WriteLine("  - filename : effects description XAML file");
                    Console.WriteLine("  - /debug : generates an assembly in Debug mode (default mode is Release)");
                    Console.WriteLine("  - /wpf:1 : generates a WPF assembly (default value wpf:1)");
                    Console.WriteLine("  - /sl:1 : generates a Silverlight assembly (default value sl:0)");
                    Console.WriteLine("");
                    Console.WriteLine(@"Exemple : FxGenerator Fxtest.xaml /debug /wpf:0 /sl:1");
                    Console.WriteLine("");
                    Console.WriteLine("Last release and license informations : see http://www.codeplex.com/perspectivefx");
                    Console.WriteLine("");
                }
                return;
            }
            string filename = args[0];
            
            bool debug = false;
            bool wpf = true;
            bool sl = false;
            for (int i = 1; i < args.Length; i++)
            {
                if (String.Equals(args[i], "/debug",
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    debug = true;
                }
                else if (String.Equals(args[i], "/sl:1",
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    sl = true;
                }
                else if (String.Equals(args[i], "/wpf:0",
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    wpf = false;
                }
            }
            GenerateAndCompile(filename, wpf, sl, debug);
            Console.ReadLine();
        }

        static void GenerateAndCompile(string filename, bool wpf, bool sl, bool debug)
        {
            StreamReader sr = new StreamReader(filename);
            EffectBuilderCollection builders = (EffectBuilderCollection)XamlReader.Load(sr.BaseStream);
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

        static void Compile(
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
