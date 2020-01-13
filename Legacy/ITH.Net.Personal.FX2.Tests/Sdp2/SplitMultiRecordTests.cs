using System;
using NUnit.Framework;
using InTheHand.Net.Tests.Sdp2;
using InTheHand.Net.Bluetooth;
using System.Net;

namespace InTheHand.Net.Tests.Sdp2
{

    [TestFixture]
    public class SplitMultiRecordTests
    {

        private void DoTest(byte[] multiRecord, params byte[][] expectedRecords)
        {
            byte[][] records = ServiceRecordParser.SplitSearchAttributeResult(multiRecord);
            Assert.AreEqual(expectedRecords.Length, records.Length, "Record count");
            for (int i = 0; i < expectedRecords.Length; ++i) {
                Assert.AreEqual(expectedRecords[i], records[i], 
                    String.Format("Record content -- record index {0}/{1}.", i, expectedRecords.Length));
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "Value cannot be null."
          + Tests_Values.NewLine + "Parameter name: multiRecord")]
        public void Null()
        {
            DoTest(null);
        }

        [Test]
        public void ZeroRecords()
        {
            DoTest(Data_SimpleRecords.Empty, new byte[0][]);
        }

        [Test]
        public void Two()
        {
            DoTest(SplitMultiRecordTests_Data.Two,
                Data_SimpleRecords.OneUInt8ZeroZero, Data_SimpleRecords.OneUInt8F123_E9);
        }

        [Test]
        [ExpectedException(typeof(ProtocolViolationException), ExpectedMessage = "Element overruns buffer section, from index 0"
#if SDP_OVERRUN_HAS_LENGTHS
 + ", item length is 14 but remaining length is only 13"
#endif
 + ".")]
        public void Two_Truncated1()
        {
            byte[] truncated = TruncatedBy(SplitMultiRecordTests_Data.Two, 1);
            DoTest(truncated,
                Data_SimpleRecords.OneUInt8ZeroZero, Data_SimpleRecords.OneUInt8F123_E9);
        }

        private byte[] TruncatedBy(byte[] buffer, int truncateAmount)
        {
            if (truncateAmount <= 0) {
                throw Tests_Values.new_ArgumentOutOfRangeException("truncateAmount", "Value must be a positive non-zero value.");
            }
            byte[] truncated = new byte[buffer.Length - truncateAmount];
            Array.Copy(buffer, 0, truncated, 0, truncated.Length);
            return truncated;
        }

        [Test]
        public void ManyA_IncreasingSize()
        {
            DoTest(SplitMultiRecordTests_Data.ManyA_IncreasingSize,
                SplitMultiRecordTests_Data.ManyA_IncreasingSize_AllItems);
        }

        [Test]
        public void ManyA_DecreasingSize()
        {
            DoTest(SplitMultiRecordTests_Data.ManyA_DecreasingSize,
                SplitMultiRecordTests_Data.ManyA_DecreasingSize_AllItems);
        }

        [Test]
        public void ManyB_LongerBufferThanContent()
        {
            DoTest(SplitMultiRecordTests_Data.ManyB_LongerBufferThanContent,
                SplitMultiRecordTests_Data.ManyA_Item0, SplitMultiRecordTests_Data.ManyA_Item1);
        }

        [Test]
        [ExpectedException(typeof(ProtocolViolationException), ExpectedMessage = "Header truncated from index 9.")]
        public void ManyB_TopLevelLengthShort_AtA()
        {
            DoTest(SplitMultiRecordTests_Data.ManyB_TopLevelLengthShort_AtA);
        }

        [Test]
        [ExpectedException(typeof(ProtocolViolationException), ExpectedMessage = "Element overruns buffer section, from index 9"
#if SDP_OVERRUN_HAS_LENGTHS
 + ", item length is 5 but remaining length is only 1"
#endif
 + ".")]
        public void ManyB_TopLevelLengthShort_AtB()
        {
            DoTest(SplitMultiRecordTests_Data.ManyB_TopLevelLengthShort_AtB);
        }

        [Test]
        [ExpectedException(typeof(ProtocolViolationException), ExpectedMessage = ServiceRecordParser.ErrorMsgMultiSeqChildElementNotSequence)]
        public void ChildNotSeq()
        {
            DoTest(SplitMultiRecordTests_Data.ChildNotSeq);
        }

    }//class

    public class SplitMultiRecordTests_Data
    {

