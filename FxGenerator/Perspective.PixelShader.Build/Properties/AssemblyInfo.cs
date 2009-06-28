using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Les informations générales relatives à un assembly dépendent de 
// l'ensemble d'attributs suivant. Changez les valeurs de ces attributs pour modifier les informations
// associées à un assembly.
[assembly: AssemblyTitle("Perspective.PixelShader.Build")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Perspective.PixelShader.Build")]
[assembly: AssemblyCopyright("Copyright ©  2009")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// L'affectation de la valeur false à ComVisible rend les types invisibles dans cet assembly 
// aux composants COM. Si vous devez accéder à un type dans cet assembly à partir de 
// COM, affectez la valeur true à l'attribut ComVisible sur ce type.
[assembly: ComVisible(false)]

// Le GUID suivant est pour l'ID de la typelib si ce projet est exposé à COM
[assembly: Guid("dab39d8b-47e5-4765-bb67-fe498197ad65")]

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
[assembly: AssemblyVersion(Perspective.PixelShader.Build.LibraryInfo.GlobalVersion)]
[assembly: AssemblyFileVersion(Perspective.PixelShader.Build.LibraryInfo.GlobalVersion)]


namespace Perspective.PixelShader.Build
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
        public const string XmlNamespace = Perspective.PixelShader.LibraryInfo.XmlNamespace;

        /// <summary>
        /// Perspective global version number.
        /// </summary>
        public const string GlobalVersion = Perspective.PixelShader.LibraryInfo.GlobalVersion;
    }
}