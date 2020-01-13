using System;
//using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using InTheHand.Net.Bluetooth;
#if FX1_1
using NotImplementedException = System.NotSupportedException;
#endif

namespace InTheHand.Net.Tests.Sdp2
{
    public 
#if ! FX1_1
        static 
#endif
        class Data_AllElementTypes
    {
        public static readonly byte[] __Template = { 0x35,/*length*/0, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 99 };
        //
        // Nil = 0, -> 0x00
        public static readonly byte[] Nil = { 0x35,/*length*/4, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x00 };
        //
        // UnsignedInteger = 1, -> 0x08
        public static readonly byte[] UInt8 = { 0x35,/*length*/5, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x08, 0xF1 };
        public static readonly byte[] UInt16 = { 0x35,/*length*/6, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x09, 0xF1,0x02  };
        public static readonly byte[] UInt32 = { 0x35,/*length*/8, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x0A, 0xF1,0x02,0x03,0xE8 };
        public static readonly byte[] UInt64 = { 0x35,/*length*/12, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x0B, 0xF1, 0x02, 0x03, 0xE8,1,2,3,4 };
        public static readonly byte[] UInt128 = { 0x35,/*length*/20, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x0C, 0xF1, 0x02, 0x03, 0xE8, 1, 2, 3, 4,5,6,7,8,9,10,11,12 };
        //
        // TwosComplementInteger = 2, -> 0x10
        public static readonly byte[] Int8 = { 0x35,/*length*/5, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x10, 0xF1 };
        public static readonly byte[] Int16 = { 0x35,/*length*/6, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x11, 0xF1,0x02 };
        public static readonly byte[] Int32 = { 0x35,/*length*/8, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x12, 0xF1,0x02,0x03,0xE8 };
        public static readonly byte[] Int64 = { 0x35,/*length*/12, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x13, 0xF1, 0x02, 0x03, 0xE8, 1, 2, 3, 4, };
        public static readonly byte[] Int128 = { 0x35,/*length*/20, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x14, 0xF1, 0x02, 0x03, 0xE8, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        //
        // Uuid = 3, -> 0x18
        public static readonly byte[] Uuid16 = { 0x35,/*length*/6, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x19, /*uuid*/0xF1, 0x02 };
        public static readonly byte[] Uuid32 = { 0x35,/*length*/8, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x1A, /*uuid*/0xF1, 0x02, 0x03, 0xE8 };
        public static readonly byte[] Uuid128 = { 0x35,/*length*/20, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x1C, 
            /*uuid*/0xF1, 0x02, 0x03, 0xE8,  /*-*/0x00,0x00,/*-*/0x10,0x00,/*-*/0x80,0x00,/*-*/0x00,0x80,0x5F,0x9B,0x34,0xFB,};
        //
        // TextString = 4, -> 0x20
        public static readonly byte[] StringX1 = { 0x35,/*length*/9, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x25, 4, (byte)'a', (byte)'b', (byte)'c', (byte)'d', };
        public static readonly byte[] StringX2 = { 0x35,/*length*/10, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x26, 0,4, (byte)'a', (byte)'b', (byte)'c', (byte)'d', };
        public static readonly byte[] StringX4 = { 0x35,/*length*/12, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x27, 0,0,0,4, (byte)'a', (byte)'b', (byte)'c', (byte)'d', };
        // Boolean = 5, -> 0x28
        public static readonly byte[] Boolean = { 0x35,/*length*/5, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x28, 0x01 };
        // DateElementSequence = 6, -> 0x30
        public static readonly byte[] SeqX1 = { 0x35,/*length*/6, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x35,1, /*value element - Nil*/ 0x00 };
        public static readonly byte[] SeqX2 = { 0x35,/*length*/7, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x36, 0, 1, /*value element - Nil*/ 0x00 };
        public static readonly byte[] SeqX4 = { 0x35,/*length*/9, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x37, 0,0,0,1, /*value element - Nil*/ 0x00 };
        // DateElementAlternative = 7, -> 0x38
        public static readonly byte[] AltX1 = { 0x35,/*length*/6, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x3D, 1, /*value element - Nil*/ 0x00 };
        public static readonly byte[] AltX2 = { 0x35,/*length*/7, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x3E, 0,1, /*value element - Nil*/ 0x00 };
        public static readonly byte[] AltX4 = { 0x35,/*length*/9, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x3F, 0,0,0,1, /*value element - Nil*/ 0x00 };
        // Url = 8 -> 0x40
        public static readonly byte[] UrlX1 = { 0x35,/*length*/12, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x45, 7, (byte)'s', (byte)':', (byte)'/', (byte)'/', (byte)'h', (byte)'/', (byte)'f', };
        public static readonly byte[] UrlX4 = { 0x35,/*length*/15, /*id*/0x09, 0xFF, 0xFF, /*value element*/ 0x47, 0, 0, 0, 7, (byte)'s', (byte)':', (byte)'/', (byte)'/', (byte)'h', (byte)'/', (byte)'f', };
    }//class

