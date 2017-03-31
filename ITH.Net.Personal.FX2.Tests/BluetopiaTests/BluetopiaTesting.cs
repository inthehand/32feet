#if BLUETOPIA
using System;
using System.Collections.Generic;
using System.Diagnostics;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Bluetooth.StonestreetOne;
using NMock2;
using InTheHand.Net.Tests.Infra;

namespace InTheHand.Net.Tests.BluetopiaTests
{
    static class BluetopiaTesting
    {
        internal static void InitMockery(StuffStackBluetopia stuff, uint stackId)
        {
            stuff.Mockery = new Mockery();
            stuff.StackId = stackId;
            var hackApi = stuff.MockedApi = stuff.Mockery.NewMock<IBluetopiaApi>();
            Expect.Once.On(hackApi).Method("BSC_Initialize")
                .WithAnyArguments() // TO-DO
                .Will(Return.Value(checked((int)stackId)));
            Expect.Once.On(hackApi).Method("GAP_Set_Pairability_Mode")
                .WithAnyArguments() // TO-DO
                .Will(Return.Value(BluetopiaError.OK));
            Expect.Once.On(hackApi).Method("GAP_Register_Remote_Authentication")
                .WithAnyArguments() // TO-DO
                .Will(Return.Value(BluetopiaError.OK));
            Expect.Once.On(hackApi).Method("GAP_Query_Local_Out_Of_Band_Data")
                .WithAnyArguments() // TO-DO
                .Will(Return.Value(BluetopiaError.INVALID_PARAMETER));
        }

        internal static StuffClientBluetopia InitMockery_Client(ClientTestingBluetopia.Behaviour behaviour)
        {
            StuffClientBluetopia stuff = new StuffClientBluetopia();
            stuff.Behaviour = behaviour;
            BluetopiaTesting.InitMockery(stuff, 10);
            if (stuff.Behaviour.MockIBtSecurity) {
                stuff.CreateMockedSecurityApi();
            }
            stuff.GetFactory();
            return stuff;
        }

        #region SdpCreator
        internal static StuffSdpCreatorBluetopia InitMockery_SdpCreator()
        {
            StuffSdpCreatorBluetopia stuff = new StuffSdpCreatorBluetopia();
            InitMockery(stuff, 99);
            Debug.Assert(stuff.StackId == 99);
            //
            stuff.SetDut(new TestBluetopiaSdpCreator(stuff.GetFactory()));
            // 
            Expect.Once.On(stuff.MockedApi).Method("SDP_Create_Service_Record")
                .WithAnyArguments() // HACK ! SDP_Create_Service_Record WithAnyArguments
                .Will(Return.Value(999));
            stuff.SrHandle = 999;
            //
            return stuff;
        }

        internal class TestBluetopiaSdpCreator : BluetopiaSdpCreator
        {
            BluetopiaFactory _fcty;

            public TestBluetopiaSdpCreator(BluetopiaFactory fcty)
                : base(fcty)
            {
                _fcty = fcty;
            }

            protected override BluetopiaError AddAttribute(Structs.SDP_Data_Element__Class eNotNative)
            {
                uint? hSR; ServiceAttributeId attrId;
                base.GetRecordAttrInfo(out hSR, out  attrId);
#pragma warning disable 618 // Obsolete
                var ret = _fcty.Api.SDP_Add_Attribute(_fcty.StackId, hSR.Value,
                    unchecked((ushort)attrId), eNotNative);
#pragma warning restore 618 // Obsolete
                return ret;
            }

        }

        public static ServiceRecord HackAddSvcClassList(ServiceRecord r)
        {
            // HACK ?? allow record with no SCList ??!!!!!!
            var sc = new Guid("{BACA3503-98AB-4f34-9127-809C55F83E2D}");
            var list = new List<ServiceAttribute>(r);
            list.Add(new ServiceAttribute(InTheHand.Net.Bluetooth.AttributeIds.UniversalAttributeId.ServiceClassIdList,
                new ServiceElement(ElementType.ElementSequence,
                    new ServiceElement(ElementType.Uuid128, sc))));
            r = new ServiceRecord(list);
            return r;
        }
        #endregion

