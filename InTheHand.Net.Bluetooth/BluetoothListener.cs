// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothListener
// 
// Copyright (c) 2003-2024 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Sdp;
using System;

namespace InTheHand.Net.Sockets
{
    /// <summary>
    /// Listens for connections from Bluetooth RFCOMM network clients.
    /// </summary>
    public sealed partial class BluetoothListener : IDisposable
    {
        private readonly IBluetoothListener _bluetoothListener;

        /// <overloads>
        /// Initializes a new instance of the <see cref="BluetoothListener"/> class.
        /// </overloads>
        /// ----
        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothListener"/> class
        /// to listen on the specified service identifier.
        /// </summary>
        /// <param name="service">The Bluetooth service to listen for.</param>
        /// <remarks>
        /// <para>
        /// An SDP record is published on successful <see cref="M:InTheHand.Net.Sockets.BluetoothListener.Start"/>
        /// to advertise the server.
        /// A generic record is created, containing the essential <c>ServiceClassIdList</c>
        /// and <c>ProtocolDescriptorList</c> attributes.  The specified service identifier is
        /// inserted into the former, and the RFCOMM Channel number that the server is
        /// listening on is inserted into the latter.  See the Bluetooth SDP specification
        /// for details on the use and format of SDP records.
        /// </para><para>
        /// If a SDP record with more elements is required, then use
        /// a constructor that takes an SDP record e.g. 
        /// <see cref="M:InTheHand.Net.Sockets.BluetoothListener.#ctor(System.Guid,InTheHand.Net.Bluetooth.ServiceRecord)"/>.
        /// The format of the generic record used here is shown there also.
        /// </para><para>
        /// Call the <see cref="Start"/> 
        /// method to begin listening for incoming connection attempts.
        /// </para>
        /// </remarks>
        public BluetoothListener(Guid service)
        {
#if ANDROID || MONOANDROID
            _bluetoothListener = new AndroidBluetoothListener();
#elif WINDOWS_UWP || WINDOWS10_0_17763_0_OR_GREATER
            _bluetoothListener = new WindowsBluetoothListener();
#elif NET462 || WINDOWS7_0_OR_GREATER
            _bluetoothListener = new Win32BluetoothListener();
#elif IOS || __IOS__ || NETSTANDARD
#else
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    _bluetoothListener = new LinuxBluetoothListener();
                    break;
                case PlatformID.Win32NT:
                    _bluetoothListener = new Win32BluetoothListener();
                    break;
            }
#endif
            if (_bluetoothListener == null)
                throw new PlatformNotSupportedException();

