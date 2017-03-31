using System;
using NUnit.Framework;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;
#if FX1_1
using NullableInt = System.Object;
#else
using NullableInt = System.Nullable<System.Int32>;
#endif


namespace InTheHand.Net.Tests.ObexRequest
{
    [TestFixture]
    public class UriFormats
    {
        ObexWebRequest CreateObexWebRequestOfClassThatThrows(Uri requestUri)
        {
            ObexWebRequest req = new ObexWebRequest(requestUri);
            return req;
        }


        [Test]
        [ExpectedException(typeof(UriFormatException))]
        public void BadUriScheme()
        {
            Uri uri = new Uri("xxxxxxxxx://11223344/foo.txt");
            ObexWebRequest req = CreateObexWebRequestOfClassThatThrows(uri);
        }

        //Depends on non-existence of chosen arbitrary address, and could be slow, so don't run.
        //[Test]
        //public void BadUriIrdaNoSuchPeer()
        //{
        //    Uri uri = new Uri("obex://00000000/foo.txt");
        //    ObexWebRequest req = CreateObexWebRequestOfClassThatThrows(uri);
        //    req.GetResponse();
        //}

        [Test]
        public void BadUriBadHostFormat()
        {
            Uri uri = new Uri("obex://0000000000000000000000000/foo.txt");
            ObexWebRequest req = CreateObexWebRequestOfClassThatThrows(uri);
            try {
                req.GetResponse();
                Assert.Fail("Should have thrown!");
            } catch (WebException we) {
                Exception innerException = we.InnerException;
                Assert.IsNotNull(innerException);
                SocketException sex = innerException as SocketException;
                Assert.IsNotNull(sex);
                const int WSAEADDRNOTAVAIL = 10049; // (SocketError enum not in CF)
                int err = sex.ErrorCode;
                Assert.AreEqual(WSAEADDRNOTAVAIL, err);
            }
        }

        //----
        const string AddrAStr = "002233445566";
        static readonly BluetoothAddress AddrA = BluetoothAddress.Parse(AddrAStr);

        [Test]
        public void CreateUrl()
        {
            var u = ObexWebRequest.CreateUrl("obex", AddrA, "foo.txt");
            Assert.AreEqual("obex://002233445566/foo.txt", u.ToString(), "ToString");
            Assert.AreEqual("/foo.txt", u.PathAndQuery, "PathAndQuery");
        }

        [Test]
        public void CreateUrl_WithSlash()
        {
            var u = ObexWebRequest.CreateUrl("obex", AddrA, "/foo.txt");
            Assert.AreEqual("obex://002233445566/foo.txt", u.ToString(), "ToString"); // HACK ??
            Assert.AreEqual("/foo.txt", u.PathAndQuery, "PathAndQuery");
        }

        [Test]
        public void CreateUrl_Path_NotSupportedByOurObexButTestAnyway()
        {
            var u = ObexWebRequest.CreateUrl("obex", AddrA, "bar/foo.txt");
            Assert.AreEqual("obex://002233445566/bar/foo.txt", u.ToString(), "ToString");
            Assert.AreEqual("/bar/foo.txt", u.PathAndQuery, "PathAndQuery");
        }

        [Test]
        public void CreateUrl_UnknownSchemeAbcd_JustUseAndLetTheClassDisallow()
        {
            var u = ObexWebRequest.CreateUrl("abcd", AddrA, "foo.txt");
            Assert.AreEqual("abcd://002233445566/foo.txt", u.ToString(), "ToString");
            Assert.AreEqual("/foo.txt", u.PathAndQuery, "PathAndQuery");
        }

        [Test]
        public void CreateUrl_UnknownSchemeHttp_JustUseAndLetTheClassDisallow()
        {
            var u = ObexWebRequest.CreateUrl("http", AddrA, "foo.txt");
            Assert.AreEqual("http://002233445566/foo.txt", u.ToString(), "ToString");
            Assert.AreEqual("/foo.txt", u.PathAndQuery, "PathAndQuery");
        }

