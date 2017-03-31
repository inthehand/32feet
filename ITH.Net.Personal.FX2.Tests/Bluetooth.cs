using System;
using System.Text;
using NUnit.Framework;
//
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using System.Diagnostics;
using InTheHand.Net.Bluetooth.Factory;
using System.Threading;
using InTheHand.Net.Bluetooth.Msft;

namespace InTheHand.Net.Tests.Bluetooth
{
    
#if WinXP
    // This represents the native structure on XP and so is marshalled in interop.
    // There is no such struct in CE, this struct is just used for storing values
    // in managed code.  Its it not marshalled, so don't do any tests of that.

    [TestFixture]
    public class BLUETOOTH_DEVICE_INFO
    {
        object CreateBLUETOOTH_DEVICE_INFO(object argument)
        {
            Type typeBDI = typeof(BluetoothAddress).Assembly.GetType("InTheHand.Net.Bluetooth.BLUETOOTH_DEVICE_INFO");
            Type argType = argument.GetType();
            System.Reflection.ConstructorInfo ctor = typeBDI.GetConstructor(new Type[] { argType });
            object obj = ctor.Invoke(new object[] { argument });
            ValueType objS = (ValueType)obj;
            return obj;
        }

        const int BDI_Size = 560;

        [Test]
        public void BluetoothDeviceInfoStructSize()
        {
            long arg = 0x1122334455L;
            object obj = CreateBLUETOOTH_DEVICE_INFO(arg);
            Assert.AreEqual(BDI_Size, System.Runtime.InteropServices.Marshal.SizeOf(obj));
        }

        [Test]
        public void BluetoothDeviceInfoStructSize_Address()
        {
            BluetoothAddress arg = new BluetoothAddress(0x1122334455L);
            object obj = CreateBLUETOOTH_DEVICE_INFO(arg);
            Assert.AreEqual(BDI_Size, System.Runtime.InteropServices.Marshal.SizeOf(obj));
        }

        [Test]
        [ExpectedException(typeof(System.ArgumentNullException), "Value cannot be null." + "\r\n" + "Parameter name: address")]
        public void BluetoothDeviceInfoStructSize_AddressNull()
        {
            try {
                Type typeBDI = typeof(BluetoothAddress).Assembly.GetType("InTheHand.Net.Bluetooth.BLUETOOTH_DEVICE_INFO");
                System.Reflection.ConstructorInfo ctor = typeBDI.GetConstructor(new Type[] { typeof(BluetoothAddress) });
                object obj = ctor.Invoke(new object[] { null });
            } catch (System.Reflection.TargetInvocationException tiex) {
                // Convert the exception into to original real one thrown.
                throw tiex.InnerException;
            }
        }
    
    }//class
#endif


    namespace TestBluetoothEndPoint
    {
        [TestFixture]
        public class ToString
        {
            /*
             * I've done a bit of work on BluetoothEndPoint. My take on the ToString
             * is that it should try to follow existing examples where possible,
             * JSR-82 and similar use a URI of the form:-
             * bluetooth://xxxxxxxxxxxx:xx
             * or
             * bluetooth://xxxxxxxxxxxx:xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
             * or btspp:// in some serialport only situations
             * 
             * So my proposal (and the code I've checked in) is as the JSR-82 but
             * without the URI prefix. If the port is non default then we use that,
             * otherwise just the full guid. I didn't see the need to make it a uri,
             * but if anyone wanted to output it in the same way they just need to
             * append the prefix.
             */

            public const String GuidOppAsString = "0000110500001000800000805f9b34fb";
            public const String GuidSerialPortAsString = "0000110100001000800000805f9b34fb";
            public const String GuidEmptyAsString = "00000000000000000000000000000000";

            [Test]
            public void ObexPush()
            {
                BluetoothEndPoint ep = new BluetoothEndPoint(
                    new BluetoothAddress(TestBluetoothAddress.Ctor.SixLengthBytes),
                    BluetoothService.ObexObjectPush);
                String result = ep.ToString();
                const String expected = TestBluetoothAddress.Ctor.SixLengthString + ":"
                    + GuidOppAsString;
                Assert.AreEqual(expected, result);
            }

            [Test]
            public void SerialPort()
            {
                BluetoothEndPoint ep = new BluetoothEndPoint(
                    new BluetoothAddress(TestBluetoothAddress.Ctor.SixLengthBytes),
                    BluetoothService.SerialPort);
                String result = ep.ToString();
                const String expected = TestBluetoothAddress.Ctor.SixLengthString + ":"
                    + GuidSerialPortAsString;
                Assert.AreEqual(expected, result);
            }

