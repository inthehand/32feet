using System;
using System.Collections.Generic;
using System.Text;
using InTheHand.Net.Bluetooth.BlueSoleil;
using System.Diagnostics;
#if NUNIT
using NUnit.Framework;
using TestClassAttribute = NUnit.Framework.TestFixtureAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace InTheHand.Net.Tests.BlueSoleil
{
    class TestSdBluesoleilApi : IBluesoleilApi
    {
        Queue<string> _log = new Queue<string>();

        public void AssertActionsInOrder(string expectedMethodName, params object[] args)
        {
            string expected = FormatLog(expectedMethodName, args);
            string actual = _log.Dequeue();
            Assert.AreEqual(expected, actual);
        }

        internal void AssertActionsNoMore()
        {
            Assert.AreEqual(0, _log.Count);
        }

        //--
        private void Log(string method, params object[] args)
        {
            _log.Enqueue(FormatLog(method, args));
        }

        //--
        private static string FormatLog(string method, params object[] args)
        {
            StringBuilder _log = new StringBuilder();
            _log.Append(method);
            const string Sep = ", ";
            _log.Append(": ");
            foreach (var cur in args) {
                _log.Append(cur);
                _log.Append(Sep);
            }
            if (args != null && args.Length > 0)
                _log.Length -= Sep.Length;
            _log.AppendLine();
            return _log.ToString();
        }

        //----
        void IBluesoleilApi.Btsdk_FreeMemory(IntPtr memblock)
        {
            Log("Btsdk_FreeMemory", memblock);
        }



        #region IBluesoleilApi NotImplemented Members

        BtSdkError IBluesoleilApi.Btsdk_Init()
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_Done()
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_RegisterCallback4ThirdParty(ref Structs.BtSdkCallbackStru call_back)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_StartBluetooth()
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_StopBluetooth()
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_SetDiscoveryMode(StackConsts.DiscoveryMode mode)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_GetDiscoveryMode(out StackConsts.DiscoveryMode mode)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_GetLocalDeviceAddress(byte[] bd_addr)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_GetLocalName(byte[] name, ref ushort len)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_GetLocalDeviceClass(out uint device_class)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_GetLocalLMPInfo(ref Structs.BtSdkLocalLMPInfoStru lmp_info)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_StartDeviceDiscovery(uint device_class, ushort max_num, ushort max_seconds)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_StopDeviceDiscovery()
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_UpdateRemoteDeviceName(uint dev_hdl, byte[] name, ref ushort plen)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_IsDevicePaired(uint dev_hdl, out bool pis_paired)
        {
            throw new NotImplementedException();
        }

        bool IBluesoleilApi.Btsdk_IsDeviceConnected(uint dev_hdl)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_GetRemoteRSSI(uint device_handle, out sbyte prssi)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_GetRemoteLinkQuality(uint device_handle, out ushort plink_quality)
        {
            throw new NotImplementedException();
        }

        uint IBluesoleilApi.Btsdk_GetRemoteDeviceHandle(byte[] bd_addr)
        {
            throw new NotImplementedException();
        }

        uint IBluesoleilApi.Btsdk_AddRemoteDevice(byte[] bd_addr)
        {
            throw new NotImplementedException();
        }

        int IBluesoleilApi.Btsdk_GetStoredDevicesByClass(uint dev_class, uint[] pdev_hdl, int max_dev_num)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_GetRemoteDeviceAddress(uint dev_hdl, byte[] bd_addr)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_GetRemoteDeviceName(uint dev_hdl, byte[] name, ref ushort plen)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_GetRemoteDeviceClass(uint dev_hdl, out uint pdevice_class)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_GetRemoteDeviceProperty(uint dev_hdl, out Structs.BtSdkRemoteDevicePropertyStru rmt_dev_prop)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_BrowseRemoteServicesEx(uint dev_hdl, Structs.BtSdkSDPSearchPatternStru[] psch_ptn, int ptn_num, uint[] svc_hdl, ref int svc_count)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_Disconnect(uint handle)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_ConnectAppExtSPPService(uint dev_hdl, ref Structs.BtSdkAppExtSPPAttrStru psvc, out uint conn_hdl)
        {
            throw new NotImplementedException();
        }

        bool IBluesoleilApi.Btsdk_IsBluetoothReady()
        {
            throw new NotImplementedException();
        }

        bool IBluesoleilApi.Btsdk_IsBluetoothHardwareExisted()
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_PairDevice(uint dev_hdl)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_PinCodeReply(uint dev_hdl, byte[] pin_code, ushort size)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_DeleteRemoteDeviceByHandle(uint dev_hdl)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_SearchAppExtSPPService(uint dev_hdl, ref Structs.BtSdkAppExtSPPAttrStru psvc)
        {
            return BtSdkError.NO_SERVICE;
        }

        BtSdkError IBluesoleilApi.Btsdk_GetRemoteServiceAttributes(uint svc_hdl, ref Structs.BtSdkRemoteServiceAttrStru attribute)
        {
            throw new NotImplementedException();
        }

        public bool Btsdk_IsSDKInitialized()
        {
            throw new NotImplementedException();
        }

        public BtSdkError Btsdk_RegisterGetStatusInfoCB4ThirdParty(ref NativeMethods.Func_ReceiveBluetoothStatusInfo statusCBK)
        {
            throw new NotImplementedException();
        }

        public BtSdkError Btsdk_SetStatusInfoFlag(ushort usMsgType)
        {
            throw new NotImplementedException();
        }

        public uint Btsdk_StartEnumConnection()
        {
            throw new NotImplementedException();
        }

        public uint Btsdk_EnumConnection(uint enum_handle, ref Structs.BtSdkConnectionPropertyStru conn_prop)
        {
            throw new NotImplementedException();
        }

        public BtSdkError Btsdk_EndEnumConnection(uint enum_handle)
        {
            throw new NotImplementedException();
        }

        public BtSdkError Btsdk_ConnectEx(uint dev_hdl, ushort service_class, uint lParam, out uint conn_hdl)
        {
            throw new NotImplementedException();
        }

        public bool Btsdk_IsServerConnected()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IBluesoleilApi Members


        public BtSdkError Btsdk_ConnectEx(uint dev_hdl, ushort service_class, ref Structs.BtSdkSPPConnParamStru lParam, out uint conn_hdl)
        {
            throw new NotImplementedException();
        }

        public BtSdkError Btsdk_InitCommObj(byte com_idx, ushort svc_class)
        {
            throw new NotImplementedException();
        }

        public BtSdkError Btsdk_DeinitCommObj(byte com_idx)
        {
            throw new NotImplementedException();
        }

        public short Btsdk_GetClientPort(uint conn_hdl)
        {
            throw new NotImplementedException();
        }

        public uint Btsdk_CommNumToSerialNum(int comportNum)
        {
            throw new NotImplementedException();
        }

        public void Btsdk_PlugOutVComm(uint serialNum, StackConsts.COMM_SET flag)
        {
            throw new NotImplementedException();
        }

        public bool Btsdk_PlugInVComm(uint serialNum, out uint comportNumber, uint usageType, StackConsts.COMM_SET flag, uint dwTimeout)
        {
            throw new NotImplementedException();
        }

        public uint Btsdk_GetASerialNum()
        {
            throw new NotImplementedException();
        }

        #endregion
    }//class
}
