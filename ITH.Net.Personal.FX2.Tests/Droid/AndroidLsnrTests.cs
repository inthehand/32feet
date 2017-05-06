// 32feet.NET - Personal Area Networking for .NET
//
// Copyright (c) 2013 Alan J McFarlane, All rights reserved.
// Copyright (c) 2013 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

#if ANDROID
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.Bluetooth;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Droid;
using Java.Util;
using Moq;
using NUnit.Framework;
using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net;
using System.Reflection;

namespace TtfTests.Droid
{
    [TestFixture]
    public class AndroidLsnrTests
    {
        class FooAndroidBthFactory : AndroidBthFactoryBase
        {
            public FooAndroidBthFactory(BluetoothAdapter a)
                : base(a)
            {
            }
        }

        //--
        Stream DoInputStream(bool accessed)
        {
            bool? canCalled = accessed ? (bool?)true : null;
            return DoStream(canCalled, null);
        }

        Stream DoOutputStream(bool accessed)
        {
            bool? canCalled = accessed ? (bool?)true : null;
            return DoStream(null, canCalled);
        }

        Stream DoStream(bool? canRead, bool? canWrite)
        {
            var sMock = new Mock<Stream>(MockBehavior.Strict);
            _allMocks.Add(sMock);
            if (canRead.HasValue) {
                sMock.Setup(x => x.CanRead).Returns(canRead.Value);
            }
            if (canWrite.HasValue) {
                sMock.Setup(x => x.CanWrite).Returns(canWrite.Value);
            }
            return sMock.Object;
        }

        //--
        class Options : AndroidTesting.Options
        {
            public string ServiceName { get; set; }
            public bool? ServerInsecure { get; set; }
            public bool? Start { get; set; }
            public bool? Accept { get; set; }
            //public bool? GetStream1 { get; set; }
            public bool? Stop { get; set; }
            public Guid ServerExpectedSvcClass { get; set; }
            public object[] ConstructParams { get; set; }
        }

        AndroidTesting.TestState Init(Options options)
        {
            _allMocks.Clear();
            //
            var aMock = new Mock<Android.Bluetooth.BluetoothAdapter>(MockBehavior.Strict);
            _allMocks.Add(aMock);
            //--
            // Accept
            Func<BluetoothSocket> setupSocketGetStreams = delegate()
            {
                var sMock = new Mock<BluetoothSocket>(MockBehavior.Strict);
                _allMocks.Add(sMock);
                bool gs = GetValueOrReport(() => options.GetStream1);
                //bool? gs = gsV ? true : (bool?)null;
                sMock.Setup(x => x.InputStream).Returns(DoInputStream(gs));
                sMock.Setup(x => x.OutputStream).Returns(DoOutputStream(gs));
                //??sMock.Setup(x => x.Close());
                return sMock.Object;
            };
            // ServerSocket
            Func<BluetoothServerSocket> setupServerSocketAcceptClose = delegate()
            {
                var ssMock = new Mock<BluetoothServerSocket>(MockBehavior.Strict);
                _allMocks.Add(ssMock);
                if (GetValueOrReport(() => options.Accept)) {
                    ssMock.Setup(x => x.Accept()).Returns(setupSocketGetStreams);
                }
                if (GetValueOrReport(() => options.Stop)) {
                    ssMock.Setup(xss => xss.Close()).Callback(() =>
                    {
                    });
                }
                return ssMock.Object;
            };
            // Listen
            Moq.Language.Flow.ISetup<BluetoothAdapter, BluetoothServerSocket> adapterListen;
            if (GetValueOrReport(() => options.Start)) {
                if (GetValueOrReport(() => options.ServerInsecure)) {
                    adapterListen = aMock.Setup(x => x.ListenUsingInsecureRfcommWithServiceRecord(
                        options.ServiceName, It.IsAny<UUID>())); //TODO
                } else {
                    adapterListen = aMock.Setup(x => x.ListenUsingRfcommWithServiceRecord(
                        options.ServiceName, It.IsAny<UUID>())); //TODO
                }
                adapterListen.Returns(setupServerSocketAcceptClose);
            }
            //
            Func<UUID> doUUID = () =>
            {
                var uMock = new Mock<UUID>(Guid.Empty);
                return uMock.Object;
            };
            //----
            // Radio init
            aMock.Setup(x => x.Address).Returns("0000000000aa");
            //
            var a = aMock.Object;
            //var f = new FooAndroidBthFactory(a);
            var fMock = new Mock<AndroidBthFactoryBase>(MockBehavior.Loose, a) { CallBase = true, };
            fMock.Setup(x => x.ToJavaUuid(It.IsAny<Guid>()))
                .Throws(new AssertionException("Expected Service Class Guid not found."));
            fMock.Setup(x => x.ToJavaUuid(options.ServerExpectedSvcClass))
                .Returns(doUUID);
            var f = fMock.Object;
            //
            var lsnr = f.DoGetBluetoothListener();
            // Construct
            //lsnr.Construct(options.ExpectedSvcClass);
            if (options.ConstructParams != null) {
                var types = (from x in options.ConstructParams
                             select x.GetType()).ToArray();
                var mi = typeof(IBluetoothListener).GetMethod("Construct", types);
                mi.Invoke(lsnr, options.ConstructParams);
            }
            return new AndroidTesting.TestState { Lsnr = lsnr };
        }