            [Test]
            public void ObexPushWithPort()
            {
                BluetoothEndPoint ep = new BluetoothEndPoint(
                    new BluetoothAddress(TestBluetoothAddress.Ctor.SixLengthBytes),
                    BluetoothService.ObexObjectPush,
                    9);
                String result = ep.ToString();
                const String expected = TestBluetoothAddress.Ctor.SixLengthString + ":"
                    + "9";
                Assert.AreEqual(expected, result);
            }

            [Test]
            public void Empty()
            {
                BluetoothEndPoint ep = new BluetoothEndPoint(
                    new BluetoothAddress(TestBluetoothAddress.Ctor.SixLengthBytes),
                    BluetoothService.Empty);
                String result = ep.ToString();
                const String expected = TestBluetoothAddress.Ctor.SixLengthString + ":"
                    + GuidEmptyAsString;
                Assert.AreEqual(expected, result);
            }

            [Test]
            public void EmptyWithPort()
            {
                BluetoothEndPoint ep = new BluetoothEndPoint(
                    new BluetoothAddress(TestBluetoothAddress.Ctor.SixLengthBytes),
                    BluetoothService.Empty,
                    10);
                String result = ep.ToString();
                const String expected = TestBluetoothAddress.Ctor.SixLengthString + ":"
                    + "10";
                Assert.AreEqual(expected, result);
            }

            [Test]
            public void EmptyWithZeroPort()
            {
                BluetoothEndPoint ep = new BluetoothEndPoint(
                    new BluetoothAddress(TestBluetoothAddress.Ctor.SixLengthBytes),
                    BluetoothService.Empty,
                    0);
                String result = ep.ToString();
                const String expected = TestBluetoothAddress.Ctor.SixLengthString + ":"
                    + "0";
                Assert.AreEqual(expected, result);
            }

        }
    }


    namespace TestBluetoothAddress
    {
        [TestFixture]
        public class TryParse : ParseBase
        {
            protected override BluetoothAddress DoTestSuccess(string input)
            {
                BluetoothAddress result;
                bool success = BluetoothAddress.TryParse(input, out result);
                Assert.IsTrue(success, "success");
                Assert.IsNotNull(result, "result != null");
                return result;
            }

            protected override void DoTestFail(string input, Type typeofException, string message)
            {
                BluetoothAddress result;
                bool success = BluetoothAddress.TryParse(input, out result);
                Assert.IsFalse(success, "success");
                Assert.IsNull(result, "result == null");
            }
        }//class

        [TestFixture]
        public class Parse : ParseBase
        {
            protected override BluetoothAddress DoTestSuccess(string input)
            {
                return BluetoothAddress.Parse(input);
            }

            protected override void DoTestFail(string input, Type typeofException, string message)
            {
                BluetoothAddress result;
                try {
                    result = BluetoothAddress.Parse(input);
                    Assert.Fail("should have thrown!");
                } catch (Exception ex) {
                    Assert.IsInstanceOfType(typeofException, ex);
                    Assert.AreEqual(message, ex.Message);
                }
            }
        }//class

        public abstract class ParseBase
        {
            protected abstract BluetoothAddress DoTestSuccess(string input);
            protected abstract void DoTestFail(string input, Type typeofException, string message);


            [Test]
            //[ExpectedException(typeof(ArgumentNullException), "Value cannot be null." + Tests_Values.NewLine + "Parameter name: bluetoothString")]
            public void Null()
            {
                DoTestFail(null, typeof(ArgumentNullException), "Value cannot be null." + Tests_Values.NewLine + "Parameter name: bluetoothString");
            }

            [Test]
            //[ExpectedException(typeof(FormatException), "bluetoothString is not a valid Bluetooth address.")]
            public void EmptyString()
            {
                DoTestFail("", typeof(FormatException), "bluetoothString is not a valid Bluetooth address.");
            }

            [Test]
            public void SixBytes()
            {
                BluetoothAddress addr = DoTestSuccess(Ctor.SixLengthString);
                Assert.AreEqual(Ctor.SixLengthBytes, addr.ToByteArray());
                Assert.AreEqual(Ctor.SixLengthLong, addr.ToInt64());
                Assert.AreEqual(Ctor.SixLengthString, addr.ToString());
            }

            [Test]
            public void SixBytesDots()
            {
                BluetoothAddress addr = DoTestSuccess(Ctor.SixLengthDotsString);
                Assert.AreEqual(Ctor.SixLengthBytes, addr.ToByteArray());
                Assert.AreEqual(Ctor.SixLengthLong, addr.ToInt64());
                Assert.AreEqual(Ctor.SixLengthString, addr.ToString());
            }

