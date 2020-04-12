// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.ServiceRecordHelper
// 
// Copyright (c) 2007 Andy Hume
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using InTheHand.Net.Bluetooth.AttributeIds;
using System.Globalization;

namespace InTheHand.Net.Bluetooth.Sdp
{

    /// <summary>
    /// Some useful methods for working with a SDP <see cref="T:InTheHand.Net.Bluetooth.ServiceRecord"/>
    /// including creating and accessing the <see cref="F:InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ProtocolDescriptorList"/>
    /// for an RFCOMM service.
    /// </summary>
    public static class ServiceRecordHelper
    {
        //--------------------------------------------------------------

        /// <summary>
        /// Reads the RFCOMM Channel Number element from the service record.
        /// </summary>
        /// -
        /// <param name="record">The <see cref="T:InTheHand.Net.Bluetooth.ServiceRecord"/>
        /// to search for the element.
        /// </param>
        /// -
        /// <returns>The <see cref="T:InTheHand.Net.Bluetooth.ServiceElement"/>
        /// holding the Channel Number.
        /// or <see langword="null"/> if at the <see cref="F:InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ProtocolDescriptorList"/>
        /// attribute is missing or contains invalid elements.
        /// </returns>
        public static ServiceElement GetRfcommChannelElement(ServiceRecord record)
        {
            return GetChannelElement(record, BluetoothProtocolDescriptorType.Rfcomm);
        }

        /// <summary>
        /// Reads the L2CAP Channel Number element from the service record.
        /// </summary>
        /// -
        /// <param name="record">The <see cref="T:InTheHand.Net.Bluetooth.ServiceRecord"/>
        /// to search for the element.
        /// </param>
        /// -
        /// <returns>The <see cref="T:InTheHand.Net.Bluetooth.ServiceElement"/>
        /// holding the Channel Number.
        /// or <see langword="null"/> if at the <see cref="F:InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ProtocolDescriptorList"/>
        /// attribute is missing or contains invalid elements.
        /// </returns>
        public static ServiceElement GetL2CapChannelElement(ServiceRecord record)
        {
            return GetChannelElement(record, BluetoothProtocolDescriptorType.L2Cap);
        }

        static ServiceElement GetChannelElement(ServiceRecord record, BluetoothProtocolDescriptorType proto)
        {
            if (!record.Contains(UniversalAttributeId.ProtocolDescriptorList)) {
                goto NotFound;
            }
            ServiceAttribute attr = record.GetAttributeById(UniversalAttributeId.ProtocolDescriptorList);

            bool? isSimpleRfcomm;

            return GetChannelElement(attr, proto, out isSimpleRfcomm);
        NotFound:
            return null;
        }

        // TODO GetRfcommChannelElement(ServiceAttribute attr) Could be public -> Tests!
        internal static ServiceElement GetChannelElement(ServiceAttribute attr,
            BluetoothProtocolDescriptorType proto,
            out bool? isSimpleRfcomm)
        {
            if (proto != BluetoothProtocolDescriptorType.L2Cap
                    && proto != BluetoothProtocolDescriptorType.Rfcomm)
                throw new ArgumentException("Can only fetch RFCOMM or L2CAP element.");

            //
            isSimpleRfcomm = true;
            Debug.Assert(attr != null, "attr != null");
            ServiceElement e0 = attr.Value;
            if (e0.ElementType == ElementType.ElementAlternative) {

                Trace.WriteLine("Don't support ElementAlternative ProtocolDescriptorList values.");

                goto NotFound;
            } else if (e0.ElementType != ElementType.ElementSequence) {

                Trace.WriteLine("Bad ProtocolDescriptorList base element.");

                goto NotFound;
            }
            IList<ServiceElement> protoStack = e0.GetValueAsElementList();
            IEnumerator<ServiceElement> etor = protoStack.GetEnumerator();
            ServiceElement layer;
            IList<ServiceElement> layerContent;
            ServiceElement channelElement;
            // -- L2CAP Layer --
            if (!etor.MoveNext()) {

                Trace.WriteLine(string.Format(CultureInfo.InvariantCulture,
                    "Protocol stack truncated before {0}.", "L2CAP"));

                goto NotFound;
            }
            layer = etor.Current; //cast here are for non-Generic version.
            layerContent = layer.GetValueAsElementList();
            if (layerContent[0].GetValueAsUuid() != BluetoothProtocol.L2CapProtocol) {

                Trace.WriteLine(String.Format(CultureInfo.InvariantCulture,
                    "Bad protocol stack, layer {0} is not {1}.", 1, "L2CAP"));
                goto NotFound;
            }
            bool hasPsmEtc = layerContent.Count != 1;
            // Cast for FX1.1 object
            isSimpleRfcomm = (bool)isSimpleRfcomm && !hasPsmEtc;
            if (proto == BluetoothProtocolDescriptorType.L2Cap) {
                if (layerContent.Count < 2) {

                    Trace.WriteLine("L2CAP PSM element was requested but the L2CAP layer in this case hasn't a second element.");

                    goto NotFound;
                }
                channelElement = (ServiceElement)layerContent[1];
                goto Success;
            }
            //
            // -- RFCOMM Layer --
            if (!etor.MoveNext()) {

                Trace.WriteLine(string.Format(CultureInfo.InvariantCulture,
                    "Protocol stack truncated before {0}.", "RFCOMM"));

                goto NotFound;
            }
            layer = etor.Current;
            layerContent = layer.GetValueAsElementList();
            if (layerContent[0].GetValueAsUuid() != BluetoothProtocol.RFCommProtocol) {

                Trace.WriteLine(String.Format(CultureInfo.InvariantCulture,
                    "Bad protocol stack, layer {0} is not {1}.", 2, "RFCOMM"));

                goto NotFound;
            }
            //
            if (layerContent.Count < 2) {

                Trace.WriteLine(String.Format(CultureInfo.InvariantCulture,
                    "Bad protocol stack, layer {0} hasn't a second element.", 2));

                goto NotFound;
            }
            channelElement = (ServiceElement)layerContent[1];
            if (channelElement.ElementType != ElementType.UInt8) {

                Trace.WriteLine(String.Format(CultureInfo.InvariantCulture,
                    "Bad protocol stack, layer {0} is not UInt8.", 2));

                goto NotFound;
            }
            // Success
            //
            // -- Any remaining layer(s) --
            bool extraLayers = etor.MoveNext();
            isSimpleRfcomm = (bool)isSimpleRfcomm && !extraLayers;
        Success:
            //
            return channelElement;
        NotFound:
            isSimpleRfcomm = null;
            return null;
        }

