using System;
using System.Text;
using NUnit.Framework;
using InTheHand.Net;


namespace InTheHand.Net.Tests.Irda.Net
{
    namespace TestIrDAEndPoint
    {
        
        public class Values
        {
            //--------------------------------------------------------------
            public static readonly IrDAAddress AddressNull = null;
            public static readonly byte[] AddressBytesNull = null;
            public static readonly IrDAAddress AddressOne = new IrDAAddress(0x04030201);

            public static IrDAEndPoint CreateWithNullAddress()
            {
                return new IrDAEndPoint(AddressNull, "SvcName1");
            }

            public static IrDAEndPoint CreateWithNullAddressBytes()
            {
#if FX1_1
        /*
#endif
#pragma warning disable 618
#if FX1_1
        */
#endif
                return new IrDAEndPoint(AddressBytesNull, "SvcName1"); // Don't fix the 'Obsolete warning!
#if FX1_1
                /*
#endif
#pragma warning restore 618
#if FX1_1
        */
#endif
            }

            public static IrDAEndPoint CreateWithNullServiceName()
            {
                return new IrDAEndPoint(AddressOne, null);
            }

            public static IrDAEndPoint CreateWithEmptyServiceName()
            {
                return new IrDAEndPoint(AddressOne, String.Empty);
            }

        }

        [TestFixture]
        public class Ctor
        {

            //--------------------------------------------------------------
            // Constructor
            //--------------------------------------------------------------

            [Test]
            [ExpectedException(typeof(System.ArgumentNullException), ExpectedMessage = "Value cannot be null." + Tests_Values.NewLine + "Parameter name: irdaDeviceAddress")]
            public void CtorNullAddress() { Values.CreateWithNullAddress(); }

            [Test]
            [ExpectedException(typeof(System.ArgumentNullException), ExpectedMessage = "Value cannot be null." + Tests_Values.NewLine + "Parameter name: irdaDeviceID")]
            public void CtorNullAddressBytes() { Values.CreateWithNullAddressBytes(); }

            [Test]
            [ExpectedException(typeof(System.ArgumentNullException), ExpectedMessage = "Value cannot be null." + Tests_Values.NewLine + "Parameter name: serviceName")]
            public void CtorNullServiceName() { Values.CreateWithNullServiceName(); }

            [Test]
            // ? [ExpectedException(typeof(System.ArgumentException), "ServiceName cannot be blank." + Tests_Values.NewLine + "Parameter name: serviceName")]
            public void CtorEmptyServiceName() { Values.CreateWithEmptyServiceName(); }


            //--------------------------------------------------------------
            // (Contructor and...) Use the fields.  Proving that they've been set correctly!
            //--------------------------------------------------------------

            // Should test Serialize result, and Create.  But they're apparently working... :-)

            [Test]
            [ExpectedException(typeof(System.ArgumentNullException), ExpectedMessage = "Value cannot be null." + Tests_Values.NewLine + "Parameter name: irdaDeviceAddress")]
            public void SerializeNullAddress() { 
                Values.CreateWithNullAddress().Serialize();
            }

            // SerializeNullAddressBytes

            [Test]
            [ExpectedException(typeof(System.ArgumentNullException), ExpectedMessage = "Value cannot be null." + Tests_Values.NewLine + "Parameter name: serviceName")]
            public void SerializeNullServiceName() { 
                Values.CreateWithNullServiceName().Serialize();
            }

            [Test]
            // ? [ExpectedException(typeof(System.ArgumentException), "ServiceName cannot be blank." + Tests_Values.NewLine + "Parameter name: serviceName")]
            public void SerializeEmptyServiceName() {
                Values.CreateWithEmptyServiceName().Serialize(); 
            }

        }


        [TestFixture]
        public class SerializeToFromSocketAddress
        {

            //--------------------------------------------------------------

            //typedef struct _SOCKADDR_IRDA
            //{
            //    u_short irdaAddressFamily;
            //    u_char  irdaDeviceID[4];
            //    char	irdaServiceName[25];
            //} SOCKADDR_IRDA, *PSOCKADDR_IRDA, FAR *LPSOCKADDR_IRDA;

