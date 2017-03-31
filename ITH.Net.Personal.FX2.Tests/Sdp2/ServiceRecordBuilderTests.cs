using System;
using NUnit.Framework;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.AttributeIds;

namespace InTheHand.Net.Tests.Sdp2
{
    [TestFixture]
    public class ServiceRecordBuilderTests
    {
        private void DoTest(string expectedDump, ServiceRecordBuilder bldr)
        {
            ServiceRecord rcd = bldr.ServiceRecord;
            String dump = ServiceRecordUtilities.Dump(rcd);
            Assert.AreEqual(expectedDump, dump);
        }

        private void DoTestFails(ServiceRecordBuilder bldr)
        {
            ServiceRecord rcd = bldr.ServiceRecord;
            Assert.Fail("Should have thrown!");
        }

        private void DoTestFailsBuilderOrLater(ServiceRecordBuilder bldr)
        {
            ServiceRecord rcd = bldr.ServiceRecord;
            byte[] raw = new ServiceRecordCreator().CreateServiceRecord(rcd);
            Assert.Fail("Should have thrown2!");
        }


        //TODO causes Dump to crash!!
        //AttrId: 0x0004
        //ElementSequence
        //    Uuid16: 0x1101



        [Test]
        public void One()
        {
            // Rfcomm/StdSvcClass
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.SerialPort);
            DoTest(ServiceRecordBuilderTests_Data.One, bldr);
            Assert.AreEqual(BluetoothProtocolDescriptorType.Rfcomm, bldr.ProtocolType);
        }

