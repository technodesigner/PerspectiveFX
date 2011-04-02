using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Les informations générales relatives à un assembly dépendent de 
// l'ensemble d'attributs suivant. Changez les valeurs de ces attributs pour modifier les informations
// associées à un assembly.
[assembly: AssemblyTitle("Perspective.PixelShader.BuildTask")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("Perspective.PixelShader.BuildTask")]
[assembly: AssemblyCopyright("Copyright © Microsoft 2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// L'affectation de la valeur false à ComVisible rend les types invisibles dans cet assembly 
// aux composants COM. Si vous devez accéder à un type dans cet assembly à partir de 
// COM, affectez la valeur true à l'attribut ComVisible sur ce type.
[assembly: ComVisible(false)]

// Le GUID suivant est pour l'ID de la typelib si ce projet est exposé à COM
[assembly: Guid("bdb04e5f-e067-4078-b2e5-9dc22dc8d090")]

// Les informations de version pour un assembly se composent des quatre valeurs suivantes :
//
//      Version principale
//      Version secondaire 
//      Numéro de build
//      Révision
//
// Vous pouvez spécifier toutes les valeurs ou indiquer les numéros de build et de révision par défaut 
// en utilisant '*', comme indiqué ci-dessous :
// [assembly: AssemblyVersion("1.0.*")]

[assembly: AssemblyVersion(Perspective.PixelShader.BuildTask.LibraryInfo.GlobalVersion)]
[assembly: AssemblyFileVersion(Perspective.PixelShader.BuildTask.LibraryInfo.GlobalVersion)]

namespace Perspective.PixelShader.BuildTask
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
        /// </summary>
        public const string GlobalVersion = "2.1.0.0";

        /// <summary>
        /// Global company name.
        /// </summary>
        public const string Company = "perspectivefx.codeplex.com";

        /// <summary>
        /// Global copyright information.
        /// </summary>
        public const string Copyright = "Copyright © http://perspectivefx.codeplex.com 2009-2011";
    }
}
