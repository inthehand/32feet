using System;
using System.Collections.Generic;
using System.Text;
using InTheHand.Net.Tests.Sdp2;
using NUnit.Framework;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Widcomm;
using System.Diagnostics;


namespace InTheHand.Net.Tests.Widcomm
{
    class SdpServiceExpectedCall
    {
        public readonly UInt16 attrId;
        public readonly SdpService.DESC_TYPE attrType;
        public readonly UInt32 attrLen;
        public readonly byte[] val;

        public SdpServiceExpectedCall(UInt16 attrId,
            SdpService.DESC_TYPE attrType, UInt32 attrLen, byte[] val)
        {
            this.attrId = attrId;
            this.attrType = attrType;
            this.attrLen = attrLen;
            this.val = val;
            Debug.Assert(attrLen == val.Length, "consistent length?!");
        }

    }


    [TestFixture]
    public class WidcommSdpRecordCreatorTest : SdpCreatorTests
    {
        Dictionary<byte[], SdpServiceExpectedCall> resultMap = new Dictionary<byte[], SdpServiceExpectedCall>();

        // The tests are included in the base class.

#if !SUPPORTS_NIL
        public override void OneNil()  // Disable this test!!
        {
        }
#endif

        protected override void DoTest(byte[] expectedRecordBytes, ServiceRecord record)
        {
            byte[] buf = new byte[1024];
            WidcommSdpServiceCreator creator = new WidcommSdpServiceCreator();
            TestSdpService2 iface = new TestSdpService2();
            creator.CreateServiceRecord(record, iface);
            //
            if (expectedRecordBytes == Data_SdpCreator_SingleElementTests.RecordBytes_Empty) {
                Debug.Assert(!resultMap.ContainsKey(expectedRecordBytes), "manually handled!");
                iface.AssertAreZeroAddAttributeCalls();
                iface.AssertAreZeroWellKnownCalls();
            } else {
                // All other expected results are in the mapping table.
                // Next can throw KeyNotFoundException.
                SdpServiceExpectedCall expected = resultMap[expectedRecordBytes];
                iface.AssertAddAttribute(expected);
            }
        }

        public WidcommSdpRecordCreatorTest()
        {
            AddExpectedResults();
        }

        void AddExpectedResults()
        {
            // Empty -- manually handled
            // UInt
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneUInt16,
                new SdpServiceExpectedCall(0x0401, SdpService.DESC_TYPE.UINT, 2, new byte[] { 0xfe, 0x012 }));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneUInt16_HighAttrId,
                new SdpServiceExpectedCall(0xF401, SdpService.DESC_TYPE.UINT, 2, new byte[] { 0xfe, 0x012 }));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneUInt32,
                new SdpServiceExpectedCall(0x0401, SdpService.DESC_TYPE.UINT, 4, new byte[] { 0xfe, 0x12, 0x56, 0x01 }));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneUInt64,
                new SdpServiceExpectedCall(0x0401, SdpService.DESC_TYPE.UINT, 8, new byte[] { 0xfe, 0x12, 0x56, 0x01, 0x23, 0x45, 0x67, 0x89 }));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneUInt8,
                new SdpServiceExpectedCall(0x0401, SdpService.DESC_TYPE.UINT, 1, new byte[] { 0xfe }));
            // Int
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneInt16,
                new SdpServiceExpectedCall(0x0401, SdpService.DESC_TYPE.TWO_COMP_INT, 2, new byte[] { 0xfe, 0x12 }));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneInt32,
                new SdpServiceExpectedCall(0x0401, SdpService.DESC_TYPE.TWO_COMP_INT, 4, new byte[] { 0xfe, 0x12, 0x56, 0x01 }));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneInt64,
                new SdpServiceExpectedCall(0x0401, SdpService.DESC_TYPE.TWO_COMP_INT, 8, new byte[] { 0xfe, 0x12, 0x56, 0x01, 0x23, 0x45, 0x67, 0x89 }));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneInt8,
                new SdpServiceExpectedCall(0x0401, SdpService.DESC_TYPE.TWO_COMP_INT, 1, new byte[] { 0xfe }));
            // Children
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_ChildElementWithOneUInt16,
                new SdpServiceExpectedCall(0x0401, SdpService.DESC_TYPE.DATA_ELE_SEQ, 3, new byte[] { 0x09, 0xfe, 0x12 }));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_ChildElementAlternativeWithTwoUInt16,
                new SdpServiceExpectedCall(0x0401, SdpService.DESC_TYPE.DATA_ELE_ALT, 6, new byte[] { 
                    0x09, 0xfe, 0x12,
                    0x09, 0x12, 0x34 }));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_ElementsAndVariableAndFixedInDeepTree1,
                new SdpServiceExpectedCall(0x0401, SdpService.DESC_TYPE.DATA_ELE_ALT, 66, new byte[] { 
                    0x35, 
                    0x2F, 0x35, 0x2D, 0x25, 0x0C, 0x61, 0x62, 0x63, 
                    0x64, 0xC3, 0xA9, 0x66, 0x67, 0x68, 0xC4, 0xAD, 
                    0x6A, 0x45, 0x1A, 0x68, 0x74, 0x74, 0x70, 0x3A, 
                    0x2F, 0x2F, 0x65, 0x78, 0x61, 0x6D, 0x70, 0x6C, 
                    0x65, 0x2E, 0x63, 0x6F, 0x6D, 0x2F, 0x66, 0x6F, 
                    0x6F, 0x2E, 0x74, 0x78, 0x74, 0x09, 0xFE, 0x12, 
                    0x25, 0x0C, 0x61, 0x62, 0x63, 0x64, 0xC3, 0xA9, 
                    0x66, 0x67, 0x68, 0xC4, 0xAD, 0x6A, 0x09, 0x12, 
                    0x34, }));
            // UUID
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneUuid16,
                new SdpServiceExpectedCall(0x0401, SdpService.DESC_TYPE.UUID, 2, new byte[] { 0xfe, 0x12 }));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneUuid32,
                new SdpServiceExpectedCall(0x0401, SdpService.DESC_TYPE.UUID, 4, new byte[] { 0xfe, 0x12, 0x56, 0x01 }));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneUuid128,
                new SdpServiceExpectedCall(0x0401, SdpService.DESC_TYPE.UUID, 16, new byte[] { 
                    0x12, 0x34, 0x56, 0x78, 
                    /*-*/0x23, 0x45, 
                    /*-*/0x34, 0x56,
                    /*-*/0x45, 0x67, 
                    /*-*/0x10, 0x20, 0x30, 0x40, 0x50, 0x60 }));
            //
