#if BLUETOPIA
#define SUPPORTS_NIL
using System;
using NUnit.Framework;
using InTheHand.Net.Tests.Sdp2;
using System.Collections.Generic;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.StonestreetOne;
using System.Diagnostics;
using NMock2;
using System.Text;

namespace InTheHand.Net.Tests.BluetopiaTests
{
    class SdpServiceExpectedCall
    {
        public readonly UInt16 attrId;
        //public readonly byte[] val;
        public readonly object element;

        public SdpServiceExpectedCall(UInt16 attrId,
            StackConsts.SDP_Data_Element_Type attrType, UInt32 len, byte[] val)
        {
            this.attrId = attrId;
            this.element = new Structs.SDP_Data_Element__Class_NonInlineByteArray(
                attrType, len, val);
        }

        public SdpServiceExpectedCall(UInt16 attrId,
            StackConsts.SDP_Data_Element_Type attrType, byte[] val)
        {
            this.attrId = attrId;
            //var reversed = (byte[])val.Clone();
            //if (attrType != StackConsts.SDP_Data_Element_Type.UUID_128) {
            //    Array.Reverse(reversed);
            //}
            //this.element = new Structs.SDP_Data_Element__Class_InlineByteArray(
            //    attrType, val.Length, reversed);
            this.element = new Structs.SDP_Data_Element__Class_InlineByteArray(
                attrType, val.Length, val);
        }

        public SdpServiceExpectedCall(UInt16 attrId,
            Structs.SDP_Data_Element__Class element)
        {
            this.attrId = attrId;
            this.element = element;
        }

    }


    [TestFixture]
    public class BluetopiaSdpCreatorTests : SdpCreatorTests
    {
        Dictionary<byte[], SdpServiceExpectedCall> resultMap = new Dictionary<byte[], SdpServiceExpectedCall>();

        // The tests are included in the base class.
        protected override void DoTest(byte[] expectedRecordBytes, ServiceRecord record)
        {
            byte[] buf = new byte[1024];
            var stuff = BluetopiaTesting.InitMockery_SdpCreator();
            //
            if (expectedRecordBytes == Data_SdpCreator_SingleElementTests.RecordBytes_Empty) {
                Debug.Assert(!resultMap.ContainsKey(expectedRecordBytes), "manually handled!");
            } else {
                // All other expected results are in the mapping table.
                // Next can throw KeyNotFoundException.
                SdpServiceExpectedCall expected = resultMap[expectedRecordBytes];
                Expect.Once.On(stuff.MockApi2).Method("SDP_Add_Attribute")
                    .With(
                    Is.Anything,
                    Is.Anything,
                    expected.attrId,
                    expected.element
                    )
                .Will(Return.Value(BluetopiaError.OK));

            }
            //==
            record = BluetopiaTesting.HackAddSvcClassList(record);
            //==
            stuff.DutSdpCreator.CreateServiceRecord(record);
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            //
            Expect.Once.On(stuff.MockApi2).Method("SDP_Delete_Service_Record")
                .With(
                Is.Anything,
                Is.Anything)
            .Will(Return.Value(BluetopiaError.OK));
            stuff.DutSdpCreator.Dispose();
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
        }

        public BluetopiaSdpCreatorTests()
        {
            AddExpectedResults();
        }

        byte[] IntegralNetworkToApi(byte[] val)
        {
            var reversed = (byte[])val.Clone();
            Array.Reverse(reversed);
            return reversed;
        }

        byte[] Uuid16And32NetworkToApi(byte[] val)
        {
            return IntegralNetworkToApi(val);
        }

