#if BLUETOPIA
using System;
using InTheHand.Net.Bluetooth.StonestreetOne;
using InTheHand.Net.Bluetooth.Factory;
using NUnit.Framework;
using System.Threading;
using System.Diagnostics;
using NMock2;
using System.Net.Sockets;
using InTheHand.Net.Bluetooth;
using System.Collections.Generic;

namespace InTheHand.Net.Tests.BluetopiaTests
{
    class ClientTestingBluetopia : InTheHand.Net.Tests.Infra.ClientTesting
    {
        //--------
        internal static void RaiseSppEvent(StuffClientBluetopia stuff, Structs.SPP_Event_Data eventData)
        {
            var done = new ManualResetEvent(false);
            Exception error = null;
            try {
                ThreadPool.QueueUserWorkItem(delegate {
                    try {
                        stuff.DutConn.HandleSPP_Event_Callback(stuff.StackId,
                            ref eventData, 0);
                    } catch (Exception ex) {
                        error = ex;
                    } finally {
                        done.Set();
                    }
                });
            } finally {
                var signalled = done.WaitOne(10 * 1000);
                //done.Close();
                Debug.Assert(signalled, "NOT done.signalled");
            }
            if (error != null)
                throw new System.Reflection.TargetInvocationException(error);
        }

        internal static void RaiseSdpEvent(StuffSdpQueryBluetopia stuff, IntPtr pSDP_Response_Data, uint sdpRequestId)
        {
            var done = new ManualResetEvent(false);
            Exception error = null;
            try {
                ThreadPool.QueueUserWorkItem(delegate {
                    try {
                        stuff.DutSdpQuery.HandleSDP_Response_Callback(stuff.StackId,
                            sdpRequestId,
                            pSDP_Response_Data, 0);
                    } catch (Exception ex) {
                        error = ex;
                    } finally {
                        done.Set();
                    }
                });
            } finally {
                var signalled = done.WaitOne(10 * 1000);
                //done.Close();
                Debug.Assert(signalled, "NOT done.signalled");
            }
            if (error != null)
                throw new System.Reflection.TargetInvocationException(error);
        }

        //--------
        internal static StuffClientBluetopia Open()
        {

            return Open(//true, true, null, StackConsts.SPP_OPEN_PORT_STATUS.Success);
                new Behaviour
                {
                    ToPortNumber = true,
                    EndConnectSuccess = true,
                    SocketError = null,
                    ConnConfStatusCode = StackConsts.SPP_OPEN_PORT_STATUS.Success
                });
        }

        internal static void Close(StuffClientBluetopia stuff)
        {
            Close(stuff, true, true, InTheHand.Net.Bluetooth.StonestreetOne.StackConsts.SPP_OPEN_PORT_STATUS.Success);
        }

        //--
        internal static void ExpectWrite(StuffClientBluetopia stuff, byte[] writeBuf, int returnValue)
        {
            Expect.Once.On(stuff.MockedApi).Method("SPP_Data_Write")
                .With(stuff.StackId, stuff.DutConn.Testing_GetPortId(),
                    checked((ushort)writeBuf.Length), writeBuf)
                .Will(Return.Value(returnValue));
        }

        //--
        internal static void RaiseRemoteClose(StuffClientBluetopia stuff)
        {
            Expect.Once.On(stuff.MockedApi).Method("SPP_Close_Port")
                .With(stuff.StackId, stuff.DutConn.Testing_GetPortId())
                .Will(Return.Value(BluetopiaError.OK));
            using (var ctor = new SppEventCreator()) {
                RaiseSppEvent(stuff,
                    ctor.CreateCloseConfirmation(stuff.DutConn.Testing_GetPortId()));
            }
            Assert_IsConnected(
                IsConnectedState.RemoteCloseAndBeforeAnyIOMethod,
                stuff.DutConn, stuff.DutClient, "after remote close");
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
        }

