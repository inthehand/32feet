#if BLUETOPIA
using System;
using NUnit.Framework;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.StonestreetOne;
using NMock2;
using System.Collections.Generic;
using InTheHand.Net.Bluetooth.AttributeIds;

namespace InTheHand.Net.Tests.BluetopiaTests
{
    [TestFixture]
    public class BluetopiaSdpCreateManualTests
    {
        //----
        void DoTest(StackConsts.SDP_Data_Element_Type expectedET,
            byte[] expectedDataValue, bool expectedValueInline,
            ServiceElement element)
        {
            var r = new ServiceRecord(new ServiceAttribute(0xF234,
                element));
            //== 
            r = BluetopiaTesting.HackAddSvcClassList(r);
            //==
            DoTest(expectedET, expectedDataValue, expectedValueInline, r);
        }

        void DoTest(StackConsts.SDP_Data_Element_Type expectedET,
            byte[] expectedDataValue, bool expectedValueInline,
            ServiceRecord r)
        {
            var stuff = BluetopiaTesting.InitMockery_SdpCreator();
            //
            const UInt16 attrId = 0xF234;
            Structs.SDP_Data_Element__Class_ByteArray expectedElement;
            if (expectedValueInline) {
                expectedElement = new Structs.SDP_Data_Element__Class_InlineByteArray(
                    expectedET,
                    expectedDataValue.Length, expectedDataValue);
            } else {
                expectedElement = new Structs.SDP_Data_Element__Class_NonInlineByteArray(
                    expectedET,
                    expectedDataValue.Length, expectedDataValue);
            }
            Expect.Once.On(stuff.MockApi2).Method("SDP_Add_Attribute")
                .With(
                    stuff.StackId, stuff.SrHandle,
                    attrId,
                    expectedElement
                    )
                .Will(Return.Value(BluetopiaError.OK));
            //
            stuff.DutSdpCreator.CreateServiceRecord(r);
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            VerifyDispose(stuff);
        }

        void DoTest_HackOneSeqDeep(StackConsts.SDP_Data_Element_Type expectedET,
            byte[] expectedDataValue,
            ServiceElement element)
        {
            var r = new ServiceRecord(new ServiceAttribute(0xF234,
                element));
            //== 
            r = BluetopiaTesting.HackAddSvcClassList(r);
            //==
            DoTest_HackOneSeqDeep(expectedET, expectedDataValue, r);
        }

        void DoTest_HackOneSeqDeep(StackConsts.SDP_Data_Element_Type expectedET,
            byte[] expectedDataValue,
            ServiceRecord r)
        {
            var stuff = BluetopiaTesting.InitMockery_SdpCreator();
            //
            const UInt16 attrId = 0xF234;
            var expectedElement = new Structs.SDP_Data_Element__Class_ElementArray(
                StackConsts.SDP_Data_Element_Type.Sequence,
                1, new Structs.SDP_Data_Element__Class[]{
                    new Structs.SDP_Data_Element__Class_InlineByteArray(
                    expectedET,
                    expectedDataValue.Length, expectedDataValue)});
            Expect.Once.On(stuff.MockApi2).Method("SDP_Add_Attribute")
                .With(
                    stuff.StackId, stuff.SrHandle,
                    attrId,
                    expectedElement
                    )
                .Will(Return.Value(BluetopiaError.OK));
            //
            stuff.DutSdpCreator.CreateServiceRecord(r);
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            VerifyDispose(stuff);
        }

        void DoTest_HackTwoSeqDeep(StackConsts.SDP_Data_Element_Type expectedET,
            byte[] expectedDataValue,
            ServiceElement element)
        {
            var r = new ServiceRecord(new ServiceAttribute(0xF234,
                element));
            //== 
            r = BluetopiaTesting.HackAddSvcClassList(r);
            //==
            DoTest_HackTwoSeqDeep(expectedET, expectedDataValue, r);
        }

        void DoTest_HackTwoSeqDeep(StackConsts.SDP_Data_Element_Type expectedET,
            byte[] expectedDataValue,
            ServiceRecord r)
        {
            var stuff = BluetopiaTesting.InitMockery_SdpCreator();
            //
            const UInt16 attrId = 0xF234;
            var expectedElement = new Structs.SDP_Data_Element__Class_ElementArray(
                StackConsts.SDP_Data_Element_Type.Sequence,
                1, new Structs.SDP_Data_Element__Class[]{
                    new Structs.SDP_Data_Element__Class_ElementArray(
                        StackConsts.SDP_Data_Element_Type.Sequence,
                        1, new Structs.SDP_Data_Element__Class[]{
                            new Structs.SDP_Data_Element__Class_InlineByteArray(
                            expectedET,
                            expectedDataValue.Length, expectedDataValue)})});
            Expect.Once.On(stuff.MockApi2).Method("SDP_Add_Attribute")
                .With(
                    stuff.StackId, stuff.SrHandle,
                    attrId,
                    expectedElement
                    )
                .Will(Return.Value(BluetopiaError.OK));
            //
            stuff.DutSdpCreator.CreateServiceRecord(r);
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            VerifyDispose(stuff);
        }

