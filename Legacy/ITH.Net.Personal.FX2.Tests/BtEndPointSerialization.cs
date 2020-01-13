using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;
using InTheHand.Net.Bluetooth;

namespace InTheHand.Net.Tests.Bluetooth.TestBluetoothEndPoint
{
    [TestFixture]
    public class IFormatterSztn : Serialization
    {
        protected override void DoTestRoundTrip(BluetoothEndPoint obj)
        {
            IFormatter szr = new BinaryFormatter();
            Stream strm = new MemoryStream();
            szr.Serialize(strm, obj);
            //
            strm.Position = 0;
            BluetoothEndPoint back = (BluetoothEndPoint)szr.Deserialize(strm);
            Assert.AreEqual(obj, back, "Equals");
            Assert.AreEqual(obj.Address, back.Address, "Address");
            Assert.AreEqual(obj.Address.ToString("C"), back.Address.ToString("C"), "Address.ToString(\"C\")");
            Assert.AreEqual(obj.Service, back.Service, "Service");
            Assert.AreEqual(obj.Port, back.Port, "Port");
        }

        [Test]
        public void OneFormat()
        {
            BluetoothEndPoint obj = new BluetoothEndPoint(
                BluetoothAddress.Parse("001122334455"), BluetoothService.SerialPort);
            //
            IFormatter szr = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
            Stream strm = new MemoryStream();
            szr.Serialize(strm, obj);
            // SZ Format
            const String NewLine = "\r\n";
            String xmlData
                = "<SOAP-ENV:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:SOAP-ENC=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:clr=\"http://schemas.microsoft.com/soap/encoding/clr/1.0\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + NewLine
                + "<SOAP-ENV:Body>" + NewLine
                + "<a1:BluetoothEndPoint id=\"ref-1\" xmlns:a1=\"http://schemas.microsoft.com/clr/nsassem/InTheHand.Net/[A-F-N]\">" + NewLine
                + "<m_id href=\"#ref-3\"/>" + NewLine
                + "<m_service>" + NewLine
                + "<_a>4353</_a>" + NewLine
                + "<_b>0</_b>" + NewLine
                + "<_c>4096</_c>" + NewLine
                + "<_d>128</_d>" + NewLine
                + "<_e>0</_e>" + NewLine
                + "<_f>0</_f>" + NewLine
                + "<_g>128</_g>" + NewLine
                + "<_h>95</_h>" + NewLine
                + "<_i>155</_i>" + NewLine
                + "<_j>52</_j>" + NewLine
                + "<_k>251</_k>" + NewLine
                + "</m_service>" + NewLine
                + "<m_port>-1</m_port>" + NewLine
                + "</a1:BluetoothEndPoint>" + NewLine
                + "<a1:BluetoothAddress id=\"ref-3\" xmlns:a1=\"http://schemas.microsoft.com/clr/nsassem/InTheHand.Net/[A-F-N]\">" + NewLine
                + "<dataString id=\"ref-4\">001122334455</dataString>" + NewLine
                + "</a1:BluetoothAddress>" + NewLine
                + "</SOAP-ENV:Body>" + NewLine
                + "</SOAP-ENV:Envelope>" + NewLine
                ;
            strm.Position = 0;
            String result = new StreamReader(strm).ReadToEnd();
            string expected = InTheHand.Net.Tests.Bluetooth.TestBluetoothAddress.IFormatterSztn
                .InsertAssemblyFullNameEscaped(xmlData);
            Assert.AreEqual(expected, result, "Equals");
        }

    }//class


    [TestFixture]
    public class XmlSztn : Serialization
    {
        protected override void DoTestRoundTrip(BluetoothEndPoint obj)
        {
            XmlSerializer szr = new XmlSerializer(obj.GetType());
            StringWriter wtr = new StringWriter();
            szr.Serialize(wtr, obj);
            //
            StringReader rdr = new StringReader(wtr.ToString());
            BluetoothEndPoint back = (BluetoothEndPoint)szr.Deserialize(rdr);
            Assert.AreEqual(obj, back, "Equals");
            Assert.AreEqual(obj.Address, back.Address, "Address");
            Assert.AreEqual(obj.Address.ToString("C"), back.Address.ToString("C"), "Address.ToString(\"C\")");
            Assert.AreEqual(obj.Service, back.Service, "Service");
            Assert.AreEqual(obj.Port, back.Port, "Port");
        }

        [Test]
        public void OneFormat()
        {
            BluetoothEndPoint obj = new BluetoothEndPoint(
                BluetoothAddress.Parse("001122334455"), BluetoothService.SerialPort);
            //
            XmlSerializer szr = new XmlSerializer(obj.GetType());
            StringWriter wtr = new StringWriter();
            szr.Serialize(wtr, obj);
            // SZ Format
            const String NewLine = "\r\n";
            const String XmlHeader = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + NewLine;
            String xmlData
                = XmlHeader
                + "<BluetoothEndPoint xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" + NewLine
                + "  <Address>001122334455</Address>" + NewLine
                + "  <Service>00001101-0000-1000-8000-00805f9b34fb</Service>" + NewLine
                + "  <Port>-1</Port>" + NewLine
                + "</BluetoothEndPoint>"
                ;
            Assert.AreEqual(xmlData, wtr.ToString(), "Equals");
        }

    }//class


    public abstract class Serialization
    {
        protected abstract void DoTestRoundTrip(BluetoothEndPoint addr);

        [Test]
        public void One()
        {
            DoTestRoundTrip(new BluetoothEndPoint(
                BluetoothAddress.Parse("001122334455"), BluetoothService.SerialPort));
        }

        [Test]
        public void Two()
        {
            DoTestRoundTrip(new BluetoothEndPoint(
                BluetoothAddress.Parse("FFEEDDCCBBAA"), 
                new Guid("{3906B199-5B79-4326-B18E-B065B211C14E}")));
        }

        [Test]
        public void None()
        {
            DoTestRoundTrip(new BluetoothEndPoint(BluetoothAddress.None, BluetoothService.Empty));
        }

    }//class
}
