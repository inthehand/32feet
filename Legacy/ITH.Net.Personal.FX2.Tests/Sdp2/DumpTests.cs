using System;
using NUnit.Framework;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.AttributeIds;
#if FX1_1
using NotImplementedException = System.NotSupportedException;
#endif

namespace InTheHand.Net.Tests.Sdp2
{

    [TestFixture]
    public class DumpCompleteThirdPartyRecords
    {

        internal static void DoTestSmart(String expected, byte[] recordBytes, params Type[] attributeIdEnumDefiningTypes)
        {
            ServiceRecordParser parser = new ServiceRecordParser();
            parser.SkipUnhandledElementTypes = true;
            ServiceRecord record = parser.Parse(recordBytes);
            DoTestSmart(expected, record, attributeIdEnumDefiningTypes);
        }

        internal static void DoTestSmart(String expected, ServiceRecord record, params Type[] attributeIdEnumDefiningTypes)
        {
            string result = ServiceRecordUtilities.Dump(record, attributeIdEnumDefiningTypes);
            Assert.AreEqual(expected, result);
        }

        internal static void DoTestSmart_NotSkip(String expected, byte[] recordBytes, params Type[] attributeIdEnumDefiningTypes)
        {
            ServiceRecordParser parser = new ServiceRecordParser();
            parser.SkipUnhandledElementTypes = false;
            ServiceRecord record = parser.Parse(recordBytes);
            //
            string result = ServiceRecordUtilities.Dump(record, attributeIdEnumDefiningTypes);
            Assert.AreEqual(expected, result);
        }

        internal static void DoTestSmart_RecordStaticMethod(String expected, byte[] recordBytes, params Type[] attributeIdEnumDefiningTypes)
        {
            ServiceRecord record = ServiceRecord.CreateServiceRecordFromBytes(recordBytes);
            //
            string result = ServiceRecordUtilities.Dump(record, attributeIdEnumDefiningTypes);
            Assert.AreEqual(expected, result);
        }

        internal static void DoTestRaw(String expected, byte[] recordBytes)
        {
            ServiceRecordParser parser = new ServiceRecordParser();
            parser.SkipUnhandledElementTypes = true;
            ServiceRecord record = parser.Parse(recordBytes);
            //
            string result = ServiceRecordUtilities.DumpRaw(record);
            Assert.AreEqual(expected, result);
        }


        [Test]
        public void UnsupportedCharacterEncoding()
        {
            DoTestSmart(Data_CompleteThirdPartyRecords.UnsupportedCharacterEncodingDump,
                Data_CompleteThirdPartyRecords.UnsupportedCharacterEncoding,
                typeof(ServiceDiscoveryServerAttributeId));
        }

        [Test]
        public void Xp1()
        {
            DoTestSmart(Data_CompleteThirdPartyRecords.Xp1Dump, Data_CompleteThirdPartyRecords.Xp1Sdp,
                typeof(ServiceDiscoveryServerAttributeId));
        }

        [Test]
        public void Xp1WithNullForEnums()
        {
            DoTestSmart(Data_CompleteThirdPartyRecords.Xp1DumpWithNullForEnums,
                Data_CompleteThirdPartyRecords.Xp1Sdp,
                null);
        }

        [Test]
        public void Xp1Raw()
        {
            DoTestRaw(Data_CompleteThirdPartyRecords.Xp1DumpRaw, Data_CompleteThirdPartyRecords.Xp1Sdp);
        }

        [Test]
        public void XpFsquirtOpp()
        {
            DoTestSmart(Data_CompleteThirdPartyRecords.XpFsquirtOpp_Dump, Data_CompleteThirdPartyRecords.XpFsquirtOpp,
                typeof(ServiceDiscoveryServerAttributeId));
        }

        [Test]
        public void PalmOsOpp()
        {
            DoTestSmart_RecordStaticMethod(Data_CompleteThirdPartyRecords.PalmOsOppDump, Data_CompleteThirdPartyRecords.PalmOsOpp,
                typeof(ServiceDiscoveryServerAttributeId));
        }

        [Test]
        public void LogitechF0228A_Headset()
        {
            DoTestSmart(Data_CompleteThirdPartyRecords.LogitechF0228A_Headset_Dump, Data_CompleteThirdPartyRecords.LogitechF0228A_Headset);
        }

        [Test]
        public void LogitechF0228A_Handsfree()
        {
            DoTestSmart_RecordStaticMethod(Data_CompleteThirdPartyRecords.LogitechF0228A_Handsfree_Dump, Data_CompleteThirdPartyRecords.LogitechF0228A_Handsfree);
        }

