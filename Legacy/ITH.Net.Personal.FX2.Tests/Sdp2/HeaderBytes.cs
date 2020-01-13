using System;
using NUnit.Framework;
using InTheHand.Net.Bluetooth;

namespace InTheHand.Net.Tests.Sdp2
{

    [TestFixture]
    public class HeaderByteTests
    {
        private void DoTest(ElementTypeDescriptor expectedEtd, SizeIndex expectedSizeIndex, byte headerByte)
        {
            ServiceRecordParser parser = new ServiceRecordParser();
            //
            // Test the (original) individual methods.
            ElementTypeDescriptor resultEtd = ServiceRecordParser.GetElementTypeDescriptor(headerByte);
            Assert.AreEqual(expectedEtd, resultEtd);
            //
            SizeIndex resultSI = ServiceRecordParser.GetSizeIndex(headerByte);
            Assert.AreEqual(expectedSizeIndex, resultSI);
            //
            // Test the single method (which calls each of the individual methods).
            ElementTypeDescriptor resultEtd2;
            SizeIndex resultSI2;
            ServiceRecordParser.SplitHeaderByte(headerByte, out resultEtd2, out resultSI2);
            Assert.AreEqual(expectedEtd, resultEtd2);
            Assert.AreEqual(expectedSizeIndex, resultSI2);
        }


        [Test]
        public void ZeroNullSize()
        {
            DoTest((ElementTypeDescriptor)0, 0, 0x00);
        }

        [Test]
        public void OneNullSize()
        {
            DoTest((ElementTypeDescriptor)1, 0, 0x08);
        }

        [Test]
        public void ThreeNullSize()
        {
            DoTest((ElementTypeDescriptor)3, 0, 0x18);
        }

        [Test]
        public void ThirtyOneNullSize()
        {
            DoTest((ElementTypeDescriptor)31, 0, 0xF8);
        }

        //--
        [Test]
        public void ZeroMidSize()
        {
            DoTest((ElementTypeDescriptor)0, (SizeIndex)5, 0x05);
        }

        [Test]
        public void OneMidSize()
        {
            DoTest((ElementTypeDescriptor)1, (SizeIndex)5, 0x0D);
        }

        [Test]
        public void ThreeMidSize()
        {
            DoTest((ElementTypeDescriptor)3, (SizeIndex)5, 0x1D);
        }

        [Test]
        public void ThirtyOneMidSize()
        {
            DoTest((ElementTypeDescriptor)31, (SizeIndex)5, 0xFD);
        }

        //--
        [Test]
        public void ZeroFullSize()
        {
            DoTest((ElementTypeDescriptor)0, (SizeIndex)7, 0x07);
        }

        [Test]
        public void OneFullSize()
        {
            DoTest((ElementTypeDescriptor)1, (SizeIndex)7, 0x0F);
        }

        [Test]
        public void ThreeFullSize()
        {
            DoTest((ElementTypeDescriptor)3, (SizeIndex)7, 0x1F);
        }

        [Test]
        public void ThirtyOneFullSize()
        {
            DoTest((ElementTypeDescriptor)31, (SizeIndex)7, 0xFF);
        }

    }//class


    [TestFixture]
    public class HeaderByteLengthEtcTests
    {
        private void DoTest(int expectedContentLength, int expectedContentOffset, byte[] headerBytes)
        {
            DoTest(expectedContentLength, expectedContentOffset, headerBytes, 0, headerBytes.Length);
        }
        private void DoTest(int expectedContentLength, int expectedContentOffset, byte[] headerBytes, int offset, int length)
        {
            int expectedElementLength = checked(expectedContentLength + expectedContentOffset);
            ServiceRecordParser parser = new ServiceRecordParser();
            Int32 contentOffset;
            Int32 contentLength;
            Int32 elementlength = TestableServiceRecordParser.GetElementLength(headerBytes, offset, length, out contentOffset, out contentLength);
            Assert.AreEqual(expectedElementLength, elementlength);
            Assert.AreEqual(expectedContentOffset, contentOffset);
            Assert.AreEqual(expectedContentLength, contentLength);
        }

