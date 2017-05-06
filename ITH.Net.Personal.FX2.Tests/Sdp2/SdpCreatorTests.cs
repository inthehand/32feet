// 32feet.NET - Personal Area Networking for .NET
//
// ServiceCreatorTests.cs
// 
// Copyright (c) 2007 Andy Hume
// Copyright (c) 2007 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
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
using NUnit.Framework;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.AttributeIds;

namespace InTheHand.Net.Tests.Sdp2
{

    public 
#if ! FX1_1
        static 
#endif
        class Assert2
    {
        //--------------------------------------------------------------
        public static void AreEqualBuffers(byte[] expectedBytes, byte[] buf, int count)
        {
            if (count < buf.Length) {
                byte[] buf2 = new byte[count];
                Array.Copy(buf, 0, buf2, 0, count);
                buf = buf2;
            }
            Assert.AreEqual(expectedBytes, buf);
        }
    }//class


    public 
#if ! FX1_1
        static 
#endif
        class Data_SdpCreator_SingleElementTests
    {
        public static readonly byte[] RecordBytes_Empty = { 0x35, 0, };

        public static readonly byte[] RecordBytes_OneUInt16 = { 
            0x35, 6,
                0x09, 0x04, 0x01,
                0x09, 0xfe, 0x012,
        };
        public static readonly byte[] RecordBytes_OneUInt16_HighAttrId = { 
            0x35, 6,
                0x09, 0xF4, 0x01,
                0x09, 0xfe, 0x012,
        };
        public static readonly byte[] RecordBytes_OneUInt32 = { 
            0x35, 8,
                0x09, 0x04, 0x01,
                0x0a, 0xfe, 0x12, 0x56, 0x01,
        };
        public static readonly byte[] RecordBytes_OneUInt8 = { 
            0x35, 5,
                0x09, 0x04, 0x01,
                0x08, 0xfe,
        };
        public static readonly byte[] RecordBytes_OneUInt64 = { 
            0x35, 12,
                0x09, 0x04, 0x01,
                0x0b, 0xfe, 0x12, 0x56, 0x01, 0x23, 0x45, 0x67, 0x89
        };

        public static readonly byte[] RecordBytes_OneInt16 = { 
            0x35, 6,
                0x09, 0x04, 0x01,
                0x11, 0xfe, 0x012,
        };
        public static readonly byte[] RecordBytes_OneInt32 = { 
            0x35, 8,
                0x09, 0x04, 0x01,
                0x12, 0xfe, 0x12, 0x56, 0x01,
        };
        public static readonly byte[] RecordBytes_OneInt8 = { 
            0x35, 5,
                0x09, 0x04, 0x01,
                0x10, 0xfe,
        };
        public static readonly byte[] RecordBytes_OneInt64 = { 
            0x35, 12,
                0x09, 0x04, 0x01,
                0x13, 0xfe, 0x12, 0x56, 0x01, 0x23, 0x45, 0x67, 0x89
        };

        public static readonly byte[] RecordBytes_ChildElementWithOneUInt16 = { 
            0x35, 8,
                0x09, 0x04, 0x01,
                0x35, 3,
                    0x09, 0xfe, 0x12,
        };
        public static readonly byte[] RecordBytes_ChildElementAlternativeWithTwoUInt16 = { 
            0x35, 11,
                0x09, 0x04, 0x01,
                0x3d, 6,
                    0x09, 0xfe, 0x12,
                    0x09, 0x12, 0x34
        };
        public static readonly byte[] RecordBytes_ElementsAndVariableAndFixedInDeepTree1 = {
            0x35, 0x47, 0x09, 0x04, 0x01, 0x3D, 0x42, 0x35, 
            0x2F, 0x35, 0x2D, 0x25, 0x0C, 0x61, 0x62, 0x63, 
            0x64, 0xC3, 0xA9, 0x66, 0x67, 0x68, 0xC4, 0xAD, 
            0x6A, 0x45, 0x1A, 0x68, 0x74, 0x74, 0x70, 0x3A, 
            0x2F, 0x2F, 0x65, 0x78, 0x61, 0x6D, 0x70, 0x6C, 
            0x65, 0x2E, 0x63, 0x6F, 0x6D, 0x2F, 0x66, 0x6F, 
            0x6F, 0x2E, 0x74, 0x78, 0x74, 0x09, 0xFE, 0x12, 
            0x25, 0x0C, 0x61, 0x62, 0x63, 0x64, 0xC3, 0xA9, 
            0x66, 0x67, 0x68, 0xC4, 0xAD, 0x6A, 0x09, 0x12, 
            0x34, 
        };
        public static readonly byte[] RecordBytes_OneUuid16 = { 
            0x35, 6,
                0x09, 0x04, 0x01,
                0x19, 0xfe, 0x012,
        };
        public static readonly byte[] RecordBytes_OneUuid32 = { 
            0x35, 8,
                0x09, 0x04, 0x01,
                0x1a, 0xfe, 0x012, 0x56, 0x01,
        };
        public static readonly byte[] RecordBytes_OneUuid128 = { 
            0x35, 20,
                0x09, 0x04, 0x01,
                0x1c, 
                    0x12, 0x34, 0x56, 0x78, 
                    /*-*/0x23, 0x45, 
                    /*-*/0x34, 0x56,
                    /*-*/0x45, 0x67, 
                    /*-*/0x10, 0x20, 0x30, 0x40, 0x50, 0x60
        };
        public static readonly byte[] RecordBytes_OneUrl = { 
            0x35, 31,
                0x09, 0x04, 0x01,
                0x45, 26,
                    (byte)'h', (byte)'t', (byte)'t', (byte)'p', (byte)':', (byte)'/', (byte)'/', (byte)'e',
                    (byte)'x', (byte)'a', (byte)'m', (byte)'p', (byte)'l', (byte)'e', (byte)'.', (byte)'c',
                    (byte)'o', (byte)'m', (byte)'/', (byte)'f', (byte)'o', (byte)'o', (byte)'.', (byte)'t',
                    (byte)'x', (byte)'t'
        };
        public static readonly byte[] RecordBytes_OneUrl_Data = { 
                    (byte)'h', (byte)'t', (byte)'t', (byte)'p', (byte)':', (byte)'/', (byte)'/', (byte)'e',
                    (byte)'x', (byte)'a', (byte)'m', (byte)'p', (byte)'l', (byte)'e', (byte)'.', (byte)'c',
                    (byte)'o', (byte)'m', (byte)'/', (byte)'f', (byte)'o', (byte)'o', (byte)'.', (byte)'t',
                    (byte)'x', (byte)'t'
        };