        [Test]
        public void BluetoothListener_DefaultRecord_ChatSample()
        {
            DoTestSmart(Data_CompleteThirdPartyRecords.BluetoothListener_DefaultRecord_ChatSample_Dump, Data_CompleteThirdPartyRecords.BluetoothListener_DefaultRecord_ChatSample);
        }
        //[Test]
        //public void GuidByteOrdering()
        //{
        //    Guid aa = new Guid("{00102030-4050-6070-8090-a0b0c0d0e0f0}");
        //    Console.WriteLine("aa: " + aa);
        //    byte[] aaBytes = aa.ToByteArray();
        //    Console.WriteLine("aa byteArray: " + BitConverter.ToString(aaBytes));
        //    //--
        //    Guid bb = new Guid(aaBytes);
        //    Console.WriteLine("bb: " + bb);
        //    byte[] bbBytes = bb.ToByteArray();
        //    Console.WriteLine("bb byteArray: " + BitConverter.ToString(bbBytes));
        //    //--
        //    System.IO.BinaryReader rdr = new System.IO.BinaryReader(
        //        new System.IO.MemoryStream(aaBytes, false));
        //    Guid cc = new Guid(rdr.ReadInt32(), rdr.ReadInt16(), rdr.ReadInt16(), rdr.ReadByte(), rdr.ReadByte(),
        //        rdr.ReadByte(), rdr.ReadByte(), rdr.ReadByte(), rdr.ReadByte(), rdr.ReadByte(), rdr.ReadByte());
        //    Console.WriteLine("cc: " + cc);
        //    byte[] ccBytes = cc.ToByteArray();
        //    Console.WriteLine("cc byteArray: " + BitConverter.ToString(ccBytes));
        //}

        [Test]
        public void XpB_0of2Sdp()
        {
            DoTestSmart(Data_CompleteThirdPartyRecords.XpB_0of2Sdp_Dump, Data_CompleteThirdPartyRecords.XpB_0of2Sdp,
                typeof(ServiceDiscoveryServerAttributeId));
        }

        [Test]
        public void XpB_1of2_1115()
        {
            DoTestSmart(Data_CompleteThirdPartyRecords.XpB_1of2_1115_Dump, Data_CompleteThirdPartyRecords.XpB_1of2_1115
                /*typeof(ServiceDiscoveryServerAttributeId)*/);
        }

        [Test]
        public void SonyEricsson_Hid_Record()
        {
            // This should the normal test of binary record to dump, but we don't have the binary...
            ServiceRecord rcd = Data_CompleteThirdPartyRecords.SonyEricsson_Hid_Record;
            String expectedDump = Data_CompleteThirdPartyRecords.SonyEricsson_Hid_Record_Dump;
            String dump = ServiceRecordUtilities.Dump(rcd);
            Assert.AreEqual(expectedDump, dump, "dump");

            //DoTest(/*!!!SonyEricsson_Hid_Record*/InTheHand.Net.Tests.Sdp2.Data_CompleteThirdPartyRecords.BluetoothListener_DefaultRecord_ChatSample,
            //    Data_SdpCreator_CompleteRecords.SonyEricsson_Hid_Record);
        }

        [Test]
        public void SonyEricssonMv100_Imaging_hasUint64()
        {
            DoTestSmart(Data_CompleteThirdPartyRecords.SonyEricssonMv100_Imaging_hasUint64_Dump, Data_CompleteThirdPartyRecords.SonyEricssonMv100_Imaging_hasUint64);
        }

        //--------------------------------------------------------------
        //[Test]
        //public void KingSt_d2_DumpRaw_all()
        //{
        //    ServiceRecordParser parser = new ServiceRecordParser();
        //    parser.SkipUnhandledElementTypes = true;
        //    byte[][] input = { 
        //        Data_CompleteThirdPartyRecords.KingSt_d2r1,
        //        Data_CompleteThirdPartyRecords.KingSt_d2r2,
        //        Data_CompleteThirdPartyRecords.KingSt_d2r3,
        //        Data_CompleteThirdPartyRecords.KingSt_d2r4,
        //        Data_CompleteThirdPartyRecords.KingSt_d2r5,
        //        Data_CompleteThirdPartyRecords.KingSt_d2r6,
        //        Data_CompleteThirdPartyRecords.KingSt_d2r1_withPdlUuid128s,
        //        Data_CompleteThirdPartyRecords.KingSt_d2r1_withPdlUuid128sNonBluetoothBase,
        //    };
        //    foreach (byte[] cur in input) {
        //        ServiceRecord record = parser.Parse(cur);
        //        String dumpR = ServiceRecordUtilities.DumpRaw(record);
        //        Console.WriteLine("----");
        //        Console.WriteLine(dumpR);
        //    }
        //}

