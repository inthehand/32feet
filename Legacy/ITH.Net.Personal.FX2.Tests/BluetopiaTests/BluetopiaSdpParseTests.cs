#if BLUETOPIA
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.StonestreetOne;
using NUnit.Framework;
using NMock2;
using InTheHand.Net.Bluetooth.AttributeIds;

namespace InTheHand.Net.Tests.BluetopiaTests
{
    [TestFixture]
    public class BluetopiaSdpParseTests
    {
        // TODO (Mockery_VerifyAllExpectationsHaveBeenMet();
        // TODO (Uuid16).

        internal static StuffSdpQueryBluetopia Create_BluetopiaSdpQuery()
        {
            var stuff = new StuffSdpQueryBluetopia();
            BluetopiaTesting.InitMockery(stuff, 205);
            //
            stuff.SetDut(new BluetopiaSdpQuery(stuff.GetFactory()));
            return stuff;
        }

        private static ServiceRecord DoTestParseOneAttribute(List<IntPtr> listAllocs, Structs.SDP_Data_Element elem, ushort attrId)
        {
            IntPtr pCur;
            ServiceRecord r;
            IntPtr pElem = CopyToNative(listAllocs, ref elem);
            //
            var attrData = new Structs.SDP_Service_Attribute_Value_Data(
                attrId, pElem);
            pCur = CopyToNative(listAllocs, ref attrData);
            var attrRsp = new Structs.SDP_Service_Attribute_Response_Data(1, pCur);
            pCur = CopyToNative(listAllocs, ref attrRsp);
            //
            var stuff = Create_BluetopiaSdpQuery();
            r = stuff.DutSdpQuery.BuildRecord(pCur);
            return r;
        }

        //--------
        [Test]
        public void ZeroRecords()
        {
            var listAllocs = new List<IntPtr>();
            IntPtr pCur;
            //
            var svcSrchAttrRsp = new Structs.SDP_Response_Data__SDP_Service_Search_Attribute_Response_Data(
                StackConsts.SDP_Response_Data_Type.ServiceSearchAttributeResponse,
                0, IntPtr.Zero);
            pCur = CopyToNative(listAllocs, ref svcSrchAttrRsp);
            //
            var stuff = Create_BluetopiaSdpQuery();
            List<ServiceRecord> rList = stuff.DutSdpQuery.BuildRecordList(pCur);
            Assert.AreEqual(0, rList.Count);
            //
            Free(listAllocs);
        }

        [Test]
        public void ZeroAttributes()
        {
            var listAllocs = new List<IntPtr>();
            IntPtr pCur;
            //
            var attrRsp = new Structs.SDP_Service_Attribute_Response_Data(0, IntPtr.Zero);
            pCur = CopyToNative(listAllocs, ref attrRsp);
            //
            var svcSrchAttrRsp = new Structs.SDP_Response_Data__SDP_Service_Search_Attribute_Response_Data(
                StackConsts.SDP_Response_Data_Type.ServiceSearchAttributeResponse,
                1, pCur);
            pCur = CopyToNative(listAllocs, ref svcSrchAttrRsp);
            //
            var stuff = Create_BluetopiaSdpQuery();
            List<ServiceRecord> rList = stuff.DutSdpQuery.BuildRecordList(pCur);
            Assert.AreEqual(1, rList.Count);
            ServiceRecord r = rList[0];
            //
            Assert.AreEqual(0, r.Count, "Count");
            //
            Free(listAllocs);
        }

