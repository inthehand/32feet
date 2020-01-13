using System;
using NUnit.Framework;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.StonestreetOne;
using NMock2;
using InTheHand.Net.Bluetooth.Factory;

namespace InTheHand.Net.Tests.BluetopiaTests
{
    [TestFixture]
    public class ConnectSetPinTests
    {
        const string Pin1234 = "1234";
        //static readonly BluetoothAddress AddrA = BluetoothAddress.Parse("002233445566");
        const byte PortB = 15;
        readonly BluetoothEndPoint EpUuidA = new BluetoothEndPoint(ClientTesting.Addr1,
            BluetoothService.VideoSource);
        readonly BluetoothEndPoint EpPortB = new BluetoothEndPoint(ClientTesting.Addr1,
            BluetoothService.VideoSource, ClientTesting.Port5);

        [Test]
        public void SetPin_Async_Service_SuccessConnect()
        {
            var behaviour = new ClientTesting.Behaviour
            {
                ToPortNumber = false,
                EndConnectSuccess = true,
                SocketError = null,
                ConnConfStatusCode = StackConsts.SPP_OPEN_PORT_STATUS.Success,
                SdpMethodResult = 543,
                SdpQueryResultPort = 5,
                MockIBtSecurity = true,
            };
            DoTest(behaviour);
        }

        [Test]
        public void SetPin_Async_Port_SuccessConnect()
        {
            var behaviour = new ClientTesting.Behaviour
            {
                ToPortNumber = true,
                EndConnectSuccess = true,
                SocketError = null,
                ConnConfStatusCode = StackConsts.SPP_OPEN_PORT_STATUS.Success,
                MockIBtSecurity = true,
            };
            DoTest(behaviour);
        }

        [Test]
        public void SetPin_Async_Service_ConnectFailRfcomm()
        {

            var behaviour = new ClientTesting.Behaviour
            {
                ToPortNumber = false,
                EndConnectSuccess = false,
                SocketError = 10061,
                ConnConfStatusCode = StackConsts.SPP_OPEN_PORT_STATUS.ConnectionRefused,
                SdpMethodResult = 543,
                SdpQueryResultPort = 5,
                MockIBtSecurity = true,
            };
            DoTest(behaviour);
        }

        [Test]
        public void SetPin_Async_Port_ConnectFailRfcomm()
        {
            var behaviour = new ClientTesting.Behaviour
            {
                ToPortNumber = true,
                EndConnectSuccess = false,
                SocketError = 10061,
                ConnConfStatusCode = StackConsts.SPP_OPEN_PORT_STATUS.ConnectionRefused,
                MockIBtSecurity = true,
            };
            DoTest(behaviour);
        }

        private static void DoTest(ClientTesting.Behaviour behaviour)
        {
            var stuff = ClientTesting.CreateClient(behaviour);
            //
            var cli = stuff.DutClient;
            cli.SetPin(Pin1234);
            stuff.Mockery.VerifyAllExpectationsHaveBeenMet();
            //
            Expect.Once.On(stuff.MockedSecurityApi)
                .Method("SetPin")
                .With(ClientTesting.Addr1, Pin1234)
                .Will(Return.Value(true));
            ClientTesting.ExpectOpen(stuff);
            ClientTesting.DoOpen(stuff, new Action(delegate {
                Expect.Once.On(stuff.MockedSecurityApi)
                    .Method("RevokePin")
                    .With(ClientTesting.Addr1)
                    .Will(Return.Value(true));
            }));
            stuff.Mockery.VerifyAllExpectationsHaveBeenMet();
            //
            if (behaviour.EndConnectSuccess) {
                ClientTesting.RaiseRemoteClose(stuff);
            } else {
                Expect.Once.On(stuff.MockedApi).Method("SPP_Close_Port")
                    .With(stuff.StackId, stuff.DutConn.Testing_GetPortId())
                    .Will(Return.Value(BluetopiaError.OK));
            }
            stuff.DutClient.Dispose();
            stuff.Mockery.VerifyAllExpectationsHaveBeenMet();
        }


    }
}
