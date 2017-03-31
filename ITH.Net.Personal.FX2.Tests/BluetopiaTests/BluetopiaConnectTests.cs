#if BLUETOPIA
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.StonestreetOne;
using NMock2;
using NUnit.Framework;
using InTheHand.Net.Tests.Infra;

namespace InTheHand.Net.Tests.BluetopiaTests
{
    [TestFixture]
    public class BluetopiaConnectTests
    {

        [Test]
        public void ErrorOnCallOpenRemotePort()
        {
            var stuff = BluetopiaTesting.InitMockery_Client(new ClientTestingBluetopia.Behaviour());
            //
            Expect.Once.On(stuff.MockedApi).Method("SPP_Open_Remote_Port")
                .WithAnyArguments()
                .Will(Return.Value((int)BluetopiaError.INTERNAL_ERROR));
            //
            var cli = stuff.CreateBluetoothClient();
            var remote = new BluetoothEndPoint(ClientTesting.Addr1, BluetoothService.Empty, ClientTesting.Port5);
            try {
                var ar = cli.BeginConnect(remote, null, null);
                cli.EndConnect(ar); // NEW
                Assert.Fail("should have thrown!");
            } catch (BluetopiaSocketException) {
                // TODO SocketError check
            }
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            //
            cli.Dispose();
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void ErrorOpenNoCallback()
        {
            var stuff = BluetopiaTesting.InitMockery_Client(new ClientTestingBluetopia.Behaviour());
            //
            const int hConn = 99;
            //
            var cli = stuff.CreateBluetoothClient();
            var cli2 = (BluetopiaClient)cli;
            var conn = (BluetopiaRfcommStream)cli2.Testing_GetConn();
            //
            Expect.Once.On(stuff.MockedApi).Method("SPP_Open_Remote_Port")
                .With(stuff.StackId, ClientTesting.Addr1Long, ClientTesting.Port5Uint,
                    (InTheHand.Net.Bluetooth.StonestreetOne.NativeMethods.SPP_Event_Callback)
                        conn.HandleSPP_Event_Callback, (uint)0)
                .Will(Return.Value(hConn));
            //
            var remote = new BluetoothEndPoint(ClientTesting.Addr1, BluetoothService.Empty, ClientTesting.Port5);
            var ar = cli.BeginConnect(remote, null, null);
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            ClientTesting.Assert_IsConnected(
                ClientTesting.IsConnectedState.Closed,
                conn, cli, "after");
            Assert.IsFalse(ar.IsCompleted, "ar.IsCompleted after");
            //
            Expect.Once.On(stuff.MockedApi).Method("SPP_Close_Port")
                .With(stuff.StackId, conn.Testing_GetPortId())
                .Will(Return.Value(BluetopiaError.OK));
            cli.Dispose();
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void OpenAndClose_Dispose()
        {
            OpenAndClose_(true);
        }

        [Explicit]
        [Test]
        public void OpenAndClose_Finalize()
        {
            // This fails, perhaps because NMock2 isn't Serialization safe...
            /*
            InTheHand.Net.Tests.BluetopiaTests.BluetopiaConnectTests.OpenAndClose_Finalize : An unhandled System.Runtime.Serialization.SerializationException was thrown while executing this test : Unable to find assembly 'NMock2, Version=2.0.0.44, Culture=neutral, PublicKeyToken=37d3be0adc87c2b7'.
              at ...
              ... ...
              at System.AppDomain.Deserialize(Byte[] blob)
              at System.AppDomain.UnmarshalObject(Byte[] blob)
            ----
            InTheHand.Net.Tests.BluetopiaTests.BluetopiaConnectTests.OpenAndClose_Finalize : NMock2.Internal.ExpectationException : not all expected invocations were performed
            Expected:
              1 time: bluetopiaApi.SPP_Close_Port(equal to <10>, equal to <99>), will return <OK> [called 0 times]
            */
            OpenAndClose_(false);
        }

        void OpenAndClose_(bool doExplicitDispose)
        {
            OpenAndClose_(doExplicitDispose, true, null, StackConsts.SPP_OPEN_PORT_STATUS.Success);
        }

        [Test]
        public void ErrorOpenConnectionRefused__AndClose_Dispose()
        {
            OpenAndClose_(true, false, (int)SocketError.ConnectionRefused, StackConsts.SPP_OPEN_PORT_STATUS.ConnectionRefused);
        }

        [Test]
        public void ErrorOpenTimeout__AndClose_Dispose()
        {
            OpenAndClose_(true, false, (int)SocketError.TimedOut, StackConsts.SPP_OPEN_PORT_STATUS.ConnectionTimeout);
        }

        [Test]
        public void ErrorOpenUnknownError__AndClose_Dispose()
        {
            OpenAndClose_(true, false, (int)SocketError.InvalidArgument, StackConsts.SPP_OPEN_PORT_STATUS.UnknownError);
        }

        [Test]
        public void OpenAndRemoteClose()
        {
            var stuff = OpenAndRemoteClose_();
        }

        //--------
        [Test]
        public void OpenToService_SdpMethodError()
        {
            bool doExplicitDispose = true;
            bool endConnectSuccess = false;
            int? socketError = null;
            StackConsts.SPP_OPEN_PORT_STATUS connConfStatusCode = StackConsts.SPP_OPEN_PORT_STATUS.ConnectionTimeout;
            StuffClientBluetopia stuff = null;
            try {
                stuff = ClientTestingBluetopia.Open(//false, endConnectSuccess, socketError, connConfStatusCode);
                    new ClientTestingBluetopia.Behaviour
                    {
                        ToPortNumber = false,
                        EndConnectSuccess = endConnectSuccess,
                        SocketError = socketError,
                        ConnConfStatusCode = connConfStatusCode,
                        SdpMethodResult = (int)BluetopiaError.SDP_NOT_INITIALIZED
                    });
                Assert.Fail("not implemented yey -- should have thrown!");
            } catch (SocketException) {
                // TODO SocketError check
            }
            if (stuff != null) {
                ClientTestingBluetopia.Close(stuff, doExplicitDispose, endConnectSuccess, connConfStatusCode);
            }
        }

        [Test]
        public void OpenToService()
        {
            bool doExplicitDispose = true;
            StackConsts.SPP_OPEN_PORT_STATUS connConfStatusCode = StackConsts.SPP_OPEN_PORT_STATUS.Success;
            StuffClientBluetopia stuff = null;
            stuff = ClientTestingBluetopia.Open(
                new ClientTestingBluetopia.Behaviour
                {
                    ToPortNumber = false,
                    EndConnectSuccess = true,
                    ConnConfStatusCode = connConfStatusCode,
                    SdpMethodResult = 543,
                    SdpQueryResultPort = 5
                });
            ClientTestingBluetopia.Close(stuff, doExplicitDispose, true, connConfStatusCode);
        }

        //--------
        [Test]
        public void OpenAndRemoteClose_AndRead()
        {
            var stuff = OpenAndRemoteClose_();
            byte[] buf = new byte[10];
            var peer = stuff.DutClient.GetStream();
            int readLen = peer.Read(buf, 0, buf.Length);
            Assert.AreEqual(0, readLen);
            //Assert.Ignore("Apparently we don't go Closed in this case......");
            //Assert_IsConnected(IsConnectedState.Closed, stuff.DutConn, stuff.DutClient, "after remote close");
            ClientTestingBluetopia.Assert_IsConnected(ClientTestingBluetopia.IsConnectedState.RemoteCloseAndBeforeAnyIOMethod,
                stuff.DutConn, stuff.DutClient, "after remote close");
            int readLen2 = peer.Read(buf, 0, buf.Length);
            Assert.AreEqual(0, readLen2);
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void OpenAndRemoteClose_AndBeginReadOverClose()
        {
            var stuff = ClientTestingBluetopia.Open();
            byte[] buf = new byte[10];
            var peer = stuff.DutClient.GetStream();
            var ar = peer.BeginRead(buf, 0, buf.Length, null, null);
            ClientTestingBluetopia.RaiseRemoteClose(stuff);
            int readLen = peer.EndRead(ar);
            Assert.AreEqual(0, readLen);
            //Assert.Ignore("Apparently we don't go Closed in this case......");
            //Assert_IsConnected(IsConnectedState.Closed, stuff.DutConn, stuff.DutClient, "after remote close");
            ClientTestingBluetopia.Assert_IsConnected(ClientTestingBluetopia.IsConnectedState.RemoteCloseAndBeforeAnyIOMethod,
                stuff.DutConn, stuff.DutClient, "after remote close");
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void OpenAndRemoteClose_AndWrite()
        {
            var stuff = OpenAndRemoteClose_();
            byte[] buf = new byte[10];
            var peer = stuff.DutClient.GetStream();
            try {
                peer.Write(buf, 0, buf.Length);
            } catch (IOException) {
                // TODO IOException check
            }
            ClientTestingBluetopia.Assert_IsConnected(ClientTestingBluetopia.IsConnectedState.Closed,
                stuff.DutConn, stuff.DutClient, "after remote close");
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void OpenAndRemoteClose_AndBeginWriteOverClose()
        {
            var stuff = ClientTestingBluetopia.Open();
            byte[] buf = new byte[10];
            var peer = stuff.DutClient.GetStream();
            ClientTestingBluetopia.ExpectWrite(stuff, buf, buf.Length);
            var ar = peer.BeginWrite(buf, 0, buf.Length, null, null);
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            //
            ClientTestingBluetopia.RaiseRemoteClose(stuff);
            try {
                peer.EndWrite(ar);
            } catch (IOException) {
                // TODO IOException check
            }
            ClientTestingBluetopia.Assert_IsConnected(ClientTestingBluetopia.IsConnectedState.RemoteCloseAndBeforeAnyIOMethod,
                stuff.DutConn, stuff.DutClient, "after remote close");
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
        }

        //--------
        private StuffClientBluetopia OpenAndRemoteClose_()
        {
            var stuff = ClientTestingBluetopia.Open(//true, true, null, StackConsts.SPP_OPEN_PORT_STATUS.Success);
                new ClientTestingBluetopia.Behaviour
                {
                    ToPortNumber = true,
                    EndConnectSuccess = true,
                    SocketError = null,
                    ConnConfStatusCode = StackConsts.SPP_OPEN_PORT_STATUS.Success
                });
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            //
            ClientTestingBluetopia.RaiseRemoteClose(stuff);
            return stuff;
        }

        //--------
        internal static void OpenAndClose_(bool doExplicitDispose,
            bool endConnectSuccess, int? socketError, StackConsts.SPP_OPEN_PORT_STATUS connConfStatusCode)
        {
            StuffClientBluetopia stuff = ClientTestingBluetopia.Open(//true, endConnectSuccess, socketError, connConfStatusCode);
                new ClientTestingBluetopia.Behaviour
                {
                    ToPortNumber = true,
                    EndConnectSuccess = endConnectSuccess,
                    SocketError = socketError,
                    ConnConfStatusCode = connConfStatusCode
                });
            ClientTestingBluetopia.Close(stuff, doExplicitDispose, endConnectSuccess, connConfStatusCode);
        }


        //--------
        [Test]
        public void Properties()
        {
            var stuff = BluetopiaTesting.InitMockery_Client(new ClientTestingBluetopia.Behaviour());
            var cli = stuff.CreateBluetoothClient();
            //
            //Assert.IsFalse(cli.Authenticate, "Authenticate");
            //try {
            //    cli.Authenticate = true;
            //    Assert.Fail("should have thrown!");
            //} catch (NotSupportedException) { }
            ////
            //Assert.IsFalse(cli.Encrypt, "Encrypt");
            //try {
            //    cli.Encrypt = true;
            //    Assert.Fail("should have thrown!");
            //} catch (NotSupportedException) { }
            //
            try {
                var s = cli.Client;
                Assert.Fail("should have thrown!");
            } catch (NotSupportedException) { }
            var sock = new System.Net.Sockets.Socket(
                System.Net.Sockets.AddressFamily.InterNetwork,
                System.Net.Sockets.SocketType.Stream,
                System.Net.Sockets.ProtocolType.Unspecified);
            try {
                cli.Client = sock;
                Assert.Fail("should have thrown!");
            } catch (NotSupportedException) {
            } finally {
                sock.Close();
            }
            //
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
        }

    }
}
#endif