        private void AllowServerClose(IBluetoothListener lsnr0)
        {
            var lsnr = (AndroidBthListener)lsnr0;
            var ss = lsnr._server;
            var ssMock = Mock.Get(ss);
            ssMock.Setup(x => x.Close()).Callback(() =>
                {
                });
        }

        public static bool GetValueOrReport(System.Linq.Expressions.Expression<Func<bool?>> f)
        {
            var dlgt = f.Compile();
            if (!dlgt().HasValue) {
                var name = ((System.Linq.Expressions.MemberExpression)f.Body).Member.Name;
                throw new ArgumentNullException("Must supply true or false for Option member '"
                    + name + "'.");
            }
            return dlgt().Value;
        }

        //--
        List<Mock> _allMocks = new List<Mock>();

        private void VerifyAll()
        {
            foreach (var cur in _allMocks) {
                cur.VerifyAll();
            }
            _allMocks.Clear();
        }

        //--

        // TODO ? Listen... + Accept return null.
        // TODO ? Listen... + Accept error.

        [Test]
        public void NoAccept_LocalEndPoint_Stop()
        {
            var state = Init(new Options
            {
                ServerInsecure = true,
                Start = true,
                Accept = false,
                Stop = true,
                ServerExpectedSvcClass = BluetoothService.ObexObjectPush,
                ConstructParams = new object[] { BluetoothService.ObexObjectPush, },
            });
            var lsnr = state.Lsnr;
            lsnr.Start();
            var lep = lsnr.LocalEndPoint;
            //
            Assert.AreEqual(BluetoothAddress.None, lep.Address, "lep.Address");
            Assert.AreEqual(BluetoothService.Empty, lep.Service, "lep.Service");
            //Assert.IsTrue(lep.HasPort, "lep.HasPort");
            var dontKnowScn = 0;
            Assert.AreEqual(dontKnowScn, lep.Port, "lep.Port");
            //
            lsnr.Stop();
            //
            VerifyAll();
        }

        [Test]
        public void Accept1_NoStop()
        {
            var state = Init(new Options
            {
                ServerInsecure = true,
                Start = true,
                Accept = true,
                GetStream1 = true,
                Stop = false,
                ServerExpectedSvcClass = BluetoothService.ObexObjectPush,
                ConstructParams = new object[] { BluetoothService.ObexObjectPush, },
            });
            var lsnr = state.Lsnr;
            lsnr.Start();
            var cli = lsnr.AcceptBluetoothClient();
            var peer = cli.GetStream();
            //
            VerifyAll();
            AndroidTesting.AllowClientClose_WillRunOnFinalizer(cli);
            AllowServerClose(lsnr);
        }