        //--------
        public static readonly byte[] Two = {
            0x35, 14,
            // OneUInt8ZeroZero
            0x35, 0x05, 
                0x09, 0x00, 0x00, /**/0x08, 0x00,
            // OneUInt8F123_E9
            0x35, 0x05, 
                0x09, 0xF1, 0x23, /**/0x08, 0xE9,
        };

        //--------
        public static readonly byte[] ManyA_IncreasingSize = {
            0x35, 26,
                // 0 content
                0x35, 0,
                // 3 content
                0x35, 3, 0x09, 0xF1, 0x23,
                // 5 content
                0x35, 5, 0x09, 0xF1, 0x23, 0x08, 0x45,
                // 10 content
                0x35, 10, 0x09, 0xF1, 0x25, 0x08, 0x46, 0x09, 0xF1, 0x24, 0x08, 0x46,
        };
        public static readonly byte[] ManyA_DecreasingSize = {
            0x35, 26,
                // 10 content
                0x35, 10, 0x09, 0xF1, 0x25, 0x08, 0x46, 0x09, 0xF1, 0x24, 0x08, 0x46,
                // 5 content
                0x35, 5, 0x09, 0xF1, 0x23, 0x08, 0x45,
                // 3 content
                0x35, 3, 0x09, 0xF1, 0x23,
                // 0 content
                0x35, 0,
        };
        public static readonly byte[] ManyA_Item0 = {
                // 0 content
                0x35, 0,
        };
        public static readonly byte[] ManyA_Item1 = {
                // 3 content
                0x35, 3, 0x09, 0xF1, 0x23,
        };
        public static readonly byte[] ManyA_Item2 = {
                // 5 content
                0x35, 5, 0x09, 0xF1, 0x23, 0x08, 0x45,
        };
        public static readonly byte[] ManyA_Item3 = {
                // 10 content
                0x35, 10, 0x09, 0xF1, 0x25, 0x08, 0x46, 0x09, 0xF1, 0x24, 0x08, 0x46,
        };
        public static readonly byte[][] ManyA_IncreasingSize_AllItems = {
            ManyA_Item0, ManyA_Item1, ManyA_Item2, ManyA_Item3, 
        };
        public static readonly byte[][] ManyA_DecreasingSize_AllItems = {
            ManyA_Item3, ManyA_Item2, ManyA_Item1, ManyA_Item0, 
        };

        //--------
        public static readonly byte[] ManyB_LongerBufferThanContent = { //originally based on ManyA_IncreasingSize 
            0x35, 7,
                // 0 content
                0x35, 0,
                // 3 content
                0x35, 3, 0x09, 0xF1, 0x23,
            // top-level length say the data ends here
            //
                // 5 content
                0x35, 5, 0x09, 0xF1, 0x23, 0x08, 0x45,
                // 10 content
                0x35, 10, 0x09, 0xF1, 0x25, 0x08, 0x46, 0x09, 0xF1, 0x24, 0x08, 0x46,
        };

        public static readonly byte[] ManyB_TopLevelLengthShort_AtA = { //originally based on ManyA_IncreasingSize 
            0x35, 8,
                // 0 content
                0x35, 0,
                // 3 content
                0x35, 3, 0x09, 0xF1, 0x23,
                // 5 content
                0x35, 
            // top-level length say the data ends here
            //
                        5, 0x09, 0xF1, 0x23, 0x08, 0x45,
                // 10 content
                0x35, 10, 0x09, 0xF1, 0x25, 0x08, 0x46, 0x09, 0xF1, 0x24, 0x08, 0x46,
        };

        public static readonly byte[] ManyB_TopLevelLengthShort_AtB = { //originally based on ManyA_IncreasingSize 
            0x35, 10,
                // 0 content
                0x35, 0,
                // 3 content
                0x35, 3, 0x09, 0xF1, 0x23,
                // 5 content
                0x35, 5, 0x09, 
            // top-level length say the data ends here
            //
                                0xF1, 0x23, 0x08, 0x45,
                // 10 content
                0x35, 10, 0x09, 0xF1, 0x25, 0x08, 0x46, 0x09, 0xF1, 0x24, 0x08, 0x46,
        };

        //--------
        public static readonly byte[] ChildNotSeq = { //originally based on ManyA_IncreasingSize 
            0x35, 10,
                // 5 content
                0x35, 5, 0x09, 0xF1, 0x23, 0x08, 0x45,
                // not a seq element!
                0x09, 0xF1, 0x23,
        };

    }//class

}
