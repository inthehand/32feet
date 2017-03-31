
32feet.NET - Personal Area Networking for .NET
==============================================

Version 2.2 (22nd November 2007)


Building the Samples against the library source code
-----------------------------------------------------

* Summary
The samples reference the library with a file reference so that they build without 
changes when a release is installed by end-users.  For developers using CodePlex, 
either live or from an archive copy, to have a particular sample program build using 
the locally build library some local configuation is necessary;
1. Add an entry to the sample's "Reference Paths" list pointing to the library build 
output folder e.g. "C:\...\InTheHand.Net.Personal\InTheHand.Net.Personal\bin\Debug\".
2. Ensure that the library reference in the sample is set to "Specific Version: False" 
(right-click Properties, or F4 on the reference, or double click it in the VB project 
pages).
3. Finally it is best that the in Project Dependencies that the sample is set to have 
a dependency on the library project, e.g. InTheHand.Net.Personal.FX2 for a desktop
sample.


* Detail
When a end-user developer has installed the library release then only the built 
versions of the library are supplied; the source projects for the library not  
being included.  Thus when they build the samples the reference to the library 
assembly needs to be a "file" reference to the installed version of the assemblies 
(and thus use respective AssemblyFolders Registry key to locate it).

However for a developer working on the project in the CodePlex repository, or 
having downloaded a copy of the source from CodePlex, then the references in the 
samples should be "project" references to the respective library project within 
the solution.  This is necessary to use the locally built versions of the library 
if they are modified.  If a new method is added and called from one of the samples 
then this will be apparent with a compiler error, but will be less obvious if only 
the behaviour has changed...

It is not simple to reconcile these two requirements.  The current solution is to 
have all the samples use file references, and for developers using the repository 
source to add a entry to the Reference Paths list to point to the library build 
output folder.  The reference in the sample must also be set to "Specific Version: 
False", which also sets "Strong Name: false".  This is required otherwise the 
locally build version will be ignored, apparently because it doesn't have the 
correct StrongName value.

Note that Reference Paths entries are stored in the <project>.user file, so are not 
stored in the repository.  However the .user files will be maintained and are thus 
a one-off addition if the source is always in the same place.

Finally to ensure that the library build is complete before the sample begins building 
a Project Dependencies should be added stating that dependency.

AlanJMcF 2007-Feb-13.