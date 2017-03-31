#if FX4
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using InTheHand.Net.Sockets;

namespace InTheHand.Net.Tests
{
    [TestFixture]
    public class TapTests
    {
        [Test]
        public void BtCliConnect()
        {
            var cli = new BluetoothClient();
            BluetoothEndPoint epNull = null;
            try {
                var task = cli.ConnectAsync(epNull, null);
                Assert.Fail("expected exception");
            } catch (ArgumentException) {
            }
        }

        [Test]
        public void L2capCliConnect()
        {
            var cli = new L2CapClient();
            BluetoothEndPoint epNull = null;
            try {
                var task = cli.ConnectAsync(epNull, null);
                Assert.Fail("expected exception");
            } catch (ArgumentException) {
            }
        }

        [Test]
        public void IrdaCliConnect()
        {
            var cli = new IrDAClient();
            IrDAEndPoint epNull = null;
            try {
                var task = cli.ConnectAsync(epNull, null);
                Assert.Fail("expected exception");
            } catch (ArgumentException) {
            }
        }

        //[Test]
        public void BtLsnrAccept2_None()
        {
            // No usage exceptions
            var lsnr = new BluetoothListener(Guid.Empty);
            var task = lsnr.AcceptBluetoothClientAsync(null);
            var task2 = lsnr.AcceptSocketAsync(null);
        }

        //[Test]
        public void L2capLsnrAccept2_None()
        {
            // No usage exceptions
            var lsnr = new L2CapListener(Guid.Empty);
            var task = lsnr.AcceptClientAsync(null);
        }

        //[Test]
        public void IrdaLsnrAccept2_None()
        {
            // No usage exceptions
            var lsnr = new IrDAListener("unit-test");
            var task = lsnr.AcceptIrDAClientAsync(null);
            var task2 = lsnr.AcceptSocketAsync(null);
        }

        //[Test]
        public void BtDevGsr_None()
        {
            // No usage exceptions
            var dev = new BluetoothDeviceInfo(BluetoothAddress.Parse("002233445566"));
            var task = dev.GetServiceRecordsAsync(Guid.Empty, null);
        }

        //[Test]
        public void ObexGetResponse_None()
        {
            // No usage exceptions
            var req = new ObexWebRequest(new Uri("obex://192.0.1.1/foo.txt"));
            var task = req.GetResponseAsync(null);
        }

    }
}
#endif
