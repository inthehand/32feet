using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

class TestSocketSocketPairFactory : ISocketPairFactory
{
    ITestSocketPair ISocketPairFactory.CreateSocketPair()
    {
        return new TestSocketPair();
    }

    bool ISocketPairFactory.SocketSupportsCloseInt32 { get { return true; } }
    bool ISocketPairFactory.SocketSupportsShutdown { get { return true; } }
}

class TestSocketPair : ITestSocketPair
{
    Socket m_svr;
    ISocketWrapper m_cli;

    public TestSocketPair()
    {
        Create();
    }

    public ISocketWrapper SocketA
    {
        get { return m_cli; }
    }

    public NetworkStream StreamA
    {
        get
        {
            return m_cli.GetStream();
        }
    }

    public Socket SocketB
    {
        get { return m_svr; }
    }

    void ITestSocketPair.PeerSendsData(byte[] data)
    {
        Socket peer = SocketB;
        peer.Send(data);
    }

    void ITestSocketPair.PeerCloses()
    {
        Socket peer = SocketB;
        peer.Close();
    }

    //----
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    internal void Create()
    {
        try {
            Create(AddressFamily.InterNetworkV6);
        } catch {
            Create(AddressFamily.InterNetwork);
        }
    }

    private void Create(AddressFamily af)
    {
        using (Socket lstnr = new Socket(af, SocketType.Stream, ProtocolType.Unspecified)) {
            lstnr.Bind(new IPEndPoint(
                af == AddressFamily.InterNetworkV6 ? IPAddress.IPv6Loopback : IPAddress.Loopback, 0));
            lstnr.Listen(1);
            EndPoint svrEp = lstnr.LocalEndPoint;
            Socket cli = new Socket(svrEp.AddressFamily, lstnr.SocketType, lstnr.ProtocolType);
            cli.Connect(svrEp);
            m_cli = new SocketWrapper(cli);
            m_svr = lstnr.Accept();
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool disposing)
    {
        if (disposing) {
            m_cli.Close();
            m_svr.Close();
        }
    }
}

class SocketWrapper : ISocketWrapper
{
    Socket m_sock;
    NetworkStream m_strm;

    public SocketWrapper(Socket sock)
    {
        m_sock = sock;
    }

    //----
    bool ISocketWrapper.Connected
    {
        get { return m_sock.Connected; }
    }

    NetworkStream ISocketWrapper.GetStream()
    {
        if (m_strm == null) {
            m_strm = new NetworkStream(m_sock, true);
        }
        return m_strm;
    }

    //----

    void ISocketWrapper.Close()
    {
        m_sock.Close();
    }

    void ISocketWrapper.Close(int timeout)
    {
        m_sock.Close(timeout);
    }

    void ISocketWrapper.Shutdown(SocketShutdown how)
    {
        m_sock.Shutdown(how);
    }
}
