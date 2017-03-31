using System;
using NUnit.Framework;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Text;
using System.Diagnostics;


/// <summary>
/// Provide access to the Socket or BluetoothClient etc being tested.
/// </summary>
/// -
/// <remarks>
/// <para>We could have just had these methods on <see cref="T:ITestSocketPair"/>
/// but it make the tests more readable when the operations are immediately visible.
/// </para>
/// </remarks>
public interface ISocketWrapper
{
    bool Connected { get;}
    NetworkStream GetStream();
    //
    void Close();
    void Close(int timeout);
    void Shutdown(SocketShutdown how);
}

public interface ITestSocketPair : IDisposable
{
    ISocketWrapper SocketA { get; }
    System.Net.Sockets.NetworkStream StreamA { get; }
    //
    void PeerSendsData(byte[] data);
    void PeerCloses();
}

public interface ISocketPairFactory
{
    ITestSocketPair CreateSocketPair();
    //
    bool SocketSupportsCloseInt32 { get;}
    bool SocketSupportsShutdown { get;}
}


public class TestConnectedProperty
{
    ISocketPairFactory m_spFactory;
    protected readonly bool m_connectedMayBeFalseEarlier;

    public TestConnectedProperty(ISocketPairFactory factory)
        : this(factory, false)
    {
    }

    public TestConnectedProperty(ISocketPairFactory factory, bool connectedMayBeFalseEarlier)
    {
        m_spFactory = factory;
        m_connectedMayBeFalseEarlier = connectedMayBeFalseEarlier;
    }

    public void RunAll()
    {
        LocalCloseOnNetworkStream();
        LocalCloseOnSocket();
        LocalCloseIntOnSocket();
        LocalShutdownSendOnSocket();
        LocalShutdownReceiveOnSocket();
        LocalShutdownBothOnSocket();
        //
        LocalCloseOnSocketAndRead();
        LocalCloseOnSocketAndWrite();
        //--------
        ReadWhenClosed();
        ReadWhenClosedAfterWrite();
        WriteWhenClosed();
        ReadWhenClosedWhenPendingData();
        //-- async --
        AsyncReadWhenClosed();
        AsyncReadWhenClosedAfterAsyncWrite();
        AsyncWriteWhenClosed();
        AsyncReadWhenClosedWhenPendingData();
        // broken
        WriteWhenClosedAfterRead();
        AsyncWriteWhenClosedAfterAsyncRead();
    }


    [Test]
    public void LocalCloseOnNetworkStream()
    {
        // Local close (NetworkStream).
        using (ITestSocketPair pair = m_spFactory.CreateSocketPair()) {
            ISocketWrapper sock = pair.SocketA;
            NetworkStream strm = sock.GetStream();
            Assert.IsTrue(sock.Connected, "1a");
            strm.Close();
            Assert.IsFalse(sock.Connected, "1b");
        }
    }

    [Test]
    public void LocalCloseOnSocket()
    {
        // Local close (Socket).
        using (ITestSocketPair pair = m_spFactory.CreateSocketPair()) {
            ISocketWrapper sock = pair.SocketA;
            NetworkStream strm = sock.GetStream();
            Assert.IsTrue(sock.Connected, "2a");
            sock.Close();
            Assert.IsFalse(sock.Connected, "2b");
        }
    }

    [Test]
    public void LocalCloseOnSocketAndRead()
    {
        // Local close (Socket).
        using (ITestSocketPair pair = m_spFactory.CreateSocketPair()) {
            ISocketWrapper sock = pair.SocketA;
            NetworkStream strm = sock.GetStream();
            Assert.IsTrue(sock.Connected, "2a");
            sock.Close();
            Assert.IsFalse(sock.Connected, "2b");
            ReadSomeBytesExpectError(strm, typeof(ObjectDisposedException));
        }
    }

    [Test]
    public void LocalCloseOnSocketAndWrite()
    {
        // Local close (Socket).
        using (ITestSocketPair pair = m_spFactory.CreateSocketPair()) {
            ISocketWrapper sock = pair.SocketA;
            NetworkStream strm = sock.GetStream();
            Assert.IsTrue(sock.Connected, "2a");
            sock.Close();
            Assert.IsFalse(sock.Connected, "2b");
            WriteSomeBytesExpectError(strm, typeof(ObjectDisposedException));
        }
    }

