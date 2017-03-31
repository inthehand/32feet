using System;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.AttributeIds;

namespace InTheHand.Net.Tests.Sdp2
{

    public 
#if ! FX1_1
        static
#endif
        class Data_HeaderBytes
    {
        public static readonly byte[] Nil = { 0x0 };
        public static readonly byte[] NilOffsetByTwo = { 0x99, 0x99, 0x0 };
        public static readonly byte[] NilBadSizeIndex = { 0x01 };
        public static readonly byte[] OneByte = { 0x08 };
        public static readonly byte[] OneByteOffsetBy9 = { 0, 0, 0, 0, 0, 0, 0, 0, 0,/**/0x08 };
        public static readonly byte[] TwoByte = { 0x09 };
        public static readonly byte[] TwoByteOffsetBy9 = { 0, 0, 0, 0, 0, 0, 0, 0, 0,/**/0x09 };
        public static readonly byte[] FourByte = { 0x0A };
        public static readonly byte[] FourByteOffsetBy9 = { 0, 0, 0, 0, 0, 0, 0, 0, 0,/**/0x0A };
        public static readonly byte[] EightByte = { 0x0B };
        public static readonly byte[] EightByteOffsetBy9 = { 0, 0, 0, 0, 0, 0, 0, 0, 0,/**/0x0B };
        public static readonly byte[] SixteenByte = { 0x0C };
        public static readonly byte[] SixteenByteOffsetBy9 = { 0, 0, 0, 0, 0, 0, 0, 0, 0,/**/0x0C };
        //
        public static readonly byte[] OneExtraOneByte = { 0x0D, 0x01 };
        public static readonly byte[] OneExtraNineBytes = { 0x0D, 0x09 };
        public static readonly byte[] OneExtraNineBytesOffsetBy9 = { 0, 0, 0, 0, 0, 0, 0, 0, 0,/**/0x0D, 0x09 };
        public static readonly byte[] OneExtraMaxBytes = { 0x0D, 0xFF };
        public static readonly byte[] OneExtraTruncated = { 0x0D };
        //
        public static readonly byte[] TwoExtraOneByte = { 0x0E, 0x00, 0x01 };
        public static readonly byte[] TwoExtraNineBytes = { 0x0E, 0x00, 0x09 };
        public static readonly byte[] TwoExtra0145Bytes = { 0x0E, 0x01, 0x45 };
        public static readonly byte[] TwoExtra0145BytesOffsetBy9 = { 0, 0, 0, 0, 0, 0, 0, 0, 0,/**/0x0E, 0x01, 0x45 };
        public static readonly byte[] TwoExtraMaxBytes = { 0x0E, 0xFF, 0xFF };
        public static readonly byte[] TwoExtraTruncatedAll = { 0x0E };
        public static readonly byte[] TwoExtraTruncatedOne = { 0x0E, 0x00 };
        //
        public static readonly byte[] FourExtraOneByte = { 0x0F, 0x00, 0x00, 0x00, 0x01 };
        public static readonly byte[] FourExtraNineBytes = { 0x0F, 0x00, 0x00, 0x00, 0x09 };
        public static readonly byte[] FourExtra01203345Bytes = { 0x0F, 0x01, 0x20, 0x33, 0x45 };
        public static readonly byte[] FourExtra01203345BytesOffsetBy9 = { 0, 0, 0, 0, 0, 0, 0, 0, 0,/**/0x0F, 0x01, 0x20, 0x33, 0x45 };
        public static readonly byte[] FourExtraMaxBytes = { 0x0F, 0xFF, 0xFF, 0xFF, 0xFF };
        public static readonly byte[] FourExtraMaxSupportedBytes = { 0x0F, 0x7F, 0xFF, 0xFF, 0xFA };
        public static readonly byte[] FourExtraTruncatedAll = { 0x0F };
        public static readonly byte[] FourExtraTruncatedOne = { 0x0F, 0x00, 0x00, 0x00 };
        public static readonly byte[] FourExtraTruncatedOneOffsetBy9 = { 0, 0, 0, 0, 0, 0, 0, 0, 0,/**/0x0F, 0x00, 0x00, 0x00 };
    }//class