            _bluetoothListener.ServiceUuid = service;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothListener"/> class
        /// to listen on the specified service identifier, 
        /// publishing the specified SDP record.
        /// </summary>
        /// -
        /// <param name="service">The Bluetooth service to listen for.</param>
        /// <param name="sdpRecord">Prepared SDP Record to publish.</param>
        /// -
        /// <remarks>
        /// <note>
        /// The constructors taking the SDP record explicitly should
        /// only be used if
        /// a specialized SDP record is required. For instance when using one of the
        /// standard profiles.  Otherwise use one of the other constructors 
        /// e.g. <see 
        /// cref="M:InTheHand.Net.Sockets.BluetoothListener.#ctor(System.Guid)"/>
        /// which create a generic SDP Record from the specified service identifier.
        /// </note>
        /// <para>Any useful SDP record will include 
        /// a <c>ProtocolDescriptor</c> element containing
        /// the RFCOMM Channel number that the server is listening on,
        /// and a <c>ServiceClassId</c> element containing the service UUIDs.
        /// The record supplied in the <paramref name="sdpRecord"/> parameter
        /// should contain those elements.  On successful <see 
        /// cref="M:InTheHand.Net.Sockets.BluetoothListener.Start"/>,
        /// the RFCOMM Channel number that the protocol stack has assigned to the
        /// server is retrieved, and copied into the service record before it is
        /// published.
        /// </para>
        /// <para>
        /// An example SDP record is as follows.  This is actually the format of the 
        /// generic record used in the other constructors.  For another example see
        /// the code in the <c>ObexListener</c> class.
        /// <code lang="C#">
        /// private static ServiceRecord CreateBasicRfcommRecord(Guid serviceClassUuid)
        /// {
        ///     ServiceElement pdl = ServiceRecordHelper.CreateRfcommProtocolDescriptorList();
        ///     ServiceElement classList = new ServiceElement(ElementType.ElementSequence,
        ///         new ServiceElement(ElementType.Uuid128, serviceClassUuid));
        ///     ServiceRecord record = new ServiceRecord(
        ///         new ServiceAttribute(
        ///             InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ServiceClassIdList,
        ///             classList),
        ///         new ServiceAttribute(
        ///             InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ProtocolDescriptorList,
        ///             pdl));
        ///     return record;
        /// }
        /// </code>
        /// </para>
        /// </remarks>
        public BluetoothListener(Guid service, ServiceRecord sdpRecord) : this(service)
        {
            ServiceRecord = sdpRecord;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Active { get => _bluetoothListener.Active; }

        /// <summary>
        /// Get or set the Service Class flags that this service adds to the host 
        /// device&#x2019;s Class Of Device field.
        /// </summary>
        /// -
        /// <remarks>
        /// <para>The Class of Device value contains a Device part which describes 
        /// the primary service that the device provides, and a Service part which 
        /// is a set of flags indicating all the service types that the device supports, 
        /// e.g. <see cref="F:InTheHand.Net.Bluetooth.ServiceClass.ObjectTransfer"/>,
        /// <see cref="F:InTheHand.Net.Bluetooth.ServiceClass.Telephony"/>,
        /// <see cref="F:InTheHand.Net.Bluetooth.ServiceClass.Audio"/> etc.
        /// This property supports setting those flags; bits set in this value will be 
        /// <strong>added</strong> to the host device&#x2019;s CoD Service Class bits when the listener
        /// is active.  For Win32 see <see href="http://msdn.microsoft.com/en-us/library/aa362940(VS.85).aspx">MSDN &#x2014; BTH_SET_SERVICE Structure</see>
        /// </para>
        /// </remarks>
        public ServiceClass ServiceClass { get => _bluetoothListener.ServiceClass; set => _bluetoothListener.ServiceClass = value; }

        /// <summary>
        /// Get or set the ServiceName the server will use in its SDP Record.
        /// </summary>
        /// -
        /// <value>A string representing the value to be used for the Service Name
        /// SDP Attribute.  Will be <see langword="null"/> if not specfied.
        /// </value>
        /// -
        /// <exception cref="T:System.InvalidOperationException">
        /// The listener is already started.
        /// <para>- or -</para>
        /// A custom Service Record was given at initialization time.  In that case 
        /// the ServiceName attribute should be added to that record.
        /// </exception>
        public string ServiceName { get => _bluetoothListener.ServiceName; set => _bluetoothListener.ServiceName = value; }

        /// <summary>
        /// Returns the SDP Service Record for this service.
        /// </summary>
        /// <remarks>
        /// <note>Returns <see langword="null"/> if the listener is not 
        /// <see cref="M:InTheHand.Net.Sockets.BluetoothListener.Start"/>ed
        /// (or a record wasn&#x2019;t supplied at initialization).
        /// </note>
        /// </remarks>
        public ServiceRecord ServiceRecord { get => _bluetoothListener.ServiceRecord; private set => _bluetoothListener.ServiceRecord = value; }

        /// <summary>
		/// Determines if there is a connection pending.
		/// </summary>
		/// <returns>true if there is a connection pending; otherwise, false.</returns>
		public bool Pending()
        {
            return _bluetoothListener.Pending();
        }

        /// <summary>
        /// Starts listening for incoming connection requests.
        /// </summary>
        public void Start()
        {
            _bluetoothListener.Start();
        }

        /// <summary>
		/// Stops the socket from monitoring connections.
		/// </summary>
		public void Stop()
        {
            _bluetoothListener.Stop();
        }

        /// <summary>
		/// Creates a client object for a connection when the specified service or endpoint is detected by the listener component.
		/// </summary>
		/// <remarks>AcceptBluetoothClient is a blocking method that returns a <see cref="BluetoothClient"/> that you can use to send and receive data.
		/// Use the <see cref="Pending"/> method to determine if connection requests are available in the incoming connection queue if you want to avoid blocking.
		/// <para>Use the <see cref="BluetoothClient.GetStream"/> method to obtain the underlying <see cref="NetworkStream"/> of the returned <see cref="BluetoothClient"/>.
		/// The <see cref="NetworkStream"/> will provide you with methods for sending and receiving with the remote host.
		/// When you are through with the <see cref="BluetoothClient"/>, be sure to call its <see cref="BluetoothClient.Close"/> method.
		/// If you want greater flexibility than a <see cref="BluetoothClient"/> offers, consider using <see cref="AcceptSocket"/>.</para></remarks>
		/// <returns>A <see cref="BluetoothClient"/> component.</returns>
		/// <exception cref="T:System.InvalidOperationException">Listener is stopped.</exception>
		public BluetoothClient AcceptBluetoothClient()
        {
            if (!Active)
                throw new InvalidOperationException("Not listening. You must call the Start() method before calling this method.");

            return _bluetoothListener.AcceptBluetoothClient();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                if (Active)
                    Stop();
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BluetoothListener()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