    [Test]
    public void LocalCloseIntOnSocket()
    {
        // Local close(int) (Socket).
        if (m_spFactory.SocketSupportsCloseInt32)
            using (ITestSocketPair pair = m_spFactory.CreateSocketPair()) {
                ISocketWrapper sock = pair.SocketA;
                NetworkStream strm = sock.GetStream();
                Assert.IsTrue(sock.Connected, "3a");
                sock.Close(1);
                Thread.Sleep(1200);
                Assert.IsFalse(sock.Connected, "3b");
            }
    }

    [Test]
    public void LocalShutdownSendOnSocket()
    {
        // Local shutdown_Send (Socket).
        if (m_spFactory.SocketSupportsShutdown)
            using (ITestSocketPair pair = m_spFactory.CreateSocketPair()) {
                ISocketWrapper sock = pair.SocketA;
                NetworkStream strm = sock.GetStream();
                Assert.IsTrue(sock.Connected, "4a");
                sock.Shutdown(SocketShutdown.Send);
                Assert.IsFalse(sock.Connected, "4b");
            }
    }

    [Test]
    public void LocalShutdownReceiveOnSocket()
    {
        // Local shutdown_Receive (Socket).
        if (m_spFactory.SocketSupportsShutdown)
            using (ITestSocketPair pair = m_spFactory.CreateSocketPair()) {
                ISocketWrapper sock = pair.SocketA;
                NetworkStream strm = sock.GetStream();
                Assert.IsTrue(sock.Connected, "5a");
                sock.Shutdown(SocketShutdown.Receive);
                Assert.IsFalse(sock.Connected, "5b");
            }
    }

    [Test]
    public void LocalShutdownBothOnSocket()
    {
        // Local shutdown_Both (Socket).
        if (m_spFactory.SocketSupportsShutdown)
            using (ITestSocketPair pair = m_spFactory.CreateSocketPair()) {
                ISocketWrapper sock = pair.SocketA;
                NetworkStream strm = sock.GetStream();
                Assert.IsTrue(sock.Connected, "6a");
                sock.Shutdown(SocketShutdown.Both);
                Assert.IsFalse(sock.Connected, "6b");
            }
    }

    [Test]
    public void ReadWhenClosed()
    {
        // Read when closed (no data pending etc).
        using (ITestSocketPair pair = m_spFactory.CreateSocketPair()) {
            ISocketWrapper sock = pair.SocketA;
            NetworkStream strm = sock.GetStream();
            Assert.IsTrue(sock.Connected, "10a");
            PeerCloses(pair);
            Assert.IsTrue(sock.Connected, "10b");
            // Eek, reading to EoF doesn't set Connected=false!!
            ReadSomeBytesButExpectZeroOf(strm);
            Assert.IsTrue(sock.Connected, "10c");
            ReadSomeBytesButExpectZeroOf(strm);
            Assert.IsTrue(sock.Connected, "10d");
        }
    }

    [Test]
    public void ReadWhenClosedAfterWrite()
    {
        // Read when closed (no data pending etc), after state refreshed by Write.
        using (ITestSocketPair pair = m_spFactory.CreateSocketPair()) {
            ISocketWrapper sock = pair.SocketA;
            NetworkStream strm = sock.GetStream();
            Assert.IsTrue(sock.Connected, "10a");
            PeerCloses(pair);
            if (!m_connectedMayBeFalseEarlier) {
                Assert.IsTrue(sock.Connected, "10b");
                WriteSomeBytes(strm);
            }
            int count = 0;
            while (true) {
                bool didFail = WriteSomeBytesExpectError(strm);
                ++count;
                if (didFail)
                    break;
                Assert.IsTrue(sock.Connected, "11c_" + count);
            }
            Assert.IsFalse(sock.Connected, "10d");
            ReadSomeBytesExpectError(strm);
            Assert.IsFalse(sock.Connected, "10e");
        }
    }

