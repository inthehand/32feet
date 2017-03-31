#if BLUETOPIA
using System;
using NUnit.Framework;
using InTheHand.Net.Bluetooth.StonestreetOne;
using InTheHand.Net.Bluetooth.Factory;
using NMock2;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using InTheHand.Net.Bluetooth;
using InTheHand.Net;

namespace InTheHand.Net.Tests.BluetopiaTests
{
    [TestFixture]
    public class BluetopiaDiscoveryTests
    {
        readonly byte[] Addr1Bytes = new byte[] { 0x55, 0x44, 0x33, 0x22, 0x11, 0x00 };

        [Test]
        public void One()
        {
            var stuff = BluetopiaTesting.InitMockery_Client(new ClientTestingBluetopia.Behaviour());
            //
            const uint NullPeriodLengths = 0;
            const uint InquiryLen = 12;
            const uint CallbackParameter = 0;
            const uint MaximumResponses = 255;
            var callback = stuff.GetFactory()._inquiryEventCallback;
            Expect.Once.On(stuff.MockedApi).Method("GAP_Perform_Inquiry")
                .With(stuff.StackId, StackConsts.GAP_Inquiry_Type.GeneralInquiry,
                    NullPeriodLengths, NullPeriodLengths,
                    InquiryLen, MaximumResponses,
                    callback, CallbackParameter)
                .Will(Return.Value(BluetopiaError.OK));
            //
            IBluetoothClient cli = stuff.CreateBluetoothClient();
            var ar = cli.BeginDiscoverDevices(255, false, false, false, true, null, null);
            Assert.IsFalse(ar.IsCompleted, "IsCompleted 0");
            //
            IntPtr pItemData = IntPtr.Zero;
            IntPtr pCompleteData = IntPtr.Zero;
            try {
                Structs.GAP_Inquiry_Entry_Event_Data itemData = new Structs.GAP_Inquiry_Entry_Event_Data(
                    Addr1Bytes,
                    new byte[] { 0x03, 0x02, 0x01 }
                    );
                pItemData = Marshal.AllocHGlobal(Marshal.SizeOf(itemData));
                Marshal.StructureToPtr(itemData, pItemData, false);
                Structs.GAP_Event_Data item1 = new Structs.GAP_Event_Data(
                    StackConsts.GAP_Event_Type.Inquiry_Entry_Result,
                    pItemData);
                //
                const ushort NumDevices = 1;
                Structs.GAP_Inquiry_Event_Data completeData = new Structs.GAP_Inquiry_Event_Data(
                    NumDevices /*, pCompleteDeviceData*/);
                pCompleteData = Marshal.AllocHGlobal(Marshal.SizeOf(completeData));
                Marshal.StructureToPtr(completeData, pCompleteData, false);
                Structs.GAP_Event_Data complete = new Structs.GAP_Event_Data(
                    StackConsts.GAP_Event_Type.Inquiry_Result,
                    pCompleteData);
                //
                //
                using (var done = new ManualResetEvent(false)) {
                    ThreadPool.QueueUserWorkItem(delegate {
                        try {
                            callback(stuff.StackId, ref item1, CallbackParameter);
                            callback(stuff.StackId, ref complete, CallbackParameter);
                        } finally {
                            done.Set();
                        }
                    });
                    bool signalled = done.WaitOne(30000);
                    Debug.Assert(signalled, "NOT signalled");
                }
            } finally {
                if (pItemData != IntPtr.Zero) Marshal.FreeHGlobal(pItemData);
                if (pCompleteData != IntPtr.Zero) Marshal.FreeHGlobal(pCompleteData);
            }
            //
            Thread.Sleep(200);
            Assert.IsFalse(ar.IsCompleted, "notIsCompleted2 before rnr");
            Thread.Sleep(2500); // Space for (possible!) name queries
            Thread.Sleep(200);
            Assert.IsTrue(ar.IsCompleted, "IsCompleted3");
            var deviceList = cli.EndDiscoverDevices(ar);
            Assert.AreEqual(1, deviceList.Length, "deviceList.Length");
            var dev = deviceList[0];
            Assert.AreEqual(BluetoothAddress.Parse("001122334455"), dev.DeviceAddress, "dev.DeviceAddress");
            Assert.AreEqual(new ClassOfDevice(0x010203), dev.ClassOfDevice, "dev.ClassOfDevice");
            //
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
        }

    }
}
#endif
