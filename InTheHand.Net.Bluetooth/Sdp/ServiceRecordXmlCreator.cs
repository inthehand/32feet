// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.ServiceRecordXmlCreator
// 
// Copyright (c) 2021 Quamotion bv, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Text;
using System.Xml.Linq;

namespace InTheHand.Net.Bluetooth.Sdp
{
    /// <summary>
    /// Creates an XML representation of a Service Record from the given 
    /// <see cref="T:InTheHand.Net.Bluetooth.ServiceRecord"/> object.
    /// </summary>
    public class ServiceRecordXmlCreator
    {
        /// <summary>
        /// Creates an XML representation of a Service Record from the given 
        /// <see cref="T:InTheHand.Net.Bluetooth.ServiceRecord"/> object.
        /// </summary>
        /// <param name="record">An instance of <see cref="T:InTheHand.Net.Bluetooth.ServiceRecord"/>
        /// containing the record to be created.
        /// </param>
        /// <returns>
        /// A <see cref="XDocument"/> which represents the XML representation of the Service
        /// Record.
        /// </returns>
        public XDocument CreateServiceRecord(ServiceRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            XDocument document = new XDocument();
            var root = new XElement("record");
            document.Add(root);

            foreach (var attribute in record)
            {
                var node = new XElement("attribute");
                node.SetAttributeValue("id", GetIdString(attribute));

                node.Add(GetAttributeValue(attribute.Value));
                root.Add(node);
            }

            return document;
        }

        private static string GetIdString(ServiceAttribute attribute)
        {
            return $"0x{attribute.Id:X}";
        }

        private static XElement GetAttributeValue(ServiceElement value)
        {
            // There isn't much formal documentation on the BlueZ SDP XML format.
            // The actual serialization is implemented in the convert_raw_data_to_xml
            // function at https://github.com/bluez/bluez/blob/9be85f867856195e16c9b94b605f65f6389eda33/src/sdp-xml.c#L637
            switch (value.ElementType)
            {
                case ElementType.Nil:
                    return new XElement("nil");

                case ElementType.Boolean:
                    var boolean = new XElement("boolean");
                    boolean.SetAttributeValue("value", (bool)value.Value ? "true" : "false");
                    return boolean;

                case ElementType.UInt8:
                    var uint8 = new XElement("uint8");
                    uint8.SetAttributeValue("value", $"0x{value.Value:x2}");
                    return uint8;

                case ElementType.UInt16:
                    var uint16 = new XElement("uint16");
                    uint16.SetAttributeValue("value", $"0x{value.Value:x4}");
                    return uint16;

                case ElementType.UInt32:
                    var uint32 = new XElement("uint32");
                    uint32.SetAttributeValue("value", $"0x{value.Value:x8}");
                    return uint32;

                case ElementType.UInt64:
                    var uint64 = new XElement("uint64");
                    uint64.SetAttributeValue("value", $"0x{value.Value:x16}");
                    return uint64;

                case ElementType.Int8:
                    var int8 = new XElement("int8");
                    int8.SetAttributeValue("value", $"{value.Value:d}");
                    return int8;

                case ElementType.Int16:
                    var int16 = new XElement("int16");
                    int16.SetAttributeValue("value", $"{value.Value:d}");
                    return int16;

                case ElementType.Int32:
                    var int32 = new XElement("int32");
                    int32.SetAttributeValue("value", $"{value.Value:d}");
                    return int32;

                case ElementType.Int64:
                    var int64 = new XElement("int64");
                    int64.SetAttributeValue("value", $"{value.Value:d}");
                    return int64;

                case ElementType.Uuid16:
                    var uuid16 = new XElement("uuid");
                    uuid16.SetAttributeValue("value", $"0x{value.Value:x4}");
                    return uuid16;

                case ElementType.Uuid32:
                    var uuid32 = new XElement("uuid");
                    uuid32.SetAttributeValue("value", $"0x{value.Value:x8}");
                    return uuid32;

                case ElementType.Uuid128:
                    var uuid128 = new XElement("uuid");
                    uuid128.SetAttributeValue("value", $"0x{value.Value:D}");
                    return uuid128;

                case ElementType.TextString:
                    var textString = new XElement("text");

                    if (value.Value is byte[])
                    {
                        textString.SetAttributeValue("encoding", "hex");
                        textString.SetAttributeValue("value", GetHexString((byte[])value.Value));
                    }
                    else
                    {
                        textString.SetAttributeValue("value", (string)value.Value);
                    }

                    return textString;

                case ElementType.Url:
                    var url = new XElement("url");
                    url.SetAttributeValue("value", value.Value);
                    return url;

                case ElementType.ElementSequence:
                    var sequence = new XElement("sequence");

                    foreach (var child in value.GetValueAsElementList())
                    {
                        sequence.Add(GetAttributeValue(child));
                    }

                    return sequence;

                case ElementType.ElementAlternative:
                    var alternate = new XElement("alternate");

                    foreach (var child in value.GetValueAsElementList())
                    {
                        alternate.Add(GetAttributeValue(child));
                    }

                    return alternate;

                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }

        private static string GetHexString(byte[] value)
        {
            StringBuilder builder = new StringBuilder(value.Length * 2);

            for (int i = 0; i < value.Length; i++)
            {
                builder.AppendFormat("{0:x2}", value[i]);
            }

            return builder.ToString();
        }
    }
}
