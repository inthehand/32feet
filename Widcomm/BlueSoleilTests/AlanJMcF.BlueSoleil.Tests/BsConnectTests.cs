using System;
using System.Net.Sockets;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.BlueSoleil;
using InTheHand.Net.Tests.Infra;
using NMock2;
using NUnit.Framework;
using InTheHand.Net.Bluetooth.Factory;
using System.IO;

namespace InTheHand.Net.Tests.BlueSoleil
{
    [TestFixture]
    public class BsConnectTests
    {
        readonly BluetoothAddress AddrA = BluetoothAddress.Parse("002233445566");
        readonly byte[] AddrABytes = new byte[] { 0x66, 0x55, 0x44, 0x33, 0x22, 0x00, };

        //--------
        [TearDown]
        public void WaitForFinalizers()
        {
            GC.Collect();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        //--------
        private static void Create_BtCli(out Mockery mkry, out IBluesoleilApi api,
            out BluesoleilFactory fcty, out IBluetoothClient cli)
        {
            mkry = new Mockery();
            api = mkry.NewMock<IBluesoleilApi>();
            //
            Expect.Exactly(2).On(api).Method("Btsdk_IsSDKInitialized")
                .WithAnyArguments()
                .Will(Return.Value(false));
            Expect.Once.On(api).Method("Btsdk_IsServerConnected")
                .WithAnyArguments()
                .Will(Return.Value(false));
            //
            Expect.AtLeastOnce.On(api).Method("Btsdk_Init")
                .WithAnyArguments()
                .Will(Return.Value(BtSdkError.OK));
            //
            //Expect.AtLeastOnce.On(api).Method("Btsdk_IsBluetoothReady")
            //    .WithAnyArguments()
            //    .Will(Return.Value(true));
            Expect.Once.On(api).Method("Btsdk_IsServerConnected")
                .WithAnyArguments()
                .Will(Return.Value(true));
            Expect.Once.On(api).Method("Btsdk_IsBluetoothHardwareExisted")
                .WithAnyArguments()
                .Will(Return.Value(true));
            Expect.AtLeastOnce.On(api).Method("Btsdk_IsSDKInitialized")
                .WithAnyArguments()
                .Will(Return.Value(true));
            Expect.AtLeastOnce.On(api).Method("Btsdk_IsServerConnected")
                .WithAnyArguments()
                .Will(Return.Value(true));
            //
            Expect.Once.On(api).Method("Btsdk_GetLocalDeviceAddress")
                .WithAnyArguments()
                .Will(Return.Value(BtSdkError.OK));
            Expect.Once.On(api).Method("Btsdk_GetLocalName")
                .WithAnyArguments()
                .Will(Return.Value(BtSdkError.OK));
            Expect.Once.On(api).Method("Btsdk_GetLocalDeviceClass")
                .With(Is.Out)
                .Will(Return.Value(BtSdkError.OK),
                    new NMock2.Actions.SetIndexedParameterAction(0, (uint)0));
            Expect.Once.On(api).Method("Btsdk_GetLocalLMPInfo")
                .WithAnyArguments()
                .Will(Return.Value(BtSdkError.OK));
            //
            fcty = new BluesoleilFactory(api);
            mkry.VerifyAllExpectationsHaveBeenMet();
            cli = fcty.DoGetBluetoothClient();
            mkry.VerifyAllExpectationsHaveBeenMet();
        }

        void ExpectConnectSetup(IBluesoleilApi api)
        {
            Expect.Exactly(4).On(api).Method("Btsdk_RegisterCallback4ThirdParty")
                .WithAnyArguments()
                .Will(Return.Value(BtSdkError.OK));
            Expect.Once.On(api).Method("Btsdk_RegisterGetStatusInfoCB4ThirdParty")
                .WithAnyArguments()
                .Will(Return.Value(BtSdkError.OK));
            Expect.Once.On(api).Method("Btsdk_SetStatusInfoFlag")
                .WithAnyArguments()
                .Will(Return.Value(BtSdkError.OK));
        }

        private void ExpectGetDeviceInfo(IBluesoleilApi api, UInt32 hDev)
        {
            Expect.Once.On(api).Method("Btsdk_GetRemoteDeviceHandle")
                .With(ArrayMatcher.MatchContentExactly(AddrABytes))
                .Will(Return.Value(hDev));
            Expect.Once.On(api).Method("Btsdk_GetRemoteDeviceAddress")
                .WithAnyArguments()
                .Will(Return.Value(BtSdkError.OK),
                    FillArrayIndexedParameterAction.Fill(1, AddrABytes, true)
                    );
            Expect.Once.On(api).Method("Btsdk_GetRemoteDeviceClass")
                .WithAnyArguments()
                .Will(Return.Value(BtSdkError.NOTFOUND), //??what code
                    new NMock2.Actions.SetIndexedParameterAction(1, (uint)0)
                    );
        }

        //--------
        [Test]
        public void OldWayComPortNoNo()
        {
            Mockery mkry;
            IBluesoleilApi api;
            BluesoleilFactory fcty;
            IBluetoothClient cli;
            Create_BtCli(out mkry, out api, out fcty, out cli);
            //
            ExpectConnectSetup(api);
            //
            UInt32 hDev = 0x123;
            ExpectGetDeviceInfo(api, hDev);
            //--
            UInt32 hConn = 0x456;
            Expect.Once.On(api).Method("Btsdk_ConnectAppExtSPPService")
                //(hDev, ref sppAttr, out hConn)
                .WithAnyArguments()
                .Will(Return.Value(BtSdkError.OK),
                    new NMock2.Actions.SetIndexedParameterAction(2, hConn)
                // ... set fields in struct ...
                    );
            short debugCOM = 0;
            Expect.Once.On(api).Method("Btsdk_GetClientPort")
                .WithAnyArguments()
                .Will(Return.Value(debugCOM));
            // Clean-up
            Expect.Once.On(api).Method("Btsdk_Disconnect")
                .WithAnyArguments()
                .Will(Return.Value(BtSdkError.OK));
            //TODO...HandleConnectionEventInd(hConn, StackConsts.ConnectionEventType.DISC_CFM, IntPtr.Zero)
            //
            try {
                cli.Connect(new BluetoothEndPoint(AddrA, BluetoothService.Wap));
                Assert.Fail("should have thrown!");
            } catch (SocketException) {
            }
            WaitForFinalizers();
            mkry.VerifyAllExpectationsHaveBeenMet();
            //
            HackVerifyShutdown(mkry, api, fcty);
        }

        [Test]
        public void OldWayComPortNoYes()
        {
            Mockery mkry;
            IBluesoleilApi api;
            BluesoleilFactory fcty;
            IBluetoothClient cli;
            Create_BtCli(out mkry, out api, out fcty, out cli);
            //
            ExpectConnectSetup(api);
            //
            UInt32 hDev = 0x123;
            ExpectGetDeviceInfo(api, hDev);
            //--
            UInt32 hConn = 0x456;
            Expect.Once.On(api).Method("Btsdk_ConnectAppExtSPPService")
                //(hDev, ref sppAttr, out hConn)
                .WithAnyArguments()
                .Will(Return.Value(BtSdkError.OK),
                    new NMock2.Actions.SetIndexedParameterAction(2, hConn)
                // ... set fields in struct ...
                    );
            short getCOM = 13;
            Expect.Once.On(api).Method("Btsdk_GetClientPort")
                .WithAnyArguments()
                .Will(Return.Value(getCOM));
            //TODO...HandleConnectionEventInd(hConn, StackConsts.ConnectionEventType.DISC_CFM, IntPtr.Zero)
            //
            cli.Connect(new BluetoothEndPoint(AddrA, BluetoothService.Wap));
            //
            var conn = cli.GetStream();
            mkry.VerifyAllExpectationsHaveBeenMet();
            //
            Expect.Once.On(api).Method("Btsdk_Disconnect")
                .WithAnyArguments()
                .Will(Return.Value(BtSdkError.OK));
            conn.Close();
            mkry.VerifyAllExpectationsHaveBeenMet();
            //
            WaitForFinalizers();
            mkry.VerifyAllExpectationsHaveBeenMet();
            //
            HackVerifyShutdown(mkry, api, fcty);
        }

        void HackVerifyShutdown(Mockery mkry, IBluesoleilApi api, BluesoleilFactory fcty)
        {
            GC.KeepAlive(fcty);
            Expect.Once.On(api).Method("Btsdk_Disconnect")
                .WithAnyArguments()
                .Will(Return.Value(BtSdkError.OK));
#if true
            Expect.AtLeast(0).On(api).Method("Btsdk_Done")
                .WithAnyArguments()
                .Will(Return.Value(BtSdkError.OK));
            //
#else
            fcty = null;
            cli = null;
            Expect.Once.On(api).Method("Btsdk_Done")
                .WithAnyArguments()
                .Will(Return.Value(BtSdkError.OK));
            WaitForFinalizers();
            WaitForFinalizers();
            mkry.VerifyAllExpectationsHaveBeenMet();
#endif
        }

        [Test]
        [Explicit]
        public void NewWay_ButClearlyNoComPortAccessibleForSerialPort()
        {
            bool doMockSerialPort = false;
            Mockery mkry;
            IBluesoleilApi api;
            BluesoleilFactory fcty;
            IBluetoothClient cli;
            UInt32 comSerialNum;
            byte comPort8;
            ExpectNewWay(doMockSerialPort, out mkry, out api, out fcty, out cli, out comSerialNum, out comPort8);
            // Clean-up after failure
            ExpectComCleanUp(api, comSerialNum, comPort8);
            //
            try {
                cli.Connect(new BluetoothEndPoint(AddrA, BluetoothService.SerialPort));
                Assert.Fail("should hae thrown!");
            } catch (IOException) {
            }
            WaitForFinalizers();
            mkry.VerifyAllExpectationsHaveBeenMet();
            //
            HackVerifyShutdown(mkry, api, fcty);
        }

        private static void ExpectComCleanUp(IBluesoleilApi api, UInt32 comSerialNum, byte comPort8)
        {
            Expect.Once.On(api).Method("Btsdk_DeinitCommObj")
                .With(comPort8)
                .Will(Return.Value(BtSdkError.OK));
            Expect.Once.On(api).Method("Btsdk_PlugOutVComm")
                .With(comSerialNum, StackConsts.COMM_SET.Record);
        }

        [Test]
        [Explicit]
        public void NewWay__OpensOk()
        {
            bool doMockSerialPort = true;
            Mockery mkry;
            IBluesoleilApi api;
            BluesoleilFactory fcty;
            IBluetoothClient cli;
            UInt32 comSerialNum;
            byte comPort8;
            ExpectNewWay(doMockSerialPort, out mkry, out api, out fcty, out cli, out comSerialNum, out comPort8);
            //
            cli.Connect(new BluetoothEndPoint(AddrA, BluetoothService.SerialPort));
            mkry.VerifyAllExpectationsHaveBeenMet();
            //
            HackVerifyShutdown(mkry, api, fcty);
        }

        private void ExpectNewWay(bool doMockSerialPort, out Mockery mkry, out IBluesoleilApi api, out BluesoleilFactory fcty, out IBluetoothClient cli, out UInt32 comSerialNum, out byte comPort8)
        {
            Create_BtCli(out mkry, out api, out fcty, out cli);
            if (doMockSerialPort) {
                ((BluesoleilClient)cli).CreateSerialPortMethod = delegate() {
                    //var sp = mkry.NewMock<ISerialPortWrapper>();
                    var sp = new InTheHand.Net.Tests.BlueSoleil.TestSerialPortWrapper();
                    return sp;
                };
            }
            //
            ExpectConnectSetup(api);
            //
            UInt32 hDev = 0x123;
            ExpectGetDeviceInfo(api, hDev);
            //--
            comSerialNum = 0x1000;
            UInt32 comPort = 250;
            comPort8 = checked((byte)comPort);
            UInt32 hConn = 0x1000000;
            //
            Expect.Once.On(api).Method("Btsdk_GetASerialNum")
                .WithNoArguments()
                .Will(Return.Value(comSerialNum));
            Expect.Once.On(api).Method("Btsdk_PlugInVComm")
                .WithAnyArguments()
                .Will(Return.Value(true),
                    new NMock2.Actions.SetIndexedParameterAction(1, comPort));
            Expect.Once.On(api).Method("Btsdk_InitCommObj")
                .WithAnyArguments()
                .Will(Return.Value(BtSdkError.OK));
            Expect.Once.On(api).Method("Btsdk_ConnectEx")
                .WithAnyArguments()
                .Will(Return.Value(BtSdkError.OK),
                    new NMock2.Actions.SetIndexedParameterAction(3, hConn)
                    );
        }

    }//class
}