        //------
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateUrl_NullPath_NotAllowed()
        {
            var u = ObexWebRequest.CreateUrl("obex", AddrA, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateUrl_NullScheme_NotAllowed()
        {
            var u = ObexWebRequest.CreateUrl(null, AddrA, "foo.txt");
        }

        //------
        [Test]
        public void CreateUrl_EmptyPath_IsAllowed()
        {
            var u = ObexWebRequest.CreateUrl("obex", AddrA, string.Empty);
            Assert.AreEqual("obex://002233445566/", u.ToString(), "ToString");
            Assert.AreEqual("/", u.PathAndQuery, "PathAndQuery");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateUrl_EmptyScheme_NotAllowed()
        {
            var u = ObexWebRequest.CreateUrl(string.Empty, AddrA, "foo.txt");
        }

    }//class


    class NewWholeProcessTests_Data
    {
        static NewWholeProcessTests_Data()
        {
            const int ExtraLen = 0x8000;
            AddExtraDataAsLongHeader(SimplePut1_16bitLengths_Responses_TEMPLATE, ExtraLen);
        }

        static void AddExtraDataAsLongHeader(byte[][] template, int extraLen)
        {
            const byte HeaderId_ObjectClass = 0x51;
            //
            int totalLen = 0;
            foreach (var chunk in template) {
                totalLen += chunk.Length;
                totalLen += extraLen;
            }
            //
            var data = new byte[totalLen];
            var extraData = new byte[extraLen];
            extraData[0] = HeaderId_ObjectClass;
            extraData[1] = 0x80;
            extraData[2] = 0x00;
            int idx = 0;
            foreach (var chunk in template) {
                chunk.CopyTo(data, idx);
                idx += chunk.Length;
                extraData.CopyTo(data, idx);
                idx += extraData.Length;
            }
            if (idx != data.Length) throw new InvalidOperationException();
            SimplePut1_16bitLengths_Responses = data;
        }

        //--------
        public static readonly byte[] SimplePut1_Data = { (byte)'a', (byte)'b', (byte)'c', (byte)'d' };
        public static readonly byte[] SimplePut1_Responses ={
            // Connect response
            0xA0, 0,7, 0x10, 0x00, 0x08, 0x00, // MTU=0x800=2048
            // Subsequent response(s)
            0x90, 0,3,
            0xA0, 0,3,
            //
            0xA0, 0,3,
        };
        public static readonly byte[] SimplePut1_ErrorServerResponseCodeAtFinal_Responses ={
            // Connect response
            0xA0, 0,7, 0x10, 0x00, 0x08, 0x00, // MTU=0x800=2048
            // Subsequent response(s)
            0x90, 0,3,
            0xD3, 0,3, // code = Service Unavailable
            //
            0xA0, 0,3,
        };
        public static readonly byte[] SimplePut1_ErrorServerResponseCodeAtFirst_Responses ={
            // Connect response
            0xA0, 0,7, 0x10, 0x00, 0x08, 0x00, // MTU=0x800=2048
            // Subsequent response(s)
            0xD3, 0,3, // code = Service Unavailable
            //
            0xA0, 0,3,
        };
        public static readonly byte[] SimplePut1_ErrorServerResponseCodeAtConnect_Responses ={
            // Connect response
            // code = Service Unavailable
            0xD3, 0,7, 0x10, 0x00, 0x08, 0x00, // MTU=0x800=2048
        };
        public static readonly byte[] SimplePut1_ErrorServerResponseCodeAtDisconnect_Responses ={
            // Connect response
            0xA0, 0,7, 0x10, 0x00, 0x08, 0x00, // MTU=0x800=2048
            // Subsequent response(s)
            0x90, 0,3,
            0xA0, 0,3, // code = Service Unavailable
            //
            0xD3, 0,3,
        };
        public static byte[] SimplePut1_16bitLengths_Responses;
        public static readonly byte[][] SimplePut1_16bitLengths_Responses_TEMPLATE ={
            // Connect response
            new byte[] { 0xA0, 0x80,7, 0x10, 0x00, 0x08, 0x00}, // MTU=0x800=2048
            // Subsequent response(s)
            new byte[] { 0x90, 0x80,3},
            new byte[] { 0xA0, 0x80,3},
            //
            new byte[] { 0xA0, 0x80,3},
        };
        public static readonly byte[] SimplePut1_ClosesBeforeDisconnect_Responses ={
            // Connect response
            0xA0, 0,7, 0x10, 0x00, 0x08, 0x00, // MTU=0x800=2048
            // Subsequent response(s)
            0x90, 0,3,
            0xA0, 0,3,
            //
            //Close before this XX0xA0, 0,3,XX
        };
        public static readonly byte[] SimplePut1_ExpectedRequests ={
            //Connect
            0x80, 0x00, 0x07, 0x10, 0x00, 0x20, 0x00,
            //Put
            0x02, 0x00, 0x1D,
                0x01, 0x00, 0x15, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x2E,
                    0x00, 0x74, 0x00, 0x78, 0x00, 0x74, 0x00, 0x00,
                0xC3, 0x00, 0x00, 0x00, 0x04, 
            //Put
            0x82, 0x00, 0x0A,
                0x49, 0x00, 0x07, 0x61, 0x62, 0x63, 0x64,
            //Disconnect
            0x81, 0x00, 0x03
        };
        public static readonly byte[] SimplePut1_PutInterrupted_ExpectedRequests ={
            //Connect
            0x80, 0x00, 0x07, 0x10, 0x00, 0x20, 0x00,
            //Put
            0x02, 0x00, 0x1D,
                0x01, 0x00, 0x15, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x2E,
                    0x00, 0x74, 0x00, 0x78, 0x00, 0x74, 0x00, 0x00,
                0xC3, 0x00, 0x00, 0x00, 0x04, 
            ////Put
            //0x82, 0x00, 0x0A,
            //    0x49, 0x00, 0x07, 0x61, 0x62, 0x63, 0x64,
            //Disconnect
            0x81, 0x00, 0x03
        };
        public static readonly byte[] SimplePut1_TooLongContentLength_ExpectedRequests ={
            //Connect
            0x80, 0x00, 0x07, 0x10, 0x00, 0x20, 0x00,
            //Put
            0x02, 0x00, 0x1D,
                0x01, 0x00, 0x15, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x2E,
                    0x00, 0x74, 0x00, 0x78, 0x00, 0x74, 0x00, 0x00,
                0xC3, 0x00, 0x00, 0x90, 0x04, 
            //Put
            0x82, 0x00, 0x0A,
                0x49, 0x00, 0x07, 0x61, 0x62, 0x63, 0x64,
            //Disconnect
            0x81, 0x00, 0x03
        };
        public static readonly byte[] SimplePut1_TooShortContentLength_ExpectedRequests ={
            //Connect
            0x80, 0x00, 0x07, 0x10, 0x00, 0x20, 0x00,
            //Put
            0x02, 0x00, 0x1D,
                0x01, 0x00, 0x15, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x2E,
                    0x00, 0x74, 0x00, 0x78, 0x00, 0x74, 0x00, 0x00,
                0xC3, 0x00, 0x00, 0x00, 0x01, 
            //Put
            0x82, 0x00, 0x0A,
                0x49, 0x00, 0x07, 0x61, 0x62, 0x63, 0x64,
            //Disconnect
            0x81, 0x00, 0x03
        };
        public static readonly byte[] SimplePut1_NoContentLength_ExpectedRequests ={
            //Connect
            0x80, 0x00, 0x07, 0x10, 0x00, 0x20, 0x00,
            //Put
            0x02, 0x00, 0x18,
                0x01, 0x00, 0x15, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x2E,
                    0x00, 0x74, 0x00, 0x78, 0x00, 0x74, 0x00, 0x00,
                //0xC3, 0x00, 0x00, 0x00, 0x01, 
            //Put
            0x82, 0x00, 0x0A,
                0x49, 0x00, 0x07, 0x61, 0x62, 0x63, 0x64,
            //Disconnect
            0x81, 0x00, 0x03
        };

        //--
        public static readonly byte[] SimplePut3EmptyEob_ExpectedRequests ={
            //Connect
            0x80, 0x00, 0x07, 0x10, 0x00, 0x20, 0x00,
            //Put
            0x02, 0x00, 0x18,
                0x01, 0x00, 0x15, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x2E,
                    0x00, 0x74, 0x00, 0x78, 0x00, 0x74, 0x00, 0x00,
                //0xC3, 0x00, 0x00, 0x00, 0x01, 
            //Put + Body
            0x02, 0x00, 0x0A,
                0x48, 0x00, 0x07, 0x61, 0x62, 0x63, 0x64,
            //Put-Final + EoB
            0x82, 0x00, 0x06,
                0x49, 0x00, 0x03,
            //Disconnect
            0x81, 0x00, 0x03
        };
        public static readonly byte[] SimplePut3_Responses ={
            // Connect response
            0xA0, 0,7, 0x10, 0x00, 0x20, 0x00, // MTU=0x800=2048
            // Subsequent response(s)
            0x90, 0,3,
            0x90, 0,3,
            0xA0, 0,3,
            //
            0xA0, 0,3,
        };

        public static readonly byte[] SimplePut3_ConnectionTruncatedBeforeFinal_ExpectedRequests ={
            //Connect
            0x80, 0x00, 0x07, 0x10, 0x00, 0x20, 0x00,
            //Put
            0x02, 0x00, 0x18,
                0x01, 0x00, 0x15, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x2E,
                    0x00, 0x74, 0x00, 0x78, 0x00, 0x74, 0x00, 0x00,
                //0xC3, 0x00, 0x00, 0x00, 0x01, 
            //Put + Body
            0x02, 0x00, 0x0A,
                0x48, 0x00, 0x07, 0x61, 0x62, 0x63, 0x64,
            ////Put-Final + EoB
            //0x82, 0x00, 0x06,
            //    0x49, 0x00, 0x03,
            ////Disconnect
            //0x81, 0x00, 0x03
        };
        public static readonly byte[] SimplePut3_ConnectionTruncatedBeforeFinal_TruncatedInPacketInHeader_ExpectedRequests ={
            //Connect
            0x80, 0x00, 0x07, 0x10, 0x00, 0x20, 0x00,
            //Put
            0x02, 0x00, 0x18,
                0x01, 0x00, 0x15, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x2E,
                    0x00, 0x74, 0x00, 0x78, 0x00, 0x74, 0x00, 0x00,
                //0xC3, 0x00, 0x00, 0x00, 0x01, 
            //Put + Body
            0x02, 0x00, //!!!! 0x0A,
            //    0x48, 0x00, 0x07, 0x61, 0x62, 0x63, 0x64,
            ////Put-Final + EoB
            //0x82, 0x00, 0x06,
            //    0x49, 0x00, 0x03,
            ////Disconnect
            //0x81, 0x00, 0x03
        };
        public static readonly byte[] SimplePut3_ConnectionTruncatedBeforeFinal_TruncatedInPacketAfterHeader_ExpectedRequests ={
            //Connect
            0x80, 0x00, 0x07, 0x10, 0x00, 0x20, 0x00,
            //Put
            0x02, 0x00, 0x18,
                0x01, 0x00, 0x15, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x2E,
                    0x00, 0x74, 0x00, 0x78, 0x00, 0x74, 0x00, 0x00,
                //0xC3, 0x00, 0x00, 0x00, 0x01, 
            //Put + Body
            0x02, 0x00, 0x0A,
                0x48, 0x00, 0x07, 0x61, 0x62, //!!!!0x63, 0x64,
            ////Put-Final + EoB
            //0x82, 0x00, 0x06,
            //    0x49, 0x00, 0x03,
            ////Disconnect
            //0x81, 0x00, 0x03
        };
        public static readonly byte[] SimplePut3_Truncated_Responses ={
            // Connect response
            0xA0, 0,7, 0x10, 0x00, 0x20, 0x00, // MTU=0x800=2048
            // Subsequent response(s)
            0x90, 0,3,
            0x90, 0,3,
            //0xA0, 0,3,
            ////
            //0xA0, 0,3,
        };

        //--
        public static readonly byte[] SimplePut1_ConnectionId_Responses ={
            // Connect response
            0xA0, 0,12, 0x10, 0x00, 0x08, 0x00, // MTU=0x800=2048
                /*ConnectionId*/0xCB, 0x00,0x00,0x16,0x0A,
            // Subsequent response(s)
            0x90, 0,3,
            0xA0, 0,3,
            //
            0xA0, 0,3,
        };
        public static readonly byte[] SimplePut1_ConnectionIdInAllResponses_Responses ={
            // Connect response
            0xA0, 0,12, 0x10, 0x00, 0x08, 0x00, // MTU=0x800=2048
                /*ConnectionId*/0xCB, 0x00,0x00,0x16,0x0A,
            // Subsequent response(s)
            0x90, 0,8, /*ConnectionId*/0xCB, 0x00,0x00,0x16,0x0A,
            0xA0, 0,8, /*ConnectionId*/0xCB, 0x00,0x00,0x16,0x0A,
            //
            0xA0, 0,8, /*ConnectionId*/0xCB, 0x00,0x00,0x16,0x0A,
        };

        public static readonly byte[] SimplePut1_ConnectionId_ExpectedRequests ={
            //Connect
            0x80, 0x00, 0x07, 0x10, 0x00, 0x20, 0x00,
            //Put
            0x02, 0x00, 0x22, 
                /*ConnectionId*/0xCB, 0x00,0x00,0x16,0x0A,
                0x01, 0x00, 0x15, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x2E,
                    0x00, 0x74, 0x00, 0x78, 0x00, 0x74, 0x00, 0x00, 0xC3, 0x00, 0x00, 0x00, 0x04, 
            //Put
            0x82, 0x00, 0x0A, 0x49, 0x00, 0x07, 0x61, 0x62, 0x63, 0x64,
            //Disconnect
            0x81, 0x00, 0x08,
                /*ConnectionId*/0xCB, 0x00,0x00,0x16,0x0A,
        };

        //--
        public static readonly byte[] SimplePut1SimpleNonAsciiFilename_ExpectedRequests ={
            //Connect
            0x80, 0x00, 0x07, 0x10, 0x00, 0x20, 0x00,
            //Put
            0x02, 0x00, 0x23, 
                0x01, 0x00, 0x1b, 
                    // "aáa†a%a.txt"
                    0x00, 0x61, 0x00, 0xE1, 0x00, 0x61, 0x20, 0x21, 0x00, 0x61, 0x00, 0x25, 0x00, 0x61, 
                    0x00, 0x2E,
                    0x00, 0x74, 0x00, 0x78, 0x00, 0x74, 0x00, 0x00, 
                0xC3, 0x00, 0x00, 0x00, 0x04, 
            //Put
            0x82, 0x00, 0x0A, 0x49, 0x00, 0x07, 0x61, 0x62, 0x63, 0x64,
            //Disconnect
            0x81, 0x00, 0x03
        };

        public static readonly byte[] SimplePut1UnicodeWithSurrogatePair_ExpectedRequests ={
            //Connect
            0x80, 0x00, 0x07, 0x10, 0x00, 0x20, 0x00,
            //Put
            0x02, 0, 33, 
                0x01, 0, 25, 
                    0x00, 0x61, 0x00, 0x61, 0xD8, 0x08, 0xDC, 0x04, 0x00, 0x61, 0x00, 0x61, 
                    0x00, 0x2E, 0x00, 0x74, 0x00, 0x78, 0x00, 0x74, 0x00, 0x00, 
                0xC3, 0x00, 0x00, 0x00, 0x04, 
            //Put
            0x82, 0x00, 0x0A, 0x49, 0x00, 0x07, 0x61, 0x62, 0x63, 0x64,
            //Disconnect
            0x81, 0x00, 0x03
        };

        public static readonly byte[] SimplePut1UnicodeChinese_ExpectedRequests ={
            //Connect
            0x80, 0x00, 0x07, 0x10, 0x00, 0x20, 0x00,
            //Put
            0x02, 0x00, 0x1D, 
                0x01, 0x00, 0x15, 
                    0x7A, 0x7A, 0x76, 0x7D, 0x65, 0x87, 0x4E, 0xF6, 
                    0x00, 0x2E, 0x00, 0x64, 0x00, 0x6F, 0x00, 0x63, 0x00, 0x00, 
                0xC3, 0x00, 0x00, 0x00, 0x04, 
            //Put
            0x82, 0x00, 0x0A, 0x49, 0x00, 0x07, 0x61, 0x62, 0x63, 0x64,
            //Disconnect
            0x81, 0x00, 0x03
        };



        public static readonly byte[] SimplePut1_PeerMtuEndsNonZero_Responses ={
            // Connect response
            0xA0, 0,7, 0x10, 0x00, 0x07, 0xFF, // MTU=0x7FF=2047
            // Subsequent response(s)
            0x90, 0,3,
            0xA0, 0,3,
            //
            0xA0, 0,3,
        };

        public static readonly byte[] SimplePut2HugePeerMruAndBigFile_Data = new byte[70000];
        public static readonly byte[] SimplePut2HugePeerMruAndBigFile_Responses ={
            // Connect response
            0xA0, 0,7, 0x10, 0x00, 0xFF, 0xFF, // MTU=0xFFFF=65535
            // Subsequent response(s)
            0x90, 0,3,
            0x90, 0,3,
            0xA0, 0,3,
            //
            0xA0, 0,3,
        };
        public static readonly byte[] SimplePut2MediumHugePeerMruAndBigFile_Responses ={
            // Connect response
            0xA0, 0,7, 0x10, 0x00, 0x80, 0x00, // MTU=0x8000=32768
            // Subsequent response(s)
            0x90, 0,3,
            0x90, 0,3,
            0xA0, 0,3,
            //
            0xA0, 0,3,
        };
        public static readonly byte[] SimplePut2SmallestPeerMruAndMultiChunks_Responses ={
            // Connect response
            0xA0, 0,7, 0x10, 0x00, 0x00, 0xFF, // MTU=0x00FF=255
            // Subsequent response(s)
            0x90, 0,3,
            0x90, 0,3,
            0x90, 0,3,
            0xA0, 0,3,
            //
            0xA0, 0,3,
        };

        //--------
        public static readonly byte[] SimpleGet1_ConnectionId_Responses ={
            // Connect response
            0xA0, 0,12, 0x10, 0x00, 0x08, 0x00, // MTU=0x800=2048
                /*ConnectionId*/ 0xCB, 0, 0, 22, 10,
            // Subsequent response(s)
            0x90, 0,10, /*Body*/0x48, 0,7, 0x61, 0x62, 0x63, 0x64,
            0xA0, 0, 6, /*EoB */0x49, 0,3,
            //Disconnect
            0xA0, 0,3,
        };
        public static readonly byte[] SimpleGet1_NoConnectionId_Responses ={
            // Connect response
            0xA0, 0,7, 0x10, 0x00, 0x08, 0x00, // MTU=0x800=2048
            // Subsequent response(s)
            0x90, 0,10, /*Body*/0x48, 0,7, 0x61, 0x62, 0x63, 0x64,
            0xA0, 0, 6, /*EoB */0x49, 0,3,
            //Disconnect
            0xA0, 0,3,
        };
        public static readonly byte[] SimpleGet1_ConnectionIdInAllResponse_Responses ={
            // Connect response
            0xA0, 0,12, 0x10, 0x00, 0x08, 0x00, // MTU=0x800=2048
                /*ConnectionId*/ 0xCB, 0, 0, 22, 10,
            // Subsequent response(s)
            0x90, 0,15, /*ConnectionId*/ 0xCB, 0, 0, 22, 10, /*Body*/0x48, 0,7, 0x61, 0x62, 0x63, 0x64,
            0xA0, 0,11, /*ConnectionId*/ 0xCB, 0, 0, 22, 10, /*EoB */0x49, 0,3,
            //Disconnect
            0xA0, 0,8,  /*ConnectionId*/ 0xCB, 0, 0, 22, 10,
        };

        public static readonly byte[] SimpleGet1_NoConnectionId_ExpectedRequests ={
            //Connect
            0x80, 0x00, 0x07, 0x10, 0x00, 0x20, 0x00,
            //Get
            0x83, 0x00, 0x18,
                0x01, 0x00, 0x15, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x2E,
                0x00, 0x74, 0x00, 0x78, 0x00, 0x74, 0x00, 0x00,
            //Get
            0x83, 0,3,
            //Disconnect
            0x81, 0x00, 0x03,
        };

        public static readonly byte[] SimpleGet1_ExpectedRequests ={
            //Connect
            0x80, 0x00, 0x07, 0x10, 0x00, 0x20, 0x00,
            //Get
            0x83, 0x00, 0x1D, /*ConnectionId*/ 0xCB, 0, 0, 22, 10,
                0x01, 0x00, 0x15, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x2E,
                0x00, 0x74, 0x00, 0x78, 0x00, 0x74, 0x00, 0x00,
            //Get
            0x83, 0,3,
            //Disconnect
            0x81, 0x00, 0x08, /*ConnectionId*/ 0xCB, 0, 0, 22, 10,
        };

        public static readonly byte[] SimpleUnknownVerb_ExpectedRequests ={
            //Connect
            0x80, 0x00, 0x07, 0x10, 0x00, 0x20, 0x00,
            // Unknown!
            0x0F, 0x00, 0x08, /*ConnectionId*/ 0xCB, 0, 0, 22, 10,
            // Unknown!
            0x8F, 0x00, 0x08, /*ConnectionId*/ 0xCB, 0, 0, 22, 10,
            //Disconnect
            0x81, 0x00, 0x08, /*ConnectionId*/ 0xCB, 0, 0, 22, 10,
        };


        //public static readonly byte[] SimpleTwoPuts_Data ={ (byte)'a', (byte)'b', (byte)'c', (byte)'d' };
        public static readonly byte[] TwoPuts_ExpectedRequests ={
            //Connect
            0x80, 0x00, 0x07, 0x10, 0x00, 0x20, 0x00,
            //Put
            0x02, 0x00, 0x1D,
                0x01, 0x00, 0x15, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x2E,
                    0x00, 0x74, 0x00, 0x78, 0x00, 0x74, 0x00, 0x00,
                0xC3, 0x00, 0x00, 0x00, 0x04, 
            //Put
            0x82, 0x00, 0x0A,
                0x49, 0x00, 0x07, 0x61, 0x62, 0x63, 0x64,
            //Put
            0x02, 0x00, 0x1D,
                0x01, 0x00, 0x15, 0x00, 0x6d, 0x00, 0x6d, 0x00, 0x6d, 0x00, 0x6d, 0x00, 0x2E,
                    0x00, 0x74, 0x00, 0x78, 0x00, 0x74, 0x00, 0x00,
                0xC3, 0x00, 0x00, 0x00, 0x04, 
            //Put
            0x82, 0x00, 0x0A,
                0x49, 0x00, 0x07, 0x6d, 0x6e, 0x6f, 0x70,
            //Disconnect
            0x81, 0x00, 0x03
        };

    }//class


    public abstract class NewWholeProcessTestsInfrastructure
    {
        public virtual Uri CreateObexUriForStreamTest(String filename)
        {
            return new Uri("obex:" + filename);
        }

        //----
        public ObexWebRequest CreateObexWebRequestWithGivenConnectionStream(Stream peer, String filename)
        {
            return new ObexWebRequest(CreateObexUriForStreamTest(filename), peer);
        }

        //----
        protected virtual MemoryStream InternalDoTest(ObexMethod method, String filename,
            byte[] data, Byte[] CannedResponses, byte[] outData, out int outDataLen, bool restrictReadsToSingleBytes,
            NullableInt truncateWrites)
        {
            if (method != ObexMethod.Put && method != ObexMethod.Get)
                throw new ArgumentOutOfRangeException("method");
            //
            MemoryStream ourRequests;
            if (truncateWrites != null) {
                byte[] fixedLenBuffer = new byte[(int)truncateWrites];
                ourRequests = new MemoryStream(fixedLenBuffer);
            } else {
                ourRequests = new MemoryStream();
            }
            //
            Stream peerResponses = new MemoryStream(CannedResponses, false);
            if (restrictReadsToSingleBytes) {
                peerResponses = new ByteByByteReadStream(peerResponses);
            }
            //
            TwoWayStream peer = new TwoWayStream(peerResponses, ourRequests);
            //--
            ObexWebRequest req = CreateObexWebRequestWithGivenConnectionStream(peer, filename);
            //
            if (data != null) {
                Stream put = req.GetRequestStream();
                put.Write(data, 0, data.Length);
                req.ContentLength = data.Length;
            }
            //
            if (method == ObexMethod.Get) {
                req.Method = "GET";
            }
            ObexWebResponse rsp = (ObexWebResponse)req.GetResponse();
            const ObexStatusCode RspOK = ObexStatusCode.OK | ObexStatusCode.Final;
            //Debug.Assert(RspOK == rsp.StatusCode, "Arghhh, fail response code: '" + rsp.StatusCode + "'");
            //
            if (outData != null) {
                Stream get = rsp.GetResponseStream();
                outDataLen = Stream_CopyTo(get, outData);
            } else {
                outDataLen = -1;
            }
            //
            return ourRequests;
        }

        internal void DoTestPut(String filename, byte[] data, Byte[] CannedResponses, Byte[] ExpectedRequests, bool restrictReadsToSingleBytes)
        {
            DoTestPut(filename, data, CannedResponses, ExpectedRequests, restrictReadsToSingleBytes, null);
        }

        internal void DoTestPut(String filename, byte[] data, Byte[] CannedResponses, Byte[] ExpectedRequests, bool restrictReadsToSingleBytes, NullableInt truncateWrites)
        {
            int dummy;
            Debug.Assert(truncateWrites == null || (int)truncateWrites < ExpectedRequests.Length,
                "Bad truncateWrites value; is greater than normal result!");
            MemoryStream ourRequests = InternalDoTest(ObexMethod.Put, filename, data, CannedResponses, null, out dummy, restrictReadsToSingleBytes, truncateWrites);
            //
            //TODO Assert.AreEqual.... for InTheHand.Net.Tests.ObexRequest.NewWholeProcessTests.DoTest
            Assert.AreEqual(ExpectedRequests, ourRequests.ToArray(), "Requests");
        }

        internal void DoTestPut_NoCheckRequest(String filename, byte[] data, Byte[] CannedResponses, bool restrictReadsToSingleBytes)
        {
            int dummy;
            MemoryStream ourRequests =
            InternalDoTest(ObexMethod.Put, filename, data, CannedResponses, null, out dummy, restrictReadsToSingleBytes, null);
            var bufB = ourRequests.GetBuffer();
            var bufA = ourRequests.ToArray();
        }

        internal void DoTestGet(String filename, byte[] expectedData, Byte[] CannedResponses, Byte[] ExpectedRequests, bool restrictReadsToSingleBytes)
        {
            DoTestGet(filename, expectedData, CannedResponses, ExpectedRequests, restrictReadsToSingleBytes, null);
        }

        internal void DoTestGet(String filename, byte[] expectedData,
            Byte[] CannedResponses, Byte[] ExpectedRequests,
            bool restrictReadsToSingleBytes, NullableInt truncateWrites)
        {
            DoTestGet(filename, expectedData,
                CannedResponses, ExpectedRequests,
                restrictReadsToSingleBytes, truncateWrites, null);
        }

        internal void DoTestGet(String filename, byte[] expectedData,
            Byte[] CannedResponses, Byte[] ExpectedRequests,
            bool restrictReadsToSingleBytes, NullableInt truncateWrites,
            string name)
        {
            if (name != null) name += " -- ";
            byte[] getData0 = new byte[1000];
            int getDataLen;
            Debug.Assert(truncateWrites == null || (int)truncateWrites < ExpectedRequests.Length,
                "Bad truncateWrites value; is greater than normal result!");
            MemoryStream ourRequests = InternalDoTest(ObexMethod.Get, filename, null, CannedResponses, getData0, out getDataLen, restrictReadsToSingleBytes, truncateWrites);
            //
            //TODO Assert.AreEqual.... for InTheHand.Net.Tests.ObexRequest.NewWholeProcessTests.DoTest
            Assert.AreEqual(ExpectedRequests, ourRequests.ToArray(), name + "Requests");
            byte[] getData = new byte[getDataLen];
            Array.Copy(getData0, 0, getData, 0, getData.Length);
            Assert.AreEqual(expectedData, getData, name + "GET data");
        }

        private static int Stream_CopyTo(Stream get, byte[] outData)
        {
            int len = 0;
            while (true) {
                int readLen = get.Read(outData, len, outData.Length - len);
                if (readLen == 0)
                    break;
                len += readLen;
            }
            return len;
        }

        protected static byte[] Truncate(byte[] src, int length)
        {
            byte[] dst = new byte[length];
            Array.Copy(src, 0, dst, 0, dst.Length);
            return dst;
        }

    }//class


    [TestFixture]
    public class NewWholeProcessTests_ByteAtATime : NewWholeProcessTests
    {
        protected override MemoryStream InternalDoTest(ObexMethod method, string filename, byte[] data, byte[] CannedResponses, byte[] outData, out int outDataLen, bool restrictReadsToSingleBytes, NullableInt truncateWrites)
        {
            return base.InternalDoTest(method, filename, data, CannedResponses, outData, out outDataLen, true, truncateWrites);
        }
    }

    [TestFixture]
    public class NewWholeProcessTests : NewWholeProcessTestsInfrastructure
    {

        [Test]
        public void SimplePut1()
        {
            DoTestPut("aaaa.txt", NewWholeProcessTests_Data.SimplePut1_Data, NewWholeProcessTests_Data.SimplePut1_Responses, NewWholeProcessTests_Data.SimplePut1_ExpectedRequests, false);
        }

        [Test]
        public void SimplePut1_ConnectionId()
        {
            DoTestPut("aaaa.txt", NewWholeProcessTests_Data.SimplePut1_Data, NewWholeProcessTests_Data.SimplePut1_ConnectionId_Responses, NewWholeProcessTests_Data.SimplePut1_ConnectionId_ExpectedRequests, false);
            DoTestPut("aaaa.txt", NewWholeProcessTests_Data.SimplePut1_Data, NewWholeProcessTests_Data.SimplePut1_ConnectionIdInAllResponses_Responses, NewWholeProcessTests_Data.SimplePut1_ConnectionId_ExpectedRequests, false);
        }

        [Test]
        public void SimplePut1_SendsConnectionIdInEachResponse()
        {
            DoTestPut("aaaa.txt", NewWholeProcessTests_Data.SimplePut1_Data, NewWholeProcessTests_Data.SimplePut1_ConnectionIdInAllResponses_Responses, NewWholeProcessTests_Data.SimplePut1_ConnectionId_ExpectedRequests, false);
        }

        [Test]
        public void SimplePut1_PeerMtuEndsNonZero()
        {
            DoTestPut("aaaa.txt", NewWholeProcessTests_Data.SimplePut1_Data, NewWholeProcessTests_Data.SimplePut1_PeerMtuEndsNonZero_Responses, NewWholeProcessTests_Data.SimplePut1_ExpectedRequests, false);
        }

        [Test]
        public void SimplePut2HugePeerMruAndBigFile()
        {
            var maxChunkSize = 0xFFFC;
            var expectedRequests = new byte[70051];
            //
            byte[] packetsAB = { 0x80, 0x00, 0x07, 0x10, 0x00, 0x20, 0x00,
                0x02, 0x00, 0x1d, 0x01, 0x00, 0x15, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x2e, 0x00, 0x74, 0x00, 0x78, 0x00, 0x74, 0x00, 0x00, 0xc3, 0x00, 0x01, 0x11, 0x70,
                0x02, 0xFF, 0xFF,
                    0x48, 0xFF, 0xFC, // ...BODY...
            };
            var endAB = packetsAB.Length + maxChunkSize - 3;
            //
            byte[] packetsC = {
                0x82, 0x11, 0x7D,
                    0x49, 0x11, 0x7A, // ...BODY...
            };
            var offsC = endAB;
            //
            byte[] packetsZ = { 0x81, 0x00, 0x03 };
            //
            Array.Copy(packetsAB, 0, expectedRequests, 0, packetsAB.Length);
            Array.Copy(packetsC, 0, expectedRequests, offsC, packetsC.Length);
            Array.Copy(packetsZ, 0, expectedRequests, expectedRequests.Length - packetsZ.Length, packetsZ.Length);
            DoTestPut("aaaa.txt", NewWholeProcessTests_Data.SimplePut2HugePeerMruAndBigFile_Data, NewWholeProcessTests_Data.SimplePut2HugePeerMruAndBigFile_Responses, expectedRequests, false);
        }

        [Test]
        public void SimplePut2MediumHugePeerMruAndBigFile()
        {
            var maxChunkSize = 0x7FFD;
            var expectedRequests = new byte[70057];
            //
            byte[] packetsAB = { 0x80, 0x00, 0x07, 0x10, 0x00, 0x20, 0x00,
                0x02, 0x00, 0x1d,
                    0x01, 0x00, 0x15, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x2e, 0x00, 0x74, 0x00, 0x78, 0x00, 0x74, 0x00, 0x00,
                    0xc3, 0x00, 0x01, 0x11, 0x70,
                // B
                0x02, 0x80, 0x00,
                    0x48, 0x7F, 0xFD, // ...BODY...
            };
            var endAB = packetsAB.Length + maxChunkSize - 3;
            //
            // offset 32804 = 0x8024
            byte[] packetsC = {
                0x02, 0x80, 0x00,
                    0x48, 0x7F, 0xFD, // ...BODY...
            };
            var offsC = endAB;
            var endC = offsC + packetsC.Length + maxChunkSize - 3;
            //
            // offset 65572
            byte[] packetsD = {
                0x82, 0x11, 0x82,
                    0x49, 0x11, 0x7F, // ...BODY...
            };
            var offsD = endC;
            //
            // offset 61572 = 0xF084
            byte[] packetsZ = { 0x81, 0x00, 0x03 };
            var offsZ = expectedRequests.Length - packetsZ.Length;
            //
            Array.Copy(packetsAB, 0, expectedRequests, 0, packetsAB.Length);
            Array.Copy(packetsC, 0, expectedRequests, offsC, packetsC.Length);
            Array.Copy(packetsD, 0, expectedRequests, offsD, packetsD.Length);
            Array.Copy(packetsZ, 0, expectedRequests, offsZ, packetsZ.Length);
            DoTestPut("aaaa.txt", NewWholeProcessTests_Data.SimplePut2HugePeerMruAndBigFile_Data, NewWholeProcessTests_Data.SimplePut2MediumHugePeerMruAndBigFile_Responses, expectedRequests, false);
        }

        [Test]
        public void SimplePut2SmallestPeerMruAndMultiChunks()
        {
            // MTU 255 
            // Thus max data len: 249 (max header: 252).
            // Want at least three chunks: First, Middle, and Final.
            // With MTU 255 and no chunk in PUT initial, we'll get {{ 249, 249, 249, 18 }}
            var data = new byte[3 * 255];
            MarkEach16thByteAndLastByteSuccessively(data);
            //
            const int expectedRequests_Length
                = 7 // Connect
                + 0x1d // PUT Initial - no body
                + 3 * 255 // PUT middle + body
                + 3 + 3 + 18 // PUT final + end-of-body
                + 3; // Disconnect
            //
            byte[] expectedRequests = {
                // Connect
                0x80, 0x00, 0x07, 0x10, 0x00, 0x20, 0x00,
                // Put initial
                0x02, 0x00, 0x1d,
                    0x01, 0x00, 0x15, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x2e, 0x00, 0x74, 0x00, 0x78, 0x00, 0x74, 0x00, 0x00,
                    0xc3, 0x00, 0x00, 0x02, 0xFD, // len 3 * 255 = 765 = 0x02FD
                // Put middle+body
                0x02, 0, 255,
                    0x48, 0, 252, // ...BODY...
                    0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x02,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x03,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x04,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x05,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x06,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x07,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x08,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x09,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x0A,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x0B,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x0C,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x0D,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x0E,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x0F,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x10,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,
                // Put middle+body
                0x02, 0, 255,
                    0x48, 0, 252, // ...BODY...
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x11, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x12, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x13, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x14, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x15, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x16, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x17, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x18, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x19, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x1A, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x1B, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x1C, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x1D, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x1E, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x1F, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x20, 0x00,
                // Put middle+body
                0x02, 0, 255,
                    0x48, 0, 252, // ...BODY...
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x21,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x22,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x23,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x24,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x25,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x26,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x27,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x28,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x29,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x2A,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x2B,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x2C,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x2D,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x2E,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x2F,0x00,
                    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00,
                // Put middle+endBody
                0x82, 0, 24,
                    0x49, 0, 21, // ...BODY...
                    0x00,0x00,0x00,0x00,0x00,0x30,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                    0x00,0x31,
                //Disconnect
                0x81, 0x00, 0x03
            };
            Debug.Assert(expectedRequests_Length == expectedRequests.Length,
                "NOT expectedRequests_Length: " + expectedRequests_Length + " == expectedRequests.Length: " + expectedRequests.Length);
            //
            DoTestPut("aaaa.txt", data, NewWholeProcessTests_Data.SimplePut2SmallestPeerMruAndMultiChunks_Responses, expectedRequests, false);
        }

        /// <summary>
        /// Marks the first byte, each successive 16th byte and the last byte
        /// with an incrementing value. (e.g. #0=1, #16=2, #32=3, etc)
        /// </summary>
        /// <param name="data"></param>
        private static void MarkEach16thByteAndLastByteSuccessively(byte[] data)
        {
            byte j = 1;
            for (int i = 0; i < data.Length; i += 16) {
                data[i] = j;
                unchecked { ++j; }
            }
            data[data.Length - 1] = j;
        }

        [Test]
        public void SimplePut1_ClosesBeforeDisconnect()
        {
            DoTestPut("aaaa.txt", NewWholeProcessTests_Data.SimplePut1_Data, NewWholeProcessTests_Data.SimplePut1_ClosesBeforeDisconnect_Responses, NewWholeProcessTests_Data.SimplePut1_ExpectedRequests, false);
        }

        [Test]
        [Explicit] // Hoping(!) that no devices send huge CONNECT packets nor huge PUT responses.
        public void SimplePut1_ResponsesHave16bitLengths()
        {
            DoTestPut("aaaa.txt", NewWholeProcessTests_Data.SimplePut1_Data, NewWholeProcessTests_Data.SimplePut1_16bitLengths_Responses, NewWholeProcessTests_Data.SimplePut1_ExpectedRequests, false);
        }

        //----  
        [Test]
        //[ExpectedException(typeof(WebException))]
        //[Explicit] // We don't check the response codes (and thus throw)! The user has to check.
        public void SimplePut1_ErrorServerResponseCodeAtFinal()
        {
            DoTestPut("aaaa.txt", NewWholeProcessTests_Data.SimplePut1_Data, NewWholeProcessTests_Data.SimplePut1_ErrorServerResponseCodeAtFinal_Responses, NewWholeProcessTests_Data.SimplePut1_ExpectedRequests, false);
        }

        [Test]
        //[ExpectedException(typeof(WebException))]
        //[Explicit] // We don't check the response codes (and thus throw)! The user has to check.
        public void SimplePut1_ErrorServerResponseCodeAtFirst()
        {
            DoTestPut("aaaa.txt", NewWholeProcessTests_Data.SimplePut1_Data, NewWholeProcessTests_Data.SimplePut1_ErrorServerResponseCodeAtFirst_Responses, NewWholeProcessTests_Data.SimplePut1_PutInterrupted_ExpectedRequests, false);
        }

        [Test]
        [ExpectedException(typeof(WebException))]
        public void SimplePut1_ErrorServerResponseCodeAtConnect_ServerThenCloses()
        {
            DoTestPut("aaaa.txt", NewWholeProcessTests_Data.SimplePut1_Data, NewWholeProcessTests_Data.SimplePut1_ErrorServerResponseCodeAtConnect_Responses, NewWholeProcessTests_Data.SimplePut1_ExpectedRequests, false);
        }

        [Test]
        public void SimplePut1_ErrorServerResponseCodeAtDisconnect()
        {
            DoTestPut("aaaa.txt", NewWholeProcessTests_Data.SimplePut1_Data, NewWholeProcessTests_Data.SimplePut1_ErrorServerResponseCodeAtDisconnect_Responses, NewWholeProcessTests_Data.SimplePut1_ExpectedRequests, false);
        }

        //---------
        [Test]
        public void SimplePut1_EarlyDisconnect_TruncateRcv()
        {
            int totalLen = NewWholeProcessTests_Data.SimplePut1_Responses.Length;
            int offsetOfDisconnect = totalLen - LenOfDisconnectResponse;
            for (int rcvTruncate = 0; rcvTruncate < totalLen; ++rcvTruncate) {
                try {
                    DoTestPut("aaaa.txt", NewWholeProcessTests_Data.SimplePut1_Data,
                        Truncate(NewWholeProcessTests_Data.SimplePut1_Responses, rcvTruncate),
                        NewWholeProcessTests_Data.SimplePut1_ExpectedRequests, false);
                    if (rcvTruncate >= offsetOfDisconnect)  // OWR ignores errors there
                        continue;
                    Assert.Fail("should have thrown");
                } catch (AssertionException) {
                    throw;
                } catch (Exception) {
                    //Console.WriteLine("sht: " + ex.GetType());
                }
            }
        }

        [Test]
        public void SimplePut1_EarlyDisconnect_WhileWriting()
        {
            int totalLen = NewWholeProcessTests_Data.SimplePut1_ExpectedRequests.Length;
            int offsetOfDisconnect = totalLen - LenOfDisconnectRequestA;
            for (int sndTruncate = 0; sndTruncate < totalLen; ++sndTruncate) {
                try {
                    DoTestPut("aaaa.txt", NewWholeProcessTests_Data.SimplePut1_Data,
                        NewWholeProcessTests_Data.SimplePut1_Responses,
                        NewWholeProcessTests_Data.SimplePut1_ExpectedRequests, false, sndTruncate);
                    Assert.Fail("should have thrown");
                } catch (AssertionException) {
                    // OWR ignores errors in disconnect, so ignore "unexpected 
                    // requests array" errors.
                    if (sndTruncate >= offsetOfDisconnect)
                        continue;
                    throw;
                } catch (Exception) {
                    //Console.WriteLine("sht: " + ex.GetType());
                }
            }
        }

        //--------------------------------------------------------------
        [Test]
        public void SimpleGet1()
        {
            DoTestGet("aaaa.txt", NewWholeProcessTests_Data.SimplePut1_Data, NewWholeProcessTests_Data.SimpleGet1_ConnectionId_Responses, NewWholeProcessTests_Data.SimpleGet1_ExpectedRequests, false);
        }

        [Test]
        public void SimpleGet1_ConnectionIdInAllResponse()
        {
            DoTestGet("aaaa.txt", NewWholeProcessTests_Data.SimplePut1_Data, NewWholeProcessTests_Data.SimpleGet1_ConnectionIdInAllResponse_Responses, NewWholeProcessTests_Data.SimpleGet1_ExpectedRequests, false);
        }

        [Test]
        public void SimpleGet1_NoConnectionId()
        {
            DoTestGet("aaaa.txt", NewWholeProcessTests_Data.SimplePut1_Data, NewWholeProcessTests_Data.SimpleGet1_NoConnectionId_Responses, NewWholeProcessTests_Data.SimpleGet1_NoConnectionId_ExpectedRequests, false);
        }

        //---------
        const int LenOfDisconnectResponse = 3;
        const int LenOfDisconnectRequestA = 8;

        [Test]
        public void SimpleGet1_EarlyDisconnect_TruncateRcv()
        {
            int totalLen = NewWholeProcessTests_Data.SimpleGet1_ConnectionId_Responses.Length;
            int offsetOfDisconnect = totalLen - LenOfDisconnectResponse;
            for (int rcvTruncate = 0; rcvTruncate < totalLen; ++rcvTruncate) {
                try {
                    DoTestGet("aaaa.txt", NewWholeProcessTests_Data.SimplePut1_Data,
                        Truncate(NewWholeProcessTests_Data.SimpleGet1_ConnectionId_Responses, rcvTruncate),
                        NewWholeProcessTests_Data.SimpleGet1_ExpectedRequests, false);
                    if (rcvTruncate >= offsetOfDisconnect)  // OWR ignores errors there
                        continue;
                    Assert.Fail("should have thrown");
                } catch (AssertionException) {
                    throw;
                } catch (Exception) {
                    //Console.WriteLine("sht: " + ex.GetType());
                }
            }
        }

        [Test]
        public void SimpleGet1_EarlyDisconnect_WhileWriting()
        {
            int totalLen = NewWholeProcessTests_Data.SimpleGet1_ExpectedRequests.Length;
            int offsetOfDisconnect = totalLen - LenOfDisconnectRequestA;
            for (int sndTruncate = 0; sndTruncate < totalLen; ++sndTruncate) {
                try {
                    DoTestGet("aaaa.txt", NewWholeProcessTests_Data.SimplePut1_Data,
                        NewWholeProcessTests_Data.SimpleGet1_ConnectionId_Responses,
                        NewWholeProcessTests_Data.SimpleGet1_ExpectedRequests, false, sndTruncate,
                        sndTruncate.ToString());
                    Assert.Fail("should have thrown");
                } catch (AssertionException) {
                    // OWR ignores errors in disconnect, so ignore "unexpected 
                    // requests array" errors.
                    if (sndTruncate >= offsetOfDisconnect)
                        continue;
                    throw;
                } catch (Exception) {
                    //Console.WriteLine("sht: " + ex.GetType());
                }
            }
        }

    }//class

    [TestFixture]
    public class NewWholeProcessTestsUriPathEncoding : NewWholeProcessTestsInfrastructure
    {
        [Test]
        public void SimplePut1()
        {
            DoTestPut("aaaa.txt", NewWholeProcessTests_Data.SimplePut1_Data, NewWholeProcessTests_Data.SimplePut1_Responses, NewWholeProcessTests_Data.SimplePut1_ExpectedRequests, false);
        }

        [Test]
        public void SimplePut1WithSlash()
        {
            DoTestPut("/aaaa.txt", NewWholeProcessTests_Data.SimplePut1_Data, NewWholeProcessTests_Data.SimplePut1_Responses, NewWholeProcessTests_Data.SimplePut1_ExpectedRequests, false);
        }

        [Test]
        public void SimplePut1SimpleNonAsciiUnicodeFilename()
        {
            DoTestPut("aáa‡a%a.txt", NewWholeProcessTests_Data.SimplePut1_Data, NewWholeProcessTests_Data.SimplePut1_Responses, NewWholeProcessTests_Data.SimplePut1SimpleNonAsciiFilename_ExpectedRequests, false);
        }

        [Test]
        public void SimplePut1UnicodeWithSurrogatePair()
        {
            DoTestPut("aa\U00012004aa.txt", NewWholeProcessTests_Data.SimplePut1_Data, NewWholeProcessTests_Data.SimplePut1_Responses, NewWholeProcessTests_Data.SimplePut1UnicodeWithSurrogatePair_ExpectedRequests, false);
        }

        [Test]
        public void SimplePut1UnicodeChinese()
        {
            DoTestPut("空白文件.doc", NewWholeProcessTests_Data.SimplePut1_Data, NewWholeProcessTests_Data.SimplePut1_Responses, NewWholeProcessTests_Data.SimplePut1UnicodeChinese_ExpectedRequests, false);
        }
    }

    [TestFixture]
    public class NewWholeProcessTestsUriPathEncoding_UserDontEscape : NewWholeProcessTestsUriPathEncoding
    {
        public override Uri CreateObexUriForStreamTest(string filename)
        {
#if FX1_1
/*
#endif
#pragma warning disable 618
#if FX1_1
*/
#endif
            return new Uri("obex:" + filename, true);
#if FX1_1
/*
#endif
#pragma warning restore 618
#if FX1_1
*/
#endif
        }
    }


    public class ByteByByteReadStream : ForV1CloseDisposeLikeV2Stream
    {
        Stream m_readStrm;

        protected override void Dispose(bool disposing)
        {
            try {
                m_readStrm.Close();
            } finally {
                base.Dispose(disposing);
            }
        }


        public ByteByByteReadStream(Stream readStrm)
        {
            m_readStrm = readStrm;
        }



        public override bool CanRead { get { return m_readStrm.CanRead; } }

        public override bool CanWrite { get { throw new Exception("The method or operation is not implemented."); /*or false*/} }

        public override bool CanSeek
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override void Flush()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override long Length
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override long Position
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            Debug.Assert(count > 0, "count: " + count);
            if (count == 0)
                throw new ApplicationException("!!ByteByByteReadStream.Read, count: " + count);
            int newCount = Math.Min(count, 1);
            return m_readStrm.Read(buffer, offset, newCount);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetLength(long value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }//class


    public class ExceptionAtReadEosStream : ForV1CloseDisposeLikeV2Stream
    {
        Stream m_child;

        protected override void Dispose(bool disposing)
        {
            try {
                m_child.Close();
            } finally {
                base.Dispose(disposing);
            }
        }


        public ExceptionAtReadEosStream(Stream readStrm)
        {
            m_child = readStrm;
        }



        public override bool CanRead { get { return m_child.CanRead; } }

        public override bool CanWrite { get { return m_child.CanWrite; } }

        public override bool CanSeek
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override void Flush()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override long Length
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override long Position
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            Debug.Assert(count > 0, "count: " + count);
            if (count == 0)
                throw new ApplicationException("!!ExceptionAtEosReadStream.Read, count: " + count);
            int readLen = m_child.Read(buffer, offset, count);
            if (readLen == 0 && count != 0)
                throw new IOException("ConnectionClosed");
            return readLen;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetLength(long value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            m_child.Write(buffer, offset, count);
        }
    }//class


    public class TwoWayStream : ForV1CloseDisposeLikeV2Stream
    {
        Stream m_readStrm;
        Stream m_writeStrm;

        protected override void Dispose(bool disposing)
        {
            try {
                m_readStrm.Close();
            } finally {
                try {
                    m_writeStrm.Close();
                } finally {
                    base.Dispose(disposing);
                }
            }
        }


        public TwoWayStream(Stream readStrm, Stream writeStrm)
        {
            m_readStrm = readStrm;
            m_writeStrm = writeStrm;
        }



        public override bool CanRead { get { return m_readStrm.CanRead; } }

        public override bool CanWrite { get { return m_writeStrm.CanWrite; } }

        public override bool CanSeek
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override void Flush()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override long Length
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override long Position
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int x = m_readStrm.Read(buffer, offset, count);
            return x;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetLength(long value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            m_writeStrm.Write(buffer, offset, count);
        }
    }//class


    /// <exclude/>
    /// <summary>
    /// An internal abstract <see cref="T:System.IO.Stream"/> used to implement some of the 
    /// Close/Dispose functionality from FXv2 when compiling the library for FXv1.1.
    /// </summary>
    public abstract class ForV1CloseDisposeLikeV2Stream : Stream
    {
#if FX1_1
        /* The sequence of related calls supplied by Stream in V2 is a follows.
         * There is no such sequence in V1 so we supply it in the 
         * ForV1CloseDisposeLikeV2Stream class.
         * 
         * public override void Close()
         *   In base (Stream) calls Dispose(true);
         * public void Dispose()
         *   In base (Stream) calls Close();
         * protected override void Dispose(bool disposing)
         *   In base (AbortableStream) calls CloseCore(true);
         *   and then base.Dispose(disposing);
         */

        public /*virtual*/override void Close()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            this.Close();
        }

        protected virtual void Dispose(bool disposing)
        { }
#endif
    }//class

}