        //--------
        [Test]
        public void UInt8()
        {
            var listAllocs = new List<IntPtr>();
            IntPtr pCur;
            //
            var elemData = new Structs.SDP_Data_Element(
                StackConsts.SDP_Data_Element_Type.UnsignedInteger1Byte, 1);
            elemData.FakeAtUnionPosition = 0xF5;
            pCur = CopyToNative(listAllocs, ref elemData);
            //
            var attrData = new Structs.SDP_Service_Attribute_Value_Data(0xF123, pCur);
            pCur = CopyToNative(listAllocs, ref attrData);
            //
            var attrRsp = new Structs.SDP_Service_Attribute_Response_Data(1, pCur);
            pCur = CopyToNative(listAllocs, ref attrRsp);
            //
            var svcSrchAttrRsp = new Structs.SDP_Response_Data__SDP_Service_Search_Attribute_Response_Data(
                StackConsts.SDP_Response_Data_Type.ServiceSearchAttributeResponse,
                1, pCur);
            pCur = CopyToNative(listAllocs, ref svcSrchAttrRsp);
            //
            var stuff = Create_BluetopiaSdpQuery();
            List<ServiceRecord> rList = stuff.DutSdpQuery.BuildRecordList(pCur);
            Assert.AreEqual(1, rList.Count);
            ServiceRecord r = rList[0];
            //
            var attr = r[0];
            Assert.AreEqual(unchecked((ServiceAttributeId)0xF123), attr.Id, "AttrId");
            //Assert.AreEqual(unchecked((short)0xF123), attr.IdAsOrdinalNumber, "IdAsOrdinalNumber");
            Assert.AreEqual(ElementType.UInt8, attr.Value.ElementType, "ET");
            Assert.AreEqual(ElementTypeDescriptor.UnsignedInteger, attr.Value.ElementTypeDescriptor, "ET");
            Assert.AreEqual(0xF5, attr.Value.Value, "v");
            //
            Free(listAllocs);
        }

        [Test]
        public void Int8()
        {
            var listAllocs = new List<IntPtr>();
            IntPtr pCur;
            //
            var elemData = new Structs.SDP_Data_Element(
                StackConsts.SDP_Data_Element_Type.SignedInteger1Byte, 1);
            elemData.FakeAtUnionPosition = 0xF5;
            pCur = CopyToNative(listAllocs, ref elemData);
            ServiceRecord r = DoTestParseOneAttribute(listAllocs, elemData, 0xF123);
            //
            var attr = r[0];
            Assert.AreEqual(unchecked((ServiceAttributeId)0xF123), attr.Id, "AttrId");
            //Assert.AreEqual(unchecked((short)0xF123), attr.IdAsOrdinalNumber, "IdAsOrdinalNumber");
            Assert.AreEqual(ElementType.Int8, attr.Value.ElementType, "ET");
            Assert.AreEqual(ElementTypeDescriptor.TwosComplementInteger, attr.Value.ElementTypeDescriptor, "ET");
            Assert.AreEqual(unchecked((sbyte)0xF5), attr.Value.Value, "v");
            //
            Free(listAllocs);
        }

        [Test]
        public void UInt32()
        {
            var listAllocs = new List<IntPtr>();
            //
            var elemData = new Structs.SDP_Data_Element(
                StackConsts.SDP_Data_Element_Type.UnsignedInteger4Bytes, 4);
            elemData.FakeAtUnionPosition3 = 0xF5;
            elemData.FakeAtUnionPosition2 = 0x23;
            elemData.FakeAtUnionPosition1 = 0x45;
            elemData.FakeAtUnionPosition = 0x67;
            ServiceRecord r = DoTestParseOneAttribute(listAllocs, elemData, 0x1234);
            //
            var attr = r[0];
            Assert.AreEqual(unchecked((ServiceAttributeId)0x1234), attr.Id, "AttrId");
            Assert.AreEqual(0x1234, attr.IdAsOrdinalNumber, "IdAsOrdinalNumber");
            Assert.AreEqual(ElementType.UInt32, attr.Value.ElementType, "ET");
            Assert.AreEqual(ElementTypeDescriptor.UnsignedInteger, attr.Value.ElementTypeDescriptor, "ETD");
            Assert.AreEqual(0xF5234567, attr.Value.Value, "v");
            //
            Free(listAllocs);
        }

