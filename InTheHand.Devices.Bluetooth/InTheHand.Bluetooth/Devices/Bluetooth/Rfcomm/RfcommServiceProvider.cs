using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth.Rfcomm
{
    /// <summary>
    /// Represents an instance of a local RFCOMM service.
    /// </summary>
    public partial class RfcommServiceProvider
    {
        /// <summary>
        /// Gets a RfcommServiceProvider object from an RFCOMM service id.
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public static Task<RfcommServiceProvider> CreateAsync(RfcommServiceId serviceId)
        {
            if (serviceId == null)
                throw new ArgumentNullException("serviceId");

            return DoCreateAsync(serviceId);
        }

        /// <summary>
        /// Begins advertising the SDP attributes.
        /// </summary>
        public void StartAdvertising()
        {
            DoStartAdvertising();
        }

        /// <summary>
        /// Event raised when an incoming connection is received.
        /// </summary>
        public event EventHandler<RfcommConnectionReceivedEventArgs> ConnectionReceived;

        /// <summary>
        /// Stops advertising the SDP attributes.
        /// </summary>
        public void StopAdvertising()
        {
            DoStopAdvertising();
        }
    }

    public sealed class RfcommConnectionReceivedEventArgs : EventArgs
    {
        internal RfcommConnectionReceivedEventArgs(Stream connection)
        {
            Connection = connection;
        }

        public Stream Connection
        {
            get; private set;
        }
    }

}
