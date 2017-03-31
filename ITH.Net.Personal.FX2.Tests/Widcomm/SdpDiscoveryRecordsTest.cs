using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using InTheHand.Net.Bluetooth.Widcomm;
using InTheHand.Net.Bluetooth;

namespace InTheHand.Net.Tests.Widcomm
{
    [TestFixture]
    public class SdpDiscoveryRecords_GetServiceRecords_Test : SdpDiscoveryRecordsTestBase
    {
        private static ServiceRecord[] Call_GetServiceRecords(Guid searchUuid, IList<SdpDiscoveryRecordsBufferBase.SimpleInfo> list)
        {
            SdrbClass sdrb = new SdrbClass(new ServiceDiscoveryParams(null, searchUuid, SdpSearchScope.Anywhere), list);
            ServiceRecord[] rcds = sdrb.GetServiceRecords();
            return rcds;
        }

        [Test]
        public void SvcClassList1()
        {
            ServiceRecord[] rcds;
            List<SdpDiscoveryRecordsBufferBase.SimpleInfo> arr;
            //
            arr = CreateTestRecords1Wap();
            rcds = Call_GetServiceRecords(BluetoothService.Wap, arr);
            Assert.AreEqual(1, rcds.Length, "len Yes Wap");
            rcds = Call_GetServiceRecords(BluetoothService.ObexObjectPush, arr);
            Assert.AreEqual(0, rcds.Length, "len Not OPP");
            rcds = Call_GetServiceRecords(BluetoothService.ServiceDiscoveryServer, arr);
            Assert.AreEqual(0, rcds.Length, "len Not SDP");
            rcds = Call_GetServiceRecords(BluetoothService.IrMCSync, arr);
            Assert.AreEqual(0, rcds.Length, "len Not IrMCSync");
            //----
            arr = CreateTestRecords1Opp();
            rcds = Call_GetServiceRecords(BluetoothService.Wap, arr);
            Assert.AreEqual(0, rcds.Length, "len Not Wap");
            rcds = Call_GetServiceRecords(BluetoothService.ObexObjectPush, arr);
            Assert.AreEqual(1, rcds.Length, "len Yes OPP");
            rcds = Call_GetServiceRecords(BluetoothService.ServiceDiscoveryServer, arr);
            Assert.AreEqual(0, rcds.Length, "len Not SDP");
            rcds = Call_GetServiceRecords(BluetoothService.IrMCSync, arr);
            Assert.AreEqual(0, rcds.Length, "len Not IrMCSync");
            //----
            arr = CreateTestRecords1Sdp();
            rcds = Call_GetServiceRecords(BluetoothService.Wap, arr);
            Assert.AreEqual(0, rcds.Length, "len Not Wap");
            rcds = Call_GetServiceRecords(BluetoothService.ObexObjectPush, arr);
            Assert.AreEqual(0, rcds.Length, "len Not OPP");
            rcds = Call_GetServiceRecords(BluetoothService.ServiceDiscoveryServer, arr);
            Assert.AreEqual(1, rcds.Length, "len Yes SDP");
            rcds = Call_GetServiceRecords(BluetoothService.IrMCSync, arr);
            Assert.AreEqual(0, rcds.Length, "len Not IrMCSync");
        }

