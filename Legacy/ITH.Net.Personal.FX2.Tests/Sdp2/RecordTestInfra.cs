using System;
using NUnit.Framework;
using InTheHand.Net.Bluetooth;

namespace InTheHand.Net.Tests.Sdp2
{

    class ExpectedServiceElement
    {
        public ExpectedServiceElement(ElementType elementType, ElementTypeDescriptor etd, Object value)
        {
            if (!ServiceRecordParser.TypeMatchesEtd(etd, elementType)) {
                throw new ArgumentException(String.Format(
                    "Test setup; ElementType does not match TypeDescriptor ({0}/{1}).",
                    elementType, etd));
            }
            this.ElementType = elementType;
            this.Etd = etd;
            //
            if (value != null) { // Can't check types when the value is null.
                if (etd == ElementTypeDescriptor.ElementSequence || etd == ElementTypeDescriptor.ElementAlternative) {
                    if (typeof(ExpectedServiceElement[]) != value.GetType()) {
                        throw new ArgumentException("DataElementSequence and DataElementAlternative need an array of ExpectedAttributeValue.");
                    }
                    this.Children = (ExpectedServiceElement[])value;
                } else {
                    if (typeof(ExpectedServiceElement[]) == value.GetType()) {
                        throw new ArgumentException("DataElementSequence and DataElementAlternative must be used for an array of ExpectedAttributeValue.");
                    }
                    this.Value = value;
                }
            }
        }

        public readonly ElementType ElementType;
        public readonly ElementTypeDescriptor Etd;
        public readonly Object Value;
        public readonly ExpectedServiceElement[] Children;
    }//class


    class ExpectedServiceAttribute : ExpectedServiceElement
    {

        public ExpectedServiceAttribute(UInt16 id, ElementType elementType, ElementTypeDescriptor etd, Object value)
            : base(elementType, etd, value)
        {
            this.Id = id;
        }

        public readonly UInt16 Id;
    }//class


#if ! FX1_1
    static 
#endif
    class TestRecordParsing
    {
        //--------------------------------------------------------------
        public static ServiceRecord DoTest(ExpectedServiceAttribute[] expected, byte[] buffer)
        {
            ServiceRecord result = new ServiceRecordParser().Parse(buffer);
            //
            DoAreEqual(expected, result, 0);
            return result;
        }

        public static ServiceRecord DoTestSkippingUnhandledTypes(ExpectedServiceAttribute[] expected, byte[] buffer)
        {
            ServiceRecordParser parser = new ServiceRecordParser();
            parser.SkipUnhandledElementTypes = true;
            ServiceRecord result = parser.Parse(buffer);
            //
            DoAreEqual(expected, result, 0);
            return result;
        }

        public static ServiceRecord DoTestLazyUrlCreation(ExpectedServiceAttribute[] expected, byte[] buffer)
        {
            ServiceRecordParser parser = new ServiceRecordParser();
            parser.LazyUrlCreation = true;
            ServiceRecord result = parser.Parse(buffer);
            //
            DoAreEqual(expected, result, 0);
            return result;
        }

        private static void DoAreEqual(ExpectedServiceAttribute[] expectedAttributes, ServiceRecord record, int depth)
        {
            Assert.AreEqual(expectedAttributes.Length, record.Count, "Number of attributes.");
            for (int i = 0; i < expectedAttributes.Length; ++i) {
                ExpectedServiceAttribute expected = expectedAttributes[i];
                ServiceAttribute row = record[i];
                ServiceAttributeId expectedId = (ServiceAttributeId)expected.Id;
                Assert.AreEqual(expectedId, row.Id, "Attr Id.");
                DoAreEqual(expected, row.Value, depth + 1);
            }//for
        }

        private static void DoAreEqual(ExpectedServiceElement expected, ServiceElement value, int depth)
        {
            if (expected.Etd != value.ElementTypeDescriptor) { }
            Assert.AreEqual(expected.Etd, value.ElementTypeDescriptor, "ElementTypeDescriptor.");
            if (expected.ElementType != value.ElementType) { }
            Assert.AreEqual(expected.ElementType, value.ElementType, "ElementType.");
            if (expected.Etd == ElementTypeDescriptor.ElementSequence
                || expected.Etd == ElementTypeDescriptor.ElementAlternative) {
                ExpectedServiceElement[] expectedChildAttributes = expected.Children;
                ServiceElement[] children = value.GetValueAsElementArray();
                DoAreEqual(expectedChildAttributes, children, depth + 1);
            } else {
                Assert.AreEqual(expected.Value, value.Value, "Element Value");
            }
        }

        private static void DoAreEqual(ExpectedServiceElement[] expectedAttributes, ServiceElement[] values, int depth)
        {
            Assert.AreEqual(expectedAttributes.Length, values.Length, "Number of elements.");
            for (int i = 0; i < expectedAttributes.Length; ++i) {
                ExpectedServiceElement expected = expectedAttributes[i];
                ServiceElement row = values[i];
                DoAreEqual(expected, row, depth + 1);
            }//for
        }

    }//class

}