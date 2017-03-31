32feet.NET - Personal Area Networking for .NET
==============================================

(Version 2.5+)
20th August 2010


Building the library projects without all the other projects
-------------------------------------------------------------

The 32feet.NET library is distributed as a release version containing the library 
assemblies and source-code samples; however the source code to the library is also 
available.  If a feature has been added, or a bug fixed, but no release containing 
that change is available, or if you wish to add a feature yourself, then you can 
download the source code to build the library locally.  The source code is stored 
in our repository at CodePlex, and all changes are made there and the releases are 
made from there.

We aim to maintain what has been called one of the primary rules of open source 
software distribution: that the project must always build.  However, there are 
various dependencies that must be fulfilled to build all of the solution; various 
projects will fail to build otherwise.  This can make it appear difficult to build 
the library at first attempt, but there are simple steps to follow to get a successful 
build.

The simplest solution to build only the library project(s) itself, and good if you're 
not going to be building repeatedly, is simply to right-click on the respective 
project and select Build from the context-menu -- build InTheHand.Net.Personal.FX2 
for desktop and InTheHand.Net.Personal.CF2 for NETCF.  If you want to be able to 
configure the solution for repeated build follow the instructions below.


In brief, to build only the FX2 full-framework library one might need to do some -- 
or all -- of the following, depending on which SDKs you have installed.

Most of these affect the non-core projects within the solution so one could of course 
just as noted above right-click on the FX2 project and select Build, to build it alone instead of 
doing Build_Solution.  Another solution is to never build this Solution itself, but 
instead just add the FX2 project to your own application's solution Add->ExistingProject
and it will build there, then just change the reference in you app to be a project 
reference to it.  In those two cases you should still do #1 below.

1. Disable the Strong-Naming of the library.
    FIX: Comment out the following line in the AssemblyInfo.cs file.
     [assembly: AssemblyKeyFile(...)]
Notes: Affects the Release configuration only.

2. Disable the building of the Samples.
    FIX: Do "Unload Projects in Solution Folder" on the "Samples" folder.
The library reference in the samples is set-up to suit the installed version.  See 
the "Build Samples readme" document for instructions on building the samples against 
the locally built version.

3. Disable the building of the Compact Framework library projects.
    FIX: Do "Unload Project" on the CF1 and CF2 projects, i.e. "InTheHand.Net.Personal.CF2" etc.
This is necessary only if the NETCF SDKs aren't present, i.e. no VS2008 Pro/no VS2005 Std.

4. Disable the building of the testing projects.
    FIX: Do 'unload' on the "ITH.Net.Personal.FX2.Tests" project, and optionally the 
    "Testing" folder too.
The main test project has a dependency on NUnit.

5. Disable the building of the Widcomm native projects.
    FIX: Do "Unload Project" on the three native projects in the Widcomm folder.
They depend on the Widcomm SDK to be present which most people won't have.

Of course, if the dependencies are present for any of the above, then the ‘fix’ 
need not be applied.