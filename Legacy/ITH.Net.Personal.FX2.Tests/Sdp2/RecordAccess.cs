using System;
using NUnit.Framework;
#if FX1_1
using IList_ServiceElement = System.Collections.IList;
using List_ServiceElement = System.Collections.ArrayList;
using IList_ServiceAttribute = System.Collections.IList;
using List_ServiceAttribute = System.Collections.ArrayList;
using IList_ServiceAttributeId = System.Collections.IList;
using List_ServiceAttributeId = System.Collections.ArrayList;
using IEnumerator_ServiceAttribute = System.Collections.IEnumerator;
#else
using IList_ServiceElement = System.Collections.Generic.IList<InTheHand.Net.Bluetooth.ServiceElement>;
using List_ServiceElement = System.Collections.Generic.List<InTheHand.Net.Bluetooth.ServiceElement>;
using IList_ServiceAttribute = System.Collections.Generic.IList<InTheHand.Net.Bluetooth.ServiceAttribute>;
using List_ServiceAttribute = System.Collections.Generic.List<InTheHand.Net.Bluetooth.ServiceAttribute>;
using IList_ServiceAttributeId = System.Collections.Generic.IList<InTheHand.Net.Bluetooth.ServiceAttributeId>;
using List_ServiceAttributeId = System.Collections.Generic.List<InTheHand.Net.Bluetooth.ServiceAttributeId>;
using IEnumerator_ServiceAttribute = System.Collections.Generic.IEnumerator<InTheHand.Net.Bluetooth.ServiceAttribute>;
using System.Collections.Generic; // e.g. KeyNotFoundException
#endif
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.AttributeIds;
#if FX1_1
using KeyNotFoundException = System.ArgumentException;
#endif


namespace InTheHand.Net.Tests.Sdp2
{

    public
#if ! FX1_1
        static
#endif
        class RecordAccess_Data
    {
        //--------
        public static ServiceRecord CreateRecordWithZeroItems()
        {
            IList_ServiceAttribute list = new List_ServiceAttribute();
            ServiceRecord record = new ServiceRecord(list);
            return record;
        }

        public static void RecordWithZeroItems_AssertIs(ServiceRecord record)
        {
            ServiceAttribute attr;
            Assert.AreEqual(0, record.Count);
            //
            IList_ServiceAttributeId ids = record.AttributeIds;
            Assert.AreEqual(0, ids.Count);
            //
            Assert.IsFalse(record.Contains(UniversalAttributeId.ServiceRecordHandle));
            //
            //TODO ((((test disabled for TryGetAttributeById)))
            //bool found = record.TryGetAttributeById(UniversalAttributeId.ServiceRecordHandle, out attr);
            //Assert.IsFalse(found);
            try {
                attr = record.GetAttributeById(UniversalAttributeId.ServiceRecordHandle);
                Assert.Fail("Record should contain no attribute.");
            } catch (KeyNotFoundException) { }
        }

        //--------
        internal static ServiceRecord CreateRecordWithOneItems()
        {
            IList_ServiceAttribute list = new List_ServiceAttribute();
            list.Add(new ServiceAttribute(UniversalAttributeId.ServiceRecordHandle,
                new ServiceElement(ElementType.UInt32, (UInt32)55)));
            ServiceRecord record = new ServiceRecord(list);
            return record;
        }

        internal static void RecordWithOneItem_AssertIsAttributeAt0(ServiceAttribute attr)
        {
            Assert.AreEqual(UniversalAttributeId.ServiceRecordHandle, attr.Id);
            Assert.AreEqual(ElementTypeDescriptor.UnsignedInteger, attr.Value.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.UInt32, attr.Value.ElementType);
            Assert.AreEqual(55, attr.Value.Value);
        }

