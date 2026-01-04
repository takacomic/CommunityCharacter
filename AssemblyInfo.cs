using System.Reflection;
using CommunityCharacter;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MelonLoader;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle(ModInfo.Name)]
[assembly: AssemblyDescription(ModInfo.Description)]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(ModInfo.Company)]
[assembly: AssemblyProduct(ModInfo.Name)]
[assembly: AssemblyCopyright("Copyright © " + ModInfo.Author + " 2025")]
[assembly: AssemblyTrademark(ModInfo.Company)]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("c1e617d6-79f6-4e97-a58c-1156e90c8859")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
[assembly: AssemblyVersion(ModInfo.Version)]
[assembly: AssemblyFileVersion(ModInfo.Version)]
// Melon related Assembly
[assembly: MelonInfo(typeof(CommunityCharacterMod), ModInfo.Name, ModInfo.Version, ModInfo.Author, ModInfo.Download)]
[assembly: MelonGame("poncle", "Vampire Survivors")]
[assembly: MelonAdditionalDependencies("SaveDataInvestigator")]