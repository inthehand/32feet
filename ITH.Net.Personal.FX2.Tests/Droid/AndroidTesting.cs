// 32feet.NET - Personal Area Networking for .NET
//
// Copyright (c) 2013 Alan J McFarlane, All rights reserved.
// Copyright (c) 2013 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

#if ANDROID
using System;
using System.Collections.Generic;
using System.IO;
using Android.Bluetooth;
using InTheHand.Net.Bluetooth.Droid;
using InTheHand.Net.Bluetooth.Factory;
using Java.Util;
using Moq;
using NUnit.Framework;

namespace TtfTests.Droid
{
    class AndroidTesting
    {
        //copied from LsnrTests
        class FooAndroidBthFactory : AndroidBthFactoryBase
        {
            public FooAndroidBthFactory(BluetoothAdapter a)
                : base(a)
            {
            }
        }

        //--
        static Stream DoInputStream(TestState state, bool accessed)
        {
            bool? canCalled = accessed ? (bool?)true : null;
            return DoStream(state, canCalled, null);
        }

        static Stream DoOutputStream(TestState state, bool accessed)
        {
            bool? canCalled = accessed ? (bool?)true : null;
            return DoStream(state, null, canCalled);
        }

        static Stream DoStream(TestState state, bool? canRead, bool? canWrite)
        {
            var sMock = new Mock<Stream>(MockBehavior.Strict);
            state.AddMock(sMock);
            if (canRead.HasValue) {
                sMock.Setup(x => x.CanRead).Returns(canRead.Value);
            }
            if (canWrite.HasValue) {
                sMock.Setup(x => x.CanWrite).Returns(canWrite.Value);
            }
            return sMock.Object;
        }

        //--
        internal class Options
        {
            // Device.Create[...]RfcommSocket
            public bool? Insecure { get; set; }
            // In Connect(...)
            public bool? GetRemoteDevice { get; set; }
            public bool? CreateSocket { get; set; }
            //TODO public int? CreateSocketToPort  { get; set; }            
            public Guid ExpectedSvcClass { get; set; }
            public bool? Connect { get; set; }
            // In GetStream()
            public bool? GetStream1 { get; set; }
            // 
            public bool? ClientDispose { get; set; }
        }

