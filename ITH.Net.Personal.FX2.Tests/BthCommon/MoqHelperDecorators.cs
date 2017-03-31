// 32feet.NET - Personal Area Networking for .NET
//
// Copyright (c) 2011 In The Hand Ltd, All rights reserved.
// Copyright (c) 2011 Alan J. McFarlane, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using Moq;
using InTheHand.Net.Bluetooth.Factory;

namespace InTheHand.Net.Tests.BthCommon
{
    public abstract class MyBluetoothFactory : BluetoothFactory
    {
        //public abstract IBluetoothClient tGetBluetoothClientForListener(CommonRfcommStream acceptedStrm);
        protected sealed override IBluetoothClient GetBluetoothClientForListener(CommonRfcommStream acceptedStrm)
        {
            //return tGetBluetoothClientForListener(acceptedStrm);
            var mockC = new Mock<IBluetoothClient>();
            var hackAlwaysSayConnected = true;
            mockC.SetupGet(px => px.Connected)
                .Returns(hackAlwaysSayConnected);
            return mockC.Object;
        }

    }


    public abstract class MyCommonRfcommStream : CommonRfcommStream
    {
        public new void HandleCONNECT_ERR(string eventId, int? socketErrorCode)
        {
            base.HandleCONNECT_ERR(eventId, socketErrorCode);
        }

        public new void HandleCONNECTED(string eventId)
        {
            base.HandleCONNECTED(eventId);
        }
    }


    public abstract class MyCommonBluetoothListener : CommonBluetoothListener
    {
        protected MyCommonBluetoothListener(BluetoothFactory fcty)
            : base(fcty)
        { }

        //--
        public abstract BluetoothEndPoint tSetupListener(BluetoothEndPoint bep, int scn);
        protected sealed override void SetupListener(BluetoothEndPoint bep, int scn, out BluetoothEndPoint liveLocalEP)
        {
            liveLocalEP = tSetupListener(bep, scn);
        }

        public abstract CommonRfcommStream tGetNewPort();
        protected sealed override CommonRfcommStream GetNewPort()
        {
            return tGetNewPort();
        }

        public abstract bool tIsDisposed { get; }
        protected sealed override bool IsDisposed
        {
            get { return tIsDisposed; }
        }

        public abstract void tOtherDispose(bool disposing);
        protected sealed override void OtherDispose(bool disposing)
        {
            tOtherDispose(disposing);
        }

        public abstract void tOtherDisposeMore();
        protected sealed override void OtherDisposeMore()
        {
            tOtherDisposeMore();
        }

        //---- Force **no** mocking ----
        protected sealed override IBluetoothClient GetBluetoothClientForListener(CommonRfcommStream strm)
        {
            return base.GetBluetoothClientForListener(strm);
        }

        protected sealed override void VerifyPortIsInRange(BluetoothEndPoint bep)
        {
            base.VerifyPortIsInRange(bep);
        }
    }

}
