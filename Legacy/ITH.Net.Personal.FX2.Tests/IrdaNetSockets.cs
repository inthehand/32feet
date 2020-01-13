using System;
using System.Text;
using NUnit.Framework;
using InTheHand.Net;
using InTheHand.Net.Sockets;



namespace InTheHand.Net.Tests.Irda.NetSockets
{

    [TestFixture]
    public class TestIrDAClient
    {
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException), ExpectedMessage = "No matching device discovered.")]
        public void CheckExceptionThrownByGetRemoteMachineName()
        {
            // Checking how the 'specified device not found' exception should be
            // created in GetRemoteMachineName.  For some odd reason the MSFT NETCF
            // developer responsible for the initial implementation chose to throw
            // ArgumentOutOfRangeException.  Note that its first parameter is 
            // "paramName", so careful not to create a message that blames a
            // specific parameter name.

            // 1.
            // The following creates message:
            //   Specified argument was out of the range of valid values.
            //   Parameter name: No matching device discovered
            //throw new ArgumentOutOfRangeException("No matching device discovered");

            // 2.
            // Good. :-)
            throw Tests_Values.new_ArgumentOutOfRangeException(null, "No matching device discovered.");
        }


        [Test]
        [ExpectedException(typeof(System.ArgumentNullException), ExpectedMessage = "Value cannot be null." + Tests_Values.NewLine + "Parameter name: irdaSocket")]
        public void DiscoverNullSocket()
        {
            IrDAClient.DiscoverDevices(4, null);
        }

        [Test,Category("Need IrDA device/stack")]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException), ExpectedMessage = "Specified argument was out of the range of valid values." + Tests_Values.NewLine + "Parameter name: maxDevices")]
        public void DiscoverHugeNumberOfItems()
        {
            System.Net.Sockets.Socket dummySock = new System.Net.Sockets.Socket(
                System.Net.Sockets.AddressFamily.Irda,
                System.Net.Sockets.SocketType.Stream,
                System.Net.Sockets.ProtocolType.Unspecified);
            IrDAClient.DiscoverDevices(Int32.MaxValue, dummySock);
        }

    }//class

    [TestFixture]
    public class DeviceInfoParse
    {
        public static IrDADeviceInfo[] DoParseDeviceList(byte[] buffer)
        {
            return IrDAClient.ParseDeviceList(buffer);
        }

        //--------------------------------------------------------------
        // Invalid input buffer
        //--------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(System.ArgumentNullException), ExpectedMessage = "Value cannot be null." + Tests_Values.NewLine + "Parameter name: buffer")]
        public void BufferNull()
        {
            DoParseDeviceList(null);
        }

        [Test]
        [ExpectedException(typeof(System.ArgumentException), ExpectedMessage = "DEVICE_LIST buffer must be at least four bytes long.")]
        public void BufferZeroLength()
        {
            DoParseDeviceList(new byte[0]);
        }

        [Test]
        [ExpectedException(typeof(System.ArgumentException), ExpectedMessage = "DEVICE_LIST buffer must be at least four bytes long.")]
        public void BufferThreeLength()
        {
            DoParseDeviceList(new byte[3]);
        }

        [Test]
        public void BufferZeroItemsMimimumLength()
        {
            byte[] bufferOfZeroItems = new byte[4];
            DoParseDeviceList(bufferOfZeroItems);
        }

        [Test]
        //Currently get:
        [ExpectedException(
#if NETCF
            typeof(System.ArgumentOutOfRangeException)
#else
            typeof(System.ArgumentException), ExpectedMessage = "Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection."
#endif
            )]
        public void BufferMimimumLengthButOneItems()
        {
            byte[] bufferOfZeroItems = new byte[4] { 1, 0, 0, 0 };
            DoParseDeviceList(bufferOfZeroItems);
        }

        //--------------------------------------------------------------
        //--------------------------------------------------------------
        readonly IrDAAddress AddressA = new IrDAAddress(new byte[] { 0xFF, 0x01, 0x00, 0x54, });

        //--------------------------------------------------------------
        [Test]
        public void oneAscii()
        {
            byte[] data = {1,0,0,0,
                //id
                0xFF,0x01,0x00,0x54,
                //name
                (byte)'n', (byte)'n', (byte)'n', (byte)'n', (byte)'n', 
                (byte)'n', (byte)'n', (byte)'n', (byte)'n', (byte)'n',  
                (byte)'n', (byte)'n', (byte)'n', (byte)'n', (byte)'n',  
                (byte)'n', (byte)'n', (byte)'n', (byte)'n', (byte)'n',  
                (byte)'n', (byte)'n', 
                //hints
                0x01, 0x51,
                //charset
                0 //ascii
            };//data
            IrDADeviceInfo[] diArray = DoParseDeviceList(data);
            Assert.AreEqual(1, diArray.Length);
            //
            Assert.AreEqual(AddressA, diArray[0].DeviceAddress);
            string name = new String('n', 22);// String.Empty.PadLeft(22, 'n');
            Assert.AreEqual(name, diArray[0].DeviceName);
            Assert.AreEqual(0x5101, (int)diArray[0].Hints);
            Assert.AreEqual(0, (int)diArray[0].CharacterSet);
        }//fn

        [Test]
        public void oneUnicode()
        {
            byte[] data = {1,0,0,0,
                //id
                0xFF,0x01,0x00,0x54,
                //name
                (byte)'n', 0, (byte)'n', 0, (byte)'n', 
                0, (byte)'n', 0, (byte)'n', 0,  
                (byte)'n', 0, (byte)'n', 0, (byte)'n',  
                0, (byte)'n', 0, (byte)'n', 0,  
                (byte)'n', 0, 
                //hints
                0x01, 0x51,
                //charset
                0xFF //unicode
            };//data
            IrDADeviceInfo[] diArray = DoParseDeviceList(data);
            Assert.AreEqual(1, diArray.Length);
            //
            Assert.AreEqual(AddressA, diArray[0].DeviceAddress);
            string name = new String('n', 11);// String.Empty.PadLeft(22, 'n');
            Assert.AreEqual(name, diArray[0].DeviceName);
            Assert.AreEqual(0x5101, (int)diArray[0].Hints);
            Assert.AreEqual(0xFF, (int)diArray[0].CharacterSet);
        }//fn

#if NETCF
        [Test]
        public void oneUnicode_CharsetUnsetAsOnCE()
        {
            byte[] data = {1,0,0,0,
                //id
                0xFF,0x01,0x00,0x54,
                //name
                (byte)'n', 0, (byte)'n', 0, (byte)'n', 
                0, (byte)'n', 0, (byte)'n', 0,  
                (byte)'n', 0, (byte)'n', 0, (byte)'n',  
                0, (byte)'n', 0, (byte)'n', 0,  
                (byte)'n', 0, 
                //hints
                0x01, 0x51,
                //charset
                0 //unset should be unicode, but is thus ascii
            };//data
            IrDADeviceInfo[] diArray = DoParseDeviceList(data);
            Assert.AreEqual(1, diArray.Length);
            //
            Assert.AreEqual(AddressA, diArray[0].DeviceAddress);
            string name = new String('n', 11);// String.Empty.PadLeft(22, 'n');
            Assert.AreEqual(name, diArray[0].DeviceName);
            //Assert.AreEqual(0x5101, (int)diArray[0].Hints);
            Assert.AreEqual(0, (int)diArray[0].CharacterSet);
        }
#endif

        [Test]
        public void twoUnicodeAbove255Codeunits()
        {
            byte[] data = {1,0,0,0,
                //id
                0xFF,0x01,0x00,0x54,
                //name
                0x00, 0x01,  0x01, 0x01,  0x02, 0x01,
                0, 0, 0, 0, 
                0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 
                0, 0, 
                //hints
                0x01, 0x51,
                //charset
                0xFF //unicode
            };//data
            IrDADeviceInfo[] diArray = DoParseDeviceList(data);
            Assert.AreEqual(1, diArray.Length);
            //
            Assert.AreEqual(AddressA, diArray[0].DeviceAddress);
            string name = "\u0100\u0101\u0102";
            Assert.AreEqual(name, diArray[0].DeviceName);
            Assert.AreEqual(0x5101, (int)diArray[0].Hints);
            Assert.AreEqual(0xFF, (int)diArray[0].CharacterSet);
        }//fn

#if NETCF
        [Test]
        public void twoUnicodeAbove255Codeunits_CharsetUnsetAsOnCE()
        {
            byte[] data = {1,0,0,0,
                //id
                0xFF,0x01,0x00,0x54,
                //name
                0x00, 0x01,  0x01, 0x01,  0x02, 0x01,
                0, 0, 0, 0, 
                0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 
                0, 0, 
                //hints
                0x01, 0x51,
                //charset
                0 //unset should be unicode, be is thus ascii
            };//data
            IrDADeviceInfo[] diArray = DoParseDeviceList(data);
            Assert.AreEqual(1, diArray.Length);
            //
            Assert.AreEqual(AddressA, diArray[0].DeviceAddress);
            string name = "\u0100\u0101\u0102";
            Assert.AreEqual(name, diArray[0].DeviceName);
            //Assert.AreEqual(0x5101, (int)diArray[0].Hints);
            Assert.AreEqual(0, (int)diArray[0].CharacterSet);
        }
#endif

#if ! PocketPC
        [Test]
        public void oneLatin1()
        {
            byte[] data = {1,0,0,0,
                //id
                0xFF,0x01,0x00,0x54,
                //name
                (byte)'µ', (byte)'Á', (byte)'n', (byte)'n', (byte)'n', 
                (byte)'n', (byte)'n', (byte)'n', (byte)'n', (byte)'n',  
                (byte)'n', (byte)'n', (byte)'n', (byte)'n', (byte)'n',  
                (byte)'n', (byte)'n', (byte)'n', (byte)'n', (byte)'n',  
                (byte)'n', (byte)'n', 
                //hints
                0x01, 0x51,
                //charset
                1 // ISO-8859-1
            };//data
            IrDADeviceInfo[] diArray = DoParseDeviceList(data);
            Assert.AreEqual(1, diArray.Length);
            //
            Assert.AreEqual(AddressA, diArray[0].DeviceAddress);
            string name = "µÁ".PadRight(22, 'n');
            Assert.AreEqual(name, diArray[0].DeviceName);
            Assert.AreEqual(0x5101, (int)diArray[0].Hints);
            Assert.AreEqual(1, (int)diArray[0].CharacterSet);
        }//fn
#else
        [Test]
        public void oneLatin1_CharsetUnsetAsOnCE()
        {
            byte[] data = {1,0,0,0,
                //id
                0xFF,0x01,0x00,0x54,
                //name
                (byte)'µ', (byte)'Á', (byte)'n', (byte)'n', (byte)'n', 
                (byte)'n', (byte)'n', (byte)'n', (byte)'n', (byte)'n',  
                (byte)'n', (byte)'n', (byte)'n', (byte)'n', (byte)'n',  
                (byte)'n', (byte)'n', (byte)'n', (byte)'n', (byte)'n',  
                (byte)'n', (byte)'n', 
                //hints
                0x01, 0x51,
                //charset
                0 //unset, should be ISO-8859-1
            };//data
            IrDADeviceInfo[] diArray = DoParseDeviceList(data);
            Assert.AreEqual(1, diArray.Length);
            //
            Assert.AreEqual(AddressA, diArray[0].DeviceAddress);
            string name = "??".PadRight(22, 'n');
            Assert.AreEqual(name, diArray[0].DeviceName);
            //Assert.AreEqual(0x5101, (int)diArray[0].Hints);
            Assert.AreEqual(0, (int)diArray[0].CharacterSet);
        }//fn
#endif

    }//class


    public struct TestHolderIrDADeviceInfo