        class TestableServiceRecordParser : ServiceRecordParser
        {
            internal static new Int32 GetElementLength(byte[] buffer, int index, int length,
                out int contentOffset, out int contentLength)
            {
                return ServiceRecordParser.GetElementLength(buffer, index, length,
                    out contentOffset, out contentLength);
            }
            
            internal static Object FakeForArgExceptionCoverage_GetElementLength(byte[] buffer, int index, int length)
            {
                int contentOffset, contentLength;
                return ServiceRecordParser.GetElementLength(buffer, index, length,
                        out contentOffset, out contentLength);
            }
        }

        //----------------------------

        [Test]
        public void Nil()
        {
            DoTest(0, 1, Data_HeaderBytes.Nil);
        }

        [Test]
        public void NilOffsetByTwo()
        {
            DoTest(0, 1, Data_HeaderBytes.NilOffsetByTwo, 2, 1);
        }

        [Test]
        [ExpectedException(typeof(System.Net.ProtocolViolationException),
            ExpectedMessage = ServiceRecordParser.ErrorMsgSizeIndexNotSuitTypeD)]
        public void NilBadSizeIndex()
        {
            DoTest(0, 1, Data_HeaderBytes.NilBadSizeIndex);
        }

        [Test]
        public void OneByte()
        {
            DoTest(1, 1, Data_HeaderBytes.OneByte);
        }

        [Test]
        public void OneByteOffsetBy9()
        {
            DoTest(1, 1, Data_HeaderBytes.OneByteOffsetBy9, 9, 1);
        }

        [Test]
        public void TwoByte()
        {
            DoTest(2, 1, Data_HeaderBytes.TwoByte);
        }

        [Test]
        public void TwoByteOffsetBy9()
        {
            DoTest(2, 1, Data_HeaderBytes.TwoByteOffsetBy9, 9, 1);
        }

        [Test]
        public void FourByte()
        {
            DoTest(4, 1, Data_HeaderBytes.FourByte);
        }

        [Test]
        public void FourByteOffset()
        {
            DoTest(4, 1, Data_HeaderBytes.FourByteOffsetBy9, 9, 1);
        }

        [Test]
        public void EightByte()
        {
            DoTest(8, 1, Data_HeaderBytes.EightByte);
        }

        [Test]
        public void EightByteOffset()
        {
            DoTest(8, 1, Data_HeaderBytes.EightByteOffsetBy9, 9, 1);
        }

        [Test]
        public void SixteenByte()
        {
            DoTest(16, 1, Data_HeaderBytes.SixteenByte);
        }

        [Test]
        public void SixteenByteOffset()
        {
            DoTest(16, 1, Data_HeaderBytes.SixteenByteOffsetBy9, 9, 1);
        }

        //-----------------

        [Test]
        public void OneExtraOneByte()
        {
            DoTest(1, 2, Data_HeaderBytes.OneExtraOneByte);
        }

        [Test]
        public void OneExtraNineBytes()
        {
            DoTest(9, 2, Data_HeaderBytes.OneExtraNineBytes);
        }

        [Test]
        public void OneExtraNineBytesOffsetBy9()
        {
            DoTest(9, 2, Data_HeaderBytes.OneExtraNineBytesOffsetBy9, 9, 2);
        }

        [Test]
        public void OneExtra255Bytes()
        {
            DoTest(255, 2, Data_HeaderBytes.OneExtraMaxBytes);
        }

        [Test]
        [ExpectedException(typeof(System.Net.ProtocolViolationException), ExpectedMessage = "Header truncated from index 0.")]
        public void OneExtraTruncated()
        {
            DoTest(-1, -1, Data_HeaderBytes.OneExtraTruncated);
        }

        //-----------------

        [Test]
        public void TwoExtraOneByte()
        {
            DoTest(1, 3, Data_HeaderBytes.TwoExtraOneByte);
        }