        [Test]
        public void KingSt_d2r1_hasPdlUuid32s_Dump()
        {
            DoTestSmart(Data_CompleteThirdPartyRecords.KingSt_d2r1_Dump, Data_CompleteThirdPartyRecords.KingSt_d2r1);
        }

        [Test]
        public void KingSt_d2r1_withPdlUuid128s_Dump()
        {
            DoTestSmart(Data_CompleteThirdPartyRecords.KingSt_d2r1_withPdlUuid128s_Dump, Data_CompleteThirdPartyRecords.KingSt_d2r1_withPdlUuid128s);
        }

        //[Test]
        //public void KingSt_d2_Dump_all()
        //{
        //    ServiceRecordParser parser = new ServiceRecordParser();
        //    parser.SkipUnhandledElementTypes = true;
        //    byte[][] input = { 
        //        Data_CompleteThirdPartyRecords.KingSt_d2r1,
        //        Data_CompleteThirdPartyRecords.KingSt_d2r2,
        //        Data_CompleteThirdPartyRecords.KingSt_d2r3,
        //        Data_CompleteThirdPartyRecords.KingSt_d2r4,
        //        Data_CompleteThirdPartyRecords.KingSt_d2r5,
        //        Data_CompleteThirdPartyRecords.KingSt_d2r6,
        //        Data_CompleteThirdPartyRecords.KingSt_d2r1_withPdlUuid128s,
        //        Data_CompleteThirdPartyRecords.KingSt_d2r1_withPdlUuid128sNonBluetoothBase,
        //    };
        //    foreach (byte[] cur in input) {
        //        ServiceRecord record = parser.Parse(cur);
        //        String dumpR = ServiceRecordUtilities.Dump(record);
        //        Console.WriteLine("----");
        //        Console.WriteLine(dumpR);
        //    }
        //}

        [Test]
        public void SemcHla_Dump()
        {
            DoTestSmart(Data_CompleteThirdPartyRecords.SemcHla_Dump,
                Data_CompleteThirdPartyRecords.SemcHla);
        }

        [Test]
        public void BenqE72ImagingResponder_Dump()
        {
            DoTestSmart_NotSkip(Data_CompleteThirdPartyRecords.BenqE72ImagingResponder_Dump,
                Data_CompleteThirdPartyRecords.BenqE72ImagingResponder);
        }

        [Test]
        public void BppDirectPrinting_TheMajor()
        {
            DoTestSmart_NotSkip(Data_CompleteThirdPartyRecords.BppDirectPrinting_TheMajor_Dump,
                Data_CompleteThirdPartyRecords.BppDirectPrinting_TheMajor);
        }

    }//class


    [TestFixture]
    public class DumpMiscRecords
    {
        public const string CrLf = "\r\n";

        public static readonly byte[] OneNilBytes = {
            0x35, 4,
                0x09,0x12,0x34,
                0x00,
        };

        public static readonly byte[] OneNilBytes_AttrIdTopBitSet = {
            0x35, 4,
                0x09,0x92,0x34,
                0x00,
        };

        public static readonly byte[] OneUnknownTypeBytes = {
            0x35, 6,
                0x09,0x12,0x34,
                0x91,0x01,0x02
        };

        public static readonly byte[] BadUtf8StringBytes = {
            0x35, 9,
                0x09,0x12,0x34,
                0x25,4,
                    0xFF,0x61,0x62,0xFF,
        };