        void AddExpectedResults()
        {
            // Empty -- manually handled
            // UInt
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneUInt16,
                new SdpServiceExpectedCall(0x0401, StackConsts.SDP_Data_Element_Type.UnsignedInteger2Bytes, IntegralNetworkToApi(new byte[] { 0xfe, 0x012 })));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneUInt16_HighAttrId,
                new SdpServiceExpectedCall(0xF401, StackConsts.SDP_Data_Element_Type.UnsignedInteger2Bytes, IntegralNetworkToApi(new byte[] { 0xfe, 0x012 })));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneUInt32,
                new SdpServiceExpectedCall(0x0401, StackConsts.SDP_Data_Element_Type.UnsignedInteger4Bytes, IntegralNetworkToApi(new byte[] { 0xfe, 0x12, 0x56, 0x01 })));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneUInt64,
                new SdpServiceExpectedCall(0x0401, StackConsts.SDP_Data_Element_Type.UnsignedInteger8Bytes, IntegralNetworkToApi(new byte[] { 0xfe, 0x12, 0x56, 0x01, 0x23, 0x45, 0x67, 0x89 })));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneUInt8,
                new SdpServiceExpectedCall(0x0401, StackConsts.SDP_Data_Element_Type.UnsignedInteger1Byte, IntegralNetworkToApi(new byte[] { 0xfe })));
            // Int
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneInt16,
                new SdpServiceExpectedCall(0x0401, StackConsts.SDP_Data_Element_Type.SignedInteger2Bytes, IntegralNetworkToApi(new byte[] { 0xfe, 0x12 })));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneInt32,
                new SdpServiceExpectedCall(0x0401, StackConsts.SDP_Data_Element_Type.SignedInteger4Bytes, IntegralNetworkToApi(new byte[] { 0xfe, 0x12, 0x56, 0x01 })));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneInt64,
                new SdpServiceExpectedCall(0x0401, StackConsts.SDP_Data_Element_Type.SignedInteger8Bytes, IntegralNetworkToApi(new byte[] { 0xfe, 0x12, 0x56, 0x01, 0x23, 0x45, 0x67, 0x89 })));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneInt8,
                new SdpServiceExpectedCall(0x0401, StackConsts.SDP_Data_Element_Type.SignedInteger1Byte, IntegralNetworkToApi(new byte[] { 0xfe })));
            // Children
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_ChildElementWithOneUInt16, 
                new SdpServiceExpectedCall(0x0401, new Structs.SDP_Data_Element__Class_ElementArray(
                    StackConsts.SDP_Data_Element_Type.Sequence,
                    1, new Structs.SDP_Data_Element__Class_InlineByteArray(
                        StackConsts.SDP_Data_Element_Type.UnsignedInteger2Bytes, 2, IntegralNetworkToApi(new byte[] { 0xfe, 0x12 })))));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_ChildElementAlternativeWithTwoUInt16,
                new SdpServiceExpectedCall(0x0401,
                    new Structs.SDP_Data_Element__Class_ElementArray(
                        StackConsts.SDP_Data_Element_Type.Alternative,
                        2,
                        new Structs.SDP_Data_Element__Class_InlineByteArray(
                            StackConsts.SDP_Data_Element_Type.UnsignedInteger2Bytes,2, IntegralNetworkToApi(new byte[] { 0xfe, 0x12 })),
                        new Structs.SDP_Data_Element__Class_InlineByteArray(
                            StackConsts.SDP_Data_Element_Type.UnsignedInteger2Bytes,2, IntegralNetworkToApi(new byte[] { 0x12, 0x34 })))));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_ElementsAndVariableAndFixedInDeepTree1,
                new SdpServiceExpectedCall(0x0401, CreateExpected_ElementsAndVariableAndFixedInDeepTree1()));
            // UUID
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneUuid16,
                new SdpServiceExpectedCall(0x0401, StackConsts.SDP_Data_Element_Type.UUID_16, Uuid16And32NetworkToApi(new byte[] { 0x12, 0xfe })));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneUuid32,
                new SdpServiceExpectedCall(0x0401, StackConsts.SDP_Data_Element_Type.UUID_32, Uuid16And32NetworkToApi(new byte[] { 0x01, 0x56, 0x12, 0xfe })));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneUuid128,
                new SdpServiceExpectedCall(0x0401, StackConsts.SDP_Data_Element_Type.UUID_128, new byte[] { 
                    0x12, 0x34, 0x56, 0x78, 
                    /*-*/0x23, 0x45, 
                    /*-*/0x34, 0x56,
                    /*-*/0x45, 0x67, 
                    /*-*/0x10, 0x20, 0x30, 0x40, 0x50, 0x60 }));
            //