        public const String RecordBytes_OneString_StringValue = "abcd\u00e9fgh\u012dj";
        public static readonly byte[] RecordBytes_OneString_StringValueAsBytes = {
            (byte)'a', (byte)'b', (byte)'c', (byte)'d', 0xC3, 0xA9, (byte)'f', (byte)'g', 
            (byte)'h', 0xC4, 0xAD, (byte)'j',
        };
        public static readonly byte[] RecordBytes_OneString_Utf8 = { 
            0x35, 17,
                0x09, 0x04, 0x01,
                0x25, 12,
                    (byte)'a', (byte)'b', (byte)'c', (byte)'d', 0xC3, 0xA9, (byte)'f', (byte)'g', 
                    (byte)'h', 0xC4, 0xAD, (byte)'j',
        };
        public static readonly byte[] RecordBytes_OneString_Utf8_Data
            = RecordBytes_OneString_StringValueAsBytes;
        public static readonly byte[] RecordBytes_OneString_Utf16le = { 
            0x35, 25,
                0x09, 0x04, 0x01,
                0x25, 20,
                    (byte)'a', 0, (byte)'b', 0, (byte)'c', 0, (byte)'d', 0, 
                    0xE9, 0, (byte)'f', 0, (byte)'g', 0, (byte)'h', 0, 
                    0x2D, 0x01, (byte)'j', 0,
        };
        public static readonly byte[] RecordBytes_OneString_Utf16le_Data = { 
                    (byte)'a', 0, (byte)'b', 0, (byte)'c', 0, (byte)'d', 0, 
                    0xE9, 0, (byte)'f', 0, (byte)'g', 0, (byte)'h', 0, 
                    0x2D, 0x01, (byte)'j', 0,
        };

        public static readonly byte[] RecordBytes_OneNil = { 
            0x35, 4,
                0x09, 0x04, 0x01,
                0x00, 
        };

        public static readonly byte[] RecordBytes_OneBooleanTrue = { 
            0x35, 5,
                0x09, 0x04, 0x01,
                0x28, 1
        };
        public static readonly byte[] RecordBytes_OneBooleanFalse = { 
            0x35, 5,
                0x09, 0x04, 0x01,
                0x28, 0
        };
    }//class



    [TestFixture]
    public class SdpCreatorTests : BaseSdpCreatorTests
    { }

    [TestFixture]
    public class SdpCreatorTests_AutomaticBuffer : BaseSdpCreatorTests
    {
        protected override void DoTest(byte[] expectedRecordBytes, ServiceRecord record)
        {
            byte[] buf = new ServiceRecordCreator().CreateServiceRecord(record);
            Assert2.AreEqualBuffers(expectedRecordBytes, buf, buf.Length);
        }
    }

    [TestFixture]
    public class SdpCreatorTests_ServiceRecordInstanceMethod : BaseSdpCreatorTests
    {
        protected override void DoTest(byte[] expectedRecordBytes, ServiceRecord record)
        {
            byte[] buf = record.ToByteArray();
            Assert2.AreEqualBuffers(expectedRecordBytes, buf, buf.Length);
        }
    }

    [TestFixture]
    public class SdpCreatorTests_ShortBuffer_AllLengths : BaseSdpCreatorTests
    {
        protected override void DoTest(byte[] expectedRecordBytes, ServiceRecord record)
        {
            for (int i = 9; i >= 0; --i) {
                byte[] buf = new byte[i];
                try {
                    int count = new ServiceRecordCreator().CreateServiceRecord(record, buf);
                    Assert2.AreEqualBuffers(expectedRecordBytes, buf, count);
                } catch (ArgumentOutOfRangeException) { }
            }//for
        }
    }//class

    [TestFixture]
    public class SdpCreatorTests_ShortBuffer10 : BaseSdpCreatorTests
    {
        protected override void DoTest(byte[] expectedRecordBytes, ServiceRecord record)
        {
            int i = 10;
            byte[] buf = new byte[i];
            try {
                int count = new ServiceRecordCreator().CreateServiceRecord(record, buf);
                Assert2.AreEqualBuffers(expectedRecordBytes, buf, count);
            } catch (ArgumentOutOfRangeException) { }
        }
    }//class