            byte[] ExpectedBuffer24LetterAThusOneNullTerminatorAndWithPadByte = { 
                /* AF */ 26, 0,
                /* ID */ 1,2,3,4,
                /* SN */ (byte)'a', (byte)'a', (byte)'a', (byte)'a', (byte)'a',
                        (byte)'a', (byte)'a', (byte)'a', (byte)'a', (byte)'a',
                        (byte)'a', (byte)'a', (byte)'a', (byte)'a', (byte)'a',
                        (byte)'a', (byte)'a', (byte)'a', (byte)'a', (byte)'a',
                        (byte)'a', (byte)'a', (byte)'a', (byte)'a', /**/0,
                /*Padding to struct size (due to first member: "u_short irdaAddressFamily;" */
                    0
                };

            public static void AssertAreEqualSocketAddressBuffer(byte[] expectedSockaddr, System.Net.SocketAddress sa)
            {
                byte[] result = new byte[sa.Size];
                for (int i = 0; i < result.Length; ++i) {
                    result[i] = sa[i];
                }
                Assert.AreEqual(expectedSockaddr, result);
            }

            //--------------------------------------------------------------

            [Test]
            public void SerializeA()
            {
                System.Net.SocketAddress sa = new IrDAEndPoint(Values.AddressOne,
                    new String('a', 24)).Serialize();
                AssertAreEqualSocketAddressBuffer(ExpectedBuffer24LetterAThusOneNullTerminatorAndWithPadByte, sa);
            }

            [Test]
            public void Serialize24ByteServiceName()
            {
                System.Net.SocketAddress sa = new IrDAEndPoint(Values.AddressOne,
                    new String('a', 24)).Serialize();
                AssertAreEqualSocketAddressBuffer(ExpectedBuffer24LetterAThusOneNullTerminatorAndWithPadByte, sa);
            }

            [Test]
            [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "ServiceName has a maximum length of 24 bytes.")]
            public void SerializeOverlongServiceName()
            {
                System.Net.SocketAddress sa = new IrDAEndPoint(Values.AddressOne,
                    new String('a', 50)).Serialize();
            }

            [Test]
            [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "ServiceName has a maximum length of 24 bytes.")]
            public void Serialize25ByteServiceName()
            {
                System.Net.SocketAddress sa = new IrDAEndPoint(Values.AddressOne,
                    new String('a', 25)).Serialize();
                //AssertAreEqual(ExpectedBuffer24LetterAThusOneNullTerminatorAndWithPadByte, sa);
            }