        [Test]
        public void UInt16()
        {
            var listAllocs = new List<IntPtr>();
            //
            var elemData = new Structs.SDP_Data_Element(
                StackConsts.SDP_Data_Element_Type.UnsignedInteger2Bytes, 2);
            elemData.FakeAtUnionPosition1 = 0xF5;
            elemData.FakeAtUnionPosition = 0x23;
            ServiceRecord r = DoTestParseOneAttribute(listAllocs, elemData, 0x1234);
            //
            var attr = r[0];
            Assert.AreEqual(unchecked((ServiceAttributeId)0x1234), attr.Id, "AttrId");
            Assert.AreEqual(0x1234, attr.IdAsOrdinalNumber, "IdAsOrdinalNumber");
            Assert.AreEqual(ElementType.UInt16, attr.Value.ElementType, "ET");
            Assert.AreEqual(ElementTypeDescriptor.UnsignedInteger, attr.Value.ElementTypeDescriptor, "ETD");
            Assert.AreEqual(0xF523, attr.Value.Value, "v");
            //
            Free(listAllocs);
        }

        [Test]
        public void UInt64()
        {
            var listAllocs = new List<IntPtr>();
            //
            var elemData = new Structs.SDP_Data_Element(
                StackConsts.SDP_Data_Element_Type.UnsignedInteger8Bytes, 8);
            elemData.FakeAtUnionPosition7 = 0xF5;
            elemData.FakeAtUnionPosition6 = 0x23;
            elemData.FakeAtUnionPosition5 = 0x45;
            elemData.FakeAtUnionPosition4 = 0x67;
            elemData.FakeAtUnionPosition3 = 0x4a;
            elemData.FakeAtUnionPosition2 = 0x5b;
            elemData.FakeAtUnionPosition1 = 0x6c;
            elemData.FakeAtUnionPosition = 0x7d;
            ServiceRecord r = DoTestParseOneAttribute(listAllocs, elemData, 0x1234);
            //
            var attr = r[0];
            Assert.AreEqual(unchecked((ServiceAttributeId)0x1234), attr.Id, "AttrId");
            Assert.AreEqual(0x1234, attr.IdAsOrdinalNumber, "IdAsOrdinalNumber");
            Assert.AreEqual(ElementType.UInt64, attr.Value.ElementType, "ET");
            Assert.AreEqual(ElementTypeDescriptor.UnsignedInteger, attr.Value.ElementTypeDescriptor, "ETD");
            Assert.AreEqual(0xF52345674a5b6c7d, attr.Value.Value, "v");
            //
            Free(listAllocs);
        }

        [Test]
        public void UInt128()
        {
            var listAllocs = new List<IntPtr>();
            //
            var elemData = new Structs.SDP_Data_Element(
                StackConsts.SDP_Data_Element_Type.UnsignedInteger16Bytes, 16);
            elemData.FakeAtUnionPositionF = 0xF5;
            elemData.FakeAtUnionPositionE = 0x23;
            elemData.FakeAtUnionPositionD = 0x45;
            elemData.FakeAtUnionPositionC = 0x67;
            elemData.FakeAtUnionPositionB = 0x4a;
            elemData.FakeAtUnionPositionA = 0x5b;
            elemData.FakeAtUnionPosition9 = 0x6c;
            elemData.FakeAtUnionPosition8 = 0x7d;
            elemData.FakeAtUnionPosition7 = 0x01;
            elemData.FakeAtUnionPosition6 = 0x02;
            elemData.FakeAtUnionPosition5 = 0x03;
            elemData.FakeAtUnionPosition4 = 0x04;
            elemData.FakeAtUnionPosition3 = 0x05;
            elemData.FakeAtUnionPosition2 = 0x06;
            elemData.FakeAtUnionPosition1 = 0x07;
            elemData.FakeAtUnionPosition = 0x08;
            ServiceRecord r = DoTestParseOneAttribute(listAllocs, elemData, 0x1234);
            //
            var attr = r[0];
            Assert.AreEqual(unchecked((ServiceAttributeId)0x1234), attr.Id, "AttrId");
            Assert.AreEqual(0x1234, attr.IdAsOrdinalNumber, "IdAsOrdinalNumber");
            Assert.AreEqual(ElementType.UInt128, attr.Value.ElementType, "ET");
            Assert.AreEqual(ElementTypeDescriptor.UnsignedInteger, attr.Value.ElementTypeDescriptor, "ETD");
            var expectedValue = new byte[] {
                0xF5, 0x23, 0x45, 0x67, 0x4a, 0x5b, 0x6c, 0x7d,
                0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08
            };
            Assert.AreEqual(expectedValue, attr.Value.Value, "v");
            //
            Free(listAllocs);
        }