        /// <summary>
        /// Reads the RFCOMM Channel Number value from the service record,
        /// or returns -1 if the element is not present.
        /// </summary>
        /// -
        /// <param name="record">The <see cref="T:InTheHand.Net.Bluetooth.ServiceRecord"/>
        /// to search for the element.
        /// </param>
        /// -
        /// <returns>The Channel Number as an unsigned byte cast to an Int32, 
        /// or -1 if at the <see cref="F:InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ProtocolDescriptorList"/>
        /// attribute is missing or contains invalid elements.
        /// </returns>
        public static int GetRfcommChannelNumber(ServiceRecord record)
        {
            ServiceElement channelElement = GetRfcommChannelElement(record);
            if (channelElement == null) {
                return -1;
            }
            return GetRfcommChannelNumber(channelElement);
        }

        internal static int GetRfcommChannelNumber(ServiceElement channelElement)
        {
            Debug.Assert(channelElement != null, "channelElement != null");
            System.Diagnostics.Debug.Assert(channelElement.ElementType == ElementType.UInt8);
            byte value = (byte)channelElement.Value;
            return value;
        }

        /// <summary>
        /// Reads the L2CAP Channel Number value from the service record,
        /// or returns -1 if the element is not present.
        /// </summary>
        /// -
        /// <param name="record">The <see cref="T:InTheHand.Net.Bluetooth.ServiceRecord"/>
        /// to search for the element.
        /// </param>
        /// -
        /// <returns>The PSM number as an uint16 cast to an Int32, 
        /// or -1 if at the <see cref="F:InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ProtocolDescriptorList"/>
        /// attribute is missing or contains invalid elements.
        /// </returns>
        public static int GetL2CapChannelNumber(ServiceRecord record)
        {
            ServiceElement channelElement = GetL2CapChannelElement(record);
            if (channelElement == null) {
                return -1;
            }
            return GetL2CapChannelNumber(channelElement);
        }

        internal static int GetL2CapChannelNumber(ServiceElement channelElement)
        {
            Debug.Assert(channelElement != null, "channelElement != null");
            System.Diagnostics.Debug.Assert(channelElement.ElementType == ElementType.UInt16);
            var value = (UInt16)channelElement.Value;
            return value;
        }

        /// <summary>
        /// Sets the RFCOMM Channel Number value in the service record.
        /// </summary>
        /// -
        /// <param name="record">The <see cref="T:InTheHand.Net.Bluetooth.ServiceRecord"/>
        /// in which to set the RFCOMM Channel number.
        /// </param>
        /// <param name="channelNumber">The Channel number to set in the record.
        /// </param>
        /// -
        /// <exception cref="T:System.InvalidOperationException">The
        /// <see cref="F:InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ProtocolDescriptorList"/>
        /// attribute is missing or contains invalid elements.
        /// </exception>
        public static void SetRfcommChannelNumber(ServiceRecord record, byte channelNumber)
        {
            ServiceElement channelElement = GetRfcommChannelElement(record);
            if (channelElement == null) {
                throw new InvalidOperationException("ProtocolDescriptorList element does not exist or is not in the RFCOMM format.");
            }
            System.Diagnostics.Debug.Assert(channelElement.ElementType == ElementType.UInt8);
            channelElement.SetValue(channelNumber);
        }