        public static readonly byte[] ProtocolDescriptorListAlternativesBytes = {
            0x35, 39,
                0x09,0x00,0x04, // ProtocolDescriptorList
                0x3D, 34,   //Alternative
                    0x35, 15,
                        0x35, 6, 
                            0x19,0x01,0x00, 0x09,0x10,0x01, // uuid16 0x0100, uint16 0x1001
                        0x35, 5,
                            0x19,0x00,0x03, 0x08,0x10,                            
                    0x35, 15,
                        0x35, 6, 
                            0x19,0x01,0x00, 0x09,0x10,0x02, // uuid16 0x0100, uint16 0x1002
                        0x35, 5,
                            0x19,0x90,0x03, 0x08,0x10,                            
        };
        public const string ProtocolDescriptorListAlternatives_Dump
            = "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementAlternative" + CrLf
            + "    ElementSequence" + CrLf
            + "        ElementSequence" + CrLf
            + "            Uuid16: 0x100" + " -- L2CapProtocol" + CrLf
            + "            UInt16: 0x1001" + CrLf
            + "        ElementSequence" + CrLf
            + "            Uuid16: 0x3" + " -- RFCommProtocol" + CrLf
            + "            UInt8: 0x10" + CrLf
            + "    ElementSequence" + CrLf
            + "        ElementSequence" + CrLf
            + "            Uuid16: 0x100" + " -- L2CapProtocol" + CrLf
            + "            UInt16: 0x1002" + CrLf
            + "        ElementSequence" + CrLf
            + "            Uuid16: 0x9003" + CrLf
            + "            UInt8: 0x10" + CrLf
            + "    ( ( L2Cap, PSM=0x1001 ), ( Rfcomm, ChannelNumber=16 ) )" + CrLf
            + "    ( ( L2Cap, PSM=0x1002 ), ( 0x9003, ... ) )" + CrLf;

        public static readonly byte[] OneUuid32Bytes = {
            0x35, 8,
                0x09,0x12,0x34,
                0x1a,0xFF,0x23,0x40,0x01
        };


        //----
        public static readonly byte[] PdlUuid16AlsoTopBitSet_Bytes ={
            0x35, 0x14,
                0x09, 0x00, 0x04,
                0x35, 0x0f, 
                    0x35, 0x06, 
                        0x19, 0x01, 0x00, 
                        0x09, 0xf0, 0xf9, 
                    0x35, 0x05, 
                        0x19, 0xfe, 0xba,
                        0x8, 0x18
        };
        public static readonly byte[] PdlUuid32AlsoTopBitSet_Bytes ={
            0x35, 0x1f,
                0x09, 0x00, 0x04,
                0x35, 0x1a, 
                    0x35, 0x08, 
                        0x1a, 0x00, 0x00, 0x01, 0x00, 
                        0x09, 0xf0, 0xf9, 
                    0x35, 0x07, 
                        0x1a, 0x10, 0x00, 0xfe, 0xba,
                        0x8, 0x17,
                    0x35, 0x05, 
                        0x1a, 0xfe, 0x00, 0x01, 0xba,
        };
        public static readonly byte[] PdlUuid128_Bytes ={
            0x35, 0x43,
                0x09, 0x00, 0x04,
                0x35, 0x3e, 
                    0x35, 0x14, 
                        0x1c, 0x00, 0x00, 0x01, 0x00, 
                            0x00, 0x00, 0x10, 0x00, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB,
                        0x09, 0xf0, 0xf9, 
                    0x35, 0x13, 
                        0x1c, 0x10, 0x00, 0xfe, 0xba,
                            0x00, 0x00, 0x10, 0x00, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB,
                        0x8, 0x17,
                    0x35, 0x11, 
                        0x1c, 0xfe, 0x00, 0x01, 0xba,
                            0x00, 0x00, 0x10, 0x00, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB,
        };
        const string PdlUuid16AlsoTopBitSet_Dump
            = "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100" + " -- L2CapProtocol" + CrLf
            + "        UInt16: 0xF0F9" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0xFEBA" + CrLf
            + "        UInt8: 0x18" + CrLf
            + "( ( L2Cap, PSM=0xF0F9 ), ( 0xFEBA, ... ) )" + CrLf;
        const string PdlUuid32AlsoTopBitSet_Dump
            = "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid32: 0x100" + " -- L2CapProtocol" + CrLf
            + "        UInt16: 0xF0F9" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid32: 0x1000FEBA" + CrLf
            + "        UInt8: 0x17" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid32: 0xFE0001BA" + CrLf
            + "( ( L2Cap, PSM=0xF0F9 ), ( 1000feba-0000-1000-8000-00805f9b34fb, ... ),"
                + " ( fe0001ba-0000-1000-8000-00805f9b34fb ) )" + CrLf;
        const string PdlUuid128_Dump
            = "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid128: 00000100-0000-1000-8000-00805f9b34fb" + " -- L2CapProtocol" + CrLf
            + "        UInt16: 0xF0F9" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid128: 1000feba-0000-1000-8000-00805f9b34fb" + CrLf
            + "        UInt8: 0x17" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid128: fe0001ba-0000-1000-8000-00805f9b34fb" + CrLf
            + "( ( L2Cap, PSM=0xF0F9 ), ( 1000feba-0000-1000-8000-00805f9b34fb, ... ),"
                + " ( fe0001ba-0000-1000-8000-00805f9b34fb ) )" + CrLf;
        const string PdlUuid128_DumpRaw
            = "AttrId: 0x0004" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid128: 00000100-0000-1000-8000-00805f9b34fb"  + CrLf
            + "        UInt16: 0xF0F9" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid128: 1000feba-0000-1000-8000-00805f9b34fb" + CrLf
            + "        UInt8: 0x17" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid128: fe0001ba-0000-1000-8000-00805f9b34fb" + CrLf
            ;
        const string PdlTcsBinCordless_Dump
            = "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100" + " -- L2CapProtocol" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x5" + " -- TcsBinProtocol" + CrLf
            + "( ( L2Cap ), ( TcsBin ) )" + CrLf
            ;
        const string PdlHid_Dump
            = "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100" + " -- L2CapProtocol" + CrLf
            + "        UInt16: 0x69" + "" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x11" + " -- HidpProtocol" + CrLf
            + "( ( L2Cap, PSM=0x69 ), ( Hidp ) )" + CrLf
            ;
        const string PdlNonHackProtocolId_Dump
            = "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100 -- L2CapProtocol" + CrLf
            + "        UInt16: 0x17" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x17 -- AvctpProtocol" + CrLf
            + "        UInt16: 0x103" + CrLf
            + "( ( L2Cap, PSM=0x17 ), ( Avctp, ... ) )" + CrLf
            ;