        //----
        [Test]
        public void Uuid32()
        {
            var listAllocs = new List<IntPtr>();
            //
            var elemData = new Structs.SDP_Data_Element(
                StackConsts.SDP_Data_Element_Type.UUID_32, 4);
            elemData.FakeAtUnionPosition = 0xF5;
            elemData.FakeAtUnionPosition1 = 0x23;
            elemData.FakeAtUnionPosition2 = 0x45;
            elemData.FakeAtUnionPosition3 = 0x67;
            ServiceRecord r = DoTestParseOneAttribute(listAllocs, elemData, 0x1234);
            //
            var attr = r[0];
            Assert.AreEqual(unchecked((ServiceAttributeId)0x1234), attr.Id, "AttrId");
            Assert.AreEqual(0x1234, attr.IdAsOrdinalNumber, "IdAsOrdinalNumber");
            Assert.AreEqual(ElementType.Uuid32, attr.Value.ElementType, "ET");
            Assert.AreEqual(ElementTypeDescriptor.Uuid, attr.Value.ElementTypeDescriptor, "ET");
            Assert.AreEqual(0xF5234567, attr.Value.Value, "v");
            //
            Free(listAllocs);
        }

        [Test]
        public void Uuid128()
        {
            var listAllocs = new List<IntPtr>();
            //
            var elemData = new Structs.SDP_Data_Element(
                StackConsts.SDP_Data_Element_Type.UUID_128, 16);
            elemData.FakeAtUnionPosition = 0xF5;
            elemData.FakeAtUnionPosition1 = 0x23;
            elemData.FakeAtUnionPosition2 = 0x45;
            elemData.FakeAtUnionPosition3 = 0x67;
            ServiceRecord r = DoTestParseOneAttribute(listAllocs, elemData, 0x1234);
            //
            var g = new Guid("F5234567-0000-0000-0000-000000000000");//001122334455");
            var attr = r[0];
            Assert.AreEqual(unchecked((ServiceAttributeId)0x1234), attr.Id, "AttrId");
            Assert.AreEqual(0x1234, attr.IdAsOrdinalNumber, "IdAsOrdinalNumber");
            Assert.AreEqual(ElementType.Uuid128, attr.Value.ElementType, "ET");
            Assert.AreEqual(ElementTypeDescriptor.Uuid, attr.Value.ElementTypeDescriptor, "ETD");
            Assert.AreEqual(g, attr.Value.Value, "v");
            //
            Free(listAllocs);
        }

        //--------
        [Test]
        public void Boolean()
        {
            Boolean_(false, 0);
            Boolean_(true, 1);
            Boolean_(true, 0x80);
            Boolean_(true, 255);
        }

        static void Boolean_(bool expected, byte byteValue)
        {
            var listAllocs = new List<IntPtr>();
            //
            var elemData = new Structs.SDP_Data_Element(
                StackConsts.SDP_Data_Element_Type.Boolean, 1);
            elemData.FakeAtUnionPosition = byteValue;
            ServiceRecord r = DoTestParseOneAttribute(listAllocs, elemData, 0xF123);
            //
            var attr = r[0];
            Assert.AreEqual(unchecked((ServiceAttributeId)0xF123), attr.Id, "AttrId");
            Assert.AreEqual(ElementType.Boolean, attr.Value.ElementType, "ET");
            Assert.AreEqual(ElementTypeDescriptor.Boolean, attr.Value.ElementTypeDescriptor, "ET");
            Assert.AreEqual(expected, attr.Value.Value, "v");
            //
            Free(listAllocs);
        }

