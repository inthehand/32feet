using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;

namespace InTheHand.Net.Tests.Bluetooth.TestBluetoothAddress
{
    [TestFixture]
    public class IFormatterSztn : BtAddrSerialization
    {
        protected override void DoTestRoundTrip(BluetoothAddress obj)
        {
            IFormatter szr = new BinaryFormatter();
            Stream strm = new MemoryStream();
            szr.Serialize(strm, obj);
            //
            strm.Position = 0;
            BluetoothAddress back = (BluetoothAddress)szr.Deserialize(strm);
            Assert.AreEqual(obj, back, "Equals");
            Assert.AreEqual(obj.ToString("C"), back.ToString("C"), "ToString(\"C\")");
        }

        [Test]
        public void OneFormat()
        {
            BluetoothAddress obj = BluetoothAddress.Parse("001122334455");
            //
            IFormatter szr = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
            Stream strm = new MemoryStream();
            szr.Serialize(strm, obj);
            // SZ Format
            const String NewLine = "\r\n";
            String xmlData
                = "<SOAP-ENV:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:SOAP-ENC=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:clr=\"http://schemas.microsoft.com/soap/encoding/clr/1.0\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + NewLine
                + "<SOAP-ENV:Body>" + NewLine
                + "<a1:BluetoothAddress id=\"ref-1\" xmlns:a1=\"http://schemas.microsoft.com/clr/nsassem/InTheHand.Net/[A-F-N]\">" + NewLine
                + "<dataString id=\"ref-3\">001122334455</dataString>" + NewLine
                + "</a1:BluetoothAddress>" + NewLine
                + "</SOAP-ENV:Body>" + NewLine
                + "</SOAP-ENV:Envelope>" + NewLine
                ;
            strm.Position = 0;
            String result = new StreamReader(strm).ReadToEnd();
            Assert.AreEqual(InsertAssemblyFullNameEscaped(xmlData), result, "Equals");
        }

        internal static string InsertAssemblyFullNameEscaped(string value)
        {
            string fn = typeof(BluetoothAddress).Assembly.FullName;
            string fnE = Uri.EscapeDataString(fn);
            //string test = "InTheHand.Net.Personal%2C%20Version%3D3.0.0.0%2C%20Culture%3Dneutral%2C%20PublicKeyToken%3Dnull";
            //Debug.Assert(test == fnE, "weeee");
            string v2 = value.Replace("[A-F-N]", fnE);
            return v2;
        }

    }//class


    [TestFixture]
    public class XmlSztn : BtAddrSerialization
    {
        protected override void DoTestRoundTrip(BluetoothAddress obj)
        {
            XmlSerializer szr = new XmlSerializer(obj.GetType());
            StringWriter wtr = new StringWriter();
            szr.Serialize(wtr, obj);
            //
            StringReader rdr = new StringReader(wtr.ToString());
            BluetoothAddress back = (BluetoothAddress)szr.Deserialize(rdr);
            Assert.AreEqual(obj, back, "Equals");
            Assert.AreEqual(obj.ToString("C"), back.ToString("C"), "ToString(\"C\")");
        }

        [Test]
        public void OneFormat()
        {
            BluetoothAddress obj = BluetoothAddress.Parse("001122334455");
            //
            XmlSerializer szr = new XmlSerializer(obj.GetType());
            StringWriter wtr = new StringWriter();
            szr.Serialize(wtr, obj);
            // SZ Format
            const String NewLine = "\r\n";
            const String XmlHeader = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + NewLine;
            String xmlData
                = XmlHeader
                + "<BluetoothAddress>001122334455</BluetoothAddress>"
                ;
            Assert.AreEqual(xmlData, wtr.ToString(), "Equals");
        }

    }//class


    public abstract class BtAddrSerialization
    {
        protected abstract void DoTestRoundTrip(BluetoothAddress addr);

        [Test]
        public void One()
        {
            DoTestRoundTrip(BluetoothAddress.Parse("001122334455"));
        }

        [Test]
        public void None()
        {
            DoTestRoundTrip(BluetoothAddress.None);
        }

        [Test]
        public void NoneParsed()
        {
            DoTestRoundTrip(BluetoothAddress.Parse("00:00:00:00:00:00"));
        }

    }//class
}