#if ! FX1_1
                                        : IEquatable<IrDADeviceInfo>
#endif
    {
        private IrDAAddress m_address;
        private String m_name;
        private IrDAHints m_hints;
        private IrDACharacterSet m_charset;

        public TestHolderIrDADeviceInfo(IrDAAddress address, String name, IrDAHints hints, IrDACharacterSet charset)
        {
            m_address = address;
            m_name = name;
            m_hints = hints;
            m_charset = charset;
        }

        public TestHolderIrDADeviceInfo(Int32 address, String name, IrDAHints hints, IrDACharacterSet charset)
            :this(new IrDAAddress (address), name, hints, charset)
        { }

        #region IEquatable<IrDADeviceInfo> Members

        public bool Equals(IrDADeviceInfo other)
        {
            return
                this.m_address == other.DeviceAddress
                && this.m_name == other.DeviceName
                && this.m_hints == other.Hints
                && this.m_charset == other.CharacterSet
                ;
        }

        #endregion

        // Conversion operator.
        public static implicit operator TestHolderIrDADeviceInfo(IrDADeviceInfo other)
        {
            return new TestHolderIrDADeviceInfo(other.DeviceAddress, other.DeviceName, other.Hints, other.CharacterSet);
        }

        //----------------------------------------------------------

        public static IrDADeviceInfo[] CreateDeviceInfoArray(TestHolderIrDADeviceInfo[] devicesList)
        {
            int offset;
            byte[] allBuffer;
            offset = CreateBufferAndWriteHeader(devicesList.Length, out allBuffer);
            //
            foreach (TestHolderIrDADeviceInfo curDevice in devicesList) {
                offset += CreateDeviceListItemBuffer(allBuffer, offset, curDevice);
            }
            System.Diagnostics.Debug.Assert(offset == allBuffer.Length);
            //----
            IrDADeviceInfo[] devices = DeviceInfoParse.DoParseDeviceList(allBuffer);
            return devices;
        }


        public static IrDADeviceInfo CreateDeviceListInfo(byte[] address, String name, byte[] hints, byte charset)
        {
            int offset;
            byte[] buffer;
            offset = CreateBufferAndWriteHeader(1, out buffer);
            //
            int length = CreateDeviceListItemBuffer(buffer, offset,
                     address, name, hints, charset);
            //
            IrDADeviceInfo[] devices = DeviceInfoParse.DoParseDeviceList(buffer);
            if (devices.Length != 1) {
                throw new ArgumentException("Expected one device info result.");
            }
            return devices[0];
        }

        private static int CreateBufferAndWriteHeader(int deviceCount, out byte[] allBuffer)
        {
            const int CountLength = 4;
            const int ItemLength = 29;
            //
            int offset = 0;
            allBuffer = new byte[CountLength + deviceCount * ItemLength];
            //
            byte[] countBuf = BitConverter.GetBytes((UInt32)deviceCount);
            countBuf.CopyTo(allBuffer, offset);
            offset += 4;
            return offset;
        }

        private static int CreateDeviceListItemBuffer(byte[] buffer, int index, TestHolderIrDADeviceInfo curDevice)
        {
            byte[] hintsBytes = BitConverter.GetBytes((UInt16)curDevice.m_hints);
            //
            int length = CreateDeviceListItemBuffer(buffer, index,
                     curDevice.m_address.ToByteArray(), curDevice.m_name, hintsBytes, (Byte)curDevice.m_charset);
            return length;
        }

        private static int CreateDeviceListItemBuffer(byte[] buffer, int index,
                    byte[] address, String name, byte[] hints, byte charset)
        {
            const int OffsetAddress = 0;
            const int OffsetName = 4;
            const int MaxLengthName = 18;
            const int OffsetHints = 26;
            const int LengthHints = 2;
            const int OffsetCharset = 28;
            const int ItemLength = 29;
            //
            // Address
            address.CopyTo(buffer, OffsetAddress + index);
            // Name
            byte[] strBytes = Encoding.ASCII.GetBytes(name);
            if (strBytes.Length > MaxLengthName) {
                throw new ArgumentException();
            }
            strBytes.CopyTo(buffer, OffsetName + index);
            //
            if (hints.Length != LengthHints) {
                throw new ArgumentException("Hints must be two bytes long.");
            }
            hints.CopyTo(buffer, OffsetHints + index);
            //
            buffer[OffsetCharset + index] = charset;
            //
            return ItemLength;
        }

    }//struct


    //--------------------------------------------------------------------------
    [TestFixture]
    public class TestIrDADeviceInfo
    {
        public static IrDADeviceInfo Factory_IrDADeviceInfo(byte[] address, String name, byte[] hints, byte charset)
        {
            IrDADeviceInfo item = TestHolderIrDADeviceInfo.CreateDeviceListInfo(address, name, hints, charset);
            return item;
        }


        public void AssertAreEqualIrDADeviceInfoEveryField(IrDADeviceInfo x, IrDADeviceInfo y)
        {
            Assert.AreEqual(x.DeviceName, y.DeviceName);
            Assert.AreEqual(x.DeviceAddress, y.DeviceAddress);
            // Check the byte array version too, so not depending on IrDAAddress.Equals.
#if FX1_1
        /*
#endif
#pragma warning disable 618
#if FX1_1
        */
#endif
            Assert.AreEqual(x.DeviceID, y.DeviceID); // Don't fix the 'Obsolete' warning!
#if FX1_1
            /*
#endif
#pragma warning restore 618
#if FX1_1
        */
#endif
            Assert.AreEqual(x.Hints, y.Hints);
            Assert.AreEqual(x.CharacterSet, y.CharacterSet);
        }

        //--------------------------------------------------------------
        // Equals
        //--------------------------------------------------------------

        [Test]
        public void EqualsDifferentInstancesAllSameFields()
        {
            byte[] addr = new byte[] { 1, 2, 3, 4 };
            String name = "abcdef";
            byte[] hints = new byte[] { 5, 6 };
            byte charset = 0x01;
#if NETCF
            charset = 0x00; // Latin-1 not supported on CF(?).
#endif
            IrDADeviceInfo Device1 = Factory_IrDADeviceInfo(addr, name, hints, charset);
            IrDADeviceInfo Device2 = Factory_IrDADeviceInfo(addr, name, hints, charset);
            Assert.AreNotSame(Device1, Device2);
            Assert.AreEqual(Device1, Device2);
        }

        [Test]
        public void EqualsDifferentInstancesOnlyAddressSame()
        {
            byte[] addr = new byte[] { 1, 2, 3, 4 };
            String name1 = "abcdef";
            String name2 = "zzzzzz";
            byte[] hints1 = new byte[] { 15, 16 };
            byte[] hints2 = new byte[] { 12, 2 };
            byte charset1 = 0x00;
            byte charset2 = 0x01;
#if NETCF
            charset2 = 0x00; // Latin-1 not supported on CF(?).
#endif
            IrDADeviceInfo Device1 = Factory_IrDADeviceInfo(addr, name1, hints1, charset1);
            IrDADeviceInfo Device2 = Factory_IrDADeviceInfo(addr, name2, hints2, charset2);
            Assert.AreNotSame(Device1, Device2);
            Assert.AreEqual(Device1, Device2);
        }

        [Test]
        public void EqualsDifferentInstancesAllSameFieldsExceptAddressDifferent()
        {
            byte[] addr1 = new byte[] { 1, 2, 3, 4 };
            byte[] addr2 = new byte[] { 91, 102, 253, 14 };
            String name = "abcdef";
            byte[] hints = new byte[] { 5, 6 };
            byte charset = 0x01;
#if NETCF
            charset = 0x00; // Latin-1 not supported on CF(?).
#endif
            IrDADeviceInfo Device1 = Factory_IrDADeviceInfo(addr1, name, hints, charset);
            IrDADeviceInfo Device2 = Factory_IrDADeviceInfo(addr2, name, hints, charset);
            Assert.AreNotSame(Device1, Device2);
            Assert.AreNotEqual(Device1, Device2);
        }

        //--------------------------------------------------------------
        // Non-mutable.
        //--------------------------------------------------------------

        [Test]
        public void MutationNotAllowedViaDeviceAddressToByteArray()
        {
            byte[] addr = new byte[] { 1, 2, 3, 4 };
            String name = "abcdef";
            byte[] hints = new byte[] { 5, 6 };
            byte charset = 0x01;
#if NETCF
            charset = 0x00; // Latin-1 not supported on CF(?).
#endif
            IrDADeviceInfo Device1 = Factory_IrDADeviceInfo(addr, name, hints, charset);
            //
            Assert.AreEqual(new IrDAAddress(addr), Device1.DeviceAddress);
            //
            byte[] internalBytes = Device1.DeviceAddress.ToByteArray();
            Assert.AreEqual(addr, internalBytes);
            internalBytes[1] = 0xFF;    // Attempt to mutate the Address!!!
            //
            Assert.AreEqual(new IrDAAddress(addr), Device1.DeviceAddress);
        }

        [Test]
        public void MutationNotAllowedViaDeviceID()
        {
            byte[] addr = new byte[] { 1, 2, 3, 4 };
            String name = "abcdef";
            byte[] hints = new byte[] { 5, 6 };
            byte charset = 0x01;
#if NETCF
            charset = 0x00; // Latin-1 not supported on CF(?).
#endif
            IrDADeviceInfo Device1 = Factory_IrDADeviceInfo(addr, name, hints, charset);
            //
            Assert.AreEqual(new IrDAAddress(addr), Device1.DeviceAddress);
            //
#if FX1_1
        /*
#endif
#pragma warning disable 618
#if FX1_1
        */
#endif
            byte[] internalBytes = Device1.DeviceID; // Don't fix the 'Obsolete' warning!
#if FX1_1
            /*
#endif
#pragma warning restore 618
#if FX1_1
        */
#endif
            Assert.AreEqual(addr, internalBytes);
            internalBytes[1] = 0xFF;    // Attempt to mutate the Address!!!
            //
            Assert.AreEqual(new IrDAAddress(addr), Device1.DeviceAddress);
        }

    }//class

}//namespace

