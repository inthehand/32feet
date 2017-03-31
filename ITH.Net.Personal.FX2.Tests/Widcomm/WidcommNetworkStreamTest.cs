using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;
using InTheHand.Net.Sockets;
using System.Net.Sockets;

namespace InTheHand.Net.Tests.Widcomm
{
    //[TestFixture]
    public partial class WidcommBluetoothClientCommsTest
    {
        [Test]
        public void NetworkStreamA()
        {
            TestRfcommPort t1;
            BluetoothClient cli;
            Stream t3;
            NetworkStream strm;
            int readLen;
            //
            Create_ConnectedBluetoothClient(out t1, out cli, out t3);
            strm = cli.GetStream();
            // Cause a second NetworkStream to be created, to test SocketPair stuff!
            Create_ConnectedBluetoothClient(out t1, out cli, out t3);
            strm = cli.GetStream();
            //
            Assert.IsTrue(cli.Connected, "isConnected 1");
            Assert.IsTrue(strm.CanRead, "CanRead 1");
            Assert.IsTrue(strm.CanWrite, "CanWrite 1");
            Assert.IsFalse(strm.CanSeek, "CanSeek 1");
            Assert_StreamIsNonSeekable(strm);
            Assert_StreamIsTimeoutable(strm);
            // Ignored
            strm.Flush();
            //
            // Write
            strm.Write(dataA, 0, dataA.Length);
            //
            // Read
            byte[] buf10 = new byte[10];
            t1.NewReceive(dataB9);
            readLen = strm.Read(buf10, 0, buf10.Length);
            Assert.AreEqual(dataB9.Length, readLen, "readLen 1");
            //
            // Read+DataAvailable
            byte[] buf5 = new byte[5];
            Assert.IsFalse(strm.DataAvailable, "DataAvailable 1a");
            t1.NewReceive(dataB9);
            Assert.IsTrue(strm.DataAvailable, "DataAvailable 1b");
            readLen = strm.Read(buf5, 0, buf5.Length);
            Assert.AreEqual(5, readLen, "readLen 1");
            Assert.IsTrue(strm.DataAvailable, "DataAvailable 1c");
            readLen = strm.Read(buf5, 0, buf5.Length);
            Assert.AreEqual(4, readLen, "readLen 1");
            Assert.IsFalse(strm.DataAvailable, "DataAvailable 1d");
            //
            // BeginRead
            byte[] buf5a = new byte[5];
            byte[] buf5b = new byte[5];
            IAsyncResult arRa, arRb;
            int signalledA = 0, signalledB = 0;
            object stateA = new object(), stateB= new object();
            bool? correctStateA = null, correctStateB = null;
            arRa = strm.BeginRead(buf5, 0, buf5.Length, delegate(IAsyncResult arCB) {
                ++signalledA;
                correctStateA = (arCB.AsyncState == stateA);
            }, stateA);
            arRb = strm.BeginRead(buf5, 0, buf5.Length, delegate(IAsyncResult arCB) {
                ++signalledB;
                correctStateB = (arCB.AsyncState == stateB);
            }, stateB);
            Assert.AreEqual(0, signalledA, "signalledA before data");
            Assert.AreEqual(0, signalledB, "signalledB before data");
            t1.NewReceive(dataB9);
            System.Threading.Thread.Sleep(20); // Give time for the callbacks to run.
            Assert.AreEqual(1, signalledA, "signalledA after data");
            Assert.AreEqual(1, signalledB, "signalledB after data");
            Assert.AreEqual(true, correctStateA, "correctStateA");
            Assert.AreEqual(true, correctStateB, "correctStateB");
            readLen = strm.EndRead(arRa);
            Assert.AreEqual(5, readLen, "readLen A");
            readLen = strm.EndRead(arRb);
            Assert.AreEqual(4, readLen, "readLen B");
            Assert.IsFalse(strm.DataAvailable, "DataAvailable 1d");
            //
            // BeginWrite
            IAsyncResult arWa;
            signalledA = 0;
            correctStateA = null;
            System.Threading.ManualResetEvent go = new System.Threading.ManualResetEvent(false);
            ProxyDelegate<IAsyncResult> proxy = new ProxyDelegate<IAsyncResult>(
                delegate(IAsyncResult arCB) {
                    try {
                        strm.EndWrite(arCB);
                        ++signalledA;
                        correctStateA = (arCB.AsyncState == stateA);
                    } finally {
                        Assert.IsTrue(go.Set(), "go.Set()");
                    }
                });
            arWa = strm.BeginWrite(dataA, 0, dataA.Length, proxy.CallbackMethod, stateA);
            Assert.IsTrue(go.WaitOne(1000, false), "go Event");
            Assert.AreEqual(1, signalledA, "signalledA BeginWrite");
            Assert.AreEqual(true, correctStateA, "correctStateA BeginWrite");
            //
            strm.Close();
        }

        // For debugging: can't set breakpoints on anonymous methods.
        class ProxyDelegate<TDelegateParam>
        {
            Action<TDelegateParam> m_callback;

            internal ProxyDelegate(Action<TDelegateParam> callback)
            {
                m_callback = callback;
            }

            public void CallbackMethod(TDelegateParam args)
            {
                m_callback(args);
            }
        }

    }//class
}