#if SUPPORTS_NIL
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneNil,
                new SdpServiceExpectedCall(0x0401, StackConsts.SDP_Data_Element_Type.NIL, new byte[] { }));
#endif
            // Boolean
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneBooleanTrue,
                new SdpServiceExpectedCall(0x0401, StackConsts.SDP_Data_Element_Type.Boolean, new byte[] { 1 }));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneBooleanFalse,
                new SdpServiceExpectedCall(0x0401, StackConsts.SDP_Data_Element_Type.Boolean, new byte[] { 0 }));
            //
            //
            // URI
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneUrl,
                new SdpServiceExpectedCall(0x0401, StackConsts.SDP_Data_Element_Type.URL,
                    (uint)Data_SdpCreator_SingleElementTests.RecordBytes_OneUrl_Data.Length,
                    Data_SdpCreator_SingleElementTests.RecordBytes_OneUrl_Data));
            // String
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneString_Utf8,
                new SdpServiceExpectedCall(0x0401, StackConsts.SDP_Data_Element_Type.TextString,
                    (uint)Data_SdpCreator_SingleElementTests.RecordBytes_OneString_Utf8_Data.Length,
                    Data_SdpCreator_SingleElementTests.RecordBytes_OneString_Utf8_Data));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneString_Utf16le,
                new SdpServiceExpectedCall(0x0401, StackConsts.SDP_Data_Element_Type.TextString,
                    (uint)Data_SdpCreator_SingleElementTests.RecordBytes_OneString_Utf16le_Data.Length,
                    Data_SdpCreator_SingleElementTests.RecordBytes_OneString_Utf16le_Data));

        }

        private Structs.SDP_Data_Element__Class CreateExpected_ElementsAndVariableAndFixedInDeepTree1()
        {
            var str = Data_SdpCreator_SingleElementTests.RecordBytes_OneString_StringValueAsBytes;
            var itemStr1 = new Structs.SDP_Data_Element__Class_NonInlineByteArray(
                StackConsts.SDP_Data_Element_Type.TextString, str.Length, str);
            var itemStr2 = new Structs.SDP_Data_Element__Class_NonInlineByteArray(
                StackConsts.SDP_Data_Element_Type.TextString, str.Length, str);
            //
            Uri uriU = new Uri("http://example.com/foo.txt");
            var uri = Encoding.ASCII.GetBytes(uriU.ToString());
            var itemUrl = new Structs.SDP_Data_Element__Class_NonInlineByteArray(
                StackConsts.SDP_Data_Element_Type.URL, uri.Length, uri);
            //
            var itemF1 = new Structs.SDP_Data_Element__Class_InlineByteArray(
                StackConsts.SDP_Data_Element_Type.UnsignedInteger2Bytes, 2, IntegralNetworkToApi(new byte[] { 0xfe, 0x12 }));
            var itemF2 = new Structs.SDP_Data_Element__Class_InlineByteArray(
                StackConsts.SDP_Data_Element_Type.UnsignedInteger2Bytes, 2, IntegralNetworkToApi(new byte[] { 0x12, 0x34 }));
            //
            var leaves2 = new List<Structs.SDP_Data_Element__Class>();
            leaves2.Add(itemStr1);
            leaves2.Add(itemUrl);
            leaves2.Add(itemF1);
            var e2 = new Structs.SDP_Data_Element__Class_ElementArray(
                StackConsts.SDP_Data_Element_Type.Sequence, leaves2.Count, leaves2.ToArray());
            //
            var e1 = new Structs.SDP_Data_Element__Class_ElementArray(
                StackConsts.SDP_Data_Element_Type.Sequence, 1, e2);
            //
            var leaves0 = new List<Structs.SDP_Data_Element__Class>();
            leaves0.Add(e1);
            leaves0.Add(itemStr2);
            leaves0.Add(itemF2);
            var e0 = new Structs.SDP_Data_Element__Class_ElementArray(
                StackConsts.SDP_Data_Element_Type.Alternative, leaves0.Count, 
                        leaves0.ToArray());
            return e0;
        }

    }
}
#endif