        [Test]
        public void L2CapOne()
        {
            // L2Cap/StdSvcClass
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.SerialPort);
            bldr.ProtocolType = BluetoothProtocolDescriptorType.L2Cap;
            DoTest(ServiceRecordBuilderTests_Data.L2CapOne, bldr);
            Assert.AreEqual(BluetoothProtocolDescriptorType.L2Cap, bldr.ProtocolType);
        }

        [Test]
        public void Two()
        {
            // Rfcomm/AllSvcClassTypesForms
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.SerialPort);
            bldr.AddServiceClass(new Guid("{00112233-4455-6677-8899-aabbccddeeff}"));
            bldr.AddServiceClass((UInt16)0x1106);
            bldr.AddServiceClass(0x7654);
            bldr.AddServiceClass(0x9901);
            bldr.AddServiceClass(0x123456);
            bldr.AddServiceClass(0x98761234);
            DoTest(ServiceRecordBuilderTests_Data.Two, bldr);
        }

        [Test]
        public void OnePlusName()
        {
            // Rfcomm/StdSvcClass/SvcName
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.SerialPort);
            bldr.ServiceName = "Hello World!";
            DoTest(ServiceRecordBuilderTests_Data.OnePlusName, bldr);
            Assert.AreEqual("Hello World!", bldr.ServiceName);
        }

        [Test]
        public void Three()
        {
            // Geop/StdSvcClass/PrvName/SvcDescr
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.ObexObjectPush);
            bldr.ProtocolType = BluetoothProtocolDescriptorType.GeneralObex;
            bldr.ProviderName = "Alan enterprises inc.";
            bldr.ServiceDescription = "\u2020 daggers to you";
            DoTest(ServiceRecordBuilderTests_Data.Three, bldr);
            Assert.AreEqual(BluetoothProtocolDescriptorType.GeneralObex, bldr.ProtocolType);
            Assert.AreEqual("Alan enterprises inc.", bldr.ProviderName);
            Assert.AreEqual("\u2020 daggers to you", bldr.ServiceDescription);
        }

        [Test]
        public void Four()
        {
            // None/Svc16
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(-1);
            bldr.ProtocolType = BluetoothProtocolDescriptorType.None;
            DoTest(ServiceRecordBuilderTests_Data.Four, bldr);
        }

        [Test]
        public void Five_HSPv1_1()
        {
            // Headset == Rfcomm/2xStdSvcClass/BtPDL/1xCustom
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.Headset);
            bldr.AddServiceClass(BluetoothService.GenericAudio);
            bldr.AddBluetoothProfileDescriptor(BluetoothService.Headset, 1, 0);
            bldr.AddCustomAttribute(new ServiceAttribute(
                HeadsetProfileAttributeId.RemoteAudioVolumeControl,
                new ServiceElement(ElementType.Boolean, false)));
            DoTest(ServiceRecordBuilderTests_Data.Five_HSPv1_1_HS, bldr);
            Assert.AreEqual(BluetoothProtocolDescriptorType.Rfcomm, bldr.ProtocolType);
        }

        [Test]
        public void Five_HSPv1_2()
        {
            // Headset == Rfcomm/2xStdSvcClass/BtPDL/1xCustom
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.HeadsetHeadset);
            bldr.AddServiceClass(BluetoothService.GenericAudio);
            bldr.AddBluetoothProfileDescriptor(BluetoothService.Headset, 1, 2);
            bldr.AddCustomAttribute(new ServiceAttribute(
                HeadsetProfileAttributeId.RemoteAudioVolumeControl,
                new ServiceElement(ElementType.Boolean, false)));
            DoTest(ServiceRecordBuilderTests_Data.Five_HSPv1_2_HS, bldr);
            Assert.AreEqual(BluetoothProtocolDescriptorType.Rfcomm, bldr.ProtocolType);
        }

        [Test]
        public void Five_HSP_AG()
        {
            // HeadsetAG == Rfcomm/2xStdSvcClass/BtPDL
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.HeadsetAudioGateway);
            bldr.AddServiceClass(BluetoothService.GenericAudio);
            bldr.AddBluetoothProfileDescriptor(BluetoothService.Headset, 1, 0);
            DoTest(ServiceRecordBuilderTests_Data.Five_HSPv1_1_AG, bldr);
            Assert.AreEqual(BluetoothProtocolDescriptorType.Rfcomm, bldr.ProtocolType);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Record has no Service Class IDs.")]
        public void NoSvcClass()
        {
            // Rfcomm/None
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            DoTestFails(bldr);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Unknown protocol type: 9999.")]
        public void BadProtoTypeWierd()
        {
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(0x1101);
            bldr.ProtocolType = (BluetoothProtocolDescriptorType)9999;
            DoTestFails(bldr);
        }

        [Test]
        public void CustomOne()
        {
            // Rfcomm/StdSvcClass/SvcName
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.SerialPort);
            bldr.ServiceName = "Hello World!";
            ServiceAttribute attr = new ServiceAttribute(
                UniversalAttributeId.ServiceAvailability,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 255));
            bldr.AddCustomAttribute(attr);
            DoTest(ServiceRecordBuilderTests_Data.OnePlusNamePlusCustomOne, bldr);
        }

        [Test]
        public void CustomTwoSeparate()
        {
            // Rfcomm/StdSvcClass/SvcName
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.SerialPort);
            bldr.ServiceName = "Hello World!";
            ServiceAttribute attr = new ServiceAttribute(
                UniversalAttributeId.ServiceAvailability,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 255));
            bldr.AddCustomAttribute(attr);
            attr = new ServiceAttribute(
                UniversalAttributeId.ServiceInfoTimeToLive,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt32, 56623104));
            bldr.AddCustomAttribute(attr);
            DoTest(ServiceRecordBuilderTests_Data.OnePlusNamePlusCustomTwo, bldr);
        }

        [Test]
        public void CustomTwoParamArray()
        {
            // Rfcomm/StdSvcClass/SvcName
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.SerialPort);
            bldr.ServiceName = "Hello World!";
            bldr.AddCustomAttributes(
                new ServiceAttribute(
                    UniversalAttributeId.ServiceAvailability,
                    ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 255)),
                new ServiceAttribute(
                    UniversalAttributeId.ServiceInfoTimeToLive,
                    ServiceElement.CreateNumericalServiceElement(ElementType.UInt32, 56623104))
            );
            DoTest(ServiceRecordBuilderTests_Data.OnePlusNamePlusCustomTwo, bldr);
        }

