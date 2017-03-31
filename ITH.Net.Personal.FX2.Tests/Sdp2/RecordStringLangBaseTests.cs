using System;
using NUnit.Framework;
using System.Text;
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
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.AttributeIds;

namespace InTheHand.Net.Tests.Sdp2
{

    [TestFixture]
    public class RecordStringLanguageBase
    {

        public const string CrLf = "\r\n";

        public static readonly byte[] RecordBytesEnIs = {
            0x35, 41,
                0x09, 0x00, 0x06,   // Languages
                0x35, 18,
                    0x09, 0x69, 0x73,   // "is"
                    0x09, 0x08, 0xcc,   // windows-1252
                    0x09, 0x01, 0x10,   // base=0x0110
                    0x09, 0x65, 0x6e,   // "en"
                    0x09, 0x00, 0x6a,   // utf-8
                    0x09, 0x01, 0x00,   // base=0x0100
                0x09, 0x01, 0x00,   // SvcName/En
                0x25, 4, (byte)'a', (byte)'b', (byte)'c', (byte)'d', 
                0x09, 0x01, 0x10,   // SvcName/IS
                0x25, 4, (byte)'a', (byte)'b', (byte)'c', (byte)'e', 
        };
        public static readonly byte[] RecordBytesEnIsEncodings = {
            0x35, 46,
                0x09, 0x00, 0x06,   // Languages
                0x35, 18,
                    0x09, 0x69, 0x73,   // "is"
                    0x09, 0x08, 0xcc,   // windows-1252
                    0x09, 0x01, 0x10,   // base=0x0110
                    0x09, 0x65, 0x6e,   // "en"
                    0x09, 0x00, 0x6a,   // utf-8
                    0x09, 0x01, 0x00,   // base=0x0100
                0x09, 0x01, 0x00,   // SvcName/En
                0x25, 7, (byte)'a', (byte)'b', 0xE2, 0x80, 0xA0, (byte)'c', (byte)'d', 
                0x09, 0x01, 0x10,   // SvcName/IS
                0x25, 6, (byte)'a', (byte)'b', 0xD0, (byte)'c', 0xDE, (byte)'e', 
        };
        public static readonly byte[] RecordBytesEnIsEncodingsNoLangBaseItems = {
            0x35, 23,
                // No LangBaseItems!!!
                //0x09, 0x00, 0x06,   // Languages
                //0x35, 18,
                //    0x09, 0x69, 0x73,   // "is"
                //    0x09, 0x08, 0xcc,   // windows-1252
                //    0x09, 0x01, 0x10,   // base=0x0110
                //    0x09, 0x65, 0x6e,   // "en"
                //    0x09, 0x00, 0x6a,   // utf-8
                //    0x09, 0x01, 0x00,   // base=0x0100
                0x09, 0x01, 0x00,   // SvcName/En
                0x25, 7, (byte)'a', (byte)'b', 0xE2, 0x80, 0xA0, (byte)'c', (byte)'d', 
                0x09, 0x01, 0x10,   // SvcName/IS
                0x25, 6, (byte)'a', (byte)'b', 0xD0, (byte)'c', 0xDE, (byte)'e', 
        };


        [Test]
        public void EnIsSimple()
        {
            String expectedEn = "abcd";
            String expectedIs = "abce";
            byte[] buffer = RecordBytesEnIs;
            ServiceRecord record = new ServiceRecordParser().Parse(buffer);
            LanguageBaseItem[] langList = record.GetLanguageBaseList();
            //
            LanguageBaseItem langIs = langList[0];
            Assert.AreEqual("is", langIs.NaturalLanguage);
            LanguageBaseItem langEn = langList[1];
            Assert.AreEqual("en", langEn.NaturalLanguage);
            //
            ServiceAttribute attrEn = record.GetAttributeByIndex(1);
            ServiceAttribute attrIs = record.GetAttributeByIndex(2);
            Assert.AreEqual((ServiceAttributeId)0x0100, attrEn.Id);
            Assert.AreEqual((ServiceAttributeId)0x0110, attrIs.Id);
            String resultEn = attrEn.Value.GetValueAsString(langEn);
            String resultIs = attrIs.Value.GetValueAsString(langIs);
            Assert.AreEqual(expectedEn, resultEn);
            Assert.AreEqual(expectedIs, resultIs);
        }