        [Test]
        public void Nil()
        {
            var listAllocs = new List<IntPtr>();
            //
            var elemData = new Structs.SDP_Data_Element(
                StackConsts.SDP_Data_Element_Type.NIL, 1);
            ServiceRecord r = DoTestParseOneAttribute(listAllocs, elemData, 0xF123);
            //
            var attr = r[0];
            Assert.AreEqual(unchecked((ServiceAttributeId)0xF123), attr.Id, "AttrId");
            Assert.AreEqual(ElementType.Nil, attr.Value.ElementType, "ET");
            Assert.AreEqual(ElementTypeDescriptor.Nil, attr.Value.ElementTypeDescriptor, "ET");
            Assert.AreEqual(null, attr.Value.Value, "v");
            //
            Free(listAllocs);
        }

        [Explicit]
        [Test]
        public void NULL()
        {
            var listAllocs = new List<IntPtr>();
            //
            var elemData = new Structs.SDP_Data_Element(
                StackConsts.SDP_Data_Element_Type.NULL, 1);
            ServiceRecord r = DoTestParseOneAttribute(listAllocs, elemData, 0xF123);
            //
            var attr = r[0];
            Assert.AreEqual(unchecked((ServiceAttributeId)0xF123), attr.Id, "AttrId");
            Assert.AreEqual(ElementType.Nil, attr.Value.ElementType, "ET");
            Assert.AreEqual(ElementTypeDescriptor.Nil, attr.Value.ElementTypeDescriptor, "ET");
            Assert.AreEqual(null, attr.Value.Value, "v");
            //
            Free(listAllocs);
        }

        //--------
        [Test]
        public void ProtoList()
        {
            var listAllocs = new List<IntPtr>();
            IntPtr pCur;
            //
            pCur = ProtoDListMake(listAllocs, 26);
            var stuff = Create_BluetopiaSdpQuery();
            ServiceRecord r = stuff.DutSdpQuery.BuildRecord(pCur);
            //
            var attr = r[0];
            ProtoDList_Assert(attr, 26);
            //
            Free(listAllocs);
        }

        [Test]
        public void ProtoListViaBuildRecords()
        {
            var listAllocs = new List<IntPtr>();
            IntPtr pCur;
            //
            pCur = ProtoDListMake_InSDPResponse_Data(listAllocs, 5);
            var stuff = Create_BluetopiaSdpQuery();
            var rList = stuff.DutSdpQuery.BuildRecordList(pCur);
            ServiceRecord r = rList[0];
            //
            var attr = r[0];
            ProtoDList_Assert(attr, 5);
            //
            Free(listAllocs);
        }

        internal static void ProtoDList_Assert(ServiceAttribute attr, byte port)
        {
            Assert.AreEqual(UniversalAttributeId.ProtocolDescriptorList, attr.Id, "AttrId");
            Assert.AreEqual(ElementType.ElementSequence, attr.Value.ElementType, "ET");
            var rootList = attr.Value.GetValueAsElementList();
            Assert.AreEqual(2, rootList.Count, "Count root");
            //
            var level0List = rootList[0].GetValueAsElementList();
            Assert.AreEqual(1, level0List.Count, "Count L0");
            var e0_0 = level0List[0];
            Assert.AreEqual(ElementType.Uuid16, e0_0.ElementType, "ET 0_0");
            Assert.AreEqual((ushort)0x0100, e0_0.Value, "value 0_0");
            //return;
            //
            var level1List = rootList[1].GetValueAsElementList();
            Assert.AreEqual(2, level1List.Count, "Count L1");
            var e1_0 = level1List[0];
            Assert.AreEqual(ElementType.Uuid16, e1_0.ElementType, "ET 1_1");
            Assert.AreEqual((ushort)0x0003, e1_0.Value, "value 1_0");
            var e1_1 = level1List[1];
            Assert.AreEqual(ElementType.UInt8, e1_1.ElementType, "ET 1_1");
            Assert.AreEqual(port, e1_1.Value, "value 1_1");
        }

