// 32feet.NET - Personal Area Networking for .NET
//
// Copyright (c) 2013 Alan J McFarlane, All rights reserved.
// Copyright (c) 2013 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

#if ANDROID
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Droid;
using InTheHand.Net.Bluetooth.Factory;
using Android.Bluetooth;
using Java.Util;

namespace TtfTests.Droid
{

    [TestFixture]
    public class AndroidConnTests
    {
        class Options : AndroidTesting.Options
        {
        }

        //---------------------
        static readonly BluetoothAddress Addr1 = BluetoothAddress.Parse("002233aaccff");
        static readonly Guid SvcA = BluetoothService.ObexFileTransfer;
        static readonly Guid SvcB = BluetoothService.BasicPrinting;
        const int PortM = 13;

        [Test]
        public void NoConnect_Dispose()
        {
            var state = AndroidTesting.Init(new Options
            {
                Insecure = true,
                GetRemoteDevice = false,
                CreateSocket = false,
                //Connect = false,
                ClientDispose = true,
                ExpectedSvcClass = BluetoothService.ObexObjectPush,
            });
            state.Cli.Dispose();
            //
            state.VerifyAll();
        }

        [Test]
        public void Connect_Get_NoDispose()
        {
            Connect_Get_DisposeOptional(AndroidTesting.Disposal.No);
        }

        [Test]
        public void Connect_Get_DisposeClient()
        {
            Connect_Get_DisposeOptional(AndroidTesting.Disposal.Client);
        }

        [Test]
        public void Connect_Get_DisposeStream()
        {
            Connect_Get_DisposeOptional(AndroidTesting.Disposal.Stream);
        }

        //[Test]
        //public void Connect_Get_Finalizer()
        //{
        //    var state = Connect_Get_DisposeOptional(AndroidTesting.Disposal.Stream);
        //    var vfy = state.Verifier;
        //    state = null;
        //    GC.Collect();
        //    GC.Collect();
        //    GC.WaitForPendingFinalizers();
        //    vfy.VerifyAll();
        //}

        AndroidTesting.TestState Connect_Get_DisposeOptional(AndroidTesting.Disposal disposal)
        {
            var state = AndroidTesting.Init(new Options
            {
                Insecure = true,
                GetRemoteDevice = true,
                CreateSocket = true,
                Connect = true,
                GetStream1 = true,
                ClientDispose = false,
                //ExpectedAddress = Addr1,
                ExpectedSvcClass = SvcA,
                //ConstructParams = new object[] { BluetoothService.ObexObjectPush, },
            });
            var cli = state.Cli;
            cli.Connect(new BluetoothEndPoint(Addr1, SvcA));
            var peer = cli.GetStream();
            //
            switch (disposal) {
                case AndroidTesting.Disposal.No:
                    break;
                case AndroidTesting.Disposal.Client:
                    AndroidTesting.AllowClientClose_WillRunOnFinalizer(cli);
                    cli.Dispose();
                    break;
                case AndroidTesting.Disposal.Stream:
                    AndroidTesting.AllowClientClose_WillRunOnFinalizer(cli);
                    peer.Close();
                    break;
                default:
                    break;
            }
            //
            state.VerifyAll();
            if (disposal == AndroidTesting.Disposal.No) {
                AndroidTesting.AllowClientClose_WillRunOnFinalizer(cli);
            }
            return state;
        }

        [Test]
        public void Connect_NoGet_NoDispose()
        {
            Connect_NoGet_DisposeOptional(AndroidTesting.Disposal.No);
        }

        [Test]
        public void Connect_NoGet_DisposeClient()
        {
            Connect_NoGet_DisposeOptional(AndroidTesting.Disposal.Client);
        }

        void Connect_NoGet_DisposeOptional(AndroidTesting.Disposal disposal)
        {
            var state = AndroidTesting.Init(new Options
            {
                Insecure = true,
                GetRemoteDevice = true,
                CreateSocket = true,
                Connect = true,
                GetStream1 = false,
                ClientDispose = false,
                ExpectedSvcClass = SvcB,
                //ConstructParams = new object[] { BluetoothService.ObexObjectPush, },
            });
            var cli = state.Cli;
            cli.Connect(new BluetoothEndPoint(Addr1, SvcB));
            switch (disposal) {
                case AndroidTesting.Disposal.No:
                    break;
                case AndroidTesting.Disposal.Client:
                    AndroidTesting.AllowClientClose_WillRunOnFinalizer(cli);
                    cli.Dispose();
                    break;
                case AndroidTesting.Disposal.Stream:
                    break;
                default:
                    break;
            }
            //
            state.VerifyAll();
            if (disposal == AndroidTesting.Disposal.No) {
                AndroidTesting.AllowClientClose_WillRunOnFinalizer(cli);
            }
        }

        // TODO [Test] Connect_ToPort_NoGet_NoDispose
        public void Connect_ToPort_NoGet_NoDispose()
        {
            var state = AndroidTesting.Init(new Options
            {
                Insecure = true,
                GetRemoteDevice = true,
                CreateSocket = true,
                Connect = true,
                GetStream1 = true,
                ClientDispose = false,
                //ExpectedAddress = Addr1,
                //ExpectedSvcClass = SvcA,
                //ConstructParams = new object[] { BluetoothService.ObexObjectPush, },
            });
            var cli = state.Cli;
            cli.Connect(new BluetoothEndPoint(Addr1, SvcA, PortM));
            var peer = cli.GetStream();
            //
            state.VerifyAll();
            AndroidTesting.AllowClientClose_WillRunOnFinalizer(cli);
        }

    }
}
#endif