        [Test]
        public void Anywhere1()
        {
            ServiceRecord[] rcds;
            List<SdpDiscoveryRecordsBufferBase.SimpleInfo> arr;
            //
            arr = CreateTestRecords1Wap();
            rcds = Call_GetServiceRecords(BluetoothService.L2CapProtocol, arr);
            Assert.AreEqual(1, rcds.Length, "len Yes L2CAP proto");
            rcds = Call_GetServiceRecords(BluetoothService.RFCommProtocol, arr);
            Assert.AreEqual(1, rcds.Length, "len Yes RFCOMM proto");
            rcds = Call_GetServiceRecords(BluetoothService.ObexProtocol, arr);
            Assert.AreEqual(0, rcds.Length, "len Not ObexProto");
            //----
            arr = CreateTestRecords1Opp();
            rcds = Call_GetServiceRecords(BluetoothService.L2CapProtocol, arr);
            Assert.AreEqual(1, rcds.Length, "len Yes L2CAP proto");
            rcds = Call_GetServiceRecords(BluetoothService.RFCommProtocol, arr);
            Assert.AreEqual(1, rcds.Length, "len Yes RFCOMM proto");
            rcds = Call_GetServiceRecords(BluetoothService.ObexProtocol, arr);
            // TODO Assert.AreEqual(1, rcds.Length, "len Yes ObexProto");
            Assert.AreEqual(0, rcds.Length, "SHOULD BE----> len Yes ObexProto");
            //----
            arr = CreateTestRecords1Sdp();
            rcds = Call_GetServiceRecords(BluetoothService.L2CapProtocol, arr);
            Assert.AreEqual(1, rcds.Length, "len Yes L2CAP proto");
            rcds = Call_GetServiceRecords(BluetoothService.RFCommProtocol, arr);
            Assert.AreEqual(0, rcds.Length, "len Not RFCOMM proto");
            rcds = Call_GetServiceRecords(BluetoothService.ObexProtocol, arr);
            Assert.AreEqual(0, rcds.Length, "len Not ObexProto");
            //----
        }


        [Test]
        public void SvcClassListN()
        {
            ServiceRecord[] rcds;
            List<SdpDiscoveryRecordsBufferBase.SimpleInfo> arr;
            //
            arr = CreateTestRecordsN();
            rcds = Call_GetServiceRecords(BluetoothService.Wap, arr);
            Assert.AreEqual(1, rcds.Length, "len 1 Wap");
            //
            Assert.AreEqual(19, ServiceRecordHelper.GetRfcommChannelNumber(rcds[0]), "WAP SCN");
            //--
            rcds = Call_GetServiceRecords(BluetoothService.ObexObjectPush, arr);
            Assert.AreEqual(1, rcds.Length, "len 1 OPP");
            Assert.AreEqual(5, ServiceRecordHelper.GetRfcommChannelNumber(rcds[0]), "OPP SCN");
            //--
            rcds = Call_GetServiceRecords(BluetoothService.ServiceDiscoveryServer, arr);
            Assert.AreEqual(1, rcds.Length, "len 1 SDP");
            Assert.AreEqual(-1, ServiceRecordHelper.GetRfcommChannelNumber(rcds[0]), "SDP SCN");
            //--
            rcds = Call_GetServiceRecords(BluetoothService.IrMCSync, arr);
            Assert.AreEqual(0, rcds.Length, "len 0 IrMCSync");
        }

        [Test]
        public void AnywhereN()
        {
            ServiceRecord[] rcds;
            List<SdpDiscoveryRecordsBufferBase.SimpleInfo> arr;
            //
            arr = CreateTestRecordsN();
            rcds = Call_GetServiceRecords(BluetoothService.L2CapProtocol, arr);
            Assert.AreEqual(3, rcds.Length, "len 3 L2CAP proto");
            //
            Assert.AreEqual(19, ServiceRecordHelper.GetRfcommChannelNumber(rcds[0]), "WAP SCN");
            Assert.AreEqual(5, ServiceRecordHelper.GetRfcommChannelNumber(rcds[1]), "OPP SCN");
            Assert.AreEqual(-1, ServiceRecordHelper.GetRfcommChannelNumber(rcds[2]), "SDP SCN");
            //--
            rcds = Call_GetServiceRecords(BluetoothService.RFCommProtocol, arr);
            Assert.AreEqual(2, rcds.Length, "len 2 RFCOMM proto");
            //
            Assert.AreEqual(19, ServiceRecordHelper.GetRfcommChannelNumber(rcds[0]), "WAP SCN");
            Assert.AreEqual(5, ServiceRecordHelper.GetRfcommChannelNumber(rcds[1]), "OPP SCN");
            //--
            rcds = Call_GetServiceRecords(BluetoothService.ObexProtocol, arr);
            // TODO Assert.AreEqual(2, rcds.Length, "len 2 ObexProto");
            Assert.AreEqual(0, rcds.Length, "SHOULD BE----> len 2 ObexProto");
        }

    }