        [Test]
        public void Accept1_EndPoint()
        {
            var state = Init(new Options
            {
                ServerInsecure = true,
                Start = true,
                Accept = true,
                GetStream1 = true,
                Stop = true,
                ServerExpectedSvcClass = BluetoothService.SerialPort,
                ConstructParams = new object[] {
                    new BluetoothEndPoint(BluetoothAddress.None,
                        BluetoothService.SerialPort), },
            });
            var lsnr = state.Lsnr;
            lsnr.Start();
            var cli = lsnr.AcceptBluetoothClient();
            var peer = cli.GetStream();
            lsnr.Stop();
            //
            VerifyAll();
            AndroidTesting.AllowClientClose_WillRunOnFinalizer(cli);
        }

        [Test]
        public void Accept1()
        {
            var state = Init(new Options
            {
                ServerInsecure = true,
                Start = true,
                Accept = true,
                GetStream1 = true,
                Stop = true,
                ServerExpectedSvcClass = BluetoothService.ObexFileTransfer,
                ConstructParams = new object[] { BluetoothService.ObexFileTransfer, },
            });
            var lsnr = state.Lsnr;
            lsnr.Start();
            var cli = lsnr.AcceptBluetoothClient();
            var peer = cli.GetStream();
            lsnr.Stop();
            //
            VerifyAll();
            AndroidTesting.AllowClientClose_WillRunOnFinalizer(cli);
        }

        [Test]
        public void Accept1_NoGet()//copied from above
        {
            var state = Init(new Options
            {
                ServerInsecure = true,
                Start = true,
                Accept = true,
                GetStream1 = false,
                Stop = true,
                ServerExpectedSvcClass = BluetoothService.ObexFileTransfer,
                ConstructParams = new object[] { BluetoothService.ObexFileTransfer, },
            });
            var lsnr = state.Lsnr;
            lsnr.Start();
            var cli = lsnr.AcceptBluetoothClient();
            //var peer = cli.GetStream();
            lsnr.Stop();
            //
            VerifyAll();
            AndroidTesting.AllowClientClose_WillRunOnFinalizer(cli);
        }

        //TODO [Test] StartStopStart_Accept1_AndStop need to mark only second socket having Accept
        public void StartStopStart_Accept1_AndStop()
        {
            var state = Init(new Options
            {
                ServerInsecure = true,
                Start = true,
                Accept = true,
                //TO-DO AcceptOnNumber = 2,
                GetStream1 = true,
                Stop = true,
                ServerExpectedSvcClass = BluetoothService.ObexFileTransfer,
                ConstructParams = new object[] { BluetoothService.ObexFileTransfer, },
            });
            var lsnr = state.Lsnr;
            lsnr.Start();
            lsnr.Stop();
            lsnr.Start();
            var cli = lsnr.AcceptBluetoothClient();
            var peer = cli.GetStream();
            //
            lsnr.Stop();
            //
            VerifyAll();
        }

        [Test]
        public void Accept1_AndFinalize()
        {
            {
                var state = Init(new Options
                {
                    ServerInsecure = true,
                    Start = true,
                    Accept = true,
                    GetStream1 = true,
                    Stop = true,
                    ServerExpectedSvcClass = BluetoothService.ObexFileTransfer,
                    ConstructParams = new object[] { BluetoothService.ObexFileTransfer, },
                });
                var lsnr = state.Lsnr;
                lsnr.Start();
                var cli = lsnr.AcceptBluetoothClient();
                var peer = cli.GetStream();
                //
                AndroidTesting.AllowClientClose_WillRunOnFinalizer(cli, false);
                cli = null;
                peer = null;
                lsnr = null;
                state = null;
            }
            GC.Collect();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            //
            VerifyAll();
        }

