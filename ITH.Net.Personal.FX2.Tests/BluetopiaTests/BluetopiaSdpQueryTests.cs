#if BLUETOPIA
using System;
using NUnit.Framework;
using InTheHand.Net.Bluetooth;
using NMock2;
using InTheHand.Net.Bluetooth.StonestreetOne;
using System.Collections.Generic;
using InTheHand.Net.Tests.Infra;
using System.Diagnostics;
using System.Net.Sockets;

namespace InTheHand.Net.Tests.BluetopiaTests
{
    [TestFixture]
    public class BluetopiaSdpQueryTests
    {
        static readonly BluetoothAddress Addr2 = BluetoothAddress.Parse("00:23:45:67:89:ab");
        static readonly Guid SvcClass2 = BluetoothService.VideoConferencingGW;
        const long Addr2Long = 0x0023456789ab;

        [Test]
        public void ErrorHandleReturned()
        {
            var listAllocs = new List<IntPtr>();
            var stuff = BluetopiaSdpParseTests.Create_BluetopiaSdpQuery();
            const int TheirRequestId_Error = (int)BluetopiaError.SDP_INITIALIZATION_ERROR;
            //
            Expect.Once.On(stuff.MockedApi).Method("SDP_Service_Search_Attribute_Request")
                //TODO With(...Is.Anything,...)*/
                /**/.With(stuff.StackId,
                    Addr2Long,
                //Is.Anything,//new byte[6],
                //--
                    (uint)1,
                    Is.Anything,//new Structs.SDP_UUID_Entry[1],
                //--
                    (uint)1,
                    Is.Anything,//new Structs.SDP_Attribute_ID_List_Entry[1],
                //--
                    new InTheHand.Net.Bluetooth.StonestreetOne.NativeMethods
                        .SDP_Response_Callback(stuff.DutSdpQuery.HandleSDP_Response_Callback),
                    Is.Anything//OurRequestId
                    )/**/
                .Will(Return.Value(TheirRequestId_Error));
            try {
                var ar = stuff.DutSdpQuery.BeginQuery(Addr2, SvcClass2, false,
                    null, null);
                Assert.Fail("should have thrown!");
            } catch (SocketException) {
            }
        }

        [Test]
        public void ZeroHandleReturned()
        {
            var listAllocs = new List<IntPtr>();
            var stuff = BluetopiaSdpParseTests.Create_BluetopiaSdpQuery();
            const int TheirRequestId_InvalidZero = 0;
            //
            Expect.Once.On(stuff.MockedApi).Method("SDP_Service_Search_Attribute_Request")
                //TODO With(...Is.Anything,...)*/
                /**/.With(stuff.StackId,
                    Addr2Long,
                //Is.Anything,//new byte[6],
                //--
                    (uint)1,
                    Is.Anything,//new Structs.SDP_UUID_Entry[1],
                //--
                    (uint)1,
                    Is.Anything,//new Structs.SDP_Attribute_ID_List_Entry[1],
                //--
                    new InTheHand.Net.Bluetooth.StonestreetOne.NativeMethods
                        .SDP_Response_Callback(stuff.DutSdpQuery.HandleSDP_Response_Callback),
                    Is.Anything//OurRequestId
                    )/**/
                .Will(Return.Value(TheirRequestId_InvalidZero));
            try {
                var ar = stuff.DutSdpQuery.BeginQuery(Addr2, SvcClass2, false,
                    null, null);
                Assert.Fail("should have thrown!");
            } catch (SocketException) {
            }
        }

        [Test]
        public void ForGsrOne()
        {
            var listAllocs = new List<IntPtr>();
            var stuff = BluetopiaSdpParseTests.Create_BluetopiaSdpQuery();
            //const uint OurRequestId = 0 + 1;
            const byte Port = 17;
            const int TheirRequestId = 100;
            //
            Expect.Once.On(stuff.MockedApi).Method("SDP_Service_Search_Attribute_Request")
                //TODO With(...Is.Anything,...)*/
                /**/.With(stuff.StackId,
                    Addr2Long,
                    //Is.Anything,//new byte[6],
                    //--
                    (uint)1,
                    Is.Anything,//new Structs.SDP_UUID_Entry[1],
                    //--
                    (uint)1,
                    Is.Anything,//new Structs.SDP_Attribute_ID_List_Entry[1],
                    //--
                    new InTheHand.Net.Bluetooth.StonestreetOne.NativeMethods
                        .SDP_Response_Callback(stuff.DutSdpQuery.HandleSDP_Response_Callback),
                    Is.Anything//OurRequestId
                    )/**/
                .Will(Return.Value(TheirRequestId));
            var ar = stuff.DutSdpQuery.BeginQuery(Addr2, SvcClass2, false,
                null, null);
            //
            Assert.IsFalse(ar.IsCompleted, "IsCompleted before event");
            ClientTestingBluetopia.RaiseSdpEvent(stuff,
                BluetopiaSdpParseTests.ProtoDListMake_InSDPResponse_Data(listAllocs, Port), TheirRequestId);
            //
            ClientTestingBluetopia.SafeWait(ar);
            Assert.IsTrue(ar.IsCompleted, "IsCompleted after event");
            List<ServiceRecord> rList = stuff.DutSdpQuery.EndQuery(ar);
            BluetopiaSdpParseTests.Free(listAllocs);
            //
            BluetopiaSdpParseTests.ProtoDList_Assert(rList[0][0], Port);
        }

