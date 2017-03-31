//#define NO_SIGNING
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyTitle("InTheHand.Net.Personal")]
[assembly: AssemblyDescription("Personal Area Networking for .NET\r\n(" +
#if WinXP
 ".NET Framework"
#elif WinCE
".NET Compact Framework"
#elif ANDROID
"Android"
#else
"unknown!"
#error Unknown platform!
#endif
 + " " +
#if FX4
 "v4.0"
#elif FX3_5
 "v3.5"
#elif V2
 "v2.0"
#elif V1
"v1.0"
#else
"unknown!"
#error Unknown version!
#endif
 + ")")]

[assembly: AssemblyCompany("In The Hand Ltd")]
[assembly: AssemblyProduct("32feet.NET")]
[assembly: AssemblyCopyright("Copyright © In The Hand Ltd 2003-2012")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: Guid("f8b087d0-bc47-48ca-958c-8fc6a41c1b65")]

//
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:

[assembly: AssemblyVersion("3.7.1102.0")]
[assembly: AssemblyInformationalVersion("3.7.1102.0")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]

#if WinXP
[assembly: ComVisible(false)]
#endif

//only sign on non-debug builds
#if !DEBUG && !CODE_ANALYSIS && !NO_SIGNING
[assembly: AssemblyDelaySign(false)]
#pragma warning disable 1699
[assembly: AssemblyKeyFile(@"..\..\..\InTheHand.Net.Personal\InTheHand.snk")]
#pragma warning restore 1699
[assembly: AssemblyKeyName("")]
#endif

#if DEBUG // for unit-tests
#if ! V1
[assembly: InternalsVisibleTo("ITH.Net.Personal.FX2.Tests"
    //+ ", PublicKey=" + "00240000048000009400000006020000002400005253413100040000010001005f213ee92d70ac14e1b91c0903122b4717e49db9adbac893810e364e6a919d3a2d3bb178359c3c3913f31848d2587aa77bdaae8088d4a49f0913cf67b9ad4be815bae8fe8d0911e96fb85d8e376d57cccf931bf73dc648e42d9f1f50f713c637fd4b1c6d0731e1d4831b00f9168073c0c6749c78a1ef7afd2869227ecf4c43a4"
    )]
[assembly: InternalsVisibleTo("ITH.Net.Personal.FX4.Tests")]
[assembly: InternalsVisibleTo("ITH.Net.CE2.Tests")]
[assembly: InternalsVisibleTo("BlueSoleilTests")]
[assembly: InternalsVisibleTo("Mocks")]
[assembly: InternalsVisibleTo("MockObjects")]
[assembly: InternalsVisibleTo("BluetopiaTests")]
#endif
#endif