        [Test]
        public void EnIsEncodings()
        {
            String expectedEn = "ab\u2020cd";
            String expectedIs = "ab\u00D0c\u00DEe";
            byte[] buffer = RecordBytesEnIsEncodings;
            ServiceRecord record = new ServiceRecordParser().Parse(buffer);
            LanguageBaseItem[] langList = record.GetLanguageBaseList();
            //
            LanguageBaseItem langIs = langList[0];
            Assert.AreEqual("is", langIs.NaturalLanguage);
            LanguageBaseItem langEn = langList[1];
            Assert.AreEqual("en", langEn.NaturalLanguage);
            //
            ServiceAttribute attrEn = record.GetAttributeByIndex(1);
            ServiceAttribute attrIs = record.GetAttributeByIndex(2);
            Assert.AreEqual((ServiceAttributeId)0x0100, attrEn.Id);
            Assert.AreEqual((ServiceAttributeId)0x0110, attrIs.Id);
            String resultEn = attrEn.Value.GetValueAsString(langEn);
            String resultIs = attrIs.Value.GetValueAsString(langIs);
            Assert.AreEqual(expectedEn, resultEn);
            Assert.AreEqual(expectedIs, resultIs);
        }

        [Test]
        public void EnIsEncodingsById()
        {
            String expectedEn = "ab\u2020cd";
            String expectedIs = "ab\u00D0c\u00DEe";
            byte[] buffer = RecordBytesEnIsEncodings;
            ServiceRecord record = new ServiceRecordParser().Parse(buffer);
            LanguageBaseItem[] langList = record.GetLanguageBaseList();
            //
            LanguageBaseItem langIs = langList[0];
            Assert.AreEqual("is", langIs.NaturalLanguage);
            LanguageBaseItem langEn = langList[1];
            Assert.AreEqual("en", langEn.NaturalLanguage);
            //
            ServiceAttribute attrEn = record.GetAttributeById(0, langEn);
            ServiceAttribute attrIs = record.GetAttributeById(0, langIs);
            Assert.AreEqual((ServiceAttributeId)0x0100, attrEn.Id);
            Assert.AreEqual((ServiceAttributeId)0x0110, attrIs.Id);
            String resultEn = attrEn.Value.GetValueAsString(langEn);
            String resultIs = attrIs.Value.GetValueAsString(langIs);
            Assert.AreEqual(expectedEn, resultEn);
            Assert.AreEqual(expectedIs, resultIs);
        }

        [Test]
        public void EnIsEncodingsRecordGetStringByIdAndLang()
        {
            String expectedEn = "ab\u2020cd";
            String expectedIs = "ab\u00D0c\u00DEe";
            byte[] buffer = RecordBytesEnIsEncodings;
            ServiceRecord record = new ServiceRecordParser().Parse(buffer);
            LanguageBaseItem[] langList = record.GetLanguageBaseList();
            //
            LanguageBaseItem langIs = langList[0];
            Assert.AreEqual("is", langIs.NaturalLanguage);
            LanguageBaseItem langEn = langList[1];
            Assert.AreEqual("en", langEn.NaturalLanguage);
            //
            // Here's the stuff really tested here!!!!
            String resultEn = record.GetMultiLanguageStringAttributeById(0, langEn);
            String resultIs = record.GetMultiLanguageStringAttributeById(0, langIs);
            Assert.AreEqual(expectedEn, resultEn);
            Assert.AreEqual(expectedIs, resultIs);
            String resultEnPrimary = record.GetPrimaryMultiLanguageStringAttributeById(0);
            Assert.AreEqual(expectedEn, resultEnPrimary);
        }