        [Test]
        public void PdlUuid16AlsoTopBitSet()
        {
            DumpCompleteThirdPartyRecords.DoTestSmart(PdlUuid16AlsoTopBitSet_Dump, PdlUuid16AlsoTopBitSet_Bytes);
        }
        [Test]
        public void PdlUuid32AlsoTopBitSet()
        {
            DumpCompleteThirdPartyRecords.DoTestSmart(PdlUuid32AlsoTopBitSet_Dump, PdlUuid32AlsoTopBitSet_Bytes);
        }
        [Test]
        public void PdlUuid128()
        {
            DumpCompleteThirdPartyRecords.DoTestSmart(PdlUuid128_Dump, PdlUuid128_Bytes);
        }
        [Test]
        public void PdlUuid128_Raw()
        {
            DumpCompleteThirdPartyRecords.DoTestRaw(PdlUuid128_DumpRaw, PdlUuid128_Bytes);
        }

        [Test]
        public void PdlUndefinedHackProtoId()
        {
            // Protocol Descriptor List
            //    Protocol #0 UUID L2CAP
            //    Protocol #1 UUID TCS-BIN-CORDLESS
            const UInt16 SvcClass16ProtocolL2CAP = 0x000000100;
            const UInt16 SvcClass16ProtocolTcsBinCordless = 0x00000005;
            ServiceRecord record = new ServiceRecord(
                new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList,
                    new ServiceElement(ElementType.ElementSequence,
                        new ServiceElement(ElementType.ElementSequence,
                            new ServiceElement(ElementType.Uuid16, SvcClass16ProtocolL2CAP)),
                        new ServiceElement(ElementType.ElementSequence,
                            new ServiceElement(ElementType.Uuid16, SvcClass16ProtocolTcsBinCordless)))
                ));
            DumpCompleteThirdPartyRecords.DoTestSmart(PdlTcsBinCordless_Dump, record);
        }