    public abstract class BaseSdpCreatorTests
    {

        //--------------------------------------------------------------
        protected virtual void DoTest(byte[] expectedRecordBytes, ServiceRecord record)
        {
            byte[] buf = new byte[1024];
            int count = new ServiceRecordCreator().CreateServiceRecord(record, buf);
            //DumpAsCSharpArray(buf, count);
            Assert2.AreEqualBuffers(expectedRecordBytes, buf, count);
        }

#if !NETCF
        private void DumpAsCSharpArray(byte[] buf, int count)
        {
            const int BytesPerLine = 8;
            StringBuilder bldr = new StringBuilder();
            bldr.AppendLine("= {");
            for (int i = 0; i < count; ) {
                bldr.Append("    ");
                for (int j = 0; j < BytesPerLine && i < count; ++j) {
                    bldr.AppendFormat("0x{0:X2}, ", buf[i]);
                    ++i;
                }
                bldr.AppendLine();
            }
            bldr.AppendLine();
            bldr.AppendLine("};");
            System.Windows.Forms.MessageBox.Show(bldr.ToString());
        }
#endif

        private void DoTest(byte[] expectedRecordBytes, IList_ServiceAttribute attrs)
        {
            DoTest(expectedRecordBytes, new ServiceRecord(attrs));
        }

        //--------------------------------------------------------------
        [Test]
        public void Empty()
        {
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_Empty, attrs);
        }