        //--------
        public const int MultipleItemsCount = 3;
        internal static ServiceRecord CreateRecordWithMultipleItems()
        {
            IList_ServiceAttribute list = new List_ServiceAttribute();
            ServiceAttribute attr;
            List_ServiceElement elements;
            //
            attr = new ServiceAttribute(UniversalAttributeId.ServiceRecordHandle,
                new ServiceElement(ElementType.UInt32, (UInt32)66));
            list.Add(attr);
            //
            elements = new List_ServiceElement();
            elements.Add(new ServiceElement(ElementType.UInt16, (UInt16)77));
            attr = new ServiceAttribute(UniversalAttributeId.ServiceClassIdList,
                new ServiceElement(ElementType.ElementSequence, elements));
            list.Add(attr);
            //
            elements = new List_ServiceElement();
            elements.Add(new ServiceElement(ElementType.UInt16, Data_LanguageBaseList.LangEn));
            elements.Add(new ServiceElement(ElementType.UInt16, Data_LanguageBaseList.EncWindows1252));
            elements.Add(new ServiceElement(ElementType.UInt16, (UInt16)0x0100));
            elements.Add(new ServiceElement(ElementType.UInt16, Data_LanguageBaseList.LangEn));
            elements.Add(new ServiceElement(ElementType.UInt16, Data_LanguageBaseList.EncUtf8));
            elements.Add(new ServiceElement(ElementType.UInt16, (UInt16)0x0120));
            attr = new ServiceAttribute(UniversalAttributeId.LanguageBaseAttributeIdList,
                new ServiceElement(ElementType.ElementSequence, elements));
            list.Add(attr);
            //
            ServiceRecord record = new ServiceRecord(list);
            return record;
        }

        internal static void RecordWithMultipleItems_AssertIsAttributeAt0(ServiceAttribute attr)
        {
            Assert.AreEqual(UniversalAttributeId.ServiceRecordHandle, attr.Id);
            Assert.AreEqual(ElementTypeDescriptor.UnsignedInteger, attr.Value.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.UInt32, attr.Value.ElementType);
            Assert.AreEqual(66, attr.Value.Value);
        }

        internal static void RecordWithMultipleItems_AssertIsAttributeAt1(ServiceAttribute attr)
        {
            Assert.AreEqual(UniversalAttributeId.ServiceClassIdList, attr.Id);
            Assert.AreEqual(ElementTypeDescriptor.ElementSequence, attr.Value.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.ElementSequence, attr.Value.ElementType);
            //
            ServiceElement[] seq = attr.Value.GetValueAsElementArray();
            Assert.AreEqual(1, seq.Length);
            ServiceElement element = seq[0];
            Assert.AreEqual(ElementTypeDescriptor.UnsignedInteger, element.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.UInt16, element.ElementType);
            Assert.AreEqual(77, element.Value);
        }

        internal static void RecordWithMultipleItems_AssertIsAttributeAt2(ServiceAttribute attr)
        {
            Assert.AreEqual(UniversalAttributeId.LanguageBaseAttributeIdList, attr.Id);
            Assert.AreEqual(ElementTypeDescriptor.ElementSequence, attr.Value.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.ElementSequence, attr.Value.ElementType);
            //
            ServiceElement[] seq = attr.Value.GetValueAsElementArray();
            Assert.AreEqual(6, seq.Length);
            ServiceElement element = seq[0];
            Assert.AreEqual(ElementTypeDescriptor.UnsignedInteger, element.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.UInt16, element.ElementType);
            Assert.AreEqual(Data_LanguageBaseList.LangEn, element.Value);
        }

    }

    [TestFixture]
    public class RecordAccess_ZeroItems
    {

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateWithNull()
        {
            IList_ServiceAttribute list = null;
            ServiceRecord record = new ServiceRecord(list);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateWithNullRecordBytes()
        {
            byte[] recordBytes = null;
            ServiceRecord record = ServiceRecord.CreateServiceRecordFromBytes(recordBytes);
        }

        [Test]
        public void CreateWithZeroItems()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithZeroItems();
            RecordAccess_Data.RecordWithZeroItems_AssertIs(record);
        }