        [Test]
        public void RealGsrOne()
        {
            var listAllocs = new List<IntPtr>();
            var stuff = BluetopiaSdpParseTests.Create_BluetopiaSdpQuery();
            var dutBdi = (BluetopiaDeviceInfo)stuff.GetFactory().DoGetBluetoothDeviceInfo(Addr2);
            stuff.SetDut(dutBdi.Testing_GetSdpQuery());
            Debug.Assert(stuff.DutSdpQuery != null, "NULL stuff.DutSdpQuery");
            //const uint OurRequestId = 0 + 1;
            const byte Port = 17;
            const int TheirRequestId = 100;
            //
            Expect.Once.On(stuff.MockedApi).Method("SDP_Service_Search_Attribute_Request")
                //TODO With(...Is.Anything,...)*/
                /**/.With(stuff.StackId,
                    Addr2Long,
                //Is.Anything,//new byte[6],
                //--
                    (uint)1,
                    Is.Anything,//new Structs.SDP_UUID_Entry[1],
                //--
                    (uint)1,
                    Is.Anything,//new Structs.SDP_Attribute_ID_List_Entry[1],
                //--
                    new InTheHand.Net.Bluetooth.StonestreetOne.NativeMethods
                        .SDP_Response_Callback(stuff.DutSdpQuery.HandleSDP_Response_Callback),
                    Is.Anything//OurRequestId
                    )/**/
                .Will(Return.Value(TheirRequestId));
            var ar = dutBdi.BeginGetServiceRecords(SvcClass2,
                null, null);
            //
            Assert.IsFalse(ar.IsCompleted, "IsCompleted before event");
            ClientTestingBluetopia.RaiseSdpEvent(stuff,
                BluetopiaSdpParseTests.ProtoDListMake_InSDPResponse_Data(listAllocs, Port), TheirRequestId);
            //
            ClientTestingBluetopia.SafeWait(ar);
            Assert.IsTrue(ar.IsCompleted, "IsCompleted after event");
            ServiceRecord[] rList = dutBdi.EndGetServiceRecords(ar);
            BluetopiaSdpParseTests.Free(listAllocs);
            //
            BluetopiaSdpParseTests.ProtoDList_Assert(rList[0][0], Port);
        }

        [Test]
        public void CliConnectOne()
        {
            var listAllocs = new List<IntPtr>();
            var stuff = BluetopiaSdpParseTests.Create_BluetopiaSdpQuery();
            //const uint OurRequestId = 0 + 1;
            const byte Port = 17;
            const int TheirRequestId = 100;
            //
            Expect.Once.On(stuff.MockedApi).Method("SDP_Service_Search_Attribute_Request")
                //TODO With(...Is.Anything,...)*/
                /**/.With(stuff.StackId,
                    Addr2Long,
                    //Is.Anything,//new byte[6],
                    //--
                    (uint)1,
                    Is.Anything,//new Structs.SDP_UUID_Entry[1],
                    //--
                    (uint)2,
                    Is.Anything,//new Structs.SDP_Attribute_ID_List_Entry[1],
                    //--
                    new InTheHand.Net.Bluetooth.StonestreetOne.NativeMethods
                        .SDP_Response_Callback(stuff.DutSdpQuery.HandleSDP_Response_Callback),
                    Is.Anything//OurRequestId
                    )/**/
                .Will(Return.Value(TheirRequestId));
            var ar = stuff.DutSdpQuery.BeginQuery(Addr2, SvcClass2, true,
                null, null);
            //
            Assert.IsFalse(ar.IsCompleted, "IsCompleted before event");
            ClientTestingBluetopia.RaiseSdpEvent(stuff,
                BluetopiaSdpParseTests.ProtoDListMake_InSDPResponse_Data(listAllocs, Port), TheirRequestId);
            //
            ClientTestingBluetopia.SafeWait(ar);
            Assert.IsTrue(ar.IsCompleted, "IsCompleted after event");
            List<ServiceRecord> rList = stuff.DutSdpQuery.EndQuery(ar);
            BluetopiaSdpParseTests.Free(listAllocs);
            //
            BluetopiaSdpParseTests.ProtoDList_Assert(rList[0][0], Port);
        }

    }
}
#endif