        /// <summary>
        /// Sets the RFCOMM Channel Number value in the service record.
        /// </summary>
        /// -
        /// <remarks>
        /// <para>Note: We use an <see cref="T:System.Int32"/> for the
        /// <paramref name="psm"/> parameter as its natural type <see cref="T:System.UInt16"/> 
        /// in not usable in CLS Compliant interfaces.
        /// </para>
        /// </remarks>
        /// -
        /// <param name="record">The <see cref="T:InTheHand.Net.Bluetooth.ServiceRecord"/>
        /// in which to set the L2CAP PSM value.
        /// </param>
        /// <param name="psm">The PSM value to set in the record.
        /// Note that although the parameter is of type <see cref="T:System.Int32"/> 
        /// the value must actually be in the range of a <see cref="T:System.UInt16"/>,
        /// see the remarks for more information.
        /// </param>
        /// -
        /// <exception cref="T:System.InvalidOperationException">The
        /// <see cref="F:InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ProtocolDescriptorList"/>
        /// attribute is missing or contains invalid elements.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// The PSM must fit in a 16-bit unsigned integer.
        /// </exception>
        public static void SetL2CapPsmNumber(ServiceRecord record, int psm)
        {
            if (psm < 0 || psm > UInt16.MaxValue)
                throw new ArgumentOutOfRangeException("psm", "A PSM is a UInt16 value.");
            var psm16 = checked((UInt16)psm);
            ServiceElement rfcommElement = GetRfcommChannelElement(record);
            if (rfcommElement != null) {
                Debug.WriteLine("Setting L2CAP PSM for a PDL that includes RFCOMM.");
            }
            ServiceElement channelElement = GetChannelElement(record, BluetoothProtocolDescriptorType.L2Cap);
            if (channelElement == null
                    || channelElement.ElementType != ElementType.UInt16) {
                throw new InvalidOperationException("ProtocolDescriptorList element does not exist, is not in the L2CAP format, or it the L2CAP layer has no PSM element.");
            }
            channelElement.SetValue(psm16);
        }

        //--------------------------------------------------------------

        /// <summary>
        /// Creates the data element for the 
        /// <see cref="F:InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ProtocolDescriptorList"/>
        /// attribute in an L2CAP service
        /// </summary>
        /// -
        /// <returns>The new <see cref="T:InTheHand.Net.Bluetooth.ServiceElement"/>.</returns>
        /// -
        /// <remarks>Thus is the following structure:
        /// <code lang="none">
        /// ElementSequence
        ///    ElementSequence
        ///       Uuid16 = L2CAP
        ///       UInt16 = 0      -- The L2CAP PSM Number.
        /// </code>
        /// </remarks>
        public static ServiceElement CreateL2CapProtocolDescriptorList()
        {
            return CreateL2CapProtocolDescriptorListWithUpperLayers();
        }

        /// <summary>
        /// Creates the data element for the 
        /// <see cref="F:InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ProtocolDescriptorList"/>
        /// attribute in an RFCOMM service
        /// </summary>
        /// -
        /// <returns>The new <see cref="T:InTheHand.Net.Bluetooth.ServiceElement"/>.</returns>
        /// -
        /// <remarks>Thus is the following structure:
        /// <code lang="none">
        /// ElementSequence
        ///    ElementSequence
        ///       Uuid16 = L2CAP
        ///    ElementSequence
        ///       Uuid16 = RFCOMM
        ///       UInt8  = 0      -- The RFCOMM Channel Number.
        /// </code>
        /// </remarks>
        public static ServiceElement CreateRfcommProtocolDescriptorList()
        {
            return CreateRfcommProtocolDescriptorListWithUpperLayers();
        }

        /// <summary>
        /// Creates the data element for the 
        /// <see cref="F:InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ProtocolDescriptorList"/>
        /// attribute in an GOEP (i.e. OBEX) service
        /// </summary>
        /// -
        /// <returns>The new <see cref="T:InTheHand.Net.Bluetooth.ServiceElement"/>.</returns>
        /// -
        /// <remarks>Thus is the following structure:
        /// <code lang="none">
        /// ElementSequence
        ///    ElementSequence
        ///       Uuid16 = L2CAP
        ///    ElementSequence
        ///       Uuid16 = RFCOMM
        ///       UInt8  = 0      -- The RFCOMM Channel Number.
        ///    ElementSequence
        ///       Uuid16 = GOEP
        /// </code>
        /// </remarks>
        public static ServiceElement CreateGoepProtocolDescriptorList()
        {
            return CreateRfcommProtocolDescriptorListWithUpperLayers(
               CreatePdlLayer((UInt16)ServiceRecordUtilities.HackProtocolId.Obex));
        }