        [Test]
        public void CreateWithZeroItems_DefaultCtor()
        {
            ServiceRecord record = new ServiceRecord();
            RecordAccess_Data.RecordWithZeroItems_AssertIs(record);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AttrByIndex0()
        {
            ServiceAttribute attr;
            ServiceRecord record = RecordAccess_Data.CreateRecordWithZeroItems();
            //
            attr = record.GetAttributeByIndex(0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AttrByIndex10()
        {
            ServiceAttribute attr;
            ServiceRecord record = RecordAccess_Data.CreateRecordWithZeroItems();
            //
            attr = record.GetAttributeByIndex(10);
        }

        [Test]
        [ExpectedException(typeof(KeyNotFoundException), ExpectedMessage = ServiceRecord.ErrorMsgNoAttributeWithId)]
        public void AttrById()
        {
            ServiceAttribute attr;
            ServiceRecord record = RecordAccess_Data.CreateRecordWithZeroItems();
            //
            attr = record.GetAttributeById(UniversalAttributeId.ServiceRecordHandle);
        }

    }//class


    [TestFixture]
    public class RecordAccess_AccessWithLangBase
    {

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AttrByIdAndLangNull()
        {
            ServiceAttribute attr;
            ServiceRecord record = RecordAccess_Data.CreateRecordWithZeroItems();
            //
            attr = record.GetAttributeById(UniversalAttributeId.ServiceRecordHandle, null);
        }

        [Test]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void AttrByIdAndLangFake()
        {
            ServiceAttribute attr;
            ServiceRecord record = RecordAccess_Data.CreateRecordWithZeroItems();
            //
            LanguageBaseItem langFake = new LanguageBaseItem(1, 2, 3);
            attr = record.GetAttributeById(UniversalAttributeId.ServiceRecordHandle, langFake);
        }


        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ContainsByIdAndLangNull()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithZeroItems();
            //
            Assert.IsFalse(record.Contains(UniversalAttributeId.ServiceRecordHandle, null));
        }

        [Test]
        public void ContainsByIdAndLangFake()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithZeroItems();
            //
            LanguageBaseItem langFake = new LanguageBaseItem(1, 2, 3);
            Assert.IsFalse(record.Contains(UniversalAttributeId.ServiceRecordHandle, langFake));
        }


        // See also class LanguageBaseListFromRecordMethods
    }


    [TestFixture]
    public class RecordAccess_OneItem
    {

        [Test]
        public void AccessAttrByIndex0()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithOneItems();
            //
            Assert.AreEqual(1, record.Count);
            IList_ServiceAttributeId ids = record.AttributeIds;
            Assert.AreEqual(1, ids.Count);
            Assert.AreEqual(UniversalAttributeId.ServiceRecordHandle, ids[0]);
            //
            ServiceAttribute attr = record.GetAttributeByIndex(0);
            RecordAccess_Data.RecordWithOneItem_AssertIsAttributeAt0(attr);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AttrByIndex10()
        {
            ServiceAttribute attr;
            ServiceRecord record = RecordAccess_Data.CreateRecordWithOneItems();
            //
            attr = record.GetAttributeByIndex(10);
        }

        [Test]
        public void AccessAttrById()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithOneItems();
            //
            Assert.AreEqual(1, record.Count);
            //
            ServiceAttribute attr = record.GetAttributeById(UniversalAttributeId.ServiceRecordHandle);
            Assert.AreEqual(UniversalAttributeId.ServiceRecordHandle, attr.Id);
            Assert.AreEqual(ElementTypeDescriptor.UnsignedInteger, attr.Value.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.UInt32, attr.Value.ElementType);
            Assert.AreEqual(55, attr.Value.Value);
        }

        [Test]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void NonExistingAttrById()
        {
            ServiceAttribute attr;
            ServiceRecord record = RecordAccess_Data.CreateRecordWithOneItems();
            //
            attr = record.GetAttributeById(UniversalAttributeId.ServiceRecordState);
        }

    }//class


    [TestFixture]
    public class RecordAccess_MultipleItems
    {

        [Test]
        public void AccessAttrByIndex0()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithMultipleItems();
            //
            Assert.AreEqual(RecordAccess_Data.MultipleItemsCount, record.Count);
            IList_ServiceAttributeId ids = record.AttributeIds;
            Assert.AreEqual(RecordAccess_Data.MultipleItemsCount, ids.Count);
            Assert.AreEqual(UniversalAttributeId.ServiceRecordHandle, ids[0]);
            //
            ServiceAttribute attr = record.GetAttributeByIndex(0);
            RecordAccess_Data.RecordWithMultipleItems_AssertIsAttributeAt0(attr);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AttrByIndex10()
        {
            ServiceAttribute attr;
            ServiceRecord record = RecordAccess_Data.CreateRecordWithMultipleItems();
            //
            attr = record.GetAttributeByIndex(10);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AttrByIndexNegative()
        {
            ServiceAttribute attr;
            ServiceRecord record = RecordAccess_Data.CreateRecordWithMultipleItems();
            //
            attr = record.GetAttributeByIndex(-10);
        }

        [Test]
        public void AccessAttrById()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithMultipleItems();
            //
            Assert.AreEqual(RecordAccess_Data.MultipleItemsCount, record.Count);
            //
            ServiceAttribute attr = record.GetAttributeById(UniversalAttributeId.ServiceRecordHandle);
            Assert.AreEqual(UniversalAttributeId.ServiceRecordHandle, attr.Id);
            Assert.AreEqual(ElementTypeDescriptor.UnsignedInteger, attr.Value.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.UInt32, attr.Value.ElementType);
            Assert.AreEqual(66, attr.Value.Value);
        }