        //----
        internal static void HackAllowShutdownCall(IBluetopiaApi mockedApi)
        {
            Expect.Between(0, 1).On(mockedApi).Method("BSC_Shutdown").WithAnyArguments();
        }

    }

    //----
    class StuffStackBluetopia
    {
        BluetopiaFactory _fcty;

        public uint StackId { get; set; }
        public Mockery Mockery { get; set; }
        public IBluetopiaApi MockedApi { get; set; }

        internal void Mockery_VerifyAllExpectationsHaveBeenMet()
        {
            this.Mockery.VerifyAllExpectationsHaveBeenMet();
        }

        public BluetopiaFactory GetFactory()
        {
            return GetFactory(null);
        }

        public BluetopiaFactory GetFactory(IBluetopiaSecurity optionalSecurityInstance)
        {
            if (_fcty == null) {
                _fcty = new BluetopiaFactory(this.MockedApi, optionalSecurityInstance);
            }
            BluetopiaTesting.HackAllowShutdownCall(this.MockedApi);
            return _fcty;
        }
    }

    internal class StuffClientBluetopia : StuffStackBluetopia
    {
        IBluetoothClient _cli;
        BluetopiaRfcommStream _conn;
        IBluetopiaSecurity _secMock;

        internal IBluetoothClient CreateBluetoothClient()
        {
            return GetFactory().DoGetBluetoothClient();
        }

        [DebuggerStepThrough]
        internal void SetDut(IBluetoothClient cli, BluetopiaRfcommStream conn)
        {
            _cli = cli;
            _conn = conn;
        }

        public IBluetoothClient DutClient { [DebuggerStepThrough] get { return _cli; } }

        public BluetopiaRfcommStream DutConn { [DebuggerStepThrough] get { return _conn; } }

        public EventHandler AddExpectOpenRemotePort { get; set; }

        public ClientTestingBluetopia.Behaviour Behaviour { get; set; }

        internal void CreateMockedSecurityApi()
        {
            _secMock = this.Mockery.NewMock<IBluetopiaSecurity>();
            Expect.Once.On(_secMock).Method("InitStack")
                .WithNoArguments()
                .Will(
        new MyDelegateAction(delegate {
                this.MockedApi.GAP_Set_Pairability_Mode(this.StackId, StackConsts.GAP_Pairability_Mode.PairableMode);
                NativeMethods.GAP_Event_Callback foo = null;
                this.MockedApi.GAP_Register_Remote_Authentication(this.StackId, foo, (uint)0);
            })
                );
            var tmp = this.GetFactory(_secMock);
        }

        public IBluetopiaSecurity MockedSecurityApi
        {
            get
            {
                if (_secMock == null) throw new InvalidOperationException();
                return _secMock;
            }
        }

    }

    internal class StuffSdpQueryBluetopia : StuffStackBluetopia
    {
        BluetopiaSdpQuery _sdpQuery;

        [DebuggerStepThrough]
        internal void SetDut(BluetopiaSdpQuery sdpQuery)
        {
            _sdpQuery = sdpQuery;
        }

        public BluetopiaSdpQuery DutSdpQuery { [DebuggerStepThrough] get { return _sdpQuery; } }
    }

    class StuffSdpCreatorBluetopia : StuffStackBluetopia
    {
        //public BluetopiaSdpCreator.Api2 MockApi2 { get; set; }
        public IBluetopiaApi MockApi2 { get { return this.MockedApi; } }
        public uint? SrHandle { get; set; }

        BluetopiaSdpCreator _sdpCtor;

        [DebuggerStepThrough]
        internal void SetDut(BluetopiaSdpCreator sdpCtor)
        {
            _sdpCtor = sdpCtor;
        }

        public BluetopiaSdpCreator DutSdpCreator { [DebuggerStepThrough] get { return _sdpCtor; } }
    }

}
#endif