        //[Test]
        //TODO verify count calls public void Accept3_AndStop()
        //{
        //    var lsnr = Init(new Options
        //    {
        //        Insecure = true,
        //        Start = true,
        //        Accept = true,
        //        Stop = true,
        //        ExpectedSvcClass = BluetoothService.ObexObjectPush,
        //        ConstructParams = new object[] { BluetoothService.ObexObjectPush, },
        //    });
        //    lsnr.Start();
        //    {
        //        var cli = lsnr.AcceptBluetoothClient();
        //        var peer = cli.GetStream();
        //    }
        //    {
        //        var cli = lsnr.AcceptBluetoothClient();
        //        var peer = cli.GetStream();
        //    }
        //    {
        //        var cli = lsnr.AcceptBluetoothClient();
        //        var peer = cli.GetStream();
        //    }
        //    lsnr.Stop();
        //    //
        //    VerifyAll();
        //}

        //--
        [Test]
        public void Auth_CheckSecure()
        {
            var state = Init(new Options
            {
                Start = true,
                ServerInsecure = false,
                Accept = false,
                Stop = true,
                ConstructParams = new object[] { BluetoothService.ObexObjectPush, },
                ServerExpectedSvcClass = BluetoothService.ObexObjectPush,
            });
            var lsnr = state.Lsnr;
            lsnr.Authenticate = true;
            lsnr.Start();
            lsnr.Stop();
            //
            VerifyAll();
        }

        [Test]
        public void Encr_CheckSecure()
        {
            var state = Init(new Options
            {
                Start = true,
                ServerInsecure = false,
                Accept = false,
                Stop = true,
                ConstructParams = new object[] { BluetoothService.ObexObjectPush, },
                ServerExpectedSvcClass = BluetoothService.ObexObjectPush,
            });
            var lsnr = state.Lsnr;
            lsnr.Encrypt = true;
            lsnr.Start();
            lsnr.Stop();
            //
            VerifyAll();
        }

        [Test]
        public void NoStart()
        {
            var state = Init(new Options
            {
                Start = false,
                ConstructParams = new object[] { BluetoothService.ObexObjectPush, },
                ServerExpectedSvcClass = BluetoothService.ObexObjectPush,
            });
            //
            VerifyAll();
        }

        [Test]
        public void SvcName1()
        {
            var state = Init(new Options
            {
                ServiceName = "SvcName1",
                Start = true,
                ServerInsecure = true,
                Accept = false,
                Stop = true,
                ConstructParams = new object[] { BluetoothService.ObexObjectPush, },
                ServerExpectedSvcClass = BluetoothService.ObexObjectPush,
            });
            var lsnr = state.Lsnr;
            lsnr.ServiceName = "SvcName1";
            lsnr.Start();
            lsnr.Stop();
            //
            VerifyAll();
        }

        [Test]
        public void SvcName2()
        {
            var state = Init(new Options
            {
                ServiceName = "2Svc  Name2",
                Start = true,
                ServerInsecure = true,
                Accept = false,
                Stop = true,
                ConstructParams = new object[] { BluetoothService.ObexObjectPush, },
                ServerExpectedSvcClass = BluetoothService.ObexObjectPush,
            });
            var lsnr = state.Lsnr;
            lsnr.ServiceName = "2Svc  Name2";
            lsnr.Start();
            lsnr.Stop();
            //
            VerifyAll();
        }

        [Test]
        public void ServiceRecord_NotSupported_NoStop()
        {
            try {
                var state = Init(new Options
                {
                    ServerInsecure = true,
                    ConstructParams = new object[] { BluetoothService.ObexObjectPush, new ServiceRecord() },
                    Start = false,
                });
            } catch (TargetInvocationException tiex) {
                var ex = tiex.InnerException;
                Assert.IsInstanceOfType(typeof(NotSupportedException), ex);
            } finally {
                VerifyAll();
            }
        }

    }
}
#endif