#if ! FX1_1
        [Test]
        public void CustomTwoListT()
        {
            // Rfcomm/StdSvcClass/SvcName
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.SerialPort);
            bldr.ServiceName = "Hello World!";
            System.Collections.Generic.List<ServiceAttribute> list
                = new System.Collections.Generic.List<ServiceAttribute>();
            list.Add(new ServiceAttribute(
                    UniversalAttributeId.ServiceAvailability,
                    ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 255)));
            list.Add(new ServiceAttribute(
                    UniversalAttributeId.ServiceInfoTimeToLive,
                    ServiceElement.CreateNumericalServiceElement(ElementType.UInt32, 56623104)));
            bldr.AddCustomAttributes(list);
            DoTest(ServiceRecordBuilderTests_Data.OnePlusNamePlusCustomTwo, bldr);
        }
#endif

        [Test]
        public void CustomTwoList()
        {
            // Rfcomm/StdSvcClass/SvcName
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.SerialPort);
            bldr.ServiceName = "Hello World!";
            System.Collections.ArrayList list
                = new System.Collections.ArrayList();
            list.Add(new ServiceAttribute(
                    UniversalAttributeId.ServiceAvailability,
                    ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 255)));
            list.Add(new ServiceAttribute(
                    UniversalAttributeId.ServiceInfoTimeToLive,
                    ServiceElement.CreateNumericalServiceElement(ElementType.UInt32, 56623104)));
            bldr.AddCustomAttributes((System.Collections.IEnumerable)list);
            DoTest(ServiceRecordBuilderTests_Data.OnePlusNamePlusCustomTwo, bldr);
        }

        [Test]
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
            DoTest(ServiceRecordBuilderTests_Data.CustomTwoFromOneWithHighAttrIdAdded, bldr);
        }

        [Test]
        public void CustomListBadItemType()
        {
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            System.Collections.ArrayList list
                = new System.Collections.ArrayList();
            list.Add(33333);
            try {
                bldr.AddCustomAttributes(list);
                Assert.Fail("should have thrown!");
            } catch (ArgumentException) {
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "ServiceRecordBuilder is configured to allow only one of each attribute id.")]
        public void CustomDuplicateBuiltIn()
        {
            // Note: not checking here WHEN the exception is thrown...
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(0x1101);
            bldr.AddCustomAttributes(new ServiceAttribute(UniversalAttributeId.ServiceClassIdList,
                new ServiceElement(ElementType.TextString, "DUMMY")));
            DoTestFailsBuilderOrLater(bldr);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "ServiceRecordBuilder is configured to allow only one of each attribute id.")]
        public void CustomDuplicateTwoArray()
        {
            // Note: not checking here WHEN the exception is thrown...
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.SerialPort);
            bldr.ServiceName = "Hello World!";
            ServiceAttribute[] array = new ServiceAttribute[]{
                new ServiceAttribute(
                    UniversalAttributeId.ServiceAvailability,
                    ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 255)),
                new ServiceAttribute(
                    UniversalAttributeId.ServiceAvailability,
                    ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 0x55))};
            System.Collections.IEnumerable eble = array;
            bldr.AddCustomAttributes(eble);
            DoTestFailsBuilderOrLater(bldr);
        }

#if ! FX1_1
        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "ServiceRecordBuilder is configured to allow only one of each attribute id.")]
        public void CustomDuplicateTwoEnumGeneric()
        {
            // Note: not checking here WHEN the exception is thrown...
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.SerialPort);
            bldr.ServiceName = "Hello World!";
            ServiceAttribute[] array = new ServiceAttribute[]{
                new ServiceAttribute(
                    UniversalAttributeId.ServiceAvailability,
                    ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 255)),
                new ServiceAttribute(
                    UniversalAttributeId.ServiceAvailability,
                    ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 0x55))};
            System.Collections.Generic.IEnumerable<ServiceAttribute> eble = array;
            bldr.AddCustomAttributes(eble);
            DoTestFailsBuilderOrLater(bldr);
        }