#if SUPPORTS_NIL
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneNil,
                new SdpServiceExpectedCall(0x0401, SdpService.DESC_TYPE.UUID, 4, new byte[] { 0xfe, 0x12, 0x56, 0x01 }));
#endif
            // Boolean
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneBooleanTrue,
                new SdpServiceExpectedCall(0x0401, SdpService.DESC_TYPE.BOOLEAN, 1, new byte[] { 1 }));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneBooleanFalse,
                new SdpServiceExpectedCall(0x0401, SdpService.DESC_TYPE.BOOLEAN, 1, new byte[] { 0 }));
            //
            //
            // URI
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneUrl,
                new SdpServiceExpectedCall(0x0401, SdpService.DESC_TYPE.URL,
                    (uint)Data_SdpCreator_SingleElementTests.RecordBytes_OneUrl_Data.Length,
                    Data_SdpCreator_SingleElementTests.RecordBytes_OneUrl_Data));
            // String
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneString_Utf8,
                new SdpServiceExpectedCall(0x0401, SdpService.DESC_TYPE.TEXT_STR,
                    (uint)Data_SdpCreator_SingleElementTests.RecordBytes_OneString_Utf8_Data.Length,
                    Data_SdpCreator_SingleElementTests.RecordBytes_OneString_Utf8_Data));
            resultMap.Add(Data_SdpCreator_SingleElementTests.RecordBytes_OneString_Utf16le,
                new SdpServiceExpectedCall(0x0401, SdpService.DESC_TYPE.TEXT_STR,
                    (uint)Data_SdpCreator_SingleElementTests.RecordBytes_OneString_Utf16le_Data.Length,
                    Data_SdpCreator_SingleElementTests.RecordBytes_OneString_Utf16le_Data));

        }

    }

    [TestFixture]
    public class WidcommSdpRecordCreatorCompleteRecordsTest : SdpCreator_CompleteRecords
    {
        Dictionary<byte[], string> resultMap = new Dictionary<byte[], string>();

        // The tests are included in the base class.

        protected override void DoTest(byte[] expectedRecordBytes, ServiceRecord record)
        {
            byte[] buf = new byte[1024];
            WidcommSdpServiceCreator creator = new WidcommSdpServiceCreator();
            TestSdpService2 iface = new TestSdpService2();
            creator.CreateServiceRecord(record, iface);
            //
            //if (expectedRecordBytes == Data_SdpCreator_SingleElementTests.RecordBytes_Empty) {
            //    Debug.Assert(!resultMap.ContainsKey(expectedRecordBytes), "manually handled!");
            //    iface.AssertZeroAddAttributeCalls();
            //} else 
            {
                // All other expected results are in the mapping table.
                // Next can throw KeyNotFoundException.
                string expected = resultMap[expectedRecordBytes];
                iface.AssertCalls(expected);
            }
        }

        public WidcommSdpRecordCreatorCompleteRecordsTest()
        {
            AddExpectedResults();
        }

        void AddExpectedResults()
        {
            resultMap.Add(InTheHand.Net.Tests.Sdp2.Data_CompleteThirdPartyRecords.Xp1Sdp,
                "AddAttribute: id: 0x0000, dt: UINT, len: 4, val: 00-00-00-00" + NewLine //ServiceRecordHandle
                + "AddServiceClassIdList: <00001000-0000-1000-8000-00805f9b34fb>" + NewLine
                //+ "AddL2capPDL/AddAttribute" + NewLine
                + "AddAttribute: id: 0x0004, dt: DATA_ELE_SEQ, len: 13, val: "
                // 0x35x06   0x19x01x00    0x09x00x01  , 0x35x03x19x00x01
                +   "35-06-"+ "19-01-00-" + "09-00-01-" + "35-03-19-00-01" + NewLine
                + "AddAttribute: id: 0x0005, dt: DATA_ELE_SEQ, len: 3, val: 19-10-02" + NewLine //BrowseGroupList
                // LanguageBaseAttributeIdList
                + "AddAttribute: id: 0x0006, dt: DATA_ELE_SEQ, len: 9, val: "
                    + "09-65-6E-" + "09-00-6A-" + "09-01-00" + NewLine
                + "AddAttribute: id: 0x0100, dt: TEXT_STR, len: 18, val: 53-65-72-76-69-63-65-20-44-69-73-63-6F-76-65-72-79-00" + NewLine
                + "AddAttribute: id: 0x0101, dt: TEXT_STR, len: 37, val: 50-75-62-6C-69-73-68-65-73-20-73-65-72-76-69-63-65-73-20-74-6F-20-72-65-6D-6F-74-65-20-64-65-76-69-63-65-73-00" + NewLine
                + "AddAttribute: id: 0x0102, dt: TEXT_STR, len: 10, val: 4D-69-63-72-6F-73-6F-66-74-00" + NewLine
                + "AddAttribute: id: 0x0200, dt: DATA_ELE_SEQ, len: 3, val: 09-01-00" + NewLine
                + "AddAttribute: id: 0x0201, dt: UINT, len: 4, val: 00-00-00-01" + NewLine
                );
            resultMap.Add(InTheHand.Net.Tests.Sdp2.Data_CompleteThirdPartyRecords.XpB_1of2_1115,
                // 0x09, 0x00, 0x00, /**/ 0x0a, 0x00, 0x01,  0x00, 0x00, 
                "AddAttribute: id: 0x0000, dt: UINT, len: 4, val: 00-01-00-00" + NewLine
                // 0x09, 0x00, 0x01, /**/ 0x35, 0x03, 0x19, 0x11, 0x15, 
                + "AddServiceClassIdList: <00001115-0000-1000-8000-00805f9b34fb>" + NewLine
                // 0x09, 0x00, 0x04, /**/ 0x35, 0x12, 0x35, 0x06, 0x19, 0x01, 0x00, 0x09, 0x00, 0x0f, 0x35, 0x08, 0x19, 0x00, 0x0f, 0x09, 0x01, 0x00, 0x35, 0x00, 
                + "AddAttribute: id: 0x0004, dt: DATA_ELE_SEQ, len: 18, val: "
                    + "35-06-19-01-00-09-00-0F-35-08-19-00-0F-09-01-00-35-00" + NewLine
                // 0x09, 0x00, 0x05, /**/0x35, 0x03, 0x19, 0x10, 0x02, 
                + "AddAttribute: id: 0x0005, dt: DATA_ELE_SEQ, len: 3, val: 19-10-02" + NewLine
                // 0x09, 0x00, 0x06, /**/ 0x35, 0x09, 0x09, 0x65, 0x6e, 0x09, 0x03, 0xf7, 0x09, 0x01, 0x00, 
                + "AddAttribute: id: 0x0006, dt: DATA_ELE_SEQ, len: 9, val: "
                    + "09-65-6E-09-03-F7-09-01-00" + NewLine
                // 0x09, 0x00, 0x09, /**/ 0x35, 0x08, 0x35, 0x06, 0x19, 0x11, 0x15, 0x09, 0x01, 0x00, 
                + "AddAttribute: id: 0x0009, dt: DATA_ELE_SEQ, len: 8, val: "
                    + "35-06-19-11-15-09-01-00" + NewLine
                // 0x09, 0x01, 0x00, /**/0x25, 0x38, ...
                + "AddAttribute: id: 0x0100, dt: TEXT_STR, len: 56, val: "
                    + "50-00-65-00-72-00-73-00-6F-00-6E-00-61-00-6C-00-"
                    + "20-00-41-00-64-00-2D-00-68-00-6F-00-63-00-20-00-"
                    + "55-00-73-00-65-00-72-00-20-00-53-00-65-00-72-00-"
                    + "76-00-69-00-63-00-65-00" + NewLine
                // 0x09, 0x01, 0x01, /**/0x25, 0x38, ..
                + "AddAttribute: id: 0x0101, dt: TEXT_STR, len: 56, val: "
                    + "50-00-65-00-72-00-73-00-6F-00-6E-00-61-00-6C-00-"
                    + "20-00-41-00-64-00-2D-00-68-00-6F-00-63-00-20-00-"
                    + "55-00-73-00-65-00-72-00-20-00-53-00-65-00-72-00-"
                    + "76-00-69-00-63-00-65-00" + NewLine
                // 0x09, 0x03, 0x0A, /**/0x09, 0x00, 0x00, 
                + "AddAttribute: id: 0x030A, dt: UINT, len: 2, val: 00-00" + NewLine
                );
            resultMap.Add(InTheHand.Net.Tests.Sdp2.Data_CompleteThirdPartyRecords.PalmOsOpp_HackMadeFirstLengthOneByteField,
                "AddAttribute: id: 0x0000, dt: UINT, len: 4, val: 00-01-00-01" + NewLine
                + "AddServiceClassIdList: <00001105-0000-1000-8000-00805f9b34fb>" + NewLine
#if USE_AddRfcommPdl_EVEN_FOR_NON_PURE_RFCOMM_PDLS
                //+ "AddRFCommProtocolDescriptor: 1" + NewLine
#else
                 + "AddAttribute: id: 0x0004, dt: DATA_ELE_SEQ, len: 17, val: "
                    + "35-03-19-01-00-"             // L2CAP
                    + "35-05-19-00-03-08-01-"       // RFCOMM
                    + "35-03-19-00-08" + NewLine    // GEOP
#endif
                // LanguageBaseAttributeIdList
                + "AddAttribute: id: 0x0006, dt: DATA_ELE_SEQ, len: 9, val: "
                    // 0x09x65x6e    0x09x08xcc    0x09x01x00
                    +   "09-65-6E-" + "09-08-CC-" + "09-01-00" + NewLine
                + "AddAttribute: id: 0x0100, dt: TEXT_STR, len: 16, val: 4F-42-45-58-20-4F-62-6A-65-63-74-20-50-75-73-68" + NewLine
                + "AddAttribute: id: 0x0303, dt: DATA_ELE_SEQ, len: 8, val: 08-01-08-02-08-03-08-FF" + NewLine
                );
            //----
            resultMap.Add(InTheHand.Net.Tests.Sdp2.Data_CompleteThirdPartyRecords.SimpleRfcommPdl,
                "AddRFCommProtocolDescriptor: 5" + NewLine);
            //
            resultMap.Add(InTheHand.Net.Tests.Sdp2.Data_CompleteThirdPartyRecords.SimpleAlternativeTwoRfcommPdl,
                "AddAttribute: id: 0x0004, dt: DATA_ELE_ALT, len: 28, val: "
                // #1
                + "35-0C-" //seq
                + "35-03-19-01-00-"             // L2CAP+PSM
                + "35-05-19-00-03-08-11-"       // RFCOMM
                // #2
                + "35-0C-" //seq
                + "35-03-19-01-00-"             // L2CAP+PSM
                + "35-05-19-00-03-08-12"       // RFCOMM
                + NewLine
                );
            //
            resultMap.Add(InTheHand.Net.Tests.Sdp2.Data_CompleteThirdPartyRecords.SimpleRfcommExplicitPsmSamePdl,
#if USE_AddRfcommPdl_WHEN_EXPLICIT_PSM_BUT_EQUALS_RFCOMM
                "AddRFCommProtocolDescriptor: 19" + NewLine
#else
                "AddAttribute: id: 0x0004, dt: DATA_ELE_SEQ, len: 15, val: "
                + "35-06-19-01-00-09-00-03-"             // L2CAP+PSM
                + "35-05-19-00-03-08-19"       // RFCOMM
                + NewLine
#endif
                );
            //
            resultMap.Add(InTheHand.Net.Tests.Sdp2.Data_CompleteThirdPartyRecords.SimpleRfcommExplicitPsmDifferentPdl,
                "AddAttribute: id: 0x0004, dt: DATA_ELE_SEQ, len: 15, val: "
                + "35-06-19-01-00-09-98-03-"             // L2CAP+PSM
                + "35-05-19-00-03-08-10"       // RFCOMM
                + NewLine
                );
        }

        const string NewLine = "\r\n";
    }

}
