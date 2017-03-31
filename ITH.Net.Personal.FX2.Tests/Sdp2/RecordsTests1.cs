using System;
using NUnit.Framework;
using System.Net;
using InTheHand.Net.Bluetooth;

namespace InTheHand.Net.Tests.Sdp2
{

    [TestFixture]
    public class MiscRecordParseToRecordTests
    {
        ServiceRecord DoParse(byte[] buffer)
        {
            return new ServiceRecordParser().Parse(buffer);
        }

        ServiceRecord DoParse(byte[] buffer, int offset, int length)
        {
            return new ServiceRecordParser().Parse(buffer, offset, length);
        }

        [Test]
        public void Nil()
        {
            ServiceRecord record = DoParse(Data_SimpleRecords.OneNil);
            Assert.AreEqual(1, record.Count);
            Assert.AreEqual(ElementType.Nil, record[0].Value.ElementType);
            Assert.AreEqual(ElementTypeDescriptor.Nil, record[0].Value.ElementTypeDescriptor);
            //
            Assert.AreEqual(Data_SimpleRecords.OneNil, record.SourceBytes, "SourceBytes");
        }

        [Test]
        public void Nil_ModifyInputArrayAfterParse_seeSourceBytes()
        {
            byte[] arrayCopy = (byte[])Data_SimpleRecords.OneNil.Clone();
            ServiceRecord record = DoParse(arrayCopy);
            Assert.AreEqual(1, record.Count);
            Assert.AreEqual(ElementType.Nil, record[0].Value.ElementType);
            Assert.AreEqual(ElementTypeDescriptor.Nil, record[0].Value.ElementTypeDescriptor);
            //
            Assert.AreEqual(Data_SimpleRecords.OneNil, record.SourceBytes, "SourceBytes");
            arrayCopy[0] = 69;
            arrayCopy[1] = 69;
            Assert.AreEqual(Data_SimpleRecords.OneNil, record.SourceBytes, "SourceBytes");
        }

        [Test]
        [ExpectedException(typeof(ProtocolViolationException), ExpectedMessage = "SizeIndex is not value for TypeDescriptor.")]
        public void NilBadSizeIndex()
        {
            ServiceRecord record = DoParse(Data_SimpleRecords.OneNilBadSizeIndex);
            Assert.Fail("should have thrown!");
        }

        [Test]
        public void Uint32()
        {
            ServiceRecord record = DoParse(Data_SimpleRecords.OneUInt32F123_E9876543);
            Assert.AreEqual(1, record.Count);
            Assert.AreEqual(Data_SimpleRecords.IdF123, record[0].Id);
            Assert.AreEqual(ElementType.UInt32, record[0].Value.ElementType);
        }

        [Test]
        public void OneUInt32F123_E9876543OffsetBy4()
        {
            ServiceRecord record = DoParse(Data_SimpleRecords.OneUInt32F123_E9876543OffsetBy4, 
                4, Data_SimpleRecords.OneUInt32F123_E9876543OffsetBy4.Length - 4);
            Assert.AreEqual(1, record.Count);
            Assert.AreEqual(Data_SimpleRecords.IdF123, record[0].Id);
            Assert.AreEqual(ElementType.UInt32, record[0].Value.ElementType);
            //
            Assert.AreEqual(null, record.SourceBytes, "SourceBytes current not set if buffer was offset");
        }

        [Test]
        public void TwoUInt32OffsetBy1()
        {
            ServiceRecord record = DoParse(Data_SimpleRecords.TwoUInt32OffsetBy1,
                1, Data_SimpleRecords.TwoUInt32OffsetBy1.Length - 1);
            Assert.AreEqual(2, record.Count);
            Assert.AreEqual(Data_SimpleRecords.Id0, record[0].Id);
            Assert.AreEqual(ElementType.UInt32, record[0].Value.ElementType);
            Assert.AreEqual(Data_SimpleRecords.IdF123, record[1].Id);
            Assert.AreEqual(ElementType.UInt32, record[1].Value.ElementType);
        }

