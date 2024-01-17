// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.NetworkStream
// 
// Copyright (c) 2018-2024 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;

namespace InTheHand.Net.Sockets
{
    /// <exclude/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class NonSocketNetworkStream : NetworkStream
    {
        /// <exclude/>
        public NonSocketNetworkStream() : base(GetAConnectedSocket(), false) { }

        internal static Socket GetAConnectedSocket()
        {
            Socket s = SocketPair.GetConnectedSocket();
            Debug.Assert(s != null);
            Debug.Assert(s.Connected);
            return s;
        }
    
    }//class

    internal sealed class SocketPair
    {
        Socket m_cli;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields",
            Justification = "m_svr is there to stop the Socket's Finalization.")]
        Socket m_svr;
        static SocketPair m_SocketPair;

        //--------
        internal static Socket GetConnectedSocket()
        {
            // No need for locking etc here, as it's ok to make one or more (not 
            // many hopefully however!)  The socket (is meant!) to be only used on 
            // initialising the base NetworkStream, so it doesn't matter if the 
            // SocketPair is finalized either.  Better to create only one, hence 
            // why we cache it.
            // Careful of a race between accessing it, it becoming null, and the
            // Finalizer running, so keep a reference locally.
            SocketPair sp = m_SocketPair;
            if (sp == null || !sp.Alive)
                m_SocketPair = sp = SocketPair.Create();
            return sp.m_cli;
        }

        //--------
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        internal static SocketPair Create()
        {
            try
            {
                return Create(AddressFamily.InterNetworkV6);
            }
            catch
            {
                return Create(AddressFamily.InterNetwork);
            }
        }

        internal static SocketPair Create(AddressFamily af)
        {
            return new SocketPair(af);
        }

        //--------
        private SocketPair(AddressFamily af)
        {
            using (Socket lstnr = new Socket(af, SocketType.Stream, ProtocolType.Unspecified))
            {
                lstnr.Bind(new IPEndPoint(
                    af == AddressFamily.InterNetworkV6 ? IPAddress.IPv6Loopback : IPAddress.Loopback, 0));
                lstnr.Listen(1);
                EndPoint svrEp = lstnr.LocalEndPoint;
                m_cli = new Socket(svrEp.AddressFamily, lstnr.SocketType, lstnr.ProtocolType);
                m_cli.Connect(svrEp);
                m_svr = lstnr.Accept();
            }
        }

        //--------
        private bool Alive
        {
            get
            {
                return m_cli != null // just for safety, shouldn't occur
                    && m_cli.Connected;
            }
        }
    }
}