    [Test]
    public void WriteWhenClosed()
    {
        // Write when closed.
        using (ITestSocketPair pair = m_spFactory.CreateSocketPair()) {
            ISocketWrapper sock = pair.SocketA;
            NetworkStream strm = sock.GetStream();
            Assert.IsTrue(sock.Connected, "11a");
            PeerCloses(pair);
            Assert.IsTrue(sock.Connected, "11b");
            if (!m_connectedMayBeFalseEarlier) {
                WriteSomeBytes(strm);
                Assert.IsTrue(sock.Connected, "11c");
            }
            int count = 0;
            while (true) {
                bool didFail = WriteSomeBytesExpectError(strm);
                ++count;
                if (didFail)
                    break;
                Assert.IsTrue(sock.Connected, "11d");
            }
            Assert.IsFalse(sock.Connected, "11d");
        }
    }

    [Test]
    public void WriteWhenClosedAfterRead()
    {
        // Write when closed.
        using (ITestSocketPair pair = m_spFactory.CreateSocketPair()) {
            ISocketWrapper sock = pair.SocketA;
            NetworkStream strm = sock.GetStream();
            Assert.IsTrue(sock.Connected, "11a");
            PeerSendsData(pair);
            PeerCloses(pair);
            Assert.IsTrue(sock.Connected, "11b");
            ReadSomeBytes(strm, 5);
            Assert.IsTrue(sock.Connected, "11c");
            if (!m_connectedMayBeFalseEarlier) {
                WriteSomeBytes(strm);
                Assert.IsTrue(sock.Connected, "11d");
            }
            int count = 0;
            while (true) {
                bool didFail = WriteSomeBytesExpectError(strm);
                ++count;
                if (didFail)
                    break;
                Assert.IsTrue(sock.Connected, "11e_" + count);
            }
            Assert.IsFalse(sock.Connected, "11f");
        }
    }

    [Test]
    public void ReadWhenClosedWhenPendingData()
    {
        // Read when closed when pending data.
        using (ITestSocketPair pair = m_spFactory.CreateSocketPair()) {
            ISocketWrapper sock = pair.SocketA;
            NetworkStream strm = sock.GetStream();
            Assert.IsTrue(sock.Connected, "12a");
            //
            PeerSendsData(pair);
            ReadSomeBytes(strm, 5);
            PeerCloses(pair);
            Assert.IsTrue(sock.Connected, "12b");
            ReadSomeBytes(strm, 5);
            Assert.IsTrue(sock.Connected, "12c");
            // Eek, reading to EoF doesn't set Connected=false!!
            ReadSomeBytesButExpectZeroOf(strm);
            Assert.IsTrue(sock.Connected, "12d");
            ReadSomeBytesButExpectZeroOf(strm);
            Assert.IsTrue(sock.Connected, "12e");
        }
    }

    [Test]
    public void AsyncReadWhenClosed()
    {
        // Read when closed (no data pending etc).
        using (ITestSocketPair pair = m_spFactory.CreateSocketPair()) {
            ISocketWrapper sock = pair.SocketA;
            NetworkStream strm = sock.GetStream();
            Assert.IsTrue(sock.Connected, "10a");
            PeerCloses(pair);
            Assert.IsTrue(sock.Connected, "10b");
            // Eek, reading to EoF doesn't set Connected=false!!
            AsyncReadSomeBytesButExpectZeroOf(strm, sock, true);
            Assert.IsTrue(sock.Connected, "10c");
            AsyncReadSomeBytesButExpectZeroOf(strm, sock, true);
            Assert.IsTrue(sock.Connected, "10d");
        }
    }

    [Test]
    public void AsyncReadWhenClosedAfterAsyncWrite()
    {
        // Read when closed (no data pending etc), when state refreshed by Write.
        using (ITestSocketPair pair = m_spFactory.CreateSocketPair()) {
            ISocketWrapper sock = pair.SocketA;
            NetworkStream strm = sock.GetStream();
            Assert.IsTrue(sock.Connected, "10a");
            PeerCloses(pair);
            Assert.IsTrue(sock.Connected, "10b");
            if (!m_connectedMayBeFalseEarlier) {
                AsyncWriteSomeBytes(strm, sock);
                Assert.IsTrue(sock.Connected, "10c");
            }
            AsyncWriteSomeBytesExpectErrorInBegin(strm, sock);
            Assert.IsFalse(sock.Connected, "10d");
            AsyncReadSomeBytesExpectErrorInBegin(strm, sock);
            Assert.IsFalse(sock.Connected, "10e");
        }
    }

