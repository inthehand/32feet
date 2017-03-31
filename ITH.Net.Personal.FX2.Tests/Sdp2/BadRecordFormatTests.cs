using System;
using NUnit.Framework;
using System.Net;
using InTheHand.Net.Bluetooth;

namespace InTheHand.Net.Tests.Sdp2
{

    [TestFixture]
    public class BadRecordFormatTests_OverrunsBuffer
    {
        public static readonly byte[] DataSeq = { 0x35, 8, 
                0x09, 0x00, 0x00, 
                0x35, 6, /**/0x09, 0x11, 0x05, /**/0x08, 0xFF };
        public static readonly byte[] DataAlt = { 0x35, 8, 
                0x09, 0x00, 0x00, 
                0x3D, 6, /**/0x09, 0x11, 0x05, /**/0x08, 0xFF };
        public static readonly byte[] DataTopSeq = { 0x35, 7, 
                0x09, 0x00, 0x00, 
                0x09, 0x11, 0x05 };

        private static void DoTest(byte[] bytes)
        {
            new ServiceRecordParser().Parse(bytes);
        }

        private static void DoTestOffsetInput(byte[] bytes)
        {
            int Offset = 100;
            byte[] atOffset = new byte[bytes.Length + Offset];
            bytes.CopyTo(atOffset, Offset);
            new ServiceRecordParser().Parse(atOffset, Offset, bytes.Length);
        }


        [Test]
        [ExpectedException(typeof(ProtocolViolationException), ExpectedMessage = ServiceRecordParser.ErrorMsgElementOverrunsBufferPrefix + "5.")]
        public void Seq()
        {
            DoTest(DataSeq);
        }

        [Test]
        [ExpectedException(typeof(ProtocolViolationException), ExpectedMessage = ServiceRecordParser.ErrorMsgElementOverrunsBufferPrefix + "5"
#if SDP_OVERRUN_HAS_LENGTHS
 + ", item length is 6 but remaining length is only 5"
#endif
 + ".")]
        public void Alt()
        {
            DoTest(DataAlt);
        }

        [Test]
        [ExpectedException(typeof(ProtocolViolationException), ExpectedMessage = ServiceRecordParser.ErrorMsgElementOverrunsBufferPrefix + "0.")]
        public void TopSeq()
        {
            DoTest(DataTopSeq);
        }

        [Test]
        [ExpectedException(typeof(ProtocolViolationException), ExpectedMessage = ServiceRecordParser.ErrorMsgElementOverrunsBufferPrefix + "105.")]
        public void Seq_Offset()
        {
            DoTestOffsetInput(DataSeq);
        }

        [Test]
        [ExpectedException(typeof(ProtocolViolationException), ExpectedMessage = ServiceRecordParser.ErrorMsgElementOverrunsBufferPrefix + "105.")]
        public void Alt_Offset()
        {
            DoTestOffsetInput(DataAlt);
        }

        [Test]
        [ExpectedException(typeof(ProtocolViolationException), ExpectedMessage = ServiceRecordParser.ErrorMsgElementOverrunsBufferPrefix + "100.")]
        public void TopSeq_Offset()
        {
            DoTestOffsetInput(DataTopSeq);
        }

    }//class


    [TestFixture]
    public class BadRecordFormatTests_ElementOverrunsSeqOrAlt
    {
        public static readonly byte[] DataUInt32OverrunsSeq = {
            0x35,10,    //this is correct, include all the UInt16's bytes
                0x09,0x70,0x00,
                0x35,4, // this is one byte too small
                    0x0A,0x01,0x02,0x03,/*]*/0x04
        };
        public static readonly byte[] DataUInt8OverrunsSeq = {
            0x35,7,
                0x09,0x70,0x00,
                0x35,1,
                    0x08,/*]*/0x01,
        };
        public static readonly byte[] DataSecondUInt32OverrunsSeq = {
            0x35,15,    //this is correct, include all the UInt16's bytes
                0x09,0x70,0x00,
                0x35,9, // this is one byte too small
                    0x0A,0x01,0x02,0x03,0x04,
                    0x0A,0x01,0x02,0x03,/*]*/0x04
        };
        public static readonly byte[] DataSecondUInt8OverrunsSeq = {
            0x35,9,
                0x09,0x70,0x00,
                0x35,3,
                    0x08, 0x01,
                    0x08,/*]*/0x01,
        };

        //--------------------------------------------------------------

        private void DoTest(byte[] buffer)
        {
            new ServiceRecordParser().Parse(buffer);
        }


        [Test]
        [ExpectedException(typeof(System.Net.ProtocolViolationException),
            ExpectedMessage = "Element overruns buffer section, from index 7"
#if SDP_OVERRUN_HAS_LENGTHS
 + ", item length is 5 but remaining length is only 4"
#endif
 + "."
            //ExpectedMessage = "Header truncated from index 8."
            )]
        public void UInt32OverrunsSeq()
        {
            DoTest(DataUInt32OverrunsSeq);
        }

        [Test]
        [ExpectedException(typeof(System.Net.ProtocolViolationException),
            ExpectedMessage = "Element overruns buffer section, from index 7"
#if SDP_OVERRUN_HAS_LENGTHS
 + ", item length is 2 but remaining length is only 1"
#endif
 + "."
            )]
        public void UInt8OverrunsSeq()
        {
            DoTest(DataUInt8OverrunsSeq);
        }

        [Test]
        [ExpectedException(typeof(System.Net.ProtocolViolationException),
            ExpectedMessage = "Element overruns buffer section, from index 12"
#if SDP_OVERRUN_HAS_LENGTHS
 + ", item length is 5 but remaining length is only 4"
#endif
 + "."
            )]
        public void SecondUInt32OverrunsSeq()
        {
            DoTest(DataSecondUInt32OverrunsSeq);
        }

        [Test]
        [ExpectedException(typeof(System.Net.ProtocolViolationException),
            ExpectedMessage = "Element overruns buffer section, from index 9"
#if SDP_OVERRUN_HAS_LENGTHS
 + ", item length is 2 but remaining length is only 1"
#endif
 + "."
            )]
        public void SecondUInt8OverrunsSeq()
        {
            DoTest(DataSecondUInt8OverrunsSeq);
        }

    }//class


    [TestFixture]
    public class BadRecordFormatTests_BadContent
    {
        public static readonly byte[] DataTwiceWrappedFromACoulter = {
            0x36, 0x00, 0x30, 
            0x36, 0x00, 0x2D, 0x09, 0x00,  0x00, 0x0A, 0x00, 0x01, 0x00, 0x00, 0x09, 0x00, 0x01, 0x35, 0x03, 
            0x19, 0x11, 0x01, 0x09, 0x00,  0x04, 0x35, 0x0C, 0x35, 0x03, 0x19, 0x01, 0x00, 0x35, 0x05, 0x19, 
            0x00, 0x03, 0x08, 0x01, 0x09,  0x01, 0x00, 0x25, 0x07, 0x50, 0x52, 0x49, 0x4E, 0x54, 0x45, 0x52
        };

        //--------
        private static void DoTest(byte[] bytes)
        {
            new ServiceRecordParser().Parse(bytes);
        }

        //--------

        [Test]
        [ExpectedException(typeof(System.Net.ProtocolViolationException),
            ExpectedMessage = "The Attribute Id at index 3 is not of type Uint16.")]
        public void TwiceWrappedFromACoulter()
        {
            DoTest(DataTwiceWrappedFromACoulter);
        }

    }
}
