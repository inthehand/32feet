// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.IrDAListener
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Net;
using System.Net.Sockets;

namespace InTheHand.Net.Sockets
{
	/// <summary>
	/// Places a socket in a listening state to monitor infrared connections from a specified service or network address.
	/// </summary>
    /// <remarks>This class monitors a service by specifying a service name or a network address.
    /// The listener does not listen until you call one of the <see cref="M:InTheHand.Net.Sockets.IrDAListener.Start"/>
    /// methods.</remarks>
    /// <seealso cref="T:System.Net.Sockets.IrDAListener"/>
#if CODE_ANALYSIS
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
#endif
    public class IrDAListener
	{
		private readonly IrDAEndPoint serverEP;

        /// <summary>
		/// Initializes a new instance of the <see cref="IrDAListener"/> class.
		/// </summary>
		/// <param name="ep">The network address to monitor for making a connection.</param>
		public IrDAListener(IrDAEndPoint ep)
		{
            Initialize();
            serverEP = ep;
		}

		/// <summary>
        /// Initializes a new instance of the <see cref="IrDAListener"/> class.
		/// </summary>
		/// <param name="service">The name of the service to listen for.</param>
		public IrDAListener(string service)
		{
            Initialize();
			serverEP = new IrDAEndPoint(IrDAAddress.None, service);
        }

        private void Initialize()
        {
            Active = false;
            Server = new Socket(AddressFamily.Irda, SocketType.Stream, ProtocolType.IP);
        }

        /// <summary>
        /// Gets the underlying network <see cref="Socket"/>.
        /// </summary>
        public Socket Server { get; private set; }

        /// <summary>
		/// Gets a value that indicates whether the <see cref="IrDAListener"/> is actively listening for client connections.
		/// </summary>
		public bool Active { get; private set; }

        /// <summary>
		/// Gets an <see cref="IrDAEndPoint"/> representing the local device.
		/// </summary>
		public IrDAEndPoint LocalEndpoint
		{
			get
			{
				if (Server != null)
				{
                    return (IrDAEndPoint)Server.LocalEndPoint;
				}

				return serverEP;
			}
		}

        /// <summary>
        /// Starts listening for incoming connection requests.
        /// </summary>
        public void Start()
        {
            Start(int.MaxValue);
        }

        /// <summary>
        /// Starts listening for incoming connection requests with a maximum number of pending connection.
        /// </summary>
        /// <param name="backlog">The maximum length of the pending connections queue.</param>
        public void Start(int backlog)
        {
            if ((backlog > int.MaxValue) || (backlog < 0))
            {
                throw new ArgumentOutOfRangeException("backlog");
            }

            if (Server == null)
            {
                throw new InvalidOperationException("The socket handle is not valid.");
            }

            if (!Active)
            {
                Server.Bind(serverEP);
                Server.Listen(backlog);
                Active = true;
            }
        }

        /// <summary>
		/// Stops the socket from monitoring connections.
		/// </summary>
		public void Stop()
		{
            if (Server != null)
            {
                Server.Close();
                Server = null;
            }

            Initialize();
        }

        /// <summary>
		/// Creates a new socket for a connection.
		/// </summary>
		/// <returns>A socket.</returns>
		public Socket AcceptSocket()
		{
            if (!Active)
            {
                throw new InvalidOperationException("Not listening. You must call the Start() method before calling this method.");
            }

			return Server.Accept();
		}

		/// <summary>
		/// Creates a client object for a connection when the specified service or endpoint is detected by the listener component.
		/// </summary>
		/// <returns>An <see cref="IrDAClient"/> object.</returns>
		public IrDAClient AcceptIrDAClient()
		{
			Socket s = AcceptSocket();
			return new IrDAClient(s);
        }

        /// <summary>
        /// Begins an asynchronous operation to accept an incoming connection attempt.
        /// </summary>
        /// -
        /// <param name="callback">An <see cref="AsyncCallback"/> delegate that references the method to invoke when the operation is complete.</param>
        /// <param name="state">A user-defined object containing information about the accept operation.
        /// This object is passed to the callback delegate when the operation is complete.</param>
        /// -
        /// <returns>An <see cref="IAsyncResult"/> that references the asynchronous creation of the <see cref="Socket"/>.</returns>
        /// -
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="Socket"/> has been closed.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId
= "0#callback")]
        public IAsyncResult BeginAcceptSocket(AsyncCallback callback, object state)
        {
            if (!Active)
            {
                throw new InvalidOperationException("Not listening. You must call the Start() method before calling this method.");
            }

            return Server.BeginAccept(callback, state);
        }

        /// <summary>
        /// Asynchronously accepts an incoming connection attempt and creates a new <see cref="Socket"/> to handle remote host communication.
        /// </summary>
        /// <param name="asyncResult">An <see cref="IAsyncResult"/> returned by a call to the <see cref="BeginAcceptSocket"/> method.</param>
        /// <returns>A <see cref="Socket"/>.</returns>
        public Socket EndAcceptSocket(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
            {
                throw new ArgumentNullException("asyncResult");
            }
            return Server.EndAccept(asyncResult);
        }

        public System.Threading.Tasks.Task<Socket> AcceptSocketAsync(object state)
        {
            return System.Threading.Tasks.Task.Factory.FromAsync<Socket>(
                BeginAcceptSocket, EndAcceptSocket,
                state);
        }

        /// <summary>
        /// Begins an asynchronous operation to accept an incoming connection attempt.
        /// </summary>
        /// -
        /// <param name="callback">An <see cref="AsyncCallback"/> delegate that references the method to invoke when the operation is complete.</param>
        /// <param name="state">A user-defined object containing information about the accept operation.
        /// This object is passed to the callback delegate when the operation is complete.</param>
        /// -
        /// <returns>An <see cref="T:System.IAsyncResult"/> that represents the 
        /// asynchronous accept, which could still be pending.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId 
= "0#callback")]
        public IAsyncResult BeginAcceptIrDAClient(AsyncCallback callback, object state)
        {
            if (!Active)
            {
                throw new InvalidOperationException("Not listening. You must call the Start() method before calling this method.");
            }

            return Server.BeginAccept(callback, state);
        }

        /// <summary>
        /// Asynchronously accepts an incoming connection attempt and creates a new <see cref="IrDAClient"/> to handle remote host communication.
        /// </summary>
        /// <param name="asyncResult">An <see cref="IAsyncResult"/> returned by a call to the <see cref="BeginAcceptIrDAClient"/> method.</param>
        /// <returns>An <see cref="IrDAClient"/>.</returns>
        public IrDAClient EndAcceptIrDAClient(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
            {
                throw new ArgumentNullException("asyncResult");
            }

            Socket s = Server.EndAccept(asyncResult);
            return new IrDAClient(s);
        }

        public System.Threading.Tasks.Task<IrDAClient> AcceptIrDAClientAsync(object state)
        {
            return System.Threading.Tasks.Task.Factory.FromAsync<IrDAClient>(
                BeginAcceptIrDAClient, EndAcceptIrDAClient,
                state);
        }

        /// <summary>
		/// Determines if a connection is pending.
		/// </summary>
		/// <returns>true if there is a connection pending; otherwise, false.</returns>
		public bool Pending()
		{
            if (!Active)
            {
                throw new InvalidOperationException("Not listening. You must call the Start() method before calling this method.");
            }

			return Server.Poll(0, SelectMode.SelectRead);
        }
    }
}