        [Test]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void NonExistingAttrById()
        {
            ServiceAttribute attr;
            ServiceRecord record = RecordAccess_Data.CreateRecordWithMultipleItems();
            //
            attr = record.GetAttributeById(UniversalAttributeId.ServiceRecordState);
        }

        //TODO RecordAccess_MultipleItems: StringMultiLangs

    }//class


    [TestFixture]
    public class RecordAccess_Enumerable
    {
        // Casts like the following are for NETCFv1 support, where there's no Generics.
        //   (ServiceAttribute)etor.Current;

        //-----------------------------
        [Test]
        public void ZeroCurrent()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithZeroItems();
            IEnumerator_ServiceAttribute etor = record.GetEnumerator();
            try {
                ServiceAttribute obj = (ServiceAttribute)etor.Current;
                Assert.Fail("should have thrown!");
            } catch (InvalidOperationException) { }
        }

        [Test]
        public void ZeroMoveNextFalse()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithZeroItems();
            IEnumerator_ServiceAttribute etor = record.GetEnumerator();
            Assert.IsFalse(etor.MoveNext());
        }

#if ! FX1_1
        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void ZeroDisposed()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithZeroItems();
            IEnumerator_ServiceAttribute etor = record.GetEnumerator();
            Assert.IsFalse(etor.MoveNext());
            etor.Dispose();
            etor.Reset();
        }
#endif

        [Test]
        public void ZeroForeach()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithZeroItems();
            int i = 0;
            foreach (ServiceAttribute attr in record) {
                ++i;
            }
            Assert.AreEqual(0, i, "Count!=loops");
        }

        //-----------------------------
        [Test]
        public void OneCurrent()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithOneItems();
            IEnumerator_ServiceAttribute etor = record.GetEnumerator();
            try {
                ServiceAttribute obj = (ServiceAttribute)etor.Current;
                Assert.Fail("should have thrown!");
            } catch (InvalidOperationException) { }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OneMoveNextMoveNextFalse()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithOneItems();
            IEnumerator_ServiceAttribute etor = record.GetEnumerator();
            Assert.IsTrue(etor.MoveNext());
            Assert.IsFalse(etor.MoveNext());
            ServiceAttribute attr = (ServiceAttribute)etor.Current;
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OneMoveNextMoveNextFalseAndGetCurrent()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithOneItems();
            IEnumerator_ServiceAttribute etor = record.GetEnumerator();
            Assert.IsTrue(etor.MoveNext());
            Assert.IsFalse(etor.MoveNext());
            ServiceAttribute attr = (ServiceAttribute)etor.Current;
        }

        [Test]
        public void OneMoveNextCurrentMoveNextFalseRepeatAfterReset()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithOneItems();
            IEnumerator_ServiceAttribute etor = record.GetEnumerator();
            Assert.IsTrue(etor.MoveNext());
            RecordAccess_Data.RecordWithOneItem_AssertIsAttributeAt0((ServiceAttribute)etor.Current);
            Assert.IsFalse(etor.MoveNext());
            //----
            Assert.IsFalse(etor.MoveNext());
            etor.Reset();
            Assert.IsTrue(etor.MoveNext());
            RecordAccess_Data.RecordWithOneItem_AssertIsAttributeAt0((ServiceAttribute)etor.Current);
            Assert.IsFalse(etor.MoveNext());
        }

        //-----------------------------
        [Test]
        public void MultipleCurrent()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithMultipleItems();
            IEnumerator_ServiceAttribute etor = record.GetEnumerator();
            try {
                ServiceAttribute obj = (ServiceAttribute)etor.Current;
                Assert.Fail("should have thrown!");
            } catch (InvalidOperationException) { }
        }

        [Test]
        public void MultipleMoveNextTimes3MoveNextFalse()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithMultipleItems();
            IEnumerator_ServiceAttribute etor = record.GetEnumerator();
            Assert.IsTrue(etor.MoveNext());
            Assert.IsTrue(etor.MoveNext());
            Assert.IsTrue(etor.MoveNext());
            Assert.IsFalse(etor.MoveNext());
        }

        [Test]
        public void MultipleMoveNextCurrentMoveNextFalseRepeatAfterReset()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithMultipleItems();
            IEnumerator_ServiceAttribute etor = record.GetEnumerator();
            Assert.IsTrue(etor.MoveNext());
            RecordAccess_Data.RecordWithMultipleItems_AssertIsAttributeAt0((ServiceAttribute)etor.Current);
            Assert.IsTrue(etor.MoveNext());
            RecordAccess_Data.RecordWithMultipleItems_AssertIsAttributeAt1((ServiceAttribute)etor.Current);
            Assert.IsTrue(etor.MoveNext());
            RecordAccess_Data.RecordWithMultipleItems_AssertIsAttributeAt2((ServiceAttribute)etor.Current);
            Assert.IsFalse(etor.MoveNext());
            //----
            Assert.IsFalse(etor.MoveNext());
            etor.Reset();
            Assert.IsTrue(etor.MoveNext());
            RecordAccess_Data.RecordWithMultipleItems_AssertIsAttributeAt0((ServiceAttribute)etor.Current);
            Assert.IsTrue(etor.MoveNext());
            Assert.IsTrue(etor.MoveNext());
            Assert.IsFalse(etor.MoveNext());
        }

        [Test]
        public void MultipleMoveNextCurrentMoveNextFalseRepeatAfterReset_NonGeneric()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithMultipleItems();
            System.Collections.IEnumerator etor = ((System.Collections.IEnumerable)record).GetEnumerator();
            Assert.IsTrue(etor.MoveNext());
            RecordAccess_Data.RecordWithMultipleItems_AssertIsAttributeAt0((ServiceAttribute)etor.Current);
            Assert.IsTrue(etor.MoveNext());
            RecordAccess_Data.RecordWithMultipleItems_AssertIsAttributeAt1((ServiceAttribute)etor.Current);
            Assert.IsTrue(etor.MoveNext());
            RecordAccess_Data.RecordWithMultipleItems_AssertIsAttributeAt2((ServiceAttribute)etor.Current);
            Assert.IsFalse(etor.MoveNext());
            //----
            Assert.IsFalse(etor.MoveNext());
            etor.Reset();
            Assert.IsTrue(etor.MoveNext());
            RecordAccess_Data.RecordWithMultipleItems_AssertIsAttributeAt0((ServiceAttribute)etor.Current);
            Assert.IsTrue(etor.MoveNext());
            Assert.IsTrue(etor.MoveNext());
            Assert.IsFalse(etor.MoveNext());
        }