        internal static TestState Init(Options options)
        {
            var state = new TestState();
            //
            var aMock = new Mock<Android.Bluetooth.BluetoothAdapter>(MockBehavior.Strict);
            state.AddMock(aMock);
            //--
            var dMock = new Mock<Android.Bluetooth.BluetoothDevice>(MockBehavior.Strict);
            state.AddMock(dMock);
            //--
            // Socket
            Func<BluetoothSocket> setupSocketConnectStop = delegate()
            {
                var sMock = new Mock<BluetoothSocket>(MockBehavior.Strict);
                state.AddMock(sMock);
                if (GetValueOrReport(() => options.Connect)) {
                    sMock.Setup(x => x.Connect())
                        ;//.Callback(doConnect);
                    if (true) {
                        bool gs = GetValueOrReport(() => options.GetStream1);
                        //bool? gs = gsV ? true : (bool?)null;
                        sMock.Setup(x => x.InputStream).Returns(DoInputStream(state, gs));
                        sMock.Setup(x => x.OutputStream).Returns(DoOutputStream(state, gs));
                    }
                }
                if (GetValueOrReport(() => options.ClientDispose)) {
                    sMock.Setup(x => x.Close()).Callback(() =>
                    {
                    });
                }
                return sMock.Object;
            };
            // CreateSocket
            Moq.Language.Flow.ISetup<BluetoothDevice, BluetoothSocket> setupDeviceCreateSocket;
            if (GetValueOrReport(() => options.CreateSocket)) {
                if (GetValueOrReport(() => options.Insecure)) {
                    setupDeviceCreateSocket = dMock.Setup(x => x.CreateInsecureRfcommSocketToServiceRecord(
                        It.IsAny<UUID>())); // TODO !
                } else {
                    setupDeviceCreateSocket = dMock.Setup(x => x.CreateRfcommSocketToServiceRecord(
                        It.IsAny<UUID>())); // TODO !
                }
                setupDeviceCreateSocket.Returns(setupSocketConnectStop);
            }
            // BluetoothClass
            Func<BluetoothClass> setupDeviceGetClass = delegate
            {
                var cMock = new Mock<BluetoothClass>();
                return cMock.Object;
            };
            // GetRemoteDevice
            //-Func<BluetoothDevice> setupAdapterGetDevice= delegate
            //
            Func<UUID> doUUID = () =>
            {
                var uMock = new Mock<UUID>(Guid.Empty);
                return uMock.Object;
            };
            //----
            //
            // Radio init
            aMock.Setup(x => x.Address).Returns("0000000000aa");
            if (GetValueOrReport(() => options.GetRemoteDevice)) {
                // Device Init
                // TODO Have AndroidBtCli not need BondState/Class
                dMock.Setup(x => x.Address).Returns("0000000000dd");
                //dMock.Setup(x => x.BondState).Returns((Bond)999);
                //dMock.Setup(x => x.BluetoothClass).Returns(setupDeviceGetClass);
                // Adapter init
                aMock.Setup(x => x.GetRemoteDevice(
                                    It.IsAny<string>())) //TODO
                    .Returns(dMock.Object);
            }
            //
            var a = aMock.Object;
            //var f = new FooAndroidBthFactory(a);
            var fMock = new Mock<AndroidBthFactoryBase>(MockBehavior.Loose, a) { CallBase = true, };
            fMock.Setup(x => x.ToJavaUuid(options.ExpectedSvcClass))
                .Returns(doUUID);
            fMock.Setup(x => x.ToJavaUuid(It.IsAny<Guid>()))
#if true
.Throws(new AssertionException("Expected Service Class Guid not found."));
#else
                .Returns(doUUID)
                .Callback((Guid gggg) =>
                {
                });
#endif
            fMock.Setup(x => x.ToJavaUuid(options.ExpectedSvcClass))
                .Returns(doUUID);
            var f = fMock.Object;
            //
            var cli = f.DoGetBluetoothClient();
            state.Cli = cli;
            return state;
        }

        static internal void AllowClientClose_WillRunOnFinalizer(IBluetoothClient cli0)
        {
            AllowClientClose_WillRunOnFinalizer(cli0, true);
        }

        static internal void AllowClientClose_WillRunOnFinalizer(IBluetoothClient cli0, bool disposal)
        {
            var cli = (AndroidBthClient)cli0;
            BluetoothSocket sock; Stream inStream; Stream outStream;
            cli.GetObjectsForTest(out sock, out inStream, out outStream);
            //
            if (disposal) { // We don't close these in the finalizer.
                var isMock = Mock.Get(inStream);
                isMock.Setup(xi => xi.Close()).Callback(() =>
                {
                });
                var osMock = Mock.Get(outStream);
                osMock.Setup(xo => xo.Close()).Callback(() =>
                {
                });
            }
            var sMock = Mock.Get(sock);
            sMock.Setup(xs => xs.Close()).Callback(() =>
            {
            });
        }

        public static bool GetValueOrReport(System.Linq.Expressions.Expression<Func<bool?>> f)
        {
            return AndroidLsnrTests.GetValueOrReport(f);
        }

        //--
        internal class TestState
        {
            public IBluetoothClient Cli { get; set; }
            public IBluetoothListener Lsnr { get; set; }

            List<Mock> _allMocks = new List<Mock>();

            public void AddMock(Mock mock)
            {
                _allMocks.Add(mock);
            }

            public void VerifyAll()
            {
                foreach (var cur in _allMocks) {
                    cur.VerifyAll();
                }
                _allMocks.Clear();
            }
        }

        //---------------------
        internal enum Disposal
        {
            No, Client, Stream
        }

    }
}
#endif