    [Test]
    public void AsyncWriteWhenClosed()
    {
        // Write when closed.
        using (ITestSocketPair pair = m_spFactory.CreateSocketPair()) {
            ISocketWrapper sock = pair.SocketA;
            NetworkStream strm = sock.GetStream();
            Assert.IsTrue(sock.Connected, "11a");
            PeerCloses(pair);
            Assert.IsTrue(sock.Connected, "11b");
            if (!m_connectedMayBeFalseEarlier) {
                AsyncWriteSomeBytes(strm, sock);
                Assert.IsTrue(sock.Connected, "11c");
            }
            bool didFail = AsyncWriteSomeBytesExpectErrorInBegin(strm, sock);
            Assert.IsFalse(sock.Connected, "11d");
        }
    }

    [Test]
    public void AsyncWriteWhenClosedAfterAsyncRead()
    {
        // Write when closed.
        using (ITestSocketPair pair = m_spFactory.CreateSocketPair()) {
            ISocketWrapper sock = pair.SocketA;
            NetworkStream strm = sock.GetStream();
            Assert.IsTrue(sock.Connected, "11a");
            PeerSendsData(pair);
            PeerCloses(pair);
            Assert.IsTrue(sock.Connected, "11b");
            AsyncReadSomeBytes(strm, sock, 5, true);
            Assert.IsTrue(sock.Connected, "11c");
            if (!m_connectedMayBeFalseEarlier) {
                AsyncWriteSomeBytes(strm, sock);
                Assert.IsTrue(sock.Connected, "11c"); //f->t
            }
            bool didFail = AsyncWriteSomeBytesExpectErrorInBegin(strm, sock);
            Assert.IsFalse(sock.Connected, "11d");
        }
    }

    [Test]
    public void AsyncReadWhenClosedWhenPendingData()
    {
        // Read when closed when pending data.
        using (ITestSocketPair pair = m_spFactory.CreateSocketPair()) {
            ISocketWrapper sock = pair.SocketA;
            NetworkStream strm = sock.GetStream();
            Assert.IsTrue(sock.Connected, "12a");
            //
            PeerSendsData(pair);
            ReadSomeBytes(strm, 5);
            PeerCloses(pair);
            Assert.IsTrue(sock.Connected, "12b");
            AsyncReadSomeBytes(strm, sock, 5, true);
            Assert.IsTrue(sock.Connected, "12c");
            // Eek, reading to EoF doesn't set Connected=false!!
            AsyncReadSomeBytesButExpectZeroOf(strm, sock, true);
            Assert.IsTrue(sock.Connected, "12d");
            AsyncReadSomeBytesButExpectZeroOf(strm, sock, true);
            Assert.IsTrue(sock.Connected, "12e");
        }
    }

    //======================================================================
    //----
    private void AsyncWriteSomeBytes(NetworkStream strm, ISocketWrapper sock)
    {
        byte[] data = Encoding.ASCII.GetBytes("ABCDEFHIJK");
        IAsyncResult ar = strm.BeginWrite(data, 0, data.Length, null, null);
        Assert.IsTrue(sock.Connected, "after Begin expected to be Connected");
        strm.EndWrite(ar);
        if (!m_connectedMayBeFalseEarlier) {
            Assert.IsTrue(sock.Connected, "after End expected to be Connected");
        }
    }
    private static void WriteSomeBytes(NetworkStream strm)
    {
        byte[] data = Encoding.ASCII.GetBytes("ABCDEFHIJK");
        strm.Write(data, 0, data.Length);
    }

    private static bool AsyncWriteSomeBytesExpectErrorInBegin(NetworkStream strm, ISocketWrapper sock)
    {
        byte[] data = Encoding.ASCII.GetBytes("MNOPQRSTUV");
        IAsyncResult ar;
        try {
            ar = strm.BeginWrite(data, 0, data.Length, null, null);
        } catch (IOException ioex) {
            Assert.IsInstanceOfType(typeof(SocketException), ioex.InnerException);
            return true;
        }
        Assert.Fail("Expected error in BeginWrite");
        //--
        Assert.IsTrue(sock.Connected, "after Begin expected to be Connected");
        try {
            strm.EndWrite(ar);
            return false;
        } catch (IOException ioex) {
            Assert.IsInstanceOfType(typeof(SocketException), ioex.InnerException);
            return true;
        }
    }