        [Test]
        public void PdlDefinedButUnhandledHackProtoId()
        {
            //Protocol Descriptor List                        0x0004          M
            //Protocol Descriptor #0          Data Element Sequence           M
            //    Protocol ID                 L2CAP   UUID    L2CAP, Note 1   M
            //    Parameter #0                PSM     Uint16  HID_Control     M
            //Protocol Descriptor #1          Data Element Sequence           M
            //    ProtocolID                  HID     UUID    HID Protocol, Note 1    M
            const UInt16 SvcClass16ProtocolL2CAP = 0x000000100;
            const UInt16 SvcClass16ProtocolHid = 0x00000011; //BluetoothService.HidpProtocol
            const UInt16 PsmHidp = 0x69;
            ServiceRecord record = new ServiceRecord(
                new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList,
                    new ServiceElement(ElementType.ElementSequence,
                        new ServiceElement(ElementType.ElementSequence,
                            new ServiceElement(ElementType.Uuid16, SvcClass16ProtocolL2CAP),
                            new ServiceElement(ElementType.UInt16, PsmHidp)),
                        new ServiceElement(ElementType.ElementSequence,
                            new ServiceElement(ElementType.Uuid16, SvcClass16ProtocolHid))
                )));
            DumpCompleteThirdPartyRecords.DoTestSmart(PdlHid_Dump, record);
        }
        [Test]
        public void PdlNonHackProtocolId_Avctp()
        {
            //const UInt16 SvcClass16Avcrp = 0x110E;
            const UInt16 SvcClass16AvcrpController_ = 0x110F;
            Guid SvcClass128AvcrpController = BluetoothService.CreateBluetoothUuid(SvcClass16AvcrpController_);
            //
            const UInt16 SvcClass16ProtocolL2CAP = 0x0100;
            //Debug_Assert_CorrectShortUuid(SvcClass16ProtocolL2CAP, BluetoothService.L2CapProtocol);
            const UInt16 SvcClass16ProtocolAvctp = 0x0017;
            //Debug_Assert_CorrectShortUuid(SvcClass16ProtocolAvctp, BluetoothService.AvctpProtocol);
            //
            const UInt16 PsmAvcrp = 0x0017;
            //const UInt16 PsmAvcrpBrowsing = 0x001B;
            //
            const UInt16 Version = 0x0103; // 1.3.......
            //
            var layer0 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, SvcClass16ProtocolL2CAP),
                new ServiceElement(ElementType.UInt16, PsmAvcrp));
            var layer1 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, SvcClass16ProtocolAvctp),
                new ServiceElement(ElementType.UInt16, Version));
            ServiceElement pdl = new ServiceElement(ElementType.ElementSequence,
                layer0, layer1);

            ServiceRecord record = new ServiceRecord(
                new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList, pdl));
            DumpCompleteThirdPartyRecords.DoTestSmart(PdlNonHackProtocolId_Dump, record);
        }
        //--------

        public static readonly byte[] DeepString_BadUtf8StringBytes = {
            0x35, 9+2,
                0x09,0x12,0x34,
                0x35,6,
                0x25,4,
                    0xFF,0x61,0x62,0xFF,
        };

        // 'a\u00F6b\u2020d'
        public static readonly byte[] DeepString_GoodUtf8StringBytes = {
            0x35, 15,
                0x09,0x12,0x34,
                0x35,10,
                    0x25,8,
                        0x61, 0xC3,0xB6, 0x62, 0xE2,0x80,0xA0, 0x64
        };
        public static readonly byte[] DeepString_GoodUtf16BEStringBytes = {
            0x35, 17,
                0x09,0x12,0x34,
                0x35,12,
                    0x25,10,
                        0,0x61, 0,0xF6, 0,0x62, 0x20,0x20, 0,0x64
        };
        public static readonly byte[] DeepString_GoodUtf16LEStringBytes = {
            0x35, 17,
                0x09,0x12,0x34,
                0x35,12,
                    0x25,10,
                        0x61,0, 0xF6,0, 0x62,0, 0x20,0x20, 0x64,0
        };

        // Without the 0xF6, so UTF-16 sees it as valid!!!: 'ab\u2020d'
        public static readonly byte[] DeepString_GoodUtf16BEStringBytes2 = {
            0x35, 15,
                0x09,0x12,0x34,
                0x35,10,
                    0x25,08,
                        0,0x61, 0,0x62, 0x20,0x20, 0,0x64
        };
        public static readonly byte[] DeepString_GoodUtf16LEStringBytes2 = {
            0x35, 15,
                0x09,0x12,0x34,
                0x35,10,
                    0x25,8,
                        0x61,0, 0x62,0, 0x20,0x20, 0x64,0
        };

        //----


        [Test]
        public void OneNil()
        {
            byte[] recordBytes = OneNilBytes;
            const string expectedDump
                = "AttrId: 0x1234" + CrLf
                + "Nil:" + CrLf;
            DumpCompleteThirdPartyRecords.DoTestSmart(expectedDump, recordBytes);
        }

        [Test]
        public void OneNil_Raw()
        {
            byte[] recordBytes = OneNilBytes;
            const string expectedDump
                = "AttrId: 0x1234" + CrLf
                + "Nil:" + CrLf;
            DumpCompleteThirdPartyRecords.DoTestRaw(expectedDump, recordBytes);
        }

        [Test]
        public void AttrIdTopBitSet()
        {
            byte[] recordBytes = OneNilBytes_AttrIdTopBitSet;
            const string expectedDump
                = "AttrId: 0x9234" + CrLf
                + "Nil:" + CrLf;
            DumpCompleteThirdPartyRecords.DoTestSmart(expectedDump, recordBytes);
        }

        [Test]
        public void AttrIdTopBitSet_Raw()
        {
            byte[] recordBytes = OneNilBytes_AttrIdTopBitSet;
            const string expectedDump
                = "AttrId: 0x9234" + CrLf
                + "Nil:" + CrLf;
            DumpCompleteThirdPartyRecords.DoTestRaw(expectedDump, recordBytes);
        }

        [Test]
        public void OneUnknownType()
        {
            byte[] recordBytes = OneUnknownTypeBytes;
            const string expectedDump 
                = "AttrId: 0x1234" + CrLf
                + "Unknown: unknown" + CrLf;
            DumpCompleteThirdPartyRecords.DoTestSmart(expectedDump, recordBytes);
        }

        [Test]
        [ExpectedException(typeof(NotImplementedException), ExpectedMessage = "Element type: 18, SizeIndex: LengthTwoBytes, at offset: 5.")]
        public void OneUnknownType_Strict()
        {
            byte[] recordBytes = OneUnknownTypeBytes;
            ServiceRecordParser parser = new ServiceRecordParser();
            parser.SkipUnhandledElementTypes = false;
            parser.Parse(recordBytes);
        }

        [Test]
        public void BadUtf8String()
        {
            const string expectedDump
                = "AttrId: 0x1234" + CrLf
                + "TextString (Unknown/bad encoding):" + CrLf
                + "Length: 4, >>FF-61-62-FF<<" + CrLf;
            DumpCompleteThirdPartyRecords.DoTestSmart(expectedDump, BadUtf8StringBytes);
        }

        [Test]
        public void ProtocolDescriptorListAlternatives()
        {
            DumpCompleteThirdPartyRecords.DoTestSmart(ProtocolDescriptorListAlternatives_Dump, ProtocolDescriptorListAlternativesBytes);
        }

        [Test]
        public void OneUuid32()
        {
            byte[] recordBytes = OneUuid32Bytes;
            const string expectedDump 
                = "AttrId: 0x1234" + CrLf
                + "Uuid32: 0xFF234001" + CrLf;
            DumpCompleteThirdPartyRecords.DoTestSmart(expectedDump, recordBytes);
        }

        [Test]
        public void OneUInt128()
        {
            byte[] recordBytes = Data_SimpleRecords.OneUInt128_F123_E987;
            const string expectedDump
                = "AttrId: 0xF123" + CrLf
                + "UInt128: E9-87-03-04-05-06-07-08-09-0A-0B-0C-0D-0E-0F-10" + CrLf;
            DumpCompleteThirdPartyRecords.DoTestSmart(expectedDump, recordBytes);
        }
        [Test]
        public void OneInt128()
        {
            byte[] recordBytes = Data_SimpleRecords.OneInt128_F123_E987;
            const string expectedDump
                = "AttrId: 0xF123" + CrLf
                + "Int128: E9-87-03-04-05-06-07-08-09-0A-0B-0C-0D-0E-0F-10" + CrLf;
            DumpCompleteThirdPartyRecords.DoTestSmart(expectedDump, recordBytes);
        }
        [Test]
        public void OneUInt128_Raw()
        {
            byte[] recordBytes = Data_SimpleRecords.OneUInt128_F123_E987;
            const string expectedDump
                = "AttrId: 0xF123" + CrLf
                + "UInt128: E9-87-03-04-05-06-07-08-09-0A-0B-0C-0D-0E-0F-10" + CrLf;
            DumpCompleteThirdPartyRecords.DoTestRaw(expectedDump, recordBytes);
        }
        [Test]
        public void OneInt128_Raw()
        {
            byte[] recordBytes = Data_SimpleRecords.OneInt128_F123_E987;
            const string expectedDump
                = "AttrId: 0xF123" + CrLf
                + "Int128: E9-87-03-04-05-06-07-08-09-0A-0B-0C-0D-0E-0F-10" + CrLf;
            DumpCompleteThirdPartyRecords.DoTestRaw(expectedDump, recordBytes);
        }

        //--------
        [Test]
        public void DeepString_BadUtf8String()
        {
            const string expectedDump
                = "AttrId: 0x1234" + CrLf
                + "ElementSequence" + CrLf
                + "    TextString (Unknown/bad encoding):" + CrLf
                + "    Length: 4, >>FF-61-62-FF<<" + CrLf;
            DumpCompleteThirdPartyRecords.DoTestSmart(expectedDump, DeepString_BadUtf8StringBytes);
        }

        const string ExpectedDump_DeepString
            = "AttrId: 0x1234" + CrLf
            + "ElementSequence" + CrLf
            + "    TextString (guessing UTF-8): 'a\u00F6b\u2020d'" + CrLf;
        const string ExpectedDump_DeepStringBE_SeenAsBadEncoding
            = "AttrId: 0x1234" + CrLf
            + "ElementSequence" + CrLf
            + "    TextString (Unknown/bad encoding):" + CrLf
            + "    Length: 10, >>00-61-00-F6-00-62-20-20-00-64<<" + CrLf;
        const string ExpectedDump_DeepStringLE_SeenAsBadEncoding
            = "AttrId: 0x1234" + CrLf
            + "ElementSequence" + CrLf
            + "    TextString (Unknown/bad encoding):" + CrLf
            + "    Length: 10, >>61-00-F6-00-62-00-20-20-64-00<<" + CrLf;
        const string ExpectedDump_DeepString2BE_SeenAsBadEncoding
            = "AttrId: 0x1234" + CrLf
            + "ElementSequence" + CrLf
            + "    TextString (Unknown/bad encoding):" + CrLf
            + "    Length: 8, >>00-61-00-62-20-20-00-64<<" + CrLf;
        const string ExpectedDump_DeepString2LE_SeenAsBadEncoding
            = "AttrId: 0x1234" + CrLf
            + "ElementSequence" + CrLf
            + "    TextString (Unknown/bad encoding):" + CrLf
            + "    Length: 8, >>61-00-62-00-20-20-64-00<<" + CrLf;
        // Do we *really* want to produce string containing nulls!!!
        const string ExpectedDump_DeepString2_AsBE
            = "AttrId: 0x1234" + CrLf
            + "ElementSequence" + CrLf
            + "    TextString (guessing UTF-8): '\u0000a\u0000b  \u0000d'" + CrLf;
        const string ExpectedDump_DeepString2_AsLE
            = "AttrId: 0x1234" + CrLf
            + "ElementSequence" + CrLf
            + "    TextString (guessing UTF-8): 'a\u0000b\u0000  d\u0000'" + CrLf;

        [Test]
        public void DeepString_GoodUtf8String()
        {
            DumpCompleteThirdPartyRecords.DoTestSmart(ExpectedDump_DeepString, DeepString_GoodUtf8StringBytes);
        }
        [Test]
        public void DeepString_GoodUtf16BEString()
        {
            DumpCompleteThirdPartyRecords.DoTestSmart(ExpectedDump_DeepStringBE_SeenAsBadEncoding, DeepString_GoodUtf16BEStringBytes);
        }
        [Test]
        public void DeepString_GoodUtf16LEString()
        {
            DumpCompleteThirdPartyRecords.DoTestSmart(ExpectedDump_DeepStringLE_SeenAsBadEncoding, DeepString_GoodUtf16LEStringBytes);
        }

        [Test]
        public void DeepString_GoodUtf16BEString2()
        {
            DumpCompleteThirdPartyRecords.DoTestSmart(ExpectedDump_DeepString2BE_SeenAsBadEncoding, DeepString_GoodUtf16BEStringBytes2);
        }
        [Test]
        public void DeepString_GoodUtf16LEString2()
        {
            DumpCompleteThirdPartyRecords.DoTestSmart(ExpectedDump_DeepString2LE_SeenAsBadEncoding, DeepString_GoodUtf16LEStringBytes2);
        }

        //----
        [Test] // Copied from ServiceRecordBuilderTests.
        public void CustomTwoFromOneWithHighAttrIdAdded()
        {
            // Rfcomm/StdSvcClass/SvcName
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.SerialPort);
            bldr.ServiceName = "Hello World!";
            ServiceAttribute attr2 = new ServiceAttribute(
                0x8000,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 0x80));
            bldr.AddCustomAttribute(attr2);
            ServiceAttribute attr2b = new ServiceAttribute(
                0xFFFF,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 255));
            bldr.AddCustomAttribute(attr2b);
            ServiceAttribute attr = new ServiceAttribute(
                UniversalAttributeId.ServiceAvailability,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 8));
            bldr.AddCustomAttribute(attr);
            DumpCompleteThirdPartyRecords.DoTestSmart(ServiceRecordBuilderTests_Data.CustomTwoFromOneWithHighAttrIdAdded, bldr.ServiceRecord);
        }



    }//class

}
