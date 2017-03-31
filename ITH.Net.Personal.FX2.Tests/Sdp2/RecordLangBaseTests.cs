using System;
using NUnit.Framework;
//using System.Text;
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
using InTheHand.Net.Bluetooth.AttributeIds;
using InTheHand.Net.Bluetooth;

namespace InTheHand.Net.Tests.Sdp2
{

    [TestFixture]
    public class LanguageBase_RecordMethod_GetList
    {
        [Test]
        public void TwoItemsAsArrayToParse()
        {
            ServiceRecord record = new ServiceRecordParser().Parse(Data_LanguageBaseList.RecordTwoItemsAsBytes);
            LanguageBaseItem[] items = record.GetLanguageBaseList();
            Assert.AreEqual(2, items.Length);
            //
            Assert.AreEqual(Data_LanguageBaseList.LangStringEn, items[0].NaturalLanguage, "NaturalLanguage");
            Assert.AreEqual(Data_LanguageBaseList.EncUtf8, items[0].EncodingId, "EncodingId");
            Assert.AreEqual((ServiceAttributeId)0x0100, items[0].AttributeIdBase, "AttributeIdBase");
            //
            Assert.AreEqual(Data_LanguageBaseList.LangStringFr, items[1].NaturalLanguage, "NaturalLanguage");
            Assert.AreEqual(Data_LanguageBaseList.EncWindows1252, items[1].EncodingId, "EncodingId");
            Assert.AreEqual((ServiceAttributeId)0x0110, items[1].AttributeIdBase, "AttributeIdBase");
        }

        [Test]
        public void NoSuchAttributeInRecord()
        {
            IList_ServiceAttribute list
                = new List_ServiceAttribute(0);
            ServiceRecord record = new ServiceRecord(list);
            LanguageBaseItem[] items = record.GetLanguageBaseList();
            Assert.IsNotNull(items, "GetLanguageBaseList() should never be null.");
            Assert.AreEqual(0, items.Length, "items.Length");
        }

        [Test]
        //[ExpectedException(typeof(System.Net.ProtocolViolationException), "LanguageBaseList attribute not of type ElementSequence.")]
        public void LangAttrNotElementTypeSeq()
        {
            List_ServiceAttribute list = new List_ServiceAttribute();
            list.Add(new ServiceAttribute(UniversalAttributeId.LanguageBaseAttributeIdList,
                new ServiceElement(ElementType.TextString, new byte[] { (byte)'f', (byte)'o', (byte)'o', })));
            ServiceRecord record = new ServiceRecord(list);
            LanguageBaseItem[] items = record.GetLanguageBaseList();
            Assert.IsNotNull(items, "GetLanguageBaseList() should never be null.");
            Assert.AreEqual(0, items.Length, "items.Length");
        }

        [Test]
        public void LangAttrInvalid()
        {
            IList_ServiceAttribute list = new List_ServiceAttribute();
            list.Add(Data_LanguageBaseList.AttrFourElements);
            ServiceRecord record = new ServiceRecord(list);
            LanguageBaseItem[] items = record.GetLanguageBaseList();
            Assert.IsNotNull(items, "GetLanguageBaseList() should never be null.");
            Assert.AreEqual(0, items.Length, "items.Length");
        }

    }//class


    [TestFixture]
    public class LanguageBase_RecordMethod_GetPrimary
    {
        [Test]
        public void TwoItemsAsArrayToParse()
        {
            ServiceRecord record = new ServiceRecordParser().Parse(Data_LanguageBaseList.RecordTwoItemsAsBytes);
            LanguageBaseItem item = record.GetPrimaryLanguageBaseItem();
            //
            Assert.AreEqual(Data_LanguageBaseList.LangStringEn, item.NaturalLanguage, "NaturalLanguage");
            Assert.AreEqual(Data_LanguageBaseList.EncUtf8, item.EncodingId, "EncodingId");
            Assert.AreEqual((ServiceAttributeId)0x0100, item.AttributeIdBase, "AttributeIdBase");
        }

        [Test]
        public void NoSuchAttributeInRecord()
        {
            IList_ServiceAttribute list
                = new List_ServiceAttribute(0);
            ServiceRecord record = new ServiceRecord(list);
            LanguageBaseItem item = record.GetPrimaryLanguageBaseItem();
            Assert.IsNull(item, "IsNull--item");
        }

        [Test]
        //[ExpectedException(typeof(System.Net.ProtocolViolationException), "LanguageBaseList attribute not of type ElementSequence.")]
        public void LangAttrNotElementTypeSeq()
        {
            List_ServiceAttribute list = new List_ServiceAttribute();
            list.Add(new ServiceAttribute(UniversalAttributeId.LanguageBaseAttributeIdList,
                new ServiceElement(ElementType.TextString, new byte[] { (byte)'f', (byte)'o', (byte)'o', })));
            ServiceRecord record = new ServiceRecord(list);
            LanguageBaseItem item = record.GetPrimaryLanguageBaseItem();
            Assert.IsNull(item, "GetPrimaryLanguageBaseItem() should be null.");
        }

        [Test]
        public void LangAttrInvalid()
        {
            IList_ServiceAttribute list = new List_ServiceAttribute();
            list.Add(Data_LanguageBaseList.AttrFourElements);
            ServiceRecord record = new ServiceRecord(list);
            LanguageBaseItem item = record.GetPrimaryLanguageBaseItem();
            Assert.IsNull(item, "GetPrimaryLanguageBaseItem() should be null.");
        }

        [Test]
        public void LangAttrNoPrimary0100Item()
        {
            IList_ServiceAttribute list = new List_ServiceAttribute();
            list.Add(Data_LanguageBaseList.AttrOneItemBadBaseZero);
            ServiceRecord record = new ServiceRecord(list);
            LanguageBaseItem item = record.GetPrimaryLanguageBaseItem();
            Assert.IsNull(item, "GetPrimaryLanguageBaseItem() should be null.");
        }

    }//class

}