    public
#if ! FX1_1
        static 
#endif
        class Data_SimpleRecords
    {
        public const ServiceAttributeId Id0 = (ServiceAttributeId)(short)0; // Casts for NETCFv1.
        public const UInt16 IdF123Uint = 0xF123;
        public const ServiceAttributeId IdF123 = unchecked((ServiceAttributeId)IdF123Uint);


        public static readonly byte[] Empty = { 0x35, 0x00 };

        public static readonly byte[] OneNil = { 0x35, 0x04, 
            0x09, 0x00, 0x00, /**/0x00, };
        public static readonly byte[] OneNilBadSizeIndex = { 0x35, 0x04, 
            0x09, 0x00, 0x00, /**/0x02, };

        //--------------------------------------------------------------
        public static readonly byte[] OneSeqWithNil = { 0x35,/*length*/6,
            /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x35, 1, /*value element - Nil*/ 0x00 };
        public static readonly byte[] OneSeqEmpty = { 0x35,/*length*/5,
            /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x35, 0, /*not content*/ };
        public static readonly byte[] OneSeqWithOneSeqWithNil = { 0x35,/*length*/8,
            /*id*/0x09, 0xFF, 0xFF,
                /*value element*/ 0x35, 3,
                    /*value element*/ 0x35, 1,
                        /*value element - Nil*/ 0x00
        };
        public static readonly byte[] OneSeqWithOneSeqWithTwoNil = { 0x35,/*length*/9,
            /*id*/0x09, 0xFF, 0xFF,
                /*value element*/ 0x35, 4,
                    /*value element*/ 0x35, 2,
                        /*value element - Nil*/ 0x00,
                        /*value element - Nil*/ 0x00
        };
        public static readonly byte[] OneSeqWith__OneSeqWithTwoNil_And_OneSeqWithOneNil__And_Int8
             = { 0x35,/*length*/17,
            /*id*/0x09, 0xFF, 0xFF,
                /*value element*/ 0x35, 7,
                    /*value element*/ 0x35, 2,
                        /*value element - Nil*/ 0x00,
                        /*value element - Nil*/ 0x00,
                    /*value element*/ 0x35, 1,
                        /*value element - Nil*/ 0x00,
            /*id*/0x09, 0x00, 0x02,
                 0x08, 0x99
        };

        //--------------------------------------------------------------
        public static readonly byte[] OneAltWithNil = { 0x35,/*length*/6,
            /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x3D, 1, /*value element - Nil*/ 0x00 };
        public static readonly byte[] OneAltWithOneAltWithNil = { 0x35,/*length*/8,
            /*id*/0x09, 0xFF, 0xFF,
                /*value element*/ 0x3D, 3,
                    /*value element*/ 0x3D, 1,
                        /*value element - Nil*/ 0x00
        };
        public static readonly byte[] OneSeqWithOneAltWithNil = { 0x35,/*length*/8,
            /*id*/0x09, 0xFF, 0xFF,
                /*value element*/ 0x35, 3,
                    /*value element*/ 0x3D, 1,
                        /*value element - Nil*/ 0x00
        };
        public static readonly byte[] OneAltWithOneSeqWithNil = { 0x35,/*length*/8,
            /*id*/0x09, 0xFF, 0xFF,
                /*value element*/ 0x3D, 3,
                    /*value element*/ 0x35, 1,
                        /*value element - Nil*/ 0x00
        };
        public static readonly byte[] OneAltWithOneAltWithTwoNil = { 0x35,/*length*/9,
            /*id*/0x09, 0xFF, 0xFF,
                /*value element*/ 0x3D, 4,
                    /*value element*/ 0x3D, 2,
                        /*value element - Nil*/ 0x00,
                        /*value element - Nil*/ 0x00
        };
        public static readonly byte[] OneAltWith__OneAltWithTwoNil_And_OneAltWithOneNil__And_Int8
             = { 0x35,/*length*/17,
            /*id*/0x09, 0xFF, 0xFF,
                /*value element*/ 0x3D, 7,
                    /*value element*/ 0x3D, 2,
                        /*value element - Nil*/ 0x00,
                        /*value element - Nil*/ 0x00,
                    /*value element*/ 0x3D, 1,
                        /*value element - Nil*/ 0x00,
            /*id*/0x09, 0x00, 0x02,
                 0x08, 0x99
        };