        internal class Behaviour
        {
            public bool ToPortNumber { get; set; }
            public bool EndConnectSuccess { get; set; }
            public int? SocketError { get; set; }
            public StackConsts.SPP_OPEN_PORT_STATUS ConnConfStatusCode { get; set; }
            public int? SdpMethodResult { get; set; }
            public byte? SdpQueryResultPort { get; set; }
            //public bool RaiseSdpResult { get; set; }
            public bool MockIBtSecurity { get; set; }
        }

        //[Obsolete]
        //internal static StuffClient Open(bool toPortNumber,
        //    bool endConnectSuccess, int? socketError,
        //    StackConsts.SPP_OPEN_PORT_STATUS connConfStatusCode)
        //{
        //    return Open(new Behaviour
        //    {
        //        //toPortNumber = toPortNumber,
        //        EndConnectSuccess = endConnectSuccess,
        //        SocketError = socketError,
        //        ConnConfStatusCode = connConfStatusCode,
        //    });
        //}

        internal static StuffClientBluetopia CreateClient(Behaviour behaviour)
        {
            var stuff = BluetopiaTesting.InitMockery_Client(behaviour);
            //
            var cli = stuff.CreateBluetoothClient();
            var cli2 = (BluetopiaClient)cli;
            var conn = (BluetopiaRfcommStream)cli2.Testing_GetConn();
            stuff.SetDut(cli, conn);
            return stuff;
        }

        internal static StuffClientBluetopia ExpectOpen(StuffClientBluetopia stuff)
        {
            var cli = stuff.DutClient;
            var cli2 = (BluetopiaClient)cli;
            var conn = (BluetopiaRfcommStream)cli2.Testing_GetConn();
            var behaviour = stuff.Behaviour;
            //
            const int hConn = 99;
            //
            Assert_IsConnected(
                IsConnectedState.Closed,
                conn, cli, "AA");
            //
            stuff.AddExpectOpenRemotePort = delegate {
                Expect.Once.On(stuff.MockedApi).Method("SPP_Open_Remote_Port")
                    .With(stuff.StackId, Addr1Long, Port5Uint,
                        (InTheHand.Net.Bluetooth.StonestreetOne.NativeMethods.SPP_Event_Callback)
                            conn.HandleSPP_Event_Callback, (uint)0)
                    .Will(Return.Value(hConn));
            };
            if (behaviour.SdpMethodResult.HasValue) {
                Debug.Assert(!behaviour.ToPortNumber, "NOT !behaviour.ToPortNumber");
                Debug.Assert(behaviour.SdpMethodResult.HasValue, "NOT behaviour.SdpMethodResult.HasValue");
                Expect.Once.On(stuff.MockedApi).Method("SDP_Service_Search_Attribute_Request")
                    .WithAnyArguments()//TODO "SDP_Service_Search_Attribute_Request"
                    .Will(Return.Value(behaviour.SdpMethodResult.Value));
            } else{
                Debug.Assert(behaviour.ToPortNumber, "NOT behaviour.ToPortNumber");
                stuff.AddExpectOpenRemotePort(null, null);
            }
            //
            return stuff;
        }

        internal static StuffClientBluetopia Open(Behaviour behaviour)
        {
            var stuff = CreateClient(behaviour);
            ExpectOpen(stuff);
            return DoOpen(stuff, null);
        }