    private static bool WriteSomeBytesExpectError(NetworkStream strm)
    {
        return WriteSomeBytesExpectError(strm, typeof(SocketException));
    }

    private static bool WriteSomeBytesExpectError(NetworkStream strm, Type expectedTypeofInnerException)
    {
        byte[] data = Encoding.ASCII.GetBytes("MNOPQRSTUV");
        try {
            strm.Write(data, 0, data.Length);
            //Debug.Fail("Expected write failure");
            return false; // Didn't fail
        } catch (IOException ioex) {
            Assert.IsInstanceOfType(expectedTypeofInnerException, ioex.InnerException);
            return true;
        }
    }

    //----
    private static void PeerSendsData(ITestSocketPair pair)
    {
        byte[] data = Encoding.ASCII.GetBytes("abcdefhijk");
        pair.PeerSendsData(data);
    }

    private static void PeerCloses(ITestSocketPair pair)
    {
        pair.PeerCloses();
    }

    //----
    private static bool AsyncReadSomeBytesExpectErrorInBegin(NetworkStream strm, ISocketWrapper sock)
    {
        byte[] buf = new byte[7];
        int count = 1;
        IAsyncResult ar;
        try {
            ar = strm.BeginRead(buf, 0, count, null, null);
        } catch (IOException ioex) {
            Assert.IsInstanceOfType(typeof(SocketException), ioex.InnerException);
            return true;
        }
        Assert.Fail("Expected error in BeginRead");
        //--
        Assert.IsTrue(sock.Connected, "after Begin expected to be Connected");
        try {
            strm.EndRead(ar);
            return false;
        } catch (IOException ioex) {
            Assert.IsInstanceOfType(typeof(SocketException), ioex.InnerException);
            return true;
        }
    }

    private static void AsyncReadSomeBytesButExpectZeroOf(NetworkStream strm, ISocketWrapper sock, bool expectedConnectedValue)
    {
        int readLen = AsyncReadSomeBytes_(strm, 1, sock, expectedConnectedValue);
        Assert.AreEqual(0, readLen);
    }


    private static void ReadSomeBytesButExpectZeroOf(Stream strm)
    {
        int readLen = ReadSomeBytes_(strm, 1);
        Assert.AreEqual(0, readLen);
    }

    private static void AsyncReadSomeBytes(Stream strm, ISocketWrapper sock, int count, bool expectedConnectedValue)
    {
        if (true != expectedConnectedValue)
            throw new ArgumentException("expectedConnectedValue NotImplemented"); // FOR
        int readLen = AsyncReadSomeBytes_(strm, count, sock, expectedConnectedValue);
        Assert.AreEqual(count, readLen);
    }

    private static void ReadSomeBytes(Stream strm, int count)
    {
        int readLen = ReadSomeBytes_(strm, count);
        Assert.AreEqual(count, readLen);
    }

    private static void ReadSomeBytesExpectError(Stream strm)
    {
        ReadSomeBytesExpectError(strm, typeof(SocketException));
    }

    private static void ReadSomeBytesExpectError(Stream strm, Type expectedTypeofInnerException)
    {
        try {
            int readLen = ReadSomeBytes_(strm, 1);
            Assert.Fail("Expected error in Read");
        } catch (IOException ioex) {
            Exception inner = ioex.InnerException;
            Assert.IsInstanceOfType(expectedTypeofInnerException, inner);
        }
    }

    private static int AsyncReadSomeBytes_(Stream strm, int count, ISocketWrapper sock, bool expectedConnectedValue)
    {
        byte[] buf = new byte[7];
        IAsyncResult ar = strm.BeginRead(buf, 0, count, null, null);
        Assert.AreEqual(expectedConnectedValue, sock.Connected, "after Begin");
        int readLen = strm.EndRead(ar);
        bool cs = ar.CompletedSynchronously;
        Assert.AreEqual(expectedConnectedValue, sock.Connected, "after End");
        return readLen;
    }

    private static int ReadSomeBytes_(Stream strm, int count)
    {
        byte[] buf = new byte[7];
        int readLen = strm.Read(buf, 0, count);
        return readLen;
    }

}