        //---------------------------
        public static readonly byte[] OneUInt8ZeroZero = { 0x35, 0x05, 
            0x09, 0x00, 0x00, /**/0x08, 0x00, };
        public static readonly byte[] OneUInt8F123_E9 = { 0x35, 0x05, 
            0x09, 0xF1, 0x23, /**/0x08, 0xE9, };
        // Cause the result value to be widened, to check that it is NOT sign-extended,
        // to check that it is stored as unsigned.
        public const Int32 OneUInt8F123_E9Value = unchecked((Int32)0x000000E9);
        //
        public static readonly byte[] OneInt8ZeroZero = { 0x35, 0x05, 
            0x09, 0x00, 0x00, /**/0x10, 0x00, };
        public static readonly byte[] OneInt8F123_E9 = { 0x35, 0x05, 
            0x09, 0xF1, 0x23, /**/0x10, 0xE9, };
        // Cause the result value to be widened, to check that it is sign-extended,
        // to check that it is stored as signed.
        public const Int32 OneInt8F123_E9Value = unchecked((Int32)0xFFFFFFE9);

        public static readonly byte[] OneUInt16ZeroZero = { 0x35, 0x06, 
            0x09, 0x00, 0x00, /**/0x09, 0x00, 0x00,  };
        public static readonly byte[] OneUInt16F123_E987 = { 0x35, 0x06, 
            0x09, 0xF1, 0x23, /**/0x09, 0xE9, 0x87, };
        public static readonly byte[] OneInt16_F123_E987 = { 0x35, 0x06, 
            0x09, 0xF1, 0x23, /**/0x11, 0xE9, 0x87, };
        public static UInt16 OneUInt16_F123_E987_Value = 0xE987;
        public static Int16 OneInt16_F123_E987_Value = unchecked((Int16)OneUInt16_F123_E987_Value);

        public static readonly byte[] OneUInt32ZeroZero = { 0x35, 0x08, 
            0x09, 0x00, 0x00, /**/0x0A, 0x00, 0x00, 0x00, 0x00, };
        public static readonly byte[] OneUInt32F123_E9876543 = { 0x35, 0x08, 
            0x09, 0xF1, 0x23, /**/0x0A, 0xE9, 0x87, 0x65, 0x43, };
        public static readonly byte[] OneInt32_F123_E9876543 = { 0x35, 0x08, 
            0x09, 0xF1, 0x23, /**/0x12, 0xE9, 0x87, 0x65, 0x43, };
        public static readonly byte[] OneUInt32F123_E9876543OffsetBy4 = { 
            0,0,0,0,
            0x35, 0x08, 
            0x09, 0xF1, 0x23, /**/0x0A, 0xE9, 0x87, 0x65, 0x43, };
        public static readonly byte[] TwoUInt32OffsetBy1 = { 
            0,
            0x35, 8+8, 
            0x09, 0x00, 0x00, /**/0x0A, 0x00, 0x00, 0x00, 0x00,
            0x09, 0xF1, 0x23, /**/0x0A, 0xE9, 0x87, 0x65, 0x43, };
        public static readonly UInt32 OneUInt32_F123_E9876543_Value = 0xE9876543;
        public static readonly Int32 OneInt32_F123_E9876543_Value = unchecked((Int32)OneUInt32_F123_E9876543_Value);