    #region Test class
    class SdrbClass : SdpDiscoveryRecordsBufferBase
    {
        IList<SimpleInfo> m_si;

        public SdrbClass(ServiceDiscoveryParams request, IList<SimpleInfo> list)
            : base(request)
        {
            m_si = list;
        }

        protected override SimpleInfo[] GetSimpleInfo()
        {
            SimpleInfo[] arr = new SimpleInfo[m_si.Count];
            m_si.CopyTo(arr, 0);
            return arr;
        }

        public override int RecordCount
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override int[] Hack_GetPorts()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int[] Hack_GetPsms()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void Dispose(bool disposing)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void EnsureNotDisposed()
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }//class
    #endregion


    public abstract class SdpDiscoveryRecordsTestBase
    {
        //--------
        internal static List<SdpDiscoveryRecordsBufferBase.SimpleInfo> CreateTestRecords1Wap()
        {
            List<SdpDiscoveryRecordsBufferBase.SimpleInfo> arr = new List<SdpDiscoveryRecordsBufferBase.SimpleInfo>();
            arr.Add(Create_SI(BluetoothService.Wap, BluetoothProtocolDescriptorType.Rfcomm, 19));
            return arr;
        }

        internal static List<SdpDiscoveryRecordsBufferBase.SimpleInfo> CreateTestRecords1Opp()
        {
            List<SdpDiscoveryRecordsBufferBase.SimpleInfo> arr = new List<SdpDiscoveryRecordsBufferBase.SimpleInfo>();
            arr.Add(Create_SI(BluetoothService.ObexObjectPush, BluetoothProtocolDescriptorType.Rfcomm, 5));
            return arr;
        }

        internal static List<SdpDiscoveryRecordsBufferBase.SimpleInfo> CreateTestRecords1Sdp()
        {
            List<SdpDiscoveryRecordsBufferBase.SimpleInfo> arr = new List<SdpDiscoveryRecordsBufferBase.SimpleInfo>();
            arr.Add(Create_SI(BluetoothService.ServiceDiscoveryServer, BluetoothProtocolDescriptorType.None, -1));
            return arr;
        }

        internal static List<SdpDiscoveryRecordsBufferBase.SimpleInfo> CreateTestRecordsN()
        {
            List<SdpDiscoveryRecordsBufferBase.SimpleInfo> arr = new List<SdpDiscoveryRecordsBufferBase.SimpleInfo>();
            arr.Add(Create_SI(BluetoothService.Wap, BluetoothProtocolDescriptorType.Rfcomm, 19));
            arr.Add(Create_SI(BluetoothService.ObexObjectPush, BluetoothProtocolDescriptorType.Rfcomm, 5));
            arr.Add(Create_SI(BluetoothService.ServiceDiscoveryServer, BluetoothProtocolDescriptorType.None, -1));
            return arr;
        }

        //----
        private static SdpDiscoveryRecordsBufferBase.SimpleInfo Create_SI(
            Guid serviceClass, BluetoothProtocolDescriptorType pdlType, int scn)
        {
            Guid info_serviceUuid = serviceClass;
            int info_scn;
            if (pdlType == BluetoothProtocolDescriptorType.Rfcomm || pdlType == BluetoothProtocolDescriptorType.GeneralObex) {
                if (scn == -1)
                    throw new ArgumentException("scn");
                info_scn = scn;
            } else {
                if (scn != -1)
                    throw new ArgumentException("scn");
                info_scn = -1;
            }
            return new SdpDiscoveryRecordsBufferBase.SimpleInfo(
                info_serviceUuid, info_scn);
        }

    }
}
