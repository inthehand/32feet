using System;
using System.Collections.Generic;
using System.Text;
using InTheHand.Net.Bluetooth.Factory;
using NUnit.Framework;

namespace InTheHand.Net.Tests.Infra
{
    class ClientTesting
    {
        internal readonly static BluetoothAddress Addr1 = BluetoothAddress.Parse("002233445566");
        internal readonly static byte[] Addr1Bytes_ = { 0x66, 0x55, 0x44, 0x33, 0x22, 0x00 };
        internal const long Addr1Long = 0x002233445566;
        internal const int Port5 = 5;
        internal const uint Port5Uint = Port5;

        //--------
        internal static void SafeWait(IAsyncResult ar)
        {
            const int timeoutMs = 10 * 1000;
            if (ar.IsCompleted) return;
            bool signalled = ar.AsyncWaitHandle.WaitOne(timeoutMs);
        }

        //--------
        internal enum IsConnectedState
        {
            Connected,
            Closed,
            RemoteCloseAndBeforeAnyIOMethod
        }

        internal static void Assert_IsConnected(IsConnectedState expected, CommonRfcommStream conn,
            IBluetoothClient cli,
            string descr)
        {
            switch (expected) {
                case IsConnectedState.Closed:
                    Assert.IsFalse(conn.LiveConnected, "conn.LiveConnected " + descr);
                    Assert.IsFalse(conn.Connected, "conn.Connected " + descr);
                    Assert.IsFalse(cli.Connected, "cli.Connected " + descr);
                    break;
                case IsConnectedState.Connected:
                    Assert.IsTrue(conn.LiveConnected, "conn.LiveConnected " + descr);
                    Assert.IsTrue(conn.Connected, "conn.Connected " + descr);
                    Assert.IsTrue(cli.Connected, "cli.Connected " + descr);
                    break;
                case IsConnectedState.RemoteCloseAndBeforeAnyIOMethod:
                    Assert.IsFalse(conn.LiveConnected, "conn.LiveConnected " + descr);
                    // These two aren't strict through...........
                    Assert.IsTrue(conn.Connected, "conn.Connected " + descr);
                    Assert.IsTrue(cli.Connected, "cli.Connected " + descr);
                    break;
                default:
                    break;
            }
        }
    }
}