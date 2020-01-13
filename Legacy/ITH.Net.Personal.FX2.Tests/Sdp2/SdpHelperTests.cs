using System;
#if FX1_1
using IList_ServiceElement = System.Collections.IList;
using List_ServiceElement = System.Collections.ArrayList;
using IList_ServiceAttribute = System.Collections.IList;
using List_ServiceAttribute = System.Collections.ArrayList;
#else
using IList_ServiceElement = System.Collections.Generic.IList<InTheHand.Net.Bluetooth.ServiceElement>;
using List_ServiceElement = System.Collections.Generic.List<InTheHand.Net.Bluetooth.ServiceElement>;
using IList_ServiceAttribute = System.Collections.Generic.IList<InTheHand.Net.Bluetooth.ServiceAttribute>;
using List_ServiceAttribute = System.Collections.Generic.List<InTheHand.Net.Bluetooth.ServiceAttribute>;
#endif
using System.Text;
using NUnit.Framework;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Tests.Sdp2;
using InTheHand.Net.Bluetooth.AttributeIds;

namespace InTheHand.Net.Tests.Sdp2
{

    [TestFixture]
    public class SdpHelperTests
    {
        const UInt16 Uuid16_L2CapProto = checked((UInt16)0x0100);
        const UInt16 Uuid16_RfcommProto = checked((UInt16)0x0003);
        const UInt16 Uuid16_BnepProto = checked((UInt16)0x000F);
        const UInt16 Uuid16_Wierdo = checked((UInt16)0x9999);

        [Test]
        public void GetChannelElement_AttrNotExists()
        {
            ServiceRecord rcd = new ServiceRecord();
            ServiceElement element = ServiceRecordHelper.GetRfcommChannelElement(rcd);
            Assert.IsNull(element);
        }

        [Test]
        public void GetChannelByte_AttrNotExists()
        {
            ServiceRecord rcd = new ServiceRecord();
            int value = ServiceRecordHelper.GetRfcommChannelNumber(rcd);
            Assert.AreEqual(-1, value);
        }

        [Test]
        public void GetChannelElement_Opp()
        {
            ServiceRecord rcd = ServiceRecord.CreateServiceRecordFromBytes(Data_CompleteThirdPartyRecords.PalmOsOpp);
            ServiceElement element = ServiceRecordHelper.GetRfcommChannelElement(rcd);
            Assert.IsNotNull(element);
            Assert.AreEqual(ElementType.UInt8, element.ElementType);
            Assert.AreEqual(Data_CompleteThirdPartyRecords.PalmOsOpp_RfcommChannelNumber, element.Value);
        }

        [Test]
        public void GetChannelByte_Opp()
        {
            ServiceRecord rcd = ServiceRecord.CreateServiceRecordFromBytes(Data_CompleteThirdPartyRecords.PalmOsOpp);
            int value = ServiceRecordHelper.GetRfcommChannelNumber(rcd);
            Assert.AreNotEqual(-1, value);
            Assert.AreEqual(Data_CompleteThirdPartyRecords.PalmOsOpp_RfcommChannelNumber, (byte)value);
        }

        [Test]
        public void GetChannelElement_SdpNone()
        {
            ServiceRecord rcd = ServiceRecord.CreateServiceRecordFromBytes(Data_CompleteThirdPartyRecords.XpB_0of2Sdp);
            ServiceElement element = ServiceRecordHelper.GetRfcommChannelElement(rcd);
            Assert.IsNull(element);
        }

        [Test]
        public void GetChannelByte_SdpNone()
        {
            ServiceRecord rcd = ServiceRecord.CreateServiceRecordFromBytes(Data_CompleteThirdPartyRecords.XpB_0of2Sdp);
            int value = ServiceRecordHelper.GetRfcommChannelNumber(rcd);
            Assert.AreEqual(-1, value);
        }

        [Test]
        public void GetChannelByte_PdlHasUuid32s()
        {
            ServiceRecord rcd = ServiceRecord.CreateServiceRecordFromBytes(Data_CompleteThirdPartyRecords.KingSt_d2r1);
            int value = ServiceRecordHelper.GetRfcommChannelNumber(rcd);
            Assert.AreEqual(2, value);
        }

        [Test]
        public void GetChannelByte_PdlHasUuid128s()
        {
            ServiceRecord rcd = ServiceRecord.CreateServiceRecordFromBytes(Data_CompleteThirdPartyRecords.KingSt_d2r1_withPdlUuid128s);
            int value = ServiceRecordHelper.GetRfcommChannelNumber(rcd);
            Assert.AreEqual(2, value);
        }