            [Test]
            public void SixBytesColons()
            {
                BluetoothAddress addr = DoTestSuccess(Ctor.SixLengthColonsString);
                Assert.AreEqual(Ctor.SixLengthBytes, addr.ToByteArray());
                Assert.AreEqual(Ctor.SixLengthLong, addr.ToInt64());
                Assert.AreEqual(Ctor.SixLengthString, addr.ToString());
            }

            [Test]
            //[ExpectedException(typeof(FormatException), "bluetoothString is not a valid Bluetooth address.")]
            public void SixBytesDotsShort()
            {
                DoTestFail(Ctor.SixLengthDotsString.Substring(1),
                    typeof(FormatException), "bluetoothString is not a valid Bluetooth address.");
            }

            [Test]
            //[ExpectedException(typeof(FormatException), "bluetoothString is not a valid Bluetooth address.")]
            public void SixBytesColonsShort()
            {
                DoTestFail(Ctor.SixLengthColonsString.Substring(1),
                    typeof(FormatException), "bluetoothString is not a valid Bluetooth address.");
            }

            [Test]
            [Ignore("What's expected for eight byte values?")]
            public void EightBytes()
            {
                BluetoothAddress addr = DoTestSuccess(Ctor.EightLengthString);
                Assert.AreEqual(Ctor.EightLengthBytes, addr.ToByteArray());
                Assert.AreEqual(Ctor.EightLengthLong, addr.ToInt64());
                Assert.AreEqual(Ctor.EightLengthString, addr.ToString());
            }

            [Test]
            //[ExpectedException(typeof(FormatException), "bluetoothString is not a valid Bluetooth address.")]
            public void Zero ()
            {
                DoTestFail("0",
                    typeof(FormatException), "bluetoothString is not a valid Bluetooth address.");
            }

            const string FormatExMessage = "Input string was not in a correct format.";

            [Test]
            public void FormatExInColons()
            {
                DoTestFail("01:E2:zz:04:05:F6",
                    typeof(FormatException), FormatExMessage);
            }

            [Test]
            public void FormatExInDots()
            {
                DoTestFail("01.E2.zz.04.05.F6",
                    typeof(FormatException), FormatExMessage);
            }

            [Test]
            public void FormatExInPlain()
            {
                DoTestFail("01E2zz0405F6",
                    typeof(FormatException), FormatExMessage);
            }

        }//class


        [TestFixture]
        public class Ctor
        {
            const string ArgExParamNamePrefix = "\r\nParameter name: ";
            public static readonly byte[] EightLengthBytes = new byte[] { 0xF8, 7, 6, 5, 4,  3, 0xE2, 1 };
            public const String EightLengthString = "01E20304050607F8";
            public const Int64 EightLengthLong = 0x01E20304050607F8;
            //
            public static readonly byte[] SixLengthBytes = new byte[] { 0xF6, 5, 4, 3, 0xE2, 1, 0, 0 };
            public const String SixLengthString = "01E2030405F6";
            public const String SixLengthColonsString = "01:E2:03:04:05:F6";
            public const String SixLengthDotsString = "01.E2.03.04.05.F6";
            public const Int64 SixLengthLong = 0x000001E2030405F6;
            //
            public static readonly byte[] ZeroBytes = new byte[] { 0,0,0,0,0,0,0,0, };
            public const String ZeroString = "000000000000";

            [Test]
            [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "Value cannot be null."
               + Tests_Values.NewLine + "Parameter name: " + "address")]
            public void Null()
            {
                new BluetoothAddress(null);
            }

            [Test]
            [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Address must be six bytes long." + ArgExParamNamePrefix + "address")]
            public void ZeroBytes_()
            {
                new BluetoothAddress(new byte[0]);
            }

            [Test]
            public void SixBytes()
            {
                BluetoothAddress addr = new BluetoothAddress(SixLengthBytes);
                Assert.AreEqual(SixLengthBytes, addr.ToByteArray());
                Assert.AreEqual(SixLengthLong, addr.ToInt64());
                Assert.AreEqual(SixLengthString, addr.ToString());
            }

            [Test]
            [Ignore("What's expected for eight byte values?")]
            public void EightBytes()
            {
                BluetoothAddress addr = new BluetoothAddress(EightLengthBytes);
                Console.WriteLine("Ctor.EightBytes result: " + BitConverter.ToString(addr.ToByteArray()));
                Assert.AreEqual(EightLengthBytes, addr.ToByteArray());
                Assert.AreEqual(EightLengthLong, addr.ToInt64());
                Assert.AreEqual(EightLengthString, addr.ToString());
            }

            [Test]
            [Ignore("What's expected for eight byte values?")]
            public void EightBytesAsLong()
            {
                BluetoothAddress addr = new BluetoothAddress(EightLengthLong);
                Assert.AreEqual(EightLengthBytes, addr.ToByteArray());
                Assert.AreEqual(EightLengthLong, addr.ToInt64());
                Assert.AreEqual(EightLengthString, addr.ToString());
            }

            [Test]
            public void ZeroLong()
            {
                BluetoothAddress addr = new BluetoothAddress(0);
                Assert.AreEqual(ZeroBytes, addr.ToByteArray());
                Assert.AreEqual(0, addr.ToInt64());
                Assert.AreEqual("000000000000", addr.ToString());
            }

        }//class

    }