        public static readonly byte[] OneInt64_F123_E987654305060708 = { 0x35, 12, 
            0x09, 0xF1, 0x23, /**/0x13, 0xE9, 0x87, 0x65, 0x43,5,6,7,8 };
        public static readonly UInt64 OneUInt64_F123_E987654305060708_Value = 0xE987654305060708;
        public static readonly Int64 OneInt64_F123_E987654305060708_Value = unchecked((Int64)OneUInt64_F123_E987654305060708_Value);

        public static readonly byte[] OneUInt128_F123_E987 = { 0x35, 0x06+14, 
            0x09, 0xF1, 0x23,
            /**/
            0x0C, 0xE9, 0x87, 3,4,5,6,7,8,9,10,11,12,13,14,15,16, };
        public static readonly byte[] OneInt128_F123_E987 = { 0x35, 0x06+14, 
            0x09, 0xF1, 0x23,
            /**/
            0x14, 0xE9, 0x87, 3,4,5,6,7,8,9,10,11,12,13,14,15,16, };
        public static readonly byte[] OneUInt128_Value
            = { 0xE9, 0x87, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,16 };


        public static readonly byte[] OneUuid16 = { 
            0x35, 6, 
            0x09, 0xF1, 0x23, /**/0x19, 0x11, 0x01 };
        public const /*U*/Int16 OneUuid16Value = 0x1101;
        public static readonly byte[] Uuid16OffsetBy1 = { 
            0,
            0x35, 6, 
            0x09, 0xF1, 0x23, /**/0x19, 0x11, 0x01 };
        public static readonly byte[] OneUuid32 = { 
            0x35, 8, 
            0x09, 0xF1, 0x23, /**/0x1A, 0xF1, 0x99, 0x11, 0x01 };
        public const UInt32 OneUuid32Value = 0xF1991101;

        //---------------------------------------------------
        public static readonly byte[] OneString = { 
            0x35, 39, 
            0x09, 0xF1, 0x23, /**/0x25, 34,
            /*content*/(byte)'h', (byte)'t', (byte)'t', (byte)'p', (byte)':', (byte)'/',
            (byte)'/', (byte)'m', (byte)'y', (byte)'.', (byte)'f', (byte)'a', (byte)'k',
            (byte)'e', (byte)'/', (byte)'p', (byte)'u', (byte)'b', (byte)'l', (byte)'i',
            (byte)'c', (byte)'/', (byte)'*', (byte)'/', (byte)'c', (byte)'l', (byte)'i',
            (byte)'e', (byte)'n', (byte)'t', (byte)'.', (byte)'e', (byte)'x', (byte)'e'
        };
        public static readonly byte[] OneStringValueBytes = { 
            /*content*/(byte)'h', (byte)'t', (byte)'t', (byte)'p', (byte)':', (byte)'/',
            (byte)'/', (byte)'m', (byte)'y', (byte)'.', (byte)'f', (byte)'a', (byte)'k',
            (byte)'e', (byte)'/', (byte)'p', (byte)'u', (byte)'b', (byte)'l', (byte)'i',
            (byte)'c', (byte)'/', (byte)'*', (byte)'/', (byte)'c', (byte)'l', (byte)'i',
            (byte)'e', (byte)'n', (byte)'t', (byte)'.', (byte)'e', (byte)'x', (byte)'e'
        };

        public static readonly byte[] OneStringZeroLength = { 
            0x35, 5, 
            0x09, 0xF1, 0x23, /**/0x25, 0,
        };

        public static readonly byte[] OneStringBadUtf8 = {
            0x35, 9,
                0x09, 0xF1, 0x23,
                0x25,4,
                    0xFF,0x61,0x62,0xFF,
        };
        public static readonly byte[] OneStringBadUtf8_ValueBytes = { 
            0xFF,0x61,0x62,0xFF,
        };

