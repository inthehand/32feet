using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using NUnit.Framework;
using System.Net;

static class SocketBehaviour
{
    public static void MyMain()
    {
        new TestConnectedProperty(new TestSocketSocketPairFactory()).RunAll();
        TestConnect();
        TestClose();
    }

    public static void TestClose()
    {
        TcpClient cli;
        //
        cli = new TcpClient();
        cli.Connect("www.microsoft.com", 80);
        Stream strm = cli.GetStream();
        byte[] get = Encoding.ASCII.GetBytes("GET / HTTP/1.0\r\nHost: www.microsoft.com\r\nConnection: Close\r\n\r\n");
        strm.Write(get, 0, get.Length);
        Thread.Sleep(5000);
        strm.Close();
        byte[] buf = new byte[1000];
        try {
            strm.Read(buf, 0, buf.Length);
            Assert.Fail("SHaT!");
        } catch (Exception ex) {
            Assert.IsInstanceOfType(typeof(ObjectDisposedException), ex, "ObjectDisposedException");
        }
        try {
            strm.Write(buf, 0, buf.Length);
            Assert.Fail("SHaT!");
        } catch (Exception ex) {
            Assert.IsInstanceOfType(typeof(ObjectDisposedException), ex, "ObjectDisposedException");
        }
    }

    public static void TestConnect()
    {
        IAsyncResult ar1, ar2;
        TcpClient cli;
        IPAddress unaccessableHost = IPAddress.Parse("192.0.99.99");
        //--------------------------------------------------------------
        // Access before Connect.
        cli = new TcpClient();
        Assert.IsFalse(cli.Connected, "Connected");
        Assert.AreEqual(0, cli.Available, "Available");
        try {
            Stream s = cli.GetStream();
            Assert.Fail("SHaT!");
        } catch (Exception ex) {
            Assert.IsInstanceOfType(typeof(InvalidOperationException), ex, "InvalidOperationException and not a sub-type");
        }
        //--------------------------------------------------------------
        // BeginConnect twice -- first INCOMPLETE.
        cli = new TcpClient();
        ar1 = cli.BeginConnect(unaccessableHost, 80, null, null);
        try {
            ar2 = cli.BeginConnect(unaccessableHost, 80, null, null);
            Assert.Fail("SHaT!");
        } catch (Exception ex) {
            // BeginConnect twice: System.InvalidOperationException: BeginConnect cannot be called while another asynchronous operation is in progress on the same Socket.
            //Console.WriteLine("BeginConnect twice: " + FirstLine(ex));
            Assert.IsInstanceOfType(typeof(InvalidOperationException), ex, "InvalidOperationException and not a sub-type");
        }
        // cancel (the timing-out) connect
        try {
            cli.Close();
            cli.EndConnect(ar1);
            Assert.Fail("SHaT!");
        } catch (Exception) {
        }
        //--------------------------------------------------------------
        // BeginConnect twice -- first complete.
        cli = new TcpClient();
        ar1 = cli.BeginConnect("www.microsoft.com", 80, null, null);
        cli.EndConnect(ar1);
        ar2 = cli.BeginConnect("www.microsoft.com", 80, null, null);
        try {
            cli.EndConnect(ar2);
            Assert.Fail("SHaT!");
        } catch (Exception ex) {
            // BeginConnect twice after Begin/End: System.Net.Sockets.SocketException: A connect request was made on an already connected socket            
            //Console.WriteLine("BeginConnect twice after Begin/End: " + FirstLine(ex));
            Assert.IsInstanceOfType(typeof(SocketException), ex);
            SocketException sex = (SocketException)ex;
            Assert.AreEqual((int)SocketError.IsConnected, sex.ErrorCode, "ErrorCode");
        }
        //--------------------------------------------------------------
        // Cancel an INCOMPLETE BeginConnect.
        cli = new TcpClient();
        ar1 = cli.BeginConnect(unaccessableHost, 80, null, null);
        Socket hackClient = cli.Client;
        // cancel (the timing-out) connect
        cli.Close();
        try {
            if (cli.Client == null) {
                // Eeeeeee: Dispose clears the Socket that EndConnect would use!!!
                hackClient.EndConnect(ar1);
            } else {
                cli.EndConnect(ar1);
                Assert.Fail("SHaT!");
            }
        } catch (ObjectDisposedException) {
        }
        // Socket--Cancel an INCOMPLETE BeginConnect.
        Socket sock = newClientSocket();
        ar1 = sock.BeginConnect(unaccessableHost, 80, null, null);
        // cancel (the timing-out) connect
        sock.Close();
        try {
            sock.EndConnect(ar1);
        } catch (ObjectDisposedException) {
        }
        //--------------------------------------------------------------
    }

    private static Socket newClientSocket()
    {
        AddressFamily af = AddressFamily.InterNetwork;
        Socket s = new Socket(af, SocketType.Stream, ProtocolType.Unspecified);
        return s;
    }

    static string FirstLine(Exception ex)
    {
        using (System.IO.StringReader rdr = new System.IO.StringReader(ex.ToString()))
            return rdr.ReadLine();
    }
}