        [Test]
        public void EnIsEncodingsRecordGetStringByIdAndLangNoLangBaseItems()
        {
            String expectedEn = "ab\u2020cd";
            byte[] buffer = RecordBytesEnIsEncodingsNoLangBaseItems;
            ServiceRecord record = new ServiceRecordParser().Parse(buffer);
            //
            LanguageBaseItem[] langList = record.GetLanguageBaseList();
            Assert.AreEqual(0, langList.Length, "#LangItems==0");
            //
            String resultEnPrimary = record.GetPrimaryMultiLanguageStringAttributeById(0);
            Assert.AreEqual(expectedEn, resultEnPrimary);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "Value cannot be null." + CrLf + "Parameter name: language")]
        public void BadGetStringByIdAndLang_LangNull()
        {
            byte[] buffer = RecordBytesEnIsEncodings;
            ServiceRecord record = new ServiceRecordParser().Parse(buffer);
            record.GetMultiLanguageStringAttributeById(0, null);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Not TextString type.")]
        public void BadNonStringAttribute()
        {
            byte[] buffer = Data_CompleteThirdPartyRecords.Xp1Sdp;
            ServiceRecord record = new ServiceRecordParser().Parse(buffer);
            LanguageBaseItem[] langList = record.GetLanguageBaseList();
            //
            LanguageBaseItem langEn = langList[0];
            Assert.AreEqual("en", langEn.NaturalLanguage);
            //
            // There's a attribute 0x0201 of type UInt32, accessing it should fail
            String resultEn = record.GetMultiLanguageStringAttributeById((ServiceAttributeId)0x101, langEn);
            Assert.Fail("should have thrown!");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Not TextString type.")]
        public void BadNonStringAttributePrimary()
        {
            byte[] buffer = Data_CompleteThirdPartyRecords.Xp1Sdp;
            ServiceRecord record = new ServiceRecordParser().Parse(buffer);
            LanguageBaseItem[] langList = record.GetLanguageBaseList();
            //
            LanguageBaseItem langEn = langList[0];
            Assert.AreEqual("en", langEn.NaturalLanguage);
            //
            // There's a attribute 0x0201 of type UInt32, accessing it should fail
            String resultEn = record.GetPrimaryMultiLanguageStringAttributeById((ServiceAttributeId)0x101);
            Assert.Fail("should have thrown!");
        }

        [Test]
#if ! PocketPC
        [ExpectedException(typeof(System.Text.DecoderFallbackException),
            ExpectedMessage = "Unable to translate bytes [E2] at index 2 from specified code page to Unicode.")]
#endif
        public void BadStringEncodingNotAscii()
        {
            byte[] buffer = RecordBytesEnIsEncodings;
            ServiceRecord record = new ServiceRecordParser().Parse(buffer);
            //
            ServiceAttribute attr = record.GetAttributeById((ServiceAttributeId)0x0100);
            const ushort langEn = 0x656e;
            const ushort ietfAscii = 3;
            LanguageBaseItem langBase = new LanguageBaseItem(langEn, ietfAscii, (ServiceAttributeId)0x0999);
            String x = attr.Value.GetValueAsString(langBase);
        }

        //--------------------------------------------------------------

        static ServiceRecord CreateRecord(params ServiceAttribute[] attributes)
        {
            //ServiceRecord record = new ServiceRecord();
            //record.Add(id, element_);
            //
            List_ServiceAttribute list = new List_ServiceAttribute(attributes);
            ServiceRecord record = new ServiceRecord(list);
            //
            return record;
        }

        [Test]
        public void StringGivenString()
        {
            ushort LangCodeEn = 0x656e; // "en"
            ushort LangCodeIs = 0x6973; // "is"
            ushort EncodingIdUtf8 = 106;
            ushort EncodingIdUtf16BE = 1013;
            ushort BaseA = 0x0100;
            ushort BaseB = 0x0100;
            //
            String str = "ambds\u2022nbdas\u00FEdlka\U00012004slkda";
            ServiceElement element_ = new ServiceElement(ElementType.TextString, str);
            ServiceAttributeId id = ServiceRecord.CreateLanguageBasedAttributeId(UniversalAttributeId.ServiceName, (ServiceAttributeId)BaseA);
            ServiceAttribute attribute = new ServiceAttribute(id, element_);
            ServiceRecord record = CreateRecord(attribute);
            //
            LanguageBaseItem langBaseEn = new LanguageBaseItem(LangCodeEn, EncodingIdUtf8, BaseA);
            LanguageBaseItem langBaseIs = new LanguageBaseItem(LangCodeIs, EncodingIdUtf16BE, BaseB);
            Assert.AreEqual(str, record.GetMultiLanguageStringAttributeById(
                InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ServiceName, 
                langBaseEn));
            Assert.AreEqual(str, record.GetMultiLanguageStringAttributeById(
                InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ServiceName,
                langBaseIs));
            //
            ServiceElement element = record.GetAttributeById(UniversalAttributeId.ServiceName, langBaseIs).Value;
            Assert.AreEqual(ElementTypeDescriptor.TextString, element.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.TextString, element.ElementType);
            Assert.IsInstanceOfType(typeof(String), element.Value);
            Assert.AreEqual(str, element.GetValueAsStringUtf8());
            Assert.AreEqual(str, element.GetValueAsString(Encoding.ASCII));
            Assert.AreEqual(str, element.GetValueAsString(Encoding.Unicode));
        }

    }//class

}