    namespace TestBluetoothEndPoint
    {
        //[TestFixture]
        //public class SocketAddress_MacroLevel
        //{
        //    public readonly BluetoothEndPoint ExpectedEndPointLocalObexPush
        //        = new BluetoothEndPoint(BluetoothRadio.PrimaryRadio.LocalAddress, BluetoothService.ObexObjectPush);
        //
        //    [Test]
        //    public void aaaa()
        //    {
        //        BluetoothListener lstnr = new BluetoothListener(BluetoothService.ObexObjectPush);
        //        lstnr.Start();
        //        BluetoothEndPoint ep = (BluetoothEndPoint)lstnr.Server.LocalEndPoint;
        //        Assert.AreEqual(ExpectedEndPointLocalObexPush, ep);
        //    }
        //}//class


        [TestFixture]
        public class SerializeToFromSocketAddress
        {
            //  Length=[2+8+16+4]=30, and the field offsets are 
            //      Addr=2, Service=[2+8]=10, Port=[2+8+16]=26.

            //-------------------------
            public readonly BluetoothEndPoint EndPointSvcOppNoPort
                = new BluetoothEndPoint(BluetoothAddress.None, BluetoothService.ObexObjectPush);
            public static readonly byte[] SockAddrBytesSvcOppNoPort = {
                    /* PF */
                    0x20,0,
                    /* Addr */
                    0,0,0,0,0,0,0,0,
                    /* Service */
                    0x05, 0x11, 0x00, 0x00, /**/0x00, 0x00, /**/0x00, 0x10, 
                    /**/0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB,
                    /* Port */
                    255,255,255,255
                };
            // Should port really accept a full 32-bit value?  Its a single byte in SDP.
            public const Int32 PortSvcNapWithPort = unchecked((int)0xFD010203);
            public readonly BluetoothEndPoint EndPointSvcNapWithPort
                = new BluetoothEndPoint(BluetoothAddress.None, BluetoothService.Nap, PortSvcNapWithPort);
            public static readonly byte[] SockAddrBytesSvcNapWithPort = {
                    /* PF */
                    0x20,0,
                    /* Addr */
                    0,0,0,0,0,0,0,0,
                    /* Service */
                    0x16, 0x11, 0x00, 0x00, /**/0x00, 0x00, /**/0x00, 0x10, 
                    /**/0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB,
                    /* Port */
                    3,2,1,0xFD
                };

            /// <summary>
            /// Show that UUIDs/Guids are not ordered in (little-endian Windows)
            /// as they appear when written down.
            /// </summary>
            /// <remarks>
            /// <code>
            /// typedef struct _GUID {
            ///   DWORD Data1;
            ///   WORD Data2;
            ///   WORD Data3;
            ///   BYTE Data4[8];
            /// } GUID;
            /// </code>
            /// So the first three segments are bit swapped.
            /// </remarks>
            [Test]
            public void CheckGuidOrdering()
            {
                byte[] result = BluetoothService.ObexObjectPush.ToByteArray();
                byte[] expected = {
                    0x05, 0x11, 0x00, 0x00, /**/0x00, 0x00, /**/0x00, 0x10, 
                    /**/0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB };
                Assert.AreEqual(expected, result);
            }


            [Test]
            public void Serialize_NoPort()
            {
#if NETCF
                Assert.Ignore("Test not implemented for CF");
#endif
                System.Net.SocketAddress sa = EndPointSvcOppNoPort.Serialize();
                //Console.WriteLine("bbbb:" + sa.ToString());
                InTheHand.Net.Tests.Irda.Net.TestIrDAEndPoint.SerializeToFromSocketAddress
                    .AssertAreEqualSocketAddressBuffer(SockAddrBytesSvcOppNoPort, sa);
            }

            [Test]
            public void Serialize_PortSet()
            {
#if NETCF
                Assert.Ignore("Test not implemented for CF");
#endif
                System.Net.SocketAddress sa = EndPointSvcNapWithPort.Serialize();
                //Console.WriteLine("bbbb:" + sa.ToString());
                InTheHand.Net.Tests.Irda.Net.TestIrDAEndPoint.SerializeToFromSocketAddress
                    .AssertAreEqualSocketAddressBuffer(SockAddrBytesSvcNapWithPort, sa);
            }

