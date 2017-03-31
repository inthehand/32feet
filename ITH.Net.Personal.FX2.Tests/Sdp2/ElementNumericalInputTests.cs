using System;
//using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using InTheHand.Net.Bluetooth;

namespace InTheHand.Net.Tests.Sdp2.ElementNumericalInputTests
{

    public
#if ! FX1_1
        static 
#endif
        class Code
    {
        public static void AssertIsElementNumericalInvalidError(Exception ex)
        {
            Assert.IsInstanceOfType(typeof(ArgumentOutOfRangeException), ex);
            //
#if !NETCF
            const string pattern = @"Value '[^\']+'  of type '[^\']+' not valid for element type .+\.";
            //                     @"Value '[^\']+' \(type [^\)]+\) out of range for element type .+\."
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(ex.Message, pattern);
            Assert.IsTrue(match.Success, "Bad exception message: " + ex.Message);
#endif
        }

    }//class


    [TestFixture]
    public class ElementNumerical_Manual
    {
        const string FooErrorMsg = "Value out of range for type.";


        private void DoTest_Valid(ElementType elementType, object value)
        {
            ServiceElement.CreateNumericalServiceElement(elementType, value);
        }

        private void DoTest_Invalid(ElementType elementType, object value)
        {
            try {
                ServiceElement.CreateNumericalServiceElement(elementType, value);
                Assert.Fail("Value '" + value + "' should have failed with element type '" + elementType + "'");
            } catch (ArgumentOutOfRangeException ex) {
                Code.AssertIsElementNumericalInvalidError(ex);
            }
        }

        //--------

        [Test]
        [ExpectedException(typeof(ArgumentException),
           ExpectedMessage = "Not a numerical type (TextString).")]
        public void NonNumericElementType()
        {
            DoTest_Valid(ElementType.TextString, "foo");
        }

        //--------

        [Test]
        public void BoolFalseInput()
        {
            try {
                DoTest_Valid(ElementType.Int32, false);
            } catch (Exception ex) {
                Code.AssertIsElementNumericalInvalidError(ex);
            }
        }

        [Test]
        public void BoolTrueInput()
        {
            try {
                DoTest_Valid(ElementType.Int32, true);
            } catch (Exception ex) {
                Code.AssertIsElementNumericalInvalidError(ex);
            }
        }

        //--------

        [Test]
        public void Uint8Byte()
        {
            DoTest_Valid(ElementType.UInt8, (byte)100);
            DoTest_Valid(ElementType.UInt8, (byte)200);
            DoTest_Valid(ElementType.UInt8, (byte)0);
        }

        [Test]
        public void Uint8Literal()
        {
            DoTest_Valid(ElementType.UInt8, 100);
            DoTest_Valid(ElementType.UInt8, 200);
            DoTest_Valid(ElementType.UInt8, 0);
            DoTest_Invalid(ElementType.UInt8, -100);
            DoTest_Invalid(ElementType.UInt8, 30000);
        }

        //--------

        [Test]
        public void Int8SByte()
        {
            DoTest_Valid(ElementType.Int8, (sbyte)100);
            DoTest_Valid(ElementType.Int8, (sbyte)0);
            DoTest_Valid(ElementType.Int8, (sbyte)-100);
        }

        [Test]
        public void Int8Literal()
        {
            DoTest_Valid(ElementType.Int8, 100);
            DoTest_Valid(ElementType.Int8, 0);
            DoTest_Valid(ElementType.Int8, -100);
            DoTest_Invalid(ElementType.Int8, 200);
        }

        //--------

        [Test]
        public void Uint16Byte()
        {
            DoTest_Valid(ElementType.UInt16, (UInt16)100);
            DoTest_Valid(ElementType.UInt16, (UInt16)200);
            DoTest_Valid(ElementType.UInt16, (UInt16)30000);
            DoTest_Valid(ElementType.UInt16, (UInt16)60000);
            DoTest_Valid(ElementType.UInt16, (UInt16)0);
        }

        [Test]
        public void Uint16Literal()
        {
            DoTest_Valid(ElementType.UInt16, 100);
            DoTest_Valid(ElementType.UInt16, 200);
            DoTest_Valid(ElementType.UInt16, 30000);
            DoTest_Valid(ElementType.UInt16, 60000);
            DoTest_Valid(ElementType.UInt16, 0);
            DoTest_Invalid(ElementType.UInt16, -100);
        }

        //--------

