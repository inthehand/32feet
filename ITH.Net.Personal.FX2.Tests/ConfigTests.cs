#if true || PocketPC
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using InTheHand.Net.Bluetooth;
using System.IO;
using System.Xml;

namespace InTheHand.Net.Tests
{
    [TestFixture]
    public class ConfigTests
    {

        static void DoTestNull(string xmlContent)
        {
            DoTest(null, null,
                xmlContent);
        }

        static void DoTest(bool? reportAllErrors, bool? oneStackOnly, string xmlContent)
        {
            BluetoothFactoryConfig.Values v = new BluetoothFactoryConfig.Values();
            using (TextReader rdr = new StringReader(xmlContent)) {
                BluetoothFactoryConfig.LoadManually(rdr, v);
            }
            Assert.AreEqual(oneStackOnly, v.oneStackOnly, "oneStackOnly");
            Assert.AreEqual(reportAllErrors, v.reportAllErrors, "reportAllErrors");
        }

        //--------
        [Test]
        public void RaeTrue()
        {
            DoTest(true, null,
                @"<configuration>
<InTheHand.Net.Personal>
<BluetoothFactory reportAllErrors=""true"" />
</InTheHand.Net.Personal>
</configuration>");
        }

        [Test]
        public void RaeFalse_OpenCloseElementThingy()
        {
            DoTest(false, null,
                @"<configuration>
<InTheHand.Net.Personal>
<BluetoothFactory reportAllErrors=""false"" >
</BluetoothFactory>
</InTheHand.Net.Personal>
</configuration>");
        }

        [Test]
        public void OsoTrue()
        {
            DoTest(null, true,
                @"<configuration>
<InTheHand.Net.Personal>
<BluetoothFactory oneStackOnly=""true"" />
</InTheHand.Net.Personal>
</configuration>");
        }

        [Test]
        public void RaeTrueOsoFalse()
        {
            DoTest(true, false,
                @"<configuration>
<InTheHand.Net.Personal>
<BluetoothFactory reportAllErrors=""true"" oneStackOnly=""false"" />
</InTheHand.Net.Personal>
</configuration>");
        }

        //--
        [Test]
        [ExpectedException(typeof(XmlException))]
        public void EmptyDoc()
        {
            DoTestNull(string.Empty);
        }

        [Test]
        public void DocElemOnly()
        {
            DoTestNull("<configuration/>");
            DoTestNull(@"<configuration>
</configuration>");
        }

        [Test]
        public void NoLeafElement()
        {
            DoTest(null, null,
                @"<configuration>
<InTheHand.Net.Personal>
</InTheHand.Net.Personal>
</configuration>");
        }

    }
}
#endif