        [Test]
        public void GetChannelByte_Bad_PdlNotList()
        {
            ServiceRecord rcd = new ServiceRecord(
                new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList,
                    new ServiceElement(ElementType.TextString, "xx")));
            int value = ServiceRecordHelper.GetRfcommChannelNumber(rcd);
            Assert.AreEqual(-1, value);
        }

        [Test]
        public void GetChannelByte_Bad_PdlEmptyList()
        {
            ServiceRecord rcd = new ServiceRecord(
                new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList,
                    new ServiceElement(ElementType.ElementSequence)));
            int value = ServiceRecordHelper.GetRfcommChannelNumber(rcd);
            Assert.AreEqual(-1, value);
        }

        [Test]
        public void GetChannelByte_Bad_PdlFirstNotL2CAP()
        {
            var layer0Bad = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, Uuid16_Wierdo)); // not L2CAP
            var layer1 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, Uuid16_RfcommProto),
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 5)); // RFCOMM
            ServiceRecord rcd = new ServiceRecord(
                new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList,
                    new ServiceElement(ElementType.ElementSequence,
                        layer0Bad, layer1)));
            int value = ServiceRecordHelper.GetRfcommChannelNumber(rcd);
            Assert.AreEqual(-1, value);
        }

        [Test]
        public void GetChannelByte_Bad_TruncatedBeforeRfcomm()
        {
            var layer0 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, Uuid16_L2CapProto)); // not L2CAP
            ServiceRecord rcd = new ServiceRecord(
                new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList,
                    new ServiceElement(ElementType.ElementSequence,
                        layer0)));
            int value = ServiceRecordHelper.GetRfcommChannelNumber(rcd);
            Assert.AreEqual(-1, value);
        }

        [Test]
        public void GetChannelByte_Bad_RfcommNoPortElement()
        {
            var layer0 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, Uuid16_L2CapProto)); // L2CAP
            var layer1Bad = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, Uuid16_RfcommProto) //RFCOMM
                //NO element(ElementType.UInt8, 5)
                );
            ServiceRecord rcd = new ServiceRecord(
                new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList,
                    new ServiceElement(ElementType.ElementSequence,
                        layer0, layer1Bad)));
            int value = ServiceRecordHelper.GetRfcommChannelNumber(rcd);
            Assert.AreEqual(-1, value);
        }

        [Test]
        public void GetChannelByte_Bad_RfcommPortElementNotUint8()
        {
            var layer0 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, Uuid16_L2CapProto)); // L2CAP
            var layer1Bad = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, Uuid16_RfcommProto), //RFCOMM
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 5));
            ServiceRecord rcd = new ServiceRecord(
                new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList,
                    new ServiceElement(ElementType.ElementSequence,
                        layer0, layer1Bad)));
            int value = ServiceRecordHelper.GetRfcommChannelNumber(rcd);
            Assert.AreEqual(-1, value);
        }

        [Test]
        public void GetChannelByte_Bad_PdlHasAlternates()
        {
            var layer0 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, Uuid16_L2CapProto)); // L2CAP
            var layer1A = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, Uuid16_RfcommProto), //RFCOMM
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 5));
            var layer1B = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, Uuid16_RfcommProto), //RFCOMM
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 15));
            ServiceRecord rcd = new ServiceRecord(
                new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList,
                    new ServiceElement(ElementType.ElementAlternative,
                        new ServiceElement(ElementType.ElementSequence,
                            layer0, layer1A),
                        new ServiceElement(ElementType.ElementSequence,
                            layer0, layer1B)
                            )));
            int value = ServiceRecordHelper.GetRfcommChannelNumber(rcd);
            Assert.AreEqual(-1, value);
        }

        //----
        [Test]
        public void GetChannelElement_BadRequestProtocol()
        {
            var layer0 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, Uuid16_L2CapProto)); // L2CAP
            var layer1 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, Uuid16_RfcommProto), //RFCOMM
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 5)
                ); // RFCOMM
            var attr = new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList,
                new ServiceElement(ElementType.ElementSequence,
                    layer0, layer1));
            bool? flag;
            try {
                var e = ServiceRecordHelper.GetChannelElement(attr, (BluetoothProtocolDescriptorType)0x9999, out flag);
                Assert.Fail("should have thrown!");
            } catch (ArgumentException ex) {
                Assert.AreEqual("Can only fetch RFCOMM or L2CAP element.", ex.Message, "ex.Message");
            }
        }

        //---- L2CAP ----
        [Test]
        public void SetL2CapPort()
        {
            SetL2CapPort_(45, "0x2D", "0x2D");
            SetL2CapPort_(9, "0x9", "0x9");
            SetL2CapPort_(0xFEFF, "0xFEFF", "0xFEFF");
            SetL2CapPort_(0xF, "0xF", "Bnep");
            SetL2CapPort_(3, "0x3", "Rfcomm");
        }

        static void SetL2CapPort_(int psm, string expectedPsmAsString1, string expectedPsmAsString2)
        {
            var layer0 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, Uuid16_L2CapProto),
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 0)
                );
            ServiceRecord rcd = new ServiceRecord(
                new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList,
                    new ServiceElement(ElementType.ElementSequence,
                        layer0)));
            ServiceRecordHelper.SetL2CapPsmNumber(rcd, psm);
            string expected
                = "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
                + "ElementSequence" + CrLf
                + "    ElementSequence" + CrLf
                + "        Uuid16: 0x100 -- L2CapProtocol" + CrLf
                + "        UInt16: " + expectedPsmAsString1 + CrLf
                + "( ( L2Cap, PSM=" + expectedPsmAsString2 + " ) )" + CrLf
                ;
            var dump = ServiceRecordUtilities.Dump(rcd);
            Assert.AreEqual(expected, dump);
        }

        [Test]
        public void SetL2CapPort_Bad_NoPortElement()
        {
            int psm = 9;
            //
            var layer0 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, Uuid16_L2CapProto)
                // NO Element.CreateNumerical(ElementType.UInt16, 0)
                );
            ServiceRecord rcd = new ServiceRecord(
                new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList,
                    new ServiceElement(ElementType.ElementSequence,
                        layer0)));
            try {
                ServiceRecordHelper.SetL2CapPsmNumber(rcd, psm);
                Assert.Fail("should have thrown!");
            } catch (InvalidOperationException ex) {
                Assert.IsInstanceOfType(typeof(InvalidOperationException), ex);
            }
        }

        [Test]
        public void SetL2CapPort_Bad_PortElementNotUint16()
        {
            int psm = 9;
            //
            var layer0 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, Uuid16_L2CapProto),
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt32, 0)
                );
            ServiceRecord rcd = new ServiceRecord(
                new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList,
                    new ServiceElement(ElementType.ElementSequence,
                        layer0)));
            try {
                ServiceRecordHelper.SetL2CapPsmNumber(rcd, psm);
                Assert.Fail("should have thrown!");
            } catch (InvalidOperationException ex) {
                Assert.IsInstanceOfType(typeof(InvalidOperationException), ex);
            }
        }

        //------------------------
        [Test]
        public void L2CapGetChannelElement_None()
        {
            ServiceRecord rcd = ServiceRecord.CreateServiceRecordFromBytes(Data_CompleteThirdPartyRecords.XpFsquirtOpp);
            ServiceElement element = ServiceRecordHelper.GetL2CapChannelElement(rcd);
            Assert.IsNull(element);
        }

        [Test]
        public void L2CapGetChannelByte_None()
        {
            ServiceRecord rcd = ServiceRecord.CreateServiceRecordFromBytes(Data_CompleteThirdPartyRecords.XpFsquirtOpp);
            int value = ServiceRecordHelper.GetL2CapChannelNumber(rcd);
            Assert.AreEqual(-1, value);
        }

        [Test]
        public void L2CapGetChannelByte_0x000F()
        {
            ServiceRecord rcd = ServiceRecord.CreateServiceRecordFromBytes(Data_CompleteThirdPartyRecords.XpB_1of2_1115);
            int value = ServiceRecordHelper.GetL2CapChannelNumber(rcd);
            Assert.AreEqual(0x0F, value);
        }

        [Test]
        public void L2CapGetChannelByte_0x0001()
        {
            ServiceRecord rcd = ServiceRecord.CreateServiceRecordFromBytes(Data_CompleteThirdPartyRecords.Xp1Sdp);
            int value = ServiceRecordHelper.GetL2CapChannelNumber(rcd);
            Assert.AreEqual(0x01, value);
        }

        [Test]
        public void L2CapGetChannelByte_0xF0F9()
        {
            ServiceRecord rcd = ServiceRecord.CreateServiceRecordFromBytes(Data_CompleteThirdPartyRecords.SemcHla);
            int value = ServiceRecordHelper.GetL2CapChannelNumber(rcd);
            Assert.AreEqual(0xF0F9, value);
        }

        [Test]
        public void L2CapGetChannelByte_Bad_L2CapNoPortElement()
        {
            var layer0 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, Uuid16_L2CapProto) // L2CAP
                //NO element(ElementType.UInt16, 0x1001)
                );
            var layer1Bad = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, Uuid16_RfcommProto), //RFCOMM
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 5)
                );
            ServiceRecord rcd = new ServiceRecord(
                new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList,
                    new ServiceElement(ElementType.ElementSequence,
                        layer0, layer1Bad)));
            int value = ServiceRecordHelper.GetL2CapChannelNumber(rcd);
            Assert.AreEqual(-1, value);
        }

        [Test]
        public void L2CapGetChannelByte_Bad_L2CapPortElementNotUint16()
        {
            var layer0 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, Uuid16_L2CapProto),
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 0x1001)
                ); // L2CAP
            var layer1Bad = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, Uuid16_RfcommProto), //RFCOMM
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 5));
            ServiceRecord rcd = new ServiceRecord(
                new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList,
                    new ServiceElement(ElementType.ElementSequence,
                        layer0, layer1Bad)));
            int value = ServiceRecordHelper.GetL2CapChannelNumber(rcd);
            Assert.AreEqual(0x1001, value);
        }


        //------------------------
        public const string CrLf = "\r\n";
        public const string ExpectedRfcommDump
            = "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100 -- L2CapProtocol" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x3 -- RFCommProtocol" + CrLf
            + "        UInt8: 0x0" + CrLf
            + "( ( L2Cap ), ( Rfcomm, ChannelNumber=0 ) )" + CrLf
            ;
        public const string ExpectedGoepDump
            = "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100 -- L2CapProtocol" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x3 -- RFCommProtocol" + CrLf
            + "        UInt8: 0x0" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x8 -- ObexProtocol" + CrLf
            + "( ( L2Cap ), ( Rfcomm, ChannelNumber=0 ), ( Obex ) )" + CrLf
            ;
        public const string ExpectedL2CapDump
            = "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100 -- L2CapProtocol" + CrLf
            + "        UInt16: 0x0" + CrLf
            + "( ( L2Cap, PSM=0x0 ) )" + CrLf
            ;
        public const string ExpectedL2CapNapDump
            = "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100 -- L2CapProtocol" + CrLf
            + "        UInt16: 0x0" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0xF -- BnepProtocol" + CrLf
            + "        UInt16: 0x100" + CrLf // version 1.0
            + "        ElementSequence" + CrLf // List of protocols
            + "            UInt16: 0x800" + CrLf
            + "            UInt16: 0x806" + CrLf
            + "( ( L2Cap, PSM=0x0 ), ( Bnep, ... ) )" + CrLf
            ;
        public const string ExpectedL2CapNapDumpEmptyNetProtoList
            = "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100 -- L2CapProtocol" + CrLf
            + "        UInt16: 0x0" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0xF -- BnepProtocol" + CrLf
            + "        UInt16: 0x100" + CrLf // version 1.0
            + "        ElementSequence" + CrLf // List of protocols
            + "( ( L2Cap, PSM=0x0 ), ( Bnep, ... ) )" + CrLf
            ;

        [Test]
        public void CreateRfcomm()
        {
            ServiceElement element = ServiceRecordHelper.CreateRfcommProtocolDescriptorList();
            //
            List_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList, element));
            ServiceRecord record = new ServiceRecord(attrs);
            String dump = ServiceRecordUtilities.Dump(record);
            Assert.AreEqual(ExpectedRfcommDump, dump, "RFCOMM ProtoDL dump");
        }

        [Test]
        public void CreateGoep()
        {
            ServiceElement element = ServiceRecordHelper.CreateGoepProtocolDescriptorList();
            //
            List_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList, element));
            ServiceRecord record = new ServiceRecord(attrs);
            String dump = ServiceRecordUtilities.Dump(record);
            Assert.AreEqual(ExpectedGoepDump, dump, "GOEP ProtoDL dump");
        }

        [Test]
        public void CreateL2Cap()
        {
            ServiceElement element = ServiceRecordHelper.CreateL2CapProtocolDescriptorList();
            //
            List_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList, element));
            ServiceRecord record = new ServiceRecord(attrs);
            String dump = ServiceRecordUtilities.Dump(record);
            Assert.AreEqual(ExpectedL2CapDump, dump, "L2CAP ProtoDL dump");
        }

        [Test]
        public void CreateL2CapNap()
        {
            var netProtoList = new ServiceElement(ElementType.ElementSequence,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 0x0800),
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 0x0806)
                );
            var layer1 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, Uuid16_BnepProto),
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 0x0100), //v1.0
                netProtoList
                );
            ServiceElement element = ServiceRecordHelper.CreateL2CapProtocolDescriptorListWithUpperLayers(
                layer1);
            //
            List_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList, element));
            ServiceRecord record = new ServiceRecord(attrs);
            String dump = ServiceRecordUtilities.Dump(record);
            Assert.AreEqual(ExpectedL2CapNapDump, dump, "L2CAP NAP ProtoDL dump");
        }

        [Test]
        public void CreateL2CapNapEmptyNetProtoList()
        {
            var netProtoList = new ServiceElement(ElementType.ElementSequence
                );
            var layer1 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, Uuid16_BnepProto),
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 0x0100), //v1.0
                netProtoList
                );
            ServiceElement element = ServiceRecordHelper.CreateL2CapProtocolDescriptorListWithUpperLayers(
                layer1);
            //
            List_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList, element));
            ServiceRecord record = new ServiceRecord(attrs);
            String dump = ServiceRecordUtilities.Dump(record);
            Assert.AreEqual(ExpectedL2CapNapDumpEmptyNetProtoList, dump, "L2CAP NAP ProtoDL dump");
        }

        [Test]
        public void PalmOsOppCompleteRecord()
        {
            ServiceRecord record = CreatePalmOsOppCompleteRecord();
            ServiceRecordHelper.SetRfcommChannelNumber(record, 1);
            byte[] buf = new byte[256];
            int length = new ServiceRecordCreator().CreateServiceRecord(record, buf);
            //ServiceRecordUtilities.Dump(Console.Out, record);
            Assert2.AreEqualBuffers(Data_CompleteThirdPartyRecords.PalmOsOpp_HackMadeFirstLengthOneByteField, buf, length);
        }

        public ServiceRecord CreatePalmOsOppCompleteRecord()
        {
            List_ServiceAttribute attrs = new List_ServiceAttribute();
            ServiceElement element;
            List_ServiceElement list;
            //
            attrs.Add(new ServiceAttribute(UniversalAttributeId.ServiceRecordHandle,
                new ServiceElement(ElementType.UInt32, (UInt32)0x10001)));
            //
            element = new ServiceElement(ElementType.Uuid16, (UInt16)0x1105);
            list = new List_ServiceElement();
            list.Add(element);
            element = new ServiceElement(ElementType.ElementSequence, list);
            attrs.Add(new ServiceAttribute(UniversalAttributeId.ServiceClassIdList, element));
            //
            element = ServiceRecordHelper.CreateGoepProtocolDescriptorList();
            attrs.Add(new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList, element));
            //
            const UInt16 Windows1252EncodingId = 2252;
            LanguageBaseItem[] languages = {
                new LanguageBaseItem("en", Windows1252EncodingId, LanguageBaseItem.PrimaryLanguageBaseAttributeId)
            };
            element = LanguageBaseItem.CreateElementSequenceFromList(languages);
            attrs.Add(new ServiceAttribute(UniversalAttributeId.LanguageBaseAttributeIdList, element));
            //
            // PalmOs really uses Windows-1252, but since the string is ASCII, UTF-8 is equivalent.
            element = new ServiceElement(ElementType.TextString, "OBEX Object Push");
            attrs.Add(new ServiceAttribute(
                ServiceRecord.CreateLanguageBasedAttributeId(UniversalAttributeId.ServiceName,
                                            LanguageBaseItem.PrimaryLanguageBaseAttributeId),
                element));
            //
            list = new List_ServiceElement();
            list.Add(new ServiceElement(ElementType.UInt8, (byte)0x1));
            list.Add(new ServiceElement(ElementType.UInt8, (byte)0x2));
            list.Add(new ServiceElement(ElementType.UInt8, (byte)0x3));
            list.Add(new ServiceElement(ElementType.UInt8, (byte)0xFF));
            element = new ServiceElement(ElementType.ElementSequence, list);
            attrs.Add(new ServiceAttribute(ObexAttributeId.SupportedFormatsList, element));
            //
            ServiceRecord record = new ServiceRecord(attrs);
            return record;
        }

        //--------
        [Test]
        public void GetPrimarySvcClassId_Opp()
        {
            ServiceRecord rcd = ServiceRecord.CreateServiceRecordFromBytes(Data_CompleteThirdPartyRecords.PalmOsOpp);
            Guid id = ServiceRecordHelper._GetPrimaryServiceClassId(rcd);
            Assert.AreEqual(BluetoothService.ObexObjectPush, id);
        }


    }//class

}