        [Test]
        public void Uuid16OffsetBy1()
        {
            ServiceRecord record = DoParse(Data_SimpleRecords.Uuid16OffsetBy1,
                1, Data_SimpleRecords.Uuid16OffsetBy1.Length - 1);
            Assert.AreEqual(1, record.Count);
            Assert.AreEqual(Data_SimpleRecords.IdF123, record[0].Id);
            Assert.AreEqual(ElementType.Uuid16, record[0].Value.ElementType);
        }

        [Test]
#if NETCF
        [ExpectedException(typeof(System.ArgumentException))]
#else
        [ExpectedException(typeof(System.Text.DecoderFallbackException),
            ExpectedMessage = "Unable to translate bytes [FF] at index -1 from specified code page to Unicode.")]
#endif
        public void OneStringBadUtf8_ElementGetUtf8()
        {
            //--
            ServiceRecord record = DoParse(Data_SimpleRecords.OneStringBadUtf8);
            Assert.AreEqual(1, record.Count);
            Assert.AreEqual(ElementType.TextString, record[0].Value.ElementType);
            Assert.AreEqual(ElementTypeDescriptor.TextString, record[0].Value.ElementTypeDescriptor);
            record[0].Value.GetValueAsStringUtf8();
        }

        //--------------------------------------------------------------

        [Test]
        public void HugeRecord()
        {
            byte[] data = Data_SimpleRecords.CreateHugeRecord();
            ServiceRecord record = DoParse(data);
            Assert.AreEqual(3, record.Count);
            Assert.AreEqual(Data_SimpleRecords.Id0, record[0].Id);
            Assert.AreEqual(ElementType.UInt32, record[0].Value.ElementType);
            Assert.AreEqual(Data_SimpleRecords.IdF123, record[2].Id);
            Assert.AreEqual(ElementType.UInt32, record[2].Value.ElementType);
            //
            Assert.AreEqual(Data_SimpleRecords.IdHuge1, record[1].Id);
            Assert.AreEqual(ElementType.TextString, record[1].Value.ElementType);
            String expectedString = new String('a', Data_SimpleRecords.HugeRecordValueTemplate_StringContentLen);
            String result = record[1].Value.GetValueAsStringUtf8();
            Assert.AreEqual(expectedString, result);
        }

    }//class


    [TestFixture]
    public class AllTypesParseToRecordTests
    {
        private ServiceRecord DoTest(ExpectedServiceAttribute[] expected, byte[] buffer)
        {
            return TestRecordParsing.DoTest(expected, buffer);
        }

        private ServiceRecord DoTestLazyUrlCreation(ExpectedServiceAttribute[] expected, byte[] buffer)
        {
            return TestRecordParsing.DoTestLazyUrlCreation(expected, buffer);
        }

        //--------------------------------------------------------------
        [Test]
        public void Empty()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[0];
            ServiceRecord record = DoTest(expected, Data_SimpleRecords.Empty);
            Assert.AreEqual(Data_SimpleRecords.Empty, record.SourceBytes, "SourceBytes");
        }
        //--------------------------------------------------------------

