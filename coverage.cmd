rem@echo off
setlocal
set ProjectRoot=%~dp0
set NcoverPath=C:\Program Files\NCover
set NunitPath=D:\Program Files\NUnit-Net-2.0 2.2.8\bin
set TestAssembly=%ProjectRoot%\ITH.Net.Personal.FX2.Tests\bin\Debug\ITH.Net.Personal.FX2.Tests.dll
rem 
pushd "%ProjectRoot%\"
echo To specify one fixture use e.g.:  /fixture=Foo.Bar.FooBarTests
"%NCoverPath%\NCover.Console.exe"  "%NUnitPath%\nunit-console.exe" "%TestAssembly%" %* //a InTheHand.Net.Personal
popd