#if ! FX1_1
        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void MultipleAndDisposedMoveNext()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithMultipleItems();
            IEnumerator_ServiceAttribute etor = record.GetEnumerator();
            Assert.IsTrue(etor.MoveNext());
            RecordAccess_Data.RecordWithMultipleItems_AssertIsAttributeAt0(etor.Current);
            etor.Dispose();
            etor.MoveNext();
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void MultipleAndDisposedCurrent()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithMultipleItems();
            IEnumerator_ServiceAttribute etor = record.GetEnumerator();
            Assert.IsTrue(etor.MoveNext());
            RecordAccess_Data.RecordWithMultipleItems_AssertIsAttributeAt0(etor.Current);
            etor.Dispose();
            ServiceAttribute attr = etor.Current;
        }
#endif

        [Test]
        public void MultipleForeachJustCount()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithMultipleItems();
            int i = 0;
            foreach (ServiceAttribute attr in record) {
                ++i;
            }
            Assert.AreEqual(RecordAccess_Data.MultipleItemsCount, i, "Count!=loops");
        }

        [Test]
        public void MultipleForeach()
        {
            ServiceRecord record = RecordAccess_Data.CreateRecordWithMultipleItems();
            int i = 0;
            foreach (ServiceAttribute attr in record) {
                if (i == 0) {
                    RecordAccess_Data.RecordWithMultipleItems_AssertIsAttributeAt0(attr);
                } else if (i == 1) {
                    RecordAccess_Data.RecordWithMultipleItems_AssertIsAttributeAt1(attr);
                } else if (i == 2) {
                    RecordAccess_Data.RecordWithMultipleItems_AssertIsAttributeAt2(attr);
                }
                ++i;
            }
            Assert.AreEqual(RecordAccess_Data.MultipleItemsCount, i, "Count!=loops");
        }

    }//class

}