            [Test]
            [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "ServiceName has a maximum length of 24 bytes.")]
            public void Serialize26ByteServiceName()
            {
                System.Net.SocketAddress sa = new IrDAEndPoint(Values.AddressOne,
                    new String('a', 26)).Serialize();
                //AssertAreEqual(ExpectedBuffer24LetterAThusOneNullTerminatorAndWithPadByte, sa);
            }

            [Test]
            [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "ServiceName has a maximum length of 24 bytes.")]
            public void Serialize27ByteServiceName()
            {
                System.Net.SocketAddress sa = new IrDAEndPoint(Values.AddressOne,
                    new String('a', 27)).Serialize();
                //AssertAreEqual(ExpectedBuffer24LetterAThusOneNullTerminatorAndWithPadByte, sa);
            }

            //--------------------------------------------------------------
            //--------------------------------------------------------------

            public static System.Net.SocketAddress Factory_SocketAddressForIrDA(byte[] buffer)
            {
                return Factory_SocketAddress(System.Net.Sockets.AddressFamily.Irda, buffer);
            }

            /// <summary>
            /// Create <see cref="T:System.Net.SocketAddress"/> filling it with
            /// the contents of the specified buffer.
            /// </summary>
            public static System.Net.SocketAddress Factory_SocketAddress(System.Net.Sockets.AddressFamily af, byte[] buffer)
            {
                System.Net.SocketAddress sa = new System.Net.SocketAddress(af, buffer.Length);
                for (int i = 0; i < buffer.Length; ++i) {
                    sa[i] = buffer[i];
                }//foreach
                return sa;
            }

            //--------------------------------------------------------------

            [Test]
            public void CreateA()
            {
                byte[] buffer = { 
                /* AF */ 26, 0,
                /* ID */ 1,2,3,4,
                /* SN */ (byte)'S', (byte)'v', (byte)'c', (byte)'N', (byte)'a',
                         (byte)'m', (byte)'e', (byte)'1', 0,0,
                         0,0,0,0,0,
                         0,0,0,0,0,
                         0,0,0,0,0,
                /* struct alignment pad */ (byte)'!'
                };
                System.Net.SocketAddress sa = Factory_SocketAddressForIrDA(buffer);
                IrDAEndPoint ep = (IrDAEndPoint)new IrDAEndPoint(Values.AddressOne, "x").Create(sa);
                //
                Assert.AreEqual(new byte[] { 1, 2, 3, 4 }, ep.Address.ToByteArray());
                Assert.AreEqual("SvcName1", ep.ServiceName);
            }

            [Test]
            public void CreateANoPadByte()
            {
                byte[] buffer = { 
                /* AF */ 26, 0,
                /* ID */ 1,2,3,4,
                /* SN */ (byte)'S', (byte)'v', (byte)'c', (byte)'N', (byte)'a',
                         (byte)'m', (byte)'e', (byte)'1', 0,0,
                         0,0,0,0,0,
                         0,0,0,0,0,
                         0,0,0,0,0,
                /* No padding byte... */
                };
                System.Net.SocketAddress sa = Factory_SocketAddressForIrDA(buffer);
                IrDAEndPoint ep = (IrDAEndPoint)new IrDAEndPoint(Values.AddressOne, "x").Create(sa);
                //
                Assert.AreEqual(new byte[] { 1, 2, 3, 4 }, ep.Address.ToByteArray());
                Assert.AreEqual("SvcName1", ep.ServiceName);
            }

            [Test]
            // Should this fail?  The ServiceName char[] is _not_ null-terminated!!!
            public void CreateB()
            {
                byte[] buffer = { 
                /* AF */ 26, 0,
                /* ID */ 1,2,3,4,
                /* SN */ (byte)'a', (byte)'a', (byte)'a', (byte)'a', (byte)'a',
                        (byte)'a', (byte)'a', (byte)'a', (byte)'a', (byte)'a',
                        (byte)'a', (byte)'a', (byte)'a', (byte)'a', (byte)'a',
                        (byte)'a', (byte)'a', (byte)'a', (byte)'a', (byte)'a',
                        (byte)'a', (byte)'a', (byte)'a', (byte)'a', (byte)'a',
                /* struct alignment pad */ (byte)'!'
                };
                System.Net.SocketAddress sa = Factory_SocketAddressForIrDA(buffer);
                IrDAEndPoint ep = (IrDAEndPoint)new IrDAEndPoint(Values.AddressOne, "x").Create(sa);
                //
                Assert.AreEqual(new byte[] { 1, 2, 3, 4 }, ep.Address.ToByteArray());
                Assert.AreEqual(new String('a', 24), ep.ServiceName);
            }

            [Test]
            // Should this fail?  The ServiceName char[] is _not_ null-terminated!!!
            public void CreateBNoPadByte()
            {
                byte[] buffer = { 
                /* AF */ 26, 0,
                /* ID */ 1,2,3,4,
                /* SN */ (byte)'a', (byte)'a', (byte)'a', (byte)'a', (byte)'a',
                        (byte)'a', (byte)'a', (byte)'a', (byte)'a', (byte)'a',
                        (byte)'a', (byte)'a', (byte)'a', (byte)'a', (byte)'a',
                        (byte)'a', (byte)'a', (byte)'a', (byte)'a', (byte)'a',
                        (byte)'a', (byte)'a', (byte)'a', (byte)'a', (byte)'a',
                /* No padding byte. */
                };
                System.Net.SocketAddress sa = Factory_SocketAddressForIrDA(buffer);
                IrDAEndPoint ep = (IrDAEndPoint)new IrDAEndPoint(Values.AddressOne, "x").Create(sa);
                //
                Assert.AreEqual(new byte[] { 1, 2, 3, 4 }, ep.Address.ToByteArray());
                Assert.AreEqual(new String('a', 24), ep.ServiceName);
            }

        }//class


        [TestFixture]
        public class ToString
        {
            //--------------------------------------------------------------
            // ToString
            //--------------------------------------------------------------

            [Test]
            public void ToStringAAddress()
            {
                IrDAEndPoint ep1 = new IrDAEndPoint(new IrDAAddress(new byte[] { 1, 2, 3, 4 }), "SvcName1");
                String str = ep1.ToString();
                Assert.AreEqual("04030201:SvcName1", str);
            }

            [Test]
            public void ToStringABytes()
            {
                IrDAEndPoint ep1 = new IrDAEndPoint(Values.AddressOne, "SvcName1");
                String str = ep1.ToString();
                Assert.AreEqual("04030201:SvcName1", str);
            }

            [Test]
            public void ToStringAddressNone()
            {
                IrDAEndPoint ep1 = new IrDAEndPoint(IrDAAddress.None, "SvcName1");
                String str = ep1.ToString();
                Assert.AreEqual("00000000:SvcName1", str);
            }

            [Test]
            public void ToStringC()
            {
                IrDAEndPoint ep1 = new IrDAEndPoint(new IrDAAddress(new byte[] { 1, 2, 3, 4 }), String.Empty);
                String str = ep1.ToString();
                Assert.AreEqual("04030201:", str);
            }

        }//class

    }


    namespace TestIrDAAddress
    {
        //--------------------------------------------------------------------------
        [TestFixture]
        public class Misc
        {
            //--------------------------------------------------------------
            // Constructor: invalid arguments.
            //--------------------------------------------------------------
            // For testing of the (Int32) constructor see the Formatting tests classes.
            // For testing of valid inputs to both constructors see the Formatting tests classes.

            [Test]
            [ExpectedException(typeof(System.ArgumentNullException), ExpectedMessage = "Value cannot be null." + Tests_Values.NewLine + "Parameter name: address")]
            public void CtorNull()
            {
                new IrDAAddress(null);
            }

            [Test]
            [ExpectedException(typeof(System.ArgumentException), ExpectedMessage = "Address bytes array must be four bytes in size.")]
            public void CtorZeroLengthArray()
            {
                new IrDAAddress(new byte[0]);
            }

            [Test]
            [ExpectedException(typeof(System.ArgumentException), ExpectedMessage = "Address bytes array must be four bytes in size.")]
            public void CtorFiveLengthArray()
            {
                new IrDAAddress(new byte[5]);
            }

            //--------------------------------------------------------------
            // Show dodgy mutating address!!!!
            //--------------------------------------------------------------

            [Test]
            public void MutationNotAllowedViaArrayCtor()
            {
                const String addrAsString = "04030201";
                byte[] bytes = { 0x01, 0x02, 0x03, 0x04 };
                IrDAAddress addr = new IrDAAddress(bytes);
                Assert.AreEqual(addrAsString, addr.ToString());
                bytes[1] = 0xFF;    // Attempt to mutate the IrDAAddress!!!
                Assert.AreEqual(addrAsString, addr.ToString());
            }

            [Test]
            public void MutationNotAllowedViaToByte()
            {
                const String addrAsString = "04030201";
                IrDAAddress addr = new IrDAAddress(0x04030201);
                Assert.AreEqual(addrAsString, addr.ToString());
                byte[] internalBytes = addr.ToByteArray();
                internalBytes[1] = 0xFF;    // Attempt to mutate the IrDAAddress!!!
                Assert.AreEqual(addrAsString, addr.ToString());
            }

            //--------------------------------------------------------------
            // * Test that the odd/bad endian-dependent behaviour doesn't change.
            // The behaviour is not ideal but to change it would be a breaking change
            // so shan't allow.
            // If porting to a big-endian platform, have to decide then what behaviour
            // to aim for there.
            //
            // * Also tests ToByteArray and ToInt32 by side-effect.
            //--------------------------------------------------------------

            [Test]
            public void KeepBadEndianBehaviourInt32()
            {
                IrDAAddress addr = new IrDAAddress(0x01020304);
                Assert.AreEqual(0x01020304, addr.ToInt32());
                Assert.AreEqual(new byte[] { 0x04, 0x03, 0x02, 0x01, }, addr.ToByteArray());
                Assert.AreEqual("01020304", addr.ToString("N"));
            }

            [Test]
            public void KeepBadEndianBehaviourByteArray()
            {
                IrDAAddress addr = new IrDAAddress(new byte[] { 0x01, 0x02, 0x03, 0x04, });
                Assert.AreEqual(0x04030201, addr.ToInt32());
                Assert.AreEqual(new byte[] { 0x01, 0x02, 0x03, 0x04, }, addr.ToByteArray());
                Assert.AreEqual("04030201", addr.ToString("N"));
            }

        }//class


        //--------------------------------------------------------------------------
        /// <summary>
        /// Test infrastructure for testing the various forms of ToString.
        /// </summary>
        // [TestFixture] implemented on derived classes: FormattingToString, and FormattingIFormattable.
        public abstract class FormattingBase
        {
            protected abstract String DoFormatting(Object obj, String format);
            protected abstract String DoFormatting(Object obj);

            //--------------------------------------------------------------
            // Test
            //--------------------------------------------------------------

            /// <summary>
            /// Test (NUnit Asserts) the object's <c>ToString()</c> method.
            /// </summary>
            /// <param name="expectedString">
            /// The expected result, as a <see cref="T:System.String"/>
            /// </param>
            /// <param name="obj">
            /// The object on which <c>ToString(String format)</c> is to be tested.
            /// </param>
            protected void DoTest(String expectedString, Object obj)
            {
                String result = DoFormatting(obj);
                Assert.AreEqual(expectedString, result);
            }

            /// <summary>
            /// Test (NUnit Asserts) the object's <c>ToString(String format)</c> method.
            /// </summary>
            /// <param name="expectedString">
            /// The expected result, as a <see cref="T:System.String"/>
            /// </param>
            /// <param name="obj">
            /// The object on which <c>ToString(String format)</c> is to be tested.
            /// </param>
            /// <param name="format">
            /// The format string to be tested.  
            /// For example <c>"X"</c> to format an <see cref="T:System.Int32"/> as hexadecimal, 
            /// see <see cref="M:System.Int32.ToString(System.String"/> and 
            /// <see cref="T:System.Globalization.NumberFormatInfo"/>.
            /// </param>
            protected void DoTest(String expectedString, Object obj, String format)
            {
                String result = DoFormatting(obj, format);
                Assert.AreEqual(expectedString, result);
            }

        }//class

        /// <summary>
        /// Provides the actual test method implementations, each marked with attribute
        /// <c>[Test]</c>.
        /// </summary>
        public abstract class Formatting : FormattingBase
        {
            //--------------------------------------------------------------
            // Simply formatting, no format string supplied
            //--------------------------------------------------------------

            [Test]
            public void AddrA()
            {
                IrDAAddress addr = new IrDAAddress(0x01020304);
                DoTest("01020304", addr);
            }

            [Test]
            public void AddrB()
            {
                IrDAAddress addr = new IrDAAddress(new byte[] { 0x01, 0x02, 0x03, 0x04 });
                DoTest("04030201", addr);
            }

            [Test]
            public void AddrZeros()
            {
                IrDAAddress addr = new IrDAAddress(0);
                DoTest("00000000", addr);
            }

            [Test]
            public void AddrAllOnes()
            {
                IrDAAddress addr = new IrDAAddress(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF });
                DoTest("FFFFFFFF", addr);
            }

            [Test]
            public void AddrNegativeOne()
            {
                IrDAAddress addr = new IrDAAddress(-1);
                DoTest("FFFFFFFF", addr);
            }

            [Test]
            public void AddrNone()
            {
                IrDAAddress addr = IrDAAddress.None;
                DoTest("00000000", addr);
            }

            //--------------------------------------------------------------
            // With formatting codes
            //--------------------------------------------------------------
            const String FcodeNotExists = "G";
            //
            const String FcodeNull = null;
            const String FcodeEmpty = "";
            //
            const String FcodePlainNumbers = "N";
            const String FcodeColons = "C";
            const String FcodePeriods = "P";

            [Test]
            [ExpectedException(typeof(System.FormatException), ExpectedMessage = "Invalid format specified - must be either \"N\", \"C\", \"P\", \"\" or null.")]
            public void BadCode()
            {
                IrDAAddress addr = new IrDAAddress(0x01020304);
                DoTest("01020304", addr, FcodeNotExists);
            }

            //------------------------------------------
            // Code null
            //------------------------------------------

            [Test]
            public void NullCodeAddrA()
            {
                IrDAAddress addr = new IrDAAddress(0x01020304);
                DoTest("01020304", addr, FcodeNull);
            }

            [Test]
            public void NullCodeAddrZeros()
            {
                IrDAAddress addr = new IrDAAddress(0);
                DoTest("00000000", addr, FcodeNull);
            }

            //------------------------------------------
            // Code ""
            //------------------------------------------

            [Test]
            public void EmptyCodeAddrA()
            {
                IrDAAddress addr = new IrDAAddress(0x01020304);
                DoTest("01020304", addr, FcodeEmpty);
            }

            [Test]
            public void EmptyCodeAddrZeros()
            {
                IrDAAddress addr = new IrDAAddress(0);
                DoTest("00000000", addr, FcodeEmpty);
            }

            //------------------------------------------
            // Code "N"
            //------------------------------------------

            [Test]
            public void NCodeAddrA()
            {
                IrDAAddress addr = new IrDAAddress(0x01020304);
                DoTest("01020304", addr, FcodePlainNumbers);
            }

            [Test]
            public void NCodeAddrZeros()
            {
                IrDAAddress addr = new IrDAAddress(0);
                DoTest("00000000", addr, FcodePlainNumbers);
            }

            //------------------------------------------
            // Code "C"
            //------------------------------------------

            [Test]
            public void CCodeAddrA()
            {
                IrDAAddress addr = new IrDAAddress(0x01020304);
                DoTest("01:02:03:04", addr, FcodeColons);
            }

            [Test]
            public void CCodeAddrZeros()
            {
                IrDAAddress addr = new IrDAAddress(0);
                DoTest("00:00:00:00", addr, FcodeColons);
            }

            //------------------------------------------
            // Code "P"
            //------------------------------------------

            [Test]
            public void PCodeAddrA()
            {
                IrDAAddress addr = new IrDAAddress(0x01020304);
                DoTest("01.02.03.04", addr, FcodePeriods);
            }

            [Test]
            public void PCodeAddrZeros()
            {
                IrDAAddress addr = new IrDAAddress(0);
                DoTest("00.00.00.00", addr, FcodePeriods);
            }

        }//class


        /// <summary>
        /// Test the <c>IrDAAddress.ToString()</c> and <c>IrDAAddress.ToString(String format)</c> methods.
        /// </summary>
        [TestFixture]
        public class FormattingToString : Formatting
        {
            /// <summary>
            /// Call <see cref="S:System.Object(System.String)"/> on the specified object.
            /// </summary>
            protected override String DoFormatting(Object obj)
            {
                return obj.ToString();
            }

            /// <summary>
            /// Call <c>ToString(String format)</c> on the specified object.
            /// </summary>
            protected override String DoFormatting(Object obj, String format)
            {
                // Call obj.ToString(format), without a interface to do so. :-(
                Type type = obj.GetType();
                System.Reflection.MethodInfo method = type.GetMethod("ToString", new Type[] { typeof(String) });
                try {
                    Object ret = method.Invoke(obj, new Object[1] { format });
                    return (String)ret;
                } catch (System.Reflection.TargetInvocationException ex) {
                    // The real exception, please!
                    throw ex.InnerException;
                }
            }

        }//class


        /// <summary>
        /// Test the <see cref="System.IFormattable.ToString">IFormattable.ToString(String, IFormatProvider)</see>
        /// method on <c>IrDAAddress</c>, both with and without format strings.
        /// </summary>
        // i.e. <see cref="M:InTheHand.Net.IrDAAddress.ToString(System.String,System.IFormatProvider)"/>.

        [TestFixture]
        public class FormattingIFormattable : Formatting
        {
            /// <summary>
            /// Call <see cref="System.IFormattable.ToString"/> without a format string.
            /// </summary>
            protected override String DoFormatting(Object obj)
            {
                return String.Format("{0}", obj);
            }

            /// <summary>
            /// Call <see cref="System.IFormattable.ToString"/> with a format string.
            /// </summary>
            protected override String DoFormatting(Object obj, String format)
            {
                // Do allow null, and empty format values.
                //
                return String.Format("{0:" + format + "}", obj);
            }

        }//class


        //--------------------------------------------------------------------------

        [TestFixture]
        public class Parse
        {
            //--------------------------------------------------------------
            private void DoTestParse(IrDAAddress expected, String s)
            {
                IrDAAddress result = IrDAAddress.Parse(s);
                Assert.AreEqual(expected, result);
            }

            private void DoTestTryParse(IrDAAddress expected, String s)
            {
                IrDAAddress result;
                bool success = IrDAAddress.TryParse(s, out result);
                Assert.IsTrue(success, "TryParse failed.");
                Assert.AreEqual(expected, result);
            }

            //----------------
            private void DoTestParseFails(String s)
            {
                IrDAAddress result = IrDAAddress.Parse(s);
                // Expected an Exception
                Assert.Fail("Expected an error from Parse");
            }

            private static void DoTestTryParseFails(String s)
            {
                IrDAAddress addr;
                try {
                    bool success = IrDAAddress.TryParse(s, out addr);
                    Assert.IsFalse(success);
                } catch {
                    Assert.Fail("TryParse should never throw an Exception.");
                }
            }

            //--------------------------------------------------------------
            [Test]
            [ExpectedException(typeof(System.ArgumentNullException), ExpectedMessage = "Value cannot be null." + Tests_Values.NewLine + "Parameter name: irdaString")]
            public void ParseNull()
            {
                DoTestParseFails(null);
            }

            [Test]
            public void TryParseNull()
            {
                DoTestTryParseFails(null);
            }

            [Test]
            [ExpectedException(typeof(System.FormatException), ExpectedMessage = "irdaString is not a valid IrDA address.")]
            public void ParseBlank()
            {
                DoTestParseFails("");
            }

            [Test]
            public void TryParseBlank()
            {
                DoTestTryParseFails("");
            }

            //--------------------------------------------------------------
            const String DigitZero = "0";
            const String StringA = "01020304";
            const String StringAColons = "01:02:03:04";
            const String StringAPeriods = "01.02.03.04";
            const String StringAPrefixHex = "0x01020304";
            readonly IrDAAddress AddressA = new IrDAAddress(0x01020304);
            //
            // *Not* an Int32!
            const String StringB = "FEDC00FF";
            const String StringBColons = "FE:DC:00:FF";
            const String StringBPeriods = "FE.DC.00.FF";
            const String StringBPrefixHex = "0xFEDC00FF";
            readonly IrDAAddress AddressB = new IrDAAddress(unchecked((int)0xFEDC00FF));


            // TODO Tests IrDAAddress.[Try-]Parse, more...

            [Test]
            [ExpectedException(typeof(System.FormatException), ExpectedMessage = "irdaString is not a valid IrDA address.")]
            public void ParseDigitZero ()
            {
                DoTestParse(IrDAAddress.None, DigitZero);
            }

            [Test]
            public void TryParseDigitZero()
            {
                DoTestTryParseFails(DigitZero);
            }

            //----------------------------------------------------------
            [Test]
            public void ParseA()
            {
                DoTestParse(AddressA, StringA);
            }

            [Test]
            public void TryParseA()
            {
                DoTestTryParse(AddressA, StringA);
            }

            [Test]
            public void ParseAColons()
            {
                DoTestParse(AddressA, StringAColons);
            }

            [Test]
            public void TryParseAColons()
            {
                DoTestTryParse(AddressA, StringAColons);
            }

            [Test]
            public void ParseAPeriods()
            {
                DoTestParse(AddressA, StringAPeriods);
            }

            [Test]
            public void TryParseAPeriods()
            {
                DoTestTryParse(AddressA, StringAPeriods);
            }

            //----------------------------------------------------------
            [Test]
            public void Int32ParseHexBigNegative(){
                int result = int.Parse(StringB, System.Globalization.NumberStyles.HexNumber);
                uint expectedU = 0xFEDC00FF;
                int expectedS = unchecked((int)expectedU);
                Assert.AreEqual(expectedS, result);
            }

            [Test]
            public void UInt32ParseHexBigNegative()
            {
                uint result = uint.Parse(StringB, System.Globalization.NumberStyles.HexNumber);
                uint expectedU = 0xFEDC00FF;
                //int expectedS = unchecked((int)expectedU);
                Assert.AreEqual(expectedU, result);
            }

            [Test]
            public void ParseB()
            {
                DoTestParse(AddressB, StringB);
            }

            [Test]
            public void TryParseB()
            {
                DoTestTryParse(AddressB, StringB);
            }

            [Test]
            public void ParseBColons()
            {
                DoTestParse(AddressB, StringBColons);
            }

            [Test]
            public void TryParseBColons()
            {
                DoTestTryParse(AddressB, StringBColons);
            }

            [Test]
            public void ParseBPeriods()
            {
                DoTestParse(AddressB, StringBPeriods);
            }

            [Test]
            public void TryParseBPeriods()
            {
                DoTestTryParse(AddressB, StringBPeriods);
            }

            //----------------------------------------------------------
            [Test]
            [ExpectedException(typeof(System.FormatException), ExpectedMessage = "irdaString is not a valid IrDA address.")]
            public void ParsePrefixHex()
            {
                DoTestParseFails(StringAPrefixHex);
            }

            [Test]
            public void TryParsePrefixHex()
            {
                DoTestTryParseFails(StringAPrefixHex);
            }
        }


    }//namespace

}//namespace
