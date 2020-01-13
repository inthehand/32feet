using System;
using NUnit.Framework;
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
using System.Net;
using InTheHand.Net.Bluetooth;

namespace InTheHand.Net.Tests.Sdp2
{

    [TestFixture]
    public class ElementTests
    {

        //------------------------------
        [Test]
        public void Nil() { new ServiceElement(ElementType.Nil, null); }
        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "CLR type 'Int32' not valid type for element type 'Nil'.")]
        public void NilNonNullVal() { new ServiceElement(ElementType.Nil, 5); }
        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "CLR type 'String' not valid type for element type 'Nil'.")]
        public void NilNonNullObj() { new ServiceElement(ElementType.Nil, "fooo"); }

        //------------------------------
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void List_NullLiteral()
        {
            new ServiceElement(ElementType.ElementSequence, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void List_Null()
        {
#if FX1_1
            System.Collections.IList list = null;
#else
            System.Collections.Generic.IList<ServiceElement> list = null;
#endif
            new ServiceElement(ElementType.ElementSequence, list);
        }


        [Test]
        public void List_EmptyArray()
        {
            ServiceElement[] array = new ServiceElement[0];
            ServiceElement element = new ServiceElement(ElementType.ElementSequence, array);
            IList_ServiceElement list = element.GetValueAsElementList();
            Assert.AreEqual(0, list.Count);
        }

        //------------------------------

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ParamsArray_ExplicitNull()
        {
            ServiceElement[] array = null;
            ServiceElement element = new ServiceElement(ElementType.ElementSequence, array);
        }

        [Test]
        public void ParamsArray_None()
        {
            ServiceElement element = new ServiceElement(ElementType.ElementSequence);
            IList_ServiceElement list = element.GetValueAsElementList();
            Assert.AreEqual(0, list.Count);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = ServiceElement.ErrorMsgSeqAltTypeNeedElementArray)]
        public void ParamsArray_None_BadNoSeqOrAlt()
        {
            ServiceElement element = new ServiceElement(ElementType.Uuid16);
        }

        [Test]
        public void ParamsArray_ExplicitNone()
        {
            ServiceElement[] array = new ServiceElement[0];
            ServiceElement element = new ServiceElement(ElementType.ElementSequence, array);
            IList_ServiceElement list = element.GetValueAsElementList();
            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void ParamsArray_Some()
        {
            ServiceElement e0 = new ServiceElement(ElementType.UInt8, (byte)5);
            ServiceElement e1 = new ServiceElement(ElementType.Uuid16, (UInt16)0x1105);
            ServiceElement e2 = new ServiceElement(ElementType.UInt16, (UInt16)0x5555);
            ServiceElement element = new ServiceElement(ElementType.ElementSequence,
                e0, e1, e2);
            IList_ServiceElement list = element.GetValueAsElementList();
            Assert.AreEqual(3, list.Count);
            Assert.AreSame(e0, list[0]);
            Assert.AreSame(e1, list[1]);
            Assert.AreSame(e2, list[2]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = ServiceElement.ErrorMsgSeqAltTypeNeedElementArray)]
        public void ParamsArray_Some_BadNoSeqOrAlt()
        {
            ServiceElement e0 = new ServiceElement(ElementType.UInt8, (byte)5);
            ServiceElement e1 = new ServiceElement(ElementType.Uuid16, (UInt16)0x1105);
            ServiceElement e2 = new ServiceElement(ElementType.UInt16, (UInt16)0x5555);
            ServiceElement element = new ServiceElement(ElementType.Uuid16,
                e0, e1, e2);
        }

        //------------------------------

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Url_NullLiteral()
        {
            new ServiceElement(ElementType.Url, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void String_NullLiteral()
        {
            new ServiceElement(ElementType.TextString, null);
        }

        //--------------------------------------------------------------
//        [Test]
//        [ExpectedException(typeof(ProtocolViolationException),
//            "ElementType 'Boolean' is not of given TypeDescriptor 'ElementAlternative'.")]
//        public void BadTypeTypePair()
//        {
//#pragma warning disable 618
//            new ServiceElement(ElementTypeDescriptor.ElementAlternative, ElementType.Boolean, 0);
//#pragma warning restore 618
//        }

        //--------------------------------------------------------------
        [Test]
        public void TestingPropertiesWithTypeBoolean()
        {
            ServiceElement element = new ServiceElement(ElementType.Boolean, true);
            Assert.AreEqual(ElementTypeDescriptor.Boolean, element.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.Boolean, element.ElementType);
            Assert.AreEqual(true, element.Value);
            element = new ServiceElement(ElementType.Boolean, false);
            Assert.AreEqual(ElementTypeDescriptor.Boolean, element.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.Boolean, element.ElementType);
            Assert.AreEqual(false, element.Value);
        }

        [Test]
        public void TestingPropertiesWithTypeInt32()
        {
            ServiceElement element = new ServiceElement(ElementType.Int32, 5);
            Assert.AreEqual(ElementTypeDescriptor.TwosComplementInteger, element.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.Int32, element.ElementType);
            Assert.AreEqual(5, element.Value);
            element = new ServiceElement(ElementType.Int32, 0x01000);
            Assert.AreEqual(ElementTypeDescriptor.TwosComplementInteger, element.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.Int32, element.ElementType);
            Assert.AreEqual(0x01000, element.Value);
        }

        [Test]
        public void TestingPropertiesWithTypeInt64()
        {
            ServiceElement element = new ServiceElement(ElementType.Int64, (Int64)5);
            Assert.AreEqual(ElementTypeDescriptor.TwosComplementInteger, element.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.Int64, element.ElementType);
            Assert.AreEqual(5, element.Value);
            element = new ServiceElement(ElementType.Int64, (Int64)0x01000);
            Assert.AreEqual(ElementTypeDescriptor.TwosComplementInteger, element.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.Int64, element.ElementType);
            Assert.AreEqual(0x01000, element.Value);
        }

        [Test]
        public void TestingPropertiesWithTypeInt128()
        {
            byte[] input1 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            byte[] inputOneShort = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, /*16*/ };
            //
            try {
                ServiceElement element = new ServiceElement(ElementType.Int128, input1);
                Assert.Fail("should have thrown!");
            } catch (ArgumentOutOfRangeException ex) {
                Assert.AreEqual("Unknown ElementType 'Int128'." + "\r\nParameter name: type",
                    ex.Message, "ex.Message");
            }
            //TO-DOAssert.AreEqual(ElementTypeDescriptor.TwosComplementInteger, element.ElementTypeDescriptor);
            //Assert.AreEqual(ElementType.Int128, element.ElementType);
            //Assert.AreEqual(input1, element.Value);
            //
            //element = new ServiceElement(ElementType.Int128, inputOneShort);
            //Assert.AreEqual(ElementTypeDescriptor.TwosComplementInteger, element.ElementTypeDescriptor);
            //Assert.AreEqual(ElementType.Int128, element.ElementType);
            //Assert.AreEqual(0x01000, element.Value);
        }

        [Test]
        public void TestingPropertiesWithTypeUri()
        {
            String str = "http://example.com/foo.html";
            Uri value = new Uri(str);
            ServiceElement element = new ServiceElement(ElementType.Url, value);
            Assert.AreEqual(ElementTypeDescriptor.Url, element.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.Url, element.ElementType);
            Assert.AreEqual(new Uri(str), element.Value);
            Assert.AreEqual(new Uri(str), element.GetValueAsUri());
        }

        [Test]
        public void TestingPropertiesWithTypeUriLazyCreation()
        {
            String str = "ftp://ftp.example.com/foo/bar.txt";
            byte[] valueBytes = Encoding.ASCII.GetBytes(str);
            ServiceElement element = new ServiceElement(ElementType.Url, valueBytes);
            Assert.AreEqual(ElementTypeDescriptor.Url, element.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.Url, element.ElementType);
            Assert.IsInstanceOfType(typeof(byte[]), element.Value);
            Assert.AreEqual(new Uri(str), element.GetValueAsUri());
        }

        //--------------------------------------------------------------
        [Test]
        public void StringUtf8Empty()
        {
            byte[] bytes = new byte[0];
            ServiceElement element = new ServiceElement(ElementType.TextString, bytes);
            Assert.AreEqual(ElementTypeDescriptor.TextString, element.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.TextString, element.ElementType);
            Assert.IsInstanceOfType(typeof(byte[]), element.Value);
            Assert.AreEqual(String.Empty, element.GetValueAsStringUtf8());
            Assert.AreEqual(0, element.GetValueAsStringUtf8().Length);
        }

        [Test]
        public void StringUtf8AllAscii()
        {
            String str = "ambdsanbdasdlkaslkda";
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            ServiceElement element = new ServiceElement(ElementType.TextString, bytes);
            Assert.AreEqual(ElementTypeDescriptor.TextString, element.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.TextString, element.ElementType);
            Assert.IsInstanceOfType(typeof(byte[]), element.Value);
            Assert.AreEqual(str, element.GetValueAsStringUtf8());
        }

        [Test]
        public void StringUtf8AllAsciiWithANullTerminationByte()
        {
            String str = "ambdsanbdasdlkaslkda";
            byte[] bytes_ = Encoding.UTF8.GetBytes(str);
            // Add a null-termination byte
            byte[] bytes = new byte[bytes_.Length + 1];
            bytes_.CopyTo(bytes, 0);
            ServiceElement element = new ServiceElement(ElementType.TextString, bytes);
            Assert.AreEqual(ElementTypeDescriptor.TextString, element.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.TextString, element.ElementType);
            Assert.IsInstanceOfType(typeof(byte[]), element.Value);
            Assert.AreEqual(str, element.GetValueAsStringUtf8());
        }

        [Test]
        public void StringUtf8WithVariousHighChars()
        {
            String str = "ambds\u2022nbdas\u00FEdlka\U00012004slkda";
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            ServiceElement element = new ServiceElement(ElementType.TextString, bytes);
            Assert.AreEqual(ElementTypeDescriptor.TextString, element.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.TextString, element.ElementType);
            Assert.IsInstanceOfType(typeof(byte[]), element.Value);
            Assert.AreEqual(str, element.GetValueAsStringUtf8());
        }

        //--------------------------------------------------------------

        [Test]
        public void StringGivenString()
        {
            String str = "ambds\u2022nbdas\u00FEdlka\U00012004slkda";
            ServiceElement element = new ServiceElement(ElementType.TextString, str);
            //
            Assert.AreEqual(ElementTypeDescriptor.TextString, element.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.TextString, element.ElementType);
            Assert.IsInstanceOfType(typeof(String), element.Value);
            Assert.AreEqual(str, element.GetValueAsStringUtf8());
            Assert.AreEqual(str, element.GetValueAsString(Encoding.ASCII));
            Assert.AreEqual(str, element.GetValueAsString(Encoding.Unicode));
        }

        //--------------------------------------------------------------

        //[Test]
        //public void SequenceZeroItems()
        //{
        //    ServiceElement element = new 
        //}

        //--------------------------------------------------------------

        Guid CreateBluetoothBasedUuid(UInt32 shortForm)
        {
            return new Guid(
#if NETCF
                unchecked((int)shortForm)
#else
                shortForm
#endif
                
                , 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "CLR type 'Guid' not valid type for element type 'Uuid16'.")]
        public void BadUuid16ButUuid128()
        {
            new ServiceElement(ElementType.Uuid16, CreateBluetoothBasedUuid(0x1105));
        }

        [Test]
        public void Uuid16As128()
        {
            ServiceElement element = new ServiceElement(ElementType.Uuid16, (UInt16)0x1105);
            Assert.AreEqual(ElementTypeDescriptor.Uuid, element.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.Uuid16, element.ElementType);
            //Assert.IsInstanceOfType(typeof(UInt16), element.Value);
            Guid expected = CreateBluetoothBasedUuid(0x1105);
            Assert.AreEqual(expected, element.GetValueAsUuid());
        }

        [Test]
        public void Uuid32As128()
        {
            ServiceElement element = new ServiceElement(ElementType.Uuid32, (UInt32)0x1105);
            Assert.AreEqual(ElementTypeDescriptor.Uuid, element.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.Uuid32, element.ElementType);
            //Assert.IsInstanceOfType(typeof(UInt32), element.Value);
            Guid expected = CreateBluetoothBasedUuid(0x1105);
            Assert.AreEqual(expected, element.GetValueAsUuid());
        }

        [Test]
        public void Uuid128As128()
        {
            ServiceElement element = new ServiceElement(ElementType.Uuid128, CreateBluetoothBasedUuid(0x1105));
            Assert.AreEqual(ElementTypeDescriptor.Uuid, element.ElementTypeDescriptor);
            Assert.AreEqual(ElementType.Uuid128, element.ElementType);
            //Assert.IsInstanceOfType(typeof(Guid), element.Value);
            Guid expected = CreateBluetoothBasedUuid(0x1105);
            Assert.AreEqual(expected, element.GetValueAsUuid());
        }


    }//class

}