    //[TestFixture]
    public class ParseAllElementTypes_NoChecking
    {
        public void DoTest(byte[] bytes)
        {
            // Aim is simply to show an exception if not yet implemented so simply parse
            // and don't do any thorough checking of the result.
            ServiceRecord result = new ServiceRecordParser().Parse(bytes);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        //--------
        [Test]
        public void Nil() { DoTest(Data_AllElementTypes.Nil); }

        //--------
        [Test]
        public void UInt8() { DoTest(Data_AllElementTypes.UInt8); }

        [Test]
        public void UInt16() { DoTest(Data_AllElementTypes.UInt16); }
        
        [Test]
        public void UInt32() { DoTest(Data_AllElementTypes.UInt32); }

        [Test]
        public void UInt64() { DoTest(Data_AllElementTypes.UInt64); }

        [Test]
        public void UInt128() { DoTest(Data_AllElementTypes.UInt128); }

        //--------
        [Test]
        public void Int8() { DoTest(Data_AllElementTypes.Int8); }
        
        [Test]
        public void Int16() { DoTest(Data_AllElementTypes.Int16); }
        
        [Test]
        public void Int32() { DoTest(Data_AllElementTypes.Int32); }

        [Test]
        public void Int64() { DoTest(Data_AllElementTypes.Int64); }

        [Test]
        public void Int128() { DoTest(Data_AllElementTypes.Int128); }

        //--------

        [Test]
        public void Uuid16() { DoTest(Data_AllElementTypes.Uuid16); }
        
        [Test]
        public void Uuid32() { DoTest(Data_AllElementTypes.Uuid32); }
        
        [Test]
        public void Uuid128() { DoTest(Data_AllElementTypes.Uuid128); }
        
        //--------
        [Test]
        public void StringX1() { DoTest(Data_AllElementTypes.StringX1); }
        
        [Test]
        public void StringX2() { DoTest(Data_AllElementTypes.StringX2); }
        
        [Test]
        public void StringX4() { DoTest(Data_AllElementTypes.StringX4); }
        
        //--------
        [Test]
        public void Boolean() { DoTest(Data_AllElementTypes.Boolean); }

        //--------
        [Test]
        public void SeqX1() { DoTest(Data_AllElementTypes.SeqX1); }
        [Test]
        public void SeqX2() { DoTest(Data_AllElementTypes.SeqX2); }
        [Test]
        public void SeqX4() { DoTest(Data_AllElementTypes.SeqX4); }

        //--------
        [Test]
        public void AltX1() { DoTest(Data_AllElementTypes.AltX1); }
        [Test]
        public void AltX2() { DoTest(Data_AllElementTypes.AltX2); }
        [Test]
        public void AltX4() { DoTest(Data_AllElementTypes.AltX4); }
        
        //--------
        [Test]
        public void UrlX1() { DoTest(Data_AllElementTypes.UrlX1); }

        [Test]
        public void UrlX4() { DoTest(Data_AllElementTypes.UrlX4); }
    }//class

}

