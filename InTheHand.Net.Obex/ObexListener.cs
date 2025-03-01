// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.ObexListener
// 
// Copyright (c) 2003-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License


using System;
using System.Net;
using System.Net.Sockets;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using InTheHand.Net.Bluetooth.Sdp;
using InTheHand.Net.Bluetooth.AttributeIds;

namespace InTheHand.Net
{
    /// <summary>
    /// Provides a simple, programmatically controlled OBEX protocol listener.
    /// </summary>
    public class ObexListener
    {
        //changed sdp to use the uuid16 of the obexpush service
        private static readonly byte[] ServiceRecordExpected = new byte[] {
            0x35,0x25,0x09,0x00,0x01,0x35,0x03,0x19,
            0x11,0x05,0x09,0x00,0x04,0x35,0x11,0x35,
            0x03,0x19,0x01,0x00,0x35,0x05,0x19,0x00,
            0x03,0x08,0x00,0x35,0x03,0x19,0x00,0x08,
            0x09,0x03,0x03,0x35,0x02,0x08,0xFF};
        private const int ServiceRecordExpectedPortOffset = 26;


        private ObexTransport transport;
#if NO_IRDA
        private readonly object iListener;
#else
        private IrDAListener iListener;
#endif
        private BluetoothListener bListener;
        private TcpListener tListener;

        private volatile bool listening = false;

        /// <overloads>
        /// Initializes a new instance of the ObexListener class.
        /// </overloads>
        /// -
        /// <summary>
        /// Initializes a new instance of the ObexListener class using the Bluetooth transport.
        /// </summary>
        public ObexListener()
            : this(ObexTransport.Bluetooth)
        {
        }
        /// <summary>
        /// Initializes a new instance of the ObexListener class specifiying the transport to use.
        /// </summary>
        /// -
        /// <param name="transport">Specifies the transport protocol to use.
        /// </param>
        public ObexListener(ObexTransport transport)
        { 
            switch (transport)
            {
                case ObexTransport.Bluetooth:
                    ServiceRecord record = CreateServiceRecord();
                    bListener = new BluetoothListener(BluetoothService.ObexObjectPush, record);
                    bListener.ServiceClass = ServiceClass.ObjectTransfer;
                    break;
                case ObexTransport.IrDA:
#if NO_IRDA
                    throw new NotSupportedException("No IrDA on this platform.");
#else
                    iListener = new IrDAListener("OBEX");
                    break;
#endif
                case ObexTransport.Tcp:
                    tListener = new TcpListener(IPAddress.Any, 650);
                    break;
                default:
                    throw new ArgumentException("Invalid transport specified");
            }
            this.transport = transport;
        }

        private static ServiceRecord CreateServiceRecord()
        {
            ServiceElement englishUtf8PrimaryLanguage = CreateEnglishUtf8PrimaryLanguageServiceElement();
            ServiceRecord record = new ServiceRecord(
                new ServiceAttribute(InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ServiceClassIdList,
                    new ServiceElement(ElementType.ElementSequence,
                        new ServiceElement(ElementType.Uuid16, (UInt16)0x1105))),
                new ServiceAttribute(InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ProtocolDescriptorList,
                    ServiceRecordHelper.CreateGoepProtocolDescriptorList()),
#if ADD_SERVICE_NAME_TO_SDP_RECORD
                // Could add ServiceName, ProviderName etc here.
                new ServiceAttribute(InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.LanguageBaseAttributeIdList,
                    englishUtf8PrimaryLanguage),
                new ServiceAttribute(ServiceRecord.CreateLanguageBasedAttributeId(
                        InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ProviderName,
                        LanguageBaseItem.PrimaryLanguageBaseAttributeId),
                    new ServiceElement(ElementType.TextString, "32feet.NET")),
#endif
                //
                new ServiceAttribute(InTheHand.Net.Bluetooth.AttributeIds.ObexAttributeId.SupportedFormatsList,
                    new ServiceElement(ElementType.ElementSequence,
                        new ServiceElement(ElementType.UInt8, (byte)0xFF)))
                );
            return record;
        }

        private static ServiceElement CreateEnglishUtf8PrimaryLanguageServiceElement()
        {
            ServiceElement englishUtf8PrimaryLanguage = LanguageBaseItem.CreateElementSequenceFromList(
                new LanguageBaseItem[] {
                    new LanguageBaseItem("en", LanguageBaseItem.Utf8EncodingId, LanguageBaseItem.PrimaryLanguageBaseAttributeId)
                });
            return englishUtf8PrimaryLanguage;
        }

        internal static void Arrays_Equal(byte[] expected, byte[] actual) // as NETCFv1 not Generic <T>
        {
            if (expected.Length != actual.Length)
            {
                throw new InvalidOperationException("diff lengs!!!");
            }
            for (int i = 0; i < expected.Length; ++i)
            {
                if (!expected[i].Equals(actual[i]))
                {
                    throw new InvalidOperationException(String.Format(System.Globalization.CultureInfo.InvariantCulture,
                        "diff at {0}, x: 0x{1:X2}, y: 0x{2:X2} !!!", i, expected[i], actual[i]));
                }
            }
        }