#endif

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "ServiceRecordBuilder is configured to allow only one of each attribute id.")]
        public void CustomDuplicateTwoEnumNonGeneric()
        {
            // Note: not checking here WHEN the exception is thrown...
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.SerialPort);
            bldr.ServiceName = "Hello World!";
            ServiceAttribute[] array = new ServiceAttribute[]{
                new ServiceAttribute(
                    UniversalAttributeId.ServiceAvailability,
                    ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 255)),
                new ServiceAttribute(
                    UniversalAttributeId.ServiceAvailability,
                    ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 0x55))};
            System.Collections.IEnumerable eble = array;
            bldr.AddCustomAttributes(eble);
            DoTestFailsBuilderOrLater(bldr);
        }

    }//class


    class ServiceRecordBuilderTests_Data
    {
        public const String CrLf = "\r\n";

        public const String StandardRfcommPdl
            = "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100 -- L2CapProtocol" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x3 -- RFCommProtocol" + CrLf
            + "        UInt8: 0x0" + CrLf
            + "( ( L2Cap ), ( Rfcomm, ChannelNumber=0 ) )" + CrLf
            ;

        public const String StandardL2CapPdl
            = "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100 -- L2CapProtocol" + CrLf
            + "        UInt16: 0x0" + CrLf
            + "( ( L2Cap, PSM=0x0 ) )" + CrLf
            ;

        public const String StandardGeopPdl
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

        public const String StandardLangBaseId
            = "AttrId: 0x0006 -- LanguageBaseAttributeIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    UInt16: 0x656E" + CrLf
            + "    UInt16: 0x6A" + CrLf
            + "    UInt16: 0x100" + CrLf
            ;

        //--------------------------------------------------------------
        public const String One
            = "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1101 -- SerialPort" + CrLf
            + CrLf
            + StandardRfcommPdl
            ;

        public const String L2CapOne
            = "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1101 -- SerialPort" + CrLf
            + CrLf
            + StandardL2CapPdl
            ;

        public const String OnePlusName
            = "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1101 -- SerialPort" + CrLf
            + CrLf
            + StandardRfcommPdl
            + CrLf
            + StandardLangBaseId
            + CrLf
            + "AttrId: 0x0100 -- ServiceName" + CrLf
            + "TextString: [en] 'Hello World!'" + CrLf
            ;

        public const String Two
            = "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1101 -- SerialPort" + CrLf
            + "    Uuid128: 00112233-4455-6677-8899-aabbccddeeff" + CrLf
            + "    Uuid16: 0x1106 -- ObexFileTransfer" + CrLf
            + "    Uuid16: 0x7654" + CrLf
            + "    Uuid16: 0x9901" + CrLf
            + "    Uuid32: 0x123456" + CrLf
            + "    Uuid32: 0x98761234" + CrLf
            + CrLf
            + StandardRfcommPdl
            ;

        public const String Three
            = "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1105 -- ObexObjectPush" + CrLf
            + CrLf
            + StandardGeopPdl
            + CrLf
            + StandardLangBaseId
            + CrLf
            + "AttrId: 0x0101 -- ServiceDescription" + CrLf
            + "TextString: [en] '\u2020 daggers to you'" + CrLf
            + CrLf
            + "AttrId: 0x0102 -- ProviderName" + CrLf
            + "TextString: [en] 'Alan enterprises inc.'" + CrLf
            ;

        public const String Four
            = "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid32: 0xFFFFFFFF" + CrLf
            ;

        public const String Five_HSPv1_1_HS
            = "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1108 -- Headset" + CrLf
            + "    Uuid16: 0x1203 -- GenericAudio" + CrLf
            + CrLf
            + StandardRfcommPdl
            + CrLf
            + "AttrId: 0x0009 -- BluetoothProfileDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x1108 -- Headset" + CrLf
            + "        UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x0302 -- RemoteAudioVolumeControl" + CrLf
            + "Boolean: False" + CrLf
            ;

        public const String Five_HSPv1_2_HS
            = "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1131 -- HeadsetHeadset" + CrLf
            + "    Uuid16: 0x1203 -- GenericAudio" + CrLf
            + CrLf
            + StandardRfcommPdl
            + CrLf
            + "AttrId: 0x0009 -- BluetoothProfileDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x1108 -- Headset" + CrLf
            + "        UInt16: 0x102" + CrLf
            + CrLf
            + "AttrId: 0x0302 -- RemoteAudioVolumeControl" + CrLf
            + "Boolean: False" + CrLf
            ;

        public const String Five_HSPv1_1_AG
            = "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1112 -- HeadsetAudioGateway" + CrLf
            + "    Uuid16: 0x1203 -- GenericAudio" + CrLf
            + CrLf
            + StandardRfcommPdl
            + CrLf
            + "AttrId: 0x0009 -- BluetoothProfileDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x1108 -- Headset" + CrLf
            + "        UInt16: 0x100" + CrLf
            ;

        public const String OnePlusNamePlusCustomOne
            = "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1101 -- SerialPort" + CrLf
            + CrLf
            + StandardRfcommPdl
            + CrLf
            + StandardLangBaseId
            + CrLf
            + "AttrId: 0x0008 -- ServiceAvailability" + CrLf
            + "UInt8: 0xFF" + CrLf
            + CrLf
            + "AttrId: 0x0100 -- ServiceName" + CrLf
            + "TextString: [en] 'Hello World!'" + CrLf
            ;

        public const String CustomTwoFromOneWithHighAttrIdAdded
            = "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1101 -- SerialPort" + CrLf
            + CrLf
            + StandardRfcommPdl
            + CrLf
            + StandardLangBaseId
            + CrLf
            + "AttrId: 0x0008 -- ServiceAvailability" + CrLf
            + "UInt8: 0x8" + CrLf
            + CrLf
            + "AttrId: 0x0100 -- ServiceName" + CrLf
            + "TextString: [en] 'Hello World!'" + CrLf
            + CrLf
            + "AttrId: 0x8000" + CrLf
            + "UInt8: 0x80" + CrLf
            + CrLf
            + "AttrId: 0xFFFF" + CrLf
            + "UInt8: 0xFF" + CrLf
            ;

        public const String OnePlusNamePlusCustomTwo
            = "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1101 -- SerialPort" + CrLf
            + CrLf
            + StandardRfcommPdl
            + CrLf
            + StandardLangBaseId
            + CrLf
            + "AttrId: 0x0007 -- ServiceInfoTimeToLive" + CrLf
            + "UInt32: 0x3600000" + CrLf
            + CrLf
            + "AttrId: 0x0008 -- ServiceAvailability" + CrLf
            + "UInt8: 0xFF" + CrLf
            + CrLf
            + "AttrId: 0x0100 -- ServiceName" + CrLf
            + "TextString: [en] 'Hello World!'" + CrLf
            ;

        //--------------------------------------------------------------
        public const String FooRfcommOne
            = "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid128: 10203040-5060-7080-90a1-b1c1d1d1e100" + CrLf
            + CrLf
            + StandardRfcommPdl
            + CrLf
            + StandardLangBaseId
            + CrLf
            + "AttrId: 0x0100 -- ServiceName" + CrLf
            + "TextString: [en] 'SPPEx'" + CrLf
            ;

        public const String FooGoepOne
            = "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid128: 12af51a9-030c-4b29-3740-7f8c9ecb238a" + CrLf
            + CrLf
            + StandardGeopPdl
            ;

        public const String FooL2CapOne
            = "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid128: 3b9fa895-2007-8c30-3355-aaa694238f08" + CrLf
            + CrLf
            + StandardL2CapPdl
            ;

        public const String FooL2CapTwoWithName
            = "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid128: 12af51a9-030c-4b29-3740-7f8c9ecb238a" + CrLf
            + CrLf
            + StandardL2CapPdl
            + CrLf
            + StandardLangBaseId
            + CrLf
            + "AttrId: 0x0100 -- ServiceName" + CrLf
            + "TextString: [en] 'Aserv'" + CrLf
            ;

        public const String FooRfcommOneWithFancyName
            = "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid128: 10203040-5060-7080-90a1-b1c1d1d1e100" + CrLf
            + CrLf
            + StandardRfcommPdl
            + CrLf
            + StandardLangBaseId
            + CrLf
            + "AttrId: 0x0100 -- ServiceName" + CrLf
            + "TextString: [en] 'aa-bb_cc dd1234'" + CrLf
            ;

    }//class
}