            //----------------------------------------------------------

            public static BluetoothEndPoint Create(byte[] sockAddrBytes)
            {
                var addr = BluetoothAddress.Parse("002233f3a5f9");
                BluetoothEndPoint epFactory = new BluetoothEndPoint(addr, Guid.Empty);
                Assert.AreEqual(addr, epFactory.Address);
                Assert.AreEqual(Guid.Empty, epFactory.Service);
                Assert.AreEqual(-1, epFactory.Port);
                //----
                // The real work
                System.Net.SocketAddress saSrc = InTheHand.Net.Tests.Irda.Net.TestIrDAEndPoint
                    .SerializeToFromSocketAddress.Factory_SocketAddress(
                        (System.Net.Sockets.AddressFamily)0x20, sockAddrBytes);
                BluetoothEndPoint result = (BluetoothEndPoint)epFactory.Create(saSrc);
                //----
                // Just check the factory BluetoothEndPoint is unaltered
                Assert.AreEqual(addr, epFactory.Address);
                Assert.AreEqual(Guid.Empty, epFactory.Service);
                Assert.AreEqual(-1, epFactory.Port);
                //----
                return result;
            }

            [Test]
            public void Create_NoPort()
            {
#if NETCF
                Assert.Ignore("Test not implemented for CF");
#endif
                BluetoothEndPoint result = Create(SockAddrBytesSvcOppNoPort);
                Assert.AreEqual(EndPointSvcOppNoPort, result);
                Assert.AreEqual(-1, result.Port);
            }

            [Test]
            public void Create_WithPort()
            {
#if NETCF
                Assert.Ignore("Test not implemented for CF");
#endif
                BluetoothEndPoint result = Create(SockAddrBytesSvcNapWithPort);
                Assert.AreEqual(EndPointSvcNapWithPort, result);
                Assert.AreEqual(PortSvcNapWithPort, result.Port);
            }


        }//class
        
    }//namespace


    [TestFixture]
    public class ListenerSdpRecordAuto
    {
        [TestFixtureSetUp]
        public virtual void Init()
        {
            ListenerMisc.SetMsftFactory();
        }

        // Unfortunately there's no way to use SdpRecordTemp without using 
        // BluetoothListener and causing it to start listening!  What will happen
        // the testing PC has no Bluetooth hardware etc.  We do try and choose UUIDs
        // however that won't conflict with any already in use.

        void DoTest(byte[] expectedSdpRecord, int portIndex, Guid guid)
        {
            Assert.AreEqual(0, expectedSdpRecord[portIndex], "Expect RFCOMM SCN byte to be uninitialised.");
            InTheHand.Net.Sockets.BluetoothListener lstnr = null;
            try {
                lstnr = new InTheHand.Net.Sockets.BluetoothListener(guid);
                Assert.IsNotNull(lstnr.ServiceRecord);
                Assert.AreEqual(expectedSdpRecord, lstnr.ServiceRecord.ToByteArray());
                Assert.IsNull(lstnr.ServiceRecord.SourceBytes);
                lstnr.Start();
                int port = lstnr.LocalEndPoint.Port;
                Assert.AreNotEqual(0, port);
                expectedSdpRecord[portIndex] = checked((byte)port);
                Assert.AreEqual(expectedSdpRecord, lstnr.ServiceRecord.ToByteArray());
                Assert.IsNull(lstnr.ServiceRecord.SourceBytes);
            } finally {
                if (lstnr != null) {
                    lstnr.Stop();
                }
            }
        }

        //--------------------------------------------------------------
        public static readonly byte[] GuidBytesOne = {
            0x00,0x11,0x22,0x33,0x44,0x55,0x66,0x77,
            0x88,0x99,0x02,0x11,0x11,0x82,0xFF,0xB6,
            };
        // Result = "33221100-5544-7766-8899-02111182FFB6"
        public static readonly Guid GuidOne = new Guid(GuidBytesOne);

        public /*static*/ readonly byte[] RecordOne = {
            0x35,0x27,0x09,0x00,0x01,0x35,0x11,0x1c,
            0x33,0x22,0x11,0x00, 0x55,0x44, 0x77,0x66,
            0x88,0x99,0x02,0x11,0x11,0x82,0xFF,0xB6,
            0x09,0x00,0x04,0x35,0x0c,0x35,0x03,0x19,
            0x01,0x00,0x35,0x05,0x19,0x00,0x03,0x08,
            0x00};

        public static readonly Guid GuidTwo = Guid.Empty;

