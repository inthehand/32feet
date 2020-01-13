@echo off
echo -------------------------------------------------------------------
set IthRoot="%~dp0"
echo %IthRoot%
set Root="%~dp0CeTest32feet"
rmdir /q /s %Root%
echo Expect 'file not found':
dir /b %Root%
mkdir %Root%
mkdir %Root%\WM5
mkdir %Root%\PPC2003
echo Copying...
copy %IthRoot%\InTheHand.Net.Personal.CF2\bin\Release\InTheHand.Net.Personal.dll %Root%\
copy %IthRoot%\InTheHand.Net.Personal.CF2\bin\Release\InTheHand.Net.Personal.pdb %Root%\
copy %IthRoot%\InTheHand.Net.Personal\bin\Release\InTheHand.Net.Personal.dll.config %Root%\app.config
rem
copy %IthRoot%"\Widcomm\32feetWidcommWM\Windows Mobile 5.0 Smartphone SDK (ARMV4I)\Release\32feetWidcomm.dll" %Root%\WM5\
copy %IthRoot%"\Widcomm\32feetWidcommWM\Windows Mobile 5.0 Smartphone SDK (ARMV4I)\Release\32feetWidcomm.pdb" %Root%\WM5\
copy %IthRoot%"\Widcomm\32feetWidcommWM\Pocket PC 2003 (ARMV4)\Release\32feetWidcomm.dll" %Root%\PPC2003\
copy %IthRoot%"\Widcomm\32feetWidcommWM\Pocket PC 2003 (ARMV4)\Release\32feetWidcomm.pdb" %Root%\PPC2003\
rem
copy %IthRoot%\Testing\DeviceMenuTesting\bin\Release\DeviceMenuTesting.exe %Root%\
copy %IthRoot%\Testing\DeviceMenuTesting\bin\Release\DeviceMenuTesting.pdb %Root%\
rem
copy %IthRoot%\Samples\BluetoothChat\Smartphone\bin\Release\BluetoothChatSp.exe %Root%\
REM copy %IthRoot%\Samples\BluetoothChat\Smartphone\bin\Release\BluetoothChatSp.pdb %Root%\
rem
copy %IthRoot%"\Samples\BluetoothChat\Pocket PC\bin\Release\BluetoothChat.exe" %Root%\
rem
copy %IthRoot%"\Samples\BluetoothChat2\Chat2Device\bin\Release\Chat2Device.exe" %Root%\
rem
copy %IthRoot%\Samples\BluetoothRemote\bin\Release\BluetoothRemote.exe %Root%\
REM copy %IthRoot%\Samples\BluetoothRemote\bin\Release\BluetoothRemote.pdb %Root%\
rem
copy %IthRoot%\Samples\BluetoothSdp\SdpBrowserDevice\bin\Release\SdpBrowserDevice.exe %Root%\
copy %IthRoot%\Samples\BluetoothSdp\SdpBrowserDevice\bin\Release\SdpBrowserDevice.pdb %Root%\
rem
copy %IthRoot%\Samples\ObjectPush\DeviceListener\bin\Release\DeviceListener.exe %Root%\
copy %IthRoot%\Samples\ObjectPush\DeviceListener\bin\Release\DeviceListener.pdb %Root%\
rem
copy %IthRoot%\Samples\IrDA\IrDAServiceClientCF2\bin\Release\IrDAServiceClientCF2.exe %Root%\
copy %IthRoot%\Samples\IrDA\IrDAServiceClientCF2\bin\Release\IrDAServiceClientCF2.pdb %Root%\
rem
copy %IthRoot%\Samples\ObjectPush\ObexPushVB\bin\Release\ObjectPushVB.exe %Root%\
rem
copy %IthRoot%\Samples\ObjectPush\ObjectPushApplication\bin\Release\ObjectPushApplication.exe %Root%\
rem
rem
rem
mkdir %Root%\Testing
copy %IthRoot%\Testing\UnitTesting\NETCF\NetcfTestRunner\bin\Debug\*.exe   %Root%\Testing\
copy %IthRoot%\Testing\UnitTesting\NETCF\NetcfTestRunner\bin\Debug\*.dll   %Root%\Testing\
rem
rem
rem
mkdir %Root%\Debug
copy %IthRoot%\Testing\DeviceMenuTesting\bin\Debug\DeviceMenuTesting.exe %Root%\Debug
copy %IthRoot%\Testing\DeviceMenuTesting\bin\Debug\DeviceMenuTesting.pdb %Root%\Debug
copy %IthRoot%\InTheHand.Net.Personal.CF2\bin\Debug\InTheHand.Net.Personal.dll %Root%\Debug
copy %IthRoot%\InTheHand.Net.Personal.CF2\bin\Debug\InTheHand.Net.Personal.pdb %Root%\Debug
copy     %IthRoot%\InTheHand.Net.Personal\bin\Debug\InTheHand.Net.Personal.dll.config %Root%\Debug\app.config


echo -------------------------------------------------------------------
rem EOF
