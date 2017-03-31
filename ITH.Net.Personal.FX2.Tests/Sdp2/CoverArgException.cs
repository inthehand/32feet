using System;
using  NUnit.Framework;
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
    public class CoverArgException
    {
        //public delegate void CoverStreamLikeReadOrWriteMethod(byte[]buf, int offset, int length);
        public delegate object CoverStreamLikeReadOrWriteMethod(byte[] buf, int offset, int length);

        public static void CoverStreamLikeReadOrWrite(CoverStreamLikeReadOrWriteMethod method)
        {
            byte[] data = null;
            try {
                method(null, 0, 1);
                Assert.Fail("buf=null did not throw!");
            } catch (ArgumentNullException) { }
            data = new byte[5];
            try {
                method(data, 0, 0);
                Assert.Fail("length=0 did not throw!");
            } catch (ArgumentOutOfRangeException) { }
            try {
                method(data, -1, 1);
                Assert.Fail("offset=-1 did not throw!");
            } catch (ArgumentOutOfRangeException) { }
            try {
                method(data, 0, 6);
                Assert.Fail("overrun did not throw!");
            } catch (ArgumentException) { }
            try {
                method(data, 2, 4);
                Assert.Fail("overrun with offset did not throw!");
            } catch (ArgumentException) { }
        }

        //--------------------------------------------------------------

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void Parser_Parse1Null() { new ServiceRecordParser().Parse(null); }

#if ! FX1_1
        [Test]
        public void Parser_ParseNull() {
            CoverStreamLikeReadOrWriteMethod method = new ServiceRecordParser().Parse;
            CoverStreamLikeReadOrWrite(method);
        }
#endif

        [Test]
        [ExpectedException(typeof(System.Net.ProtocolViolationException), ExpectedMessage = "The top element must be a Element Sequence type.")]
        public void Parser_ParseNonElementSeqThusNotCompleteRecord()
        {
            byte[] buf = { 0x09, 0x12, 0x34 };
            new ServiceRecordParser().Parse(buf);
        }

        //--------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException), ExpectedMessage = "Unknown ElementType 'Unknown'."
            +"\r\nParameter name: type")]
        public void EtdFromTypeUnknown()
        {
            ServiceRecordParser.GetEtdForType(ElementType.Unknown);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException), ExpectedMessage = "Unknown ElementType '10000'."
            + "\r\nParameter name: type")]
        public void EtdFromTypeInvalid()
        {
            ServiceRecordParser.GetEtdForType((ElementType)10000);
        }

        //--------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Type ElementSequence and ElementAlternative need an list of ServiceElement.")]
        public void Element_Ctor_NonListForElementSeqAlt()
        {
            new ServiceElement(ElementType.ElementSequence, "booooom");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Type ElementSequence and ElementAlternative must be used for an list of ServiceElement.")]
        public void Element_Ctor_ListForNonElementSeqAlt()
        {
            new ServiceElement(ElementType.Int16, new List_ServiceElement());
        }

        private ServiceElement CreateInt16ServiceElement()
        {
            return new ServiceElement(ElementType.Int16, (Int16)11);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void Element_GetStringENull()
        {
            Encoding enc = null;
            CreateInt16ServiceElement().GetValueAsString(enc);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void Element_GetStringLNull()
        {
            LanguageBaseItem enc = null;
            CreateInt16ServiceElement().GetValueAsString(enc);
        }

        //--------------------------------------------------------------

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void Element_GetWrongType_String()
        {
            Encoding enc = Encoding.UTF8;
            CreateInt16ServiceElement().GetValueAsString(enc);
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void Element_GetWrongType_Uuid()
        {
            CreateInt16ServiceElement().GetValueAsUuid();
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void Element_GetWrongType_ElemArray()
        {
            object foo = CreateInt16ServiceElement().GetValueAsElementArray();
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void Element_GetWrongType_ElemList()
        {
            object foo = CreateInt16ServiceElement().GetValueAsElementList();
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void Element_GetWrongType_Uri()
        {
            object foo = CreateInt16ServiceElement().GetValueAsUri();
        }

        //--------------------------------------------------------------

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void Utils_Dump_NullWriter()
        {
            ServiceRecord rec = new ServiceRecord(new List_ServiceAttribute());
            ServiceRecordUtilities.Dump(null, rec);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void Utils_Dump_NullRecord()
        {
            ServiceRecordUtilities.Dump(new System.IO.StringWriter(), null);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void Utils_DumpRaw_NullWriter()
        {
            ServiceRecord rec = new ServiceRecord(new List_ServiceAttribute());
            ServiceRecordUtilities.DumpRaw(null, rec);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void Utils_DumpRaw_NullRecord()
        {
            ServiceRecordUtilities.DumpRaw(new System.IO.StringWriter(), null);
        }

        //--------------------------------------------------------------

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void AttributeIdLookup_NullType()
        {
            LanguageBaseItem langBase;
            AttributeIdLookup.GetName(UniversalAttributeId.ProtocolDescriptorList,
                null, new LanguageBaseItem[0], out langBase);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void AttributeIdLookup_NullLangBaseList()
        {
            LanguageBaseItem langBase;
            AttributeIdLookup.GetName(UniversalAttributeId.ProtocolDescriptorList,
                new Type[] { typeof(UniversalAttributeId) },
                null, out langBase);
        }

    }//class
}
