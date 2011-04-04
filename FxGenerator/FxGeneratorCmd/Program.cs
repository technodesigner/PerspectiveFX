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
using System.Threading;
using Microsoft.Build.Logging;
using System.Collections.Generic;
using Microsoft.Build.Framework;

namespace FxGeneratorCmd
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
                    Console.WriteLine("  - fichier : fichier XAML contenant un objet EffectBuilderCollection et ses éléments enfants.");
                    Console.WriteLine("  - /debug : génère un assembly en mode Debug (mode Release par défaut)");
                    Console.WriteLine("  - /wpf:1 : génère un assembly WPF (valeur par défaut wpf:1)");
                    Console.WriteLine("  - /sl:1 : génère un assembly Silverlight (valeur par défaut sl:0)");
                    Console.WriteLine("");
                    Console.WriteLine(@"Exemple : FxGeneratorCmd Fxtest.xaml /debug /wpf:0 /sl:1");
                    Console.WriteLine("");
                    Console.WriteLine("Dernière version et informations de licence : voir http://www.codeplex.com/perspectivefx");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine("Generates an effect assembly for WPF and/or Silverlight");
                    Console.WriteLine("Arguments : filename [/debug] [/wpf:0/1] [/sl:0/1]");
                    Console.WriteLine("  - filename : XAML file containing an EffectBuilderCollection object and its children.");
                    Console.WriteLine("  - /debug : generates an assembly in Debug mode (default mode is Release)");
                    Console.WriteLine("  - /wpf:1 : generates a WPF assembly (default value wpf:1)");
                    Console.WriteLine("  - /sl:1 : generates a Silverlight assembly (default value sl:0)");
                    Console.WriteLine("");
                    Console.WriteLine(@"Exemple : FxGeneratorCmd Fxtest.xaml /debug /wpf:0 /sl:1");
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

            ConsoleLogger logger = new ConsoleLogger();
            List<ILogger> loggers = new List<ILogger>();
            loggers.Add(logger);
            Perspective.PixelShader.Build.EffectLibraryBuilder.BuildFromXaml(filename, wpf, sl, debug, loggers);
            // Console.ReadLine();
        }
    }
}