        private static void VerifyDispose(StuffSdpCreatorBluetopia stuff)
        {
            Expect.Once.On(stuff.MockApi2).Method("SDP_Delete_Service_Record")
                .With(
                Is.Anything,
                Is.Anything)
                .Will(Return.Value(BluetopiaError.OK));
            stuff.DutSdpCreator.Dispose();
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
        }


        //---- Fixed Length ----
        [Test]
        public void UInt8()
        {
            byte[] expectedDataValue = { 0xF5 };
            DoTest(StackConsts.SDP_Data_Element_Type.UnsignedInteger1Byte, expectedDataValue, true,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 0xF5));
        }

        [Test]
        [Explicit]
        public void UInt8_Bad_TestTheTesting()
        {
            byte[] expectedDataValue = { 0xF5 };
            DoTest(StackConsts.SDP_Data_Element_Type.UnsignedInteger1Byte, expectedDataValue, true,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 99));
        }

        [Test]
        public void UInt16()
        {
            byte[] expectedDataValue = { 0x34, 0xF5 };
            DoTest(StackConsts.SDP_Data_Element_Type.UnsignedInteger2Bytes, expectedDataValue, true,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 0xF534));
        }

        [Test]
        public void Uuid16()
        {
            byte[] expectedDataValue = { 0xF5, 0x34 };
            DoTest(StackConsts.SDP_Data_Element_Type.UUID_16, expectedDataValue, true,
                new ServiceElement(ElementType.Uuid16, (ushort)0xF534));
        }

        [Test, Explicit] // Test the testing.
        public void Uuid128_Bad_TestTheTesting()
        {
            byte[] expectedDataValue =  { 99, 99, 99, 99 ,0,0,0,0,
                                        0,0,0,0,0,0,0,0};
            Uuid128_(expectedDataValue);
        }

        [Test]
        public void Uuid128()
        {
            byte[] expectedDataValue = {
                0x00, 0x00, 0x11, 0x29,
                0x00, 0x00,
                0x10, 0x00,
                0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB};
            Uuid128_(expectedDataValue);

        }

        void Uuid128_(byte[] expectedDataValue)
        {
            DoTest(StackConsts.SDP_Data_Element_Type.UUID_128, expectedDataValue, true,
                new ServiceElement(ElementType.Uuid128, BluetoothService.VideoConferencingGW));
        }

        //----
        [Test]
        public void Nil()
        {
            byte[] expectedDataValue = { };
            DoTest(StackConsts.SDP_Data_Element_Type.NIL, expectedDataValue, true,
                new ServiceElement(ElementType.Nil, null));
        }

        [Test]
        public void BooleanTrue()
        {
            byte[] expectedDataValue = { 1 };
            DoTest(StackConsts.SDP_Data_Element_Type.Boolean, expectedDataValue, true,
                new ServiceElement(ElementType.Boolean, true));
        }

        [Test]
        public void BooleanFalse()
        {
            byte[] expectedDataValue = { 0 };
            DoTest(StackConsts.SDP_Data_Element_Type.Boolean, expectedDataValue, true,
                new ServiceElement(ElementType.Boolean, false));
        }

        //---- Variable Length ----
        [Test]
        public void StringString()
        {
            byte[] expectedDataValue = { (byte)'a', (byte)'b', (byte)'c', (byte)'d', };
            DoTest(StackConsts.SDP_Data_Element_Type.TextString, expectedDataValue, false,
                new ServiceElement(ElementType.TextString, "abcd"));
        }

        [Test]
        public void StringBytes()
        {
            byte[] expectedDataValue = { (byte)'a', (byte)'b', (byte)'c', (byte)'d', };
            DoTest(StackConsts.SDP_Data_Element_Type.TextString, expectedDataValue, false,
                new ServiceElement(ElementType.TextString, new byte[] { (byte)'a', (byte)'b', (byte)'c', (byte)'d', }));
        }

        //---- Sequence ----
        [Test]
        public void SequenceHoldingUInt16()
        {
            byte[] expectedDataValue = { 0x34, 0xF5 };
            DoTest_HackOneSeqDeep(StackConsts.SDP_Data_Element_Type.UnsignedInteger2Bytes, expectedDataValue,
                new ServiceElement(ElementType.ElementSequence,
                    ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 0xF534)));
        }
        [Test]
        public void SequenceHoldingSequenceHoldingUInt16()
        {
            byte[] expectedDataValue = { 0x34, 0xF5 };
            DoTest_HackTwoSeqDeep(StackConsts.SDP_Data_Element_Type.UnsignedInteger2Bytes, expectedDataValue,
                new ServiceElement(ElementType.ElementSequence,
                    new ServiceElement(ElementType.ElementSequence,
                        ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 0xF534))));
        }
        [Test]
        [Explicit]
        public void SequenceHoldingSequenceHoldingUInt16_Bad_TestTheTesting()
        {
            byte[] expectedDataValue = { 0x34, 0xF5 };
            DoTest_HackTwoSeqDeep(StackConsts.SDP_Data_Element_Type.UnsignedInteger2Bytes, expectedDataValue,
                new ServiceElement(ElementType.ElementSequence,
                    new ServiceElement(ElementType.ElementSequence,
                        ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 0x6363))));
        }

        //---- Calling ----
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CallingTwice()
        {
            // Haven't decided whether to keep the instance to hold one
            // live record or like the base to create multiple records,
            // probably the first.
            //
            var stuff = BluetopiaTesting.InitMockery_SdpCreator();
            try {
                stuff.DutSdpCreator.CreateServiceRecord(BluetopiaTesting.HackAddSvcClassList(new ServiceRecord()));
                stuff.DutSdpCreator.CreateServiceRecord(BluetopiaTesting.HackAddSvcClassList(new ServiceRecord()));
            } finally {
                VerifyDispose(stuff);
            }
        }

        //----
        [Test]
        public void RecordA()
        {
            var stuff = BluetopiaTesting.InitMockery_SdpCreator();
            var bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.Wap);
            bldr.AddBluetoothProfileDescriptor(BluetoothService.WapClient, 0x1, 0x2);
            //
            Expect.AtLeastOnce.On(stuff.MockApi2).Method("SDP_Add_Attribute")
                .With(
                    stuff.StackId, stuff.SrHandle,
                    (ushort)UniversalAttributeId.ProtocolDescriptorList,
                    //Is.Anything
                    new Structs.SDP_Data_Element__Class_ElementArray(
                        StackConsts.SDP_Data_Element_Type.Sequence,
                        2,
                        new Structs.SDP_Data_Element__Class_ElementArray(
                            StackConsts.SDP_Data_Element_Type.Sequence,
                            1,
                            new Structs.SDP_Data_Element__Class_InlineByteArray(
                                StackConsts.SDP_Data_Element_Type.UUID_16,
                                2, new byte[] { 0x01, 0x00 }
                            )
                        ),
                        new Structs.SDP_Data_Element__Class_ElementArray(
                            StackConsts.SDP_Data_Element_Type.Sequence,
                            2,
                            new Structs.SDP_Data_Element__Class_InlineByteArray(
                                StackConsts.SDP_Data_Element_Type.UUID_16,
                                2, new byte[] { 0x00, 0x03 }
                            ),
                            new Structs.SDP_Data_Element__Class_InlineByteArray(
                                StackConsts.SDP_Data_Element_Type.UnsignedInteger1Byte,
                                1, new byte[] { 0 }
                            )
                        )
                    )
                )
                .Will(Return.Value(BluetopiaError.OK));
            Expect.AtLeastOnce.On(stuff.MockApi2).Method("SDP_Add_Attribute")
                .With(
                    stuff.StackId, stuff.SrHandle,
                    (ushort)UniversalAttributeId.BluetoothProfileDescriptorList,
                    new Structs.SDP_Data_Element__Class_ElementArray(
                        StackConsts.SDP_Data_Element_Type.Sequence,
                        1,
                        new Structs.SDP_Data_Element__Class_ElementArray(
                            StackConsts.SDP_Data_Element_Type.Sequence,
                            2,
                            new Structs.SDP_Data_Element__Class_InlineByteArray(
                                StackConsts.SDP_Data_Element_Type.UUID_16,
                                2, new byte[] { 0x11, 0x14, }
                            ),
                            new Structs.SDP_Data_Element__Class_InlineByteArray(
                                StackConsts.SDP_Data_Element_Type.UnsignedInteger2Bytes,
                                2, new byte[] { 0x02, 0x01, }//endian!
                            )
                        )
                    )
                )
                .Will(Return.Value(BluetopiaError.OK));
            //
            stuff.DutSdpCreator.CreateServiceRecord(bldr.ServiceRecord);
            //--
            VerifyDispose(stuff);
        }


    }
}
#endif