        internal static IntPtr ProtoDListMake_InSDPResponse_Data(List<IntPtr> listAllocs, byte port)
        {
            IntPtr pSDP_Service_Attribute_Response_Data = ProtoDListMake(listAllocs, port);
            //
            var stru = new Structs.SDP_Response_Data__SDP_Service_Search_Attribute_Response_Data(
                StackConsts.SDP_Response_Data_Type.ServiceSearchAttributeResponse,
                1, pSDP_Service_Attribute_Response_Data);
            IntPtr pCur = CopyToNative(listAllocs, ref stru);
            return pCur;
        }

        internal static IntPtr ProtoDListMake(List<IntPtr> listAllocs, byte port)
        {
            IntPtr pCur;
            var elemDataRfcomm = new Structs.SDP_Data_Element(
                StackConsts.SDP_Data_Element_Type.UUID_16, 2);
            elemDataRfcomm.FakeAtUnionPosition = 0x00; // TODO Endian???!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            elemDataRfcomm.FakeAtUnionPosition1 = 0x03;
            IntPtr pCurRfcomm = CopyToNative(listAllocs, ref elemDataRfcomm);
            //
            var elemDataScn = new Structs.SDP_Data_Element(
                StackConsts.SDP_Data_Element_Type.UnsignedInteger1Byte, 1);
            elemDataScn.FakeAtUnionPosition = port;
            IntPtr pCurScn = CopyToNative(listAllocs, ref elemDataScn);
            //
            var elemListL1Arr = new[] { elemDataRfcomm, elemDataScn };
            var elemListL1 = new Structs.SDP_Data_Element(
                StackConsts.SDP_Data_Element_Type.Sequence, 2);
            SetPointerToArray(listAllocs, ref elemListL1, elemListL1Arr);
            IntPtr pelemListL1 = CopyToNative(listAllocs, ref elemListL1);
            //
            var elemDataL2Cap = new Structs.SDP_Data_Element(
                StackConsts.SDP_Data_Element_Type.UUID_16, 2);
            elemDataL2Cap.FakeAtUnionPosition = 0x01;
            elemDataL2Cap.FakeAtUnionPosition1 = 0x00;
            IntPtr pCurL2Cap = CopyToNative(listAllocs, ref elemDataL2Cap);
            //
            var elemListL0Arr = new[] { elemDataL2Cap };
            var elemListL0 = new Structs.SDP_Data_Element(
                StackConsts.SDP_Data_Element_Type.Sequence, 1);
            SetPointerToArray(listAllocs, ref elemListL0, elemListL0Arr);
            IntPtr pelemListL0 = CopyToNative(listAllocs, ref elemListL0);
            //
            var elemListRootArr = new[] { elemListL0, elemListL1 };
            var elemListRoot = new Structs.SDP_Data_Element(
                StackConsts.SDP_Data_Element_Type.Sequence, 2);
            SetPointerToArray(listAllocs, ref elemListRoot, elemListRootArr);
            IntPtr pelemListRoot = CopyToNative(listAllocs, ref elemListRoot);
            //
            var attrData = new Structs.SDP_Service_Attribute_Value_Data(0x4, pelemListRoot);
            pCur = CopyToNative(listAllocs, ref attrData);
            var attrRsp = new Structs.SDP_Service_Attribute_Response_Data(1, pCur);
            pCur = CopyToNative(listAllocs, ref attrRsp);
            return pCur;
        }


