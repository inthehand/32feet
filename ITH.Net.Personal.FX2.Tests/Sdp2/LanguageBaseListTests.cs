using System;
using NUnit.Framework;
using System.Text;
//using System.Collections.Generic;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.AttributeIds;

namespace InTheHand.Net.Tests.Sdp2
{

    [TestFixture]
    public class LanguageBaseList_Construction
    {
        //--------------------------------------------------------------
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BadBaseUshort()
        {
            ushort badBase = 0;
            new LanguageBaseItem(1, 2, badBase);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BadBaseAttrId()
        {
            ServiceAttributeId badBase = 0;
            new LanguageBaseItem(1, 2, badBase);
        }

        //--------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = LanguageBaseItem.ErrorMsgLangMustAsciiTwoChars)]
        public void BadFromStringEmpty()
        {
            new LanguageBaseItem(String.Empty, 2252, LanguageBaseItem.PrimaryLanguageBaseAttributeId);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = LanguageBaseItem.ErrorMsgLangMustAsciiTwoChars)]
        public void BadFromStringThree()
        {
            new LanguageBaseItem("eng", 2252, LanguageBaseItem.PrimaryLanguageBaseAttributeId);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = LanguageBaseItem.ErrorMsgLangMustAsciiTwoChars)]
        public void BadFromStringUtf8OneCharTwoBytes()
        {
            new LanguageBaseItem("\u00E0", 2252, LanguageBaseItem.PrimaryLanguageBaseAttributeId);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = LanguageBaseItem.ErrorMsgLangMustAsciiTwoChars)]
        public void BadFromStringUtf8OneCharThreeBytes()
        {
            new LanguageBaseItem("\u201d", 2252, LanguageBaseItem.PrimaryLanguageBaseAttributeId);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = LanguageBaseItem.ErrorMsgLangMustAsciiTwoChars)]
        public void BadFromStringUtf8TwoCharsFourBytes()
        {
            new LanguageBaseItem("e\u201d", 2252, LanguageBaseItem.PrimaryLanguageBaseAttributeId);
        }

        //--------------------------------------------------------------

        [Test]
        public void FromStringA()
        {
            LanguageBaseItem item = new LanguageBaseItem("en", 2252, LanguageBaseItem.PrimaryLanguageBaseAttributeId);
            Assert.AreEqual(Data_LanguageBaseList.LangStringEn, item.NaturalLanguage);
            Assert.AreEqual(Data_LanguageBaseList.LangEn, item.NaturalLanguageAsUInt16);
            Assert.AreEqual((Int16)Data_LanguageBaseList.LangEn, item.NaturalLanguageAsInt16);
        }

        [Test]
        public void FromStringB()
        {
            LanguageBaseItem item = new LanguageBaseItem("fr", 2252, LanguageBaseItem.PrimaryLanguageBaseAttributeId);
            Assert.AreEqual(Data_LanguageBaseList.LangStringFr, item.NaturalLanguage);
            Assert.AreEqual(Data_LanguageBaseList.LangFr, item.NaturalLanguageAsUInt16);
            Assert.AreEqual((Int16)Data_LanguageBaseList.LangFr, item.NaturalLanguageAsInt16);
        }

        [Test]
        public void FromNumberA()
        {
            LanguageBaseItem item = new LanguageBaseItem(Data_LanguageBaseList.LangEn, 2252, LanguageBaseItem.PrimaryLanguageBaseAttributeId);
            Assert.AreEqual(Data_LanguageBaseList.LangEn, item.NaturalLanguageAsUInt16);
            Assert.AreEqual((Int16)Data_LanguageBaseList.LangEn, item.NaturalLanguageAsInt16);
            Assert.AreEqual(Data_LanguageBaseList.LangStringEn, item.NaturalLanguage);
        }

        [Test]
        public void FromNumberB()
        {
            LanguageBaseItem item = new LanguageBaseItem(Data_LanguageBaseList.LangFr, 2252, LanguageBaseItem.PrimaryLanguageBaseAttributeId);
            Assert.AreEqual(Data_LanguageBaseList.LangFr, item.NaturalLanguageAsUInt16);
            Assert.AreEqual((Int16)Data_LanguageBaseList.LangFr, item.NaturalLanguageAsInt16);
            Assert.AreEqual(Data_LanguageBaseList.LangStringFr, item.NaturalLanguage);
        }

        [Test]
        public void FromNumberBSignedValues()
        {
            LanguageBaseItem item = new LanguageBaseItem((Int16)Data_LanguageBaseList.LangFr, (Int16)2252, LanguageBaseItem.PrimaryLanguageBaseAttributeId);
            Assert.AreEqual(Data_LanguageBaseList.LangFr, item.NaturalLanguageAsUInt16);
            Assert.AreEqual((Int16)Data_LanguageBaseList.LangFr, item.NaturalLanguageAsInt16);
            Assert.AreEqual(Data_LanguageBaseList.LangStringFr, item.NaturalLanguage);
        }

    }


    public 