        public /*static*/ readonly byte[] RecordTwo = {
            0x35,0x27,0x09,0x00,0x01,0x35,0x11,0x1c,
            0x00,0x00,0x00,0x00, 0x00,0x00, 0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x09,0x00,0x04,0x35,0x0c,0x35,0x03,0x19,
            0x01,0x00,0x35,0x05,0x19,0x00,0x03,0x08,
            0x00};

        // 0x1105 <= 00001105-0000-1000-8000-00805f9b34fb
        public /*static*/ readonly byte[] RecordObexPush = {
            0x35,0x19,0x09,0x00,0x01,0x35,0x03,0x19,
            0x11,0x05,0x09,0x00,0x04,0x35,0x0c,0x35,
            0x03,0x19,0x01,0x00,0x35,0x05,0x19,0x00,
            0x03,0x08,0x00};

        //--------------------------------------------------------------
        [Test]
        public void AutoOne()
        {
            DoTest(RecordOne, 40, GuidOne);
        }

        [Test]
        public void AutoTwo()
        {
            DoTest(RecordTwo, 40, GuidTwo);
        }

        [Test]
        public void AutoObexPush()
        {
            DoTest(RecordObexPush, RecordObexPush.Length - 1, InTheHand.Net.Bluetooth.BluetoothService.ObexObjectPush);
        }

        //--------------------------------------------------------------
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Address_NullAddress()
        {
            new BluetoothListener((BluetoothAddress)null, BluetoothService.ObexObjectPush);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Address_NullUuid()
        {
            new BluetoothListener(BluetoothAddress.None, Guid.Empty);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EndPoint_NullEndPoint()
        {
            new BluetoothListener(null);
        }

    }//class


    [TestFixture]
    public class ListenerSdpRecordByteArrayGiven
    {
        [TestFixtureSetUp]
        public virtual void Init()
        {
            ListenerMisc.SetMsftFactory();
        }

        void DoTest(Guid guid, byte[] sdpRecordGiven, int channelOffset)
        {
            byte[] sdpRecord = new byte[sdpRecordGiven.Length];
            sdpRecordGiven.CopyTo(sdpRecord, 0);
            //
            Assert.AreEqual(0, sdpRecord[channelOffset]);
            InTheHand.Net.Sockets.BluetoothListener lstnr = null;
            try {
                lstnr = new InTheHand.Net.Sockets.BluetoothListener(guid, sdpRecord, channelOffset);
                Assert.AreEqual(sdpRecord, lstnr.ServiceRecord.ToByteArray());
                Assert.AreEqual(sdpRecordGiven, lstnr.ServiceRecord.SourceBytes);
                lstnr.Start();
                int port = lstnr.LocalEndPoint.Port;
                Assert.AreNotEqual(0, port);
                sdpRecord[channelOffset] = checked((byte)port);
                Assert.AreEqual(sdpRecord, lstnr.ServiceRecord.ToByteArray());
                Assert.AreEqual(sdpRecordGiven, lstnr.ServiceRecord.SourceBytes);
            } finally {
                if (lstnr != null) {
                    lstnr.Stop();
                }
            }
        }

        void DoTestCreateFails(Guid guid, byte[] sdpRecordGiven, int channelOffset)
        {
            byte[] sdpRecord = new byte[sdpRecordGiven.Length];
            sdpRecordGiven.CopyTo(sdpRecord, 0);
            //
            BluetoothListener lstnr 
                = new InTheHand.Net.Sockets.BluetoothListener(guid, sdpRecord, channelOffset);
            Assert.Fail("DoTestCreateFails didn't fail!");
        }

        //--------------------------------------------------------------
        const String NewLine = "\r\n";

        private static readonly byte[] ObexListener_ServiceRecord = new byte[] {
		    0x35,0x25,0x09,0x00,0x01,0x35,0x03,0x19,
		    0x11,0x05,0x09,0x00,0x04,0x35,0x11,0x35,
		    0x03,0x19,0x01,0x00,0x35,0x05,0x19,0x00,
		    0x03,0x08,0x00,0x35,0x03,0x19,0x00,0x08,
		    0x09,0x03,0x03,0x35,0x02,0x08,0xFF};
        private const int ObexListener_ServiceRecordChannelOffset = 26;

        //--------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "sdpRecord must not be empty.")]
        public void ArgEx_EmptyRecord()
        {
            DoTestCreateFails(BluetoothService.ObexObjectPush, new byte[0], 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException),
            ExpectedMessage = "Specified argument was out of the range of valid values." + NewLine
            + "Parameter name: channelOffset")]
        public void ArgEx_OffsetOffEnd100()
        {
            DoTestCreateFails(BluetoothService.ObexObjectPush, ObexListener_ServiceRecord, 100);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException), ExpectedMessage = "Specified argument was out of the range of valid values." + NewLine
            + "Parameter name: channelOffset")]
        public void ArgEx_OffsetOffEnd1()
        {
            DoTestCreateFails(BluetoothService.ObexObjectPush, ObexListener_ServiceRecord, ObexListener_ServiceRecord.Length);
        }