        //---------------------------------------------------
        public static readonly byte[] OneBooleanZero = { 
            0x35, 5, 
            0x09, 0xF1, 0x23, /**/0x28, 0x00 };
        public static readonly byte[] OneBooleanOne = { 
            0x35, 5, 
            0x09, 0xF1, 0x23, /**/0x28, 0x01 };
        public static readonly byte[] OneBooleanNonZero = { 
            0x35, 5, 
            0x09, 0xF1, 0x23, /**/0x28, 0x99 };
        //---------------------------------------------------
        public static readonly byte[] OneUrl = { 
            0x35, 39, 
            0x09, 0xF1, 0x23, /**/0x45, 34,
            /*content*/(byte)'h', (byte)'t', (byte)'t', (byte)'p', (byte)':', (byte)'/',
            (byte)'/', (byte)'m', (byte)'y', (byte)'.', (byte)'f', (byte)'a', (byte)'k',
            (byte)'e', (byte)'/', (byte)'p', (byte)'u', (byte)'b', (byte)'l', (byte)'i',
            (byte)'c', (byte)'/', (byte)'*', (byte)'/', (byte)'c', (byte)'l', (byte)'i',
            (byte)'e', (byte)'n', (byte)'t', (byte)'.', (byte)'e', (byte)'x', (byte)'e'
        };
        public const string OneUrlStringValue = "http://my.fake/public/*/client.exe";
        public static readonly byte[] OneUrlByteArrayValue = {
            /*content*/(byte)'h', (byte)'t', (byte)'t', (byte)'p', (byte)':', (byte)'/',
            (byte)'/', (byte)'m', (byte)'y', (byte)'.', (byte)'f', (byte)'a', (byte)'k',
            (byte)'e', (byte)'/', (byte)'p', (byte)'u', (byte)'b', (byte)'l', (byte)'i',
            (byte)'c', (byte)'/', (byte)'*', (byte)'/', (byte)'c', (byte)'l', (byte)'i',
            (byte)'e', (byte)'n', (byte)'t', (byte)'.', (byte)'e', (byte)'x', (byte)'e'
        };

        //--------------------------------------------------------------
        public static byte[] CreateHugeRecord()
        {
            // Create a huge record from a template and string bytes.
            byte[] data = new byte[Data_SimpleRecords.HugeRecordValueTemplate_PrefixLength
                + Data_SimpleRecords.HugeRecordValueTemplate_StringContentLen
                + Data_SimpleRecords.HugeRecordValueTemplate_SuffixLength];
            // Prefix
            Array.Copy(Data_SimpleRecords.HugeRecordValueTemplate, 0, data, 0, Data_SimpleRecords.HugeRecordValueTemplate_PrefixLength);
            // Suffix
            Array.Copy(Data_SimpleRecords.HugeRecordValueTemplate,
                Data_SimpleRecords.HugeRecordValueTemplate.Length - Data_SimpleRecords.HugeRecordValueTemplate_SuffixLength,
                data,
                data.Length - Data_SimpleRecords.HugeRecordValueTemplate_SuffixLength,
                Data_SimpleRecords.HugeRecordValueTemplate_SuffixLength);
            // String content
            for (int i = 0; i < Data_SimpleRecords.HugeRecordValueTemplate_StringContentLen; ++i) {
                data[i + Data_SimpleRecords.HugeRecordValueTemplate_PrefixLength] = (byte)'a';
            }
            return data;
        }

        public static readonly byte[] HugeRecordValueTemplate = {
            0x37, /*66000+24=*/0x00,0x01,0x01,0xE8,
            0x09, 0x00, 0x00, /**/0x0A, 0x00, 0x00, 0x00, 0x00,
            0x09, 0x10,0x02, /**/0x27, /*66000=*/0x00,0x01,0x01,0xD0,
              //0x61,0x61,0x61,0x61,0x61,0x61,0x61,0x61, 0x61,0x61,0x61,0x61,0x61,0x61,0x61,0x61,
            0x09, 0xF1, 0x23, /**/0x0A, 0xE9, 0x87, 0x65, 0x43,
        };
        public const int HugeRecordValueTemplate_StringContentLen = 66000;
        public const int HugeRecordValueTemplate_PrefixLength = 21;
        public const int HugeRecordValueTemplate_SuffixLength = 8;
        public const ServiceAttributeId IdHuge1 = unchecked((ServiceAttributeId)0x1002);

    }//class
}