#if ! FX1_1
        static 
#endif
        class Data_LanguageBaseList
    {
        public const UInt16 LangBaseAttrId = 0x0006;
        //
        public const UInt16 LangEn = 0x656e; // "en"
        public const UInt16 LangFr = 0x6672; // "fr"
        public const String LangStringEn = "en";
        public const String LangStringFr = "fr";
        //
        public const UInt16 EncUtf8 = 0x006a;
        public const UInt16 EncWindows1252 = 0x08cc;
        //
        public const UInt16 DefaultBaseId = 0x0100;


        public static readonly ServiceAttribute AttrOneItem = new ServiceAttribute(LangBaseAttrId,
            new ServiceElement(ElementType.ElementSequence,
                new ServiceElement[] {
                    new ServiceElement(ElementType.UInt16, LangEn),
                    new ServiceElement(ElementType.UInt16, EncUtf8),
                    new ServiceElement(ElementType.UInt16, (UInt16)0x0100),
                })
            );

        public static readonly ServiceAttribute AttrTwoItems = new ServiceAttribute(LangBaseAttrId,
            new ServiceElement(ElementType.ElementSequence,
                new ServiceElement[] {
                    new ServiceElement(ElementType.UInt16, LangEn),
                    new ServiceElement(ElementType.UInt16, EncUtf8),
                    new ServiceElement(ElementType.UInt16, (UInt16)0x0100),
                    new ServiceElement(ElementType.UInt16, LangFr),
                    new ServiceElement(ElementType.UInt16, EncWindows1252),
                    new ServiceElement(ElementType.UInt16, (UInt16)0x0110),
                })
            );

        public static readonly ServiceAttribute AttrOneItemBadBaseZero = new ServiceAttribute(LangBaseAttrId,
            new ServiceElement(ElementType.ElementSequence,
                new ServiceElement[] {
                    new ServiceElement(ElementType.UInt16, LangEn),
                    new ServiceElement(ElementType.UInt16, EncUtf8),
                    new ServiceElement(ElementType.UInt16, (UInt16)00),
                })
            );

        public static readonly ServiceAttribute AttrOneItemBadHasUInt32 = new ServiceAttribute(LangBaseAttrId,
            new ServiceElement(ElementType.ElementSequence,
                new ServiceElement[] {
                    new ServiceElement(ElementType.UInt16, LangEn),
                    new ServiceElement(ElementType.UInt32, (UInt32)EncUtf8),
                    new ServiceElement(ElementType.UInt16, (UInt16)0x0100),
                })
            );

        public static readonly ServiceAttribute AttrOneItemBadNotSeq = new ServiceAttribute(LangBaseAttrId,
            new ServiceElement(ElementType.ElementAlternative,
                new ServiceElement[] {
                    new ServiceElement(ElementType.UInt16, LangEn),
                    new ServiceElement(ElementType.UInt32, (UInt32)EncUtf8),
                    new ServiceElement(ElementType.UInt16, (UInt16)0x0100),
                })
            );

        public static readonly byte[] RecordTwoItemsAsBytes = {
            0x35, 23,
            0x09, (LangBaseAttrId >> 8), (LangBaseAttrId & 0xFF), // attrId
            0x35, 18,
            0x09, (byte)'e',(byte)'n',/**/0x09, 0x00, 0x6A,/**/0x09, 0x01, 0x00,
            0x09, (byte)'f',(byte)'r',/**/0x09, 0x08, 0xcc,/**/0x09, 0x01, 0x10,
        };

        public static readonly ServiceAttribute AttrZeroElements = new ServiceAttribute(LangBaseAttrId,
            new ServiceElement(ElementType.ElementSequence,
                new ServiceElement[] { //oops...
                })
            );

        public static readonly ServiceAttribute AttrFourElements = new ServiceAttribute(LangBaseAttrId,
            new ServiceElement(ElementType.ElementSequence,
                new ServiceElement[] {
                    new ServiceElement(ElementType.UInt16, LangEn),
                    new ServiceElement(ElementType.UInt16, EncUtf8),
                    new ServiceElement(ElementType.UInt16, (UInt16)0x0100),
                    // oops...
                    new ServiceElement(ElementType.UInt16, LangEn),
                })
            );

    }//class

    
    [TestFixture]
    public class LanguageBaseList_Parse
    {

        //--------------------------------------------------------------
        [Test]
        [ExpectedException(typeof(System.Net.ProtocolViolationException), ExpectedMessage = "LanguageBaseAttributeIdList must contain items in groups of three.")]
        public void ZeroElements()
        {
            LanguageBaseItem[] items = LanguageBaseItem.ParseListFromElementSequence(Data_LanguageBaseList.AttrZeroElements.Value);
        }

        [Test]
        [ExpectedException(typeof(System.Net.ProtocolViolationException), ExpectedMessage = "LanguageBaseAttributeIdList must contain items in groups of three.")]
        public void FourElements()
        {
            LanguageBaseItem[] items = LanguageBaseItem.ParseListFromElementSequence(Data_LanguageBaseList.AttrFourElements.Value);
        }

        //--------------------------------------------------------------

        [Test]
        public void OneItem()
        {
            LanguageBaseItem[] items = LanguageBaseItem.ParseListFromElementSequence(Data_LanguageBaseList.AttrOneItem.Value);
            Assert.AreEqual(1, items.Length);
            Assert.AreEqual(Data_LanguageBaseList.LangStringEn, items[0].NaturalLanguage, "NaturalLanguage");
            Assert.AreEqual(Data_LanguageBaseList.LangEn, items[0].NaturalLanguageAsUInt16, "NaturalLanguageUInt16");
            Assert.AreEqual(Data_LanguageBaseList.EncUtf8, items[0].EncodingId, "EncodingId");
            Assert.AreEqual(Data_LanguageBaseList.EncUtf8, items[0].EncodingIdAsInt16, "EncodingId");
            Assert.AreEqual((ServiceAttributeId)0x0100, items[0].AttributeIdBase, "AttributeIdBase");
        }

        [Test]
        public void TwoItems()
        {
            LanguageBaseItem[] items = LanguageBaseItem.ParseListFromElementSequence(Data_LanguageBaseList.AttrTwoItems.Value);
            Assert.AreEqual(2, items.Length);
            Assert.AreEqual(Data_LanguageBaseList.LangStringEn, items[0].NaturalLanguage, "NaturalLanguage");
            Assert.AreEqual(Data_LanguageBaseList.LangEn, items[0].NaturalLanguageAsUInt16, "NaturalLanguageUInt16");
            Assert.AreEqual(Data_LanguageBaseList.EncUtf8, items[0].EncodingId, "EncodingId");
            Assert.AreEqual((ServiceAttributeId)0x0100, items[0].AttributeIdBase, "AttributeIdBase");
            //
            Assert.AreEqual(Data_LanguageBaseList.LangStringFr, items[1].NaturalLanguage, "NaturalLanguage");
            Assert.AreEqual(Data_LanguageBaseList.LangFr, items[1].NaturalLanguageAsUInt16, "NaturalLanguageUInt16");
            Assert.AreEqual(Data_LanguageBaseList.EncWindows1252, items[1].EncodingId, "EncodingId");
            Assert.AreEqual((ServiceAttributeId)0x0110, items[1].AttributeIdBase, "AttributeIdBase");
        }

        //--------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(System.Net.ProtocolViolationException), ExpectedMessage = LanguageBaseItem.ErrorMsgLangBaseListParseBaseInvalid)]
        public void OneItemBadBaseZero()
        {
            LanguageBaseItem[] items = LanguageBaseItem.ParseListFromElementSequence(Data_LanguageBaseList.AttrOneItemBadBaseZero.Value);
        }

        [Test]
        [ExpectedException(typeof(System.Net.ProtocolViolationException), ExpectedMessage = LanguageBaseItem.ErrorMsgLangBaseListParseNotU16)]
        public void OneItemBadHasUInt32()
        {
            LanguageBaseItem[] items = LanguageBaseItem.ParseListFromElementSequence(Data_LanguageBaseList.AttrOneItemBadHasUInt32.Value);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "LanguageBaseAttributeIdList elementSequence not an ElementSequence.")]
        public void OneItemBadNotSeq()
        {
            LanguageBaseItem[] items = LanguageBaseItem.ParseListFromElementSequence(Data_LanguageBaseList.AttrOneItemBadNotSeq.Value);
        }

    }//class


    [TestFixture]
    public class LanguageBaseList_EncodingFromIetfCharsetId
    {

        /// <summary>
        /// Check that the expected Encoding is mapped from the given IETF charset id,
        /// as are used in the LanguageBaseList Attribute.
        /// </summary>
        private void DoTest(Encoding expectedEncoding, ushort ietfCharsetId)
        {
            LanguageBaseItem item =
                new LanguageBaseItem(/*"en"*/Data_LanguageBaseList.LangEn, ietfCharsetId, (ServiceAttributeId)0x0100);
            Encoding encResult = item.GetEncoding();
            Assert.AreEqual(expectedEncoding, encResult);
        }


        //--------------------------------------------------------------
        [Test]
        [ExpectedException(typeof(NotSupportedException),
          ExpectedMessage = "Unrecognized character encoding (1); add to LanguageBaseItem mapping table.")]
        public void Unknown_1()
        {
            DoTest(null, 1);
        }

        [Test]
        public void UTF8_0x006a()
        {
            DoTest(Encoding.UTF8, 0x006a);
        }

        [Test]
        public void Windows1252_0x08cc()
        {
            DoTest(Encoding.GetEncoding(1252), 0x08cc);
        }

#if ! PocketPC
        [Test]
        public void Windows1258_0x08D2()
        {
            DoTest(Encoding.GetEncoding(1258), 0x08d2);
        }

        [Test]
        public void Latin1_iso1_4()
        {
            DoTest(Encoding.GetEncoding(28591), 4);
        }

        [Test]
        public void Latin5_iso9_12()
        {
            DoTest(Encoding.GetEncoding(28599), 12);
        }

        [Test]
        public void Latin9_iso15_111()
        {
            DoTest(Encoding.GetEncoding(28605), 111);
        }
#endif


        [Test]
        public void TestAllDefinedCharsetMappingRows()
        {
            int numberSuccessful, numberFailed;
            String resultText = 
                LanguageBaseItem.TestAllDefinedEncodingMappingRows(out numberSuccessful, out numberFailed);
            int successExpected = 16;
            int failedExpected = 3;
#if NETCF
            successExpected = 10;
            failedExpected = 9;
#endif
            Assert.AreEqual(successExpected, numberSuccessful, "numberSuccessful");
            Assert.AreEqual(failedExpected, numberFailed, "numberFailed");
            //-Console.WriteLine(resultText);
        }
    }//class
    
}
