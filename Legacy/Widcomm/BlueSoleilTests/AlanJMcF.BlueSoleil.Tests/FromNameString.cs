using System;
using System.Text;
using System.Collections.Generic;
#if NUNIT
using NUnit.Framework;
using TestClassAttribute = NUnit.Framework.TestFixtureAttribute;
using TestMethodAttribute = NUnit.Framework.TestAttribute;
using TestContext = System.Version; //dummy
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using InTheHand.Net.Bluetooth.BlueSoleil;
using System.Diagnostics;

namespace InTheHand.Net.Tests.BlueSoleil
{
    /// <summary>
    /// Summary description for FromNameString
    /// </summary>
    [TestClass]
    public class FromNameString
    {
        public FromNameString()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void NoLen_Normal()
        {
            byte[] arr = AppendZeros(ToArray("abcd"), 4);
            Debug.Assert(8 == arr.Length, "!WAS: " + arr.Length);
            string result = BluesoleilUtils.FromNameString(arr);
            Assert.AreEqual("abcd", result);
        }

        [TestMethod]
        public void NoLen_NoPadding()
        {
            byte[] arr = ToArray("abcd");
            Debug.Assert(4 == arr.Length, "!WAS: " + arr.Length);
            string result = BluesoleilUtils.FromNameString(arr);
            Assert.AreEqual("abcd", result);
        }

        [TestMethod]
        public void NoLen_OnePadding()
        {
            byte[] arr = AppendZeros(ToArray("abcd"), 1);
            Debug.Assert(5 == arr.Length, "!WAS: " + arr.Length);
            string result = BluesoleilUtils.FromNameString(arr);
            Assert.AreEqual("abcd", result);
        }

        [TestMethod]
        public void NoLen_ZeroLenArray()
        {
            byte[] arr = { };
            Debug.Assert(0 == arr.Length, "!WAS: " + arr.Length);
            string result = BluesoleilUtils.FromNameString(arr);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void NoLen_OnlyOneZero()
        {
            byte[] arr = { 0 };
            Debug.Assert(1 == arr.Length, "!WAS: " + arr.Length);
            string result = BluesoleilUtils.FromNameString(arr);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void NoLen_OnlyManyZeros()
        {
            byte[] arr = { 0, 0, 0, 0, 0, 0, 0, 0 };
            Debug.Assert(8 == arr.Length, "!WAS: " + arr.Length);
            string result = BluesoleilUtils.FromNameString(arr);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void NoLen_ShortString()
        {
            byte[] arr = AppendZeros(ToArray("a"), 4);
            Debug.Assert(5 == arr.Length, "!WAS: " + arr.Length);
            string result = BluesoleilUtils.FromNameString(arr);
            Assert.AreEqual("a", result);
        }

        [TestMethod]
        public void NoLen_Utf8CharInString()
        {
            byte[] arr = AppendZeros(Prepend(new byte[] { 0xC3, 0x81 }, ToArray("bcd")), 4);
            Debug.Assert(9 == arr.Length, "!WAS: " + arr.Length);
            string result = BluesoleilUtils.FromNameString(arr);
            Assert.AreEqual("\u00C1bcd", result);
        }

        //----
        [TestMethod]
        public void Len_Normal()
        {
            byte[] arr = AppendZeros(ToArray("abcd"), 4);
            Debug.Assert(8 == arr.Length, "!WAS: " + arr.Length);
            string result = BluesoleilUtils.FromNameString(arr, 6);
            Assert.AreEqual("abcd", result);
        }

        //--------
        private static byte[] ToArray(string txt)
        {
            var list = new List<byte>();
            foreach (char ch in txt) {
                byte b = checked((byte)ch);
                list.Add(b);
            }
            var arr = list.ToArray();
            return arr;
        }

        private byte[] AppendZeros(byte[] p, int p_2)
        {
            var all = new byte[p.Length + p_2];
            p.CopyTo(all, 0);
            return all;
        }

        private byte[] Prepend(byte[] p, byte[] p_2)
        {
            var all = new byte[p.Length + p_2.Length];
            Array.Copy(p,   0, all, 0,        p.Length);
            Array.Copy(p_2, 0, all, p.Length, p_2.Length);
            return all;
        }

    }
}