        [Test]
        public void OneUInt16()
        {
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.UInt16, (UInt16)0xfe12)));
            ServiceRecord record = new ServiceRecord(attrs);
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_OneUInt16, record);
        }
        [Test]
        public void OneUInt16_HighAttrId()
        {
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(
                new ServiceAttribute(0xF401, new ServiceElement(ElementType.UInt16, (UInt16)0xfe12)));
            ServiceRecord record = new ServiceRecord(attrs);
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_OneUInt16_HighAttrId, record);
        }
        [Test]
        public void OneUInt32()
        {
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.UInt32, unchecked((UInt32)0xfe125601))));
            ServiceRecord record = new ServiceRecord(attrs);
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_OneUInt32, record);
        }
        [Test]
        public void OneUInt8()
        {
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.UInt8, unchecked((Byte)0xfe))));
            ServiceRecord record = new ServiceRecord(attrs);
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_OneUInt8, record);
        }
        [Test]
        public void OneUInt64()
        {
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.UInt64, unchecked((UInt64)0xfe12560123456789))));
            ServiceRecord record = new ServiceRecord(attrs);
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_OneUInt64, record);
        }

        //--------
        [Test]
        public void OneInt16()
        {
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.Int16, unchecked((Int16)0xfe12))));
            ServiceRecord record = new ServiceRecord(attrs);
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_OneInt16, record);
        }
        [Test]
        public void OneInt32()
        {
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.Int32, unchecked((Int32)0xfe125601))));
            ServiceRecord record = new ServiceRecord(attrs);
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_OneInt32, record);
        }
        [Test]
        public void OneInt8()
        {
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.Int8, unchecked((SByte)0xfe))));
            ServiceRecord record = new ServiceRecord(attrs);
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_OneInt8, record);
        }
        [Test]
        public void OneInt64()
        {
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.Int64, unchecked((Int64)0xfe12560123456789))));
            ServiceRecord record = new ServiceRecord(attrs);
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_OneInt64, record);
        }

        //[Test]
        //TODO ((public void OneInt64()
        //{
        //    byte[] buf = new byte[1024];
        //    IList_ServiceAttribute attrs = new List_ServiceAttribute();
        //    attrs.Add(
        //        new ServiceAttribute(0x0401, new ServiceElement(
        //            ElementTypeDescriptor.TwosComplementInteger, ElementType.Int64, unchecked((Int64)0x1100000fe125601))));
        //    ServiceRecord record = new ServiceRecord(attrs);
        //    int count = new Xxxx().Yyyy(record, buf);
        //    //Assert_AreEqualBuffers(RecordBytes_OneInt32, buf, count);
        //}

        //--------
        [Test]
        public void ChildElementWithOneUInt16()
        {
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            IList_ServiceElement leaves = new List_ServiceElement();
            ServiceElement leaf = new ServiceElement(ElementType.UInt16, (UInt16)0xfe12);
            leaves.Add(leaf);
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.ElementSequence, leaves)));
            ServiceRecord record = new ServiceRecord(attrs);
            //
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_ChildElementWithOneUInt16, record);
        }

        [Test]
        public void ChildElementAlternativeWithTwoUInt16()
        {
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            IList_ServiceElement leaves = new List_ServiceElement();
            ServiceElement leaf;
            leaf = new ServiceElement(ElementType.UInt16, (UInt16)0xfe12);
            leaves.Add(leaf);
            leaf = new ServiceElement(ElementType.UInt16, (UInt16)0x1234);
            leaves.Add(leaf);
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.ElementAlternative,
                        leaves)));
            ServiceRecord record = new ServiceRecord(attrs);
            //
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_ChildElementAlternativeWithTwoUInt16, record);
        }

        [Test]
        public void ElementsAndVariableAndFixedInDeepTree1()
        {
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            //
            String str = Data_SdpCreator_SingleElementTests.RecordBytes_OneString_StringValue;
            ServiceElement itemStr1 = new ServiceElement(ElementType.TextString, str);
            ServiceElement itemStr2 = new ServiceElement(ElementType.TextString, str);
            //
            Uri uri = new Uri("http://example.com/foo.txt");
            ServiceElement itemUrl = new ServiceElement(ElementType.Url, uri);
            //
            ServiceElement itemF1 = new ServiceElement(ElementType.UInt16, (UInt16)0xfe12);
            ServiceElement itemF2 = new ServiceElement(ElementType.UInt16, (UInt16)0x1234);
            //
            IList_ServiceElement leaves2 = new List_ServiceElement();
            leaves2.Add(itemStr1);
            leaves2.Add(itemUrl);
            leaves2.Add(itemF1);
            ServiceElement e2 = new ServiceElement(ElementType.ElementSequence, leaves2);
            //
            ServiceElement e1 = new ServiceElement(ElementType.ElementSequence, e2);
            //
            IList_ServiceElement leaves0 = new List_ServiceElement();
            leaves0.Add(e1);
            leaves0.Add(itemStr2);
            leaves0.Add(itemF2);
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.ElementAlternative,
                        leaves0)));
            ServiceRecord record = new ServiceRecord(attrs);
            //
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_ElementsAndVariableAndFixedInDeepTree1, record);
        }

        //--------
        [Test]
        public void OneUuid16()
        {
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.Uuid16, (UInt16)0xfe12)));
            ServiceRecord record = new ServiceRecord(attrs);
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_OneUuid16, record);
        }
        [Test]
        public void OneUuid32()
        {
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.Uuid32, (UInt32)0xfe125601)));
            ServiceRecord record = new ServiceRecord(attrs);
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_OneUuid32, record);
        }
        [Test]
        public void OneUuid128()
        {
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.Uuid128,
                    new Guid("12345678-2345-3456-4567-102030405060"))));
            ServiceRecord record = new ServiceRecord(attrs);
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_OneUuid128, record);
        }

        [Test]
        public virtual void OneNil() // 'virtual' so it can be overridden.
        {
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.Nil, null)));
            ServiceRecord record = new ServiceRecord(attrs);
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_OneNil, record);
        }

        [Test]
        public void OneBooleanTrue()
        {
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.Boolean, true)));
            ServiceRecord record = new ServiceRecord(attrs);
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_OneBooleanTrue, record);
        }

        [Test]
        public void OneBooleanFalse()
        {
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.Boolean, false)));
            ServiceRecord record = new ServiceRecord(attrs);
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_OneBooleanFalse, record);
        }

        //--------------------------------------------------------------
        [Test]
        public void Url_AsUri()
        {
            Uri uri = new Uri("http://example.com/foo.txt");
            //
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.Url, uri)));
            ServiceRecord record = new ServiceRecord(attrs);
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_OneUrl, record);
        }

        [Test]
        public void Url_AsBytes()
        {
            byte[] uriBytes = { (byte)'h', (byte)'t', (byte)'t', (byte)'p', (byte)':', (byte)'/', (byte)'/', (byte)'e', (byte)'x', (byte)'a', (byte)'m', (byte)'p', (byte)'l', (byte)'e', (byte)'.', (byte)'c', (byte)'o', (byte)'m', (byte)'/', (byte)'f', (byte)'o', (byte)'o', (byte)'.', (byte)'t', (byte)'x', (byte)'t' };
            //
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.Url, uriBytes)));
            ServiceRecord record = new ServiceRecord(attrs);
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_OneUrl, record);
        }

        [Test]
        public void String_AsString()
        {
            String str = Data_SdpCreator_SingleElementTests.RecordBytes_OneString_StringValue;
            //
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            try {
                attrs.Add(
                    new ServiceAttribute(0x0401, new ServiceElement(ElementType.TextString, str)));
            } catch (ArgumentException ex) {
                Assert.AreEqual("CLR type 'String' not valid type for element type 'TextString'.", ex.Message);
                //
                Assert.Ignore("Not yet implemented support for new ServiceElement(TextString, <String>).");
            }
            ServiceRecord record = new ServiceRecord(attrs);
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_OneString_Utf8, record);
        }

        [Test]
        public void String_AsBytesUtf8()
        {
            byte[] str = Encoding.UTF8.GetBytes(Data_SdpCreator_SingleElementTests.RecordBytes_OneString_StringValue);
            //
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.TextString, str)));
            ServiceRecord record = new ServiceRecord(attrs);
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_OneString_Utf8, record);
        }

        [Test]
        public void String_AsBytesUtf16le()
        {
            byte[] str = Encoding.Unicode.GetBytes(Data_SdpCreator_SingleElementTests.RecordBytes_OneString_StringValue);
            //
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.TextString, str)));
            ServiceRecord record = new ServiceRecord(attrs);
            DoTest(Data_SdpCreator_SingleElementTests.RecordBytes_OneString_Utf16le, record);
        }

        //--------------------------------------------------------------
        //--------------------------------------------------------------
        [Test]
        [ExpectedException(typeof(NotSupportedException), ExpectedMessage = "Only ServiceRecords shorter that 256 bytes are supported currently.")]
        public void BadOverrunChildElementWithManyUuids()
        {
            byte[] buf = new byte[1024];
            IList_ServiceAttribute attrs = new List_ServiceAttribute();
            IList_ServiceElement leaves = new List_ServiceElement();
            Guid guid = new Guid("12345678-2345-3456-4567-102030405060");
            ServiceElement leaf = new ServiceElement(ElementType.Uuid128,
                    guid);
            for (int i = 0; i < 16; ++i) {
                leaves.Add(leaf);
            }
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.ElementSequence,
                        leaves)));
            ServiceRecord record = new ServiceRecord(attrs);
            //
            int count = new ServiceRecordCreator().CreateServiceRecord(record, buf);
            Assert.Fail("should have thrown!");
        }

    }//class


    [TestFixture]
    public class SdpCreatorOtherTests
    {
        private static int DoCreate(ServiceRecord record, byte[] buf)
        {
            int count = new ServiceRecordCreator().CreateServiceRecord(record, buf);
            return count;
        }

        //--------------------------------------------------------------
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullRecord()
        {
            DoCreate(null, new byte[10]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullBuffer()
        {
            DoCreate(new ServiceRecord(), null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ZeroLengthBuffer()
        {
            DoCreate(new ServiceRecord(), new byte[0]);
        }

    }//class


    //-[TestFixture]
    //[Explicit]
    public class AllTypes
    {
        [TestFixtureSetUp]
        public void Init()
        {
            Console.WriteLine("\nRunning SdpCreator AllTypes tests.");
        }


        const int BufSize = 1024;

        void DoTest(ServiceElement element)
        {
            ServiceAttribute attr = new ServiceAttribute((ServiceAttributeId)0x1234, element);
            List_ServiceAttribute items = new List_ServiceAttribute();
            items.Add(attr);
            ServiceRecord record = new ServiceRecord(items);
            byte[] buf = new byte[BufSize];
            new ServiceRecordCreator().CreateServiceRecord(record, buf);
        }

        //--------------------------------------------------------------

        [Test]
        public void Nil()
        {
            DoTest(new ServiceElement(ElementType.Nil, null));
        }

        //----------------
        [Test]
        public void UInt8()
        {
            DoTest(new ServiceElement(ElementType.UInt8, (Byte)69));
        }

        [Test]
        public void UInt16()
        {
            DoTest(new ServiceElement(ElementType.UInt16, (UInt16)69));
        }

        [Test]
        public void UInt32()
        {
            DoTest(new ServiceElement(ElementType.UInt32, (UInt32)69));
        }

        [Test]
        [Ignore("No element support for type")]
        public void UInt64()
        {
            DoTest(new ServiceElement(ElementType.UInt64, (UInt64)69));
        }

        [Test]
        [Ignore("No element support for type")]
        public void UInt128()
        {
            DoTest(new ServiceElement(ElementType.UInt128, 69));
        }

        //----------------
        [Test]
        public void Int8()
        {
            DoTest(new ServiceElement(ElementType.Int8, (SByte)69));
        }

        [Test]
        public void Int16()
        {
            DoTest(new ServiceElement(ElementType.Int16, (Int16)69));
        }

        [Test]
        public void Int32()
        {
            DoTest(new ServiceElement(ElementType.Int32, (Int32)69));
        }

        [Test]
        [Ignore("No element support for type")]
        public void Int64()
        {
            DoTest(new ServiceElement(ElementType.Int64, (Int64)69));
        }

        [Test]
        [Ignore("No element support for type")]
        public void Int128()
        {
            DoTest(new ServiceElement(ElementType.Int128, 69));
        }

        //----------------
        [Test]
        public void Uuid16()
        {
            DoTest(new ServiceElement(ElementType.Uuid16, (UInt16)69));
        }

        [Test]
        public void Uuid32()
        {
            DoTest(new ServiceElement(ElementType.Uuid32, (UInt32)69));
        }

        [Test]
        public void Uuid128()
        {
            DoTest(new ServiceElement(ElementType.Uuid128, 
                new Guid("00112233-4455-6677-8899-aabbccddeeff")
                //before NETCFv1 port, was: Guid.NewGuid()
                ));
        }

        //----------------
        [Test]
        public void TextString_String()
        {
            String str = "skdkaáfdfdë--aa";
            ServiceElement element;
            try {
                element = new ServiceElement(ElementType.TextString, str);
                DoTest(element);
            } catch (ArgumentException ex) {
                Assert.AreEqual("CLR type 'String' not valid type for element type 'TextString'.", ex.Message);
                //
                Assert.Ignore("Not yet implemented support for new ServiceElement(TextString, <String>).");
            }
        }

        [Test]
        public void TextString_StringBytes()
        {
            byte[] str = Encoding.UTF8.GetBytes("skdkaáfdfdë--aa");
            ServiceElement element;
            try {
                element = new ServiceElement(ElementType.TextString, str);
                DoTest(element);
            } catch (ArgumentException ex) {
                Assert.AreEqual("CLR type 'String' not valid type for element type 'TextString'.", ex.Message);
                //
                Assert.Ignore("Not yet implemented support for new ServiceElement(TextString, <String>).");
            }
        }

        //----------------
        [Test]
        public void Boolean()
        {
            DoTest(new ServiceElement(ElementType.Boolean, true));
        }

        //----------------
        [Test]
        public void Sequence()
        {
            List_ServiceElement seq = new List_ServiceElement();
            seq.Add(new ServiceElement(ElementType.UInt8, (Byte)69));
            DoTest(new ServiceElement(ElementType.ElementSequence, seq));
        }

        [Test]
        public void Alternative()
        {
            List_ServiceElement seq = new List_ServiceElement();
            seq.Add(new ServiceElement(ElementType.UInt8, (Byte)69));
            DoTest(new ServiceElement(ElementType.ElementAlternative, seq));
        }

        //----------------
        [Test]
        public void Url_AsUri()
        {
            Uri uri = new Uri("http://example.com/foo.txt");
            DoTest(new ServiceElement(ElementType.Url, uri));
        }

        [Test]
        public void Url_AsBytes()
        {
            byte[] uriBytes = { (byte)'h', (byte)'t', (byte)'t', (byte)'p', (byte)':', (byte)'/', (byte)'/', (byte)'e', (byte)'x', (byte)'a', (byte)'m', (byte)'p', (byte)'l', (byte)'e', (byte)'.', (byte)'c', (byte)'o', (byte)'m', (byte)'/', (byte)'f', (byte)'o', (byte)'o', (byte)'.', (byte)'t', (byte)'x', (byte)'t' };
            DoTest(new ServiceElement(ElementType.Url, uriBytes));
        }

    }//class


    public class Data_SdpCreator_CompleteRecords
    {
        public static ServiceRecord CreateServiceRecord(ServiceAttribute firstAttribute, params ServiceAttribute[] otherAttributes)
        {
            ServiceAttribute[] attrs = new ServiceAttribute[otherAttributes.Length + 1];
            attrs[0] = firstAttribute;
            otherAttributes.CopyTo(attrs, 1);
            ServiceRecord record = new ServiceRecord(attrs);
            return record;
        }

        public static ServiceAttribute CreateServiceAttribute(int id, ElementTypeDescriptor etd, ElementType elementType, object value)
        {
            ServiceAttribute attr = new ServiceAttribute((ServiceAttributeId)id, CreateServiceElement(
                etd, elementType, value
                ));
            return attr;
        }

        public static ServiceAttribute CreateServiceAttribute(int id, ElementTypeDescriptor etd, ElementType elementType, params ServiceElement[] otherElements)
        {
            ServiceAttribute attr = new ServiceAttribute((ServiceAttributeId)id, CreateServiceElement(
                etd, elementType, otherElements
                ));
            return attr;
        }

        public static ServiceElement CreateServiceElement(ElementTypeDescriptor etd, ElementType type, Object value)
        {
            Object value2 = value;
            if (type == ElementType.UInt8) {
                //if (value2 is int) {
                value2 = unchecked((Byte)(int)value2);
            } else if (type == ElementType.UInt16 || type == ElementType.Uuid16) {
                //if (value2 is int) {
                value2 = unchecked((UInt16)(int)value2);
            } else if (type == ElementType.UInt32 || type == ElementType.Uuid32) {
                value2 = unchecked((UInt32)(int)value2);
            }
            ServiceElement element = new ServiceElement(type, value2);
            if (element.ElementTypeDescriptor != etd) {
                throw new ArgumentException("wot happened there Element's ETD isn't as expected");
            }
            return element;
        }

        public static ServiceElement CreateServiceElement(ElementTypeDescriptor etd, ElementType type, params ServiceElement[] childElements)
        {
            return CreateServiceElement(etd, type, (object)childElements);
        }

        //--------------------------------------------------------------
        public static readonly ServiceRecord Xp1_Sdp_Record = CreateServiceRecord(
            /// (type: 0x0001/0x0110) UInt16: 0x0000  -- ServiceRecordHandle
            /// (type: 0x0001/0x0210) UInt32: 0x00000000
            CreateServiceAttribute(0, ElementTypeDescriptor.UnsignedInteger, ElementType.UInt32, 0),
            /// (type: 0x0001/0x0110) UInt16: 0x0001  -- ServiceClassIdList
            /// (type: 0x0006/0x0000) Element Sequence:
            /// (type: 0x0003/0x0130)     UUID16: 0x1000
            CreateServiceAttribute(1, ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[]{
                CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x1000)
            }),
            /// (type: 0x0001/0x0110) UInt16: 0x0004  -- ProtocolDescriptorList
            /// (type: 0x0006/0x0000) Element Sequence:
            /// (type: 0x0006/0x0000)     Element Sequence:
            /// (type: 0x0003/0x0130)         UUID16: 0x0100
            /// (type: 0x0001/0x0110)         UInt16: 0x0001
            /// (type: 0x0006/0x0000)     Element Sequence:
            /// (type: 0x0003/0x0130)         UUID16: 0x0001
            CreateServiceAttribute(4, ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[]{
                CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[]{
                    CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x0100),
                    CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x0001)}),
                CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[]{
                    CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x0001)}),
            }),
            /// (type: 0x0001/0x0110) UInt16: 0x0005  -- SDP_ATTRIB_BROWSE_GROUP_LIST
            /// (type: 0x0006/0x0000) Element Sequence:
            /// (type: 0x0003/0x0130)     UUID16: 0x1002
            CreateServiceAttribute(5, ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[]{
                CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x1002)
            }),
            /// (type: 0x0001/0x0110) UInt16: 0x0006  -- LanguageBaseAttributeId
            /// (type: 0x0006/0x0000) Element Sequence:
            /// (type: 0x0001/0x0110)     UInt16: 0x656e
            /// (type: 0x0001/0x0110)     UInt16: 0x006a    UTF-8
            /// (type: 0x0001/0x0110)     UInt16: 0x0100
            CreateServiceAttribute(6, ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[]{
                CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x656e),
                CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x006a),
                CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x0100)
            }),
            /// (type: 0x0001/0x0110) UInt16: 0x0100  -- ServiceName
            /// (type: 0x0004/0x0000) (String: Service Discovery)BluetoothSdpGetString0 error: 0x80010106 & 0x277e
            CreateServiceAttribute(0x0100, ElementTypeDescriptor.TextString, ElementType.TextString,
                InTheHand.Net.Tests.Sdp2.Data_CompleteThirdPartyRecords.Xp1String1),
            /// (type: 0x0001/0x0110) UInt16: 0x0101  -- Unknown
            /// (type: 0x0004/0x0000) (String: Publishes services to remote devices)BluetoothSdpGetString0 error: 0x80010106 & 0x277e
            CreateServiceAttribute(0x0101, ElementTypeDescriptor.TextString, ElementType.TextString,
                InTheHand.Net.Tests.Sdp2.Data_CompleteThirdPartyRecords.Xp1StringBytes2),
            /// (type: 0x0001/0x0110) UInt16: 0x0102  -- Unknown
            /// (type: 0x0004/0x0000) (String: Microsoft)BluetoothSdpGetString0 error: 0x80010106 & 0x277e
            CreateServiceAttribute(0x0102, ElementTypeDescriptor.TextString, ElementType.TextString,
                InTheHand.Net.Tests.Sdp2.Data_CompleteThirdPartyRecords.Xp1StringBytes3),
            /// (type: 0x0001/0x0110) UInt16: 0x0200  -- Unknown
            /// (type: 0x0006/0x0000) Element Sequence:
            /// (type: 0x0001/0x0110)     UInt16: 0x0100
            CreateServiceAttribute(0x0200, ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[]{
                CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x0100)
            }),
            /// (type: 0x0001/0x0110) UInt16: 0x0201  -- Unknown
            /// (type: 0x0001/0x0210) UInt32: 0x00000001
            CreateServiceAttribute(0x0201, ElementTypeDescriptor.UnsignedInteger, ElementType.UInt32, 1)
        );

        public static readonly ServiceRecord PalmOsOpp_Record = CreateServiceRecord(
            //AttrId: 0x0000 -- ServiceRecordHandle
            //UInt32: 0x10001
            CreateServiceAttribute(0, ElementTypeDescriptor.UnsignedInteger, ElementType.UInt32, 0x00010001),
            //AttrId: 0x0001 -- ServiceClassIdList
            //ElementSequence
            //    Uuid16: 0x1105
            CreateServiceAttribute(1, ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[] {
                CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x1105)
            }),
            //AttrId: 0x0004 -- ProtocolDescriptorList
            //  ( ( L2Cap ), ( Rfcomm, CN= 1 ), ( Obex ) )
            CreateServiceAttribute(4, ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[]{ 
                CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[] {
                    CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x0100),
                }),
                CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[] {
                    CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x0003),
                    CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt8, 0x01),
                }),
                CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[] {
                    CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x0008),
                }),
            }),
            //AttrId: 0x0006 -- LanguageBaseAttributeIdList
            //ElementSequence
            //    UInt16: 0x656E
            //    UInt16: 0x8CC     Windows-1252
            //    UInt16: 0x100
            CreateServiceAttribute(6, ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[] {
                    CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x656e),
                    CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x08cc),
                    CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x0100),
            }),
            //AttrId: 0x0100 -- ServiceName
            //TextString: [en] 'OBEX Object Push'
            CreateServiceAttribute(0x0100, ElementTypeDescriptor.TextString, ElementType.TextString,
                InTheHand.Net.Tests.Sdp2.Data_CompleteThirdPartyRecords.PalmOsOppStringBytes1),
            //AttrId: 0x0303
            //ElementSequence
            //    UInt8: 0x1
            //    UInt8: 0x2
            //    UInt8: 0x3
            //    UInt8: 0xFF
            CreateServiceAttribute(0x0303, ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[] {
                CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt8, 0x01),
                CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt8, 0x02),
                CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt8, 0x03),
                CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt8, 0xFF),
            })
        );


        public static readonly ServiceRecord XpB_1of2_1115_Record = CreateServiceRecord(
            CreateServiceAttribute(0x0000,
                ElementTypeDescriptor.UnsignedInteger, ElementType.UInt32, 0x10000),
            //--
            CreateServiceAttribute(0x0001,
                ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence,
                    CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x1115)),
            //--
            CreateServiceAttribute(0x0004, ElementTypeDescriptor.ElementSequence,
                ElementType.ElementSequence,
                    CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence,
                        CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x100),
                        CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0xF)),
                    CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence,
                        CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0xF),
                        CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x100),
                        CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence,
                            new ServiceElement[0]))),
            //--
            CreateServiceAttribute(0x0005, ElementTypeDescriptor.ElementSequence,
                ElementType.ElementSequence,
                    CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x1002)),
            //--
            CreateServiceAttribute(0x0006, ElementTypeDescriptor.ElementSequence,
                ElementType.ElementSequence,
                    CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x656E),
                    CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x3F7), //
                    CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x100)),
            //--
            CreateServiceAttribute(0x0009, ElementTypeDescriptor.ElementSequence,
                ElementType.ElementSequence,
                    CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence,
                        CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x1115),
                        CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x100))),
            //--
            CreateServiceAttribute(0x0100, ElementTypeDescriptor.TextString, ElementType.TextString,
                Encoding.Unicode.GetBytes("Personal Ad-hoc User Service")),
            //--
            CreateServiceAttribute(0x0101, ElementTypeDescriptor.TextString, ElementType.TextString,
                Encoding.Unicode.GetBytes("Personal Ad-hoc User Service")),
            //--
            CreateServiceAttribute(0x030A, ElementTypeDescriptor.UnsignedInteger,
                ElementType.UInt16, 0x0)
        );

        public static readonly ServiceRecord SimpleRfcommPdl_Record = CreateServiceRecord(
            //AttrId: 0x0004 -- ProtocolDescriptorList
            //  ( ( L2Cap ), ( Rfcomm, CN= 1 ) )
            CreateServiceAttribute(4, ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[]{ 
                CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[] {
                    CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x0100),
                }),
                CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[] {
                    CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x0003),
                    CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt8, 0x05),
                }),
            })
        );

        public static readonly ServiceRecord SimpleRfcommExplicitPsmSamePdl_Record = CreateServiceRecord(
            //AttrId: 0x0004 -- ProtocolDescriptorList
            //  ( ( L2Cap, Psm=0x0003 ), ( Rfcomm, CN= 1 ) )
            CreateServiceAttribute(4, ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[]{ 
                CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[] {
                    CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x0100),
                    CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x0003),
                }),
                CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[] {
                    CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x0003),
                    CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt8, 0x19),
                }),
            })
        );

        public static readonly ServiceRecord SimpleRfcommExplicitPsmDifferentPdl_Record = CreateServiceRecord(
            //AttrId: 0x0004 -- ProtocolDescriptorList
            //  ( ( L2Cap, Psm=0x9803 ), ( Rfcomm, CN= 1 ) )
            CreateServiceAttribute(4, ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[]{ 
                CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[] {
                    CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x0100),
                    CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x9803),
                }),
                CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[] {
                    CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x0003),
                    CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt8, 0x10),
                }),
            })
        );

        public static readonly ServiceRecord SimpleAlternativeTwoRfcommPdl_Record = CreateServiceRecord(
            //AttrId: 0x0004 -- ProtocolDescriptorList
            //  (alt ( ( L2Cap ), ( Rfcomm, CN= 1 ) ) ( ( L2Cap ), ( Rfcomm, CN= 1 ) ) )
            CreateServiceAttribute(4, ElementTypeDescriptor.ElementAlternative, ElementType.ElementAlternative, 
                new ServiceElement[] {
                    //
                    CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[]{ 
                        CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[] {
                            CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x0100),
                        }),
                        CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[] {
                            CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x0003),
                            CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt8, 0x11),
                        }),
                    }),
                    //
                    CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[]{ 
                        CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[] {
                            CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x0100),
                        }),
                        CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence, new ServiceElement[] {
                            CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x0003),
                            CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt8, 0x12),
                        }),
                    })
            })
        );

    }//class

    [TestFixture]
    public class SdpCreator_CompleteRecords
    {
        protected virtual void DoTest(byte[] expectedRecordBytes, ServiceRecord serviceRecord)
        {
            byte[] buf = new byte[256];
            int length = new ServiceRecordCreator().CreateServiceRecord(serviceRecord, buf);
            Assert2.AreEqualBuffers(expectedRecordBytes, buf, length);
        }

        //--------------------------------------------------------------
        [Test]
        public void XpSdp()
        {
            DoTest(InTheHand.Net.Tests.Sdp2.Data_CompleteThirdPartyRecords.Xp1Sdp,
                Data_SdpCreator_CompleteRecords.Xp1_Sdp_Record);
        }

        [Test]
        public void XpB_1of2_1115()
        {
            DoTest(InTheHand.Net.Tests.Sdp2.Data_CompleteThirdPartyRecords.XpB_1of2_1115,
                Data_SdpCreator_CompleteRecords.XpB_1of2_1115_Record);
        }

        [Test]
        public void PalmOsOpp()
        {
            DoTest(InTheHand.Net.Tests.Sdp2.Data_CompleteThirdPartyRecords.PalmOsOpp_HackMadeFirstLengthOneByteField,
                Data_SdpCreator_CompleteRecords.PalmOsOpp_Record);
        }

        [Test]
        public void SimpleRfcomm()
        {
            DoTest(InTheHand.Net.Tests.Sdp2.Data_CompleteThirdPartyRecords.SimpleRfcommPdl,
                Data_SdpCreator_CompleteRecords.SimpleRfcommPdl_Record);
        }

        [Test]
        public void SimpleRfcommExplicitPsmSame()
        {
            DoTest(InTheHand.Net.Tests.Sdp2.Data_CompleteThirdPartyRecords.SimpleRfcommExplicitPsmSamePdl,
                Data_SdpCreator_CompleteRecords.SimpleRfcommExplicitPsmSamePdl_Record);
        }

        [Test]
        public void SimpleRfcommExplicitPsmDifferent()
        {
            DoTest(InTheHand.Net.Tests.Sdp2.Data_CompleteThirdPartyRecords.SimpleRfcommExplicitPsmDifferentPdl,
                Data_SdpCreator_CompleteRecords.SimpleRfcommExplicitPsmDifferentPdl_Record);
        }

        [Test]
        public void SimpleAlternativeTwoRfcommPdl()
        {
            DoTest(InTheHand.Net.Tests.Sdp2.Data_CompleteThirdPartyRecords.SimpleAlternativeTwoRfcommPdl,
                Data_SdpCreator_CompleteRecords.SimpleAlternativeTwoRfcommPdl_Record);
        }

    }//class

}
