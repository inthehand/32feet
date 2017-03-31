#if BLUETOPIA
using System;
using NUnit.Framework;
using System.Net.Sockets;
using System.Diagnostics;
using InTheHand.Net.Bluetooth.StonestreetOne;

namespace InTheHand.Net.Tests.BluetopiaTests
{
    [TestFixture]
    public class BluetopiaExceptionTests
    {
        [Test]
        public void SoEx_KnownBluetopiaErrorCode()
        {
            BluetopiaSocketException ex;
            ex = new BluetopiaSocketException(BluetopiaError.HCI_TIMEOUT_ERROR, SocketError.NotSocket);
            Assert.AreEqual("HCI_TIMEOUT_ERROR", ex.BluetopiaError, "BluetopiaError");
            Assert.AreEqual((int)BluetopiaError.HCI_TIMEOUT_ERROR, ex.BluetopiaErrorCode, "BluetopiaErrorCode");
            //
            Assert.AreEqual(SocketError.NotSocket, ex.SocketErrorCode, "SocketErrorCode");
            Assert.AreEqual((int)SocketError.NotSocket, ex.ErrorCode, "ErrorCode");
            Assert.AreEqual((int)SocketError.NotSocket, ex.NativeErrorCode, "NativeErrorCode");
            // Just for historical tracking
            Assert.AreEqual("An operation was attempted on something that is not a socket"
                + " (Bluetopia: HCI_TIMEOUT_ERROR (-17)).",
                ex.Message, "Message");
        }

        [Test]
        public void SoEx_UnknownBluetopiaErrorCode()
        {
            BluetopiaSocketException ex;
            ex = new BluetopiaSocketException((BluetopiaError)(-100000), (int)SocketError.NotSocket);
            Assert.AreEqual("-100000", ex.BluetopiaError, "BluetopiaError");
            Assert.AreEqual(-100000, ex.BluetopiaErrorCode, "BluetopiaErrorCode");
            //
            Assert.AreEqual(SocketError.NotSocket, ex.SocketErrorCode, "SocketErrorCode");
            Assert.AreEqual((int)SocketError.NotSocket, ex.ErrorCode, "ErrorCode");
            Assert.AreEqual((int)SocketError.NotSocket, ex.NativeErrorCode, "NativeErrorCode");
            // Just for historical tracking
            Assert.AreEqual("An operation was attempted on something that is not a socket"
                + " (Bluetopia: -100000 (-100000)).",
                ex.Message, "Message");
        }

    }
}
#endif