        internal static StuffClientBluetopia DoOpen(StuffClientBluetopia stuff, Action beforeEndConnect)
        {
            var cli = stuff.DutClient;
            var cli2 = (BluetopiaClient)cli;
            var conn = (BluetopiaRfcommStream)cli2.Testing_GetConn();
            var behaviour = stuff.Behaviour;
            //
            BluetoothEndPoint remote;
            if (behaviour.ToPortNumber) {
                remote = new BluetoothEndPoint(Addr1, BluetoothService.Empty, Port5);
            } else {
                remote = new BluetoothEndPoint(Addr1, BluetoothService.VideoSource);
            }
            //
            bool ourCallbackCalled = false;
            var ourCallback = (AsyncCallback)delegate { ourCallbackCalled = true; };
            //
            var ar = cli.BeginConnect(remote, ourCallback, null);
            Assert_IsConnected(
                IsConnectedState.Closed,
                conn, cli, "BB");
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            //
            Assert_IsConnected(
                IsConnectedState.Closed,
                conn, cli, "CC0");
            if (behaviour.SdpQueryResultPort.HasValue) {
                var listAllocs = new List<IntPtr>();
                IntPtr/*"SDP_Response_Data *"*/ pSdp = BluetopiaSdpParseTests
                    .ProtoDListMake_InSDPResponse_Data(listAllocs, behaviour.SdpQueryResultPort.Value);
                var sdpQuery = cli2.Testing_GetSdpQuery();
                uint SDPRequestID = 0;
                uint CallbackParameter = 0;
                //
                stuff.AddExpectOpenRemotePort(null, null);
                // TODO raise callback on thread pool
                sdpQuery.HandleSDP_Response_Callback(stuff.StackId, SDPRequestID,
                    pSdp, CallbackParameter);
                BluetopiaSdpParseTests.Free(listAllocs);
                Thread.Sleep(2000);//HACK
            }
            //
            Assert_IsConnected(
                IsConnectedState.Closed,
                conn, cli, "CC");
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            var openConfData = new Structs.SPP_Open_Port_Confirmation_Data(
                conn.Testing_GetPortId(), behaviour.ConnConfStatusCode);
            using (var ctor = new SppEventCreator()) {
                Structs.SPP_Event_Data eventData = ctor.CreateOpenConfirmation(
                    conn.Testing_GetPortId(), behaviour.ConnConfStatusCode);
                RaiseSppEvent(stuff, eventData);
            }
            SafeWait(ar); //NEW
            Assert_IsConnected(
                behaviour.EndConnectSuccess ? IsConnectedState.Connected : IsConnectedState.Closed,
                conn, cli, "DD");
            Assert.IsTrue(ar.IsCompleted, "ar.IsCompleted before");
            if (beforeEndConnect != null) beforeEndConnect();
            if (behaviour.EndConnectSuccess) {
                Debug.Assert(!behaviour.SocketError.HasValue, "Behaviour settings: Success BUT errorCode!!");
                cli.EndConnect(ar);
            } else {
                Debug.Assert(behaviour.SocketError.HasValue, "Behaviour settings: not Success BUT NO errorCode!!");
                try {
                    cli.EndConnect(ar);
                    Assert.Fail("should have thrown!");
                } catch (SocketException ex) {
                    //TODO Assert.AreEqual(SocketError.ConnectionRefused, ex.SocketErrorCode, "SocketErrorCode");
                    Assert.AreEqual(behaviour.SocketError ?? 0, ex.ErrorCode, "(Socket)ErrorCode");
                }
            }
            Thread.Sleep(200); // let the async async-callback run
            Assert.IsTrue(ourCallbackCalled, "ourCallbackCalled");
            //
            if (behaviour.EndConnectSuccess) {
                Assert_IsConnected(
                    IsConnectedState.Connected,
                    conn, cli, "DD2");
                BluetoothEndPoint expectedRemote = new BluetoothEndPoint(Addr1, BluetoothService.Empty, Port5);
                Assert.AreEqual(expectedRemote, cli.RemoteEndPoint, "cli.RemoteEndPoint");
            }
            //
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            return stuff;
        }

        internal static void Close(StuffClientBluetopia stuff,
            bool doExplicitDispose,
            bool endConnectSuccess, StackConsts.SPP_OPEN_PORT_STATUS connConfStatusCode)
        {
            //----
            Expect.Once.On(stuff.MockedApi).Method("SPP_Close_Port")
                .With(stuff.StackId, stuff.DutConn.Testing_GetPortId())
                .Will(Return.Value(BluetopiaError.OK));
            var mockery = stuff.Mockery;
            if (doExplicitDispose) {
                stuff.DutClient.Dispose();
                Assert_IsConnected(
                    IsConnectedState.Closed,
                    stuff.DutConn, stuff.DutClient, "EE");
            } else {
                var mockApi = stuff.MockedApi;
                //stuff.DutCli = null;
                //cli2 = null;
                //stuff.DutConn = null;
                stuff = null;
                GC.Collect();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                mockApi = null;
            }
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
        }


    }
}
#endif