        //--------------------------------------------------------------

        private bool IsBluetoothListener()
        {
            if (bListener == null)
            { // Non-Bluetooth
                Debug.Assert(tListener != null || iListener != null, "No listener created!");
                return false;
            }
            return true;
        }


        /// <summary>
        /// Gets a value that indicates whether the <see cref="T:InTheHand.Net.ObexListener"/> has been started.
        /// </summary>
        public bool IsListening
        {
            get
            {
                return listening;
            }
        }

        /// <summary>
        /// Allows this instance to receive incoming requests.
        /// </summary>
        public void Start()
        {
            switch (transport)
            {
                case ObexTransport.Bluetooth:
                    bListener.Start();
                    break;
#if !NO_IRDA
                case ObexTransport.IrDA:
                    iListener.Start();
                    break;
#endif
                case ObexTransport.Tcp:
                    tListener.Start();
                    break;
            }

            listening = true;
        }

        /// <summary>
        /// Causes this instance to stop receiving incoming requests.
        /// </summary>
        public void Stop()
        {
            listening = false;
            switch (transport)
            {
                case ObexTransport.Bluetooth:
                    bListener.Stop();
                    break;
#if !NO_IRDA
                case ObexTransport.IrDA:
                    iListener.Stop();
                    break;
#endif
                case ObexTransport.Tcp:
                    tListener.Stop();
                    break;
            }
        }

        /// <summary>
        /// Shuts down the ObexListener.
        /// </summary>
        public void Close()
        {
            if (listening)
            {
                this.Stop();
            }
        }

        /// <summary>
        /// Waits for an incoming request and returns when one is received.
        /// </summary>
        /// -
        /// <remarks>
        /// <para>This method blocks waiting for a new connection.  It will
        /// return when a new connection completes or 
        /// <see cref="M:InTheHand.Net.ObexListener.Stop"/>/<see cref="M:InTheHand.Net.ObexListener.Close"/>
        /// has been called.
        /// </para>
        /// </remarks>
        /// -
        /// <returns>Returns a <see cref="T:InTheHand.Net.ObexListenerContext"/>
        /// or <see langword="null"/> if
        /// <see cref="M:InTheHand.Net.ObexListener.Stop"/>/<see cref="M:InTheHand.Net.ObexListener.Close"/>
        /// has been called.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public ObexListenerContext GetContext()
        {
            if (!listening)
            {
                throw new InvalidOperationException("Listener not started");
            }

            try
            {
                Socket socket;
                Stream stream = null;

                switch (transport)
                {
                    case ObexTransport.Bluetooth:
                        var bluetoothClient = bListener.AcceptBluetoothClient();
                        socket = bListener.AcceptBluetoothClient().Client;
                        if (socket == null)
                        {
                            stream = bluetoothClient.GetStream(); // platforms which don't use System.Net.Sockets can return a stream instead
                        }
                        break;

                    case ObexTransport.IrDA:
#if NO_IRDA
                        throw new NotSupportedException("No IrDA on this platform.");
#else
                        socket = iListener.AcceptIrDAClient().Client;
                        break;
#endif
                    default:
                        socket = tListener.AcceptTcpClient().Client;
                        break;
                }

                if (socket != null)
                {
                    Debug.WriteLine($"Socket {socket.GetHashCode():X8}: Accepted", "ObexListener");
                    return new ObexListenerContext(socket);
                }
                else if (stream != null)
                {
                    Debug.WriteLine($"Stream {stream.GetHashCode():X8}: Accepted", "ObexListener");
                    // call to stream implementation to go here
                    
                }
                
                return null;
            }
            catch
            {
                return null;
            }
        }
        
        public async Task<ObexListenerContext> GetContextAsync()
        {
            if (!listening)
            {
                throw new InvalidOperationException("Listener not started");
            }

            try
            {
                Socket socket;
                Stream stream = null;

                switch (transport)
                {
                    case ObexTransport.Bluetooth:
                        var bluetoothClient = await bListener.AcceptBluetoothClientAsync();
                        socket = (await bListener.AcceptBluetoothClientAsync()).Client;
                        if (socket == null)
                        {
                            stream = bluetoothClient.GetStream(); // platforms which don't use System.Net.Sockets can return a stream instead
                        }
                        break;

                    case ObexTransport.IrDA:
#if NO_IRDA
                        throw new NotSupportedException("No IrDA on this platform.");
#else
                        socket = (await iListener.AcceptIrDAClientAsync(null)).Client;
                        break;
#endif
                    default:
                        socket = (await tListener.AcceptTcpClientAsync()).Client;
                        break;
                }

                if (socket != null)
                {
                    Debug.WriteLine($"Socket {socket.GetHashCode():X8}: Accepted", "ObexListener");
                    return new ObexListenerContext(socket);
                }
                else if (stream != null)
                {
                    Debug.WriteLine($"Stream {stream.GetHashCode():X8}: Accepted", "ObexListener");
                    // call to stream implementation to go here
                    
                }
                
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}