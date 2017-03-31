using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SeparateBlueSoleil")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("In The Hand Ltd")]
[assembly: AssemblyProduct("32feet.NET -- SeparateBlueSoleil")]
[assembly: AssemblyCopyright("Copyright © In The Hand Ltd 2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: System.CLSCompliant(true)]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("1c0cc915-7f4b-4954-a443-6bc0763c6eba")]

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
[assembly: AssemblyVersion("3.0.2.0")]
[assembly: AssemblyInformationalVersion("3.0.1008.0")]

#if DEBUG
[assembly: InternalsVisibleTo("BlueSoleilTests")]
[assembly: InternalsVisibleTo("Mocks")]
[assembly: InternalsVisibleTo("MockObjects")]
#endif