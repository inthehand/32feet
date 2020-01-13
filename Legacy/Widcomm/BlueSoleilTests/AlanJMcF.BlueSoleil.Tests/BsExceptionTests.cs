using System;
using System.Collections.Generic;
using NUnit.Framework;
using InTheHand.Net.Bluetooth.BlueSoleil;
using System.Net.Sockets;
using System.Diagnostics;

namespace InTheHand.Net.Tests.BlueSoleil
{
    [TestFixture]
    public class BsExceptionTests
    {
        [Test]
        public void SoEx_KnownBtSdkErrorCode()
        {
            BlueSoleilSocketException ex;
            ex = new BlueSoleilSocketException(BtSdkError.PAGE_TIMEOUT, SocketError.NotSocket);
            Assert.AreEqual("PAGE_TIMEOUT", ex.BlueSoleilError, "BlueSoleilError");
            Assert.AreEqual((int)BtSdkError.PAGE_TIMEOUT, ex.BlueSoleilErrorCode, "BlueSoleilErrorCode");
            //
            Assert.AreEqual(SocketError.NotSocket, ex.SocketErrorCode, "SocketErrorCode");
            Assert.AreEqual((int)SocketError.NotSocket, ex.ErrorCode, "ErrorCode");
            Assert.AreEqual((int)SocketError.NotSocket, ex.NativeErrorCode, "NativeErrorCode");
            // Just for historical tracking
            Assert.AreEqual("An operation was attempted on something that is not a socket"
                + " (BlueSoleil: PAGE_TIMEOUT (0x0404)).",
                ex.Message, "Message");
        }

        [Test]
        public void SoEx_UnknownBtSdkErrorCode()
        {
            BlueSoleilSocketException ex;
            ex = new BlueSoleilSocketException((BtSdkError)65099, (int)SocketError.NotSocket);
            Assert.AreEqual("0xFE4B", ex.BlueSoleilError, "BlueSoleilError");
            Assert.AreEqual(65099, ex.BlueSoleilErrorCode, "BlueSoleilErrorCode");
            //
            Assert.AreEqual(SocketError.NotSocket, ex.SocketErrorCode, "SocketErrorCode");
            Assert.AreEqual((int)SocketError.NotSocket, ex.ErrorCode, "ErrorCode");
            Assert.AreEqual((int)SocketError.NotSocket, ex.NativeErrorCode, "NativeErrorCode");
            // Just for historical tracking
            Assert.AreEqual("An operation was attempted on something that is not a socket"
                + " (BlueSoleil: 65099 (0xFE4B)).",
                ex.Message, "Message");
        }

    }
}