        //--------------------------------------------------------------

        [Test]
        public void ObexListener()
        {
            DoTest(BluetoothService.ObexObjectPush, ObexListener_ServiceRecord, ObexListener_ServiceRecordChannelOffset);
        }

        [Test]
        [ExpectedException(typeof(System.Net.ProtocolViolationException), ExpectedMessage = "Element overruns buffer section, from index 0.")]
        // With no record pre-parsing this fails in NativeMethods.WSASetService 
        // with error 10022 which we throw as: SocketException : An invalid argument was supplied
        public void ObexListenerBadTruncated()
        {
            // Truncate the record.
            byte[] record = new byte[ObexListener_ServiceRecord.Length - 10];
            Array.Copy(ObexListener_ServiceRecord, 0, record, 0, record.Length);
            const int dummyOffset = 5;
            //DoTest(BluetoothService.ObexObjectPush, record, dummyOffset);
            BluetoothListener lstnr = new InTheHand.Net.Sockets.BluetoothListener(BluetoothService.ObexObjectPush,
                                        record, dummyOffset);
            lstnr.Start();
        }

    }//class


    [TestFixture]
    public class ListenerSdpRecordServiceRecordGiven
    {
        [TestFixtureSetUp]
        public virtual void Init()
        {
            ListenerMisc.SetMsftFactory();
        }

        //----------------
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BadNull()
        {
            new BluetoothListener(BluetoothService.GenericAudio, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void BadEmpty()
        {
            new BluetoothListener(BluetoothService.GenericAudio, new ServiceRecord());
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void BadOnlyClassIdList()
        {
            Guid uuid = BluetoothService.GenericAudio;
            ServiceRecord rcd = new ServiceRecord(
                new ServiceAttribute(InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ServiceClassIdList,
                    new ServiceElement(ElementType.ElementSequence,
                        new ServiceElement(ElementType.Uuid128, uuid))));
            new BluetoothListener(uuid, rcd);
        }

        //----------------
        [Test]
        public void GoodOne()
        {
            Guid uuid = BluetoothService.GenericAudio;
            ServiceRecord rcd = new ServiceRecord(
                new ServiceAttribute(InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ServiceClassIdList,
                    new ServiceElement(ElementType.ElementSequence,
                        new ServiceElement(ElementType.Uuid128, uuid))),
                new ServiceAttribute(InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ProtocolDescriptorList,
                    ServiceRecordHelper.CreateRfcommProtocolDescriptorList())
                );
            BluetoothListener lsnr = new BluetoothListener(uuid, rcd);
            lsnr.Start();
            lsnr.Stop();
        }

        [Test]
        public void GoodOneDifferentUuids_Address()
        {
            Guid uuid1 = BluetoothService.GenericAudio;
            Guid uuid2 = BluetoothService.HardcopyCableReplacement;
            ServiceRecord rcd = new ServiceRecord(
                new ServiceAttribute(InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ServiceClassIdList,
                    new ServiceElement(ElementType.ElementSequence,
                        new ServiceElement(ElementType.Uuid128, uuid1))),
                new ServiceAttribute(InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ProtocolDescriptorList,
                    ServiceRecordHelper.CreateRfcommProtocolDescriptorList())
                );
            BluetoothListener lsnr = new BluetoothListener(BluetoothAddress.None, uuid2, rcd);
            lsnr.Start();
            lsnr.Stop();
        }

        [Test]
        public void GoodOneDifferentUuids_EndPoint()
        {
            Guid uuid1 = BluetoothService.GenericAudio;
            Guid uuid2 = BluetoothService.HardcopyCableReplacement;
            ServiceRecord rcd = new ServiceRecord(
                new ServiceAttribute(InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ServiceClassIdList,
                    new ServiceElement(ElementType.ElementSequence,
                        new ServiceElement(ElementType.Uuid128, uuid1))),
                new ServiceAttribute(InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ProtocolDescriptorList,
                    ServiceRecordHelper.CreateRfcommProtocolDescriptorList())
                );
            BluetoothListener lsnr = new BluetoothListener(new BluetoothEndPoint(BluetoothAddress.None, uuid2), rcd);
            lsnr.Start();
            lsnr.Stop();
        }

    }//class

    [TestFixture]
    public class ListenerMisc
    {
        static readonly Guid DummySvcClass = new Guid("{D85716BF-A5CF-40f8-8D0F-CE59E7AC30ED}");

        [TestFixtureSetUp]
        public virtual void Init()
        {
            SetMsftFactory();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Not listening. You must call the Start() method before calling this method.")]
        public void NotActive()
        {
            BluetoothListener lsnr = new BluetoothListener(DummySvcClass);
            lsnr.AcceptBluetoothClient();
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException), ExpectedMessage = "Specified argument was out of the range of valid values.\r\nParameter name: "
            + "backlog")]
        public void NegativeBacklog()
        {
            BluetoothListener lsnr = new BluetoothListener(DummySvcClass);
            lsnr.Start(-10);
        }

        [Test]
        public void StartStopStart()
        {
            BluetoothListener lsnr = new BluetoothListener(DummySvcClass);
            DoTestStartStop(lsnr);
            DoTestStartStop(lsnr);
        }

        private void DoTestStartStop(BluetoothListener lsnr)
        {
            Action<BluetoothListener> dlgt = Foo;
            ThreadStart dlgt0 = delegate { dlgt(lsnr); };
            //
            lsnr.Start();
            IAsyncResult ar0 = Delegate2.BeginInvoke(dlgt0, null, null);
            Assert.IsFalse(ar0.IsCompleted);
            //
            lsnr.Stop();
            System.Threading.Thread.Sleep(50);
            Assert.IsTrue(ar0.IsCompleted);
            try {
                Delegate2.EndInvoke(dlgt0, ar0);
            } catch (System.Reflection.TargetInvocationException tex){
                Assert.IsInstanceOfType(typeof(System.Net.Sockets.SocketException), tex.InnerException, "InnerException");
            }
        }

        void Foo(BluetoothListener lsnr)
        {
            BluetoothClient conn = lsnr.AcceptBluetoothClient();
        }

        //--
        internal static void SetMsftFactory()
        {
#if !FX1_1
            //BluetoothFactory f0 = BluetoothFactory.Factory;
            BluetoothFactory.SetFactory(new SocketsBluetoothFactory());
#endif
        }
    }//class


    [TestFixture]
    public class Misc
    {
        [Test]
        public void ServiceClassIdDefinitions()
        {
            byte[] ExpectedBaseUuidPart = { //0x00, 0x00, 0x00, 0x00, 
                /*-*/ 0x00, 0x00, /*-*/ 0x00, 0x10, 
                /*-*/ 0x80, /*-*/ 0x00,
                /*-*/ 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB };
            System.Reflection.FieldInfo[] members = typeof(BluetoothService).GetFields(
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (System.Reflection.FieldInfo curFieldInfo in members) {
                if (curFieldInfo.Name == "Empty") { continue; }
                //
                Guid value = (Guid)curFieldInfo.GetValue(null);
                byte[] asArray = value.ToByteArray();
                Assert.AreEqual(16, asArray.Length, "Infra failure, Guid_array should be length 16.");
                byte[] basePart = new byte[16 - 4];
                Array.Copy(asArray, 4, basePart, 0, 16 - 4);
                Assert.AreEqual(ExpectedBaseUuidPart, basePart, "UUID not Bluetooth-based: " + curFieldInfo.Name);
            }//for
        }

#if !NO_WINFORMS
        [Test]
        [Explicit]
        public void SelectBluetoothDeviceDialog()
        {
            InTheHand.Windows.Forms.SelectBluetoothDeviceDialog dlg
                = new InTheHand.Windows.Forms.SelectBluetoothDeviceDialog();
            SelectBluetoothDeviceDialogTest(dlg);
        }

#if !FX1_1
        [Test]
        [Explicit]
        public void SelectBluetoothDeviceDialogForceCustom()
        {
            InTheHand.Windows.Forms.SelectBluetoothDeviceDialog dlg
                = new InTheHand.Windows.Forms.SelectBluetoothDeviceDialog(true);
            SelectBluetoothDeviceDialogTest(dlg);
        }
#endif

        public void SelectBluetoothDeviceDialogTest(InTheHand.Windows.Forms.SelectBluetoothDeviceDialog dlg)
        {
            bool x;
            dlg.ShowUnknown = x = dlg.ShowUnknown;
            dlg.ShowRemembered = dlg.ShowRemembered;
            dlg.ShowAuthenticated = dlg.ShowAuthenticated;
            dlg.ForceAuthentication = dlg.ForceAuthentication;
            dlg.Reset();
            BluetoothDeviceInfo bdi = dlg.SelectedDevice;
#if ! PocketPC
            string t = dlg.Info;
            x = dlg.SkipServicesPage;
            x = dlg.AddNewDeviceWizard;
#endif
        }
#endif

    }//class
}
