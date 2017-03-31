using System;
using NUnit.Framework;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth.Widcomm;
using System.IO;
using System.Net.Sockets;


namespace InTheHand.Net.Tests.Widcomm
{
    [TestFixture]
    public class WidcommCliConnectedProperty : TestConnectedProperty
    {
        const bool ConnectedMayBeFalseEarlier = true;

        public WidcommCliConnectedProperty()
            : base(new BluetoothClientTestSocketPairFactory(), ConnectedMayBeFalseEarlier)
        {
        }

        public class BluetoothClientWrapper : ISocketWrapper
        {
            BluetoothClient m_cli;

            public BluetoothClientWrapper(BluetoothClient cli)
            {
                m_cli = cli;
            }

            bool ISocketWrapper.Connected
            {
                get { return m_cli.Connected; }
            }

            NetworkStream ISocketWrapper.GetStream()
            {
                return m_cli.GetStream();
            }

            void ISocketWrapper.Close()
            {
                m_cli.Close();
            }

            [Obsolete("NotSupported")]
            void ISocketWrapper.Close(int timeout)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            [Obsolete("NotSupported")]
            void ISocketWrapper.Shutdown(SocketShutdown how)
            {
                throw new Exception("The method or operation is not implemented.");
            }

        }

        public class BluetoothClientTestSocketPairFactory : ISocketPairFactory
        {
            ITestSocketPair ISocketPairFactory.CreateSocketPair()
            {
                return new BluetoothClientTestSocketPair();
            }

            bool ISocketPairFactory.SocketSupportsCloseInt32 { get { return false; } }

            bool ISocketPairFactory.SocketSupportsShutdown { get { return false; } }
        }

        public class BluetoothClientTestSocketPair : ITestSocketPair
        {
            TestRfcommPort m_port;
            ISocketWrapper m_cliA;
            TestRfCommIf m_rfcommIf;

            public BluetoothClientTestSocketPair()
            {
                TestRfCommIf rfcommIf;
                TestRfcommPort port;
                BluetoothClient cli;
                Stream strm2;
                WidcommBluetoothClientCommsTest.Create_ConnectedBluetoothClient(
                    out rfcommIf, out port, out cli, out strm2);
                m_cliA = new BluetoothClientWrapper(cli);
                m_port = port;
                m_rfcommIf = rfcommIf;
            }

            ISocketWrapper ITestSocketPair.SocketA
            {
                get { return m_cliA; }
            }

            NetworkStream ITestSocketPair.StreamA
            {
                get { return m_cliA.GetStream(); }
            }

            void ITestSocketPair.PeerSendsData(byte[] data)
            {
                OneDataEventFirer firer = new OneDataEventFirer(m_port);
                firer.Run(data);
                firer.Complete();
            }

            void ITestSocketPair.PeerCloses()
            {
                OneEventFirer firer = new OneEventFirer(m_port);
                firer.Run(PORT_EV.CONNECT_ERR);
                firer.Complete();
            }

            TestRfcommPort Port
            {
                get { return m_port; }
            }

            public void Dispose()
            {
                m_cliA.Close();
            }

        }//class2
    }
}