        [Test]
        public void Int16SByte()
        {
            DoTest_Valid(ElementType.Int16, (Int16)100);
            DoTest_Valid(ElementType.Int16, (Int16)200);
            DoTest_Valid(ElementType.Int16, (Int16)30000);
            DoTest_Valid(ElementType.Int16, (Int16)0);
            DoTest_Valid(ElementType.Int16, (Int16)(-100));
            DoTest_Valid(ElementType.Int16, (Int16)(-200));
            DoTest_Valid(ElementType.Int16, (Int16)(-30000));
        }

        [Test]
        public void Int16Literal()
        {
            DoTest_Valid(ElementType.Int16, 100);
            DoTest_Valid(ElementType.Int16, 200);
            DoTest_Valid(ElementType.Int16, 30000);
            DoTest_Valid(ElementType.Int16, 0);
            DoTest_Valid(ElementType.Int16, -100);
            DoTest_Valid(ElementType.Int16, -100);
            DoTest_Valid(ElementType.Int16, -200);
            DoTest_Valid(ElementType.Int16, -30000);
            DoTest_Invalid(ElementType.Int16, 60000);
            DoTest_Invalid(ElementType.Int16, 2000000000);
            DoTest_Invalid(ElementType.Int16, -60000);
        }

        //--------

        [Test]
        public void Uint32Byte()
        {
            DoTest_Valid(ElementType.UInt32, (UInt32)100);
            DoTest_Valid(ElementType.UInt32, (UInt32)200);
            DoTest_Valid(ElementType.UInt32, (UInt32)30000);
            DoTest_Valid(ElementType.UInt32, (UInt32)2000000000);
            DoTest_Valid(ElementType.UInt32, (UInt32)4000000000);
            DoTest_Valid(ElementType.UInt32, (UInt32)0);
        }

        [Test]
        public void Uint32Literal()
        {
            DoTest_Valid(ElementType.UInt32, 100);
            DoTest_Valid(ElementType.UInt32, 200);
            DoTest_Valid(ElementType.UInt32, 30000);
            DoTest_Valid(ElementType.UInt32, 2000000000);
            DoTest_Valid(ElementType.UInt32, 4000000000);
            DoTest_Valid(ElementType.UInt32, 0);
            DoTest_Invalid(ElementType.UInt32, -100);
        }

        //--------

        [Test]
        public void Int32SByte()
        {
            DoTest_Valid(ElementType.Int32, (Int32)100);
            DoTest_Valid(ElementType.Int32, (Int32)200);
            DoTest_Valid(ElementType.Int32, (Int32)30000);
            DoTest_Valid(ElementType.Int32, (Int32)2000000000);
            DoTest_Valid(ElementType.Int32, (Int32)0);
            DoTest_Valid(ElementType.Int32, (Int32)(-100));
            DoTest_Valid(ElementType.Int32, (Int32)(-200));
            DoTest_Valid(ElementType.Int32, (Int32)(-30000));
            DoTest_Valid(ElementType.Int32, (Int32)(-2000000000));
        }

        [Test]
        public void Int32Literal()
        {
            DoTest_Valid(ElementType.Int32, 100);
            DoTest_Valid(ElementType.Int32, 200);
            DoTest_Valid(ElementType.Int32, 30000);
            DoTest_Valid(ElementType.Int32, 2000000000);
            DoTest_Valid(ElementType.Int32, 0);
            DoTest_Valid(ElementType.Int32, -100);
            DoTest_Valid(ElementType.Int32, -100);
            DoTest_Valid(ElementType.Int32, -200);
            DoTest_Valid(ElementType.Int32, -30000);
            DoTest_Valid(ElementType.Int32, -2000000000);
            DoTest_Invalid(ElementType.Int32, 4000000000);
        }

        //--------

        [Test]
        public void Uint64Byte()
        {
            DoTest_Valid(ElementType.UInt64, (UInt64)100);
            DoTest_Valid(ElementType.UInt64, (UInt64)200);
            DoTest_Valid(ElementType.UInt64, (UInt64)0);
            DoTest_Valid(ElementType.UInt64, (UInt64)30000);
            DoTest_Valid(ElementType.UInt64, (UInt64)2000000000);
            DoTest_Valid(ElementType.UInt64, (UInt64)4000000000);
            DoTest_Valid(ElementType.UInt64, (UInt64)9223372036854775807);
            DoTest_Valid(ElementType.UInt64, (UInt64)18446744073709551615);
        }