        // Test every type, each in a very simple record.
        //--------------------------------------------------------------
        [Test]
        public void Nil()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { new ExpectedServiceAttribute(0, ElementType.Nil, ElementTypeDescriptor.Nil, null) };
            DoTest(expected, Data_SimpleRecords.OneNil);
        }

        //--------------------------------------------------------------
        [Test]
        public void UInt8()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { new ExpectedServiceAttribute(0, ElementType.UInt8, ElementTypeDescriptor.UnsignedInteger, 0) };
            DoTest(expected, Data_SimpleRecords.OneUInt8ZeroZero);
        }

        [Test]
        public void UInt8TopBitSet()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { new ExpectedServiceAttribute(Data_SimpleRecords.IdF123Uint, ElementType.UInt8, ElementTypeDescriptor.UnsignedInteger, Data_SimpleRecords.OneUInt8F123_E9Value) };
            DoTest(expected, Data_SimpleRecords.OneUInt8F123_E9);
        }

        //--------------------------------------------------------------
        [Test]
        public void Int8()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { new ExpectedServiceAttribute(0, ElementType.Int8, ElementTypeDescriptor.TwosComplementInteger, 0) };
            DoTest(expected, Data_SimpleRecords.OneInt8ZeroZero);
        }
        [Test]
        public void Int8TopBitSet()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { new ExpectedServiceAttribute(Data_SimpleRecords.IdF123Uint, ElementType.Int8, ElementTypeDescriptor.TwosComplementInteger, Data_SimpleRecords.OneInt8F123_E9Value) };
            DoTest(expected, Data_SimpleRecords.OneInt8F123_E9);
        }

        //--------------------------------------------------------------
        [Test]
        public void Int16()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { new ExpectedServiceAttribute(Data_SimpleRecords.IdF123Uint, ElementType.Int16, ElementTypeDescriptor.TwosComplementInteger, Data_SimpleRecords.OneInt16_F123_E987_Value) };
            DoTest(expected, Data_SimpleRecords.OneInt16_F123_E987);
        }
        [Test]
        public void Int32()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { new ExpectedServiceAttribute(Data_SimpleRecords.IdF123Uint, ElementType.Int32, ElementTypeDescriptor.TwosComplementInteger, Data_SimpleRecords.OneInt32_F123_E9876543_Value) };
            DoTest(expected, Data_SimpleRecords.OneInt32_F123_E9876543);
        }
        [Test]
        public void Int64()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { new ExpectedServiceAttribute(Data_SimpleRecords.IdF123Uint, ElementType.Int64, ElementTypeDescriptor.TwosComplementInteger, Data_SimpleRecords.OneInt64_F123_E987654305060708_Value) };
            DoTest(expected, Data_SimpleRecords.OneInt64_F123_E987654305060708);
        }
        //--------------------------------------------------------------
        [Test]
        public void Int128()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { new ExpectedServiceAttribute(Data_SimpleRecords.IdF123Uint, ElementType.Int128, ElementTypeDescriptor.TwosComplementInteger, Data_SimpleRecords.OneUInt128_Value) };
            DoTest(expected, Data_SimpleRecords.OneInt128_F123_E987);
        }
        [Test]
        public void UInt128()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { new ExpectedServiceAttribute(Data_SimpleRecords.IdF123Uint, ElementType.UInt128, ElementTypeDescriptor.UnsignedInteger, Data_SimpleRecords.OneUInt128_Value) };
            DoTest(expected, Data_SimpleRecords.OneUInt128_F123_E987);
        }

        //--------------------------------------------------------------
        [Test]
        public void OneUuid16()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { new ExpectedServiceAttribute(Data_SimpleRecords.IdF123Uint, ElementType.Uuid16, ElementTypeDescriptor.Uuid, Data_SimpleRecords.OneUuid16Value) };
            DoTest(expected, Data_SimpleRecords.OneUuid16);
        }
        [Test]
        public void OneUuid32()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { new ExpectedServiceAttribute(Data_SimpleRecords.IdF123Uint, ElementType.Uuid32, ElementTypeDescriptor.Uuid, Data_SimpleRecords.OneUuid32Value) };
            DoTest(expected, Data_SimpleRecords.OneUuid32);
        }

        //--------------------------------------------------------------
        [Test]
        public void OneString()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { 
                new ExpectedServiceAttribute(Data_SimpleRecords.IdF123Uint, ElementType.TextString, ElementTypeDescriptor.TextString, 
                    Data_SimpleRecords.OneStringValueBytes) };
            DoTest(expected, Data_SimpleRecords.OneString);
        }

        [Test]
        public void OneStringZeroLength()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { 
                new ExpectedServiceAttribute(Data_SimpleRecords.IdF123Uint, ElementType.TextString, ElementTypeDescriptor.TextString, 
                    new byte[0]) };
            DoTest(expected, Data_SimpleRecords.OneStringZeroLength);
        }

        [Test]
        public void OneStringBadUtf8()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { 
                new ExpectedServiceAttribute(Data_SimpleRecords.IdF123Uint, ElementType.TextString, ElementTypeDescriptor.TextString, 
                    Data_SimpleRecords.OneStringBadUtf8_ValueBytes) };
            DoTest(expected, Data_SimpleRecords.OneStringBadUtf8);
        }

        //--------------------------------------------------------------
        [Test]
        public void OneBooleanZero()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { new ExpectedServiceAttribute(Data_SimpleRecords.IdF123Uint, ElementType.Boolean, ElementTypeDescriptor.Boolean, false) };
            DoTest(expected, Data_SimpleRecords.OneBooleanZero);
        }

        [Test]
        public void OneBooleanOne()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { new ExpectedServiceAttribute(Data_SimpleRecords.IdF123Uint, ElementType.Boolean, ElementTypeDescriptor.Boolean, true) };
            DoTest(expected, Data_SimpleRecords.OneBooleanOne);
        }

        [Test]
        public void OneBooleanNonZero()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { new ExpectedServiceAttribute(Data_SimpleRecords.IdF123Uint, ElementType.Boolean, ElementTypeDescriptor.Boolean, true) };
            DoTest(expected, Data_SimpleRecords.OneBooleanNonZero);
        }

        //--------------------------------------------------------------
        [Test]
        public void OneSeqWithNil()
        {
            ExpectedServiceElement expectedValue = new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null);
            ExpectedServiceElement[] expectedFirstElementsChildren = new ExpectedServiceElement[] { expectedValue };
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { 
                new ExpectedServiceAttribute(65535, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, expectedFirstElementsChildren ) };
            DoTest(expected, Data_SimpleRecords.OneSeqWithNil);
        }

        [Test]
        public void OneSeqEmpty()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { 
                new ExpectedServiceAttribute(65535, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[0]) };
            DoTest(expected, Data_SimpleRecords.OneSeqEmpty);
        }

        [Test]
        public void OneSeqWithOneSeqWithNil()
        {
            ExpectedServiceElement expectedValue = new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null);
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { 
                new ExpectedServiceAttribute(65535, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] { 
                        new ExpectedServiceElement(ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] { 
                            new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null)
                        })
                })
            };
            DoTest(expected, Data_SimpleRecords.OneSeqWithOneSeqWithNil);
        }

        [Test]
        public void OneSeqWithOneSeqWithTwoNil()
        {
            ExpectedServiceElement expectedValue = new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null);
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { 
                new ExpectedServiceAttribute(65535, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] { 
                        new ExpectedServiceElement(ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] { 
                            new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null),
                            new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null)
                        })
                })
            };
            DoTest(expected, Data_SimpleRecords.OneSeqWithOneSeqWithTwoNil);
        }

        [Test]
        public void OneSeqWith__OneSeqWithTwoNil_And_OneSeqWithOneNil_And_Int8()
        {
            ExpectedServiceElement expectedValue = new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null);
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { 
                new ExpectedServiceAttribute(65535, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] { 
                        new ExpectedServiceElement(ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] { 
                            new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null),
                            new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null)
                        }),
                        new ExpectedServiceElement(ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] { 
                            new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null),
                        })}),
                    new ExpectedServiceAttribute(2, ElementType.UInt8, ElementTypeDescriptor.UnsignedInteger, 0x99 )
            };
            DoTest(expected, Data_SimpleRecords.OneSeqWith__OneSeqWithTwoNil_And_OneSeqWithOneNil__And_Int8);
        }

        //--------------------------------------------------------------
        [Test]
        public void OneAltWithNil()
        {
            ExpectedServiceElement expectedValue = new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null);
            ExpectedServiceElement[] expectedFirstElementsChildren = new ExpectedServiceElement[] { expectedValue };
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { 
                new ExpectedServiceAttribute(65535, ElementType.ElementAlternative, ElementTypeDescriptor.ElementAlternative, expectedFirstElementsChildren) };
            DoTest(expected, Data_SimpleRecords.OneAltWithNil);
        }

        [Test]
        public void OneAltWithOneAltWithNil()
        {
            ExpectedServiceElement expectedValue = new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null);
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { 
                new ExpectedServiceAttribute(65535, ElementType.ElementAlternative, ElementTypeDescriptor.ElementAlternative, new ExpectedServiceElement[] { 
                        new ExpectedServiceElement(ElementType.ElementAlternative, ElementTypeDescriptor.ElementAlternative, new ExpectedServiceElement[] { 
                            new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null)
                        })
                })
            };
            DoTest(expected, Data_SimpleRecords.OneAltWithOneAltWithNil);
        }

        [Test]
        public void OneSeqWithOneAltWithNil()
        {
            ExpectedServiceElement expectedValue = new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null);
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { 
                new ExpectedServiceAttribute(65535, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] { 
                        new ExpectedServiceElement(ElementType.ElementAlternative, ElementTypeDescriptor.ElementAlternative, new ExpectedServiceElement[] { 
                            new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null)
                        })
                })
            };
            DoTest(expected, Data_SimpleRecords.OneSeqWithOneAltWithNil);
        }

        [Test]
        public void OneAltWithOneSeqWithNil()
        {
            ExpectedServiceElement expectedValue = new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null);
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { 
                new ExpectedServiceAttribute(65535, ElementType.ElementAlternative, ElementTypeDescriptor.ElementAlternative, new ExpectedServiceElement[] { 
                        new ExpectedServiceElement(ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] { 
                            new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null)
                        })
                })
            };
            DoTest(expected, Data_SimpleRecords.OneAltWithOneSeqWithNil);
        }

        [Test]
        public void OneAltWithOneAltWithTwoNil()
        {
            ExpectedServiceElement expectedValue = new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null);
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { 
                new ExpectedServiceAttribute(65535, ElementType.ElementAlternative, ElementTypeDescriptor.ElementAlternative, new ExpectedServiceElement[] { 
                        new ExpectedServiceElement(ElementType.ElementAlternative, ElementTypeDescriptor.ElementAlternative, new ExpectedServiceElement[] { 
                            new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null),
                            new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null)
                        })
                })
            };
            DoTest(expected, Data_SimpleRecords.OneAltWithOneAltWithTwoNil);
        }

        [Test]
        public void OneAltWith__OneAltWithTwoNil_And_OneAltWithOneNil_And_Int8()
        {
            ExpectedServiceElement expectedValue = new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null);
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { 
                new ExpectedServiceAttribute(65535, ElementType.ElementAlternative, ElementTypeDescriptor.ElementAlternative, new ExpectedServiceElement[] { 
                        new ExpectedServiceElement(ElementType.ElementAlternative, ElementTypeDescriptor.ElementAlternative, new ExpectedServiceElement[] { 
                            new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null),
                            new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null)
                        }),
                        new ExpectedServiceElement(ElementType.ElementAlternative, ElementTypeDescriptor.ElementAlternative, new ExpectedServiceElement[] { 
                            new ExpectedServiceElement(ElementType.Nil, ElementTypeDescriptor.Nil, null),
                        })}),
                    new ExpectedServiceAttribute(2, ElementType.UInt8, ElementTypeDescriptor.UnsignedInteger, 0x99 )
            };
            DoTest(expected, Data_SimpleRecords.OneAltWith__OneAltWithTwoNil_And_OneAltWithOneNil__And_Int8);
        }

        //--------------------------------------------------------------
        [Test]
        public void OneUrl()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { new ExpectedServiceAttribute(Data_SimpleRecords.IdF123Uint, ElementType.Url, ElementTypeDescriptor.Url, 
                new Uri(Data_SimpleRecords.OneUrlStringValue)) };
            DoTest(expected, Data_SimpleRecords.OneUrl);
        }

        [Test]
        public void OneUrlLazyCreation()
        {
            ExpectedServiceAttribute[] expected = new ExpectedServiceAttribute[] { new ExpectedServiceAttribute(Data_SimpleRecords.IdF123Uint, ElementType.Url, ElementTypeDescriptor.Url, 
                Data_SimpleRecords.OneUrlByteArrayValue) };
            ServiceRecord record = DoTestLazyUrlCreation(expected, Data_SimpleRecords.OneUrl);
            Assert.AreEqual(Data_SimpleRecords.OneUrl, record.SourceBytes, "SourceBytes");
        }

    }//class

}