        [Test]
        public void String()
        {
            var listAllocs = new List<IntPtr>();
            //
            var strBytes = new byte[] { (byte)'a', (byte)'b', (byte)'c', (byte)'d', };
            var elem = new Structs.SDP_Data_Element(
                StackConsts.SDP_Data_Element_Type.TextString, checked((uint)strBytes.Length));
            SetPointerToArray(listAllocs, ref elem, strBytes);
            ServiceRecord r = DoTestParseOneAttribute(listAllocs, elem, 0x1234);
            //
            var attr = r[0];
            Assert.AreEqual(unchecked((ServiceAttributeId)0x1234), attr.Id, "AttrId");
            Assert.AreEqual(ElementType.TextString, attr.Value.ElementType, "ET");
            Assert.AreEqual("abcd", attr.Value.GetValueAsStringUtf8(), "value");
            //
            Free(listAllocs);
        }

        [Test]
        public void Url()
        {
            var listAllocs = new List<IntPtr>();
            //
            var strBytes = new byte[] { (byte)'a', (byte)'b', (byte)':', (byte)'c', (byte)'d', };
            var elem = new Structs.SDP_Data_Element(
                StackConsts.SDP_Data_Element_Type.URL, checked((uint)strBytes.Length));
            SetPointerToArray(listAllocs, ref elem, strBytes);
            ServiceRecord r = DoTestParseOneAttribute(listAllocs, elem, 0x1234);
            //
            var attr = r[0];
            Assert.AreEqual(unchecked((ServiceAttributeId)0x1234), attr.Id, "AttrId");
            Assert.AreEqual(ElementType.Url, attr.Value.ElementType, "ET");
            Assert.AreEqual(new Uri("ab:cd"),
                attr.Value.GetValueAsUri(), "value");
            //
            Free(listAllocs);
        }


        //----------------
        private static IntPtr CopyToNative<T>(List<IntPtr> listAllocs, ref T stru)
            where T : struct
        {
            IntPtr pCur;
            pCur = Marshal.AllocHGlobal(Marshal.SizeOf(stru));
            listAllocs.Add(pCur);
            Marshal.StructureToPtr(stru, pCur, false);
            return pCur;
        }

        internal static void Free(List<IntPtr> listAllocs)
        {
            foreach (var cur in listAllocs) {
                Marshal.FreeHGlobal(cur);
            }
        }

        //--------
        private static void SetPointerToArray<T>(List<IntPtr> listAllocs,
            ref Structs.SDP_Data_Element parent,
            T[] child)
            where T : struct
        {
            var sizeOfElem = Marshal.SizeOf(typeof(T));
            IntPtr pAlloc = Marshal.AllocHGlobal(sizeOfElem * child.Length);
            listAllocs.Add(pAlloc);
            IntPtr pCur = pAlloc;
            for (int i = 0; i < child.Length; ++i) {
                Marshal.StructureToPtr(child[i], pCur, false);
                // Next
                pCur = PointerAdd(pCur, sizeOfElem);
            }//for
            //
            var iPtr = pAlloc.ToInt64();
            var bytesPtr = BitConverter.GetBytes(iPtr);
            parent.FakeAtUnionPosition = bytesPtr[0];
            parent.FakeAtUnionPosition1 = bytesPtr[1];
            parent.FakeAtUnionPosition2 = bytesPtr[2];
            parent.FakeAtUnionPosition3 = bytesPtr[3];
            parent.FakeAtUnionPosition4 = bytesPtr[4];
            parent.FakeAtUnionPosition5 = bytesPtr[5];
            parent.FakeAtUnionPosition6 = bytesPtr[6];
            parent.FakeAtUnionPosition7 = bytesPtr[7];
        }

        //--------
        private static IntPtr PointerAdd(IntPtr x, int y)
        {
            checked {
                var xi = x.ToInt64();
                xi += y;
                IntPtr p = new IntPtr(xi);
                return p;
            }
        }

        private static IntPtr PointerAdd(IntPtr x, IntPtr y)
        {
            checked {
                var xi = x.ToInt64();
                var yi = y.ToInt64();
                xi += yi;
                IntPtr p = new IntPtr(xi);
                return p;
            }
        }

    }
}
#endif