        [Test]
        public void Uint64Literal()
        {
            DoTest_Valid(ElementType.UInt64, 100);
            DoTest_Valid(ElementType.UInt64, 200);
            DoTest_Valid(ElementType.UInt64, 0);
            DoTest_Valid(ElementType.UInt64, 30000);
            DoTest_Valid(ElementType.UInt64, 2000000000);
            DoTest_Valid(ElementType.UInt64, 4000000000);
            DoTest_Valid(ElementType.UInt64, 9223372036854775807);
            DoTest_Valid(ElementType.UInt64, 18446744073709551615);
            DoTest_Invalid(ElementType.UInt64, -200);
            DoTest_Invalid(ElementType.UInt64, -2000000000);
        }

        //--------

        [Test]
        public void Int64SByte()
        {
            DoTest_Valid(ElementType.Int64, (Int64)100);
            DoTest_Valid(ElementType.Int64, (Int64)0);
            DoTest_Valid(ElementType.Int64, (Int64)(-100));
            DoTest_Valid(ElementType.Int64, (Int64)2000000000);
            DoTest_Valid(ElementType.Int64, (Int64)(-2000000000));
            DoTest_Valid(ElementType.Int64, (Int64)9223372036854775807);
            DoTest_Valid(ElementType.Int64, (Int64)(-9223372036854775808));
        }

        [Test]
        public void Int64Literal()
        {
            DoTest_Valid(ElementType.Int64, 100);
            DoTest_Valid(ElementType.Int64, 0);
            DoTest_Valid(ElementType.Int64, -100);
            DoTest_Valid(ElementType.Int64, 2000000000);
            DoTest_Valid(ElementType.Int64, -2000000000);
            DoTest_Valid(ElementType.Int64,    9223372036854775807);
            DoTest_Valid(ElementType.Int64,   -9223372036854775808);
            DoTest_Invalid(ElementType.Int64, 10000000000000000000);
        }

        //--------

        [Test]
        public void FromEnum()
        {
            DoTest_Valid(ElementType.UInt32, (DummyEnumInt32)(300));
            DoTest_Valid(ElementType.UInt16, (DummyEnumInt32)(300));
            DoTest_Invalid(ElementType.UInt8, (DummyEnumInt32)(300));
            //Error	1	Constant value '80000' cannot be converted to a 'DummyEnumUInt16' (use 'unchecked' syntax to override)
            //DoTest_Invalid(ElementType.Int64, (DummyEnumInt326)(3000000000));
            //
            DoTest_Valid(ElementType.UInt32, (DummyEnumUInt16)(300));
            DoTest_Valid(ElementType.UInt16, (DummyEnumUInt16)(300));
            DoTest_Invalid(ElementType.UInt8, (DummyEnumUInt16)(300));
        }

        enum DummyEnumInt32
        {
            // No real values for testing, will just cast integer literals.
        }
        enum DummyEnumUInt16 : ushort
        {
            // No real values for testing, will just cast integer literals.
        }

        //--------
    }//class


    [TestFixture]
    public class ElementNumerical_IntegerAllRanges
    {
        enum RangeTest
        {
            i8neg,
            i8,
            u8,
            //--
            i16neg,
            i16,
            u16,
        }


        [Test]
        public void Int8()
        {
            DoTest(ElementType.Int8, true, true);
        }

        [Test]
        public void UInt8()
        {
            DoTest(ElementType.UInt8, false, true, true);
        }

        private void DoTest(ElementType elementType, params bool[] inRange)
        {

            int[] rangeTestValues = { -100, 100, 200, -30000, 30000, 60000, -2000000000, 2000000000 /*, 4000000000*/};
            int TestRangesCount = rangeTestValues.Length;
            //
            bool[] inRangeValid_Complete = new bool[TestRangesCount];
            inRange.CopyTo(inRangeValid_Complete, 0);
            //
            for (int i = 0; i < inRangeValid_Complete.Length; ++i) {
                int value = rangeTestValues[i];
                try {
                    ServiceElement.CreateNumericalServiceElement(elementType, value);
                    if (!inRangeValid_Complete[i]) {
                        Assert.Fail("Invalid value for ServiceElement but didn't fail: '" + elementType + "' i=" + i);
                    }
                } catch (ArgumentOutOfRangeException /*expectedEx*/) {
                    if (inRangeValid_Complete[i]) {
                        Assert.Fail("Valid value for ServiceElement but failed: '" + elementType + "' i=" + i);
                    }
                }
            }
        }

        //private string[] ToString(params int[] rangeTestValues)
        //{
        //    int x = Int32.MaxValue;
        //    string[] rangeTestValuesStrings = new string[rangeTestValues.Length];
        //    rangeTestValuesStrings = Array.ConvertAll<int, string>(rangeTestValues, delegate(int value) {
        //        return value.ToString();
        //    });
        //    return rangeTestValuesStrings;
        //}

    }//class

}