        private static ServiceElement CreateRfcommProtocolDescriptorListWithUpperLayers(params ServiceElement[] upperLayers)
        {
            IList<ServiceElement> baseChildren = new List<ServiceElement>();
            baseChildren.Add(CreatePdlLayer((UInt16)ServiceRecordUtilities.HackProtocolId.L2Cap));
            baseChildren.Add(CreatePdlLayer((UInt16)ServiceRecordUtilities.HackProtocolId.Rfcomm,
                new ServiceElement(ElementType.UInt8, (byte)0)));
            foreach (ServiceElement nextLayer in upperLayers) {
                if (nextLayer.ElementType != ElementType.ElementSequence) {
                    throw new ArgumentException("Each layer in a ProtocolDescriptorList must be an ElementSequence.");
                }
                baseChildren.Add(nextLayer);
            }//for
            ServiceElement baseElement = new ServiceElement(ElementType.ElementSequence, baseChildren);
            return baseElement;
        }

        /// <summary>
        /// Creates the data element for the 
        /// <see cref="F:InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ProtocolDescriptorList"/>
        /// attribute in an L2CAP service,
        /// with upper layer entries.
        /// </summary>
        /// -
        /// <returns>The new <see cref="T:InTheHand.Net.Bluetooth.ServiceElement"/>.</returns>
        /// -
        /// <remarks>Thus is the following structure at the first layer:
        /// <code lang="none">
        /// ElementSequence
        ///    ElementSequence
        ///       Uuid16 = L2CAP
        ///       UInt16 = 0      -- The L2CAP PSM Number.
        /// </code>
        /// One can add layers above that; remember that all layers are formed
        /// of an ElementSequence.  See the example below.
        /// </remarks>
        /// -
        /// <example>
        /// <code>
        /// var netProtoList = new ServiceElement(ElementType.ElementSequence,
        ///     ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 0x0800),
        ///     ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 0x0806)
        ///     );
        /// var layer1 = new ServiceElement(ElementType.ElementSequence,
        ///     new ServiceElement(ElementType.Uuid16, Uuid16_BnepProto),
        ///     ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 0x0100), //v1.0
        ///     netProtoList
        ///     );
        /// ServiceElement element = ServiceRecordHelper.CreateL2CapProtocolDescriptorListWithUpperLayers(
        ///     layer1);
        /// </code>
        /// </example>
        /// -
        /// <param name="upperLayers">The list of upper layer elements, one per layer.
        /// As an array.
        /// </param>
        public static ServiceElement CreateL2CapProtocolDescriptorListWithUpperLayers(params ServiceElement[] upperLayers)
        {
            IList<ServiceElement> baseChildren = new List<ServiceElement>();
            baseChildren.Add(CreatePdlLayer((UInt16)ServiceRecordUtilities.HackProtocolId.L2Cap,
                new ServiceElement(ElementType.UInt16, (UInt16)0)));
            foreach (ServiceElement nextLayer in upperLayers) {
                if (nextLayer.ElementType != ElementType.ElementSequence) {
                    throw new ArgumentException("Each layer in a ProtocolDescriptorList must be an ElementSequence.");
                }
                baseChildren.Add(nextLayer);
            }//for
            ServiceElement baseElement = new ServiceElement(ElementType.ElementSequence, baseChildren);
            return baseElement;
        }

        private static ServiceElement CreatePdlLayer(UInt16 uuid, params ServiceElement[] data)
        {
            IList<ServiceElement> curSeqChildren;
            ServiceElement curValueElmt, curSeqElmt;
            //
            curSeqChildren = new List<ServiceElement>();
            curValueElmt = new ServiceElement(ElementType.Uuid16, uuid);
            curSeqChildren.Add(curValueElmt);
            foreach (ServiceElement element in data) {
                curSeqChildren.Add(element);
            }
            curSeqElmt = new ServiceElement(ElementType.ElementSequence, curSeqChildren);
            return curSeqElmt;
        }

        //--------------------------------------------------------------

        internal
        static Guid _GetPrimaryServiceClassId(ServiceRecord sr)
        {
            var a = sr.GetAttributeById(UniversalAttributeId.ServiceClassIdList);
            var eL = a.Value;
            var eClassList = eL.GetValueAsElementList();
            var e0 = eClassList[0];
            var classId = e0.GetValueAsUuid();
            return classId;
        }

    }//class
}