        [Test]
        public void TwoExtraNineBytes()
        {
            DoTest(9, 3, Data_HeaderBytes.TwoExtraNineBytes);
        }

        [Test]
        public void TwoExtra0145Bytes()
        {
            DoTest(0x0145, 3, Data_HeaderBytes.TwoExtra0145Bytes);
        }

        [Test]
        public void TwoExtra0145BytesOffsetBy9()
        {
            DoTest(0x0145, 3, Data_HeaderBytes.TwoExtra0145BytesOffsetBy9, 9, 3);
        }

        [Test]
        public void TwoExtraMaxBytes()
        {
            DoTest(65535, 3, Data_HeaderBytes.TwoExtraMaxBytes);
        }

        [Test]
        [ExpectedException(typeof(System.Net.ProtocolViolationException), ExpectedMessage = "Header truncated from index 0.")]
        public void TwoExtraTruncatedAll()
        {
            DoTest(-1, -1, Data_HeaderBytes.TwoExtraTruncatedAll);
        }

        [Test]
        [ExpectedException(typeof(System.Net.ProtocolViolationException), ExpectedMessage = "Header truncated from index 0.")]
        public void TwoExtraTruncatedOne()
        {
            DoTest(-1, -1, Data_HeaderBytes.TwoExtraTruncatedOne);
        }

        //-----------------

        [Test]
        public void FourExtraOneByte()
        {
            DoTest(1, 5, Data_HeaderBytes.FourExtraOneByte);
        }

        [Test]
        public void FourExtraNineBytes()
        {
            DoTest(9, 5, Data_HeaderBytes.FourExtraNineBytes);
        }

        [Test]
        public void FourExtra01203345Bytes()
        {
            DoTest(0x01203345, 5, Data_HeaderBytes.FourExtra01203345Bytes);
        }

        [Test]
        public void FourExtra01203345BytesOffsetBy9()
        {
            DoTest(0x01203345, 5, Data_HeaderBytes.FourExtra01203345BytesOffsetBy9, 9, 5);
        }

        [Test]
        [ExpectedException(typeof(System.Net.ProtocolViolationException), ExpectedMessage = "No support for full sized 32bit length values (index 0).")]
        public void FourExtraMaxBytes()
        {
            DoTest(65535, 5, Data_HeaderBytes.FourExtraMaxBytes);
        }

        [Test]
        public void FourExtraMaxSupportedBytes()
        {
            Assert.AreEqual(0x7FFFFFFA, 2147483642, "Tests setup");
            DoTest(2147483642, 5, Data_HeaderBytes.FourExtraMaxSupportedBytes);
        }

        [Test]
        [ExpectedException(typeof(System.Net.ProtocolViolationException), ExpectedMessage = "Header truncated from index 0.")]
        public void FourExtraTruncatedAll()
        {
            DoTest(-1, -1, Data_HeaderBytes.FourExtraTruncatedAll);
        }

        [Test]
        [ExpectedException(typeof(System.Net.ProtocolViolationException), ExpectedMessage = "Header truncated from index 0.")]
        public void FourExtraTruncatedOne()
        {
            DoTest(-1, -1, Data_HeaderBytes.FourExtraTruncatedOne);
        }

        [Test]
        [ExpectedException(typeof(System.Net.ProtocolViolationException), ExpectedMessage = "Header truncated from index 9.")]
        public void FourExtraTruncatedOneOffsetBy9()
        {
            DoTest(-1, -1, Data_HeaderBytes.FourExtraTruncatedOneOffsetBy9, 9, 4);
        }

        //---------------

        [Test]
        public void GetElementLengthArgExCoverage()
        {
            CoverArgException.CoverStreamLikeReadOrWriteMethod method =
                new CoverArgException.CoverStreamLikeReadOrWriteMethod(
                    TestableServiceRecordParser.FakeForArgExceptionCoverage_GetElementLength);
            CoverArgException.CoverStreamLikeReadOrWrite(method);
        }
    }//class

}